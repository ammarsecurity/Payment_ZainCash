# ZainCash .NET Core Package for Online Payments in Iraq

[![NuGet Version](https://img.shields.io/nuget/v/ZainCashPayment.svg)](https://www.nuget.org/packages/Payment_ZainCash/1.0.0)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ZainCashPayment.svg)](https://www.nuget.org/packages/Payment_ZainCash/1.0.0)

This package provides a .NET Core implementation of ZainCash API for online payments in Iraq.

## Usage

To use this package, follow these steps:

1. Install the package using NuGet.

```powershell
Install-Package Payment_ZainCash

using Payment_ZainCash;

ZainPayment zainPayment = new ZainPayment();
string url = zainPayment.GenerateZaincashUrl(
    orderid: null,
    amount: 100,
    isdollar: false,
    dollar_exchange_rate: 1190,
    redirectionUrl: "https://example.com/redirect",
    msisdn: "9647712345678",
    serviceType: "Service",
    secret: "your_secret_key",
    merchantid: "your_merchant_id",
    lang: "en",
    developemntMode: true);


Dictionary<string, string> response = zainPayment.AfterRedirection(
    token: "your_token",
    secret: "your_secret_key");
string status = response["status"];
if (status == "success")
{
    // Payment succeeded
}
else if (status == "failed")
{
    string message = response["msg"];
    // Payment failed
}


License
This package is licensed under the MIT License.




