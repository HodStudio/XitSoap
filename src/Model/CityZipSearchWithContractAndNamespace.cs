namespace HodStudio.XitSoap.Tests.Model
{
    [WsContract("UsZipContract", "http://www.webserviceX.NET")]
    public class CityZipSearchWithContractAndNamespace
    {
        [WsMapper("NewDataSet")]
        public CityResultSet Result { get; set; }
    }
}
