# XitSoap

<img align="left" width="150" height="150" src="https://github.com/HodStudio/XitSoap/blob/master/xitSoap.png" margin="0,0,10,0">

_An alternative way to do SOAP Requests without use the WSDL_

[![Quality Gate](https://sonarqube.com/api/badges/gate?key=HodStudio.XitSoap)](https://sonarqube.com/dashboard?id=HodStudio.XitSoap) [![Quality Gate](https://sonarqube.com/api/badges/measure?key=HodStudio.XitSoap&metric=code_smells)](https://sonarqube.com/dashboard/index/HodStudio.XitSoap) [![Quality Gate](https://sonarqube.com/api/badges/measure?key=HodStudio.XitSoap&metric=bugs)](https://sonarqube.com/dashboard/index/HodStudio.XitSoap) [![Quality Gate](https://sonarqube.com/api/badges/measure?key=HodStudio.XitSoap&metric=vulnerabilities)](https://sonarqube.com/dashboard/index/HodStudio.XitSoap) [![Quality Gate](https://sonarqube.com/api/badges/measure?key=HodStudio.XitSoap&metric=duplicated_lines_density)](https://sonarqube.com/dashboard/index/HodStudio.XitSoap)



In 2015, we face a very simple problem: the automatically generated code by Visual Studio to use webservices has a lot of code smells. Facing this problem, we created a library that generates the SOAP requests based on your models, without code smells in your code.

## Download it using NuGet
```
Install-Package HodStudio.XitSoap
```
Link: https://www.nuget.org/packages/HodStudio.XitSoap
## ATTENTION
If you are using the version 1.x from our library, please, read carefully how to upgrade to version 2.x because it is a major refactor to improve some usages. It's nothing complicated, but if you just upgrade from version 1 to version 2, your code will have some problems.
### [Migrating from v1 to v2](https://github.com/HodStudio/XitSoap/wiki/Migrating-from-v1-to-v2).

## How to use
Create an object of _WebService_, with the base URL and the namespace that will be used.
```cs
var wsCon = new WebService("http://www.webservicex.net/ConvertSpeed.asmx", "http://www.webserviceX.NET/");
```
Add the parameters to execute your call.
```cs
wsCon.AddParameter("speed", 100D);
wsCon.AddParameter("FromUnit", "milesPerhour");
wsCon.AddParameter("ToUnit", "kilometersPerhour");
```
Invoke the method, providing the return type.
```cs
var result = wsCon.Invoke<double>("ConvertSpeed");
```
## Documentation
For more situations and examples, please, take a look on our Documentation on the [Wiki](https://github.com/HodStudio/XitSoap/wiki).

To see the planned new features, take a look on our [RoadMap](https://github.com/HodStudio/XitSoap/wiki#road-map-in-eternal-construction).
## Download the source code and running
To run the tests, we provide a mock project _Web_. It should be runned on http://localhost/XitSoap so the tests can try to connect to it. Some of the tests are runned against published web services on the internet. In case you are not connected to the internet, they will fail.
