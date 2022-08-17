# Wordnet Database

## Initial Setup

### Edit appsettinga

Edit appsettings.json file with the appropriate connection information.

### Create the database

Login onto the database server and run the following script to create the database

`postgresql-wordnet30-createdb.sql`

Next, execute

`dotnet run`

on the project in order upgrade the database