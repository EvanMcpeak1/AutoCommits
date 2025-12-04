@echo off
REM Automated GitHub Push Script
REM This script can be scheduled using Windows Task Scheduler

cd /d %~dp0
dotnet run

REM Optional: Log to a file for debugging
REM dotnet run >> push-log.txt 2>&1

pause

