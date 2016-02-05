#!/usr/bin/python

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