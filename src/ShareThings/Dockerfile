#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# FROM mcr.microsoft.com/dotnet/aspnet:5.0.3-alpine3.13 AS base
# FROM mcr.microsoft.com/dotnet/aspnet:5.0.3-alpine3.13 AS base
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base

RUN apt-get update \
	&& apt-get install -y --no-install-recommends unzip \
	&& curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg \
	&& dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# RUN apk update \
# 	&& apk add curl \
# 	&& apk add unzip \
# #install -y --no-install-recommends unzip \	
# 	&& curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg \
# 	&& dotnet tool install --global dotnet-ef --version 5.0.2
# ENV PATH="${PATH}:/root/.dotnet/tools"



# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
# WORKDIR /src
# COPY ["ShareThings/ShareThings.csproj", "ShareThings/"]
# RUN dotnet restore "ShareThings/ShareThings.csproj"
# COPY . .
# WORKDIR "/src/ShareThings"
# RUN dotnet build "ShareThings.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "ShareThings.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "ShareThings.dll"]

# COPY entrypoint.sh ./

# RUN chmod +x ./entrypoint.sh
# CMD /bin/bash ./entrypoint.sh