## Docker Compose

To start up the application, execute the following:

```
docker-compose up
```

or, if you have a specific environment file you'd like to use. Execute something like

```
docker-compose --env-file ./dev/dev.env up
```

## NGINX
On the host machine, create the following folder structure from the context of docker-compose:

```
- data
  - nginx
```

Next, add your nginx configuration to the created "nginx" folder.

## SSL

### Required folders

On the host machine, create the following folder structure from the context of docker-compose:

```
- data
  - certbot
    - conf
    - www
```

Then, initialize letsencrypt. Make the appropriate edits in "init-letsencrypt.sh" and
execute the following:

`sudo init-letsencrypt.sh`

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

### Useful Postgres Commands

List all databases
```
> \list
```

Connect to a database
```
> \connect <database name>
```