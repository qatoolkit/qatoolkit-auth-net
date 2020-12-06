# QAToolKit Authentication library
[![Build .NET Library](https://github.com/qatoolkit/qatoolkit-auth-net/workflows/Build%20.NET%20Library/badge.svg)](https://github.com/qatoolkit/qatoolkit-auth-net/actions)
[![CodeQL](https://github.com/qatoolkit/qatoolkit-auth-net/workflows/CodeQL%20Analyze/badge.svg)](https://github.com/qatoolkit/qatoolkit-auth-net/security/code-scanning)
[![Sonarcloud Quality gate](https://github.com/qatoolkit/qatoolkit-auth-net/workflows/Sonarqube%20Analyze/badge.svg)](https://sonarcloud.io/dashboard?id=qatoolkit_qatoolkit-auth-net)
[![NuGet package](https://img.shields.io/nuget/v/QAToolKit.Auth?label=QAToolKit.Auth)](https://www.nuget.org/packages/QAToolKit.Auth/)

## Description
`QAToolKit.Auth` is a .NET Standard 2.1 library, that retrieves the JWT access tokens from different identity providers.

Currently it supports next Identity providers and Oauth2 flows:
- `Keycloak`: Library supports Keycloak [client credentials flow](https://tools.ietf.org/html/rfc6749#section-4.4) or `Protection API token (PAT)` flow. Additionally you can replace the PAT with user token by exchanging the token.

Supported .NET frameworks and standards: `netstandard2.0`, `netstandard2.1`, `netcoreapp3.1`, `net5.0`

## 1. Keycloak support

Keycloak support is limited to the `client credential` or `Protection API token (PAT)` flow in combination with `token exchange`.

### 1.1. Client credential flow

A mocked request below is sent to the Keycloak endpoint, and the PAT token is retrieved:

```bash
curl -X POST \
    -H "Content-Type: application/x-www-form-urlencoded" \
    -d 'grant_type=client_credentials&client_id=${client_id}&client_secret=${client_secret}' \
    "http://localhost:8080/auth/realms/${realm_name}/protocol/openid-connect/token"
```

Read [more](https://www.keycloak.org/docs/latest/authorization_services/#_service_protection_whatis_obtain_pat) here in the Keycloak documentation.

Now let's retrive a PAT token with QAToolKit Auth libraray:

```csharp
var auth = new KeycloakAuthenticator(options =>
{
    options.AddClientCredentialFlowParameters(
                new Uri("https://my.keycloakserver.com/auth/realms/realmX/protocol/openid-connect/token"), 
                "my_client",
                "client_secret");
});

var token = await auth.GetAccessToken();
```

### 1.2. Client credential flow with token exchange

If you want to replace the PAT token with user token, you can additionally specify a username. A mocked request looks like this:

```bash
curl -X POST \
    -H "Content-Type: application/x-www-form-urlencoded" \
    -d 'grant_type=urn:ietf:params:oauth:grant-type:token-exchange&client_id=${client_id}&client_secret=${client_secret}&subject_token=eyJhbGciOiJI...&requested_subject=myuser@users.com' \
    "http://localhost:8080/auth/realms/${realm_name}/protocol/openid-connect/token"
```

As you see it has a different `grant_type` and additionally 2 more properties in the URL; PAT token (`subject_token`) and userName for which we want to replace the token (`requested_subject`).

```csharp
var auth = new KeycloakAuthenticator(options =>
{
    options.AddClientCredentialFlowParameters(
                new Uri("https://my.keycloakserver.com/auth/realms/realmX/protocol/openid-connect/token"), 
                "my_client",
                "client_secret"); 
    options.AddUserNameForImpersonation("myuser@users.com");
});

var token = await auth.GetAccessToken();
```

## To-do

- **This library is an early alpha version**
- Add more providers identity providers.
- Add more OAuth2 flows.

## License

MIT License

Copyright (c) 2020 Miha Jakovac

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.