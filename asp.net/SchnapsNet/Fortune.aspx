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
    <noscript>
        <meta http-equiv="refresh" content="16; url=https://darkstar.work/mono/fortune/" />
    </noscript>
    <title>Fortune Mono WebApi</title>
        <!-- Google tag (gtag.js) -->
        <script async src="https://www.googletagmanager.com/gtag/js?id=G-01S65129V7"></script>
        <script>
                window.dataLayer = window.dataLayer || [];
                function gtag(){dataLayer.push(arguments);}
                gtag('js', new Date());

                gtag('config', 'G-01S65129V7');
        </script>
        <script type="text/javascript">
                function reloadFortune()  {
                        var url = "https://darkstar.work/mono/fortune/";
                        var delay = "16000";
                        setTimeout(function () {
                                window.location.href = "https://darkstar.work/mono/fortune/";
                                }, 16000); //will call the function after 2 secs.
                }
        </script>
</head>

<script runat="server" language="C#">

    void Page_Load(object sender, EventArgs e)
    {
        Literal1.Text = Fortune("/usr/games/fortune", "-a -s  ");
        PreFortune.InnerText = Fortune("/usr/games/fortune", " -a -l ");
    }

    string Fortune(string filepath = "/usr/games/fortune", string args = "-a -l")
    {
        string consoleOutput = "No fortune today";
        try
        {
            using (Process compiler = new Process())
            {
                compiler.StartInfo.FileName = filepath;
                string argTrys = (!string.IsNullOrWhiteSpace(args)) ? args : $"";
                compiler.StartInfo.Arguments = $"{argTrys}";
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
            return $"Exception: {exi.Message}";
        }
    }

</script>
<body onload="reloadFortune(); return false;">
    <form id="form1" runat="server">
        <pre id="PreFortune" runat="server" style="text-align: left; border-style:none; background-color='#bfbfbf'; font-size: larger; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif"></pre>
        <hr />
        <div align="left" style="background-color='#bfbfbf'; font-size: larger; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>
        <hr />
        <div align="right" style="background-color='#bfbfbf'; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
            <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://darkstar.work">darkstar.work</a>
        </div>
    </form>
</body>
</html>
