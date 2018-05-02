
from the Source folder of the project

docker build -t axi-identity-service -f ..\Production\identity-service.dockerfile .
docker run axi-identity-service

docker-compose up -d            # detaches console
docker-compose down
docker-compose down -v          # tears down volumes too


docker build -t axibuild -f ..\Production\build.dockerfile .
docker run -it axibuild /bin/bash