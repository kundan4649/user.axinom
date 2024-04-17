FROM mcr.microsoft.com/dotnet/core/sdk:2.2.207
ARG source=.
WORKDIR /app
COPY . .
RUN ["chmod", "+x", "/app/init.sh"]
ENTRYPOINT ["/bin/bash", "/app/init.sh"]