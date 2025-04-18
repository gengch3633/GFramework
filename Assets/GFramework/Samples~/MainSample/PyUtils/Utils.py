#!/usr/bin/python
# -*-coding: utf-8-*-

import json
import os
import sys
from FileUtils_GTest import FileUtils

class Utils():
	def __init__(self):
		projectPath = __file__[0:__file__.index("Assets")]
		self.assetPath = f"{projectPath}Assets"
		self.dataPath = f"{self.assetPath}/Game/Data"
		self.fontTextFile = f"{self.assetPath}/Game/FontText.txt"

	def createConfigs(self):
		excelFiles = FileUtils.getFiles(self.assetPath, ".xls")
		excelFiles = [item for item in excelFiles if("EventTracker" not in item)]
		repeatFiles = self.getRepeatFiles(excelFiles)
		if(len(repeatFiles) != 0):
			for repeatFile in repeatFiles:
				print(f"==> [Utils] [getRepeatFiles] 1: {FileUtils.getFileName(repeatFile)}")
				for excelFile in excelFiles:
					if(repeatFile in excelFile):
						print(f"==> [Utils] [getRepeatFiles] 2: {excelFile}")
			return
		self.createNormalConfig(excelFiles)
		for excelFile in excelFiles:
			excelName = FileUtils.getFileName(excelFile, False)
			jsonFile = os.path.join(self.dataPath, f"{excelName}.json")
			jsonConfigs = FileUtils.readConfigExcel(excelFile, [])
			FileUtils.writeFile(jsonFile, json.dumps(jsonConfigs, indent = 4, ensure_ascii=False))
			
			print(f"==> [Utils] [createConfigs]: {FileUtils.getFileName(excelName)}")

	def getRepeatFiles(self, excelFiles):
		excelNames = [FileUtils.getFileName(item) for item
		 in excelFiles]

		excelDict = {}
		for excelName in excelNames:
			if(excelName not in excelDict):
				excelDict[excelName] = 0
			excelDict[excelName] = excelDict[excelName]  + 1

		excelDict = {k:v for k,v in excelDict.items() if(v != 1)}
		repeatFiles = list(excelDict.keys())
		return repeatFiles

	def createNormalConfig(self, excelFiles):
		languageInfoExcels = [item for item in excelFiles if("LanguageInfo" in item)]
		languageInfoNames = [FileUtils.getFileName(item) for item in languageInfoExcels]

		itemMap = {}
		itemMap["languageInfoKeys"] = languageInfoNames

		normalConfigFile = f"{self.dataPath}/NormalConfig.json"
		FileUtils.writeFile(normalConfigFile, json.dumps(itemMap, indent = 4, ensure_ascii=False))
		print(f"==> [Utils] [createNormalConfig]: {FileUtils.getFileName(normalConfigFile)}")

	def createFontText(self, languageKeysString):
		languageKeys = languageKeysString.split("-")
		print(f"==> [createFontText] languageKeys: {languageKeys}")
		excelFiles = FileUtils.getFiles(self.assetPath, ".xls")
		excelFiles = [item for item in excelFiles if("LanguageInfo" in item)]
		fontTexts = []
		for excelFile in excelFiles:
			print(f"==> [createFontText] excelFile: {excelFile}")
			excelName = FileUtils.getFileName(excelFile, False)
			jsonFile = os.path.join(self.dataPath, f"{excelName}.json")
			jsonConfigs = FileUtils.readConfigExcel(excelFile, [])
			for jsonConfig in jsonConfigs:
				itemKey = jsonConfig["KEY"]
				isInKeys = itemKey in languageKeys
				if(not isInKeys):
					continue
				for key, value in jsonConfig.items():
					if(key == "id"):
						continue
					fontTexts.extend(value)
		
		asciiChars = [chr(item) for item in range(32, 127)]
		asciiString = ''.join(asciiChars)
		fontTexts.extend(asciiString)
		
		fontTexts = list(set(fontTexts))
		fontTexts.sort()
		fontString = "".join(fontTexts)
		FileUtils.writeFile(self.fontTextFile, fontString)

languageKeysString = sys.argv[1] if(len(sys.argv) > 1) else ""
utils = Utils()
utils.createConfigs()
utils.createFontText(languageKeysString)
