# Використовуємо офіційний SDK образ для білду
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копіюємо csproj і відновлюємо залежності
COPY *.csproj ./
RUN dotnet restore

# Копіюємо весь код і публікуємо релізну збірку
COPY . ./
RUN dotnet publish -c Release -o out

# Використовуємо легкий runtime образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Запускаємо твою програму
ENTRYPOINT ["dotnet", "ToDoList.dll"]
