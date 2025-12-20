@echo off
echo "[1] Start-and-poll Docker Swarm in the background till it can be started... "
docker desktop start
echo "[2] Start Docker EE postgres:17.0  in the background ... "
docker run --rm --name some-postgres -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=admin -p 5432:5432  postgres:17.0
set /p id="Finish"

