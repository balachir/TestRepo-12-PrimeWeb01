# PrimeWeb01

A .NET 10 ASP.NET Core API implementing the prime-checking behavior of
[TestRepo-11-PrimeWeb01](https://github.com/balachir/TestRepo-11-PrimeWeb01).

## Requirements and commands

Install the .NET 10 SDK pinned in [global.json](global.json), or a later patch
in the same feature band, then run from the solution directory:

```powershell
dotnet build PrimeWeb01.slnx
dotnet test PrimeWeb01.slnx
dotnet run --project .\PrimeWeb01\PrimeWeb01.csproj --launch-profile http
```

The HTTP profile listens at `http://localhost:5028`. Stop it with Ctrl+C.
Request examples are in [PrimeWeb01.http](PrimeWeb01/PrimeWeb01.http).

## HTTP behavior

Responses are plain text with HTTP 200, including invalid input and unknown paths,
for compatibility with the reference:

| Request | Response |
| --- | --- |
| `/checkprime?5` | `5 is prime!` |
| `/checkprime?6` | `6 is NOT prime!` |
| `/checkprime` or `/checkprime?abc` | `Pass in a number to check in the form /checkprime?5` |
| `/` or any other path | `Hello World! To check if a number is prime, provide URL of the form /checkprime?5` |

Supply a bare signed 32-bit integer query, not `number=5`. Values below two are
not prime; out-of-range values receive the invalid-input instructions. Invalid
input is also logged as a warning. Path matching is case-insensitive, but a
trailing slash does not match `/checkprime`.

The prime-checking middleware is intentionally terminal. Weather and OpenAPI
endpoints are not included. It supports a custom path via `UsePrimeChecker`;
the application uses `/checkprime`.

## Solution layout

- [PrimeWeb01](PrimeWeb01): API host, `PrimeService`, and configurable middleware.
- [PrimeWeb01.UnitTests](PrimeWeb01.UnitTests): xUnit service and HTTP tests.
  HTTP tests use a separate `TestServer` startup that runs the same middleware
  as the application.

Both projects target `net10.0`. No database, authentication, or external service
is required.
