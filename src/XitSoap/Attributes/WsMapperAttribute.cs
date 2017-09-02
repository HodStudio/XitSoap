using System;

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
