FROM mono:5.10 AS builder
WORKDIR /build/
COPY . .
RUN nuget restore
RUN xbuild /p:Configuration=Release

FROM mono:5.10 
WORKDIR /app/
COPY --from=builder /build/Services/Axiverse.Services.IdentityService/bin/Release .
EXPOSE 32000
CMD [ "mono",  "./Axiverse.Services.IdentityService.exe" ]