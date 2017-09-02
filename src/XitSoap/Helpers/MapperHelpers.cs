using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HodStudio.XitSoap.Helpers
{
    internal static class MapperHelpers
    {
        private static void GetMapperAttributes(this IDictionary<string, string> mappers, MemberInfo info)
        {
            var ca = ((WsMapperAttribute[])info.GetCustomAttributes(typeof(WsMapperAttribute), true)).ToList();
            ca.ForEach(x => mappers.Add(x.Source, info.Name));
        }

        internal static void GetMapperAttributes(this Dictionary<string, string> mappers, Type type)
        {
            mappers.GetMapperAttributes((MemberInfo)type);

            var props = type.GetProperties();
            foreach (var item in props)
                GetMapperAttributes(mappers, item);
        }
        internal static void GetMapperAttributes(this Dictionary<string, string> mappers, PropertyInfo prop)
        {
            mappers.GetMapperAttributes((MemberInfo)prop);

            Type type = prop.PropertyType;
            if (type.GenericTypeArguments.Length > 0)
                foreach (var item in type.GenericTypeArguments)
                    GetMapperAttributes(mappers, item);
            else if (!type.IsPrimitive && type != typeof(string) && type != typeof(DateTime))
                GetMapperAttributes(mappers, type);
        }

        internal static string ApplyMappers(this Dictionary<string, string> mappers, string input)
        {
            var inputSb = new StringBuilder(input);
            foreach (var item in mappers)
                inputSb.ReplaceMapper(item.Key, item.Value);
            return inputSb.ToString();
        }
        internal static StringBuilder ApplyMappers(this Dictionary<string, string> mappers, StringBuilder input)
        {
            foreach (var item in mappers)
                input.ReplaceMapper(item.Value, item.Key);
            return input;
        }

        private static StringBuilder ReplaceMapper(this StringBuilder input, string source, string destiny)
        {
            input = input.Replace($"<{source}>", $"<{destiny}>");
            input = input.Replace($"</{source}>", $"</{destiny}>");
            input = input.Replace($"<{source}/>", $"<{destiny}/>");
            return input;
        }
    }
}
