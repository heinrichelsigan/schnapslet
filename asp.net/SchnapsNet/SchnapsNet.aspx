<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchnapsNet.aspx.cs" Inherits="SchnapsNet.SchnapsNet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Schnaps.Net</title>
    <script>
        let helpWin = null;

        function HelpOpen() {
            var Mleft = (screen.width/2) - (720/2);
            var Mtop = (screen.height / 2) - (640 / 2);
            if (helpWin == null)
                helpWin = window.open('Help.aspx', 'helpWin',
                    'height=640,width=720,location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,top=\' + Mtop + \', left=\' + Mleft + \'');
            else try {
                helpWin.focus();
            } catch (Exception) {
            }
        }

        function highLightOnOver(highLightId) {

            if (highLightId != null && document.getElementById(highLightId) != null) {

                if (document.getElementById(highLightId).style.borderStyle == "dotted") 
                    return; // do nothing when dotted                    

                if ((document.getElementById("b20a") != null && document.getElementById("b20a").style.borderColor == "purple") ||
                    (document.getElementById("b20b") != null && document.getElementById("b20b").style.borderColor == "purple")) 
                        return; // don't highlight other cards in case of pair marriage

                // set border-width: 1; border-style: dashed
                document.getElementById(highLightId).style.borderWidth = "medium";
                document.getElementById(highLightId).style.borderColor = "indigo";
                document.getElementById(highLightId).style.borderStyle = "dashed";
            }
        }

        function unLightOnOut(unLightId) {
            
            if (unLightId != null && document.getElementById(unLightId) != null) {

                if (document.getElementById(unLightId).style.borderStyle == "dotted") 
                    return; // do nothing when dotted

                // if (document.getElementById(highLightId).style.borderStyle == "dashed" ||
                //    document.getElementById(highLightId).style.borderWidth == 1) {
                document.getElementById(unLightId).style.borderWidth = "medium";
                document.getElementById(unLightId).style.borderColor = "#f7f7f7";
                document.getElementById(unLightId).style.borderStyle = "solid";
                // }
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">        
        <div id="DivSchnapsButtons" style="line-height: normal; min-height: 40px; min-width: 400px; height: 8%; margin-top: 8px; vertical-align:middle; width: 100%; font-size: larger; table-layout: fixed; inset-block-start: initial">
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: large" align="left" valign="middle">
                <asp:Button ID="bContinue" Width="15%" Height="8%" runat="server" ToolTip="Continue" Text="Continue" OnClick="Continue_Click" Enabled="true" 
                    style="min-height: 40px; min-width: 56px; font-size: large; border-color: darkslategray" />&nbsp;
            </span>
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: large" align="left" valign="middle">
                <asp:Button ID="bChange" Width="15%" Height="8%" runat="server" ToolTip="Change Atou" Text="Change Atou Card" OnClick="Change_Click" Enabled="false"
                style="min-height: 40px; min-width: 56px; font-size: large; border-color: darkslategray"/>&nbsp;
            </span>
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: large" align="left" valign="middle">
                <asp:Button ID="b20a" Width="15%" Height="8%" runat="server" ToolTip="Say marriage 20" Text="Marriage 20" OnClick="A20_Click" Enabled="false" 
                    style="min-height: 40px; min-width: 56px; font-size: large; border-color: darkslategray" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: large" align="right" valign="middle">
                <asp:Button ID="b20b" Width="15%" Height="8%" runat="server" ToolTip="Say marriage 40" Text="Marriage 40" OnClick="B20_Click" Enabled="false"
                    style="min-height: 40px; min-width: 56px; font-size: large; border-color: darkslategray"  />&nbsp;                
            </span>  
            <span style="min-height: 40px; min-width: 60px; width:12%; height: 8%; vertical-align:middle; text-align: left; font-size: large" align="left" valign="middle">
                <asp:Button ID="bMerge" Width="12%" Height="8%" runat="server" ToolTip="Start" Text="Continue"  OnClick="Merge_Click" Enabled="true"
                    style="min-height: 40px; min-width: 40px; font-size: large; border-color: darkslategray" />
                <asp:Button ID="bStop" Width="12%" Height="8%" runat="server" ToolTip="Stop" Text="Stop" OnClick="Stop_Click" Enabled="false" Visible="false"
                    style="min-height: 40px; min-width: 40px; font-size: large; border-color: darkslategray"  />
            </span>            
            <span style="visibility: visible; min-height: 40px; min-width: 36px; width:10%; height: 8%; vertical-align:middle; text-align: left; font-size: large" align="right" valign="middle">
                <asp:Button ID="bHelp" Width="10%" Height="8%" runat="server" ToolTip="Help" Text="Help"  Enabled="true" OnClientClick="HelpOpen();"
                    style="min-height: 40px; min-width: 36px; font-size: large; border-color: darkslategray" />
            </span>
        </div>
        <div id="DivSchnapsStack" style="line-height: normal; min-height: 96px; min-width: 72px; height:10%; width: 100%; margin-top: 8px; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">                
                <asp:ImageButton ID="imOut21" runat="server" ImageUrl="~/cardpics/e.gif" Width="15%" Height="10%" OnClick="Continue_Click"
                    style="font-size: medium; border-width: medium; border-color: #f7f7f7; border-style: solid" />                
                </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">                
                <asp:ImageButton ID="imOut20" runat="server" ImageUrl="~/cardpics/e.gif" Width="15%" Height="10%" OnClick="Continue_Click" 
                    style="font-size: medium; border-width: medium; border-color: #f7f7f7; border-style: solid" />                
            </span>
            <span style="visibility: visible; min-height: 96px; min-width: 96px; height:10%; width:26%; z-index: 10;  margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imMerge11" runat="server" ImageUrl="~/cardpics/mergeshort.gif" Width="20%" OnClick="Merge_Click" ToolTip="Click here to start"
                    style="z-index: 2; border-width: medium; border-color: #f7f7f7; border-style: solid" />&nbsp;
            </span>
            <span id="SpanAtouTalon" runat="server" 
                style="min-height: 96px; min-width: 72px; height:10%; width:26%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium; visibility: hidden">
                <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px;  z-index: 10;  margin-top: 0px; text-align: left; font-size: medium;">
                    <asp:ImageButton ID="imAtou10" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" ToolTip="Close Game" Visible="false" OnClick="ImageCard_Click" 
                        style="z-index: 1; border-width: medium; border-color: #f7f7f7; border-style: solid" />
                </span>
                <span style="min-height: 72px; min-width: 96px; height:8%; width:18%; margin-left: -6%; margin-top: 2%; z-index: 100; text-align: left; vertical-align: top; font-size: medium">                
                    <asp:Image ID="imTalon" runat="server" ImageUrl="~/cardpics/t.gif" Visible="false" 
                        style="width:18%; margin-top: 2%; z-index: 110; tab-size: inherit; border-style: none" Width="18%" />
                </span>
            </span>
            <span id="SpanComputerStitches" runat="server" 
                style="min-height: 96px; min-width: 96px; height: 10%; width: 18%; margin-left: 0px; margin-top: 0px;  z-index: 10; font-size: medium; visibility: visible;">
                <asp:ImageButton ID="ImageComputerStitch0a" runat="server" ImageUrl="~/cardpics/n1.gif" Width="15%" style="z-index: 2" BorderStyle="None" Visible="false" OnClick="ImageComputerStitch_Click" />
                <asp:ImageButton ID="ImageComputerStitch0b" runat="server" ImageUrl="~/cardpics/n1.gif" Width="15%" style="z-index: 2; margin-left: -12%; margin-top: 1px" BorderStyle="None" Visible="false" OnClick="ImageComputerStitch_Click" />
            </span>            
        </div>
        <div id="DivPlayerStack" style="line-height: normal; min-height: 96px; min-width: 72px; height:10%; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="im0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click"  
                    Style="border-width: medium; border-color: #f7f7f7; border-style: solid" onmouseover="highLightOnOver('im0')" onmouseout="unLightOnOut('im0')" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im1" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click"  
                    Style="border-width: medium; border-color: #f7f7f7; border-style: solid" onmouseover="highLightOnOver('im1')" onmouseout="unLightOnOut('im1')" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im2" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click"  
                    Style="border-width: medium; border-color: #f7f7f7; border-style: solid" onmouseover="highLightOnOver('im2')" onmouseout="unLightOnOut('im2')" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%;  margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im3" runat="server" ImageUrl="~/cardpics/n0.gif"  Width="15%" Height="10%" OnClick="ImageCard_Click"  
                    Style="border-width: medium; border-color: #f7f7f7; border-style: solid" onmouseover="highLightOnOver('im3')" onmouseout="unLightOnOut('im3')" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im4" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%"  OnClick="ImageCard_Click"
                    Style="border-width: medium; border-color: #f7f7f7; border-style: solid" onmouseover="highLightOnOver('im4')" onmouseout="unLightOnOut('im4')" />
            </span>
            <span style="min-height: 96px; min-width: 24%; height:10%; width: 24%; margin-right: 0px; margin-top: -8%; vertical-align:top; text-align: right; visibility: visible; font-size: medium">
                <asp:Table ID="tableTournement" runat="server" style="display:inline-table; font-size: large; vertical-align:top; text-align: right">
                    <asp:TableHeaderRow style="border-bottom: thick; border-bottom-style: double">
                        <asp:TableCell style="border-bottom: thick; border-right: medium">You</asp:TableCell>
                        <asp:TableCell style="border-bottom: thick; border-left: medium">Computer</asp:TableCell>
                    </asp:TableHeaderRow>
                </asp:Table>                
            </span>
        </div>        
        <div style="line-height: normal; vertical-align:middle; height: 8%; width: 100%; font-size: large; margin-top: 4px; table-layout: fixed; inset-block-start: initial">
            <span style="width:4%; vertical-align:middle; text-align: left; font-size: large; height: 8%;" align="left" valign="middle">
                <asp:TextBox ID="tPoints" Width="4%" Height="8%"  runat="server" ToolTip="text message" Enabled="false"
                    style="min-height: 32px; min-width: 24px; font-size: large; text-align: right; border-color: darkslategray; border-style: solid" >0</asp:TextBox> 
            </span>
            <span style="width:8%; vertical-align:central; text-align: left; font-size: large; height: 8%;" align="right" valign="middle">
                <asp:Label ID="lPoints" Width="8%" Height="8%"  runat="server" ToolTip="Points" 
                    style="min-height: 32px; min-width: 24px; font-size: large">Points</asp:Label>
            </span>
            <span style="width:76%; vertical-align:middle; text-align: left; font-size: large; height: 8%;" align="middle" valign="middle">            
                <asp:TextBox ID="tMsg" runat="server" ToolTip="text message" Width="76%" Height="8%" 
                    style="min-height: 32px; font-size: large; border-color: darkslategray; border-style: solid">Short Information</asp:TextBox>
            </span>       
        </div>
        <pre id="preOut" style="width: 100%; height: 12%; visibility: hidden; font-size: large; scroll-behavior: auto; max-height: 54px" runat="server">
        </pre>        
        <div align="left" style="text-align: left; width: 100%; height: 8%; visibility: inherit; background-color: #bfbfbf; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
            <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="https://github.com/heinrichelsigan" target="_blank">github.com/heinrichelsigan</a>/<a href="https://github.com/heinrichelsigan/schnapslet" target="_blank">schnapslet</a>]            
        </div>    
    </form>
</body>
</html>
