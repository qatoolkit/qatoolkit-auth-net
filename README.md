# QAToolKit Authentication library
[![Build .NET Library](https://github.com/qatoolkit/qatoolkit-auth-net/workflows/Build%20.NET%20Library/badge.svg)](https://github.com/qatoolkit/qatoolkit-auth-net/actions)
[![CodeQL](https://github.com/qatoolkit/qatoolkit-auth-net/workflows/CodeQL%20Analyze/badge.svg)](https://github.com/qatoolkit/qatoolkit-auth-net/security/code-scanning)
[![Sonarcloud Quality gate](https://github.com/qatoolkit/qatoolkit-auth-net/workflows/Sonarqube%20Analyze/badge.svg)](https://sonarcloud.io/dashboard?id=qatoolkit_qatoolkit-auth-net)
[![NuGet package](https://img.shields.io/nuget/v/QAToolKit.Auth?label=QAToolKit.Auth)](https://www.nuget.org/packages/QAToolKit.Auth/)
[![Discord](https://img.shields.io/discord/787220825127780354?color=%23267CB9&label=Discord%20chat)](https://discord.gg/hYs6ayYQC5)

## Description
`QAToolKit.Auth` is a .NET Standard 2.1 library, that retrieves the JWT access tokens from different identity providers. This library should only be used for testing software and not in applications to retrieve the token.

Currently it supports next Identity providers and Oauth2 flows:
- `Keycloak`: Library supports:
  - [client credentials flow](https://tools.ietf.org/html/rfc6749#section-4.4) or `Protection API token (PAT)` flow. Additionally you can replace the PAT with user token by exchanging the token.
  - [resource owner password credentials grant](https://www.appsdeveloperblog.com/keycloak-requesting-token-with-password-grant/)
- `Azure B2C`: Library supports:
  - [client credentials flow](https://azure.microsoft.com/en-us/services/active-directory/external-identities/b2c/)
  - [resource owner password credentials flow](https://docs.microsoft.com/en-us/azure/active-directory-b2c/add-ropc-policy?tabs=app-reg-ga&pivots=b2c-user-flow)
- `Identity Server 4`: Library supports:
  - [client credentials flow](https://identityserver4.readthedocs.io/en/latest/quickstarts/1_client_credentials.html)
  - [resource owner password]()  

Supported .NET frameworks and standards: `netstandard2.0`, `netstandard2.1`, `netcoreapp3.1`, `net5.0`.

Get in touch with me on:

[![Discord](https://img.shields.io/discord/787220825127780354?color=%23267CB9&label=Discord%20chat)](https://discord.gg/hYs6ayYQC5)

**Please note**: _The token expiration time is read from the tokens and is used to minimize the hits on the token endpoint. Another benefit is faster tests :)._

## 1. Keycloak support

Keycloak support is limited to the `client credential` or `Protection API token (PAT)` flow in combination with `token exchange`. Additionally you can also get the token by using `resource owner password credentials grant`.

### 1.1. Client credential flow

A mocked request below is sent to the Keycloak endpoint, and the PAT token is retrieved:

```bash
curl -X POST \
    -H "Content-Type: application/x-www-form-urlencoded" \
    -d 'grant_type=client_credentials&client_id=${client_id}&client_secret=${client_secret}' \
    "http://localhost:8080/auth/realms/${realm_name}/protocol/openid-connect/token"
```

Read [more](https://www.keycloak.org/docs/latest/authorization_services/#_service_protection_whatis_obtain_pat) here in the Keycloak documentation.

Now let's retrive a PAT token with QAToolKit Auth library:

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

### 1.2. Exchange token for user token

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
});

//Get client credentials flow access token
var token = await auth.GetAccessToken();
//Replace client credentials flow token for user access token
var userToken = await auth.ExchangeForUserToken("myuser@email.com");
```

### 1.3. Resource owner password credentials grant

```csharp
var auth = new KeycloakAuthenticator(options =>
{
    options.AddResourceOwnerPasswordCredentialFlowParameters(
                new Uri("https://my.keycloakserver.com/auth/realms/realmX/protocol/openid-connect/token"), 
                "my_client",
                "client_secret",
                "user",
                "pass");
});

var token = await auth.GetAccessToken();
```

## 2. Identity Server 4 support

Under the hood it's the same code that retrieves the `client credentials flow` access token, but authenticator is explicit for Identity Server 4. Additionally you can also get the token by using `resource owner password`.

```csharp
var auth = new IdentityServer4Authenticator(options =>
{
    options.AddClientCredentialFlowParameters(
            new Uri("https://<myserver>/token"),
            "my_client"
            "<client_secret>");
});
            
var token = await auth.GetAccessToken();
```

#### Resource owner password

```csharp
var auth = new IdentityServer4Authenticator(options =>
{
    options.AddResourceOwnerPasswordCredentialFlowParameters(
            new Uri("https://<myserver>/token"),
            "my_client"
            "<client_secret>",
            "user",
            "pass");
});
            
var token = await auth.GetAccessToken();
```

## 3. Azure B2C support

Under the hood it's the same code that retrieves the `client credentials flow` access token, but authenticator is explicit for Azure B2C. Additionally you can also get the token by using `resource owner password credentials flow`.

Azure B2C client credentials flow needs a defined scope which is usually `https://graph.windows.net/.default`.

```csharp
var auth = new AzureB2CAuthenticator(options =>
{
    options.AddClientCredentialFlowParameters(
            new Uri("https://login.microsoftonline.com/<tenantID>/oauth2/v2.0/token"),
            "<clientId>"
            "<clientSecret>"
            new string[] { "https://graph.windows.net/.default" });
});
            
var token = await auth.GetAccessToken();
```

#### Resource owner password credentials flow

```csharp
var auth = new AzureB2CAuthenticator(options =>
{
    options.AddResourceOwnerPasswordCredentialFlowParameters(
            new Uri("https://login.microsoftonline.com/<tenantID>/oauth2/v2.0/token"),
            "<clientId>"
            "<clientSecret>"
            new string[] { "https://graph.windows.net/.default" },
            "user",
            "pass");
});
            
var token = await auth.GetAccessToken();
```

## To-do

- **This library is an early alpha version**

## License

MIT License

Copyright (c) 2020-2021 Miha Jakovac

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