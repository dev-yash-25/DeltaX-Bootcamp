# JWT & Authentication in ASP.NET Core 5

## Index

-   [1. Authentication & Authorization](#1-authentication--authorization)
-   [2. What is JWT?](#2-what-is-jwt)
-   [3. JWT Structure](#3-jwt-structure)
    -   [3.1 Header](#31-header)
    -   [3.2 Payload](#32-payload)
    -   [3.3 Signature](#33-signature)
-   [4. JWT Claims](#4-jwt-claims)
-   [5. JWT Algorithms](#5-jwt-algorithms)
    -   [5.1 HS256](#51-hs256)
    -   [5.2 RS256](#52-rs256)
    -   [5.3 ES256](#53-es256)
    -   [5.4 Algorithm Comparison](#54-algorithm-comparison)
-   [6. How JWT Authentication Works](#6-how-jwt-authentication-works)
-   [7. JWT in ASP.NET Core 5.0](#7-jwt-in-aspnet-core-50)
    -   [7.1 Package](#71-package)
    -   [7.2 `appsettings.json`](#72-appsettingsjson)
    -   [7.3 `Startup.cs`](#73-startupcs)
    -   [7.4 Middleware Order](#74-middleware-order)
-   [8. Generating a JWT](#8-generating-a-jwt)
    -   [8.1 Login Model](#81-login-model)
    -   [8.2 Token Generation](#82-token-generation)
    -   [8.3 Login Endpoint](#83-login-endpoint)
-   [9. Protecting APIs](#9-protecting-apis)
    -   [9.1 `[Authorize]`](#91-authorize)
    -   [9.2 `[AllowAnonymous]`](#92-allowanonymous)
    -   [9.3 Roles](#93-roles)
    -   [9.4 Claims & Policies](#94-claims--policies)
-   [10. Postman Flow](#10-postman-flow)
-   [11. Expiration & Refresh Tokens](#11-expiration--refresh-tokens)
-   [12. JWT Security Rules](#12-jwt-security-rules)
-   [13. Common Errors](#13-common-errors)
-   [14. Complete JWT Template](#14-complete-jwt-template)
-   [15. JWT Cheat Sheet](#15-jwt-cheat-sheet)
-   [16. Final Mental Model](#16-final-mental-model)


<br>

---

<br>


# 1. Authentication & Authorization



### Authentication

Answers:

> **Who are you?**

Examples:

``` text
Username + Password
JWT
Cookie
OAuth / OpenID Connect
```

### Authorization

Answers:

> **What are you allowed to do?**

``` text
Authentication
      ↓
Who are you?
      ↓
Authorization
      ↓
What can you access/do?
```

Example:

``` text
JWT valid
    ↓
User authenticated
    ↓
Role = Admin
    ↓
Admin endpoint allowed
```


<br>

---

<br>


# 2. What is JWT?

**JWT = JSON Web Token**

A JWT is a compact, URL-safe token format used to transmit claims
between parties. In Web APIs, it is commonly used as a **bearer access
token**.

<br>
<table>
  <tr>
    <td align="center">
      <img width="700" alt="image" src="https://github.com/user-attachments/assets/47f70ea2-a481-49aa-a33a-ddc10ed4bb91" />
    </td>
    <td align="center">
      <img width="650" alt="image" src="https://github.com/user-attachments/assets/91883c29-3a21-431b-acc8-1d2a95eed05c" />
    </td>
  </tr>
</table>
<br>
Typical flow:

``` text
CLIENT
  ↓
POST /login
  ↓
Username + Password
  ↓
SERVER validates credentials
  ↓
Generate JWT
  ↓
Return JWT
  ↓
CLIENT sends JWT on later requests
```

Later:

``` http
Authorization: Bearer <JWT>
```

JWT works particularly well for APIs because the server can validate the
signed token and establish the user's identity without maintaining a
traditional server-side session for every access token.

> [!Important]

JWT is **signed, not automatically encrypted**. Its payload is
Base64URL-encoded and can normally be decoded. Never put passwords, API
secrets, or other confidential data into a normal JWT payload.


<br>

---

<br>


# 3. JWT Structure

<br>
<div align = "center">
<img width="600" alt="image" src="https://github.com/user-attachments/assets/b5d9fefd-bac7-4d33-bc8a-39cbd71d4ee5" />
</div>
<br>

A JWT has three dot-separated parts:

``` text
HEADER.PAYLOAD.SIGNATURE
```

Example shape:

``` text
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9
.
eyJzdWIiOiIxMjMiLCJyb2xlIjoiQWRtaW4ifQ
.
<signature>
```

``` text
JWT
 ├── Header
 ├── Payload
 └── Signature
```

`<br>`{=html}

## 3.1 Header

Contains token metadata.

``` json
{
    "alg": "HS256",
    "typ": "JWT"
}
```

`alg` identifies the cryptographic signing algorithm.

`typ` identifies the token type.

`<br>`{=html}

## 3.2 Payload

Contains **claims**.

``` json
{
    "sub": "123",
    "name": "Yash",
    "role": "Admin",
    "exp": 1760000000
}
```

The payload is not secret simply because it is inside the JWT.

`<br>`{=html}

## 3.3 Signature

Conceptually:

``` text
Signature =
    Sign(
        Base64Url(Header)
        +
        "."
        +
        Base64Url(Payload),
        Key
    )
```

The server verifies the signature with the appropriate trusted key.

If someone changes:

``` text
role = User
```

to:

``` text
role = Admin
```

without being able to produce a valid signature, verification fails.

### Remember

``` text
Encoding ≠ Encryption ≠ Signing
```


<br>

---

<br>


# 4. JWT Claims

A **claim** is a statement/information about the token subject.

### Registered JWT claims

  Claim   Meaning
  ------- -----------------
  `iss`   Issuer
  `sub`   Subject
  `aud`   Audience
  `exp`   Expiration time
  `nbf`   Not before
  `iat`   Issued at
  `jti`   JWT ID

Example:

``` json
{
    "iss": "BookStoreAPI",
    "sub": "123",
    "aud": "BookStoreClient",
    "iat": 1759996400,
    "exp": 1760000000
}
```

### Custom claims

Applications can add their own:

``` csharp
new Claim("userId", user.Id.ToString())
```

``` csharp
new Claim("department", "IT")
```

Role claim:

``` csharp
new Claim(ClaimTypes.Role, "Admin")
```

### Claims mental model

``` text
JWT
 ↓
Claims
 ├── Subject / User ID
 ├── Role
 ├── Department
 ├── Permission
 └── Other identity information
```


<br>

---

<br>


# 5. JWT Algorithms

The JWT header contains an `alg` value describing the signing algorithm.

Common algorithms:

``` text
HS256
RS256
ES256
```

`<br>`{=html}

## 5.1 HS256

``` text
HS256 = HMAC using SHA-256
```

Uses a **shared secret**.

``` text
Issuer
  │
  │ shared secret
  ↓
Sign JWT
  ↓
JWT
  ↓
API
  │
  │ same shared secret
  ↓
Verify
```

ASP.NET Core configuration commonly uses:

``` csharp
var key = Encoding.UTF8.GetBytes("strong-secret");

var securityKey =
    new SymmetricSecurityKey(key);

var credentials =
    new SigningCredentials(
        securityKey,
        SecurityAlgorithms.HmacSha256
    );
```

### Advantage

Simple and efficient.

### Limitation

Every party that verifies tokens needs access to the same secret.


<br>

---

<br>


## 5.2 RS256

``` text
RS256 = RSA + SHA-256
```

Uses asymmetric cryptography:

``` text
Private Key → Sign
Public Key  → Verify
```

``` text
Identity Server
      │
 Private Key
      ↓
    Sign
      ↓
    JWT
      ↓
 Public Key
      ↓
API verifies
```

Useful when a centralized issuer signs tokens and multiple services need
to verify them without receiving the private signing key.


<br>

---

<br>


## 5.3 ES256

``` text
ES256 = ECDSA using P-256 + SHA-256
```

Also asymmetric:

``` text
Private Key → Sign
Public Key  → Verify
```

It provides strong public-key cryptography with relatively compact keys.


<br>

---

<br>


## 5.4 Algorithm Comparison

  Algorithm   Type         Sign            Verify
  ----------- ------------ --------------- --------------------
  HS256       Symmetric    Shared secret   Same shared secret
  RS256       Asymmetric   Private key     Public key
  ES256       Asymmetric   Private key     Public key

### Mental model

``` text
HS256
Secret → Sign
Secret → Verify

RS256
Private → Sign
Public  → Verify

ES256
Private → Sign
Public  → Verify
```

> [!Important]

The API should explicitly trust the algorithms/keys appropriate to its
architecture. Do not blindly accept an incoming token's claimed
algorithm.


<br>

---

<br>


# 6. How JWT Authentication Works

## Login

``` text
POST /api/auth/login
        ↓
Username + Password
        ↓
Find user
        ↓
Verify password hash
        ↓
Valid?
   ┌────┴────┐
   No        Yes
   ↓          ↓
 401       Create claims
              ↓
          Sign JWT
              ↓
          Return JWT
```

## Protected Request

``` text
GET /api/books

Authorization:
Bearer eyJ...
        ↓
ASP.NET Core JWT Middleware
        ↓
Read token
        ↓
Validate:
  Signature
  Issuer
  Audience
  Lifetime
        ↓
Valid?
   ┌────┴────┐
   No        Yes
   ↓          ↓
 401      ClaimsPrincipal
              ↓
         Authorization
              ↓
          Controller
```


<br>

---

<br>


# 7. JWT in ASP.NET Core 5.0

## 7.1 Package

Install:

``` text
Microsoft.AspNetCore.Authentication.JwtBearer
```

This adds JWT Bearer authentication support.


<br>

---

<br>


## 7.2 `appsettings.json`

``` json
{
  "Jwt": {
    "Key": "your-strong-development-secret-key",
    "Issuer": "BookStoreAPI",
    "Audience": "BookStoreClient",
    "DurationInMinutes": 30
  }
}
```

For production, keep real secrets in an appropriate secret-management
mechanism rather than committing them to source control.


<br>

---

<br>


## 7.3 `Startup.cs`

``` csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public void ConfigureServices(
    IServiceCollection services)
{
    services.AddControllers();

    var key = Encoding.UTF8.GetBytes(
        Configuration["Jwt:Key"]
    );

    services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(key),

                    ValidateIssuer = true,

                    ValidIssuer =
                        Configuration["Jwt:Issuer"],

                    ValidateAudience = true,

                    ValidAudience =
                        Configuration["Jwt:Audience"],

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                };
        });

    services.AddAuthorization();
}
```

### Important settings

``` csharp
ValidateIssuerSigningKey = true
```

Validate the JWT signature with the trusted key.

``` csharp
ValidateIssuer = true
```

Check `iss`.

``` csharp
ValidateAudience = true
```

Check `aud`.

``` csharp
ValidateLifetime = true
```

Check token lifetime.

``` csharp
ClockSkew = TimeSpan.Zero
```

This example removes the normal clock-skew allowance.


<br>

---

<br>


## 7.4 Middleware Order

ASP.NET Core 5:

``` csharp
public void Configure(
    IApplicationBuilder app,
    IWebHostEnvironment env)
{
    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
```

Remember:

``` text
Routing
   ↓
Authentication
   ↓
Authorization
   ↓
Endpoints
```

### Authentication

``` text
Who is this user?
```

### Authorization

``` text
Is this user allowed?
```

> [!Important]

`UseAuthentication()` must execute before `UseAuthorization()`.


<br>

---

<br>


# 8. Generating a JWT

## 8.1 Login Model

``` csharp
public class LoginModel
{
    public string Username { get; set; }

    public string Password { get; set; }
}
```


<br>

---

<br>


## 8.2 Token Generation

``` csharp
private string GenerateToken(
    int userId,
    string username,
    string role)
{
    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"]
        )
    );

    var credentials =
        new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

    var claims = new[]
    {
        new Claim(
            JwtRegisteredClaimNames.Sub,
            userId.ToString()
        ),

        new Claim(
            JwtRegisteredClaimNames.UniqueName,
            username
        ),

        new Claim(
            ClaimTypes.Role,
            role
        )
    };

    var token = new JwtSecurityToken(
        issuer:
            _configuration["Jwt:Issuer"],

        audience:
            _configuration["Jwt:Audience"],

        claims: claims,

        expires:
            DateTime.UtcNow.AddMinutes(30),

        signingCredentials:
            credentials
    );

    return new JwtSecurityTokenHandler()
        .WriteToken(token);
}
```

### Generation flow

``` text
User data
   ↓
Claims
   ↓
Signing key
   ↓
SigningCredentials
   ↓
JwtSecurityToken
   ↓
WriteToken()
   ↓
JWT string
```


<br>

---

<br>


## 8.3 Login Endpoint

Simplified demonstration:

``` csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(LoginModel model)
    {
        // Real application:
        // 1. Find user in database.
        // 2. Verify stored password hash.
        // 3. Generate token only if valid.

        if (model.Username != "yash" ||
            model.Password != "password")
        {
            return Unauthorized();
        }

        var token = GenerateToken(
            1,
            model.Username,
            "Admin"
        );

        return Ok(new
        {
            token
        });
    }
}
```

> [!Important]

The hardcoded credentials are only a demonstration. Never store
plaintext passwords in a real application. Use a proper
password-hashing/identity system.


<br>

---

<br>


# 9. Protecting APIs

## 9.1 `[Authorize]`

``` csharp
[Authorize]
[HttpGet]
public IActionResult GetBooks()
{
    return Ok();
}
```

Without a valid token:

``` text
401 Unauthorized
```

With a valid token:

``` text
Controller executes
```


<br>

---

<br>


## 9.2 `[AllowAnonymous]`

Used for endpoints that should not require authentication.

Typical login:

``` csharp
[AllowAnonymous]
[HttpPost("login")]
public IActionResult Login(LoginModel model)
{
    ...
}
```

Otherwise the client could not obtain its first token.


<br>

---

<br>


## 9.3 Roles

Create role claim:

``` csharp
new Claim(
    ClaimTypes.Role,
    "Admin"
)
```

Protect endpoint:

``` csharp
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public IActionResult DeleteBook(int id)
{
    return Ok();
}
```

Multiple roles:

``` csharp
[Authorize(Roles = "Admin,Manager")]
```

### 401 vs 403

``` text
401 Unauthorized
    ↓
Authentication failed/missing

403 Forbidden
    ↓
Authenticated, but authorization failed
```

Example:

``` text
Valid JWT
   ↓
Role = User
   ↓
Admin endpoint
   ↓
403 Forbidden
```


<br>

---

<br>


## 9.4 Claims & Policies

Read a claim:

``` csharp
var userId = User.FindFirst(
    JwtRegisteredClaimNames.Sub
)?.Value;
```

Inspect claims:

``` csharp
foreach (var claim in User.Claims)
{
    Console.WriteLine(
        $"{claim.Type} = {claim.Value}"
    );
}
```

### Policy example

``` csharp
services.AddAuthorization(options =>
{
    options.AddPolicy(
        "ITOnly",
        policy =>
        {
            policy.RequireClaim(
                "department",
                "IT"
            );
        }
    );
});
```

Endpoint:

``` csharp
[Authorize(Policy = "ITOnly")]
[HttpGet("internal")]
public IActionResult InternalData()
{
    return Ok();
}
```

Flow:

``` text
JWT
 ↓
department = IT
 ↓
Policy
 ↓
Authorization
```


<br>

---

<br>


# 10. Postman Flow

### Step 1 --- Login

``` text
POST /api/auth/login
```

Body:

``` json
{
    "username": "yash",
    "password": "password"
}
```

Response:

``` json
{
    "token": "eyJhbGciOi..."
}
```

### Step 2 --- Protected endpoint

``` text
GET /api/books
```

Header:

``` http
Authorization: Bearer eyJhbGciOi...
```

In Postman:

``` text
Authorization
Type: Bearer Token
Token: <JWT>
```

### Step 3 --- API validates

``` text
Token
 ↓
Signature
 ↓
Issuer
 ↓
Audience
 ↓
Expiration
 ↓
Claims
 ↓
Authorization
```


<br>

---

<br>


# 11. Expiration & Refresh Tokens

## Access-token expiration

Generate a short-lived access token:

``` csharp
expires: DateTime.UtcNow.AddMinutes(30)
```

After expiration:

``` text
JWT
 ↓
Lifetime validation
 ↓
Expired
 ↓
401 Unauthorized
```

The exact lifetime should depend on the application's security and UX
requirements.

`<br>`{=html}

## Refresh tokens

A common architecture is:

``` text
Short-lived Access Token
+
Longer-lived Refresh Token
```

Flow:

``` text
LOGIN
  ↓
Access Token + Refresh Token
  ↓
Use Access Token
  ↓
Access Token expires
  ↓
Send Refresh Token
  ↓
Server validates refresh token
  ↓
Issue new Access Token
```

Example response:

``` json
{
    "accessToken": "eyJ...",
    "refreshToken": "random-long-value"
}
```

> [!Important]

A refresh token is not simply an access JWT with a longer expiration.
Production systems commonly treat refresh tokens as high-entropy
credentials and handle expiration, revocation, rotation, secure storage
and reuse detection.


<br>

---

<br>


# 12. JWT Security Rules

### 1. Always use HTTPS

JWTs are credentials.

``` text
HTTPS
 ↓
Encrypted transport
 ↓
JWT
```

### 2. Never put passwords in JWTs

Do not put:

``` text
Password
Password hash
Private API keys
```

into the payload.

### 3. Keep the payload small

Use claims needed for identity/authorization.

### 4. Use strong signing keys

Bad:

``` text
123456
password
secret
```

Good:

``` text
Strong random secret/key
```

### 5. Do not hardcode production secrets

Use:

``` text
Environment variables
Secret stores
Managed key/secret services
```

as appropriate.

### 6. Validate more than the signature

Configure appropriate:

``` text
Signing key
Issuer
Audience
Lifetime
Algorithm
```

### 7. Use HTTPS and secure token storage

The storage mechanism for browser applications depends on the
application architecture and threat model. Avoid exposing bearer tokens
unnecessarily to client-side JavaScript.

### 8. Keep access tokens short-lived

Short lifetimes reduce the useful lifetime of a stolen access token.

### 9. Protect refresh tokens

Handle:

``` text
Expiration
Revocation
Rotation
Secure storage
Reuse detection
```

### 10. Do not log full JWTs

A JWT is a bearer credential. Treat it like a password-equivalent
credential while it is valid.


<br>

---

<br>


# 13. Common Errors

## `401 Unauthorized`

Possible causes:

``` text
No token
Malformed token
Expired token
Invalid signature
Wrong secret/key
Wrong issuer
Wrong audience
Unsupported/untrusted algorithm
```

Check:

``` http
Authorization: Bearer <token>
```

`<br>`{=html}

## `403 Forbidden`

Usually:

``` text
Authentication succeeded
        ↓
Authorization failed
```

Example:

``` text
User authenticated
BUT
Role ≠ Admin
```

`<br>`{=html}

## Signature validation failed

Possible reasons:

``` text
Wrong signing key
Token modified
Different algorithm
Configuration mismatch
```

`<br>`{=html}

## Role authorization not working

Check that the emitted claim matches what authorization expects:

``` csharp
new Claim(
    ClaimTypes.Role,
    "Admin"
)
```

and:

``` csharp
[Authorize(Roles = "Admin")]
```

`<br>`{=html}

## `User.Claims` unexpected/empty

Check:

``` text
UseAuthentication()
        ↓
UseAuthorization()
        ↓
UseEndpoints()
```

Also inspect:

``` text
Token validity
Claim names
Claim mapping/configuration
```


<br>

---

<br>


# 14. Complete JWT Template

## `appsettings.json`

``` json
{
  "Jwt": {
    "Key": "your-strong-development-secret-key",
    "Issuer": "BookStoreAPI",
    "Audience": "BookStoreClient",
    "DurationInMinutes": 30
  }
}
```

`<br>`{=html}

## `Startup.cs`

``` csharp
public void ConfigureServices(
    IServiceCollection services)
{
    services.AddControllers();

    var key = Encoding.UTF8.GetBytes(
        Configuration["Jwt:Key"]
    );

    services
        .AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(key),

                    ValidateIssuer = true,

                    ValidIssuer =
                        Configuration["Jwt:Issuer"],

                    ValidateAudience = true,

                    ValidAudience =
                        Configuration["Jwt:Audience"],

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                };
        });

    services.AddAuthorization();
}
```

`<br>`{=html}

## `Configure()`

``` csharp
public void Configure(
    IApplicationBuilder app,
    IWebHostEnvironment env)
{
    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
```

`<br>`{=html}

## Protected Controller

``` csharp
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    [HttpGet]
    public IActionResult GetBooks()
    {
        return Ok("Authenticated");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        return Ok("Admin access");
    }
}
```


<br>

---

<br>


# 15. JWT Cheat Sheet

  Concept                          Remember
  -------------------------------- -------------------------------------------------
  JWT                              JSON Web Token
  Structure                        Header.Payload.Signature
  Header                           Algorithm + token metadata
  Payload                          Claims
  Signature                        Integrity/authenticity
  HS256                            Shared secret
  RS256                            Private sign / public verify
  ES256                            Private sign / public verify
  Bearer header                    `Authorization: Bearer <token>`
  ASP.NET package                  `Microsoft.AspNetCore.Authentication.JwtBearer`
  Protect endpoint                 `[Authorize]`
  Public endpoint                  `[AllowAnonymous]`
  Role authorization               `[Authorize(Roles = "Admin")]`
  Policy authorization             `[Authorize(Policy = "...")]`
  Authentication middleware        `UseAuthentication()`
  Authorization middleware         `UseAuthorization()`
  Missing/invalid authentication   `401`
  Authenticated but forbidden      `403`
  Token lifetime                   `exp`
  Issuer                           `iss`
  Audience                         `aud`
  Subject                          `sub`


<br>

---

<br>


# 16. Final Mental Model

``` text
                         LOGIN
                           │
                           ↓
                  Username + Password
                           │
                           ↓
                    Verify Credentials
                           │
                           ↓
                       Create Claims
                           │
                           ↓
                    Sign JWT with Key
                           │
                           ↓
                       Return JWT
                           │
                           ↓
                         CLIENT
                           │
                           │
             Authorization: Bearer <JWT>
                           │
                           ↓
                    ASP.NET Core API
                           │
                           ↓
                  JWT Authentication
                           │
                 ┌─────────┴─────────┐
                 ↓                   ↓
              Invalid              Valid
                 ↓                   ↓
          401 Unauthorized      ClaimsPrincipal
                                      │
                                      ↓
                               Authorization
                                      │
                         ┌────────────┴────────────┐
                         ↓                         ↓
                      Allowed                    Denied
                         ↓                         ↓
                    Controller               403 Forbidden
                         │
                         ↓
                      Response
```

## JWT Internals

``` text
JWT
 │
 ├── Header
 │      └── alg / typ
 │
 ├── Payload
 │      └── Claims
 │
 └── Signature
        └── Integrity + authenticity
```

## ASP.NET Core Request Pipeline

``` text
Request
   ↓
UseRouting()
   ↓
UseAuthentication()
   ↓
JWT validation
   ↓
ClaimsPrincipal
   ↓
UseAuthorization()
   ↓
[Authorize] / Roles / Policies
   ↓
Controller
   ↓
Response
```

## One-Line Memory Trick

``` text
LOGIN
  → GENERATE JWT
  → SEND Bearer TOKEN
  → VALIDATE
  → BUILD USER/CLAIMS
  → AUTHORIZE
  → EXECUTE API
```
