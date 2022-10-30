#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /sln 


COPY ./*.sln  ./

# Copy the main source project files
COPY src/*/*/*.csproj ./
# Recreate solution structure - src folder

RUN for file in $(ls *.csproj); \
		do \
			projectName=${file%.*} \
			&& projectLayer=$(echo ${projectName} | cut -d"." -f2)\
			&& mkdir -p src/$projectLayer/$projectName/ \
			&& mv $file src/$projectLayer/$projectName/; \
	done

# Copy the test project files
COPY test/*/*/*.csproj ./
# Recreate solution structure - test folder
RUN for file in $(ls *.csproj); \
		do \
			projectName=${file%.*} \
			&& projectLayer=$(echo ${projectName} | cut -d"." -f2)\
			&& mkdir -p test/$projectLayer/$projectName/ \
			&& mv $file test/$projectLayer/$projectName/; \
	done

RUN ls -d * */*

RUN dotnet restore
COPY . .
WORKDIR "src/Api/ScoreboardApp.Api"
RUN dotnet build "ScoreboardApp.Api.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "ScoreboardApp.Api.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["cert.pfx", "/https/cert.pfx"]
ENTRYPOINT ["dotnet", "ScoreboardApp.Api.dll"]