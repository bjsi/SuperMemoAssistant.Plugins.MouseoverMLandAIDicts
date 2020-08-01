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
  using SuperMemoAssistant.Services.Sentry;

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

    private const string TheMLDictRegex = @"^https?\:\/\/(?:www\.)?cse\.unsw\.edu\.au\/\~billw\/mldict\.html(#\w+)?";
    private const string TheAIDictRegex = @"^https?\:\/\/(?:www\.)?cse\.unsw\.edu\.au\/\~billw\/aidict\.html(#\w+)?";
    private const string TheNLPDictRegex = @"^https?\:\/\/(?:www\.)?cse\.unsw\.edu\.au\/\~billw\/nlpdict\.html(#\w+)?";
    private const string GoogleMLGlossRegex = @"^https?\:\/\/(?:www\.)?developers\.google\.com\/machine-learning\/glossary(#[\w-]+)?";

    #endregion

    #region Methods Impl

    /// <inheritdoc />
    protected override void PluginInit()
    {

    }

    private void RegisterTheNLPDictProvider()
    {

    }

    private void RegisterTheAIDictProvider()
    {

    }

    private void RegisterTheMLDictProvider()
    {

    }

    private void RegisterGoogleMLGlossProvider()
    {
      var referenceRegexes = new ReferenceRegexes(TitleRegexes, AuthorRegexes, LinkRegexes, SourceRegexes);
      KeywordScanningOptions opts = new KeywordScanningOptions(referenceRegexes, Keywords.HelpKeywordMap, MapType.URL, CategoryPathRegexes);

      // Register with MouseoverPopup
      if (!this.RegisterProvider(ProviderName + " Guru", new string[] { UrlUtils.GuruGlossaryRegex }, opts, _guruContentProvider))
      {
        LogTo.Error($"Failed to Register provider {ProviderName} with MouseoverPopup Service");
        return;
      }

      LogTo.Debug($"Successfully registered provider {ProviderName} with MouseoverPopup Service");

    }

    // /// <inheritdoc />
    public override void ShowSettings()
    {
    }

    #endregion

    #region Methods

    #endregion
  }
}
