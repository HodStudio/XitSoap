using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HodStudio.XitSoap.Helpers
{
    internal static class MapperHelpers
    {
        internal static void GetMapperAttributes(this WebService service, Type type)
        {
            MemberInfo info = type;
            var ca = ((WsMapperAttribute[])info.GetCustomAttributes(typeof(WsMapperAttribute), true)).ToList();
            ca.ForEach(x => service.mappers.Add(x.Source, type.Name));

            var props = type.GetProperties();
            foreach (var item in props)
            {
                GetMapperAttributes(service, item);
            }
        }
        internal static void GetMapperAttributes(this WebService service, PropertyInfo prop)
        {
            var ca = ((WsMapperAttribute[])prop.GetCustomAttributes(typeof(WsMapperAttribute), true)).ToList();
            ca.ForEach(x => service.mappers.Add(x.Source, prop.Name));
            Type type = prop.PropertyType;
            if (type.GenericTypeArguments.Length > 0)
            {
                foreach (var item in type.GenericTypeArguments)
                {
                    GetMapperAttributes(service, item);
                }
            }
            else if (!type.IsPrimitive && type != typeof(string) && type != typeof(DateTime))
            {
                GetMapperAttributes(service, type);
            }
        }

        internal static string ApplyMappers(this WebService service, string input)
        {
            var ret = input;
            foreach (var item in service.mappers)
            {
                ret = ret.Replace($"<{item.Key}>", $"<{item.Value}>");
                ret = ret.Replace($"</{item.Key}>", $"</{item.Value}>");
                ret = ret.Replace($"<{item.Key}/>", $"<{item.Value}/>");
            }
            return ret;
        }
        internal static StringBuilder ApplyMappersInput(this WebService service, StringBuilder input)
        {
            var ret = input;
            foreach (var item in service.mappers)
            {
                ret = ret.Replace($"<{item.Value}>", $"<{item.Key}>");
                ret = ret.Replace($"</{item.Value}>", $"</{item.Key}>");
                ret = ret.Replace($"<{item.Value}/>", $"<{item.Key}/>");
            }
            return ret;
        }
    }
}
