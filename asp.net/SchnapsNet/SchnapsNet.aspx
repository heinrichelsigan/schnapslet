<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchnapsNet.aspx.cs" Inherits="SchnapsNet.SchnapsNet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Schnaps.Net</title>
</head>
<body>
    <form id="form1" runat="server">        
        <div style="line-height: normal; height: 10%; width: 100%; table-layout: fixed; inset-block-start: auto">                
            <span style="width:92%; height:10%; margin: 0px 0px 0px 0px; text-align: right; vertical-align:top; font-size: larger" align="right" valign="top">
                <asp:Label ID="lPoints" Width="10%" Height="10%"  runat="server" ToolTip="Points">Points</asp:Label>
                <asp:TextBox ID="tPoints" Width="12%" Height="10%"  runat="server" ToolTip="text message" Enabled="false">0</asp:TextBox>
                <asp:Label ID="lRest" Width="10%" Height="10%"  runat="server" ToolTip="Rest">Rest</asp:Label>
                <asp:TextBox ID="tRest" Width="12%"  Height="10%"  runat="server" ToolTip="text message" Enabled="false">10</asp:TextBox>
                <asp:Button ID="bMerge" Width="12%" Height="10%"  runat="server" Text="Start" OnClick="bMerge_Click" />
                <asp:Button ID="bStop" Width="12%"  Height="10%"  runat="server" Text="Stop" OnClick="bStop_Click" style="vertical-align: top; tab-size: inherit" />           
            </span>            
        </div>
        <div style="nowrap; line-height: normal; height: 10%;  margin-top: 12px; vertical-align:middle; width: 100%; font-size: larger; table-layout: fixed; inset-block-start: initial">
            <span style="width:20%; height: 10%; vertical-align:middle; text-align: left; font-size: medium" align="left" valign="middle">
                <asp:Button ID="bContinue" Width="20%" Height="10%" runat="server" ToolTip="Continue" Text="Continue" OnClick="bContinue_Click" />&nbsp;                
            </span>
            <span style="width:20%; height: 10%;  vertical-align:middle; text-align: left" align="left" valign="middle">
                <asp:Button ID="bChange" Width="20%" Height="10%" runat="server" ToolTip="Change Atou" Text="Change Atou Card" OnClick="bChange_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="width:20%; height: 10%;  vertical-align:middle; text-align: left; font-size: medium" align="left" valign="middle">
                <asp:Button ID="b20a" Width="20%" Height="10%" runat="server" ToolTip="Say marriage 20" Text="Marriage 20" OnClick="b20a_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="width:20%; height: 10%;  vertical-align:middle; text-align: left; font-size: medium" align="right" valign="middle">
                <asp:Button ID="b20b" Width="20%" Height="10%" runat="server" ToolTip="Say marriage 40" Text="Marriage 40" OnClick="b20b_Click" Enabled="false" />&nbsp;                
            </span>            
        </div>
        <div style="nowrap; line-height: normal; height: 12%; width: 100%; margin-top: 12px; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="width:15%; height:12%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="imOut1" runat="server" ImageUrl="~/cardpics/e.gif" 
                    Width="15%" Height="12%" />
            </span>
            <span style="width:15%; height:12%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imOut0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="12%" />
            </span>
            <span style="width:15%; height:12%; margin-left: 0px; margin-top: 0px;  z-index: 10;  margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imAtou10" runat="server" ImageUrl="~/cardpics/n0.gif" 
                    Width="15%" Height="12%" OnClick="ImageCard_Click" style="z-index: 1" />
            </span>
            <span style="width:15%; height:11%; margin-left: -4%; margin-top: 2%; z-index: 100; text-align: left; vertical-align: top; font-size: medium">                
                <asp:Image ID="imTalon" runat="server" ImageUrl="~/cardpics/t.gif" 
                    style="z-index: 110; tab-size: inherit"  Width="12%" />
            </span>                        
        </div>
        <div style="nowrap; line-height: normal; height: 12%; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="width:15%; height:12%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="im0" runat="server" ImageUrl="~/cardpics/n0.gif" 
                    Width="15%" Height="12%" OnClick="ImageCard_Click" />
            </span>
            <span style="width:15%; height:12%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im1" runat="server" ImageUrl="~/cardpics/n0.gif" 
                     Width="15%" Height="12%" OnClick="ImageCard_Click" />
            </span>
            <span style="width:15%; height:12%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im2" runat="server" ImageUrl="~/cardpics/n0.gif" 
                    Width="15%" Height="12%" OnClick="ImageCard_Click" />
            </span>
            <span style="width:15%; height:12%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im3" runat="server" ImageUrl="~/cardpics/n0.gif" 
                    Width="15%" Height="12%" OnClick="ImageCard_Click" />
            </span>
            <span style="width:15%; height:12%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im4" runat="server" ImageUrl="~/cardpics/n0.gif" 
                    Width="15%" Height="12%"  OnClick="ImageCard_Click" />
            </span>
        </div>        
        <div style="nowrap; line-height: normal; vertical-align:middle; height: 8%; margin-top: 8px; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: initial">
            <span style="width:100%; vertical-align:middle; text-align: left; font-size: medium; height: 36px;" align="left"  valign="middle">            
                <asp:TextBox ID="tMsg" runat="server" ToolTip="text message" Width="92%" Height="8%" AutoPostBack="True">Short Information</asp:TextBox>
            </span>
        </div>
        <pre id="preOut" style="width: 100%; height: 12%; visibility: visible; scroll-behavior: auto;" runat="server">
        </pre>
        <div align="left" style="text-align: left; width: 100%; height: 12%; visibility: collapse; background-color='#bfbfbf'; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
            <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://@arkstar.work">darkstar.work</a>
            <asp:Button ID="bHelp" runat="server" ToolTip="Help" Text="Help" OnClick="bHelp_Click" Enabled="false" Visible="false" />
        </div>    
    </form>
</body>
</html>
