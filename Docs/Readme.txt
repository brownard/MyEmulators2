My Emulators plugin for Media Portal


Readme
===========================================

CHANGELOG
-------------------------------------------
v3.5 by BadMrFrosty

1. Games in your collection are now retrieved from the database instead of the file system. Useful for people who have roms on removeable disks / network drives that are not always connected.

2. More artwork. - Games can have box front and back covers, title and in-game screenshots and a fanart backdrop. (Supported skin required)

3. Ability to enter company, year, grade, description, logo and fanart for Emulators / systems
	This image has been resized. Click this bar to view the full image. The original image is sized 861x493.

4. Front cover / fanart for games will be displayed where available. If not the fanart / logo for the parent system / emulator will be displayed.

5. Added PC as a normal system to allow descriptions / art to be added in same way as other emulators.

6. Support for multiple roms within the same zip / 7zip / rar file AKA GoodMerge support.
-First go into the options section in myemulators config and set the path to your copy of 7z.dll. This should be in your 7zip installation directory. If you dont have 7-zip, download from 7-Zip
-Selecting a game with the play button will use the preferred rules as defined in the emulator config
-Selecting a game with the select/enter button will present a list of all ROMS stored in that games zip/7zip/rar file. You can then manually choose the ROM you want to run. The choice you make will be saved to the database and the next time you select that same game, it will automatically run your saved selection.
-Selecting a game with pause button will clear your previously saved ROM choice for that game and allow you to choose a new one as above.
-Games that only contain a single ROM will run automatically regardless of the button pressed.

7. PDF manuals can be added for systems and individual games. Press F8 while highlighting a item to open the manual in your default PDF viewer.

8. Fixed bug in facade update to load the next games information after scrolling in icon / filmstrip mode.

9. In the config utility removed the separate module for thumbnail upload. All artwork configuration is now done in the emulator / game config screens.

10. System filter added to games database

11. Online lookup - Retrieval of game information / artwork from MobyGames. This system uses the same scraper engine as moving pictures so any interested party can write a xml scraper script for other sites which can be easily integrated into the plugin.

12. Batch update games either manually or via online source. Batch update can be run on all games, only new games or only games with missing data / artwork.
	This image has been resized. Click this bar to view the full image. The original image is sized 896x696.

13. Update single games either via online source or local (click O or L button in the game list)

14. Doing online look-up will automatically select the game if only 1 search result is returned. Otherwise you will see a list of matches. Double click on the correct match to download and overwrite existing data if present. To select images, drag and drop the image from the box art / screenshots section on the left over to the box on the right. E.g. find a suitable ingame screenshot in the available images on the left, and drag and drop it into the ingame screenshot box on the right. Images can also be dragged and dropped in this way directly from web pages and from images stored on your hard drive. Press the V button to view the image full size, press X to delete it (nothing will be deleted until the save button is pressed)
	This image has been resized. Click this bar to view the full image. The original image is sized 923x594.

15. hulkhaugen's Streamedmp skin updated to use all of the new artwork, information and all views fixed


v3.2 Contributed by squega
-There is now an option within emulator
 configuration to toggle Mediaportal to
 automatically suspend itself and its 
 rendering while an executed ROM is running.
 This behavior now happens automatically 
 for PC games.
-The initial load of this plugin
 after opening Mediaportal or its
 configuration is no longer slow. 
  -A menu has been added to the 
   Game Database section named "Refresh Games 
   Database" menu in the configuration 
   that checks for any new roms within each 
   emulator's ROM directory. This functionality
   used to be called when the emulator
   initially loaded, causing the slowdown.
-A new menu has been added to the Configuration
 under the Game Database named "Backup/restore
 database" that will do a complete dump of all
 the information in the database of your 
 emulators and games (including PC games) to an
 XML file and allow you to restore it back to 
 the database.
-An option was added to the configuration 
 under the Options section that sets this 
 plugin to only display PC games.
-Pressing a number key to skip to a letter
 section in a list of games no longer runs
 the first game highlighted.
-Directories within ROM listings are now
 alphabetically sorted.

v3.1.1
-Changed how %ROM_WITHOUT_EXT% works: it is
 now replaced with the filename without the 
 path and extension

v3.1
-A new prefix, %ROM_WITHOUT_EXT%, that
 works like %ROM% but will not include
 the extension of the file
-New section in Configuration for setting
 thumbnails
-Bugfix regarding exporting game meta data
-Esc can be used to go back one level
 instead of just exiting
-Maximize box in Configuration window
-Bugfix in resizing method
-Configuration window resizable
-Blue3wide skin included

v3.0
-Built for Mediaportal 1.0
-Code completely rewritten
-New Configuration interface
-Improved translation system, see below
-Autoconfiguration of filter string
-Working folder for emulator can be set,
 and default value is the location of
 the emulator executable file
-Games can now be in subfolders
-More fields in the game database, among
 others one to hide the game
-Create a custom view by marking some
 games as favourites, and this view
 can be shown on launch
-Ctrl + F opens a search field in the
 database tab in the configuration
-Database optimization, so it should be
 a lot faster and easier to work with now
-Possibility to import and export games
 meta data through custom xml files
-PC Games have their own group and config

v2.0
-Filter for showing files made case-
 insensitive

v1.9
-Support for Windows Vista and MP RC1  
-Execute PC games, or other programs without
 arguments. See USAGE below for instructions.
-Support for using .bat files as emulators
-Added a column for genre, or type of game,
 which can be used to filter games via the
 context menu in MP. The translation files
 will have to be updated with two new
 strings, shown in the TRANSLATION section
 below.

v1.8
-A couple more skin files are now included
-More flexible passing of arguments, see USAGE
-Use delete key in Emulator list in setup

v1.7
-Built for MP 0.2.3.0
-Fixed bugs with paths to thumbs and data-
 base, fixing error with SQLite: CANTOPEN

v1.6
-Solved problem when returning from
 a launched emulator
-Added support for translations, see below
-The ROM images must now be placed in a
 subfolder of thumbs\myEmulators\games
 named as the emulator it belongs to

v1.5.1
-Fixed compability issues with MP

v1.5
-A ROM Database accessed from the 
 configuration that lets the user:
  -Change the name of each game
  -Write a description
  -Set a grade that is displayed in the
   plugin (can also be set from the
   context menu inside MP)
  -Change and view a play count and the 
   latest date the game has been launched.
-Buttons to sort the ROMs according to grade,
 number of times played or the latest date
 launched.
-Thumbnail support! After the first run,
 directories will be created in the thumbs-
 directory. Name the image exactly the same
 as the emulator or game to make it visible.
-An option to change the label of the plugin
-Switched to rar file format for the 
 distribution file, and included source code.
 Most of the comments are in Swedish though.
-Loads of GUI improvements to the setup...
-...and a much better looking skin...
-...with animations and different views.

v1.0.2
-Option to not use quotes on ROM paths,
 which should solve some compability
 issues
-All settings are now stored in a
 database (which means the plugin
 must be reconfigured)
-Tabbed configuration layout
-New header logo

v1.0.1
-Added an option to not close MediaPortal
 after launching a game

PURPOSE
-------------------------------------------
The plugin gives a nice overview of the 
installed emulators and games, and lets you 
start them with one click.

It also features a database function that
can hold information about how many times
a game has been played, a grade and more.

REQUIREMENTS
-------------------------------------------
The plugin has been build from and tested
to work with MediaPortal version 1.0,
but it may work just fine for older
versions too. No guarantee though.

INSTALLATION
-------------------------------------------
Extract all the files in the archive to
the folder where Mediaportal.exe is located.
Remember to keep the directory structure.

(Alternatively, you can extract only the
neccesary files: 
-myEmulators.dll to plugins\windows folder, 
-myEmulators.xml to current skin folder. 
-myEmulators_logo.png to the Media folder
in current skin folder
-hover_myEmulators.png to the same folder
-selected thumbnail files to 
the correct place (see below)

USAGE
--------------------------------------------
Before first use, enter Mediaportal configur-
ation and activate and setup the plugin. In
the setup, you should enter a path to the
emulator that is to load the ROMs, a folder
where all the ROMs are stored, and a name
and filter for which files will be shown.
When you run the plugin in Mediaportal you
will be presented with a list of the ROMs.
By clicking on a game, that one is loaded 
in the correct emulator.

Whenever something is changed in the setup,
a star in the title bar appears. After pres-
sing Apply, the game database will be updated.
This tab makes it easy to edit the name of
the games shown in MP as well as other
fields, such as a grade and description.

To pass argments to the emulator, just add
them in the Arguments to emulator field, ie
-switch1. To have switches after the executable,
add the widcard %ROM% where the path is to 
be inserted, ie -switch1 %ROM% -switch2. 
When a game is launched from the list, the 
wildcard will be replaced by the correct game 
file. If no %ROM% is defined, the path to the 
game file will be inserted at the end.

Make certain to disable Mediaportal always
on top in the configuration to be able to
launch the games correctly.

IMAGES
--------------------------------------------
Each emulator can have its own image. To show
that, it must be placed in the 
myEmulators\emulators folder in the
MediaPortal thumbs folder (currently in 
C:\Documents and Settings\All Users\
Application Data\Team MediaPortal\
MediaPortal\thumbs. The image should be named
exactly the same as the title of the
emulator. For example, to give the emulator
with title SNES an image, it would be placed
in C:\Documents and Settings\All Users\
Application Data\Team MediaPortal\MediaPortal\
thumbs\myEmulators\emulators\SNES.png

To show images of specific games,
create a subfolder in thumbs\myEmulators\
games named as the emulator and name the
image exactly the same as the game. For
example to give the game with title
Chrono Trigger under the SNES emulator
an image, it would be placed in C:\Documents
and Settings\All Users\Application Data\Team 
MediaPortal\MediaPortal\thumbs\myEmulators\
games\SNES\Chrono Trigger.png

If a subfolder contains an image named
folder.png, folder.jpg or folder.gif that
one will be used instead of the default
folder icon.

TRANSLATION
--------------------------------------------
To translate My Emulators into another lang-
uage, copy the default language file named
English.xml that is placed in the folder
named myEmulators in the language folder
of the Mediaportal installation. Translate
the text strings inside the file and give it
a descriptive file name. That file name will
then be visible from the configuration, and
it can be selected instead of the default.

Note that the setup is not translated, only
the buttons in the main program. If you want
to translate the title of the plugin, you
can do that from the plugin setup.

============================================

Chreekar
www.carlsund.se
chrille110@hotmail.com