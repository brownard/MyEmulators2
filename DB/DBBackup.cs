using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SQLite.NET;
using MediaPortal.Database;
using System.IO;

namespace MyEmulators2
{
    internal delegate void BackupDataErrorHandler(DataErrorType errorType, string message);
    internal delegate void BackupProgressHandler(int perc, int currentItem, int totalItems, string message, params object[] args);

    /// <summary>
    /// Provides methods for backing up or restoring the database
    /// </summary>
    class DBBackup
    {
        public event BackupDataErrorHandler OnBackupDataError; //error event
        public event BackupProgressHandler OnBackupProgress; //progress event

        int currentItem = 0;
        int totalItems = 0;

        string backupDirectory = null;

        bool clean = false;
        string thumbDir = null;

        bool backupThumbs = true;
        public bool BackupThumbs
        {
            get { return backupThumbs; }
            set { backupThumbs = value; }
        }

        bool restoreThumbs = true;
        public bool RestoreThumbs
        {
            get { return restoreThumbs; }
            set { restoreThumbs = value; }
        }

        //this is checked after an 'invalid data' event is raised
        //to determine whether to just skip current item or stop restore completely
        bool? shouldContinue = null;
        public bool? ShouldContinue
        {
            get { return shouldContinue; }
            set { shouldContinue = value; }
        }

        #region Backup Methods

        /// <summary>
        /// Backup the database to the specified save path
        /// </summary>
        /// <param name="savePath">The path where the backup xml will be created</param>
        public void Backup(string savePath)
        {
            backupDirectory = null;
            try
            {
                backupDirectory = System.IO.Path.GetDirectoryName(savePath);
                if (!Directory.Exists(backupDirectory))
                    backupDirectory = null;
            }
            catch
            {
                backupDirectory = null;
            }

            if (backupDirectory == null)
            {
                //error locating save path
                if (OnBackupDataError != null)
                    OnBackupDataError(DataErrorType.SaveFile, string.Format("Unable to locate specified save directory '{0}'.", backupDirectory));
                return;
            }

            if (backupThumbs)
            {
                backupDirectory += string.Format(@"\{0}_Thumbs", Path.GetFileNameWithoutExtension(savePath));
                if (!Directory.Exists(backupDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(backupDirectory);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Unable to create backup thumb directory '{0}' - {1}", backupDirectory, ex.Message);
                        if (OnBackupDataError != null)
                            OnBackupDataError(DataErrorType.SaveFile, string.Format("Unable to create backup thumb directory '{0}'.", backupDirectory));
                        return;
                    }
                }
            }

            //create xml doc
            XmlDocument doc = new XmlDocument();
            //add declaration
            XmlNode headNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(headNode);
            //add top node
            XmlNode docNode = doc.CreateElement("backup");
            doc.AppendChild(docNode);
            //add db version info
            XmlNode dbVersion = doc.CreateElement("version");
            XmlAttribute attr = doc.CreateAttribute("value");
            attr.Value = DB.Instance.CurrentDBVersion.ToString(System.Globalization.CultureInfo.InvariantCulture);
            dbVersion.Attributes.Append(attr);
            docNode.AppendChild(dbVersion);
            
            createNodes(doc, docNode, DataType.Emulator, 0); //add emu nodes
            copyThumbs(backupDirectory, "-1", DataType.Emulator); //copy pcEmu thumbs

            createNodes(doc, docNode, DataType.EmulatorProfile, 20); //add profile nodes
            createNodes(doc, docNode, DataType.Game, 40); //add game nodes
            createNodes(doc, docNode, DataType.PCGameProfile, 60);
            createNodes(doc, docNode, DataType.GameDisc, 80);

            if (OnBackupProgress != null)
                OnBackupProgress(100, 3, 3, "Saving...");
            try
            {
                doc.Save(savePath);
            }
            catch (Exception ex) //error saving doc
            {
                if (OnBackupDataError != null)
                    OnBackupDataError(DataErrorType.SaveFile, ex.Message);
            }
            if (OnBackupProgress != null)
                OnBackupProgress(100, 0, 0, "");
        }

        /// <summary>
        /// Creates xml nodes from database entries based on the specified DataType
        /// and appends them to the specified docNode
        /// </summary>
        /// <param name="doc">The parent xml documnet</param>
        /// <param name="docNode">The node to append the newly created items</param>
        /// <param name="type"></param>
        void createNodes(XmlDocument doc, XmlNode docNode, DataType type, int startPerc)
        {
            XmlNode parent;
            SQLiteResultSet result;
            string friendlyName = "";
            string idColumn = null;
            //create parent node and get db data based on specified datatype
            switch (type)
            {
                case DataType.Emulator:
                    friendlyName = "Emulators";
                    idColumn = "uid";
                    parent = doc.CreateElement("emulators");
                    result = DB.Instance.Execute("SELECT * FROM {0}", Emulator.TABLE_NAME); //get all emulators
                    break;
                case DataType.EmulatorProfile:
                    friendlyName = "Emulator Profiles";
                    parent = doc.CreateElement("emulatorprofiles");
                    result = DB.Instance.Execute("SELECT * FROM {0} WHERE emulator_id != -1", EmulatorProfile.TABLE_NAME); //get all profiles
                    break;
                case DataType.Game:
                    friendlyName = "Games";
                    idColumn = "gameid";
                    parent = doc.CreateElement("games");
                    result = DB.Instance.Execute("SELECT * FROM {0}", Game.TABLE_NAME); //get all games
                    break;
                case DataType.PCGameProfile:
                    friendlyName = "Game Profiles";
                    parent = doc.CreateElement("gameprofiles");
                    result = DB.Instance.Execute("SELECT * FROM {0} WHERE emulator_id = -1", EmulatorProfile.TABLE_NAME);
                    break;
                case DataType.GameDisc:
                    friendlyName = "Discs";
                    parent = doc.CreateElement("discs");
                    result = DB.Instance.Execute("SELECT * FROM {0}", GameDisc.TABLE_NAME);
                    break;
                default:
                    return;
            }
            
            //append parent to docNode
            docNode.AppendChild(parent);

            string progStr = string.Format("Saving {0}...", friendlyName);
            int total = result.Rows.Count;
            if (total < 1)
                return;

            double dPerc = startPerc;
            double increment = 20.0 / total;

            if (OnBackupProgress != null)
                OnBackupProgress(startPerc, 0, total, progStr);

            //loop through results
            for (int x = 0; x < result.Rows.Count; x++)
            {
                if (OnBackupProgress != null)
                {
                    OnBackupProgress((int)dPerc, x + 1, total, progStr);
                    dPerc += increment;
                }

                //create item node
                XmlNode item = doc.CreateElement("item");

                bool foundId = idColumn == null ? true : false;
                string id = null;

                //loop through each column
                foreach (string column in result.ColumnNames)
                {
                    //create column node
                    XmlNode columnNode = doc.CreateElement(column);

                    //add column value attribute
                    XmlAttribute attr = doc.CreateAttribute("value");
                    string columnValue = DatabaseUtility.Get(result, x, column);
                    attr.Value = columnValue;

                    if (!foundId && column == idColumn)
                    {
                        id = columnValue;
                        foundId = true;
                    }

                    columnNode.Attributes.Append(attr);

                    //append column node to item node
                    item.AppendChild(columnNode);
                }

                //append item to parent
                parent.AppendChild(item);
                if (backupThumbs && id != null)
                    copyThumbs(backupDirectory, id, type);
            }
        }

        void copyThumbs(string saveDir, string id, DataType dataType)
        {
            if (dataType != DataType.Emulator && dataType != DataType.Game)
                return;

            string dirName;
            if (dataType == DataType.Emulator)
                dirName = ThumbGroup.EMULATOR_DIR_NAME;
            else
                dirName = ThumbGroup.GAME_DIR_NAME;

            string currDir = string.Format(@"{0}\{1}\{2}\{3}", Emulators2Settings.Instance.ThumbDirectory, ThumbGroup.THUMB_DIR_NAME, dirName, id);
            if (!Directory.Exists(currDir))
                return;

            string newDir = string.Format(@"{0}\{1}\{2}", saveDir, dirName, id);
            if (!Directory.Exists(newDir))
            {
                try
                {
                    Directory.CreateDirectory(newDir);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error creating backup thumb directory '{0}' - {1}", newDir, ex.Message);
                    return;
                }
            }

            foreach(string file in Directory.GetFiles(currDir))
            {
                try
                {
                    File.Copy(file, Path.Combine(newDir, Path.GetFileName(file)), true);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error copying thumb '{0}' to backup directory '{1}' - {2}", file, newDir, ex.Message);
                }
            }

        }

        #endregion

        #region Restore Methods

        //used to map old and new id's
        Dictionary<string, string> updatedEmuIds = null;
        List<string> newEmuIds = null;
        Dictionary<string, string> updatedProfileIds = null;
        Dictionary<string, string> updatedGameIds = null;
        Dictionary<string, string> newGameIds = null;
        List<string> gameIdsNeedProfileUpdate = null;

        /// <summary>
        /// Restore the database from the specified xml file using the specified merge settings
        /// </summary>
        /// <param name="xmlPath">The path to the xml file containing backup data</param>
        /// <param name="emuMergeType">The merge setting to apply if an emulator with the same name already exists</param>
        /// <param name="profileMergeType">The merge setting to apply if a profile with the same name and belonging to the same emulator already exists</param>
        /// <param name="gameMergeType">The merge setting to apply if a game with the same path already exists</param>
        /// <param name="clean">Whether to perform a clean restore by deleting all existing db data/thumbs</param>
        public void Restore(string xmlPath, MergeType emuMergeType, MergeType profileMergeType, MergeType gameMergeType, bool clean)
        {
            if (!System.IO.File.Exists(xmlPath)) //error locating specified file
            {
                if (OnBackupDataError != null)
                    OnBackupDataError(DataErrorType.LoadFile, "Unable to locate specified backup file.");
                return;
            }

            //create xml from file
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlPath);
            }
            catch (Exception ex)
            {
                //error creating xml
                if (OnBackupDataError != null)
                    OnBackupDataError(DataErrorType.LoadFile, ex.Message);
                Logger.LogError("Error loading specified backup - {0}", ex.Message);
                return;
            }

            XmlNodeList versions = doc.GetElementsByTagName("version");
            double version;
            if (versions.Count < 1 || versions[0].Attributes["value"] == null || !double.TryParse(versions[0].Attributes["value"].Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out version))
            {
                if (OnBackupDataError != null)
                    OnBackupDataError(DataErrorType.InvalidVersion, "No version info found for backup");
                return;
            }
            if (version < 1.2)
            {
                if (OnBackupDataError != null)
                    OnBackupDataError(DataErrorType.InvalidVersion, "Backups from database versions lower than 1.2 are not supported");
                return;
            }
            else if (version > DB.Instance.CurrentDBVersion)
            {
                if (OnBackupDataError != null)
                    OnBackupDataError(DataErrorType.InvalidVersion, "This backup is from a newer version of Emulators 2 and is not supported");
                return;
            }

            if (restoreThumbs)
            {
                thumbDir = Path.GetDirectoryName(xmlPath) + string.Format(@"\{0}_Thumbs", Path.GetFileNameWithoutExtension(xmlPath));
                if (!Directory.Exists(thumbDir))
                {
                    Logger.LogError("Unable to locate thumb backup folder '{0}'", thumbDir);
                    thumbDir = null;
                }
            }

            this.clean = clean;

            //initialise id dictionaries
            updatedEmuIds = new Dictionary<string, string>();
            newEmuIds = new List<string>();
            updatedProfileIds = new Dictionary<string, string>();
            updatedGameIds = new Dictionary<string, string>();
            newGameIds = new Dictionary<string, string>();
            gameIdsNeedProfileUpdate = new List<string>();

            totalItems = 0;

            //get total number of items to process for progress reporting
            XmlNodeList emuNodes = doc.GetElementsByTagName("emulators");
            if (emuNodes.Count > 0)
                totalItems += emuNodes[0].ChildNodes.Count;
            XmlNodeList profileNodes = doc.GetElementsByTagName("emulatorprofiles");
            if (profileNodes.Count > 0)
                totalItems += profileNodes[0].ChildNodes.Count;
            XmlNodeList gamesNodes = doc.GetElementsByTagName("games");
            if (gamesNodes.Count > 0)
                totalItems += gamesNodes[0].ChildNodes.Count;
            XmlNodeList gameProfileNodes = doc.GetElementsByTagName("gameprofiles");
            if (gameProfileNodes.Count > 0)
                totalItems += gameProfileNodes[0].ChildNodes.Count;
            XmlNodeList discNodes = doc.GetElementsByTagName("discs");
            if (discNodes.Count > 0)
                totalItems += discNodes[0].ChildNodes.Count;

            currentItem = 0;

            //must update tables in order emulators -> emulatorprofiles -> games,
            //as profiles need to know changes to emu ids and games need to know
            //changes to emu AND profile ids
            if (emuNodes.Count > 0)
                mergeTable(emuNodes[0].ChildNodes, DataType.Emulator, emuMergeType);
            if (clean || emuMergeType == (MergeType.Merge | MergeType.Create))
            {
                updateThumbs("-1", "-1", DataType.Emulator, emuMergeType == MergeType.Merge);
            }

            if (profileNodes.Count > 0)
                mergeTable(profileNodes[0].ChildNodes, DataType.EmulatorProfile, profileMergeType);

            if (gamesNodes.Count > 0)
                mergeTable(gamesNodes[0].ChildNodes, DataType.Game, gameMergeType);

            if (gameProfileNodes.Count > 0)
            {
                mergeTable(gameProfileNodes[0].ChildNodes, DataType.PCGameProfile, MergeType.Create);
                foreach (string gameId in gameIdsNeedProfileUpdate)
                {
                    SQLiteResultSet result = DB.Instance.Execute("SELECT emuprofile FROM {0} WHERE gameid={1}", Game.TABLE_NAME, gameId);
                    if (result.Rows.Count > 0)
                    {
                        string oldProfileId = result.Rows[0].fields[0];
                        if (updatedProfileIds.ContainsKey(oldProfileId))
                        {
                            Logger.LogDebug("DB Backup: Updating game {0} with new profile id {1}", gameId, updatedProfileIds[oldProfileId]);
                            DB.Instance.Execute("UPDATE {0} SET emuprofile={1} WHERE gameid={2}", Game.TABLE_NAME, updatedProfileIds[oldProfileId], gameId);
                        }
                    }
                }
            }

            if (discNodes.Count > 0)
                mergeTable(discNodes[0].ChildNodes, DataType.GameDisc, MergeType.Create);

            if (OnBackupProgress != null)
                OnBackupProgress(100, 0, 0, "");
        }

        /// <summary>
        /// Adds data from the specified xml items to the database using the specified merge setting
        /// </summary>
        /// <param name="items"></param>
        /// <param name="dataType"></param>
        void mergeTable(XmlNodeList items, DataType dataType, MergeType mergeType)
        {
            //don't allow MergeType.Merge for profiles as there cannot be missing data
            if (dataType == DataType.EmulatorProfile)
            {
                if (mergeType == MergeType.Merge)
                {
                    Logger.LogDebug("MergeType.Merge is not allowed for Emulator Profiles, using MergeType.Ignore instead");
                    mergeType = MergeType.Ignore;
                }
            }
            //don't allow MergeType.Create for games as shouldn't have 2 games with same path
            else if (dataType == DataType.Game)
            {
                if (mergeType == MergeType.Create)
                {
                    Logger.LogDebug("MergeType.Create is not allowed for Games, using MergeType.Ignore instead");
                    mergeType = MergeType.Ignore;
                }
            }

            //table to update
            string tableName;
            //the id column of the table, if specified the corresponding backup data will be
            //updated to the next available id
            string idColumnStr = null;
            //any column that is always required e.g 'path' in games table
            string requiredColumnStr = null;
            //the column that contains the id of a profile/game's parent emulator. If specified
            //this will be updated to account for any change of emulator ids when restoring
            string emuColumnStr = null;
            //same as emuColumn but for profile ids
            string profileColumnStr = null;
            string gameColumnStr = null;
            //any column that should be inserted as NULL e.g 'game_id' in games table 
            string nullColumnStr = null;
            //the column that will be checked for an existing entry in the database,
            //if an entry is found action is taken based on the specified MergeType
            string mergeColumnStr = null;
            string isDefaultColumnStr = null;

            //init column names based on specified DataType
            switch (dataType)
            {
                case DataType.Emulator:
                    tableName = Emulator.TABLE_NAME;
                    idColumnStr = "uid";
                    mergeColumnStr = "title";
                    break;
                case DataType.EmulatorProfile:
                    tableName = EmulatorProfile.TABLE_NAME;
                    idColumnStr = "uid";
                    emuColumnStr = "emulator_id";
                    mergeColumnStr = "title";
                    isDefaultColumnStr = "defaultprofile";
                    break;
                case DataType.Game:
                    tableName = Game.TABLE_NAME;
                    emuColumnStr = "parentemu";
                    profileColumnStr = "emuprofile";
                    requiredColumnStr = "path";
                    idColumnStr = "gameid";
                    nullColumnStr = "gameid";
                    mergeColumnStr = "path";
                    break;
                case DataType.GameDisc:
                    tableName = GameDisc.TABLE_NAME;
                    idColumnStr = "uid";
                    gameColumnStr = "gameid";
                    mergeType = MergeType.Create;
                    break;
                case DataType.PCGameProfile:
                    tableName = EmulatorProfile.TABLE_NAME;
                    idColumnStr = "uid";
                    gameColumnStr = "game_id";
                    mergeType = MergeType.Create;
                    break;
                default:
                    return;
            }

            lock (DB.Instance.SyncRoot) //Lock the DB
            {
                if (clean && dataType != DataType.PCGameProfile) //if clean restore
                    cleanTable(dataType, tableName); //delete all existing data in table and any associated thumbs

                //get list of current columns in specified table
                List<ColumnInfo> columns = getColumns(tableName);

                //loop through each xml item
                foreach (XmlNode item in items)
                {
                    currentItem++;

                    //build a list of each childNode's name and value
                    Dictionary<string, string> itemInfo = new Dictionary<string, string>();
                    try
                    {
                        foreach (XmlNode val in item.ChildNodes)
                            if (!itemInfo.ContainsKey(val.Name))
                                itemInfo.Add(val.Name, val.Attributes["value"].Value);
                    }
                    catch (Exception ex)
                    {
                        //error with backup data, report error and wait for user to specify action
                        shouldContinue = null;

                        if (OnBackupDataError != null)
                            OnBackupDataError(DataErrorType.InvalidData, ex.Message);

                        while (shouldContinue == null)
                        {
                            System.Threading.Thread.Sleep(200);
                        }

                        if (shouldContinue == true) //user selected skip
                            continue;
                        else //user selected quit
                            return;
                    }

                    //if a required column has been specified
                    if (requiredColumnStr != null)
                    {
                        //and the corresponding value is not present in the backup, skip
                        if (!itemInfo.ContainsKey(requiredColumnStr) || string.IsNullOrEmpty(itemInfo[requiredColumnStr]))
                        {
                            Logger.LogError("DB Backup: Skipping backup item, required column '{0}' was not present", requiredColumnStr);
                            continue;
                        }
                    }

                    if (gameColumnStr != null)
                    {
                        if (itemInfo.ContainsKey(gameColumnStr))
                        {
                            string id = itemInfo[gameColumnStr];
                            if (newGameIds.ContainsKey(id))
                            {
                                if (dataType == DataType.PCGameProfile && !gameIdsNeedProfileUpdate.Contains(newGameIds[id]))
                                    gameIdsNeedProfileUpdate.Add(newGameIds[id]);
                            }
                            else
                            {
                                Logger.LogInfo("DB Backup: Skipping game item, game has been merged or ignored");
                                continue;
                            }
                        }
                    }

                    int x = 0;
                                        
                    ColumnInfo mergeColumn = null;
                    ColumnInfo idColumn = null;
                    ColumnInfo emuColumn = null;
                    ColumnInfo isDefaultColumn = null;
                    string newId = null;
                    if (idColumnStr != null)
                    {
                        //update value to next available id in table
                        newId = getNextID(tableName, idColumnStr);
                    }

                    string oldId = null;

                    //add values to columns from backup, if backup data not present default values will be used
                    foreach (ColumnInfo column in columns)
                    {
                        string colVal = null;
                        //see if we have data matching column name
                        if (itemInfo.ContainsKey(column.Name))
                            colVal = itemInfo[column.Name];

                        //if we're the merge column, keep reference
                        if (mergeColumnStr != null && mergeColumnStr == column.Name)
                            mergeColumn = column;

                        //if it's specified id column
                        if (idColumnStr != null && idColumnStr == column.Name)
                        {
                            column.SQLValue = newId; //add to sql

                            //keep a record of old and new id so all references in
                            //the backup data can be updated
                            idColumn = column;
                            if (colVal != null)
                            {
                                if (dataType == DataType.Emulator)
                                {
                                    if (!updatedEmuIds.ContainsKey(colVal))
                                        updatedEmuIds.Add(colVal, newId);
                                }
                                else if (dataType == DataType.EmulatorProfile || dataType == DataType.PCGameProfile)
                                {
                                    if (!updatedProfileIds.ContainsKey(colVal))
                                        updatedProfileIds.Add(colVal, newId);
                                }
                                else if (dataType == DataType.Game)
                                {
                                    if (!updatedGameIds.ContainsKey(colVal))
                                        updatedGameIds.Add(colVal, newId);
                                }
                                //keep old id as will be needed when merging
                                oldId = colVal;
                            }
                        }

                        //else if it's specified emuColumn
                        else if (emuColumnStr != null && emuColumnStr == column.Name)
                        {
                            //check to see if value needs updating due to change in emu id's
                            if (updatedEmuIds.ContainsKey(colVal))
                                colVal = updatedEmuIds[colVal];
                            column.SQLValue = colVal;
                            emuColumn = column;
                        }

                        else if (profileColumnStr != null && profileColumnStr == column.Name)
                        {
                            //account for changes in profile id's
                            if (updatedProfileIds.ContainsKey(colVal))
                                colVal = updatedProfileIds[colVal];
                            column.SQLValue = colVal;
                        }

                        else if (gameColumnStr != null && gameColumnStr == column.Name)
                        {
                            if (updatedGameIds.ContainsKey(colVal))
                                colVal = updatedGameIds[colVal];
                            column.SQLValue = colVal;
                        }
                        else if (isDefaultColumnStr != null && isDefaultColumnStr == column.Name)
                        {
                            isDefaultColumn = column;
                            column.SQLValue = colVal;
                        }
                        //else just add data
                        else
                            column.SQLValue = colVal;

                        //if current column is specified nullColumn
                        if (nullColumnStr != null && nullColumnStr == column.Name)
                        {
                            column.SQLValue = "NULL";
                        }

                        x++;
                    }
                    
                    try
                    {
                        //if we're not set to create and we've found a merge column,
                        //see if there's a matching entry in the database
                        if (mergeType != MergeType.Create && mergeColumn != null && idColumn != null)
                        {
                            //get the name of the column to select from the database
                            string colSelect = idColumn.Name;

                            //build sql select statement using column name and backup value
                            string mergeSql = string.Format("{0}={1}", mergeColumn.Name, mergeColumn.SQLValue);
                            if (dataType == DataType.EmulatorProfile && emuColumn != null)
                                mergeSql += string.Format(" AND {0}={1}", emuColumn.Name, emuColumn.SQLValue); //if emu profile, ensure parent emu id's also match

                            //get entries matching backup value
                            SQLiteResultSet mergeResult = DB.Instance.ExecuteWithoutLock("SELECT {0} FROM {1} WHERE {2}", colSelect, tableName, mergeSql);
                            if (mergeResult.Rows.Count > 0)
                            {
                                string uid = mergeResult.Rows[0].fields[0];
                                if (dataType == DataType.Emulator)
                                    updatedEmuIds[oldId] = uid; //update change of id to existing db entry's id
                                else if (dataType == DataType.EmulatorProfile)
                                    updatedProfileIds[oldId] = uid;
                                else if (dataType == DataType.Game)
                                    updatedGameIds[oldId] = uid;
                                newId = uid;

                                //if we want to merge
                                if (mergeType == MergeType.Merge)
                                {
                                    if (OnBackupProgress != null)
                                        OnBackupProgress(getPerc(), currentItem, totalItems, "Merging {0}", mergeColumn.SQLValue);

                                    //merge data
                                    mergeData(tableName, colSelect, uid, columns, dataType);
                                    if (restoreThumbs && oldId != null && newId != null)
                                        updateThumbs(oldId, newId, dataType, true);
                                }
                                else //else ignore backup item
                                {
                                    if (OnBackupProgress != null)
                                        OnBackupProgress(getPerc(), currentItem, totalItems, "Ignoring {0}", mergeColumn.SQLValue);
                                }

                                continue;
                            }
                        }

                        //If we've made it here we're either set to Create or no entry was
                        //found in the DB matching the backup's merge column

                        if (dataType == DataType.Game && oldId != null)
                            newGameIds[oldId] = updatedGameIds[oldId];
                        else if (dataType == DataType.Emulator && !newEmuIds.Contains(newId))
                            newEmuIds.Add(newId);
                        else if (dataType == DataType.EmulatorProfile && isDefaultColumn != null && emuColumn != null && !newEmuIds.Contains(emuColumn.SQLValue))
                            isDefaultColumn.SQLValue = "False";
                        
                        //build sql insert statement
                        string sql = "";
                        for (int y = 0; y < columns.Count; y++)
                        {
                            sql += columns[y].SQLValue;
                            //if not last entry add ','
                            if (y != columns.Count - 1)
                                sql += ", ";
                        }

                        string message;
                        if (mergeColumn != null)
                            message = mergeColumn.SQLValue;
                        else
                            message = currentItem.ToString();

                        if (OnBackupProgress != null)
                            OnBackupProgress(getPerc(), currentItem, totalItems, "Adding {0}", message);

                        //insert new entry into table
                        DB.Instance.ExecuteWithoutLock("INSERT INTO {0} VALUES({1})", tableName, sql);
                        if (restoreThumbs && oldId != null && newId != null)
                            updateThumbs(oldId, newId, dataType, false);
                    }
                    catch (Exception ex)
                    {
                        shouldContinue = null;

                        if (OnBackupDataError != null)
                            OnBackupDataError(DataErrorType.SQLError, ex.Message);

                        while (shouldContinue == null)
                        {
                            System.Threading.Thread.Sleep(200);
                        }

                        if (shouldContinue == true)
                            continue;
                        else
                            return;
                    }
                }
            }
        }

        /// <summary>
        /// Selects the database entry with the specified uid, if the entry is missing mergable data it is
        /// updated with mergable data from the corresponding backup entry.
        /// </summary>
        /// <param name="tableName">The name of the table to select the entry from</param>
        /// <param name="colSelect">The name of the uid column. This should be 'uid' for Emulators/Profiles or 'path' for Games</param>
        /// <param name="uid">The uid value of the database entry to merge</param>
        /// <param name="backupData">The list of backup columns and their backup values</param>
        /// <param name="dataType">Whether the entry is an Emulator, EmulatorProfile or Game (used to get mergable columns)</param>
        void mergeData(string tableName, string colSelect, string uid, List<ColumnInfo> backupData, DataType dataType)
        {
            //get existing db entry
            SQLiteResultSet result = DB.Instance.Execute("SELECT * FROM {0} WHERE {1} = {2}", tableName, colSelect, uid);
            //entry should be unique
            if (result.Rows.Count != 1)
                return;

            //get all existing db fields
            List<string> dbData = result.Rows[0].fields;
            //db fields and backup fields length should match
            if (dbData.Count != backupData.Count)
                return;

            //get list of mergable columns and their default values
            Dictionary<string, string> mergableColumns = getMergableColumns(dataType);
            //create list to store any columns that need updating
            List<ColumnInfo> mergedColumns = new List<ColumnInfo>();

            //loop through each column in the current db entry
            for (int x = 0; x < dbData.Count; x++)
            {
                string data = dbData[x]; //existing db data
                ColumnInfo column = backupData[x]; //backup data

                //if we're a mergable column and the existing db value is the column default value
                if (mergableColumns.ContainsKey(column.Name) && data == mergableColumns[column.Name] && data != column.columnValue)
                    //add backup data to list of columns to merge
                    mergedColumns.Add(column);
            }

            //nothing to update
            if (mergedColumns.Count == 0)
                return;

            //build sql update statement
            string sql = "";
            for (int x = 0; x < mergedColumns.Count; x++)
            {
                ColumnInfo column = mergedColumns[x];
                //unique column shouldn't be in mergable columns, but just in case
                if (column.Name == colSelect)
                    continue;

                sql += string.Format("{0}={1}", column.Name, column.SQLValue);

                //add comma if we're not the last column
                if (x != mergedColumns.Count - 1)
                    sql += ", ";
            }

            sql = string.Format("UPDATE {0} SET {1} WHERE {2}={3}", tableName, sql, colSelect, uid);
            try
            {
                DB.Instance.Execute(sql);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error merging backup data - {0}", ex.Message);
            }
        }

        void updateThumbs(string oldId, string newId, DataType dataType, bool merge)
        {
            if (thumbDir == null || (dataType != DataType.Emulator && dataType != DataType.Game))
                return;

            string dirName;
            if (dataType == DataType.Emulator)
                dirName = ThumbGroup.EMULATOR_DIR_NAME;
            else
                dirName = ThumbGroup.GAME_DIR_NAME;

            string currDir = string.Format(@"{0}\{1}\{2}", thumbDir, dirName, oldId);
            if (!Directory.Exists(currDir))
                return;

            string newDir = string.Format(@"{0}\{1}\{2}\{3}", Emulators2Settings.Instance.ThumbDirectory, ThumbGroup.THUMB_DIR_NAME, dirName, newId);
            if (!Directory.Exists(newDir))
            {
                try
                {
                    Directory.CreateDirectory(newDir);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Unable to create thumb directory '{0}' - {1}", newDir, ex.Message);
                    return;
                }
            }

            foreach (string file in Directory.GetFiles(currDir))
            {
                if (isThumb(file) && (!merge || !thumbExists(newDir, Path.GetFileNameWithoutExtension(file))))
                {
                    try
                    {
                        string savePath = newDir + "\\" + Path.GetFileName(file);
                        File.Copy(file, savePath, true);
                        ThumbGroup.RemoveAlternateThumb(savePath);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Unable to copy thumb to new directory '{0}' - {1}", newDir, ex.Message);
                        continue;
                    }
                }
            }

        }

        string logoName = ThumbGroup.LOGO_NAME.ToLower();
        string boxFrontName = ThumbGroup.BOX_FRONT_NAME.ToLower();
        string boxBackName = ThumbGroup.BOX_BACK_NAME.ToLower();
        string titleName = ThumbGroup.TITLESCREEN_NAME.ToLower();
        string ingameName = ThumbGroup.INGAME_NAME.ToLower();
        string fanartName = ThumbGroup.FANART_NAME.ToLower();
        string manualName = ThumbGroup.MANUAL_NAME.ToLower();

        bool isThumb(string path)
        {
            path = path.ToLower();
            string filename = Path.GetFileNameWithoutExtension(path);

            if (filename != logoName &&
                filename != boxFrontName &&
                filename != boxBackName &&
                filename != titleName &&
                filename != ingameName &&
                filename != fanartName &&
                filename != manualName)
                return false;
            
            if (filename != ThumbGroup.MANUAL_NAME)
            {
                if (!path.EndsWith(".jpg") && !path.EndsWith(".png"))
                    return false;
            }
            else if (!path.EndsWith("pdf"))
                return false;

            return true;    
        }

        bool thumbExists(string directory, string filename)
        {
            string newPath = directory + "\\" + filename;
            if (filename != ThumbGroup.MANUAL_NAME)
            {
                if (File.Exists(newPath + ".jpg") || File.Exists(newPath + ".png"))
                    return true;
            }
            else if (File.Exists(newPath + ".pdf"))
                return true;

            return false;
        }

        //get next available id in table
        string getNextID(string tableName, string idColumn)
        {
            if (tableName == Game.TABLE_NAME)
            {
                if (MediaPortal.Database.DatabaseUtility.GetAsInt(DB.Instance.ExecuteWithoutLock("SELECT COUNT(gameid) FROM {0}", Game.TABLE_NAME), 0, 0) > 0)
                    return MediaPortal.Database.DatabaseUtility.GetAsInt(DB.Instance.Execute("SELECT seq + 1 FROM sqlite_sequence WHERE name='{0}'", Game.TABLE_NAME), 0, 0).ToString();

                DB.Instance.Execute("DELETE FROM sqlite_sequence WHERE name='{0}'", Game.TABLE_NAME);
                return "1";
            }

            SQLiteResultSet result = DB.Instance.Execute("SELECT MAX({0})+1 FROM {1}", idColumn, tableName);
            if (result.Rows.Count > 0)
            {
                string id = result.Rows[0].fields[0];
                if (!string.IsNullOrEmpty(id))
                    return id;
            }
            
            return "0";
        }

        //get list of columns in current database and their corresponding data type
        List<ColumnInfo> getColumns(string tableName)
        {
            List<ColumnInfo> columns = new List<ColumnInfo>();
            //get table info
            SQLiteResultSet result = DB.Instance.Execute("PRAGMA table_info({0})", tableName);
            foreach (SQLiteResultSet.Row row in result.Rows)
            {
                columns.Add(new ColumnInfo(row.fields[0], row.fields[1], row.fields[2])); //index, name, type
            }

            return columns;
        }

        /// <summary>
        /// Gets a list of mergable columns for the specified datatype and their corresponding
        /// default values.
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        Dictionary<string, string> getMergableColumns(DataType dataType)
        {
            //Profiles have no mergable columns
            if (dataType == DataType.EmulatorProfile)
                return new Dictionary<string, string>();

            Dictionary<string, string> columns = new Dictionary<string, string>();

            if (dataType == DataType.Emulator)
            {
                columns.Add("Grade", "0");
                columns.Add("Company", "");
                columns.Add("Yearmade", "0");
                columns.Add("Description", "");
                columns.Add("videopreview", "");
            }
            else if (dataType == DataType.Game)
            {
                columns.Add("grade", "0");
                columns.Add("yearmade", "0");
                columns.Add("latestplay", DateTime.MinValue.ToString());
                columns.Add("description", "");
                columns.Add("genre", "");
                columns.Add("company", "");
                columns.Add("favourite", "False");
                columns.Add("videopreview", "");
            }

            return columns;
        }

        //removes all existing data from the specified table and any associated thumbs
        void cleanTable(DataType dataType, string tableName)
        {
            if (OnBackupProgress != null)
                OnBackupProgress(getPerc(), currentItem, totalItems, "Deleting data from {0}...", tableName);

            DB.Instance.Execute("DELETE FROM {0}", tableName); //delete db data

            if (dataType == DataType.EmulatorProfile)
                return;

            //get associated thumb directory
            string delPath = null;
            if (dataType == DataType.Emulator)
                delPath = ThumbGroup.EMULATOR_DIR_NAME;
            else if (dataType == DataType.Game)
                delPath = ThumbGroup.GAME_DIR_NAME;

            if (delPath != null)
            {
                if (OnBackupProgress != null)
                    OnBackupProgress(getPerc(), currentItem, totalItems, "Removing associated thumbs...");

                delPath = string.Format("{0}\\{1}\\{2}", Emulators2Settings.Instance.ThumbDirectory, ThumbGroup.THUMB_DIR_NAME, delPath);
                try
                {
                    System.IO.Directory.Delete(delPath, true); //delete entire directory
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error deleting thumb directory '{0}' - {1}", delPath, ex.Message);
                }
            }
        }

        int getPerc()
        {
            int perc = (int)Math.Round(((double)currentItem / totalItems) * 100);
            if (perc > 100)
                perc = 100;
            else if (perc < 0)
                perc = 0;

            return perc;
        }

        #endregion
    }

    enum DataType
    {
        Emulator,
        EmulatorProfile,
        Game,
        GameDisc,
        PCGameProfile
    }

    enum MergeType
    {
        Create,
        Ignore,
        Merge
    }

    enum DataErrorType
    {
        LoadFile,
        InvalidData,
        SQLError,
        SaveFile,
        InvalidVersion
    }
}
