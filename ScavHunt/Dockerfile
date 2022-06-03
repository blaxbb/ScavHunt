#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ScavHunt/ScavHunt.csproj", "ScavHunt/"]
RUN dotnet restore "ScavHunt/ScavHunt.csproj"
COPY . .
WORKDIR "/src/ScavHunt"
RUN dotnet build "ScavHunt.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScavHunt.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScavHunt.dll"]