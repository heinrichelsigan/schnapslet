<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchnapsNet.aspx.cs" Inherits="SchnapsNet.SchnapsNet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Schnaps.Net</title>
</head>
<body>
    <form id="form1" runat="server">        
        <div style="line-height: normal; height: 96px; width: 100%; table-layout: fixed; inset-block-start: auto">    
            <span style="width:20%; height:96px; margin-left: 0px; margin-top: 0px; vertical-align:top; text-align: left; font-size: medium">
                <asp:ImageButton ID="imCOut0" runat="server" ImageUrl="~/cardpics/e.gif" Visible="true" Enabled="false" Width="72" Height="96" />
            </span>
            <span style="width:20%; height: 96px; margin-left: 0px; margin-top: 0px; vertical-align:top; text-align: left; font-size: medium">
                <asp:ImageButton ID="imCOut1" runat="server" ImageUrl="~/cardpics/e.gif" Visible="true" Enabled="false" Width="72" Height="96" />
            </span>                
            <span style="width:60%; height:96px; margin: 0px 0px 0px 0px; text-align: right; vertical-align:top; font-size: medium" align="right" valign="top">
                <asp:Label ID="lPoints" runat="server" ToolTip="Points">Points</asp:Label>
                <asp:TextBox ID="tPoints" runat="server" ToolTip="text message" Width="32px" Enabled="false">0</asp:TextBox>
                <asp:Label ID="lRest" runat="server" Width="32" ToolTip="Rest">Rest</asp:Label>
                <asp:TextBox ID="tRest" runat="server" ToolTip="text message" Enabled="false" Width="32px">10</asp:TextBox>
                <asp:Button ID="bMerge" runat="server" Text="Start" OnClick="bMerge_Click" />
            </span>            
        </div>
        <div style="nowrap; line-height: normal; height: 96px; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="width:20%; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="imOut1" runat="server" ImageUrl="~/cardpics/e.gif" Width="72" Height="96" />
            </span>
            <span style="width:20%; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imOut0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" />
            </span>
            <span style="width:20%; height:96px; margin-left: 0px; margin-top: 0px;  z-index: 10;  margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imAtou10" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageCard_Click" style="z-index: 1" />
            </span>
            <span style="width:20%; height:72px; margin-left: -14px; margin-top: 12px; z-index: 100; text-align: left; vertical-align: top; font-size: medium">                
                <asp:Image ID="imTalon" runat="server" ImageUrl="~/cardpics/t.gif" style="z-index: 110; tab-size: inherit"  Width="96" Height="72" />
            </span>                        
            <span style="width:20%; text-align: right; vertical-align: top; font-size: medium; margin-left: 0px; margin-top: 0px">
                <asp:Button ID="bStop" runat="server" Text="Stop" OnClick="bStop_Click" style="vertical-align: top; tab-size: inherit" />
            </span>
        </div>
        <div style="nowrap; line-height: normal; height: 96px; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="im0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageCard_Click" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im1" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageCard_Click" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im2" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageCard_Click" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im3" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageCard_Click" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im4" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageCard_Click" />
            </span>
        </div>        
        <div style="nowrap; line-height: normal; vertical-align:middle; height: 36px; margin-top: 8px; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: initial">
            <span style="width:100%; vertical-align:middle; text-align: left; font-size: medium; height: 36px;" align="left"  valign="middle">            
                <asp:TextBox ID="tMsg" runat="server" ToolTip="text message" Width="366" Height="36" AutoPostBack="True">Short Information</asp:TextBox>
            </span>
        </div>
        <div style="nowrap; line-height: normal; height: 48px;  margin-top: 16px; vertical-align:middle; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: initial">
            <span style="width:25%; vertical-align:middle; text-align: left; font-size: medium" align="left" valign="middle">
                <asp:Button ID="bContinue" runat="server" ToolTip="Continue" Text="Continue" OnClick="bContinue_Click" />&nbsp;                
            </span>
            <span style="width:25%; vertical-align:middle; text-align: left" align="left" valign="middle">
                <asp:Button ID="bChange" runat="server" ToolTip="Change Atou" Text="Change Atou Card" OnClick="bChange_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="width:25%; vertical-align:middle; text-align: left; font-size: medium" align="left" valign="middle">
                <asp:Button ID="b20a" runat="server" ToolTip="Say marriage 20" Text="Marriage 20" OnClick="b20a_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="width:25%; vertical-align:middle; text-align: left; font-size: medium" align="right" valign="middle">
                <asp:Button ID="b20b" runat="server" ToolTip="Say marriage 40" Text="Marriage 40" OnClick="b20b_Click" Enabled="false" />&nbsp;                
            </span>            
        </div>
        <pre id="preOut" style="visibility: hidden;" runat="server">
        </pre>
        <hr style="visibility: collapse" />
        <div align="left" style="text-align: left; visibility: collapse; background-color='#bfbfbf'; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
            <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://@arkstar.work">darkstar.work</a>
            <asp:Button ID="bHelp" runat="server" ToolTip="Help" Text="Help" OnClick="bHelp_Click" Enabled="false" Visible="false" />
        </div>    
    </form>
</body>
</html>
