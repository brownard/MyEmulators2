using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace CommunityServerWindowsService
{
    /// <summary>
    /// Class is the heart of the server. It holds all the logic for selecting the right details for a rom, and also stores submitted details. Topologically it exists between the database and the web interface.
    /// </summary>
    public class Engine
    {

        /// <summary>
        /// Client send the hash and the filename of a rom, and the server returns a gamedetails object containing the most selected details for that game.
        /// It takes both a hash and the filename so that it can resort to using the filename if no hash is found.
        /// </summary>
        /// <param name="hash">The hash of the file provided by the client.</param>
        /// <param name="filename">The filename of the file provided by the client.</param>
        /// <returns>A gamedetails object filled with the most likely info availible for the given rom hash or filename. It may come back mostly empty if no info was found.</returns>
        public GameDetails GameRequested(GameDetails gameDetails)
        {
            Service.Log.Info("Game Details Requested");

            HashMatch hashMatch = null;
            FilenameMatch filenameMatch = null;
            Game DBGame = null;
            Filename DBFilename = null;
            Hash DBHash = null;
            ISession session = Service.SessionFactory.OpenSession();

            DBHash = session.CreateCriteria(typeof(Hash))
                .Add(Restrictions.Eq("hash", gameDetails.Hash))
                .UniqueResult<Hash>();

            if (DBHash == null)
            {
                Service.Log.Info("No matching hash, trying filename");

                DBFilename = session.CreateCriteria(typeof(Filename))
                    .Add(Restrictions.Eq("filename", gameDetails.Filename))
                    .UniqueResult<Filename>();

                if (DBFilename != null)
                {
                    Service.Log.Info("Filename found");

                    // Select the hashMatch. If none found, return a empty game object.
                    if (DBFilename.filenameMatches[0] != null)
                    {
                        // Sort the list by how many uses each match has.
                        // This essentially puts the most assoicated game with a filename on top.
                        DBFilename.filenameMatches.OrderBy(x => x.count);
                        filenameMatch = DBFilename.filenameMatches[0];

                        // Grab the game.
                        DBGame = filenameMatch.game;
                    }
                    else
                    {
                        session.Close();
                        return gameDetails;
                    }
                }
                else
                {
                    Service.Log.Info("Filename not found, returning nothing");
                    session.Close();
                    return gameDetails;
                }


            }
            else
            {
                Service.Log.Info("Hash found");

                // Sort the list by how many uses each match has.
                // This essentially puts the most assoicated game with a hash on top.
                DBHash.hashMatches.OrderBy(x => x.count);

                // Select the hashMatch. If none found, return a empty game object.
                if (DBHash.hashMatches[0] != null)
                    hashMatch = DBHash.hashMatches[0];
                else
                    return gameDetails;

                // Grab the game.
                DBGame = hashMatch.game;
            }

            session.Refresh(DBGame);
            gameDetails = GetMostLikelyGame(DBGame);

            session.Close();

            return gameDetails;
        }


        /// <summary>
        /// Client submits a gamedetails object to the server. The server then checks each piece of game info to see if it already exists. If it
        /// doesnt the server creates it and assoicates it to the given game. If it does exist, it checks if its assoicated with the given game. If not
        /// it assoicates it, if it does, it increases the usage count for it.
        /// </summary>
        /// <param name="gameDetails">The details of the game being submitted.</param>
        /// <param name="hash">Hash of the rom file for the game being submitted.</param>
        /// <param name="filename">Filename of the rom file for the game being submitted.</param>
        public int GameSubmitted(GameDetails gameDetails)
        {
            // FIX ME: use the unique result option of the createCriteria function for getting results.

            Service.Log.Info("Game Details submitted");

            ISession session = Service.SessionFactory.OpenSession();

            if (gameDetails.Hash != null)
            {
                Service.Log.Info("Hash submitted with game");

                // Try hash first. If that doesnt come up with a result, try using the filename.
                // If that doesnt work, try the title. If all these dont work, make a new game record.

                // There can only be one result
                var hashResult = session.CreateCriteria<Hash>()
                    .Add(Restrictions.Eq("hash", gameDetails.Hash))
                    .List<Hash>();

                if (hashResult.Count > 0)
                {
                    Service.Log.Info("Found a matching hash");

                    IList<HashMatch> matches = hashResult[0].hashMatches;

                    if (matches != null)
                    {
                        // Sort the list by how many uses each match has.
                        // This essentially puts the most assoicated game with a hash on top.
                        matches.OrderBy(x => x.count);

                        if (matches.Count > 0)
                        {
                            // Grab the game.
                            Game gameResult = matches[0].game;

                            if (gameResult != null)
                                NewGameDetails(gameResult, gameDetails, session);
                        }
                    }
                }
                else // No hash result, try filename.
                {
                    Service.Log.Info("No hash found, trying filename");

                    // There can only be one result
                    var filenameResult = session.CreateCriteria<Filename>()
                        .Add(Restrictions.Eq("filename", gameDetails.Filename))
                        .List<Filename>();

                    if (filenameResult.Count > 0)
                    {
                        Service.Log.Info("Filename found");

                        if (filenameResult[0].filenameMatches.Count > 0)
                        {
                            // Sort the list by how many uses each match has.
                            // This essentially puts the most assoicated game with a filename on top.
                            filenameResult[0].filenameMatches.OrderBy(x => x.count);

                            Game gameResult = filenameResult[0].filenameMatches[0].game;

                            if (gameResult != null)
                                NewGameDetails(gameResult, gameDetails, session);
                        }
                    }
                    else // No Filename result ether. Try name.
                    {
                        Service.Log.Info("Filename not found, trying title");

                        var titleResult = session.CreateCriteria<Title>()
                            .Add(Restrictions.Eq("title", gameDetails.Title))
                            .List<Title>();

                        if (titleResult.Count > 0)
                        {
                            Service.Log.Info("Title found");

                            if (titleResult[0].titleMatches.Count > 0)
                            {
                                // Sort the list by how many uses each match has.
                                // This essentially puts the most assoicated game with a title on top.
                                titleResult[0].titleMatches.OrderBy(x => x.count);

                                Game gameResult = titleResult[0].titleMatches[0].game;

                                if (gameResult != null)
                                    NewGameDetails(gameResult, gameDetails, session);
                            }
                        }
                        else // No hash, filename, or title result. Must be a new game for the community server.
                        {
                            Service.Log.Info("No title found. This must be a new game");
                            UnknownGame(gameDetails, session);
                        }
                    }
                }
            }
            session.Close();

            // TODO change this to return the ID of the transaction. The idea is to have usage count of records reduced if the client doesnt use them anymore.
            return 1;
        }

        /// <summary>
        /// Takes new game details for a game we already have in the DB.
        /// It correctly puts the details in the right tables and increases counter when needed.
        /// </summary>
        /// <param name="DBgame">The object representing the game in the DB</param>
        /// <param name="newGameDetails">The new details for the game to be put into the DB</param>
        /// /// <param name="session">The session to the Database</param>
        void NewGameDetails(Game DBgame, GameDetails newGameDetails, ISession session)
        {
            Service.Log.Info("Adding new details to existing game");
            bool found = false;
            // Check Title
            if (!string.IsNullOrEmpty(newGameDetails.Title))
            {
                found = false;
                foreach (TitleMatch titleMatch in DBgame.titleMatches)
                {
                    if (titleMatch.title.title == newGameDetails.Title)
                    {
                        titleMatch.count++;
                        session.Update(titleMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    Title title = new Title();
                    title.title = newGameDetails.Title;
                    session.Save(title);

                    TitleMatch titleMatch = new TitleMatch();
                    titleMatch.title = title;
                    titleMatch.game = DBgame;
                    session.Save(titleMatch);
                }
            }

            // Check Year
            if (!string.IsNullOrEmpty(newGameDetails.YearMade))
            {
                found = false;
                foreach (YearMatch yearMatch in DBgame.yearMatches)
                {
                    if (yearMatch.year.year == newGameDetails.YearMade)
                    {
                        yearMatch.count++;
                        session.Update(yearMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    Year year = new Year();
                    year.year = newGameDetails.YearMade;
                    session.Save(year);

                    YearMatch yearMatch = new YearMatch();
                    yearMatch.year = year;
                    yearMatch.game = DBgame;
                    session.Save(yearMatch);
                }
            }

            // check filename
            if (!string.IsNullOrEmpty(newGameDetails.Filename))
            {
                found = false;
                foreach (FilenameMatch filenameMatch in DBgame.filenameMatches)
                {
                    if (filenameMatch.filename.filename == newGameDetails.Filename)
                    {
                        filenameMatch.count++;
                        session.Update(filenameMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    Filename newFilename = new Filename();
                    newFilename.filename = newGameDetails.Filename;
                    session.Save(newFilename);

                    FilenameMatch filenameMatch = new FilenameMatch();
                    filenameMatch.filename = newFilename;
                    filenameMatch.game = DBgame;
                    session.Save(filenameMatch);
                }
            }

            // check genre
            if (!string.IsNullOrEmpty(newGameDetails.Genre))
            {
                found = false;
                foreach (GenreMatch genreMatch in DBgame.genreMatches)
                {
                    if (genreMatch.genre.genre == newGameDetails.Genre)
                    {
                        genreMatch.count++;
                        session.Update(genreMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    Genre genre = new Genre();
                    genre.genre = newGameDetails.Genre;
                    session.Save(genre);

                    GenreMatch genreMatch = new GenreMatch();
                    genreMatch.genre = genre;
                    genreMatch.game = DBgame;
                    session.Save(genreMatch);
                }
            }

            //check Grade
            if (!string.IsNullOrEmpty(newGameDetails.Grade))
            {
                found = false;
                foreach (GradeMatch gradeMatch in DBgame.gradeMatches)
                {
                    if (gradeMatch.grade.grade == newGameDetails.Grade)
                    {
                        gradeMatch.count++;
                        session.Update(gradeMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    Grade grade = new Grade();
                    grade.grade = newGameDetails.Grade;
                    session.Save(grade);

                    GradeMatch gradeMatch = new GradeMatch();
                    gradeMatch.grade = grade;
                    gradeMatch.game = DBgame;
                    session.Save(gradeMatch);
                }
            }

            //check Description
            if (!string.IsNullOrEmpty(newGameDetails.Description))
            {
                found = false;
                foreach (DescriptionMatch descriptionMatch in DBgame.descriptionMatches)
                {
                    if (descriptionMatch.description.description == newGameDetails.Description)
                    {
                        descriptionMatch.count++;
                        session.Update(descriptionMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    Description description = new Description();
                    description.description = newGameDetails.Description;
                    session.Save(description);

                    DescriptionMatch descriptionMatch = new DescriptionMatch();
                    descriptionMatch.description = description;
                    descriptionMatch.game = DBgame;
                    session.Save(descriptionMatch);
                }
            }

            // check hash
            if (!string.IsNullOrEmpty(newGameDetails.Hash))
            {
                found = false;
                foreach (HashMatch hashMatch in DBgame.hashMatches)
                {
                    if (hashMatch.hash.hash == newGameDetails.Hash)
                    {
                        hashMatch.count++;
                        session.Update(hashMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    Hash newHash = new Hash();
                    newHash.hash = newGameDetails.Hash;
                    session.Save(newHash);

                    HashMatch hashMatch = new HashMatch();
                    hashMatch.hash = newHash;
                    hashMatch.game = DBgame;
                    session.Save(hashMatch);
                }
            }

            // check manual
            if (!string.IsNullOrEmpty(newGameDetails.Manual))
            {
                found = false;
                foreach (ManualMatch manualMatch in DBgame.manualMatches)
                {
                    if (manualMatch.manual.manual == newGameDetails.Manual)
                    {
                        manualMatch.count++;
                        session.Update(manualMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    Manual manual = new Manual();
                    manual.manual = newGameDetails.Manual;
                    session.Save(manual);

                    ManualMatch manualMatch = new ManualMatch();
                    manualMatch.manual = manual;
                    manualMatch.game = DBgame;
                    session.Save(manualMatch);
                }
            }
            
            // check ImageBack
            if (!string.IsNullOrEmpty(newGameDetails.ImageBack))
            {
                found = false;
                foreach (ImageBackMatch ImageBackMatch in DBgame.ImageBackMatches)
                {
                    if (ImageBackMatch.ImageBack.url == newGameDetails.ImageBack)
                    {
                        ImageBackMatch.count++;
                        session.Update(ImageBackMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    ImageBack ImageBack = new ImageBack();
                    ImageBack.url = newGameDetails.ImageBack;
                    session.Save(ImageBack);

                    ImageBackMatch ImageBackMatch = new ImageBackMatch();
                    ImageBackMatch.ImageBack = ImageBack;
                    ImageBackMatch.game = DBgame;
                    session.Save(ImageBackMatch);
                }
            }

            // check ImageFront
            if (!string.IsNullOrEmpty(newGameDetails.ImageFront))
            {
                found = false;
                foreach (ImageFrontMatch ImageFrontMatch in DBgame.ImageFrontMatches)
                {
                    if (ImageFrontMatch.ImageFront.url == newGameDetails.ImageFront)
                    {
                        ImageFrontMatch.count++;
                        session.Update(ImageFrontMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    ImageFront ImageFront = new ImageFront();
                    ImageFront.url = newGameDetails.ImageFront;
                    session.Save(ImageFront);

                    ImageFrontMatch ImageFrontMatch = new ImageFrontMatch();
                    ImageFrontMatch.ImageFront = ImageFront;
                    ImageFrontMatch.game = DBgame;
                    session.Save(ImageFrontMatch);
                }
            }

            // check ImageFanart
            if (!string.IsNullOrEmpty(newGameDetails.ImageFanart))
            {
                found = false;
                foreach (ImageFanartMatch ImageFanartMatch in DBgame.ImageFanartMatches)
                {
                    if (ImageFanartMatch.ImageFanart.url == newGameDetails.ImageFanart)
                    {
                        ImageFanartMatch.count++;
                        session.Update(ImageFanartMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    ImageFanart ImageFanart = new ImageFanart();
                    ImageFanart.url = newGameDetails.ImageFanart;
                    session.Save(ImageFanart);

                    ImageFanartMatch ImageFanartMatch = new ImageFanartMatch();
                    ImageFanartMatch.ImageFanart = ImageFanart;
                    ImageFanartMatch.game = DBgame;
                    session.Save(ImageFanartMatch);
                }
            }

            //check ImageIngame
            if (!string.IsNullOrEmpty(newGameDetails.ImageIngame))
            {
                found = false;
                foreach (ImageIngameMatch ImageIngameMatch in DBgame.ImageIngameMatches)
                {
                    if (ImageIngameMatch.ImageIngame.url == newGameDetails.ImageIngame)
                    {
                        ImageIngameMatch.count++;
                        session.Update(ImageIngameMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    ImageIngame ImageIngame = new ImageIngame();
                    ImageIngame.url = newGameDetails.ImageIngame;
                    session.Save(ImageIngame);

                    ImageIngameMatch ImageIngameMatch = new ImageIngameMatch();
                    ImageIngameMatch.ImageIngame = ImageIngame;
                    ImageIngameMatch.game = DBgame;
                    session.Save(ImageIngameMatch);
                }
            }

            // check ImageTitleScreen
            if (!string.IsNullOrEmpty(newGameDetails.ImageTitleScreen))
            {
                found = false;
                foreach (ImageTitleScreenMatch ImageTitleScreenMatch in DBgame.ImageTitleScreenMatches)
                {
                    if (ImageTitleScreenMatch.ImageTitleScreen.url == newGameDetails.ImageTitleScreen)
                    {
                        ImageTitleScreenMatch.count++;
                        session.Update(ImageTitleScreenMatch);
                        found = true;
                    }
                }
                if (!found)
                {
                    ImageTitleScreen ImageTitleScreen = new ImageTitleScreen();
                    ImageTitleScreen.url = newGameDetails.ImageTitleScreen;
                    session.Save(ImageTitleScreen);

                    ImageTitleScreenMatch ImageTitleScreenMatch = new ImageTitleScreenMatch();
                    ImageTitleScreenMatch.ImageTitleScreen = ImageTitleScreen;
                    ImageTitleScreenMatch.game = DBgame;
                    session.Save(ImageTitleScreenMatch);
                }
            }

            session.Flush();
        }

        /// <summary>
        /// Creates a whole new game record in the DB. Used when no hash, filename, or title match was found in the DB.
        /// </summary>
        /// <param name="gameDetails">Details of the unknown game.</param>
        /// <param name="hash">Hash of the unknown game.</param>
        /// <param name="filename">Filename of the unknown game.</param>
        void UnknownGame(GameDetails gameDetails, ISession session)
        {
            Service.Log.Info("Creating completely new game records");

            // create new details. We know that a new game, hash, filename, title and their match tables need to be created.
            // the other details might already have records (assigned to another game in error possibly)
            Game game = new Game();
            session.Save(game);

            Hash newHash = new Hash();
            newHash.hash = gameDetails.Hash;
            session.Save(newHash);

            HashMatch hashMatch = new HashMatch();
            hashMatch.hash = newHash;
            hashMatch.game = game;
            session.Save(hashMatch);

            Filename newFilename = new Filename();
            newFilename.filename = gameDetails.Filename;
            session.Save(newFilename);

            FilenameMatch filenameMatch = new FilenameMatch();
            filenameMatch.filename = newFilename;
            filenameMatch.game = game;
            session.Save(filenameMatch);

            Title newTitle = new Title();
            newTitle.title = gameDetails.Title;
            session.Save(newTitle);

            TitleMatch titleMatch = new TitleMatch();
            titleMatch.title = newTitle;
            titleMatch.game = game;
            session.Save(titleMatch);

            // now add the other game details to our new game.
            // we check if there is an instance of each game detail first, and if there is
            // we create a match to our new game. If there isnt an instance of the game detail
            // already in our db, then we create one and match that to our game.

            // Year
            Service.Log.Info("Checking if year already exists in DB");
            // There can only be one result
            var yearResult = session.CreateCriteria<Year>()
                .Add(Restrictions.Eq("year", gameDetails.YearMade))
                .List<Year>();

            YearMatch yearMatch = new YearMatch();
            Year year;
            if (yearResult.Count > 0) // an instance was found
            {
                Service.Log.Info("Year already existed in DB");
                year = yearResult[0];
            }
            else // no instance was found
            {
                Service.Log.Info("Year did not exist in DB. Creating year");
                year = new Year();
                year.year = gameDetails.YearMade;
                session.Save(year);
            }

            Service.Log.Info("Creating match for year to game");
            yearMatch.game = game;
            yearMatch.year = year;
            session.Save(yearMatch);

            //Manual
            var manualResult = session.CreateCriteria<Manual>()
                .Add(Restrictions.Eq("manual", gameDetails.Manual))
                .List<Manual>();

            ManualMatch manualMatch = new ManualMatch();
            Manual manual;
            if (manualResult.Count > 0) // an instance in the db was found
                manual = manualResult[0];
            else
            {
                manual = new Manual();
                manual.manual = gameDetails.Manual;
                session.Save(manual);
            }

            manualMatch.game = game;
            manualMatch.manual = manual;
            session.Save(manualMatch);

            //Genre
            var genreResult = session.CreateCriteria<Genre>()
                .Add(Restrictions.Eq("genre", gameDetails.Genre))
                .List<Genre>();

            GenreMatch genreMatch = new GenreMatch();
            Genre genre;
            if (genreResult.Count > 0) // an instance in the db was found
                genre = genreResult[0];
            else
            {
                genre = new Genre();
                genre.genre = gameDetails.Genre;
                session.Save(genre);
            }

            genreMatch.game = game;
            genreMatch.genre = genre;
            session.Save(genreMatch);

            //Grade
            var gradeResult = session.CreateCriteria<Grade>()
                .Add(Restrictions.Eq("grade", gameDetails.Grade))
                .List<Grade>();

            GradeMatch gradeMatch = new GradeMatch();
            Grade grade;
            if (gradeResult.Count > 0) // an instance in the db was found
                grade = gradeResult[0];
            else
            {
                grade = new Grade();
                grade.grade = gameDetails.Grade;
                session.Save(grade);
            }

            gradeMatch.game = game;
            gradeMatch.grade = grade;
            session.Save(gradeMatch);

            // Description
            var descriptionResult = session.CreateCriteria<Description>()
                .Add(Restrictions.Eq("description", gameDetails.Description))
                .List<Description>();

            DescriptionMatch descriptionMatch = new DescriptionMatch();
            Description description;
            if (descriptionResult.Count > 0) // an instance in the db was found
                description = descriptionResult[0];
            else
            {
                description = new Description();
                description.description = gameDetails.Description;
                session.Save(description);
            }

            descriptionMatch.game = game;
            descriptionMatch.description = description;
            session.Save(descriptionMatch);

            //ImageBack
            var ImageBackResult = session.CreateCriteria<ImageBack>()
                .Add(Restrictions.Eq("url", gameDetails.ImageBack))
                .List<ImageBack>();

            ImageBackMatch ImageBackMatch = new ImageBackMatch();
            ImageBack ImageBack;
            if (ImageBackResult.Count > 0) // an instance in the db was found
                ImageBack = ImageBackResult[0];
            else
            {
                ImageBack = new ImageBack();
                ImageBack.url = gameDetails.ImageBack;
                session.Save(ImageBack);
            }

            ImageBackMatch.game = game;
            ImageBackMatch.ImageBack = ImageBack;
            session.Save(ImageBackMatch);

            //ImageFront
            var ImageFrontResult = session.CreateCriteria<ImageFront>()
                .Add(Restrictions.Eq("url", gameDetails.ImageFront))
                .List<ImageFront>();

            ImageFrontMatch ImageFrontMatch = new ImageFrontMatch();
            ImageFront ImageFront;
            if (ImageFrontResult.Count > 0) // an instance in the db was found
                ImageFront = ImageFrontResult[0];
            else
            {
                ImageFront = new ImageFront();
                ImageFront.url = gameDetails.ImageFront;
                session.Save(ImageFront);
            }

            ImageFrontMatch.game = game;
            ImageFrontMatch.ImageFront = ImageFront;
            session.Save(ImageFrontMatch);

            //ImageTitleScreen
            var ImageTitleScreenResult = session.CreateCriteria<ImageTitleScreen>()
                .Add(Restrictions.Eq("url", gameDetails.ImageTitleScreen))
                .List<ImageTitleScreen>();

            ImageTitleScreenMatch ImageTitleScreenMatch = new ImageTitleScreenMatch();
            ImageTitleScreen ImageTitleScreen;
            if (ImageTitleScreenResult.Count > 0) // an instance in the db was found
                ImageTitleScreen = ImageTitleScreenResult[0];
            else
            {
                ImageTitleScreen = new ImageTitleScreen();
                ImageTitleScreen.url = gameDetails.ImageTitleScreen;
                session.Save(ImageTitleScreen);
            }

            ImageTitleScreenMatch.game = game;
            ImageTitleScreenMatch.ImageTitleScreen = ImageTitleScreen;
            session.Save(ImageTitleScreenMatch);

            //ImageIngame
            var ImageIngameResult = session.CreateCriteria<ImageIngame>()
                .Add(Restrictions.Eq("url", gameDetails.ImageIngame))
                .List<ImageIngame>();

            ImageIngameMatch ImageIngameMatch = new ImageIngameMatch();
            ImageIngame ImageIngame;
            if (ImageIngameResult.Count > 0) // an instance in the db was found
                ImageIngame = ImageIngameResult[0];
            else
            {
                ImageIngame = new ImageIngame();
                ImageIngame.url = gameDetails.ImageIngame;
                session.Save(ImageIngame);
            }

            ImageIngameMatch.game = game;
            ImageIngameMatch.ImageIngame = ImageIngame;
            session.Save(ImageIngameMatch);

            //ImageFanart
            var ImageFanartResult = session.CreateCriteria<ImageFanart>()
                .Add(Restrictions.Eq("url", gameDetails.ImageFanart))
                .List<ImageFanart>();

            ImageFanartMatch ImageFanartMatch = new ImageFanartMatch();
            ImageFanart ImageFanart;
            if (ImageFanartResult.Count > 0) // an instance in the db was found
                ImageFanart = ImageFanartResult[0];
            else
            {
                ImageFanart = new ImageFanart();
                ImageFanart.url = gameDetails.ImageFanart;
                session.Save(ImageFanart);
            }

            ImageFanartMatch.game = game;
            ImageFanartMatch.ImageFanart = ImageFanart;
            session.Save(ImageFanartMatch);


            session.Flush();
        }

        GameDetails GetMostLikelyGame(Game game)
        {


            GameDetails likelyGame = new GameDetails();

            // Get most likely data

            // Genre
            IEnumerable<GenreMatch> Genres = game.genreMatches.OrderByDescending(x => x.count);
            if (Genres.Count() != 0)
                likelyGame.Genre = Genres.ElementAt(0).genre.genre;

            // Grade
            IEnumerable<GradeMatch> Grades = game.gradeMatches.OrderByDescending(x => x.count);
            if (Grades.Count() != 0)
                likelyGame.Grade = Grades.ElementAt(0).grade.grade;

            // Description
            IEnumerable<DescriptionMatch> Descriptions = game.descriptionMatches.OrderByDescending(x => x.count);
            if (Descriptions.Count() != 0)
                likelyGame.Description = Descriptions.ElementAt(0).description.description;

            // Back cover image
            IEnumerable<ImageBackMatch> ImageBacks = game.ImageBackMatches.OrderByDescending(x => x.count);
            if (ImageBacks.Count() != 0)
                likelyGame.ImageBack = ImageBacks.ElementAt(0).ImageBack.url;

            // Front cover image
            IEnumerable<ImageFrontMatch> ImageFronts = game.ImageFrontMatches.OrderByDescending(x => x.count);
            if (ImageFronts.Count() != 0)
                likelyGame.ImageFront = ImageFronts.ElementAt(0).ImageFront.url;

            // Ingame image
            IEnumerable<ImageIngameMatch> ImageIngames = game.ImageIngameMatches.OrderByDescending(x => x.count);
            if (ImageIngames.Count() != 0)
                likelyGame.ImageIngame = ImageIngames.ElementAt(0).ImageIngame.url;

            // Title screen image
            IEnumerable<ImageTitleScreenMatch> ImageTitleScreens = game.ImageTitleScreenMatches.OrderByDescending(x => x.count);
            if (ImageTitleScreens.Count() != 0)
                likelyGame.ImageTitleScreen = ImageTitleScreens.ElementAt(0).ImageTitleScreen.url;

            // Fanart
            IEnumerable<ImageFanartMatch> ImageFanarts = game.ImageFanartMatches.OrderByDescending(x => x.count);
            if (ImageFanarts.Count() != 0)
                likelyGame.ImageFanart = ImageFanarts.ElementAt(0).ImageFanart.url;

            // manual
            IEnumerable<ManualMatch> Manuals = game.manualMatches.OrderByDescending(x => x.count);
            if (Manuals.Count() != 0)
                likelyGame.Manual = Manuals.ElementAt(0).manual.manual;

            // title
            IEnumerable<TitleMatch> Titles = game.titleMatches.OrderByDescending(x => x.count);
            if (Titles.Count() != 0)
                likelyGame.Title = Titles.ElementAt(0).title.title;

            // year released
            IEnumerable<YearMatch> Years = game.yearMatches.OrderByDescending(x => x.count);
            if (Years.Count() != 0)
                likelyGame.YearMade = Years.ElementAt(0).year.year;

            return likelyGame;
        }
    }
}
