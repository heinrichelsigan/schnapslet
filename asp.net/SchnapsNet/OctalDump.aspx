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
        string args = " -A x -t x" + DropDown_HexWidth.SelectedItem.Value.ToString() + "z " +
            " -w" + DropDown_WordWidth.SelectedItem.Value.ToString() + " -v " +
            " -j " + Convert.ToInt32(TextBox_Seek.Text) + " " +
            " -N " + DropDown_ReadBytes.SelectedItem.Value.ToString() + " /dev/urandom";
        TextBox_OdCmd.Text = "/usr/bin/od" + args;

        // if (!Page.IsPostBack)
            preOut.InnerText = Process_Od("/usr/bin/od", args);
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
                return "Exception: " + exi.Message;
            }
        }


        protected void Button_OctalDump_Click(object sender, EventArgs e)
        {
            string args = " -A x -t x" + DropDown_HexWidth.SelectedItem.Value.ToString() + "z " +
                " -w" + DropDown_WordWidth.SelectedItem.Value.ToString() + " -v " +
                " -j " + Convert.ToInt32(TextBox_Seek.Text) + " " +
                " -N " + DropDown_ReadBytes.SelectedItem.Value.ToString() + " /dev/urandom";

            this.TextBox_OdCmd.Text = "/usr/bin/od" + args;
            preOut.InnerText = Process_Od("/usr/bin/od", args);
        }

        protected void LinkButton_Od_Click(object sender, EventArgs e)
        {
            string args = " -A x -t x" + DropDown_HexWidth.SelectedItem.Value.ToString() + "z " +
                " -w" + DropDown_WordWidth.SelectedItem.Value.ToString() + " -v " +
                " -j " + Convert.ToInt32(TextBox_Seek.Text) + " " +
                " -N " + DropDown_ReadBytes.SelectedItem.Value.ToString() + " /dev/urandom";

            this.TextBox_OdCmd.Text = "/usr/bin/od" + args;
            preOut.InnerText = Process_Od("/usr/bin/od", args);
        }


</script>

<body>
    <form id="form1" runat="server">
        <div style="nowrap; line-height: normal; height: 24pt; width: 100%; table-layout: fixed; inset-block-start: initial">
            <span style="width:30%; vertical-align:middle; text-align: left; font-size: medium" align="left" valign="middle">
                <span style="vertical-align: middle">hex width: </span>
                <asp:DropDownList ID="DropDown_HexWidth" runat="server" ToolTip="Hexadecimal width" AutoPostBack="True">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem Selected="True">4</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem Enabled="false">16</asp:ListItem>
                </asp:DropDownList>
            </span>
            <span style="width:40%; vertical-align:middle; text-align: center; font-size: medium" align="center" valign="middle">
                &nbsp;
                <span style="vertical-align: middle">word width: </span>
                    <asp:DropDownList ID="DropDown_WordWidth" runat="server" ToolTip="Word with for bytes" AutoPostBack="True">
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem Selected="True">16</asp:ListItem>
                    <asp:ListItem>32</asp:ListItem>
                    <asp:ListItem>64</asp:ListItem>
                </asp:DropDownList>
                &nbsp;
            </span>
            <span style="width:30%; vertical-align:middle; text-align: right; font-size: medium" align="right" valign="middle">
                <span style="vertical-align: middle">read bytes: </span>
                    <asp:DropDownList ID="DropDown_ReadBytes" runat="server" ToolTip="Bytes to read on octal dump" AutoPostBack="True">
                    <asp:ListItem>128</asp:ListItem>
                    <asp:ListItem>256</asp:ListItem>
                    <asp:ListItem Selected="True">512</asp:ListItem>
                    <asp:ListItem>1024</asp:ListItem>
                    <asp:ListItem>2048</asp:ListItem>
                </asp:DropDownList>
            </span>
        </div>
<div style="nowrap; line-height: normal; vertical-align:middle; height: 24pt; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: initial">

            <span style="width:40%; vertical-align:middle; text-align: left; font-size: medium; height: 24pt;" align="left"  valign="middle">
                <span style="vertical-align: middle; height: 24pt;">seek bytes: </span>
                <asp:TextBox ID="TextBox_Seek" runat="server" ToolTip="seek bytes" Width="64pt" Height="24pt" AutoPostBack="True">0</asp:TextBox>
            </span>
            <span style="width:60%; vertical-align:middle; text-align: right; font-size: medium" align="right" valign="middle">
                <span style="vertical-align: middle">od shell: </span>
                <asp:TextBox ID="TextBox_OdCmd" runat="server" ToolTip="od shell command"  ReadOnly Width="60%" Height="24pt"></asp:TextBox>
            </span>
        </div>
        <div style="nowrap; line-height: normal; height: 24pt; vertical-align:middle; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: initial">
            <span style="width:25%; vertical-align:middle; text-align: left; font-size: medium" align="left" valign="middle">
                <asp:Button ID="Button_OctalDump" runat="server" ToolTip="Click to perform octal dump" Text="octal dump" OnClick="Button_OctalDump_Click" />
                &nbsp;
            </span>
            <span style="width:40%; vertical-align:middle; text-align: left" align="left" valign="middle">
                <asp:LinkButton ID="LinkButton_Od" runat="server"  ToolTip="Click to exec octal dump" Text="od (octal dump)"  OnClick="LinkButton_Od_Click"></asp:LinkButton>
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
