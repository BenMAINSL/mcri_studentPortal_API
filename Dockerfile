FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY MCRI_Student_Employee_Data/*.csproj MCRI_Student_Employee_Data/
RUN dotnet restore MCRI_Student_Employee_Data/MCRI_Student_Employee_Data.csproj

COPY . .
RUN dotnet publish MCRI_Student_Employee_Data/MCRI_Student_Employee_Data.csproj \
    -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

# Render injects the port it expects the container to listen on as PORT, so the app
# binds to that rather than a hard-coded number. The 8080 fallback keeps
# "docker run -p 8080:8080" working locally.
ENTRYPOINT ["sh", "-c", "exec dotnet MCRI_Student_Employee_Data.dll --urls http://0.0.0.0:${PORT:-8080}"]
