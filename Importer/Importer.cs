using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.ServiceModel;
using System.Drawing;
using System.IO;

namespace MyEmulators2.Import
{
        #region Delegates

        //Sends progress updates
        public delegate void ImportProgressHandler(int percentDone, int taskCount, int taskTotal, string taskDescription);
        //Sends item update events
        public delegate void ImportStatusChangedHandler(object sender, ImportStatusChangedEventArgs e);
        //Sends item update events
        public delegate void RomStatusChangedHandler(object sender, RomStatusChangedEventArgs e);

        public class ImportStatusChangedEventArgs
        {
            public ImportStatusChangedEventArgs(object obj, ImportAction action)
            {
                Object = obj;
                Action = action;
            }

            public object Object { get; protected set; }
            public ImportAction Action { get; protected set; }            
        }

        public class RomStatusChangedEventArgs
        {
            public RomStatusChangedEventArgs(RomMatch romMatch, RomMatchStatus status)
            {
                RomMatch = romMatch;
                Status = status;
            }

            public RomMatch RomMatch { get; protected set; }
            public RomMatchStatus Status { get; protected set; }
        }
        
        #endregion
    
    //Scans the file system and attempts to retrieve details for any new items.
    //The importer is based on code from MovingPictures. Credits to the MovingPictures team!
    public class Importer
    {
        CommunityServerWCFServiceClient client = null;
        DateTime lastConnectionErrorTime = new DateTime();
        int threadCount = 5;
        int hashThreadCount = 2;
        /// <summary>
        /// used to synchronise Start() and Stop()
        /// </summary>
        readonly object syncRoot = new object();
        volatile bool doWork = false;
        /// <summary>
        /// Used to store a complete list of roms currently importing
        /// </summary>
        Dictionary<int, RomMatch> lookupMatch;
        readonly object lookupSync = new object();
        ArrayList allMatches;

        List<Thread> importerThreads;
        volatile int percentDone = 0;
        volatile bool pause = false;

        #region Constructors

        public Importer()
        {
            init();
        }

        /// <summary>
        /// Creates a new Importer with the specified background state.
        /// </summary>
        /// <param name="isBackground">
        /// If true the importer will start downloading data immediately, else the importer will
        /// return a list of files to import and await a call to StartRetrieving()
        /// </param>
        public Importer(bool isBackground, bool justRefresh = false)
        {
            this.isBackground = isBackground;
            this.justRefresh = justRefresh;
            init();
        }

        #endregion

        #region Events

        //Sends progress updates
        public event ImportProgressHandler Progress;
        //Sends import status events
        public event ImportStatusChangedHandler ImportStatusChanged;
        //Sends item update events
        public event RomStatusChangedHandler RomStatusChanged;

        void setRomStatus(RomMatch romMatch, RomMatchStatus status)
        {
            if (ImporterStatus == ImportAction.ImportRestarting)
                return;
            if (RomStatusChanged != null)
                RomStatusChanged(this, new RomStatusChangedEventArgs(romMatch, status));
        }

        void setImporterStatus(object obj, ImportAction action)
        {
            if (ImportStatusChanged != null)
                ImportStatusChanged(this, new ImportStatusChangedEventArgs(obj, action));
        }

        void scanProgress(string message)
        {
            if (Progress != null)
            {
                UpdatePercentDone();
                int processed = allMatches.Count - pendingMatches.Count - priorityPendingMatches.Count - pendingServer.Count - priorityPendingServer.Count - pendingHashes.Count - priorityPendingHashes.Count;
                int total = allMatches.Count;
                Progress(percentDone, processed, total, message);
            }
        }

        private void retrieveProgress(string message)
        {
            if (Progress != null)
            {
                UpdatePercentDone();
                int total = allMatches.Count - matchesNeedingInput.Count;
                int processed = total - approvedMatches.Count - priorityApprovedMatches.Count - pendingMatches.Count - priorityPendingMatches.Count - pendingServer.Count - priorityPendingServer.Count - pendingHashes.Count - priorityPendingHashes.Count;
                Progress(percentDone, processed, total, message);
            }
        }

        private void UpdatePercentDone()
        {
            // calculate the total actions that need to be performed this session
            int mediaToScan = allMatches.Count;
            int mediaToCommit = allMatches.Count - matchesNeedingInput.Count;
            int totalActions = mediaToScan + mediaToCommit;

            // if there is nothing to do, set progress to 100%
            if (totalActions == 0)
            {
                percentDone = 100;
                return;
            }

            // calculate the number of actions completed so far
            int mediaScanned = allMatches.Count - pendingMatches.Count - priorityPendingMatches.Count - pendingServer.Count - priorityPendingServer.Count - pendingHashes.Count - priorityPendingHashes.Count;
            int mediaCommitted = commitedMatches.Count;

            percentDone = (int)Math.Round(((double)mediaScanned + mediaCommitted) * 100 / ((double)totalActions));

            if (percentDone > 100)
                percentDone = 100;
        }

        #endregion

        #region Public Properties
        
        bool isBackground = false;
        bool justRefresh = false;
        public bool JustRefresh
        {
            get { return justRefresh; }
            set { justRefresh = value; }
        }

        ScraperProvider scraperProvider = null;
        internal ScraperProvider ScraperProvider
        {
            get { return scraperProvider; }
        }

        object importStatusLock = new object();
        ImportAction importerStatus = ImportAction.ImportStopped;
        public ImportAction ImporterStatus
        {
            get
            {
                lock (importStatusLock)
                    return importerStatus;
            }
            protected set
            {
                lock (importStatusLock)
                {
                    if (importerStatus == value || (importerStatus == ImportAction.ImportRestarting && value != ImportAction.ImportStarting))
                        return;
                    importerStatus = value;
                    setImporterStatus(null, importerStatus);
                }
            }
        }

        // Matches that have not yet been hashed.
        public ArrayList PendingHashes
        {
            get { return ArrayList.ReadOnly(pendingHashes); }
        } private ArrayList pendingHashes;

        // Matches that have not yet been checked with the community server.
        public ArrayList PendingServer
        {
            get { return ArrayList.ReadOnly(pendingServer); }
        } private ArrayList pendingServer;

        // Matches that have not yet been scraped.
        public ArrayList PendingMatches
        {
            get { return ArrayList.ReadOnly(pendingMatches); }
        } private ArrayList pendingMatches;

        // Same as PendingHashes, but this list gets priority. Used for user based interaction.
        public ArrayList PriorityPendingHashes
        {
            get { return ArrayList.ReadOnly(priorityPendingHashes); }
        } private ArrayList priorityPendingHashes;

        // Same as PendingServer, but this list gets priority. Used for user based interaction.
        public ArrayList PriorityPendingServer
        {
            get { return ArrayList.ReadOnly(priorityPendingServer); }
        } private ArrayList priorityPendingServer;

        // Same as PendingMatches, but this list gets priority. Used for user based interaction.
        public ArrayList PriorityPendingMatches
        {
            get { return ArrayList.ReadOnly(priorityPendingMatches); }
        } private ArrayList priorityPendingMatches;

        // Matches that are not close enough for auto approval and require user input.
        public ArrayList MatchesNeedingInput
        {
            get { return ArrayList.ReadOnly(matchesNeedingInput); }
        } private ArrayList matchesNeedingInput;

        // Matches that the importer is currently pulling details for
        public ArrayList RetrievingDetailsMatches
        {
            get { return ArrayList.ReadOnly(retrievingDetailsMatches); }
        } private ArrayList retrievingDetailsMatches;

        // Matches that are accepted and are awaiting details retrieval and commital. 
        public ArrayList ApprovedMatches
        {
            get { return ArrayList.ReadOnly(approvedMatches); }
        } private ArrayList approvedMatches;

        // Same as ApprovedMatches but this list get's priority. Used for user based interaction.
        public ArrayList PriorityApprovedMatches
        {
            get { return ArrayList.ReadOnly(priorityApprovedMatches); }
        } private ArrayList priorityApprovedMatches;

        // Matches that have been ignored/committed and saved to the database. 
        public ArrayList CommitedMatches
        {
            get { return ArrayList.ReadOnly(commitedMatches); }
        } private ArrayList commitedMatches;

        #endregion
        
        #region Start/Stop

        void init()
        {
            lock (syncRoot)
            {
                threadCount = Options.Instance.GetIntOption("importthreadcount");
                if (threadCount < 1) //0 threads will take a very long time to complete :)
                    threadCount = 1;

                hashThreadCount = Options.Instance.GetIntOption("hashthreadcount");
                if (hashThreadCount < 1)
                    hashThreadCount = 1;

                pendingHashes = ArrayList.Synchronized(new ArrayList());
                pendingServer = ArrayList.Synchronized(new ArrayList());
                pendingMatches = ArrayList.Synchronized(new ArrayList());
                priorityPendingHashes = ArrayList.Synchronized(new ArrayList());
                priorityPendingServer = ArrayList.Synchronized(new ArrayList());
                priorityPendingMatches = ArrayList.Synchronized(new ArrayList());
                matchesNeedingInput = ArrayList.Synchronized(new ArrayList());
                approvedMatches = ArrayList.Synchronized(new ArrayList());
                priorityApprovedMatches = ArrayList.Synchronized(new ArrayList());
                retrievingDetailsMatches = ArrayList.Synchronized(new ArrayList());
                commitedMatches = ArrayList.Synchronized(new ArrayList());

                importerThreads = new List<Thread>();
                allMatches = ArrayList.Synchronized(new ArrayList());
                lookupMatch = new Dictionary<int, RomMatch>();

                scraperProvider = new ScraperProvider();
                scraperProvider.DoWork += new DoWorkDelegate(() => doWork);
            }
        }

        public void Start()
        {
            lock (syncRoot)
            {
                ImporterStatus = ImportAction.ImportStarting;

                if (Options.Instance.GetBoolOption("retrieveGameDetials") || Options.Instance.GetBoolOption("submitGameDetails"))
                {
                    //var myBinding = new BasicHttpBinding();
                    //var myEndpoint = new EndpointAddress("http://" + Options.Instance.GetStringOption("communityServerAddress") + "/CommunityServerService/service");
                    //client = new CommunityServerWCFServiceClient(myBinding, myEndpoint);
                }

                doWork = true;
                if (importerThreads.Count == 0)
                {
                    if (!justRefresh) //start retrieving immediately
                    {
                        ScanRomsDelegate scanMethod;
                        if (isBackground)
                            scanMethod = scanRomsBackground; //retrieve info as soon as match found
                        else
                            scanMethod = scanRomsDefault; //get all possible matches before retrieving so user can check/edit

                        Thread firstThread = new Thread(new ThreadStart(delegate()
                        {
                            getFilesToImport(); //first thread needs to locate games for import
                            if (doWork)
                            {
                                ImporterStatus = ImportAction.ImportStarted;
                                scanRoms(scanMethod, true);
                            }
                            ImporterStatus = ImportAction.ImportFinished;
                        }
                            ));
                        firstThread.Name = "Importer Thread";
                        firstThread.Start();
                        importerThreads.Add(firstThread);

                        //start rest of threads
                        for (int x = 0; x < threadCount - 1; x++)
                        {
                            Thread thread = new Thread(new ThreadStart(delegate() { scanRoms(scanMethod, false); }));
                            thread.Name = "Importer Thread";
                            thread.Start();
                            importerThreads.Add(thread);
                        }

                    }
                    else //just get list of files to import
                    {
                        Thread firstThread = new Thread(new ThreadStart(delegate()
                        {
                            lock (importStatusLock)
                                ImporterStatus = ImportAction.ImportRefreshing;
                            getFilesToImport();
                            lock (importStatusLock)
                                ImporterStatus = ImportAction.ImportFinished;
                        }
                            ));
                        firstThread.Name = "Importer Thread";
                        importerThreads.Add(firstThread);
                        firstThread.Start();
                    }
                }
            }
        }

        public void Stop()
        {
            lock (syncRoot)
            {
                ImporterStatus = ImportAction.ImportStopping;
                doWork = false;

                if (importerThreads.Count > 0)
                {
                    Logger.LogInfo("Shutting Down Importer Threads...");
                    // wait for all threads to shut down
                    bool waiting = true;
                    while (waiting)
                    {
                        waiting = false;
                        foreach (Thread currThread in importerThreads)
                            waiting = waiting || currThread.IsAlive;
                        Thread.Sleep(100);
                    }

                    importerThreads.Clear();
                }

                if (Progress != null)
                    Progress(100, 0, 0, "Stopped");
                ImporterStatus = ImportAction.ImportStopped;
                Logger.LogInfo("Stopped Importer");
            }
        }

        public void Restart()
        {
            lock (importStatusLock)
            {
                if (importerStatus == ImportAction.ImportRestarting)
                    return;
                ImporterStatus = ImportAction.ImportRestarting;
            }
            new Thread(delegate()
                {
                    lock (syncRoot)
                    {
                        Stop();
                        init();
                        Start();
                    }
                }) { Name = "Importer restarter", IsBackground = true }.Start();
        }

        public void Pause()
        {
            pause = true;
        }

        public void UnPause()
        {
            pause = false;
        }

        #endregion

        #region Public Methods

        internal void AddGames(IEnumerable<Game> games)
        {
            lock (importStatusLock)
            {
                List<int> gameIds = new List<int>();
                DB.Instance.ExecuteTransaction(games, game =>
                {
                    if (game == null || gameIds.Contains(game.GameID))
                        return;

                    gameIds.Add(game.GameID);
                    RomMatch romMatch;
                    lock (lookupSync)
                    {
                        if (lookupMatch.ContainsKey(game.GameID))
                        {
                            romMatch = lookupMatch[game.GameID];
                            lock (romMatch.SyncRoot)
                            {
                                RemoveFromMatchLists(romMatch);
                                romMatch.CurrentThreadId = null;
                                romMatch.Status = RomMatchStatus.Removed;
                            }
                        }
                        game.Reset();
                        romMatch = new RomMatch(game);
                        lookupMatch[game.GameID] = romMatch;
                        allMatches.Add(romMatch);
                    }

                    game.IsInfoChecked = false;
                    game.SaveInfoCheckedStatus();
                    setRomStatus(romMatch, RomMatchStatus.PendingHash);
                    lock (priorityPendingHashes.SyncRoot)
                        priorityPendingHashes.Add(romMatch);

                });

                if (importerStatus != ImportAction.ImportRestarting && importerStatus != ImportAction.ImportStarted)
                {
                    justRefresh = false;
                    Restart();
                }
            }
        }
        /// <summary>
        /// Resets RomMatch status and re-retrieves info
        /// </summary>
        /// <param name="romMatch"></param>
        public void ReProcess(RomMatch romMatch)
        {
            if (romMatch == null)
                return;

            lock (lookupSync)
            {
                if (lookupMatch.ContainsKey(romMatch.ID))
                    romMatch = lookupMatch[romMatch.ID];
                lock (romMatch.SyncRoot)
                {
                    RemoveFromMatchLists(romMatch);
                    romMatch.Status = RomMatchStatus.PendingHash;
                    romMatch.HighPriority = true;
                    romMatch.CurrentThreadId = null;
                    romMatch.GameDetails = null;
                    romMatch.PossibleGameDetails = new List<ScraperResult>();
                }
                lookupMatch[romMatch.ID] = romMatch;
                allMatches.Add(romMatch);
            }
            romMatch.Game.IsInfoChecked = false;
            romMatch.Game.SaveInfoCheckedStatus();
            setRomStatus(romMatch, RomMatchStatus.PendingHash);
            lock (priorityPendingHashes.SyncRoot)
                priorityPendingHashes.Add(romMatch);
        }

        public void Approve(RomMatch romMatch)
        {
            Approve(new RomMatch[] { romMatch });
        }
        /// <summary>
        /// Approve a RomMatch requiring input and queue downloading selected info
        /// </summary>
        /// <param name="romMatches"></param>
        public void Approve(IEnumerable<RomMatch> romMatches)
        {
            DB.Instance.ExecuteTransaction(romMatches, romMatch =>
            {
                if (romMatch.PossibleGameDetails == null || romMatch.PossibleGameDetails.Count < 1)
                    return;

                int gameId = romMatch.Game.GameID;
                lock (lookupSync)
                {
                    if (lookupMatch.ContainsKey(gameId))
                    {
                        romMatch = lookupMatch[gameId];
                        lock (romMatch.SyncRoot)
                        {
                            if (romMatch.Status != RomMatchStatus.Approved && romMatch.Status != RomMatchStatus.NeedsInput)
                                return;

                            RemoveFromMatchLists(romMatch);
                            romMatch.Status = RomMatchStatus.Approved;
                            romMatch.CurrentThreadId = null;
                            romMatch.HighPriority = true;
                        }
                        romMatch.Game.IsInfoChecked = false;
                        romMatch.Game.SaveInfoCheckedStatus();
                        lookupMatch[romMatch.ID] = romMatch;
                        allMatches.Add(romMatch);
                        setRomStatus(romMatch, RomMatchStatus.Approved);
                        lock (priorityApprovedMatches.SyncRoot)
                            priorityApprovedMatches.Add(romMatch);
                    }
                }
            });
        }

        public void UpdateSelectedMatch(RomMatch romMatch, ScraperResult newResult)
        {
            if (newResult == null)
                return;
            lock (lookupSync)
            {
                if (lookupMatch.ContainsKey(romMatch.ID))
                {
                    romMatch = lookupMatch[romMatch.ID];
                    lock (romMatch.SyncRoot)
                    {
                        RemoveFromMatchLists(romMatch);
                        romMatch.Status = RomMatchStatus.Approved;
                        romMatch.CurrentThreadId = null;
                        romMatch.HighPriority = true;
                    }
                    romMatch.GameDetails = newResult;
                    romMatch.Game.IsInfoChecked = false;
                    romMatch.Game.SaveInfoCheckedStatus();
                    lookupMatch[romMatch.ID] = romMatch;
                    allMatches.Add(romMatch);
                    lock (priorityApprovedMatches.SyncRoot)
                        priorityApprovedMatches.Add(romMatch);
                    setRomStatus(romMatch, RomMatchStatus.Approved);
                }
            }
        }

        public void Ignore(RomMatch romMatch)
        {
            Ignore(new RomMatch[] { romMatch });
        }
        /// <summary>
        /// Removes the RomMatch from the Importer
        /// </summary>
        /// <param name="romMatch"></param>
        public void Ignore(IEnumerable<RomMatch> romMatches)
        {
            DB.Instance.ExecuteTransaction(romMatches, romMatch =>
            {
                lock (lookupSync)
                {
                    if (lookupMatch.ContainsKey(romMatch.ID))
                    {
                        RomMatch lMatch = lookupMatch[romMatch.ID];
                        lock (lMatch.SyncRoot)
                        {
                            RemoveFromMatchLists(lMatch);
                            lMatch.Status = RomMatchStatus.Ignored;
                            lMatch.CurrentThreadId = null;
                        }
                        lMatch.Game.Reset();
                        lMatch.Game.IsInfoChecked = true;
                        lMatch.Game.Save();
                        setRomStatus(lMatch, RomMatchStatus.Ignored);
                    }
                }
            });
        }

        public void Remove(int gameId)
        {
            RomMatch romMatch;
            lock (lookupSync)
            {
                if (lookupMatch.ContainsKey(gameId))
                {
                    romMatch = lookupMatch[gameId];
                    lock (romMatch.SyncRoot)
                    {
                        RemoveFromMatchLists(romMatch);
                        romMatch.Status = RomMatchStatus.Removed;
                        romMatch.CurrentThreadId = null;
                    }
                    setRomStatus(romMatch, RomMatchStatus.Removed);
                }
            }
        }

        #endregion

        #region Scan Logic
        // The main loop that the import threads will run, checks for pending actions and updates status
        void scanRoms(ScanRomsDelegate scanMethod, bool raiseEvents)
        {
            try
            {
                bool processHashes = true;
                bool skippedHash = false;
                while (doWork)
                {
                    if (pause)
                    {
                        if (raiseEvents)
                            setImporterStatus(null, ImportAction.ImportPaused);
                        while (pause)
                            if (!checkAndWait(100, 10, true))
                                return;
                        if (raiseEvents)
                            setImporterStatus(null, ImportAction.ImportResumed);
                    }

                    int previousCommittedCount = commitedMatches.Count;
                    int previousMatchesCount = allMatches.Count;
                    // if there is nothing to process, then sleep
                    while (pendingHashes.Count == 0 &&
                           pendingServer.Count == 0 &&
                           pendingMatches.Count == 0 &&
                           approvedMatches.Count == 0 &&
                           priorityPendingMatches.Count == 0 &&
                           priorityApprovedMatches.Count == 0 &&
                           priorityPendingHashes.Count == 0 &&
                           priorityPendingServer.Count == 0 &&
                           commitedMatches.Count == previousCommittedCount &&
                           previousMatchesCount == allMatches.Count &&
                           doWork
                           )
                    {
                        if (!checkAndWait(100, 10, false))
                            return;
                    }

                    if (skippedHash)
                    {
                        processHashes = true;
                        skippedHash = false;
                    }
                    if (!processHashes)
                        skippedHash = true;

                    scanMethod.Invoke(ref processHashes);

                    if (!doWork)
                        return;

                    UpdatePercentDone();
                    lock (importStatusLock)
                    {
                        if (!doWork)
                            return;

                        // if we are now just waiting on the user, say so
                        if (pendingHashes.Count == 0 && priorityPendingHashes.Count == 0 &&
                            pendingServer.Count == 0 && priorityPendingServer.Count == 0 &&
                            pendingMatches.Count == 0 && approvedMatches.Count == 0 &&
                            priorityPendingMatches.Count == 0 && priorityApprovedMatches.Count == 0 &&
                            allMatches.Count == commitedMatches.Count + matchesNeedingInput.Count &&
                            matchesNeedingInput.Count > 0)
                        {
                            if (Progress != null)
                                Progress(percentDone, 0, matchesNeedingInput.Count, "Waiting for Approvals...");

                            if (isBackground)
                            {
                                doWork = false;
                                ImporterStatus = ImportAction.ImportFinished;
                                return;
                            }
                        }

                        // if we are now just waiting on the user, say so
                        if (pendingHashes.Count == 0 && priorityPendingHashes.Count == 0 &&
                            pendingServer.Count == 0 && priorityPendingServer.Count == 0 &&
                            pendingMatches.Count == 0 && approvedMatches.Count == 0 &&
                            priorityPendingMatches.Count == 0 && priorityApprovedMatches.Count == 0 &&
                            matchesNeedingInput.Count == 0)
                        {                            
                            if (commitedMatches.Count == allMatches.Count)
                            {
                                if (Progress != null)
                                {
                                    UpdatePercentDone();
                                    if (percentDone == 100)
                                        Progress(100, 0, 0, "Done!");
                                }

                                if (isBackground)
                                {
                                    doWork = false;
                                    ImporterStatus = ImportAction.ImportFinished;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Unhandled error in Importer - {0}", e.Message);
            }
        }

        delegate void ScanRomsDelegate(ref bool processHashes);
        // The order is to have the user selected roms be done first, and then the standard import order second.
        // The order needs to be Hash, Server, Pending Matches, Approved Matches
        void scanRomsDefault(ref bool processHashes)
        {
            if (processHashes && priorityPendingHashes.Count > 0)
                processHashes = processNextPendingHash();
            else if (priorityPendingServer.Count > 0)
                processNextPendingServer();
            else if (priorityPendingMatches.Count > 0)
                processNextPendingMatch();
            else if (priorityApprovedMatches.Count > 0)
                processNextApprovedMatch();
            else if (processHashes && pendingHashes.Count > 0)
                processHashes = processNextPendingHash();
            else if (pendingServer.Count > 0)
                processNextPendingServer();
            else if (pendingMatches.Count > 0)
                processNextPendingMatch();
            else if (approvedMatches.Count > 0)
                processNextApprovedMatch();
        }

        //alternate order when background, get info asap so GUI can be updated
        void scanRomsBackground(ref bool processHashes)
        {
            if (priorityApprovedMatches.Count > 0)
                processNextApprovedMatch();
            else if (priorityPendingMatches.Count > 0)
                processNextPendingMatch();
            else if (priorityPendingServer.Count > 0)
                processNextPendingServer();
            else if (processHashes && priorityPendingHashes.Count > 0)
                processHashes = processNextPendingHash();
            else if (approvedMatches.Count > 0)
                processNextApprovedMatch();
            else if (pendingMatches.Count > 0)
                processNextPendingMatch();
            else if (pendingServer.Count > 0)
                processNextPendingServer();
            else if (processHashes && pendingHashes.Count > 0)
                processHashes = processNextPendingHash();            
        }
        #endregion

        #region Refresh Database

        //Refreshes the DB and builds list of RomMatches based on Game.IsInfoChecked
        //called first when started
        void getFilesToImport()
        {
            try
            {
                deleteMissingGames();
                if (!doWork)
                    return;
                refreshDatabase();
                if (justRefresh || !doWork)
                    return;

                List<RomMatch> localMatches = new List<RomMatch>();

                foreach (Game game in DB.Instance.GetGames())
                {
                    if (!doWork)
                        return;

                    if (!game.IsInfoChecked)
                    {
                        if (!System.IO.File.Exists(game.Path))
                        {
                            Logger.LogError("Unable to locate '{0}', removing {1} from importer", game.Path, game.Title);
                            game.IsInfoChecked = true;
                            game.SaveInfoCheckedStatus();
                        }
                        else
                        {
                            game.Reset();
                            localMatches.Add(new RomMatch(game));
                        }
                    }
                }

                Logger.LogDebug("Adding {0} item{1} to importer", localMatches.Count, localMatches.Count == 1 ? "" : "s");
                if (localMatches.Count > 0)
                {
                    for (int x = 0; x < localMatches.Count; x++)
                    {
                        lock (lookupSync)
                        {
                            if (!lookupMatch.ContainsKey(localMatches[x].ID))
                            {
                                lookupMatch[localMatches[x].ID] = localMatches[x];
                                allMatches.Add(localMatches[x]);

                                lock (pendingHashes.SyncRoot)
                                {
                                    pendingHashes.Add(localMatches[x]);
                                }
                            }
                        }
                    }

                    setImporterStatus(localMatches, ImportAction.PendingFilesAdded);
                    if (Progress != null)
                        Progress(0, 0, 0, "Ready");
                }
                else //no files need importing
                {
                    if (isBackground)
                    {
                        doWork = false;
                        return;
                    }
                    else
                        setImporterStatus(null, ImportAction.NoFilesFound);
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Unhandled error in Importer - {0} - {1}", e.Message, e.StackTrace);
            }
        }

        void refreshDatabase()
        {
            Logger.LogDebug("Refreshing list of roms");
            List<string> dbPaths = DB.Instance.GetAllGamePaths();
            List<Emulator> emus = DB.Instance.GetEmulators(true);
            
            if (!doWork)
                return;

            Dictionary<Emulator, List<string>> allNewPaths = new Dictionary<Emulator, List<string>>();
            int filesFound = 0;
            if (Progress != null)
                Progress(0, 0, 0, "Refreshing Database");

            //loop through each emu
            foreach (Emulator emu in emus)
            {
                if (!doWork)
                    return;

                Logger.LogDebug("Getting files for emulator {0}", emu.Title);

                //check if rom dir exists
                string romDir = emu.PathToRoms;
                if (string.IsNullOrEmpty(romDir))
                    continue;
                if (!System.IO.Directory.Exists(romDir))
                {
                    Logger.LogError("{0} rom directory does not exist", emu.Title);
                    continue; //rom directory doesn't exist, skip
                }

                List<string> newPaths = new List<string>();
                //get list of files using each filter
                foreach (string filter in emu.Filter.Split(';'))
                {
                    if (!doWork)
                        return;

                    string[] gamePaths;
                    try
                    {
                        gamePaths = System.IO.Directory.GetFiles(romDir, filter, System.IO.SearchOption.AllDirectories); //get list of matches
                    }
                    catch
                    {
                        Logger.LogError("Error locating files in {0} rom directory using filter '{1}'", emu.Title, filter);
                        continue; //error with filter, skip
                    }

                    //loop through each new file
                    for (int x = 0; x < gamePaths.Length; x++)
                    {
                        if (!doWork)
                            return;

                        string path = gamePaths[x];
                        //check that path is not ignored, already in DB or not already picked up by a previous filter
                        if (!Options.Instance.ShouldIgnoreFile(path) && !dbPaths.Contains(path) && !newPaths.Contains(path))
                        {
                            newPaths.Add(path);
                            filesFound++;
                            if (Progress != null)
                                Progress(0, 0, filesFound, "Getting new items");
                        }
                    }
                }

                Logger.LogDebug("Found {0} new file(s)", newPaths.Count);
                if (newPaths.Count > 0)
                    allNewPaths.Add(emu, newPaths);
            }

            if (allNewPaths.Count < 1)
                return;

            int filesAdded = 0;

            Logger.LogDebug("Commit started at {0}", DateTime.Now.ToLongTimeString());
            DateTime startTime = DateTime.Now;

            foreach (KeyValuePair<Emulator, List<string>> val in allNewPaths)
            {
                //loop through each new file and commit
                DB.Instance.ExecuteTransaction(val.Value, path =>
                {
                    Game game = new Game(path, val.Key);
                    DB.Instance.ExecuteWithoutLock(game.GetDBInsertOrIgnoreString());
                    filesAdded++;
                    int perc = (int)Math.Round(((double)filesAdded / filesFound) * 100);
                    if (Progress != null)
                        Progress(perc, filesAdded, filesFound, "Adding " + game.Title);
                });
            }

            DateTime finishTime = DateTime.Now;
            setImporterStatus(null, ImportAction.NewFilesFound);
            Logger.LogDebug("Commit finished at {0}", finishTime.ToLongTimeString());
            Logger.LogDebug("Total time {0}ms", finishTime.Subtract(startTime).TotalMilliseconds);
        }

        void deleteMissingGames()
        {
            if (Progress != null)
                Progress(0, 0, 0, "Removing deleted games");
            List<Game> games = DB.Instance.GetGames();
            List<string> missingDrives = new List<string>();
            DB.Instance.ExecuteTransaction(games, game =>
            {
                string path = game.Path;
                string drive = Path.GetPathRoot(path);
                if (!missingDrives.Contains(drive))
                {
                    //if path root is missing assume file is on disconnected
                    //removable/network drive and don't delete
                    if (!Directory.Exists(drive))
                        missingDrives.Add(drive); 
                    else if (!File.Exists(path))
                    {
                        if (Progress != null)
                            Progress(0, 0, 0, string.Format("Removing {0}", game.Title));
                        Logger.LogDebug("Removing {0} from the database, file not found", game.Title);
                        game.Delete();
                    }
                }
            });
        }

        #endregion

        #region Hash
        //object hashLock = new object();
        //int currentHashThreads = 0;

        /// <summary>
        /// CURRENTLY DISABLED UNTIL COMMUNITY SERVER READY
        /// Grabs the next game in the list and hashes it.
        /// </summary>
        private bool processNextPendingHash()
        {
            //lock (hashLock)
            //{
            //    if (currentHashThreads >= hashThreadCount)
            //        return false;
            //    currentHashThreads++;
            //}

            RomMatch romMatch = takeFromList(pendingHashes, priorityPendingHashes);
            if (romMatch == null)
                return true;

            //try
            //{
            //    Hasher.Hasher.Hashes hashes;
            //    if (romMatch.Game.Hash.Trim().Length == 0)
            //    {
            //        hashes = Hasher.Hasher.CalculateHashes(romMatch.Game.Path, delegate(string fileName, int percentComplete)
            //        {
            //            if (romMatch.Ignore || !doWork)
            //                return 0;
            //            else
            //                return OnHashProgress(fileName, percentComplete);
            //        });
            //    }
            //    else
            //    {
            //        hashes = new Hasher.Hasher.Hashes();
            //        hashes.ed2k = romMatch.Game.Hash;
            //        hashes.crc32 = "";
            //        hashes.md5 = "";
            //        hashes.sha1 = "";
            //    }

            //    // Put hash into game object
            //    romMatch.Game.Hash = hashes.ed2k;

            //}
            //catch (Exception ex)
            //{
            //    Logger.LogError("Error hashing {0} - {1}", romMatch.Game.Title, ex.Message);
            //    Logger.LogInfo("Importer: Ignoring {0}", romMatch.Game.Title);
            //    Ignore(romMatch);

            //    lock (hashLock)
            //        currentHashThreads--;

            //    return true;
            //}

            //lock (hashLock)
            //    currentHashThreads--;


            //// Pass game onto Community server queue
            //if (romMatch.HighPriority)
            //{
            //    lock (priorityPendingServer.SyncRoot)
            //    {
            //        if (!romMatch.Ignore)
            //            priorityPendingServer.Add(romMatch);
            //    }
            //}
            //else
            //{
            //    lock (pendingServer.SyncRoot)
            //    {
            //        if (!romMatch.Ignore)
            //            pendingServer.Add(romMatch);
            //    }
            //}

            //skip straight to pending matches until community server finishes
            addToList(romMatch, RomMatchStatus.PendingMatch, pendingMatches, priorityPendingMatches);
            return true;
        }

        /// <summary>
        /// Receives current percentage of the hash progress of the specific file
        /// </summary>
        /// <param name="fileName">Path of file being hashed</param>
        /// <param name="percentComplete">Percentage of hash progress completed</param>
        /// <returns></returns>
        public int OnHashProgress(string fileName, int percentComplete)
        {
            if (Progress != null)
            {
                UpdatePercentDone();
                int processed = allMatches.Count - pendingHashes.Count - priorityPendingHashes.Count;
                int total = allMatches.Count;
                Progress(percentDone, processed, total, "Hashing File: " + fileName + " - " + percentComplete + "%");
            }

            if (!doWork)
                return 0;

            return 1; //continue hashing (return 0 to abort)
        }
        #endregion

        #region Server
        private void processNextPendingServer()
        {
            RomMatch romMatch = takeFromList(pendingServer, priorityPendingServer);
            if (romMatch == null)
                return;

            // Get game info from community server.
            TimeSpan checkingTime = new TimeSpan(0, Options.Instance.GetIntOption("communityServerConnectionRetryTime"), 0);
            DateTime checkTime = DateTime.Now.Subtract(checkingTime);

            myemulators2.v1.GameDetails receivedGameDetails = null;

            if (lastConnectionErrorTime > checkTime && Options.Instance.GetBoolOption("retrieveGameDetials"))
            {
                try
                {
                    myemulators2.v1.GameDetails sendGameDetails = new myemulators2.v1.GameDetails();

                    sendGameDetails.Hash = romMatch.Game.Hash;
                    sendGameDetails.Filename = romMatch.Path;

                    receivedGameDetails = client.RequestGameDetials(sendGameDetails);

                    Logger.LogDebug("{0}", client.State.ToString());
                }
                catch (TimeoutException timeout)
                {
                    // Handle the timeout exception.
                    Logger.LogError("Community Server Timeout Error: " + timeout.Message);
                    lastConnectionErrorTime = DateTime.Now;
                    client.Abort();
                }
                catch (CommunicationException commException)
                {
                    // Handle the communication exception.
                    Logger.LogError("Community Server Error: " + commException.Message);
                    lastConnectionErrorTime = DateTime.Now;
                    client.Abort();
                }
            }

            // Pass game onto meding matches queue
            addToList(romMatch, RomMatchStatus.PendingMatch, PendingMatches, PriorityPendingMatches);
        }
        #endregion

        #region Pending Matches
        /// <summary>
        /// Selects the next pending match and searches for possible game matches, if a close match is found the RomMatch
        /// is added to the Approve queue else it is added to the matches needing input queue.
        /// </summary>
        private void processNextPendingMatch()
        {
            RomMatch romMatch = takeFromList(pendingMatches, priorityPendingMatches);
            if (romMatch == null)
                return;

            scanProgress("Retrieving matches: " + romMatch.Title);

            //get possible matches and
            //update RomMatch
            if (!romMatch.Game.IsMissingInfo())
            {
                lock (romMatch.SyncRoot)
                {
                    if (!romMatch.OwnedByThread())
                        return;

                    romMatch.Game.IsInfoChecked = true;
                    romMatch.Game.SaveGameDetails();
                    romMatch.Status = RomMatchStatus.Committed;
                    lock (commitedMatches.SyncRoot)
                    {
                        commitedMatches.Add(romMatch);
                        setRomStatus(romMatch, RomMatchStatus.Committed);
                    }
                }
                return;
            }
            
            ScraperResult bestResult; bool approved;
            List<ScraperResult> results = scraperProvider.GetMatches(romMatch, out bestResult, out approved);
            RomMatchStatus status;
            ArrayList addList;
            ArrayList priorityAddList;
            lock (romMatch.SyncRoot)
            {
                if (!romMatch.OwnedByThread())
                    return;

                romMatch.PossibleGameDetails = results;
                romMatch.GameDetails = bestResult;
                if (approved) //close match found, add to approve list
                {
                    status = RomMatchStatus.Approved;
                    addList = approvedMatches;
                    priorityAddList = priorityApprovedMatches;
                }
                else //close match not found, request user input
                {
                    status = RomMatchStatus.NeedsInput;
                    addList = matchesNeedingInput;
                    priorityAddList = null;
                }
            }
            addToList(romMatch, status, addList, priorityAddList);
        }
        #endregion

        #region Approved Matches
        //Selects next Match from approved match lists.
        //Updates the Game with the specified Match details and commits
        void processNextApprovedMatch()
        {
            RomMatch romMatch = takeFromList(approvedMatches, priorityApprovedMatches);
            if (romMatch == null)
                return;
            Scraper selectedScraper;
            ScraperResult selectedMatch;
            lock (romMatch.SyncRoot)
            {
                if (!romMatch.OwnedByThread())
                    return;
                selectedScraper = romMatch.GameDetails.DataProvider;
                selectedMatch = romMatch.GameDetails;
            }

            if (!doWork)
                return;
            retrieveProgress("Updating: " + romMatch.Title);

            //get info
            ScraperGame scraperGame = scraperProvider.DownloadInfo(selectedMatch); //selectedScraper.DownloadInfo(selectedMatch);
            if (!doWork || !romMatch.OwnedByThread())
                return;

            ThumbGroup thumbGroup = new ThumbGroup(romMatch.Game);
            if (scraperGame.BoxFrontUrl != null)
            {
                using (Bitmap image = getImage(scraperGame.BoxFrontUrl, romMatch))
                {
                    if (!doWork)
                        return;
                    if (image != null)
                    {
                        lock (romMatch.SyncRoot)
                        {
                            if (!romMatch.OwnedByThread())
                                return;
                            thumbGroup.FrontCover.Image = image;
                            thumbGroup.SaveThumb(ThumbType.FrontCover);
                            thumbGroup.FrontCover.Image = null;
                        }
                    }
                }
            }

            if (scraperGame.BoxBackUrl != null)
            {
                using (Bitmap image = getImage(scraperGame.BoxBackUrl, romMatch))
                {
                    if (!doWork)
                        return;
                    if (image != null)
                    {
                        lock (romMatch.SyncRoot)
                        {
                            if (!romMatch.OwnedByThread())
                                return;
                            thumbGroup.BackCover.Image = image;
                            thumbGroup.SaveThumb(ThumbType.BackCover);
                            thumbGroup.BackCover.Image = null;
                        }
                    }
                }
            }

            if (scraperGame.TitleScreenUrl != null)
            {
                using (Bitmap image = getImage(scraperGame.TitleScreenUrl, romMatch))
                {
                    if (!doWork)
                        return;
                    if (image != null)
                    {
                        lock (romMatch.SyncRoot)
                        {
                            if (!romMatch.OwnedByThread())
                                return;
                            thumbGroup.TitleScreen.Image = image;
                            thumbGroup.SaveThumb(ThumbType.TitleScreen);
                            thumbGroup.TitleScreen.Image = null;
                        }
                    }
                }
            }

            if (scraperGame.InGameUrl != null)
            {
                using (Bitmap image = getImage(scraperGame.InGameUrl, romMatch))
                {
                    if (!doWork)
                        return;
                    if (image != null)
                    {
                        lock (romMatch.SyncRoot)
                        {
                            if (!romMatch.OwnedByThread())
                                return;
                            thumbGroup.InGame.Image = image;
                            thumbGroup.SaveThumb(ThumbType.InGameScreen);
                            thumbGroup.InGame.Image = null;
                        }
                    }
                }
            }

            if (scraperGame.FanartUrl != null)
            {
                using (Bitmap image = getImage(scraperGame.FanartUrl, romMatch))
                {
                    if (!doWork)
                        return;
                    if (image != null)
                    {
                        lock (romMatch.SyncRoot)
                        {
                            if (!romMatch.OwnedByThread())
                                return;
                            thumbGroup.Fanart.Image = image;
                            thumbGroup.SaveThumb(ThumbType.Fanart);
                            thumbGroup.Fanart.Image = null;
                        }
                    }
                }
            }

            lock (romMatch.SyncRoot)
            {
                if (!romMatch.OwnedByThread())
                    return;
                romMatch.ScraperGame = scraperGame;
                commitGame(romMatch);
                if (!doWork)
                    return;
            }
            addToList(romMatch, RomMatchStatus.Committed, commitedMatches, null);
        }

        Bitmap getImage(string url, RomMatch romMatch)
        {
            Bitmap img = null;
            BitmapDownloadResult result = ImageHandler.BeginBitmapFromWeb(url);
            if (result != null)
            {
                bool cancel = false;
                while (!result.IsCompleted)
                {
                    if (!doWork || !romMatch.OwnedByThread())
                    {
                        cancel = true;
                        break;
                    }
                    System.Threading.Thread.Sleep(100);
                }
                if (cancel)
                {
                    result.Cancel();
                    return null;
                }
                img = result.Bitmap;
            }
            return img;
        }

        //update and commit game
        void commitGame(RomMatch romMatch)
        {
            List<Game> games = DB.Instance.GetGames("WHERE path='" + DB.Encode(romMatch.Game.Path) + "'");
            if (games.Count < 1) //rom deleted
                return;

            Game dbGame = games[0];

            ScraperGame details = romMatch.ScraperGame;
            if (details == null || !doWork)
                return;

            if (!string.IsNullOrEmpty(details.Title))
                dbGame.Title = details.Title;

            if (!string.IsNullOrEmpty(details.Company))
                dbGame.Company = details.Company;

            if (!string.IsNullOrEmpty(details.Description))
                dbGame.Description = details.Description;

            if (!string.IsNullOrEmpty(details.Year))
            {
                int year;
                if (!int.TryParse(details.Year, out year))
                    year = 0;
                dbGame.Yearmade = year;
            }

            if (!string.IsNullOrEmpty(details.Genre))
                dbGame.Genre = details.Genre;

            if (!string.IsNullOrEmpty(details.Grade))
            {
                double grade;
                if (!double.TryParse(details.Grade, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out grade))
                    grade = 0;

                while (grade > 10)
                {
                    grade = grade / 10;
                }

                dbGame.Grade = (int)Math.Round(grade);
            }

            if (!doWork)
                return;

            dbGame.IsInfoChecked = true;
            dbGame.SaveGameDetails();
        }
        #endregion

        // removes the given match from all pending process lists
        private void RemoveFromMatchLists(RomMatch match)
        {
            lock (pendingHashes.SyncRoot)
            {
                if (pendingHashes.Contains(match))
                    pendingHashes.Remove(match);
            }

            lock (priorityPendingHashes.SyncRoot)
            {
                if (priorityPendingHashes.Contains(match))
                {
                    priorityPendingHashes.Remove(match);
                }
            }

            lock (pendingServer.SyncRoot)
            {
                if (pendingServer.Contains(match))
                    pendingServer.Remove(match);
            }

            lock (priorityPendingServer.SyncRoot)
            {
                if (priorityPendingServer.Contains(match))
                {
                    priorityPendingServer.Remove(match);
                }
            }

            lock (pendingMatches.SyncRoot)
            {
                if (pendingMatches.Contains(match))
                    pendingMatches.Remove(match);
            }

            lock (priorityPendingMatches.SyncRoot)
            {
                if (priorityPendingMatches.Contains(match))
                {
                    priorityPendingMatches.Remove(match);
                }
            }

            lock (matchesNeedingInput.SyncRoot)
            {
                if (matchesNeedingInput.Contains(match))
                    matchesNeedingInput.Remove(match);
            }

            lock (approvedMatches.SyncRoot)
            {
                if (approvedMatches.Contains(match))
                    approvedMatches.Remove(match);
            }

            lock (priorityApprovedMatches.SyncRoot)
            {
                if (priorityApprovedMatches.Contains(match))
                    priorityApprovedMatches.Remove(match);
            }

            lock (commitedMatches.SyncRoot)
            {
                if (commitedMatches.Contains(match))
                {
                    commitedMatches.Remove(match);
                }
            }

            lock (retrievingDetailsMatches.SyncRoot)
            {
                if (retrievingDetailsMatches.Contains(match))
                {
                    retrievingDetailsMatches.Remove(match);
                }
            }

            lock (lookupSync)
            {
                if (lookupMatch.ContainsKey(match.ID))
                {
                    lookupMatch.Remove(match.ID);
                    allMatches.Remove(match);
                }
            }
        }

        RomMatch takeFromList(ArrayList list, ArrayList priorityList)
        {
            RomMatch romMatch = null;
            if (priorityList != null)
            {
                lock (priorityList.SyncRoot)
                    if (priorityList.Count > 0)
                    {
                        romMatch = (RomMatch)priorityList[0];
                        romMatch.CurrentThreadId = Thread.CurrentThread.ManagedThreadId;
                        priorityList.Remove(romMatch);
                    }
            }
            if (romMatch == null && list != null)
                lock (list.SyncRoot)
                    if (list.Count > 0)
                    {
                        romMatch = (RomMatch)list[0];
                        romMatch.CurrentThreadId = Thread.CurrentThread.ManagedThreadId;
                        list.Remove(romMatch);
                    }
            return romMatch;
        }

        void addToList(RomMatch romMatch, RomMatchStatus newStatus, ArrayList list, ArrayList priorityList)
        {
            lock (romMatch.SyncRoot)
            {
                if (!romMatch.OwnedByThread())
                    return;
                romMatch.CurrentThreadId = null;
                romMatch.Status = newStatus;

                if (priorityList != null && romMatch.HighPriority)
                    lock (priorityList.SyncRoot)
                        priorityList.Add(romMatch);
                else if (list != null)
                    lock (list.SyncRoot)
                        list.Add(romMatch);
                setRomStatus(romMatch, newStatus);
            }
        }

        bool checkAndWait(int interval, int count, bool checkPause)
        {
            if (interval < 1 || count < 1)
                return true;

            for (int x = 0; x < count; x++)
            {
                if (!doWork)
                    return false;
                if (checkPause && !pause)
                    return true;
                Thread.Sleep(interval);
            }

            return true;
        }
    }

    public enum ImportAction
    {
        ImportFinishing,
        ImportFinished,
        ImportStarting,
        ImportStarted,
        ImportStopping,
        ImportStopped,
        ImportRestarting,
        NoFilesFound,
        PendingFilesAdded,
        ImportPaused,
        ImportResumed,
        NewFilesFound,
        ImportRefreshing,
    }

    
}
