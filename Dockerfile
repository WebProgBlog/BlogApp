FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BlogApp/BlogApp.csproj", "BlogApp/"]
RUN dotnet restore "BlogApp/BlogApp.csproj"
COPY . .
WORKDIR "/src/BlogApp"
RUN dotnet build "BlogApp.csproj" -c Release -o /app/build
RUN dotnet publish "BlogApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BlogApp.dll"]
