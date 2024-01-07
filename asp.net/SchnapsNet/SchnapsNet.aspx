<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchnapsNet.aspx.cs" Inherits="SchnapsNet.SchnapsNet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title>Schnaps.Net</title>
        <link rel="stylesheet" href="res/schnapsnet.css" />
        <script type="text/javascript" src="res/schnapsnet.js"></script>
        <meta name="author" content="Heinrich Elsigan" />
        <meta id="metaAudio" runat="server" name="audiowav" content="" />
        <meta id="metaLastAudio" runat="server" name="audiolastwav" content="" />
    </head>
    <body onload="schnapsStateInit()">
        <script>
            window.loaded = schnapsStateInit();
        </script>
        <form id="form1" runat="server">        
            <div id="DivSchnapsButtons" class="SchnapsBtnDiv">
                <span class="SchnapsBtnSpan" align="left" valign="middle">
                    <asp:Button ID="bContinue" Width="15%" Height="8%" runat="server" ToolTip="Continue" Text="Continue" OnClick="Continue_Click" Enabled="true" 
                        CssClass="ButtonContinue"/>&nbsp;
                </span>
                <span class="SchnapsBtnSpan" align="left" valign="middle">
                    <asp:Button ID="bChange" Width="15%" Height="8%" runat="server" ToolTip="Change Atou" Text="Change Atou Card" OnClick="Change_Click" Enabled="false"
                        CssClass="ButtonChange" />&nbsp;
                </span>
                <span class="SchnapsBtnSpan" align="left" valign="middle">
                    <asp:Button ID="b20a" Width="15%" Height="8%" runat="server" ToolTip="Say marriage 20" Text="Marriage 20" OnClick="A20_Click" Enabled="false" 
                        CssClass="Button20a" />&nbsp;                
                </span>
                <span class="SchnapsBtnSpan" align="right" valign="middle">
                    <asp:Button ID="b20b" Width="15%" Height="8%" runat="server" ToolTip="Say marriage 40" Text="Marriage 40" OnClick="B20_Click" Enabled="false"
                        CssClass="Button20b" />&nbsp;                
                </span>  
                <span class="SchnapsBtnSpan" align="left" valign="middle">
                    <asp:Button ID="bMerge" Width="15%" Height="8%" runat="server" ToolTip="Start" Text="Continue"  OnClick="Merge_Click" Enabled="true"
                        CssClass="ButtonMerge" />
                    <asp:Button ID="bStop" Width="15%" Height="8%" runat="server" ToolTip="Stop" Text="Stop" OnClick="Stop_Click" Enabled="false" Visible="false"
                        CssClass="ButtonStop" />
                </span>            
                <span class="SchnapsBtnSpanWidth12" align="right" valign="middle">
                    <asp:Button ID="bHelp" Width="12%" Height="8%" runat="server" ToolTip="Help" Text="Help"  Enabled="true" OnClientClick="HelpOpen();"
                        CssClass="ButtonHelp" />
                </span>
            </div>
            <div id="SchnapsStack" class="SchnapsStack">
                <span class="SpanOut" valign="left">                
                    <asp:ImageButton ID="imOut21" runat="server" ImageUrl="~/cardpics/e.gif" Width="15%" Height="10%" OnClick="Continue_Click"
                        CssClass="OutImage" />                
                    </span>
                <span class="SpanOut">
                    <asp:ImageButton ID="imOut20" runat="server" ImageUrl="~/cardpics/e.gif" Width="15%" Height="10%" OnClick="Continue_Click" 
                        CssClass="OutImage" />                
                </span>
                <span id="spanMerge" class="SpanMerge">
                    <asp:ImageButton ID="imMerge11" runat="server" ImageUrl="~/cardpics/mergeshort.gif" Width="20%" OnClick="Merge_Click" ToolTip="Click here to start"
                        CssClass="MergeImage" />&nbsp;
                     <span id="spanAtouTalon" runat="server" class="SpanAtouTalon">
                         <span id="spanAtou" runat="server" class="SpanAtou">
                             <asp:ImageButton ID="imAtou10" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" ToolTip="Close Game" Visible="false" OnClick="ImageCard_Click" CssClass="AtouImage" />
                         </span>
                         <span id="spanTalon" class="SpanTalon">
                             <asp:Image ID="imTalon" runat="server" ImageUrl="~/cardpics/t.gif" Visible="false" CssClass="TalonImage" Width="18%" />
                         </span>
                     </span>
                </span>               
                <span id="SpanComputerStitches" runat="server" class="SchnapsStackStitches">
                    <asp:ImageButton ID="ImageComputerStitch0a" runat="server" ImageUrl="~/cardpics/n1.gif" Width="15%" Visible="false" OnClick="ImageComputerStitch_Click" CssClass="ComputerStich0a" />
                    <asp:ImageButton ID="ImageComputerStitch0b" runat="server" ImageUrl="~/cardpics/n1.gif" Width="15%" BorderStyle="None" Visible="false" OnClick="ImageComputerStitch_Click" CssClass="ComputerStich0b" />
                </span>            
            </div>
            <div id="DivPlayerStack" class="PlayerStack">
                <span class="CardImgageSpan" valign="left">
                    <asp:ImageButton ID="im0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" CssClass="ImageCard" 
                        OnClick="ImageCard_Click" onmouseover="highLightOnOver('im0')" onmouseout="unLightOnOut('im0')" />
                </span>
                <span class="CardImgageSpan">
                    <asp:ImageButton ID="im1" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" CssClass="ImageCard" 
                        OnClick="ImageCard_Click" onmouseover="highLightOnOver('im1')" onmouseout="unLightOnOut('im1')" />
                </span>
                <span lass="CardImgageSpan">
                    <asp:ImageButton ID="im2" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" CssClass="ImageCard" 
                        OnClick="ImageCard_Click" onmouseover="highLightOnOver('im2')" onmouseout="unLightOnOut('im2')" />
                </span>
                <span lass="CardImgageSpan">
                    <asp:ImageButton ID="im3" runat="server" ImageUrl="~/cardpics/n0.gif"  Width="15%" Height="10%" CssClass="ImageCard" 
                        OnClick="ImageCard_Click" onmouseover="highLightOnOver('im3')" onmouseout="unLightOnOut('im3')" />
                </span>
                <span lass="CardImgageSpan">
                    <asp:ImageButton ID="im4" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" CssClass="ImageCard" 
                        OnClick="ImageCard_Click" onmouseover="highLightOnOver('im4')" onmouseout="unLightOnOut('im4')" />
                </span>
                <span class="TournamentTableSpan">
                    <asp:Table ID="tableTournement" runat="server" CssClass="TableTournament">
                        <asp:TableHeaderRow style="border-bottom: thick; border-bottom-style: double">
                            <asp:TableCell style="border-bottom: thick; border-right: medium">You</asp:TableCell>
                            <asp:TableCell style="border-bottom: thick; border-left: medium">Computer</asp:TableCell>
                        </asp:TableHeaderRow>
                    </asp:Table>                
                </span>
            </div>        
            <div id="schnapsPointMsg" class="SchnapsPointMsg">
                <span class="PointSpanTextBox" align="left" valign="middle">
                    <asp:TextBox ID="tPoints" Width="3%" Height="8%"  runat="server" ToolTip="text message" Enabled="false" CssClass="PointTextBox">0</asp:TextBox> 
                    <asp:Label ID="lPoints" Width="5%" Height="8%"  runat="server" ToolTip="Points" CssClass="PointLabel"></asp:Label>                    
                </span>
                <span class="PointSpanLabel" align="left" valign="middle">
                    <asp:Label ID="lAtouIs" Width="7%" Height="8%"  runat="server" ToolTip="Atou" CssClass="PointLabel"></asp:Label>
                </span>
                <span class="MsgSpan" align="center" valign="middle">        
                    <asp:TextBox ID="tMsg" runat="server" ToolTip="text message" Width="64%" Height="8%" CssClass="MsgTextBox">Short Information</asp:TextBox>
                </span>       
            </div>
            <pre id="preOut" class="PreFormated" runat="server" visible="false">
            </pre>
            <div id="SchnapsFooterDiv" class="SchnapsFooter" align="left">
                <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="https://github.com/heinrichelsigan" target="_blank">github.com/heinrichelsigan</a>/<a href="https://github.com/heinrichelsigan/schnapslet" target="_blank">schnapslet</a>]                
            </div>    
        </form>
    </body>
</html>
