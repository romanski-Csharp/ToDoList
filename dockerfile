# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копіюємо solution і проект (обидва в корені)
COPY *.sln ./
COPY *.csproj ./

# Відновлюємо залежності
RUN dotnet restore

# Копіюємо весь код
COPY . ./

# Публікуємо проект
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Запуск додатку
ENTRYPOINT ["dotnet", "ToDoList.dll"]
