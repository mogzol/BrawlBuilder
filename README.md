# BrawlBuilder
The Ultimate Super Smash Bros Brawl ISO Builder

## About
This is a tool to build modded SSBB ISOs. It currently works with many Brawl mods, although there are some issues. All versions of Project M should run fine, including 3.6 and above, with full support for sfx .sawnd files and alternate stages. Most Project M mods should work as well. Psycho Ghost's 3.6 build has been tested and seems to work fine. Legacy M, however, does not work at this time.

Other Brawl mods may or may not work, they are widely untested. It seems like anything using BrawlEx, however, does not work at this time, so all Brawl- versions beyond 2.x.6 will not work.

Hopefully compatibility will be improved in the future.

If you want to discuss BrawlBuilder, or if you have any issues, check out the official BrawlBuilder thread over on Smashboards: http://smashboards.com/threads/432675

## Downloads
BrawlBuilder can be downloaded from the releases page: https://github.com/mogzol/BrawlBuilder/releases

Just grab the latest zip, extract it, and then run BrawlBuilder.exe. The app should launch and you should be good to go.

## Command line arguments
`--show-wit`
This will show the wit command line windows rather than hiding them and displaying the progress in the UI.

`--show-wit-debug`
This will do the same thing as `--show-wit`, but it will keep the wit windows open once wit closes, allowing you to read the output and figure out what is going wrong.

`--no-gct-patch`
This will disable patching of the GCT based on the CodePatches.txt file in the Resources folder. Use of this option is not recommended.

`--notify-gct-patch`
This will notify you of how many GCT patches were applied from the CodePatches.txt file.

`--no-alt-pad`
This will disable alternate stage padding. Alternate stages are padded to the size of their largest alt. Disabling this will cause any alternate stage that has a file size greater than its non-alt version to stop working.

`--offset=X`
This will change the offset that the GCT is applied at (by default 80570000) to whatever X is. Setting this to 80568000 will allow Legacy M to progress a bit further than it normally does before crashing. If your mod isn't working check its gameconfig.txt for the codeliststart line, and try using the offset listed there. Note that Resources/patch/PatchCommon.xml also patches some stuff to 8057 and 0000, I don't know if thats related, but you could try editing that to match your codeliststart as well and see if it helps.

## Notes
- BrawlBuilder attempts to patch any GCT files you give to it in order to fix numerous problems caused by loading files from disc instead of SD. The original GCT won't be modified, patches are applied to a temporary copy of it. GCT files are patched based on the CodePatches.txt file in the Resources folder. That file currently contains patches allowing for alternate stage compatibility, replacement soundbank engine compatibility, and P:M 3.6 support in general. Feel free to modify the CodePatches file with your own patches, there is a brief description of how to use it in the comments at the top of the file.

- If you are experiencing issues when playing the output ISO, make sure that you either don't have an SD card inserted, or your SD card doesn't have any mod files on it, as most mods (depending on the GCT patches that get applied) will still try to load from SD before loading from disc. In Dolphin, to eject the SD card just go in to the 'Config' menu, then go to the 'Wii' tab and untick 'Insert SD Card'.

- If you are building this from source, make sure the Resources folder from the base directory is getting copied into the same directory as the exe file, or else nothing will work. This should happen automatically on build, but if it doesn't, just copy it manually.

## Credits
- First of all, thanks to Wiimm for his amazing wit tool, which is used to extract, patch, and build the game images.
- Secondly, thanks to the Dolphin team for the awesome debug mode of Dolphin, I wouldn't have been able to get alternate stages and the replacement soundbank engine working without that.
- The folder select dialog uses Ookii dialogs, a great little library for nice looking dialogs.
- Detailed wit error messages make use of FlexibleMessageBox by JReichert
- And thanks to the creators of all the ISO builder scripts for Project M and the like, they are what I initially based this off of.
