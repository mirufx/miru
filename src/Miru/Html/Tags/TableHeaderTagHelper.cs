using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-th2")]
[HtmlTargetElement("miru-th2", Attributes = "for")]
[HtmlTargetElement("miru-th2", Attributes = "model")]
public class TableHeaderTagHelper : MiruTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlAttr.Th;
        output.TagMode = TagMode.StartTagAndEndTag;

        TagModifier.TableHeaderFor(ElementRequest.Create(this), output);
    }
}