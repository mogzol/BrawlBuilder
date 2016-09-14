------------------------------------/
------- BRAWLBUILDER v1.2.2 -------/
----------------------------------/

"The Ultimate Super Smash Bros. Brawl ISO Builder"

Author: Mogzol (aka person66)
Source: https://github.com/mogzol/BrawlBuilder


----------------/
------ About --/
--------------/

This is a tool to build modded SSBB ISOs. It currently works with many Brawl mods, although there are some issues.
All versions of Project M should run fine, including 3.6 and above, with full support for sfx .sawnd files and
alternate stages. Most Project M mods should work as well. Psycho Ghost's 3.6 build has been tested and seems to
work fine. Legacy M, however, does not work at this time.

Other Brawl mods may or may not work, they are widely untested. It seems like anything using BrawlEx, however,
does not work at this time, so all Brawl- versions beyond 2.x.6 will not work.

Hopefully compatibility will be improved in the future.


---------------------------------/
------ Command Line Arguments --/
-------------------------------/

There are a couple command line arguments if you want to use them, I will probably add more in the future.

'--show-wit'
This will show the wit command line windows rather than hiding them and displaying the progress in the UI.

'--show-wit-debug'
This will do the same thing as --show-wit, but it will keep the wit windows open once wit closes, allowing you to
read the output and figure out what is going wrong.

'--no-gct-patch'
This will disable patching of the GCT based on the CodePatches.txt file in the Resources folder. Use of this
option is not recommended.

'--notify-gct-patch'
This will notify you of how many GCT patches were applied from the CodePatches.txt file.

'--no-alt-pad'
This will disable alternate stage padding. Alternate stages are padded to the size of their largest alt. Disabling
this will cause any alternate stage that has a file size greater than its non-alt version to stop working.

'--offset=X'
This will change the offset that the GCT is applied at (by default 80570000) to whatever X is. Setting this to
80568000 will allow Legacy M to progress a bit further than it normally does before crashing. If your mod isn't
working check its gameconfig.txt for the codeliststart line, and try using the offset listed there. Note that
Resources/patch/PatchCommon.xml also patches some stuff to 8057 and 0000, I don't know if thats related, but you
could try editing that to match your codeliststart as well and see if it helps.


----------------/
------ Notes --/
--------------/

Note #1:
--------
BrawlBuilder attempts to patch any GCT files you give to it in order to fix numerous problems caused by loading
files from disc instead of SD. The original GCT won't be modified, patches are applied to a temporary copy of it.
GCT files are patched based on the CodePatches.txt file in the Resources folder. That file currently contains
patches allowing for alternate stage compatibility, replacement soundbank engine compatibility, and P:M 3.6
support in general. Feel free to modify the CodePatches file with your own patches, there is a brief description
of how to use it in the comments at the top of the file.

Note #2:
--------
If you are experiencing issues when playing the output ISO, make sure that you either don't have an SD card
inserted, or your SD card doesn't have any mod files on it, as most mods (depending on the GCT patches that get
applied) will still try to load from SD before loading from disc. In Dolphin, to eject the SD card just go in to
the 'Config' menu, then go to the 'Wii' tab and untick 'Insert SD Card'.

Note #3:
--------
If you are building this from source, make sure the Resources folder from the base directory is getting copied
into the same directory as the exe file, or else nothing will work. This should happen automatically on build, but
if it doesn't, just copy it manually.


--------------------/
------ Changelog --/
------------------/
1.2.2:
 - Fix custom working directories not working (again)

1.2.1:
 - Remove the BrawlBuilder.exe.config file
 - Fix 'Custom banner' browse button appearance for certain Windows versions/themes

1.2:
 - Fix some stages glitching/crashing due to improper .rel files
 
1.1.2:
 - Fix BrawlBuilder not working with custom working directories

1.1.1:
 - Fix 'wit exited with error code -1' happening with almost every build for some people
 - Add 'Finalizing...' status at end of build so it doesn't seem like build is frozen at 99%
 
1.1:
 - Improve error messages, most of them should now actually be somewhat helpful
 - Fix "Build Completed" sometimes appearing even if the build did not complete properly
 - Add the --show-wit-debug option to show wit windows and keep them open after wit closes
 - Modify the help text for the Brawl ISO to clarify how the ssbb.d folder works
 - Fix crash if building with no GCT selected
 - Other code cleanup and misc. fixes

1.0:
 - Initial release


------------------/
------ Credits --/
----------------/

- First of all, thanks to Wiimm for his amazing wit tool, which is used to extract, patch, and build the game
  images.

- Secondly, thanks to the Dolphin team for the awesome debug mode of Dolphin, I wouldn't have been able to get
  alternate stages and the replacement soundbank engine working without that.

- The folder select dialog uses Ookii dialogs, a great little library for nice looking dialogs.

- Detailed wit error messages make use of FlexibleMessageBox by JReichert

- And thanks to the creators of all the ISO builder scripts for Project M and the like, they are what I initially
  based this off of.
