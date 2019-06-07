using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using SevenZip;
using System.IO;
using SharpCompress.Archive;

namespace MyEmulators2
{
    class Extractor
    {
        const string GOODMERGE_EXTRACT_FOLDER = "Emulators2_GoodMerge_Extract_Folder";
        const string GOODMERGE_FILENAME_PREFIX = "Emulators2_GoodMerge_";

        object syncRoot = new object();
        IArchive extractor = null;

        string GoodmergeTempPath = null;
        bool ready = false;
        bool allowCache = true;
        Dictionary<string, string> cachedFiles = null;

        #region Singleton
        protected static object instanceLock = new object();
        protected static Extractor instance = null;
        public static Extractor Instance
        {
            get
            {
                if (instance == null)
                    lock (instanceLock)
                        if (instance == null)
                            instance = new Extractor();
                return instance;
            }
        }
        #endregion

        bool initExtractor()
        {
            if (ready)
                return true;

            cachedFiles = new Dictionary<string, string>();

            Logger.LogDebug("Goodmerge: Attempting to initialise temporary extract folder...");
            try
            {
                GoodmergeTempPath = Path.Combine(System.IO.Path.GetTempPath(), GOODMERGE_EXTRACT_FOLDER);
            }
            catch (Exception ex)
            {
                Logger.LogError("Goodmerge: Error getting system temp directory - {0}", ex.Message);
                return false;
            }

            if (!Directory.Exists(GoodmergeTempPath))
            {
                Logger.LogDebug("Goodmerge: Creating temporary extract folder '{0}'", GoodmergeTempPath);
                try
                {
                    Directory.CreateDirectory(GoodmergeTempPath);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Goodmerge: Error creating temporary extract folder - {0}", ex.Message);
                    return false;
                }
            }
            else
            {
                Logger.LogDebug("Goodmerge: Cleaning temporary extract folder '{0}'", GoodmergeTempPath);
                foreach (string s in Directory.GetDirectories(GoodmergeTempPath))
                {
                    if (Path.GetFileName(s).StartsWith(GOODMERGE_FILENAME_PREFIX))
                    {
                        try
                        {
                            Logger.LogDebug("Goodmerge: Deleting folder '{0}'", s);
                            Directory.Delete(s, true);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError("Goodmerge: Error deleting folder '{0}' - {1}, ignoring", s, ex.Message);
                        }
                    }
                }
            }

            Logger.LogInfo("Goodmerge: Initialising complete");
            ready = true;
            return true;
        }

        bool loadArchive(string archivePath)
        {
            if (!initExtractor())
                return false;
            
            Logger.LogDebug("Goodmerge: Loading archive '{0}' ...", archivePath);
            try
            {
                extractor = ArchiveFactory.Open(File.OpenRead(archivePath));
                Logger.LogDebug("Extractor type: {0}, size: {1}kb, solid: {2}, complete: {3}", extractor.Type, extractor.TotalSize / 1024, extractor.IsSolid, extractor.IsComplete);
            }
            catch(Exception ex)
            {
                if (extractor != null)
                {
                    extractor.Dispose();
                    extractor = null;
                }
                Logger.LogError("Goodmerge: Error loading archive '{0}' - {1}", archivePath, ex.Message);
                return false;
            }
            Logger.LogDebug("Goodmerge: Archive loaded successfully");
            return true;
        }

        //If this function returns null the plugin will attempt to launch the item
        //as a rom
        public List<string> ViewFiles(Game game, out int matchIndex)
        {
            lock (syncRoot)
            {
                matchIndex = -1;
                if (!loadArchive(game.CurrentDisc.Path))
                    throw new ExtractException("{0} {1}", Translator.Instance.goodmergearchiveerror, game.CurrentDisc.Path);

                List<string> files = new List<string>();
                using (extractor)
                {
                    //List<IArchiveEntry> entries;
                    List<IArchiveEntry> entries;
                    if (extractor.Entries == null || (entries = extractor.Entries.Where(e => !e.IsDirectory).ToList()).Count < 1)
                    {
                        Logger.LogError("Goodmerge: Archive '{0}' appears to be empty");
                        throw new ExtractException(Translator.Instance.goodmergeempty);
                    }

                    Logger.LogInfo("Goodmerge: Viewing archive '{0}', found {1} file{2}", game.CurrentDisc.Path, entries.Count, entries.Count == 1 ? "" : "s");

                    bool matchFile = !string.IsNullOrEmpty(game.CurrentDisc.LaunchFile);
                    if (entries.Count > 0)
                    {
                        for (int i = 0; i < entries.Count; i++)
                        {
                            string file = entries[i].FilePath;
                            if (matchFile && file == game.CurrentDisc.LaunchFile)
                            {
                                matchIndex = i;
                                matchFile = false;
                            }
                            files.Add(file);
                        }
                        if (matchIndex < 0)
                        {
                            EmulatorProfile profile = game.GetSelectedProfile();
                            if (profile != null)
                                matchIndex = getBestGoodmergeMatch(entries, profile.GetGoodmergeTags());
                        }
                    }
                }
                return files;
            }
        }

        public string ExtractGame(Game game, EmulatorProfile profile = null, ExecutorLaunchProgressHandler progressHandler = null)
        {
            if (profile == null || profile.EmulatorID != game.ParentEmulator.UID)
                profile = game.GetSelectedProfile();

            lock (syncRoot)
            {
                if (progressHandler != null)
                    progressHandler("Loading archive...", 0);

                if (!loadArchive(game.CurrentDisc.Path))
                    throw new ExtractException("{0} {1}", Translator.Instance.goodmergearchiveerror, game.CurrentDisc.Path);

                using (extractor)
                {
                    List<IArchiveEntry> entries;
                    if (extractor.Entries == null || (entries = extractor.Entries.Where(e => !e.IsDirectory).ToList()).Count < 1)
                    {
                        Logger.LogError("Goodmerge: Archive '{0}' appears to be empty");
                        throw new ExtractException(Translator.Instance.goodmergeempty);
                    }

                    if (progressHandler != null)
                        progressHandler("Selecting file...", 0);
                    IArchiveEntry selectedEntry = selectEntry(entries, game.CurrentDisc.LaunchFile, profile.GetGoodmergeTags());
                    Logger.LogDebug("Goodmerge: Selected entry {0}", selectedEntry.FilePath);

                    string cacheKey = string.Format("{0}****{1}", game.CurrentDisc.Path, selectedEntry.FilePath);
                    string extractionPath;
                    if (isInCache(cacheKey, out extractionPath))
                    {
                        Logger.LogDebug("Goodmerge: Using cached file '{0}'", extractionPath);
                        return extractionPath;
                    }

                    extractionPath = Path.Combine(GoodmergeTempPath, GOODMERGE_FILENAME_PREFIX + game.Filename);
                    string extractedFile = extractFile(extractor, selectedEntry, extractionPath, progressHandler);
                    if (extractedFile != null)
                        addToCache(cacheKey, extractedFile);

                    return extractedFile;
                }
            }
        }

        string extractFile(IArchive extractor, IArchiveEntry selectedEntry, string extractionPath, ExecutorLaunchProgressHandler progressHandler = null)
        {
            if (progressHandler != null)
                progressHandler("Creating directory...", 0);

            if (!createExtractionDirectory(extractionPath, selectedEntry.FilePath))
            {
                Logger.LogError("Goodmerge: Unable to find alternate extract directory, hard-coded limit has been reached - check '{0}' for any leftover files", GoodmergeTempPath);
                throw new ExtractException(Translator.Instance.goodmergefileerror);
            }

            Logger.LogDebug("Goodmerge: Extracting file '{0}' to directory '{1}'...", selectedEntry.FilePath, extractionPath);
            if (progressHandler != null)
            {
                progressHandler("Extracting...", 0);
                long totalSize = selectedEntry.Size;

                extractor.EntryExtractionBegin += (sender, e) =>
                {
                    Logger.LogDebug("EntryExtractionBegin {0}", e.Item.FilePath);
                };
                extractor.FilePartExtractionBegin += (sender, e) =>
                {
                    Logger.LogDebug("FilePartExtractionBegin {0}, compressed: {1}kb, total: {2}kb", e.Name, e.CompressedSize / 1024, e.Size / 1024);
                };
                extractor.CompressedBytesRead += (sender, e) =>
                {
                    Logger.LogDebug("CompressedBytesRead {0}, current: {1}kb, total: {2}kb", e.CurrentFilePartCompressedBytesRead / 1024, e.CompressedBytesRead / 1024);
                    int perc = (int)((e.CurrentFilePartCompressedBytesRead * 100) / totalSize);
                    progressHandler(string.Format("Extracting ({0}%)...", perc), perc);
                };
                extractor.EntryExtractionEnd += (sender, e) =>
                {
                    Logger.LogDebug("Extraction complete {0}", e.Item.FilePath);
                    progressHandler("Extraction complete...", 100);
                };
            }

            Logger.LogDebug("Extracting {0}, {1}kb", selectedEntry.FilePath, selectedEntry.Size / 1024);
            string outputFile = Path.Combine(extractionPath, selectedEntry.FilePath);
            try
            {
                selectedEntry.WriteToFile(outputFile);
            }
            catch (Exception ex)
            {
                Logger.LogError("Goodmerge: Error extracting to directory '{0}' - {1}", extractionPath, ex.Message);
                throw new ExtractException(Translator.Instance.goodmergeextracterror);
            }
            return outputFile;
        }

        bool isInCache(string key, out string path)
        {
            if (allowCache)
            {
                if (cachedFiles.TryGetValue(key, out path))
                {
                    if (File.Exists(path))
                        return true;
                    else
                        cachedFiles.Remove(key);
                }
            }
            path = null;
            return false;
        }

        void addToCache(string key, string value)
        {
            if (allowCache)
                cachedFiles[key] = value;
        }

        bool createExtractionDirectory(string extractionPath, string fileName)
        {
            int i = 1;
            string lPath = extractionPath;
            while (i < 11)
            {
                DirectoryInfo directory = new DirectoryInfo(lPath);
                if (!directory.Exists)
                {
                    try
                    {
                        directory.Create();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Goodmerge: Error creating alternate directory '{0}' - {1}", lPath, ex.Message);
                    }
                }
                else
                {
                    FileInfo file = new FileInfo(Path.Combine(lPath, fileName));
                    if (file.Exists)
                    {
                        try
                        {
                            file.Delete();
                            break;
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError("Goodmerge: Error deleting existing file '{0}' - {1}, attempting to use alternate directory", file.FullName, ex.Message);
                        }
                    }
                    else
                        break;
                }

                lPath = string.Format("{0}{1}", extractionPath, i);
                i++;
            }

            return i < 11;
        }

        static IArchiveEntry selectEntry(List<IArchiveEntry> entries, string launchFile, List<string> prefixList)
        {
            if (entries.Count == 1)
                return entries[0];

            if (launchFile != "")
            {
                //there should be a match so loop through first for speed
                for (int x = 0; x < entries.Count; x++)
                {
                    if (launchFile == entries[x].FilePath)
                        return entries[x];
                }
            }

            return entries[getBestGoodmergeMatch(entries, prefixList)];
        }

        static int getBestGoodmergeMatch(List<IArchiveEntry> entries, List<string> prefixList)
        {
            //loop through all items in archive and select best match based on emulator settings
            int priority = prefixList.Count * 2;
            int index = 0;

            for (int x = 0; x < entries.Count; x++)
            {
                string selectedFile = entries[x].FilePath;
                for (int y = 0; y < prefixList.Count; y++)
                {
                    if (priority > y * 2 && selectedFile.Contains(prefixList[y]))
                    {
                        index = x;
                        if (selectedFile.Contains("[!]"))
                            priority = y * 2;
                        else if (priority > y * 2 + 1)
                            priority = y * 2 + 1;
                    }
                }

                if (priority < 1)
                    break;
            }

            return index;
        }
    }
}
