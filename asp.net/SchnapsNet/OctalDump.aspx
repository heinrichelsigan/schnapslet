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
            function gtag() { dataLayer.push(arguments); }
            gtag('js', new Date());

            gtag('config', 'G-01S65129V7');
        </script>
</head>

<script runat="server" language="C#">

    string radix = "n";
    int hexWidth = 8;
    int wordWidth = 32;
    long seekBytes = 0;
    long readBytes = 1024;
    const string odCmdPath = "/usr/bin/od";
    const string odArgsFormat = " -A {0} -t x{1}z -w{2} -v -j {3} -N {4} /dev/urandom";

    protected string OdArgs
    {
        get
        {
            if (RBL_Radix != null && RBL_Radix.SelectedItem != null && RBL_Radix.SelectedItem.Value != null && RBL_Radix.SelectedItem.Value.Length > 0)
                radix = RBL_Radix.SelectedItem.Value.ToString();
            if (!Int32.TryParse(DropDown_HexWidth.SelectedItem.Value.ToString(), out hexWidth))
                hexWidth = 0;
            if (!Int32.TryParse(DropDown_WordWidth.SelectedItem.Value.ToString(), out wordWidth))
                wordWidth = 32;
            if (!Int64.TryParse(TextBox_Seek.Text, out seekBytes))
                seekBytes = 0;
            if (!Int64.TryParse(DropDown_ReadBytes.SelectedItem.Value.ToString(), out readBytes))
                readBytes = 1024;

            string odArgs = String.Format(" -A {0} -t x{1}z -w{2} -v -j {3} -N {4} /dev/urandom",
                radix, hexWidth, wordWidth, seekBytes, readBytes);

            return odArgs;
        }
    }

    void Page_Load(object sender, EventArgs e)
    {
        // if (!Page.IsPostBack)
        Perform_Od();
    }

    protected void Perform_Od()
    {
        TextBox_OdCmd.Text = odCmdPath + OdArgs;
        preOut.InnerText = Process_Od(odCmdPath, OdArgs);
    }

    protected string Process_Od(
        string filepath = odCmdPath,
        string args = "-A n -t x8z -w32 -v -j 0 -N 1024 /dev/urandom")
    {
        string consoleOutput = "Exec: \t" + filepath + " " + args + "\n";
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

                //while (!compiler.StandardOutput.EndOfStream)
                //{
                //    string newLine = compiler.StandardOutput.ReadLine();
                //    int idx1st = newLine.IndexOf(' ');
                //    if (idx1st < 0)
                //        idx1st = newLine.IndexOf('\t');
                //    if (idx1st > 0)
                //    {
                //        consoleOutput += "<b>" + newLine.Substring(0, idx1st) + "</b>}\t";
                //        consoleOutput += newLine.Substring(idx1st);
                //    }
                //    else 
                //        consoleOutput += newLine;

                //}
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
        Perform_Od();
    }

    protected void RBL_Radix_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RBL_Radix != null && RBL_Radix.SelectedItem != null && RBL_Radix.SelectedItem.Value != null && RBL_Radix.SelectedItem.Value.Length > 0)
            radix = RBL_Radix.SelectedItem.Value.ToString();
        Perform_Od();
    }


</script>

<body>
    <form id="form1" runat="server">
        <div style="nowrap; line-height: normal; height: 28pt; width: 100%; table-layout: fixed; font-size: medium; inset-block-start: initial">
            <span style="width:25%; vertical-align:middle; text-align: left; font-size: medium; height: 24pt" align="left" valign="middle">
                <span style="text-align: left; vertical-align: middle; font-size: medium;">hex width: </span>
                <asp:DropDownList style="font-size: medium; height: 24pt" ID="DropDown_HexWidth" runat="server" ToolTip="Hexadecimal width" AutoPostBack="True">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem Selected="True">4</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                </asp:DropDownList>
            </span>
            <span style="width:25%; vertical-align:middle; text-align: center; font-size: medium; height: 24pt" align="center" valign="middle">
                <span style="text-align: left; vertical-align: middle; font-size: medium; height: 24pt"> word width: </span>
                    <asp:DropDownList style="font-size: medium; height: 24pt" ID="DropDown_WordWidth" runat="server" ToolTip="Word with for bytes" AutoPostBack="True">
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem Selected="True">16</asp:ListItem>
                    <asp:ListItem>32</asp:ListItem>
                    <asp:ListItem>64</asp:ListItem>
                </asp:DropDownList>
            </span>
            <span style="width:25%; vertical-align:middle; text-align: center; font-size: medium; height: 24pt" align="center" valign="middle">
                <span style="text-align: left; vertical-align: middle; font-size: medium; height: 24pt"> read bytes: </span>
                    <asp:DropDownList style="font-size: medium; height: 24pt"  ID="DropDown_ReadBytes" runat="server" ToolTip="Bytes to read on octal dump" AutoPostBack="True">
                    <asp:ListItem>128</asp:ListItem>
                    <asp:ListItem>256</asp:ListItem>
                    <asp:ListItem Selected="True">512</asp:ListItem>
                    <asp:ListItem>1024</asp:ListItem>
                    <asp:ListItem>2048</asp:ListItem>
                    <asp:ListItem>4096</asp:ListItem>
                </asp:DropDownList>
            </span>
            <span style="width:25%; vertical-align:middle; text-align: right; font-size: small; height: 24pt" align="right" valign="middle">
                <span style="text-align: left; vertical-align: middle; font-size: medium"> seek bytes: </span>
                <asp:TextBox style="font-size: small; height: 24pt" ID="TextBox_Seek" runat="server" ToolTip="seek bytes" MaxLength="8" Width="48pt" Height="24pt" AutoPostBack="True">0</asp:TextBox>
            </span>
        </div>
        <div style="nowrap; line-height: normal; vertical-align:middle; height: 24pt; width: 100%;table-layout: fixed;  font-size: medium; inset-block-start: initial">            
            <span style="nowrap; width:90%; vertical-align:middle; text-align: left; font-size: medium; height: 24pt" align="left" valign="middle">
                <asp:RadioButtonList style="font-size: medium" ID="RBL_Radix" runat="server" AutoPostBack="true"
                    ToolTip="Radix format" RepeatDirection="Horizontal" OnSelectedIndexChanged="RBL_Radix_SelectedIndexChanged">
                    <asp:ListItem Selected="False" Value="d">Decimal</asp:ListItem>
                    <asp:ListItem Selected="False" Value="o">Octal</asp:ListItem>
                    <asp:ListItem Selected="False" Value="x">Hex</asp:ListItem>
                    <asp:ListItem Selected="True" Value="n">None</asp:ListItem>
                </asp:RadioButtonList>
            </span>
        </div>
        <div style="nowrap; line-height: normal; height: 30pt; width: 100%; table-layout: fixed; font-size: medium; inset-block-start: initial">
            <span style="width:32%; vertical-align:middle; text-align: left; font-size: small; height: 24pt" align="left" valign="middle">
                <asp:Button style="font-size: small; height: 24pt" ID="Button_OctalDump" runat="server" ToolTip="Click to perform octal dump" Text="octal dump" OnClick="Button_OctalDump_Click" />
            </span>
            <span style="width:68%; vertical-align:middle; text-align: right; font-size: small; height: 24pt" align="right" valign="middle">
                <span style="width:12%; text-align: left; vertical-align: middle; font-size: medium">od cmd: </span>
                <asp:TextBox style="width:32%; vertical-align:middle; text-align: left; font-size: small; height: 24pt" ID="TextBox_OdCmd" runat="server" ToolTip="od shell command"  ReadOnly Width="32%" MaxLength="60" Height="24pt"></asp:TextBox>
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
