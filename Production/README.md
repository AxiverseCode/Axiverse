
from the Source folder of the project

docker build -t axi-identity-service -f ..\Production\identity-service.dockerfile .
docker run axi-identity-service

docker build -t axi-web -f ..\Production\web.dockerfile .
docker run -p 8080:8080/tcp axi-web


docker-compose up -d            # detaches console
docker-compose down
docker-compose down -v          # tears down volumes too

docker-compose up -f ..\Production\docker-compose.yml

docker build -t axibuild -f ..\Production\build.dockerfile .
docker run -it axibuild /bin/bash