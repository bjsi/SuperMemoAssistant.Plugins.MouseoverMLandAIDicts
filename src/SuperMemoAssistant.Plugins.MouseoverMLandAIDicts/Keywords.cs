using Anotar.Serilog;
using SuperMemoAssistant.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.MouseoverMLandAIDicts
{
  public static class Keywords
  {

    private const string TheMLDictJson = @"keyword_url_map_1.json";
    private const string TheNLPDictJson = @"keyword_url_map_2.json";
    private const string TheAIDictJson = @"keyword_url_map_2.json";

    private const string GoogleMLGlossJson = @"";

    public static Dictionary<string, string> TheMLDictKeywords => CreateJsonDict(TheMLDictJson);
    public static Dictionary<string, string> TheAIDictKeywords => CreateJsonDict(TheAIDictJson);
    public static Dictionary<string, string> TheNLPDictKeywords => CreateJsonDict(TheNLPDictJson);

    private static Dictionary<string, string> CreateJsonDict(string jsonPath)
    {

      try
      {

        using (StreamReader r = new StreamReader(jsonPath))
        {

          string json = r.ReadToEnd();
          return json.Deserialize<Dictionary<string, string>>();

        }
      }
      catch (FileNotFoundException)
      {

        LogTo.Error($"Failed to CreateKeywordMap because {jsonPath} does not exist");
        return null;

      }
      catch (IOException e)
      {

        LogTo.Error($"Exception {e} thrown when attempting to read from {jsonPath}");
        return null;

      }
    }
  }
}
