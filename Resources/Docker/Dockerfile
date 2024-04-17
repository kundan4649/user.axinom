FROM microsoft/aspnetcore:2.0.0
ARG source=.
WORKDIR /app
COPY . .
RUN ["chmod", "+x", "/app/init.sh"]
ENTRYPOINT ["/bin/bash", "/app/init.sh"]