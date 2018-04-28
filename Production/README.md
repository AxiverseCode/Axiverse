
from the root of the project

docker build -f .\Production\IdentityService\dockerfile .

docker-compose up -d            # detaches console
docker-compose down
docker-compose down -v          # tears down volumes too