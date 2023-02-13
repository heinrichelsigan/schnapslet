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

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Octal Dump Mono WebApi</title>
        <!-- Google tag (gtag.js) -->
        <script async src="https://www.googletagmanager.com/gtag/js?id=G-01S65129V7"></script>
        <script>
                window.dataLayer = window.dataLayer || [];
                function gtag(){dataLayer.push(arguments);}
                gtag('js', new Date());

                gtag('config', 'G-01S65129V7');
        </script>
</head>

<script runat="server" language="C#">

    void Page_Load(object sender, EventArgs e)
    {
        imTalon.ImageUrl = HttpContext.Current.Request.ApplicationPath + "cardpics/talon.png";
        imOut0.ImageUrl = HttpContext.Current.Request.ApplicationPath +  "cardpics/e0.gif";
        imOut1.ImageUrl = HttpContext.Current.Request.ApplicationPath +  "cardpics/e0.gif";

        preOut.InnerText = "[pre][/pre]";
    }

    string Process_Od(
        string filepath = "/usr/bin/od",
        string args = "-A x -t x8z -w32 -v -j 0 -N 1024 /dev/urandom")
    {
        string consoleOutput = "Exec: " + filepath + " " + args;
        try
        {
            using (Process compiler = new Process())
            {
                compiler.StartInfo.FileName = filepath;
                string argTrys = (!string.IsNullOrWhiteSpace(args)) ? args : "";
                compiler.StartInfo.Arguments = argTrys;
                compiler.StartInfo.UseShellExecute = false;
                compiler.StartInfo.RedirectStandardOutput = true;
                compiler.Start();

                consoleOutput = compiler.StandardOutput.ReadToEnd();

                compiler.WaitForExit();

                return consoleOutput;
            }
        }
        catch (Exception exi)
        {
            return "Exception: " + exi.Message;
        }
    }


    protected void bContinue_Click(object sender, EventArgs e)
    {
        string msg = "bContinue_Click";
        preOut.InnerText += "\r\n" + msg;
    }

    protected void bChange_Click(object sender, EventArgs e)
    {
        string msg = "bChange_Click";
        preOut.InnerText += "\r\n" + msg;
    }

    protected void b20a_Click(object sender, EventArgs e)
    {
        string msg = "b20a_Click";
        preOut.InnerText += "\r\n" + msg;
    }

    protected void b20b_Click(object sender, EventArgs e)
    {
        string msg = "b20b_Click";
        preOut.InnerText += "\r\n" + msg;
    }


</script>

<body>
    <form id="form1" runat="server">
        <div style="line-height: normal; height: 96px; width: 100%; table-layout: fixed; inset-block-start: initial">          
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="imOut1" runat="server" ImageUrl="~/cardpics/e.gif" Width="72" Height="96" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imOut0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" />
            </span>
            <span style="width:144px; height:96px; margin-left: 0px; z-index: 100; margin-top: 0px; text-align: left; font-size: medium">                
                <asp:ImageButton ID="imTalon" runat="server" ImageUrl="~/cardpics/talon.png" Width="144" Height="96" />                 
            </span>            
            <span style="width:72px; height:96px; margin-left: -48px;  z-index: 10;  margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imAtou" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" />
            </span>                        
        </div>
        <div style="nowrap; line-height: normal; height: 96px; width: 100%; font-size: medium; ; table-layout: fixed; inset-block-start: auto">
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="im0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im1" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im2" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im3" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im4" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" />
            </span>
        </div>        
        <div style="nowrap; line-height: normal; vertical-align:middle; height: 36px; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: initial">
            <span style="width:100%; vertical-align:middle; text-align: left; font-size: medium; height: 36px;" align="left"  valign="middle">            
                <asp:TextBox ID="tMsg" runat="server" ToolTip="text message" Width="360" Height="36" AutoPostBack="True">Short Information</asp:TextBox>
            </span>
        </div>
        <div style="nowrap; line-height: normal; height: 60px;  margin-top: 16px; vertical-align:middle; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: initial">
            <span style="width:40%; vertical-align:middle; text-align: left; font-size: medium" align="left" valign="middle">
                <asp:Button ID="bContinue" runat="server" ToolTip="Continue" Text="Continue" OnClick="bContinue_Click" />
                &nbsp;
                <asp:Button ID="bChange" runat="server" ToolTip="Change Atou" Text="Change Atou Card" OnClick="bChange_Click" Enabled="false" />
            </span>
            <span style="width:30%; vertical-align:middle; text-align: left" align="left" valign="middle">
                <asp:TextBox ID="tPoints" runat="server" ToolTip="text message" Width="36" Enabled="false">0</asp:TextBox>                
                &nbsp;
                <asp:TextBox ID="tRest" runat="server" ToolTip="text message" Width="36" Enabled="false">10</asp:TextBox>
            </span>
            <span style="width:40%; vertical-align:middle; text-align: left; font-size: medium" align="left" valign="right">
                <asp:Button ID="b20a" runat="server" ToolTip="Say marriage 20" Text="Marriage 20" OnClick="b20a_Click" Enabled="false" />
                &nbsp;
                <asp:Button ID="b20b" runat="server" ToolTip="Say marriage 40" Text="Marriage 40" OnClick="b20b_Click" Enabled="false" />
            </span>
        </div>
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
