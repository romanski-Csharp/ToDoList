FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копіюємо csproj та solution
COPY *.sln ./
COPY *.csproj ./

# Відновлюємо залежності
RUN dotnet restore

# Копіюємо весь код
COPY . ./

# Публікуємо
RUN dotnet publish -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ToDoList.dll"]
