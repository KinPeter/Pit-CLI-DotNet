using System;
using System.Net;
using System.Text;
using Pit.Config;
using Pit.Http;

namespace Pit.Jira
{
    public static class JiraUtils
    {
        public static PitHeader ConstructAuthHeader(JiraProject config)
        {
            var stringToEncode = $"{config.User}:{config.ApiToken}";
            byte[] stringAsBytes = Encoding.UTF8.GetBytes(stringToEncode);
            return new PitHeader(
                HttpRequestHeader.Authorization.ToString(),
                $"Basic {Convert.ToBase64String(stringAsBytes)}"
            );
        }
    }
}