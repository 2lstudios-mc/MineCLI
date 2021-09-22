@echo off

:: Build for Windows
echo Building for Windows.
dotnet build

:: Build for Linux
echo Building for Linux
dotnet build --runtime ubuntu.16.04-x64