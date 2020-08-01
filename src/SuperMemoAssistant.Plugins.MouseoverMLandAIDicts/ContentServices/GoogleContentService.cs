using Anotar.Serilog;
using HtmlAgilityPack;
using MouseoverPopup.Interop;
using SuperMemoAssistant.Interop.SuperMemo.Elements.Builders;
using SuperMemoAssistant.Plugins.MouseoverMLandAIDicts.ContentServices;
using SuperMemoAssistant.Sys.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.MouseoverMLandAIDicts
{

  [Serializable]
  public class GoogleContentService : ContentServiceBase, IMouseoverContentProvider
  {

    private HtmlDocument CachedHtmlDoc { get; set; }

    public RemoteTask<PopupContent> FetchHtml(RemoteCancellationToken ct, string url)
    {

      try
      {

        if (url.IsNullOrEmpty() || ct.IsNull())
          return null;

        Match match = new Regex(UrlUtils.GoogleMLGlossRegex).Match(url);

        if (!match.Success)
          return null;

        string term = match.Groups[1].Value;
         if (term.IsNullOrEmpty())
          return null;

        return GetGuruGlossaryItem(ct, url, term);

      }
      catch (TaskCanceledException) { }
      catch (Exception ex)
      {
        LogTo.Error($"Failed to FetchHtml for url {url} with exception {ex}");
        throw;
      }

      return null;

    }

    private async Task<PopupContent> GetGuruGlossaryItem(RemoteCancellationToken ct, string url, string term)
    {

      if (CachedHtmlDoc.IsNull())
      {
        string response = await GetAsync(ct.Token(), url).ConfigureAwait(false);
        if (response.IsNullOrEmpty())
          return null;

        var doc = new HtmlDocument();
        doc.LoadHtml(response);
        CachedHtmlDoc = doc.ConvRelToAbsLinks("https://developers.google.com/machine-learning/glossary");

        return CreateGuruGlossaryContent(url, term);

      }

      return await Task.FromResult(CreateGuruGlossaryContent(url, term)).ConfigureAwait(false);

    }

    private PopupContent CreateGuruGlossaryContent(string url, string term)
    {

      if (url.IsNullOrEmpty() || term.IsNullOrEmpty())
        return null;

      var titleNode = CachedHtmlDoc.DocumentNode.SelectSingleNode("//" + term.Substring(1));
      if (titleNode.IsNull())
        return null;

      string content = string.Empty;

      var cur = titleNode;
      while (!cur.IsNull() && cur.Name.ToLower() != "h2")
      {
        content += cur.OuterHtml;
        cur = cur.NextSibling;
      }

      if (content.IsNullOrEmpty())
        return null;

      string html = @"
          <html>
            <body>
              <p>{0}</p>
            </body>
          </html>";

      html = string.Format(html, content);

      var refs = new References();
      refs.Title = titleNode.InnerText;
      refs.Link = url;
      refs.Source = "Google Machine Learning Glossary";

      return new PopupContent(refs, html, true, browserQuery: url);

    }
  }
}
