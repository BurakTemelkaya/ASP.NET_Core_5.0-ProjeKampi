#pragma checksum "C:\Users\burak\source\repos\BurakTemelkaya\ASP.NET_Core_5.0-ProjeKampi\CoreDemo\Areas\Admin\Views\Writer\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e1cf4b4dab9f87fe8f396996bea28555eecbdf08"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_Writer_Index), @"mvc.1.0.view", @"/Areas/Admin/Views/Writer/Index.cshtml")]
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
#line 1 "C:\Users\burak\source\repos\BurakTemelkaya\ASP.NET_Core_5.0-ProjeKampi\CoreDemo\Areas\Admin\Views\_ViewImports.cshtml"
using CoreDemo;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\burak\source\repos\BurakTemelkaya\ASP.NET_Core_5.0-ProjeKampi\CoreDemo\Areas\Admin\Views\_ViewImports.cshtml"
using CoreDemo.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\burak\source\repos\BurakTemelkaya\ASP.NET_Core_5.0-ProjeKampi\CoreDemo\Areas\Admin\Views\_ViewImports.cshtml"
using CoreDemo.Areas.Admin.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\burak\source\repos\BurakTemelkaya\ASP.NET_Core_5.0-ProjeKampi\CoreDemo\Areas\Admin\Views\_ViewImports.cshtml"
using EntityLayer.Concrete;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e1cf4b4dab9f87fe8f396996bea28555eecbdf08", @"/Areas/Admin/Views/Writer/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b3e273681fe023035aba1d643f355c88c8bae0c4", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Areas_Admin_Views_Writer_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\burak\source\repos\BurakTemelkaya\ASP.NET_Core_5.0-ProjeKampi\CoreDemo\Areas\Admin\Views\Writer\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/AdminLayout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h1>Yazar Ajax ????lemleri</h1>
<br />
<button type=""button"" id=""btnGetList"" class=""btn btn-outline-primary"">Yazar Listesi</button>
<button type=""button"" id=""btnGetById"" class=""btn btn-outline-success"">Yazar Getir</button>
<button type=""button"" id=""btnAddWriter"" class=""btn btn-outline-info"">Yazar Ekle</button>
<button type=""button"" id=""btndeletewriter"" class=""btn btn-outline-danger"">Yazar Sil</button>
<button type=""button"" id=""btnupdatewriter"" class=""btn btn-outline-warning"">Yazar G??ncelle</button>
<br />
<br />
<div id=""writerList"">
</div>
<br />
<div id=""writerGet"">
</div>
<br />
<div>
    <input type=""text"" class=""form-control"" id=""writerId"" placeholder=""Yazar Id de??erini giriniz"" />
</div>
<br />
<br />
<div>
    <h1 class=""text-center"">Yazar Ekle</h1>
    <input type=""text"" id=""txtWriterId"" class=""form-control"" placeholder=""Yazar ID"" />
    <br />
    <input type=""text"" id=""txtWriterName"" class=""form-control"" placeholder=""Yazar Ad??"" />
</div>
<br />
<div>
    <h1 class=""text-c");
            WriteLiteral(@"enter"">Yazar Sil</h1>
    <input type=""text"" id=""txtid"" class=""form-control"" placeholder=""Silinecek Yazar ID"" />
</div>
<br />
<h1 class=""text-center"">Yazar G??ncelle</h1>
<input type=""text"" class=""form-control"" id=""txtUpdateWriterId"" placeholder=""G??ncellenecek ID"" />
<br />
<input type=""text"" class=""form-control"" id=""txtUpdateWriterName"" placeholder=""G??ncellenecek ??sim"" />

");
            DefineSection("scripts", async() => {
                WriteLiteral(@"
    <script>
        $(""#btnGetList"").click(function () {
            $.ajax({
                contentType: ""application/json"",
                dataType: ""json"",
                type: ""Get"",
                url: ""/Admin/Writer/WriterList"",
                success: function (func) {
                    let w = jQuery.parseJSON(func);
                    console.log(w);
                    let tablehtml = ""<table class='table table-bordered' <tr><th>Yazar ID</th><th>Yazar Ad??</th></tr> "";
                    $.each(w, (index, value) => {
                        tablehtml += `<tr><td>${value.Id}</td> <td>${value.Name}</td></tr>`
                    });
                    tablehtml += ""</table>""
                    $(""#writerList"").html(tablehtml);
                }
            })
        })
    </script>
    <script>
        $(""#btnGetById"").click(function () {
            let writerId = $(""#writerId"").val();
            console.log(writerId);
            $.ajax({
                cont");
                WriteLiteral(@"entType: ""application/json"",
                dataType: ""json"",
                type: ""Get"",
                url: ""/Admin/Writer/GetWriterByID/"",
                data: { writerId: writerId },
                success: function (func) {
                    let w = jQuery.parseJSON(func);
                    console.log(w);

                    let getValue = `<table class=""table table-bordered""> <tr> <th>Yazar Id</th> <th>Yazar Ad??</th></tr>
                            <tr><td>${w.Id}</td> <td>${w.Name}</td></tr></table>`
                    $(""#writerGet"").html(getValue);
                }
            });
        });
    </script>
    <script>
        $(""#btnAddWriter"").click(function () {
            let writer = {
                Id: $(""#txtWriterId"").val(),
                Name: $(""#txtWriterName"").val()
            };
            $.ajax({
                type: ""post"",
                url: ""/Admin/Writer/AddWriter"",
                data: writer,
                success: function (fu");
                WriteLiteral(@"nc) {
                    let result = jQuery.parseJSON(func);
                    alert(""Mail b??ltenimize abone oldunuz"");
                }
            });
        });
    </script>
    <script>
        $(""#btndeletewriter"").click(x => {
            let id = $(""#txtid"").val();
            $.ajax({
                type: ""post"",
                url: ""/Admin/Writer/DeleteWriter/"" + id,
                dataType: ""json"",
                success: function (func) {
                    alert(""Yazar Silme i??lemi ba??ar??l?? bir ??ekilde ger??ekle??ti"");
                }
            });
        });
    </script>
    <script>
        $(""#btnupdatewriter"").click(function () {
            let writer = {
                Id: $(""#txtUpdateWriterId"").val(),
                Name: $(""#txtUpdateWriterName"").val()
            };
            $.ajax({
                type: ""post"",
                url: ""/Admin/Writer/UpdateWriter"",
                data: writer,
                success: function (func) {
 ");
                WriteLiteral("                   alert(\"G??ncelleme yap??ld??\");\r\n                }\r\n            });\r\n        });\r\n    </script>\r\n");
            }
            );
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
