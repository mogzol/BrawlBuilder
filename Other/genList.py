#!/usr/bin/python

# This is what I used to generate the BrawlFileList.txt file.
# Give it a folder and it will walk through it and write filenames and sizes to a txt file.

import xml.sax
import sys
import os

f = open("brawlFileList.txt", "w")

for root, dirs, files in os.walk(sys.argv[1]):
    for file in files:
        info = os.stat(os.path.join(root, file))
        relDir = os.path.relpath(root, sys.argv[1])
        relFile = os.path.join(relDir, file)
        f.write(relFile + " " + str(info.st_size) + "\n")
		
f.close()