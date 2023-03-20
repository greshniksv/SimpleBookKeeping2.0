#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Application/Application.csproj", "Application/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY . .
RUN dotnet restore "LegalCollection.sln"

WORKDIR "/src"
RUN dotnet build "LegalCollection.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LegalCollection.sln" -c Release

FROM base AS final
WORKDIR /app
COPY --from=publish /src/Application/bin/Release/net6.0/publish .
ENTRYPOINT ["dotnet", "Application.dll"]