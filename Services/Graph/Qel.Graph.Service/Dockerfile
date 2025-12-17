FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY . ./src/
WORKDIR /src

RUN dotnet restore 
WORKDIR "/src/Services/Graph/Qel.Graph.Service"
RUN dotnet build "./Qel.Graph.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Services/Graph/Qel.Graph.Service"
RUN dotnet publish "./Qel.Graph.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
RUN apt-get update && apt-get install -y curl
WORKDIR /app
ENV TZ=Europe/Moscow
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Qel.Graph.Service.dll"]