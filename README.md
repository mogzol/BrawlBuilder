# BrawlBuilder
The Ultimate Super Smash Bros Brawl ISO Builder

## About
This is a tool to build modded SSBB ISOs. As far as I know, it should work with pretty much any Brawl mod, the only ones that may cause issues are Project M 3.6 and above. Currently, vanilla P:M 3.6 works, including alternate stages, as do many 3.6-based mods. 3.61 Community Complete also works, if you're into that sort of thing. Legacy M 1.21 does not currently work, and I assume that some of the other more complex 3.6-based mods also won't work.

## Command line arguments
There are a couple command line arguments if you want to use them, I will probably add more in the future.

`--show-wit` Will show wit command line windows rather than hiding them and displaying the progress in the UI

`--offset=X` Will change the offset that the GCT is applied at (by default 80570000) to whatever X is. Setting this to 80568000 will allow Legacy M to progress a bit further than it normally does before crashing. If your mod isn't working check its gameconfig.txt for the codeliststart line, and try using the offset listed there. Note that Resources/patch/PatchCommon.xml also patches some stuff to 8057 and 0000, I don't know if thats related, but you could try editing that to match your codeliststart as well and see if it helps.

## Notes on Project M 3.6
Project M 3.6 support is achieved by through slightly modifying the given GCT. If the builder detects that you are using a 3.6+ GCT file, it will ask if you want to attempt to fix known problem codes. The first code that is changed is the stock icons code. I don't know why the code caused a lockup at startup, but I just have it set up to modify that code to what the netplay build uses, which seems to work fine. Alternate stage support is achieved through my own mod of the SD loader code, where instead of trying to load from SD, it tries to load from disc. Unfortunately, I found that if the alternate stage is larger than the normal stage, it will crash, so to solve this I just set up the builder to pad the default stage files to be the same size as their largest alternate stage. While not the most elegant solution, it works.

## Other notes
If you are experiencing issues when playing the output ISO, make sure that you either don't have an SD card inserted, or your SD card doesn't have any mod files on it, as mods will still try to read from the SD before reading from the disc (unless you are on P:M 3.6 and told the builder to fix problem codes, and even then, some files are still loaded from SD).

If you are building this from source, make sure the Resources folder from the base directory is getting copied into the same directory as the exe file, or else nothing will work. This should happen automatically on build, but if it doesn't, just copy it manually.

## Credits
First of all thanks to Wiimm for his amazing wit tool, which is used to extract, patch, and build the game images.  
Secondly, thanks to the Dolphin team for the awesome debug mode of Dolphin, I wouldn't have been able to get alternate stages working without that.  
The folder select dialog uses Ookii dialogs, a great little library for nice looking dialogs.  
And thanks to the creators of all the ISO builder scripts for Project M and the like, they are what I based this off of.
