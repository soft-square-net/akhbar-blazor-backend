@echo off
echo "[1] Starting a Docker Swarm with Postgres ... "
:repeat
docker ps -a > output.txt || ( timeout /t 5 && goto :repeat; )
echo " ... Docker started ... "
timeout /t 5
echo "[2] Starting a Docker Swarm with Postgres ... "
docker stack deploy -c docker-compose.yml devhome
echo " ... Docker Swarm started ... "