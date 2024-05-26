#!/usr/bin/python
# -*-coding: utf-8-*-

import os
import json
from FileUtils_GTest import FileUtils

class ProfileConverter():
	def __init__(self, excelFolder, dataFolder):
		self.excelFolder = excelFolder
		self.dataFolder = dataFolder

	def createGameConfigs(self):
		excelFiles = FileUtils.getFiles(self.excelFolder, ".xls")
		for excelFile in excelFiles:
			excelName = FileUtils.getFileName(excelFile, False)
			jsonFile = os.path.join(self.dataFolder, f"{excelName}.json")
			jsonConfigs = FileUtils.readConfigExcel(excelFile, [])
			FileUtils.writeFile(jsonFile, json.dumps(jsonConfigs, indent = 4, ensure_ascii=False))
			print(json.dumps(jsonConfigs, indent = 4, ensure_ascii=False))




