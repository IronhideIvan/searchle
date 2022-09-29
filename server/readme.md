Searchle Backend

## Setting Secrets on a Development Environment

In order to set application secrets locally, we will use the [dotnet secrets manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows#set-multiple-secrets). 

1. First, create a file named "secrets.json" in the project directory.
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
