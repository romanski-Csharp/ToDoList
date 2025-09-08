FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копіюємо csproj і відновлюємо залежності
COPY *.csproj ./
RUN dotnet restore

# Копіюємо весь код і збираємо реліз
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime образ
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ToDoList.dll"]
