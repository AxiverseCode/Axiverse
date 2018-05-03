FROM node:9.11
WORKDIR /build/
COPY ./package*.json ./
RUN npm install
# If you are building your code for production
# RUN npm install --only=production

COPY . .
RUN npx webpack --mode production
EXPOSE 8000
CMD [ "npx", "ts-node", "src/server/index.ts" ]