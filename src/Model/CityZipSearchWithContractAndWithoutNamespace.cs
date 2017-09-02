namespace HodStudio.XitSoap.Tests.Model
{
    [WsContract("UsZipContract")]
    public class CityZipSearchWithContractAndWithoutNamespace
    {
        [WsMapper("NewDataSet")]
        public CityResultSet Result { get; set; }
    }
}
