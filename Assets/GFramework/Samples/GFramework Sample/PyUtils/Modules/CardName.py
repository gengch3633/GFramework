#!/usr/bin/python
# -*-coding: utf-8-*-

import os
import json
from FileUtils_GTest import FileUtils

class CardName():
	def __init__(self, cardPicture):
		self.cardPicture = cardPicture

	def getNewName(self, origName):
		splitInfos = origName.split("_")

		colorName = splitInfos[0]
		numName = splitInfos[1]

		colorDict = {}
		colorDict["HEITAO"] = "S"
		colorDict["HONGTAO"] = "H"
		colorDict["MEIHUA"] = "C"
		colorDict["FANGKUAI"] = "D"

		numDict = {}
		numDict["0"] = "A"
		numDict["1"] = "2"
		numDict["2"] = "3"
		numDict["3"] = "4"
		numDict["4"] = "5"
		numDict["5"] = "6"
		numDict["6"] = "7"
		numDict["7"] = "8"
		numDict["8"] = "9"
		numDict["9"] = "T"
		numDict["10"] = "J"
		numDict["11"] = "Q"
		numDict["12"] = "K"

		newName = f"{numDict[numName]}{colorDict[colorName]}"
		return newName

	def changeName(self):
		cardFolder = FileUtils.getFileFolder(self.cardPicture)
		cardName = FileUtils.getFileName(self.cardPicture, False)
		newCardName = self.getNewName(cardName)
		newCardPicture = f"{cardFolder}/{newCardName}.png"
		newCardPictureMetaFile = f"{newCardPicture}.meta"
		os.rename(self.cardPicture, newCardPicture)

		print(f"==> [CardName] [changeName]: {cardName}, {newCardName}")




