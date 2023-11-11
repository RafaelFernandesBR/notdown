using Microsoft.AspNetCore.Html;
using Westwind.AspNetCore.Markdown;

namespace NoteDown.Markdown
{
    public static class Markdown
    {
        public static string Parse(string markdown, bool usePragmaLines = false, bool forceReload = false)
        {
            if (string.IsNullOrEmpty(markdown))
                return "";

            var factory = new MarkdigMarkdownParserFactory();
            var parser = factory.GetParser(usePragmaLines, forceReload);

            return parser.Parse(markdown);
        }

        public static HtmlString ParseHtmlString(string markdown, bool usePragmaLines = false, bool forceReload = false)
        {
            return new HtmlString(Parse(markdown, usePragmaLines, forceReload));
        }
    }
}
