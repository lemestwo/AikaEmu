# AikaEmu

AikaEmu is a completely open source **C# .Net Core 2.2** emulator for the MMORPG **Aika Online**. 

It's code is based on several ideas from others emulators, mostly AAEmu and Aura.

It's being developed only for educational purposes. 

AikaEmu is free and licensed under [GNU GPL v3.0](LICENSE.md).

Join our community at [Discord]() if you have any questions.

## Getting Started

* Compile the server.
* Follow instructions in [Web folder](web/README.md) to setup web.
* Create MySql database "aikaemu_auth" and "aikaemu_game".
* Run MySql script [Auth](sql/aikaemu_auth.sql) and [Game](sql/aikaemu_game.sql).
* Insert new GameServer into DB.
```
INSERT INTO `aikaemu_auth`.`game_servers`(`id`, `name`, `ip`, `port`) VALUES (1, 'AikaEmu', '127.0.0.1', 8822);
```
* Insert new Account into DB.
```
INSERT INTO `aikaemu_auth`.`accounts`(`server`, `user`, `pass`, `level`, `last_ip`, `session_hash`, `session_time`) VALUES (1, 'admin', 'admin', 0, 0, '760c18a928f10a6acc6dd7b0a71fb294', '2019-04-17 17:42:02');
```
* Start AuthServer and GameServer.

> Right now we are using latest AikaBR client to connect, should change in the future to NA.

> Paste "SL.bin" (client folder) inside your client folder to connect to 127.0.0.1.

## Issues

You can use **Github's issue tracker** to register issues.

Just make sure to take your time checking before submitting to avoid duplicates.