#region License & Metadata

// The MIT License (MIT)
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// 
// 
// Created On:   8/1/2020 3:42:18 AM
// Modified By:  james

#endregion




namespace SuperMemoAssistant.Plugins.MouseoverMLandAIDicts
{
  using System.Diagnostics.CodeAnalysis;
  using Anotar.Serilog;
  using MouseoverPopup.Interop;
  using SuperMemoAssistant.Services;
  using SuperMemoAssistant.Services.IO.HotKeys;
  using SuperMemoAssistant.Services.Sentry;
  using SuperMemoAssistant.Services.UI.Configuration;

  // ReSharper disable once UnusedMember.Global
  // ReSharper disable once ClassNeverInstantiated.Global
  [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
  public class MouseoverMLandAIDictsPlugin : SentrySMAPluginBase<MouseoverMLandAIDictsPlugin>
  {
    #region Constructors

    /// <inheritdoc />
    public MouseoverMLandAIDictsPlugin() : base("Enter your Sentry.io api key (strongly recommended)") { }

    #endregion

    #region Properties Impl - Public

    /// <inheritdoc />
    public override string Name => "MouseoverMLandAIDicts";

    /// <inheritdoc />
    public override bool HasSettings => false;

    public MouseoverMLandAIDictCfg Config;

    // Services
    private GoogleContentService _googleContentService = new GoogleContentService();
    private TheDictContentService _theDictContentService = new TheDictContentService();

    // Reference Regexes
    private string[] TitleRegexes => Config.ReferenceTitleRegexes?.Replace("\r\n", "\n")?.Split('\n');
    private string[] AuthorRegexes => Config.ReferenceAuthorRegexes?.Replace("\r\n", "\n")?.Split('\n');
    private string[] LinkRegexes => Config.ReferenceLinkRegexes?.Replace("\r\n", "\n")?.Split('\n');
    private string[] SourceRegexes => Config.ReferenceSourceRegexes?.Replace("\r\n", "\n")?.Split('\n');

    // Category Path Regexes
    private string[] CategoryPathRegexes => Config.ConceptNameRegexes?.Replace("\r\n", "\n")?.Split('\n');

    #endregion

    #region Methods Impl

    private void LoadConfig()
    {
      Config = Svc.Configuration.Load<MouseoverMLandAIDictCfg>() ?? new MouseoverMLandAIDictCfg();
    }

    /// <inheritdoc />
    protected override void PluginInit()
    {

      LoadConfig();

      // Register providers
      RegisterGoogleMLGlossProvider();
      RegisterTheDictProvider();

    }

    // /// <inheritdoc
    public override void ShowSettings()
    {
      ConfigurationWindow.ShowAndActivate(HotKeyManager.Instance, Config);
    }

    private void RegisterTheDictProvider()
    {

      var referenceRegexes = new ReferenceRegexes(TitleRegexes, AuthorRegexes, LinkRegexes, SourceRegexes);
      KeywordScanningOptions opts = new KeywordScanningOptions(referenceRegexes, Keywords.TheDictKeywords, MapType.URL, CategoryPathRegexes);

      // Register with MouseoverPopup
      if (!this.RegisterProvider(Name + " The Dict", new string[] { UrlUtils.TheAIDictRegex, UrlUtils.TheMLDictRegex, UrlUtils.TheNLPDictRegex }, opts, _theDictContentService))
      {
        LogTo.Error($"Failed to Register provider {Name + " The Dict"} with MouseoverPopup Service");
        return;
      }

      LogTo.Debug($"Successfully registered provider {Name + " The Dict"} with MouseoverPopup Service");


    }

    private void RegisterGoogleMLGlossProvider()
    {
      var referenceRegexes = new ReferenceRegexes(TitleRegexes, AuthorRegexes, LinkRegexes, SourceRegexes);
      KeywordScanningOptions opts = new KeywordScanningOptions(referenceRegexes, Keywords.GoogleKeywords, MapType.URL, CategoryPathRegexes);

      // Register with MouseoverPopup
      if (!this.RegisterProvider(Name + " Google", new string[] { UrlUtils.GoogleMLGlossRegex }, opts, _googleContentService))
      {
        LogTo.Error($"Failed to Register provider {Name + " Google"} with MouseoverPopup Service");
        return;
      }

      LogTo.Debug($"Successfully registered provider {Name + " Google"} with MouseoverPopup Service");

    }

    #endregion

    #region Methods

    #endregion
  }
}
