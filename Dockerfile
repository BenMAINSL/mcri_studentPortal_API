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
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MCRI_Student_Employee_Data.dll"]
