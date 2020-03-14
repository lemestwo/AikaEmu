# AikaEmu

AikaEmu is a completely open source **C# .Net Core 3.1** emulator for the MMORPG **Aika Online**. 

It's code is based on several ideas from others emulators and is being developed only for educational purposes. 

AikaEmu is free and licensed under [GNU GPL v3.0](LICENSE.md).

If you want more information or just talk, please join our [Discord](https://discord.gg/pVAXHWS).

## Getting Started

### Setup Server

* Follow instructions in [Web folder](src/AikaEmu.WebServer/README.md) to setup web.
* Create MySql database "aikaemu_auth" and "aikaemu_game".
* Run MySql script [Auth](sql/aikaemu_auth.sql) and [Game](sql/aikaemu_game.sql).
* Insert new GameServer into DB.
```
INSERT INTO `aikaemu_auth`.`game_servers`(`id`, `name`, `ip`, `port`) VALUES (1, 'AikaEmu', '127.0.0.1', 8822);
```
* Insert new Account into DB (user: admin / pass: admin).
```
INSERT INTO `aikaemu_auth`.`accounts`(`user`, `pass`) VALUES ('admin', '21232f297a57a5a743894a0e4a801fc3');
```
* Change each **"Config.json"** file inside each server to your settings.
* Run **WebServer**, **AuthServer** and **GameServer**.

### Setup client

* Paste "SL.bin" (client folder) inside your client folder to connect to 127.0.0.1.
> Right now we are using latest AikaBR client to connect, should change in the future to NA.

### Compile Server with "dotnet publish"

* Open **"Publish.sh"** and change RUNTIME variable to your environment, save and run it.
* Run **"Start Servers.bat"**

## Issues

You can use **Github's issue tracker** to register issues.

Just make sure to take your time checking before submitting to avoid duplicates.
