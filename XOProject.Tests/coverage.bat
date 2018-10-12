@echo off

dotnet clean
dotnet build /p:DebugType=Full
dotnet minicover instrument --workdir ../ --assemblies XOProject.Tests/**/bin/**/*.dll --sources XOProject/**/*.cs --exclude-sources XOProject/Migrations/**/*.cs --exclude-sources XOProject/*.cs --exclude-sources XOProject\Repository\ExchangeContext.cs

dotnet minicover reset --workdir ../

dotnet test --no-build
dotnet minicover uninstrument --workdir ../
dotnet minicover report --workdir ../ --threshold 60