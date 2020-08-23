#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Moosetrail.Playgrounds.API/Moosetrail.Playgrounds.API.csproj", "Moosetrail.Playgrounds.API/"]
RUN dotnet restore "Moosetrail.Playgrounds.API/Moosetrail.Playgrounds.API.csproj"
COPY . .
WORKDIR "/src/Moosetrail.Playgrounds.API"
RUN dotnet build "Moosetrail.Playgrounds.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Moosetrail.Playgrounds.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Moosetrail.Playgrounds.API.dll"]