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
        <link rel="stylesheet" href="res/schnapsennet.css" />
    </head>

    <script runat="server" language="C#">

        void Page_Load(object sender, EventArgs e)
        {
            // if (!Page.IsPostBack)
            Help_Click(sender, e);
            
        }

        public void Help_Click(object sender, EventArgs e)
        {
            schnapsDiv.InnerHtml = ResReader.GetRes("help_text", Paths.Locale) + "\n";
            GlobalAppSettings globalVariable = (GlobalAppSettings)this.Context.Session[Constants.APPNAME];
            if (globalVariable != null && globalVariable.InnerPreText != null)
                preOut.InnerText = globalVariable.InnerPreText + "\n";
        }    
        
    </script>

    <body>
        <form id="form1" runat="server">           
            <div id="schnapsDiv" class="SchnapsDiv" runat="server">
            </div> 
            <hr />
            <pre id="preOut" class="PreFormated" runat="server">
            </pre>
            <hr />
            <div id="SchnapsFooterDiv" class="SchnapsFooter" align="left">
                <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://darkstar.work">darkstar.work</a>
            </div>    
        </form>
    </body>
</html>
