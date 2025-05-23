﻿ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:8.0
FROM ${DOTNET_SDK} AS build
WORKDIR /src
EXPOSE 80
EXPOSE 443

COPY ["HiСlass.Backend.sln", "./"]
COPY ["HiClass.API/HiClass.API.csproj", "HiClass.API/"]
COPY ["HiClass.Application/HiClass.Application.csproj", "HiClass.Application/"]
COPY ["HiClass.Domain/HiClass.Domain.csproj", "HiClass.Domain/"]
COPY ["HiClass.Infrastructure/HiClass.Infrastructure.csproj", "HiClass.Infrastructure/"]
COPY ["HiClass.Persistence/HiClass.Persistence.csproj", "HiClass.Persistence/"]
COPY ["HiClass.API.Tests/HiClass.API.Tests.csproj", "HiClass.API.Tests/"]
COPY ["HiClass.Application.Tests/HiClass.Application.Tests.csproj", "HiClass.Application.Tests/"]
COPY ["HiClass.Infrastructure.Tests/HiClass.Infrastructure.Tests.csproj", "HiClass.Infrastructure.Tests/"]
COPY ["HiClass.Persistence.Tests/HiClass.Persistence.Tests.csproj", "HiClass.Persistence.Tests/"]

RUN dotnet restore HiСlass.Backend.sln

COPY . .
WORKDIR "/src/HiClass.API"
RUN dotnet build "HiClass.API.csproj" -c Release -o /app/build

RUN dotnet tool install --global dotnet-ef --version 8.0.0

FROM build AS publish
RUN dotnet publish "HiClass.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HiClass.API.dll"]