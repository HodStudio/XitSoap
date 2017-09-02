# XitSoap
_An alternative way to do SOAP Requests without use the WSDL_

[![Quality Gate](https://sonarqube.com/api/badges/gate?key=HodStudio.XitSoap)](https://sonarqube.com/dashboard?id=HodStudio.XitSoap) [![Quality Gate](https://sonarqube.com/api/badges/measure?key=HodStudio.XitSoap&metric=code_smells)](https://sonarqube.com/dashboard/index/HodStudio.XitSoap) [![Quality Gate](https://sonarqube.com/api/badges/measure?key=HodStudio.XitSoap&metric=bugs)](https://sonarqube.com/dashboard/index/HodStudio.XitSoap) [![Quality Gate](https://sonarqube.com/api/badges/measure?key=HodStudio.XitSoap&metric=vulnerabilities)](https://sonarqube.com/dashboard/index/HodStudio.XitSoap)

In 2015, we face a very simple problem: the code generated automatically by Visual Studio to use webservices has a lot of shits. Facing this problem, we resolve to create a library that, based on you model, generates the SOAP requests, without code smells on you code.

## Download it using NuGet
```
Install-Package HodStudio.XitSoap
```
Link: https://www.nuget.org/packages/HodStudio.XitSoap
## ATTENTION
If you are using the version 1.x from our library, please, read carefully how to upgrade to version 2.x because it is a major refactory to improve some usages. It's nothing complicated, but if you just upgrade from version 1 to version 2, you code will have some problems.
### [Migrating from v1 to v2](https://github.com/HodStudio/XitSoap/wiki/0.1.-Migrating-from-v1-to-v2).

## How to use
Crete a object of _WebService_, with the base URL and the namespace that will be used.
```cs
var wsCon = new WebService("http://www.webservicex.net/ConvertSpeed.asmx", "http://www.webserviceX.NET/");
```
Add the parameters to execute you call.
```cs
wsCon.AddParameter("speed", 100D);
wsCon.AddParameter("FromUnit", "milesPerhour");
wsCon.AddParameter("ToUnit", "kilometersPerhour");
```
Invoke the method, providing the return type.
```cs
var result = wsCon.Invoke<double>("ConvertSpeed");
```
## Other situations
For more situations and examples, please, take a look on our [Wiki](https://github.com/HodStudio/XitSoap/wiki).
## Download the source code and running
To run the tests, we provide a mock project Web. It should be runned on http://localhost/XitSoap so the tests can try to connect to it. Some of the tests are runned against some published web services on the internet. Case you are not connect to it, they will fail.
