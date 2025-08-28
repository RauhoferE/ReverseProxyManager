# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080 
EXPOSE 80 
#EXPOSE 8081 


# Put all of the settings from the appsettings in here
# For the files use the default nginx folders /nginx -> /includes /conf.d /ssl /html

FROM node:22 AS build-frontend
WORKDIR /frontend
COPY "reverse-proxy/package.json" "reverse-proxy/package-lock.json" ./
RUN npm install
#RUN --mount=type=cache,target=/root/.npm npm ci
COPY reverse-proxy/ .
RUN npm run build -- --output-path=./wwwroot
#RUN npm run build

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ReverseProxyManager/ReverseProxyManager.csproj", "ReverseProxyManager/"]
RUN dotnet restore "./ReverseProxyManager/ReverseProxyManager.csproj"
COPY . .
WORKDIR "/src/ReverseProxyManager"
RUN dotnet build "./ReverseProxyManager.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ReverseProxyManager.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build-frontend /frontend/wwwroot ./wwwroot
USER root
RUN mkdir /etc/data
RUN mkdir -p /etc/nginx/conf.d
RUN mkdir -p /etc/ssl/certs/nginx
RUN chown -R app:app /etc/data
RUN chown -R app:app /etc/nginx
RUN chown -R app:app /etc/ssl/certs/nginx
RUN chmod -R 755 /etc/ssl/certs/nginx
RUN chmod -R 755 /etc/nginx
RUN chmod -R 755 /etc/data
RUN apt-get update && apt-get install -y nginx
COPY "ReverseProxyManager/docker-entrypoint.sh" .
RUN chmod +x docker-entrypoint.sh
USER root


#ENTRYPOINT ["dotnet", "ReverseProxyManager.dll"]
ENTRYPOINT ["/app/docker-entrypoint.sh"]