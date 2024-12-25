#!/usr/bin/python
# -*-coding: utf-8-*-

import json
import os
import sys
import subprocess
import pandas as pd
from FileUtils_GTest import FileUtils

class BatUtils():
	def __init__(self):
		projectPath = __file__[0:__file__.index("\\Assets")]
		self.assetPath = f"{projectPath}/Assets"

	def setEnv(self):
		batFiles = FileUtils.getFiles(self.assetPath, ".bat")
		excelFiles = FileUtils.getFiles(self.assetPath, ".xls")

		envSetterBat = [item for item in batFiles if(FileUtils.getFileName(item, False) == "EnvSetter")][0]
		buildSettingsExcel = [item for item in excelFiles if(FileUtils.getFileName(item, False) == "BuildSettings")][0]

		buildSettingsData = pd.read_excel(buildSettingsExcel)
		buildSettingsDict = dict(zip(buildSettingsData['key'],buildSettingsData['value']))
		pathValueDict = dict(filter(lambda x: "_Path" in x[0], buildSettingsDict.items()))
		envValueDict = dict(filter(lambda x: "Path" not in x[0], buildSettingsDict.items()))

		for key,value in envValueDict.items():
			print(f"[BatUtils] [envValueDict] [setEnv]: {key}, isValueExists: {os.path.exists(value)}, value: {value}")
			subprocess.run([envSetterBat, key, value], shell=True)

		for key,value in pathValueDict.items():
			print(f"[BatUtils] [pathValueDict] [setEnv]: {key}, isValueExists: {os.path.exists(value)}, value: {value}")
			pathString = os.environ.get('Path')
			pathList = pathString.split(";")
			if(value in pathList):
				continue

			pathList.append(value)
			newPathString = ";".join(pathList)
			subprocess.run([envSetterBat, "Path", newPathString], shell=True)

utils = BatUtils()
utils.setEnv()
