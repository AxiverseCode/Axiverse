FROM mono:5.10
WORKDIR /build/
COPY . .
RUN nuget restore
RUN xbuild /p:Configuration=Release