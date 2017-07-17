FROM microsoft/dotnet:2.0.0-preview2-runtime
WORKDIR /dragonsmtp
COPY PublishOutput .
EXPOSE 25/tcp
ENTRYPOINT ["dotnet", "DragonMail.SMTP.dll"]