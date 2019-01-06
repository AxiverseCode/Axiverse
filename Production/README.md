
from the Source folder of the project

// Creates the build
docker build -t axi-build -f ..\Production\build.dockerfile .
docker run -it axi-build /bin/bash

docker build -t axi-services -f ..\Production\services.dockerfile .
docker run axi-services

docker build -t axi-web -f ..\Production\web.dockerfile .
docker run -p 8080:8080/tcp axi-web


docker-compose up -d            # detaches console
docker-compose down
docker-compose down -v          # tears down volumes too

docker-compose up -f ..\Production\docker-compose.yml
