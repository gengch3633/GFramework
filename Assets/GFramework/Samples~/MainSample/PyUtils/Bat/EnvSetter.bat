rem @echo off
set KEY=%1
set VALUE=%2
setx %KEY% %VALUE% /m
