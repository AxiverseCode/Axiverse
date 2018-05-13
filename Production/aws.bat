# push image
# https://docs.aws.amazon.com/AmazonECR/latest/userguide/docker-push-ecr-image.html

# aws ecr get-login --no-include-email --region us-east-1
# docker login

docker build -t production_build -f .\build.dockerfile ..\Source

docker build -t calibrate-identity-dev -f .\service-identity.dockerfile .
docker tag calibrate-identity-dev:latest 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-identity-dev:latest
docker push 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-identity-dev:latest

docker build -t calibrate-chat-dev -f .\service-chat.dockerfile .
docker tag calibrate-chat-dev:latest 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-chat-dev:latest
docker push 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-chat-dev:latest

docker build -t calibrate-entity-dev -f .\service-entity.dockerfile .
docker tag calibrate-entity-dev:latest 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-entity-dev:latest
docker push 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-entity-dev:latest

docker build -t calibrate-web-dev -f .\web.dockerfile ..\Web
docker tag calibrate-web-dev:latest 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-web-dev:latest
docker push 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-web-dev:latest