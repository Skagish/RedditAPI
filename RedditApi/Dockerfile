#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RedditApi.csproj", ""]
COPY ["../RedditApi.Services/RedditApi.Services.csproj", "../RedditApi.Services/"]
COPY ["../RedditApi.Data/RedditApi.Data.csproj", "../RedditApi.Data/"]
COPY ["../RedditApi.Core/RedditApi.Core.csproj", "../RedditApi.Core/"]
RUN dotnet restore "./RedditApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "RedditApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RedditApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RedditApi.dll"]