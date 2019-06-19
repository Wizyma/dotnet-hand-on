FROM mcr.microsoft.com/dotnet/core/sdk:2.1

EXPOSE 5000

WORKDIR /api

COPY . .

CMD ["dotnet", "run", "-p", "CrashCourse"]
