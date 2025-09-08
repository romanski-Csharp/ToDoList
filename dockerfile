FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копіюємо solution
COPY ToDoList.sln ./

# Створюємо папку ToDoList і копіюємо csproj туди
RUN mkdir ToDoList
COPY ToDoList.csproj ./ToDoList/

# Відновлюємо залежності
RUN dotnet restore

# Копіюємо весь код
COPY . ./

# Публікуємо
RUN dotnet publish ToDoList/ToDoList.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ToDoList.dll"]
