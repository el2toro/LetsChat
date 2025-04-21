FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LetsChat/LetsChat.csproj", "LetsChat/"]
COPY ["Core/Core/Core.csproj", "Core/Core/"]
RUN dotnet restore "LetsChat/LetsChat.csproj"
COPY . .
WORKDIR "/src/LetsChat"
RUN dotnet publish "LetsChat.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LetsChat.dll"]
