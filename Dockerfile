FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src

COPY . .
RUN dotnet restore ConversationPlanner.csproj
RUN dotnet publish ConversationPlanner.csproj -c Release -o /app
RUN dotnet test --logger "trx;LogFileName=unittests.trx"
# unittests.trx ends up in the TestResults directory, this directory can be mapped with a volume so the results
# can be picked up by Jenkins.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
COPY --from=build /app .
# Using CMD instead of ENTRYPOINT so it can be overridden in a Jenkins pipeline.
CMD ["dotnet", "ConversationPlanner.dll"]
EXPOSE 80
