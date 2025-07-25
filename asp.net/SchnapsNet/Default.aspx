<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Drawing" %>
<%@ Import namespace="System.Linq" %>
<%@ Import namespace="System.Reflection" %>
<%@ Import namespace="System.Web"%>
<%@ Import namespace="System.Diagnostics"%>
<%@ Import namespace="System.Web.UI"  %>
<%@ Import namespace="System.Web.UI.WebControls" %>
<%@ Import Namespace="SchnapsNet" %>
<%@ Import Namespace="SchnapsNet.Models" %>
<%@ Import Namespace="SchnapsNet.ConstEnum" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Schnaps.Net</title>
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
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("SchnapsNet.aspx");
    }

</script>

<body>
    <form id="form1" runat="server">                
        <div align="left" style="text-align: left; width: 100%; height: 8%; visibility: inherit; background-color: #bfbfbf; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
            <a href="mailto:zen@area23.at>Heinrich Elsigan</a>, GNU General Public License 3.0, [<a href="https://area23-at.blogspot.com/">blog.</a>]<a href="https://area23.at">area23.at</a>
        </div>    
    </form>
</body>
</html>
