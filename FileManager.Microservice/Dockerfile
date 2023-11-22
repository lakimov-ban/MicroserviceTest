FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FileManager.Microservice.csproj", "FileManager.Microservice/"]
RUN dotnet restore "FileManager.Microservice/FileManager.Microservice.csproj"

WORKDIR "/src/FileManager.Microservice"
COPY . .

RUN dotnet build "FileManager.Microservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileManager.Microservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir /app/images
ENTRYPOINT ["dotnet", "FileManager.Microservice.dll"]
