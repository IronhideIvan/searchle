Searchle Backend

## Building and Running in Docker

From the `server/` directory, run the following command to build a docker image:

```
docker build -f webapi.Dockerfile -t searchle/latest .  
```

Now run the following command to start a new container using the image:

```
docker run -d -p 8080:80 --name searchle-server searchle/latest
```

Open a browser and navigate to localhost:8080 and you should see the API up and running!

## Setting Secrets on a Development Environment

In order to set application secrets locally, we will use the [dotnet secrets manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows#set-multiple-secrets). 

1. Then, create a file named "secrets.json" in the project directory.
2. Add the following contents into the file:

```json
{
  "Searchle": {
    "DictionaryConnectionString": "",
    "RootKey": ""
  }
}
```

3. Replace the value of `DictionaryConnectionString` with the connection string to your dictionary database. Also, if you are encrypting your local values, add the value of the root secret that is used to decrypt the other values.

4. In a command like, execute the following command from the project directory:

_In Windows_
`type .\secrets.json | dotnet user-secrets set`

_In Linux/MacOS_
`cat ./secrets.json | dotnet user-secrets set`

5. NOTE: If you are creating a new project and secrets aren't enabled, run:

`dotnet user-secrets init`