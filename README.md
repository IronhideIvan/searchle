## Docker Compose

To start up the application, execute the following:

```
docker-compose up
```

or, if you have a specific environment file you'd like to use. Execute something like

```
docker-compose --env-file ./dev/dev.env up
```

## Postgres

### Main Commands
Start Postgres DB
```
> docker run --name postgres -e POSTGRES_PASSWORD=mysecretpassword -d -p 5432:5432 postgres
```

To connect to the server from within the container
```
> su postgres
> psql
```

### Useful Ppstgres Commands

List all databases
```
> \list
```

Connect to a database
```
> \connect <database name>
```