// using Miru.Html;
// using Miru.UI;
//
// namespace Corpo.Skeleton.Config;
//
// public class HtmlConfig : HtmlConfiguration
// {
//     public HtmlConfig()
//     {
//         this.AddTwitterBootstrap();
//         this.AddMiruForm();
//         this.AddMiruFormSummary();
//         this.AddRequiredLabels();
//         
//         // Tables.Always.ModifyWith(tag => tag.CurrentTag.AddClass("table-responsive"));
//         
//         Tables.Always.ModifyWith(tag =>
//         {
//             var wrapper = new HtmlTag("div").AddClass("table-responsive");
//             // var table = tag.CurrentTag;
//             //
//             // tag.CurrentTag.
//             tag.CurrentTag.AddClass("table table-striped table-hover");
//             //
//             tag.CurrentTag.WrapWith(wrapper);
//         });
//     }
// }