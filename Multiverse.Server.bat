set ASPNETCORE_ENVIRONMENT=Development
set ASPNETCORE_HTTPS_PORT=20011
set ASPNETCORE_URLS=https://localhost:20011;http://localhost:20012
set Multiverse:AllowedWorlds=1,2
set Multiverse:Universes=X:\Dropbox\Projects\Multiverse\Multiverse.SimpleUniverse\bin\Debug\net5.0\Multiverse.SimpleUniverse.dll

cd Multiverse.Server
bin\Debug\net5.0\Multiverse.Server.exe
