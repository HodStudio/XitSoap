using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HodStudio.XitSoap.Tests.Model
{
    [ExcludeFromCodeCoverage]
    public class ApiError
    {
        public ApiError() { }
        public ApiError(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; set; }
        public string Description { get; set; }
    }
}
