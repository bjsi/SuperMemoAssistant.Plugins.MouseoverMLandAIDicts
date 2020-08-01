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
  public class TheDictContentService : ContentServiceBase, IMouseoverContentProvider
  {

    private HtmlDocument CachedAIHtmlDoc { get; set; }
    private HtmlDocument CachedMLHtmlDoc { get; set; }
    private HtmlDocument CachedNLPHtmlDoc { get; set; }

    public RemoteTask<PopupContent> FetchHtml(RemoteCancellationToken ct, string url)
    {

      try
      {

        if (url.IsNullOrEmpty() || ct.IsNull())
          return null;

        Match ai = new Regex(UrlUtils.TheAIDictRegex).Match(url);
        Match ml = new Regex(UrlUtils.TheMLDictRegex).Match(url);
        Match nlp = new Regex(UrlUtils.TheNLPDictRegex).Match(url);

        if (ai.Success)
          return GetAIDictItem(ct, url, ai);
        else if (ml.Success)
          return GetMLDictItem(ct, url, ml);
        else if (nlp.Success)
          return GetNLPDictItem(ct, url, nlp);

        return null;

      }
      catch (TaskCanceledException) { }
      catch (Exception ex)
      {
        LogTo.Error($"Failed to FetchHtml for url {url} with exception {ex}");
        throw;
      }

      return null;

    }

    private async Task<PopupContent> GetAIDictItem(RemoteCancellationToken ct, string url, Match match)
    {

      if (url.IsNullOrEmpty() || ct.IsNull() || match.IsNull())
        return null;

      string term = match.Groups[1].Value;
      if (term.IsNullOrEmpty())
        return null;

      if (CachedAIHtmlDoc.IsNull())
      {

        string response = await GetAsync(ct.Token(), url).ConfigureAwait(false);
        if (response.IsNullOrEmpty())
          return null;

        var doc = new HtmlDocument();
        doc.LoadHtml(response);
        CachedAIHtmlDoc = doc.ConvRelToAbsLinks("http://www.cse.unsw.edu.au/~billw/aidict.html");

      }

      return CreateGuruGlossaryContent(url, term, CachedAIHtmlDoc, "The Artificial Intelligence Dictionary");

    }

    private async Task<PopupContent> GetMLDictItem(RemoteCancellationToken ct, string url, Match match)
    {

      if (url.IsNullOrEmpty() || ct.IsNull() || match.IsNull())
        return null;

      string term = match.Groups[1].Value;
      if (term.IsNullOrEmpty())
        return null;

      if (CachedMLHtmlDoc.IsNull())
      {

        string response = await GetAsync(ct.Token(), url).ConfigureAwait(false);
        if (response.IsNullOrEmpty())
          return null;

        var doc = new HtmlDocument();
        doc.LoadHtml(response);
        CachedMLHtmlDoc = doc.ConvRelToAbsLinks("http://www.cse.unsw.edu.au/~billw/mldict.html");

      }

      return CreateGuruGlossaryContent(url, term, CachedMLHtmlDoc, "The Machine Learning Dictionary");

    }

    private async Task<PopupContent> GetNLPDictItem(RemoteCancellationToken ct, string url, Match match)
    {
      if (url.IsNullOrEmpty() || ct.IsNull() || match.IsNull())
        return null;

      string term = match.Groups[1].Value;
      if (term.IsNullOrEmpty())
        return null;

      if (CachedNLPHtmlDoc.IsNull())
      { 
      
        string response = await GetAsync(ct.Token(), url).ConfigureAwait(false);
        if (response.IsNullOrEmpty())
          return null;

        var doc = new HtmlDocument();
        doc.LoadHtml(response);
        CachedNLPHtmlDoc = doc.ConvRelToAbsLinks("http://www.cse.unsw.edu.au/~billw/nlpdict.html");

      }

      return CreateGuruGlossaryContent(url, term, CachedNLPHtmlDoc, "The Natural Language Processing Dictionary");

    }

    private PopupContent CreateGuruGlossaryContent(string url, string term, HtmlDocument doc, string source)
    {

      if (url.IsNullOrEmpty() || term.IsNullOrEmpty())
        return null;

      var dls = doc.DocumentNode.SelectNodes("//dl");

      HtmlNode titleNode = null;
      HtmlNode contentNode = null;

      foreach (var dl in dls)
      {
        var a = dl
          .SelectNodes("//a")
          .Where(x => x.GetAttributeValue("name", null) == term.Substring(1))
          .FirstOrDefault();

        if (a.IsNull())
          continue;

        titleNode = a.SelectSingleNode("//dt");
        contentNode = a.NextSibling;

      }

      if (titleNode.IsNull() || contentNode.IsNull())
        return null;

      string title = titleNode.OuterHtml;
      string definition = contentNode.OuterHtml;

      if (title.IsNullOrEmpty() || definition.IsNullOrEmpty())
        return null;

      string html = @"
          <html>
            <body>
              <h1>{0}</h1>
              <p>{1}</p>
            </body>
          </html>";

      html = string.Format(html, title, definition);

      var refs = new References();
      refs.Title = titleNode.InnerText;
      refs.Author = "Bill Wilson";
      refs.Link = url;
      refs.Source = source;

      return new PopupContent(refs, html, true, browserQuery: url, editUrl: $"https://supermemo.guru/index.php?title={term}&action=edit");

    }
  }
}
