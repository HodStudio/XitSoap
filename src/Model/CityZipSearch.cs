using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HodStudio.XitSoap.Tests.Model
{
    public class CityZipSearch
    {
        [WsMapper("NewDataSet")]
        public CityResultSet Result { get; set; }
    }

    public class CityResultSet
    {
        [WsMapper("Table")]
        public CityInfo City { get; set; }
    }

    public class CityInfo
    {
        [WsMapper("CITY")]
        public string Name { get; set; }
        [WsMapper("STATE")]
        public string State { get; set; }
        [WsMapper("ZIP")]
        public int ZipCode { get; set; }
        [WsMapper("AREA_CODE")]
        public int AreaCode { get; set; }
        [WsMapper("TIME_ZONE")]
        public string TimeZone { get; set; }
    }
}
