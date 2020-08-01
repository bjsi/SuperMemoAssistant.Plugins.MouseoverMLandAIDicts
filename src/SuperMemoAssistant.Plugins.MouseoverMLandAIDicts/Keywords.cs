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

    // Json File Paths
    private const string TheDictJson = @"C:\Users\james\SuperMemoAssistant\Plugins\Development\SuperMemoAssistant.Plugins.MouseoverMLandAIDicts\dictionaries\google_keyword_url_map.json";
    private const string GoogleMLGlossJson = @"C:\Users\james\SuperMemoAssistant\Plugins\Development\SuperMemoAssistant.Plugins.MouseoverMLandAIDicts\dictionaries\the_dict_keyword_url_map.json";

    // Keyword -> Url maps
    public static Dictionary<string, string> TheDictKeywords => CreateJsonDict(TheDictJson);
    public static Dictionary<string, string> GoogleKeywords => CreateJsonDict(GoogleMLGlossJson);

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
