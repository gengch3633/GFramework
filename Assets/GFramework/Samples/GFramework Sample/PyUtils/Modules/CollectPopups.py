#!/usr/bin/python
# -*-coding: utf-8-*-

import os
import json
from FileUtils_GTest import FileUtils

rootFolder = "../../"
codeFiles = FileUtils.getFiles(rootFolder, ".cs")
popupFiles = []
for codeFile in codeFiles:
	fr = open(codeFile,'r', encoding='utf-8', errors = 'ignore')   #读写模式：r-只读；r+读写；w-新建（会覆盖原有文件）;更多信息见参考文章2
	fileData = fr.read()
	if(": UIPopup" in fileData):
		popupFiles.append(codeFile)

popupFiles = [FileUtils.getFileName(item, False) for item in popupFiles]
print(",\n".join(popupFiles))

