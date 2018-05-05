FROM mono:5.10 
WORKDIR /app/
COPY --from=production_build /build/Services/Axiverse.Services.ChatService/bin/Release .
EXPOSE 32000
CMD [ "mono",  "./Axiverse.Services.ChatService.exe" ]