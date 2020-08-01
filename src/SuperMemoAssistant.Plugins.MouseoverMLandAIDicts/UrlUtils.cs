using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.MouseoverMLandAIDicts
{

  public static class UrlUtils
  {

    public const string TheMLDictRegex = @"^https?\:\/\/(?:www\.)?cse\.unsw\.edu\.au\/\~billw\/mldict\.html(#\w+)?";
    public const string TheAIDictRegex = @"^https?\:\/\/(?:www\.)?cse\.unsw\.edu\.au\/\~billw\/aidict\.html(#\w+)?";
    public const string TheNLPDictRegex = @"^https?\:\/\/(?:www\.)?cse\.unsw\.edu\.au\/\~billw\/nlpdict\.html(#\w+)?";
    public const string GoogleMLGlossRegex = @"^https?\:\/\/(?:www\.)?developers\.google\.com\/machine-learning\/glossary(#[\w-]+)?";

    public static string ConvRelToAbsLink(string baseUrl, string relUrl)
    {
      if (!string.IsNullOrEmpty(baseUrl) && !string.IsNullOrEmpty(relUrl))
      {
        // UriKind.Relative will be false for rel urls containing #
        if (Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
        {
          if (baseUrl.EndsWith("/"))
          {
            baseUrl = baseUrl.TrimEnd('/');
          }

          return $"{baseUrl}/{relUrl}";
        }
      }
      return relUrl;
    }
  }
}
