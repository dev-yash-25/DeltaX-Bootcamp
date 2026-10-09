# Options Pattern
 **Options Pattern is basically the clean way of taking a section from `appsettings.json` and putting it into a C# configuration class.**

### Benefits
1. The bigger benefit is that it turns loosely typed configuration into a structured, typed object that can be validated, 
   - Like by default, loosely typed mean that all values are string by default,
   - But with class, they can be strong typed to int, Eg:- ExpiryMinutes is an int, not a string.
2. it allows validation
3. It groups related settings

<br>

### Without Options Pattern

Suppose `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "secret",
    "Issuer": "IMDB_API",
    "Audience": "IMDB_API",
    "ExpiryMinutes": 60
  }
}
```

You could access values like:

```csharp
Configuration["Jwt:Key"]
Configuration["Jwt:Issuer"]
Configuration["Jwt:Audience"]
```

This works, but you're dealing with **strings everywhere**.

---

### With Options Pattern

You create a class that represents the configuration:

```csharp
public class JwtOptions
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryMinutes { get; set; }
}
```

Then you tell ASP.NET:

```csharp
services.Configure<JwtOptions>(
    Configuration.GetSection("Jwt"));
```

This basically says:

```text
appsettings.json
       ↓
     "Jwt"
       ↓
   JwtOptions
       ↓
 ┌───────────────┐
 │ Key            │
 │ Issuer         │
 │ Audience       │
 │ ExpiryMinutes  │
 └───────────────┘
```

Then your `JwtService` can receive:

```csharp
IOptions<JwtOptions>
```

```csharp
public JwtService(IOptions<JwtOptions> options)
{
    var jwtOptions = options.Value;

    var key = jwtOptions.Key;
    var issuer = jwtOptions.Issuer;
}
```

### So what's the actual benefit?

Instead of this everywhere:

```csharp
Configuration["Jwt:Key"]
Configuration["Jwt:Issuer"]
Configuration["Jwt:Audience"]
```

you get a **strongly typed object**:

```csharp
jwtOptions.Key
jwtOptions.Issuer
jwtOptions.Audience
```

So think of it as:

> **Options Pattern = map a configuration section into a strongly typed C# class and inject that class where needed.**

And importantly, this **doesn't put the actual secret back into the class**.

The class:

```csharp
public class JwtOptions
{
    public string Key { get; set; }
}
```

defines the **shape**.

The actual value still comes from:

```text
appsettings / environment / Vault
             ↓
      configuration system
             ↓
        JwtOptions
```

That's the connection between the two concepts you were just discussing.
