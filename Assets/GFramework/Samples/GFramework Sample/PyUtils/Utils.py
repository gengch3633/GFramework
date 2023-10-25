#!/usr/bin/python
# -*-coding: utf-8-*-

import json
import os
from FileUtils_GTest import FileUtils
from Modules.CardName import CardName
from Modules.ProfileConverter import ProfileConverter
from Modules.PictureNameConverter import PictureNameConverter

class Utils():
	def __init__(self):

		pass
	def changeCardName(self, cardFolder):
		cardPictures = FileUtils.getFiles(cardFolder, ".png")
		for cardPicture in cardPictures:
			cardName = CardName(cardPicture)
			cardName.changeName()

	def changeCardIndex(self, cardFolder):
		cardPictures = FileUtils.getFiles(cardFolder, ".png")
		cardPictures = sorted(cardPictures, key = lambda  cardPicture : int(FileUtils.getFileName(cardPicture, False).split("_")[1]))

		# print(json.dumps(cardPictures, indent = 4))

		for cardPicture in cardPictures:
			cardFolder = FileUtils.getFileFolder(cardPicture)
			cardName = FileUtils.getFileName(cardPicture, False)
			cardType = cardName.split("_")[0]
			newIndex = int(cardName.split("_")[1]) - 1
			newCardName = f"{cardType}_{newIndex}"

			print(cardName, newCardName)

			newCardPicture = f"{cardFolder}/{newCardName}.png"
			os.rename(cardPicture, newCardPicture)
		
	def createConfigs(self):
		excelFolder = "./Excels"
		dataFolder = "./Resources/Data"
		profileConverter = ProfileConverter(excelFolder, dataFolder)
		profileConverter.createGameConfigs()

	def renameAvatars(self, avatarFolder):
		profileConverter = PictureNameConverter(avatarFolder)
		profileConverter.renameAvatars("男", "avatar_boy_{0:03d}.png")
		profileConverter.renameAvatars("女", "avatar_girl_{0:03d}.png")

avatarFolder = "C:/Users/Administrator/Desktop/红桃/红桃/头像"
cardFolder = "C:/Users/Administrator/Desktop/纸牌4/纸牌4"

utils = Utils()
utils.createConfigs()
# utils.renameAvatars(avatarFolder)

# utils.changeCardIndex(cardFolder)
# utils.changeCardName(cardFolder)

