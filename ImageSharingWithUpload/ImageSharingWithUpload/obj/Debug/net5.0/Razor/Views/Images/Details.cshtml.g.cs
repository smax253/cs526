#pragma checksum "C:\Users\Max\Source\Repos\cs526\ImageSharingWithUpload\ImageSharingWithUpload\Views\Images\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f866adac7a7276d2d166acadd235f205f80f393e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Images_Details), @"mvc.1.0.view", @"/Views/Images/Details.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Max\Source\Repos\cs526\ImageSharingWithUpload\ImageSharingWithUpload\Views\_ViewImports.cshtml"
using ImageSharingWithUpload;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Max\Source\Repos\cs526\ImageSharingWithUpload\ImageSharingWithUpload\Views\_ViewImports.cshtml"
using ImageSharingWithUpload.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f866adac7a7276d2d166acadd235f205f80f393e", @"/Views/Images/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e4b087b64877d1b3287cb00bec8e40a5c874c638", @"/Views/_ViewImports.cshtml")]
    public class Views_Images_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ImageSharingWithUpload.Models.Image>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Max\Source\Repos\cs526\ImageSharingWithUpload\ImageSharingWithUpload\Views\Images\Details.cshtml"
  
    ViewBag.Title = "Image Details";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h2>");
#nullable restore
#line 7 "C:\Users\Max\Source\Repos\cs526\ImageSharingWithUpload\ImageSharingWithUpload\Views\Images\Details.cshtml"
Write(ViewBag.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n\r\n<p><img");
            BeginWriteAttribute("src", " src=\"", 127, "\"", 180, 1);
#nullable restore
#line 9 "C:\Users\Max\Source\Repos\cs526\ImageSharingWithUpload\ImageSharingWithUpload\Views\Images\Details.cshtml"
WriteAttributeValue("", 133, Url.Content("~/data/images/"+@Model.Id+".jpg"), 133, 47, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" /></p>\r\n\r\n<p>Image id: ");
#nullable restore
#line 11 "C:\Users\Max\Source\Repos\cs526\ImageSharingWithUpload\ImageSharingWithUpload\Views\Images\Details.cshtml"
        Write(Model.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n\r\n<p>Caption: ");
#nullable restore
#line 13 "C:\Users\Max\Source\Repos\cs526\ImageSharingWithUpload\ImageSharingWithUpload\Views\Images\Details.cshtml"
       Write(Model.Caption);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n\r\n<p>Description: <br />");
#nullable restore
#line 15 "C:\Users\Max\Source\Repos\cs526\ImageSharingWithUpload\ImageSharingWithUpload\Views\Images\Details.cshtml"
                 Write(Model.Description);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n\r\n<p>Date taken: ");
#nullable restore
#line 17 "C:\Users\Max\Source\Repos\cs526\ImageSharingWithUpload\ImageSharingWithUpload\Views\Images\Details.cshtml"
          Write(Model.DateTaken.ToShortDateString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n\r\n<p>Uploader: ");
#nullable restore
#line 19 "C:\Users\Max\Source\Repos\cs526\ImageSharingWithUpload\ImageSharingWithUpload\Views\Images\Details.cshtml"
        Write(Model.Userid);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ImageSharingWithUpload.Models.Image> Html { get; private set; }
    }
}
#pragma warning restore 1591
