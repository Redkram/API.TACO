@echo off
setlocal enabledelayedexpansion

for %%F in ("%~dp0.") do set "FOLDER_NAME=%%~nxF"

set IMAGE_NAME=!FOLDER_NAME!

set CONTAINER_ID=
FOR /F "tokens=*" %%i IN ('docker ps --filter "ancestor=!IMAGE_NAME!" --format "{{.ID}}"') DO (
    if not defined CONTAINER_ID call set CONTAINER_ID=%%i
)

git fetch origin master > NUL 2>&1
for /f %%i in ('git rev-parse HEAD') do set LOCAL=%%i
for /f %%i in ('git rev-parse origin/master') do set REMOTE=%%i

if "!LOCAL!"=="!REMOTE!" (
    if not defined CONTAINER_ID (
        echo El contenedor no está corriendo. Levantando...
        docker compose up --build -d
    ) else (
        echo No hay cambios y el contenedor está activo. Nada que hacer.
    )
) else (
    echo Cambios detectados. Reiniciando...
    if defined CONTAINER_ID (
        docker stop !CONTAINER_ID!
    )
    git pull origin master
    docker compose up --build -d
)

endlocal
exit
