FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build

RUN mkdir /app
WORKDIR /src

# copy csproj and restore as distinct layers

COPY ./src/ .

RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app
COPY --from=build /app ./
#VOLUME ["/app/conect-sql.json"]
ENTRYPOINT ["dotnet", "NoteDown.dll"]