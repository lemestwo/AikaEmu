# WebFiles

Placeholder files to be able to login into the server, note that these files must be replaced in the future with a proper authenticate system. AikaClient uses https protocol to authenticate accounts.


## Getting Started

### Requisites

- SSL certificate installed into your development environment. 
- Https protocol redirected to port 8090.


## Files

### member/aika_get_token.asp

Authenticate user and generates a hash to be used in AuthServer.

**Input:**
> Method: GET

> Arguments
```
id - Username
pw - Password
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
> Method: GET

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
> Replace XX with server id.

**Output:**
```
List with 56 numeric int values.
```