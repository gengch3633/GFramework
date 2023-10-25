#!/usr/bin/python
# -*-coding: utf-8-*-

import os
import json
from FileUtils_GTest import FileUtils

class PictureNameConverter():
	def __init__(self, pictureFolder):
		self.pictureFolder = pictureFolder

	def renameAvatars(self, preName, newNameFormat):
		pngFiles = FileUtils.getFiles(self.pictureFolder, ".png")
		pngFiles = [item for item in pngFiles if(preName in FileUtils.getFileName(item, False))]
		for fileIndex in range(len(pngFiles)):
			pngFile = pngFiles[fileIndex]
			fileFolder = FileUtils.getFileFolder(pngFile)
			newPngFile = f"{fileFolder}/{newNameFormat.format(fileIndex)}"
			os.rename(pngFile, newPngFile)

