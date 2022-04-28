**Build:**

`dotnet build`

**Run:**

`dotnet run --project ./MartenPlayground`


**Local infrastructure:**

Starts postgres container in background:\
`docker-compose -f ./MartenPlayground/docker-compose.yml up -d`

Shutdown with:\
`docker-compose -f ./MartenPlayground/docker-compose.yml down`
