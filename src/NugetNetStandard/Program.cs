using HodStudio.XitSoap;
using System;

namespace NugetNetStandard
{
    class Program
    {
        static void Main(string[] args)
        {
            var wsCon = new WebService("http://www.webservicex.net/ConvertSpeed.asmx", "http://www.webserviceX.NET/");
            wsCon.AddParameter("speed", 100D);
            wsCon.AddParameter("FromUnit", "milesPerhour");
            wsCon.AddParameter("ToUnit", "kilometersPerhour");
            var result = wsCon.Invoke<double>("ConvertSpeed");
            Console.WriteLine(result);

            if (!result.Equals(160.93470878864446D))
                throw new Exception("Error");

            Console.ReadKey();
        }
    }
}
