FROM mcr.microsoft.com/dotnet/core/sdk:2.1

EXPOSE 80

WORKDIR /api

COPY ./CrashCourse/bin/Release/netcoreapp2.1/publish .

CMD ["dotnet", "CrashCourse.Api.dll"]
