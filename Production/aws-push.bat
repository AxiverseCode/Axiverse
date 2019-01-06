# push image
# https://docs.aws.amazon.com/AmazonECR/latest/userguide/docker-push-ecr-image.html

# aws ecr get-login --no-include-email --region us-east-1
# docker login

docker build -t production_build -f .\build.dockerfile ..\Source

docker build -t calibrate-services-dev -f .\services.dockerfile .
docker tag calibrate-services-dev:latest 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-services-dev:latest
docker push 214054665271.dkr.ecr.us-east-1.amazonaws.com/calibrate-services-dev:latest