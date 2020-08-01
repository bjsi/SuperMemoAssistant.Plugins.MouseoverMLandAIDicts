using Forge.Forms.Annotations;
using Newtonsoft.Json;
using SuperMemoAssistant.Services.UI.Configuration;
using SuperMemoAssistant.Sys.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.MouseoverMLandAIDicts
{

  [Form(Mode = DefaultFields.None)]
  [Title("Dictionary Settings",
     IsVisible = "{Env DialogHostContext}")]
  [DialogAction("cancel",
    "Cancel",
    IsCancel = true)]
  [DialogAction("save",
    "Save",
    IsDefault = true,
    Validates = true)]
  public class MouseoverMLandAIDictCfg   : CfgBase<MouseoverMLandAIDictCfg>, INotifyPropertyChangedEx
  {
    [Title("Mouseover Machine Learning and AI Dictionaries Plugin")]

    [Heading("By Jamesb | Experimental Learning")]

    [Heading("Features:")]
    [Text(@"- Open glossary definitions for Machine Learning and AI topics in a popup window
- Supports glossary definitions from ")]

    [Heading("General Settings")]

    [Field(Name = "Scan SM-related articles for glossary terms?")]
    public bool ScanElements { get; set; } = true;

    [Heading("Keyword Scanning Settings")]

    [Heading("Reference Regexes")]
    [Field(Name = "Title Regexes")]
    [MultiLine]
    public string ReferenceTitleRegexes { get; set; }

    [Field(Name = "Author Regexes")]
    [MultiLine]
    public string ReferenceAuthorRegexes { get; set; } = @"
piotr wozniak
woz";

    [Field(Name = "Source Regexes")]
    [MultiLine]
    public string ReferenceSourceRegexes { get; set; } = @"
.*supermemo.*
.*super-memo.*
.*SM.*";

    [Field(Name = "Link Regexes")]
    public string ReferenceLinkRegexes { get; set; } = @"
.*supermemo.*";

    [Field(Name = "Concept Regexes")]
    [MultiLine]
    public string ConceptNameRegexes { get; set; }

    [JsonIgnore]
    public bool IsChanged { get; set; }

    public override string ToString()
    {
      return "Mouseover Glossary Settings";
    }

    public event PropertyChangedEventHandler PropertyChanged;

  }
}
