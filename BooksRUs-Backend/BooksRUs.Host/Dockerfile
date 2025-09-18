# ---- build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy everything to restore/publish
COPY . .
# restore using the Host csproj (adjust path if your .sln is higher)
RUN dotnet restore BooksRUs.Host/BooksRUs.Host.csproj
RUN dotnet publish BooksRUs.Host/BooksRUs.Host.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://0.0.0.0:5050
EXPOSE 5050

ENTRYPOINT ["dotnet", "BooksRUs.Host.dll"]