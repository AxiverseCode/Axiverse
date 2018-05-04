FROM mono:5.10
WORKDIR /build/
COPY . .
RUN nuget restore './Axiverse Services.sln'

ENV XBUILD_COLORS=errors=brightred,warnings=brightyellow
RUN xbuild /p:Configuration=Release './Axiverse Services.sln'