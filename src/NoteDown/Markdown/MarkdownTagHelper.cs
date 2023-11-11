using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NoteDown.Markdown
{
    [HtmlTargetElement("markdown")]
    public class MarkdownTagHelper : TagHelper
    {
        // créditos:
        // https://www.codemag.com/Article/1811071/Marking-up-the-Web-with-ASP.NET-Core-and-Markdown
        [HtmlAttributeName("normalize-whitespace")]
        public bool NormalizeWhitespace { get; set; } = true;

        [HtmlAttributeName("markdown")]
        public ModelExpression Markdown { get; set; }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            string content = null;
            if (Markdown != null)
                content = Markdown.Model?.ToString();

            if (content == null)
                content = (await output.GetChildContentAsync(NullHtmlEncoder.Default)).GetContent(NullHtmlEncoder.Default);

            if (string.IsNullOrEmpty(content))
                return;

            content = content.Trim('\n', '\r');

            var html = NoteDown.Markdown.Markdown.Parse(content);

            output.TagName = null;  // Remove the <markdown> element
            output.Content.SetHtmlContent(html);
        }
    }
}
