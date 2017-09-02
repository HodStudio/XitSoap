using System.Collections.Generic;

namespace HodStudio.XitSoap.Tests.Model
{
    public class ApiResult
    {
        /// <summary>
        /// Error result. Returns "1" in case of success. Return "0" in case of error.
        /// </summary>
        public int ReturnCode { get; set; } = 1;
        /// <summary>
        /// Errors list. It will only have data if ReturnCode equals 0.
        /// </summary>
        public List<ApiError> Errors { get; set; } = new List<ApiError>();
    }
}
