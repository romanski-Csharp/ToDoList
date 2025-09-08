# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копіюємо solution і csproj у підпапку ToDoList
COPY ToDoList.sln ./
COPY ToDoList/ToDoList.csproj ./ToDoList/

# Відновлюємо залежності
RUN dotnet restore

# Копіюємо весь код проекту
COPY ToDoList/ ./ToDoList/

# Публікуємо додаток
RUN dotnet publish ToDoList/ToDoList.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Копіюємо результати з build stage
COPY --from=build /app/out .

# Запуск додатку
ENTRYPOINT ["dotnet", "ToDoList.dll"]
