FROM mono:5.10 
WORKDIR /app/
COPY --from=axi-build /build/Deployment/Services/bin/Release .
EXPOSE 32000
CMD [ "mono",  "./Services.exe" ]