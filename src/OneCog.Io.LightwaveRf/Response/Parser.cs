using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OneCog.Io.LightwaveRf.Response
{
    public interface IParser
    {
        IEnumerable<IResponse> Parse(string response);
    }

    internal class Parser : IParser
    {
        private static readonly string IdGroupName = "id";
        private static readonly string StatusGroupName = "status";
        private static readonly string OkStatus = "ok";
        private static readonly string ResponseRegexPattern = $"(?<{IdGroupName}>\\d{3}),(?<{StatusGroupName}>\\w+)";
        private static readonly Regex ResponseRegex = new Regex(ResponseRegexPattern);

        public IEnumerable<IResponse> Parse(string response)
        {
            Match match = ResponseRegex.Match(response);
            uint id;

            if (match.Success && match.Groups[IdGroupName].Success && match.Groups[StatusGroupName].Success && uint.TryParse(match.Groups[IdGroupName].Value, out id))
            {
                return (match.Groups[StatusGroupName].Value.Equals(OkStatus, StringComparison.OrdinalIgnoreCase))
                    ? new IResponse[] { new Success(id) }
                    : new IResponse[] { new Failure(id, match.Groups[StatusGroupName].Value) };
            }
            else
            {
                return Enumerable.Empty<IResponse>();
            }
        }
    }
}
