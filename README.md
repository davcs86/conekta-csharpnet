# Conekta .Net C# client v 1.0.7

[![NuGet downloads](https://img.shields.io/nuget/dt/ConektaClient.svg)](https://www.nuget.org/packages/ConektaClient)

[![Build Status](https://travis-ci.org/davcs86/conekta-csharpnet.svg)](https://travis-ci.org/davcs86/conekta-csharpnet)

This is an un-official C# library that allows interaction with https://api.conekta.io API. This project is **Mono/.Net40 compatible**.

### Bugfix for SSL error


Change the [security protocol](https://msdn.microsoft.com/en-us/library/system.net.securityprotocoltype(v=vs.100).aspx) to TLS with


```
    Conekta.SecurityProtocol =  Conekta.SecurityProtocolType.Tls;

```


## DUMMY DEMO AVAILABLE IN

ASP.Net MVC 5 Dummy demo for this library [here](https://github.com/davcs86/conekta-csharpnet-dummydemo)

## Installation

Obtain the latest version of the Conekta C# bindings with:

    git clone https://github.com/davcs86/conekta-csharpnet

OR

Use the Nuget Package Manager Console

    Install-Package ConektaClient

## Usage

To get started, add the following to your code:

    using ConektaCSharp;


```csharp    
Conekta.ApiKey ="1tv5yJp3xnVZ7eK67m4h";

JObject valid_payment_method = JObject.Parse("{'description':'Stogies'," +
                                             "'reference_id':'9839-wolf_pack'," +
                                             "'amount':20000," +
                                             "'currency':'MXN'," +
                                             "'card':'tok_test_visa_4242'}");

try
{
    Charge charge = Charge.create(_params);
    Console.WriteLine(charge.ToString());
}
catch (Error e)
{
    Console.WriteLine(e.ToString());
}

// Console.WriteLine(charge.ToString());

{
  "id": "54f5e2f419ce8824a00086ce",
  "livemode": false,
  "created_at": 1425400564,
  "status": "paid",
  "currency": "MXN",
  "description": "Stogies",
  "reference_id": "9839-wolf_pack",
  "amount": 20000,
  "paid_at": 1425400582,
  "fee": 963,
  "payment_method": {
    "name": "Jorge Lopez",
    "exp_month": "12",
    "exp_year": "19",
    "auth_code": "000000",
    "type": "credit",
    "last4": "4242",
    "brand": "visa"
  },
  "details": {}
}

```

## Documentation

Please see https://www.conekta.io/docs/api for up-to-date documentation.

## Tests

The library has Unit Tests and you can run them separately.

License
-------
Developed by [David Castillo](mailto:davcs86@gmail.com). Available with [MIT License](LICENSE).
