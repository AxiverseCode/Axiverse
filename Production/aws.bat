# push image
# https://docs.aws.amazon.com/AmazonECR/latest/userguide/docker-push-ecr-image.html

# aws ecr get-login --no-include-email --region us-east-1
# docker login

docker build -t production_build -f ..\Production\build.dockerfile .

docker build -t calibrate-identity-dev -f ..\Production\identity-service.dockerfile .
docker tag calibrate-identity-dev:latest 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-identity-dev:latest
docker push 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-identity-dev:latest

docker build -t calibrate-identity-dev -f ..\Production\chat-service.dockerfile .
docker tag calibrate-chat-dev:latest 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-chat-dev:latest
docker push 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-chat-dev:latest

docker build -t calibrate-identity-dev -f ..\Production\entity-service.dockerfile .
docker tag calibrate-entity-dev:latest 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-entity-dev:latest
docker push 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-entity-dev:latest