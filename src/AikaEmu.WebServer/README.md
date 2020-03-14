# AikaEmu.WebServer

WebServer made in **ASP.Net Core 3.1**.

## Getting Started

### Requisites

* SSL certificate installed into your development environment. 
* Https protocol redirected to port "8090".

### Dev-Certs

* To use a localhost SSL certificate for development, run in console:
```
dotnet dev-certs https --trust
```

### Setup

* Find "Settings.txt" in client folder.
* Edit to something like this:

```
[AIKA]
https://localhost:8090/
https://localhost:8090/patch.html
33 136 405 195 https://localhost:8090/news/index.aspx
AikaBR.exe
140 340 00FFFFFF 00212018 Tahoma 11
20  349
335 363
138 363
237 363
141 418
141 430
1 https://localhost:8090/
...
```

## Routes

### member/aika_get_token.asp

Authenticate user and generates a hash to be used in AuthServer.

**Input:**
> Method: POST

> Arguments
```
id - Username
pw - Password (md5)
```

**Output:**
> Success
```
Hash MD5
```
> Failure
```
Int number less or equal 0.
```

### servers/aika_get_chrcnt.asp

Show created characters from that account with nation if available.

**Input:**
> Method: POST

> Arguments
```
id - Username
pw - Hash
```

**Output template:**
```
CNT 0 0 0 0
<br>
0 0 0 0
```

### servers/servXX.asp

Show server population and more unknown information.

**Input:**
> XX is server id (default = 00).

**Output:**
```
List with 56 numeric int values.
```