using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HodStudio.XitSoap
{
    [AttributeUsage(AttributeTargets.All)]
    public class WsMapperAttribute : System.Attribute
    {
        public readonly string Source;

        public WsMapperAttribute(string _souce)
        {
            this.Source = _souce;
        }
    }
}
