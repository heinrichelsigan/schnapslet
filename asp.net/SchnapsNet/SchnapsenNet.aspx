<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="SchnapsenNet.aspx.cs" Inherits="SchnapsNet.SchnapsenNet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Schnapsen.Net</title>
        <link rel="stylesheet" href="res/schnapsennet.css" />
        <meta id="metaAudioId" runat="server" name="" onchange="alert(document.getElementById('metaAudioId').Name); metaAudioNameChanged('metaAudioId')" />
        <script type="text/javascript" src="res/schnapsennet.js"></script>
</head>
<body onload="schnapsStateInit()">
    <script>
        window.loaded = schnapsStateInit();
    </script>
    <form id="form1" runat="server">        
        <div id="SchnapsBtn1" class="SchnapsBtnDiv">
            <span class="SchnapsBtnSpan" align="left" valign="middle">
                <asp:Button ID="bContinue" runat="server" Text="Continue" ToolTip="Continue" OnClick="Continue_Click" 
                    CssClass="SchnapsBtnWidth56px" Width="15%" Height="8%" Enabled="false" />&nbsp;
            </span>                        
            <span class="SchnapsBtnSpan" align="left" valign="middle">
                <asp:Button ID="bChange" runat="server" Text="Change Atou" ToolTip="Change atou card" OnClick="Change_Click" 
                    CssClass="SchnapsBtnWidth56px" Width="15%" Height="8%" Enabled="false" />&nbsp;                
            </span>
            <span class="SchnapsBtnSpan" align="left" valign="middle">
                <asp:Button ID="b20a" runat="server" Text="Marriage 20" ToolTip="Say marriage 20" OnClick="A20_Click"  
                    CssClass="SchnapsBtnWidth56px" Width="15%" Height="8%" Enabled="false" />&nbsp;                
            </span>
            <span class="SchnapsBtnSpan" align="right" valign="middle">
                <asp:Button ID="b20b" runat="server" Text="Marriage 40" ToolTip="Say marriage 40" OnClick="B20_Click"   
                    CssClass="SchnapsBtnWidth56px" Width="15%" Height="8%" Enabled="false" />&nbsp;                
            </span>   
            <span class="SchnapsBtnSpanWidth12" valign="middle">
                <asp:Button ID="bMerge" runat="server" Text="Start" ToolTip="Start" OnClick="Merge_Click" 
                    CssClass="SchnapsBtnWidth40px" Width="12%" Height="8%" Enabled="true" Visible="true" />
                <asp:Button ID="bStop" runat="server" Text="Stop" ToolTip="Stop" OnClick="Stop_Click" 
                    CssClass="SchnapsBtnWidth36px" Width="10%" Height="8%" Enabled="true" Visible="false" />
            </span>
            <span class="SchnapsBtnSpanWidth10" align="right" valign="middle">
                <asp:Button ID="bHelp" runat="server" Text="Help" ToolTip="Help" OnClick="Help_Click"
                    CssClass="SchnapsBtnWidth36px" Width="10%" Height="8%" Enabled="true" /> 
            </span>            
        </div>
        <div id="SchnapsStack" class="SchnapsStackDiv">
            <span class="SchnapsStackSpanWidthH96W72">
                <asp:ImageButton ID="imOut21" runat="server" ImageUrl="~/cardpics/e.gif" Width="15%" Height="10%" OnClick="ImOut_Click" />
            </span>
            <span class="SchnapsStackSpanWidthH96W72">
                <asp:ImageButton ID="imOut20" runat="server" ImageUrl="~/cardpics/e.gif" Width="15%" Height="10%"  OnClick="ImOut_Click"  />
            </span>
            <span id="spanMerge" runat="server" class="SchnapsStackSpanH96W96">
                <asp:ImageButton ID="imMerge11" runat="server" ImageUrl="~/cardpics/mergeshort.gif" Width="20%" style="z-index: 2" OnClick="ImMerge11_Click" BorderStyle="None" />&nbsp;
            </span>
            <span id="spanAtou" runat="server" class="SchnapsStackSpanWidthH96W72A">
                <asp:ImageButton ID="imAtou10" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" style="z-index: 1" />
            </span>
            <span id="spanTalon" runat="server" class="SchnapsStackSpanH72W96">                
                <asp:Image ID="imTalon" runat="server" ImageUrl="~/cardpics/t.gif" Width="18%" CssClass="TalonImage" />
            </span>               
            <span id="spanComputerStitches" class="SchnapsStackStitches" runat="server" visible="false">
                <asp:ImageButton ID="ImageComputerStitch0a" runat="server" Visible="false" ImageUrl="~/cardpics/n1.gif" Width="15%" CssClass="ComputerStich0a" BorderStyle="None" OnClick="ImageComputerStitch_Click" />
                <asp:ImageButton ID="ImageComputerStitch0b" runat="server" Visible="false" ImageUrl="~/cardpics/n1.gif" Width="15%" CssClass="ComputerStich0b" BorderStyle="None" OnClick="ImageComputerStitch_Click" />
            </span>            
        </div>
        <div id="DivPlayerStack" class="PlayerStack">
            <span ID="spanIm0" class="cardImgSpan" valign="left">
                <asp:ImageButton ID="im0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" onmouseover="highLightOnOver('im0')" onmouseout="unLightOnOut('im0')" />
            </span>
            <span ID="spanIm1" class="cardImgSpan">
                <asp:ImageButton ID="im1" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" onmouseover="highLightOnOver('im1')" onmouseout="unLightOnOut('im1')" />
            </span>
            <span ID="spanIm2" class="cardImgSpan">
                <asp:ImageButton ID="im2" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" onmouseover="highLightOnOver('im2')" onmouseout="unLightOnOut('im2')" />
            </span>
            <span ID="spanIm3" class="cardImgSpan">
                <asp:ImageButton ID="im3" runat="server" ImageUrl="~/cardpics/n0.gif"  Width="15%" Height="10%" OnClick="ImageCard_Click" onmouseover="highLightOnOver('im3')" onmouseout="unLightOnOut('im3')" />
            </span>
            <span ID="spanIm4" class="cardImgSpan" 
                style="">
                <asp:ImageButton ID="im4" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%"  OnClick="ImageCard_Click" onmouseover="highLightOnOver('im4')" onmouseout="unLightOnOut('im4')" />
            </span>
            <span class="tableTournamentSpan">
                <asp:Table ID="tableTournement" CssClass="TableTournament" runat="server">
                    <asp:TableHeaderRow style="border-bottom: thick">
                        <asp:TableCell style="border-bottom: thick; border-right: medium">You</asp:TableCell>
                        <asp:TableCell style="border-bottom: thick; border-left: medium">Computer</asp:TableCell>
                    </asp:TableHeaderRow>
                </asp:Table>                
            </span>
        </div>        
        <div id="SchnapsPointMsg" class="SchnapsPointMsgDiv">
            <span style="width:5%; vertical-align: central; text-align: left; font-size: large; height: 8%;" align="left" valign="middle">
                <asp:TextBox ID="tPoints" Width="5%" Height="8%"  runat="server" ToolTip="text message" style="min-height: 40px; min-width: 32px; font-size: large" Enabled="false">0</asp:TextBox>                
            </span>
            <span style="width:8%; vertical-align:central; text-align: left; font-size: large; height: 8%;" align="right" valign="middle">
                <asp:Label ID="lPoints" Width="8%" Height="8%"  runat="server" ToolTip="Points" style="min-height: 40px; min-width: 40px; font-size: large;">Points</asp:Label>
            </span>
            <span style="width:75%; vertical-align:middle; text-align: left; font-size: large; height: 8%;" align="middle" valign="middle">            
                <asp:TextBox ID="tMsg" runat="server" ToolTip="text message" Width="75%" Height="8%" style="font-size: larger">Short Information</asp:TextBox>
            </span>            
        </div>
        <pre id="preOut" class="PreFormated" runat="server">
        </pre>
        <div id="SchnapsFooterDiv" class="SchnapsFooter" align="left">
            <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://darkstar.work">darkstar.work</a>
            <a id="aAudio" runat="server" name="" href="" onchange="aAudioChanged()">audio</a>
        </div>    
    </form>
</body>
</html>
