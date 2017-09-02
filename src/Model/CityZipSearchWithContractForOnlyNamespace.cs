namespace HodStudio.XitSoap.Tests.Model
{
    [WsContract("UsZipContractWithoutUrl", "http://www.webserviceX.NET")]
    public class CityZipSearchWithContractForOnlyNamespace
    {
        [WsMapper("NewDataSet")]
        public CityResultSet Result { get; set; }
    }
}
