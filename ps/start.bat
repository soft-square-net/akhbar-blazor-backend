@echo off
echo "[2] Start Docker EE in the background ... "
start "" "run-docker-desktop.bat"
echo "[2] Start api server in the background ... "
start "" "run-api-server.bat"
echo "[2] Start Blazor Client in the background ... "
start "" "run-blazor-client.bat"
echo "[2] Start Code in the background ... "


code ..\src\apps\blazor &

set /p id="Finish"