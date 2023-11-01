<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import namespace="Newtonsoft.Json" %>
<%@ Import namespace="Newtonsoft.Json.Linq" %>
<%@ Import namespace="Newtonsoft.Json.Bson" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Linq" %>
<%@ Import namespace="System.Reflection" %>
<%@ Import namespace="System.Web"%>
<%@ Import namespace="System.Diagnostics"%>
<%@ Import namespace="System.Web.UI"  %>
<%@ Import namespace="System.Web.UI.WebControls" %>
<%@ Import namespace="SchnapsNet.Models" %>
<%@ Import namespace="SchnapsNet.Utils" %>
<%@ Import namespace="SchnapsNet.ConstEnum" %>

    <!DOCTYPE html>

    <html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title>SchnapsNet Help</title>
            <!-- Google tag (gtag.js) -->
            <script async src="https://www.googletagmanager.com/gtag/js?id=G-01S65129V7"></script>
            <script>
                window.dataLayer = window.dataLayer || [];
                function gtag() { dataLayer.push(arguments); }
                gtag('js', new Date());

                gtag('config', 'G-01S65129V7');
            </script>
    </head>

    <script runat="server" language="C#">

        void Page_Load(object sender, EventArgs e)
        {
            // if (!Page.IsPostBack)
            Help_Click(sender, e);
        }

        public void Help_Click(object sender, EventArgs e)
        {
            preOut.InnerText += ResReader.GetValue("help_text", Paths.ISO2Lang) + "\n";
        }
        
    </script>

    <body>
        <form id="form1" runat="server">           
            <hr />
            <pre id="preOut" runat="server">
            </pre>
            <hr />
            <div align="right" style="text-align: right; background-color='#bfbfbf'; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
                <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://@arkstar.work">darkstar.work</a>
            </div>
        </form>
    </body>
    </html>
