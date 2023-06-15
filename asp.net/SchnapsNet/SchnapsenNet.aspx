<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchnapsenNet.aspx.cs" Inherits="SchnapsNet.SchnapsenNet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Schnapsen.Net</title>
    <script>
        function sleep(ms) {
            setTimeout(codingCourse, 3000);
        }
            
        function codingCourse() {
            console.log("freeCodeCamp");
        }


        function schnapsStateInit() {
            const myUrl2 = new URL(window.location.toLocaleString());
            // alert(myUrl2);

            var url = new URL(myUrl2);

            var initSchnapsState = url.searchParams.get("initState");
            console.log(initSchnapsState);            

            if (document.getElementById("ImageMerge") != null)
                document.getElementById("ImageMerge").style.visibility = "visible";

            if (initSchnapsState != null) {                

                if (initSchnapsState == 4) {
                    if (document.getElementById("ImageMerge") != null)
                        document.getElementById("ImageMerge").style.visibility = "hidden";
                    document.getElementById("im0").style.visibility = "visible";
                    document.getElementById("im1").style.visibility = "visible";
                    document.getElementById("im2").style.visibility = "visible";
                    sleep(1000);
                    document.getElementById("imOut20").src = "cardpics/a3.gif";
                    sleep(1000);
                    if (document.getElementById("imAtou10") != null)
                        document.getElementById("imAtou10").style.visibility = "visible";
                    sleep(500);
                    document.getElementById("im3").style.visibility = "visible";
                    document.getElementById("im4").style.visibility = "visible";
                    sleep(1000);
                    document.getElementById("imOut21").src = "cardpics/a2.gif";
                    sleep(1000);
                    document.getElementById("imOut20").src = "https://area23.at/schnapsen/cardpics/e.gif";
                    document.getElementById("imOut21").src = "https://area23.at/schnapsen/cardpics/e.gif";
                    if (document.getElementById("imTalon") != null)
                        document.getElementById("imTalon").style.visibility = "visible";
                    sleep(1000);
                    window.location.href = "SchnapsenNet.aspx"
                }
                if (initSchnapsState == 8) {
                    if (document.getElementById("ImageMerge") != null)
                        document.getElementById("ImageMerge").style.visibility = "hidden";

                    document.getElementById("im0").style.visibility = "visible";
                    document.getElementById("im1").style.visibility = "visible";
                    document.getElementById("im2").style.visibility = "visible";
                    document.getElementById("im3").style.visibility = "visible";
                    document.getElementById("im4").style.visibility = "visible";
                    sleep(1000);
                    if (document.getElementById("imAtou10") != null)
                        document.getElementById("imAtou10").style.visibility = "visible";
                    sleep(1000);
                    document.getElementById("imOut20").src = "cardpics/a3.gif";
                    document.getElementById("imOut21").src = "cardpics/a2.gif";
                    sleep(1000);
                    document.getElementById("imOut20").src = "https://area23.at/schnapsen/cardpics/e.gif";
                    document.getElementById("imOut21").src = "https://area23.at/schnapsen/cardpics/e.gif";
                    if (document.getElementById("imTalon") != null)
                        document.getElementById("imTalon").style.visibility = "visible";
                    sleep(1000);
                    window.location.href = "SchnapsenNet.aspx"
                }
            }
        }
    </script>
</head>
<body onload="schnapsStateInit()">
    <script>
        window.loaded = schnapsStateInit();
    </script>
    <form id="form1" runat="server">        
        <div id="DivSchnapsButtons" style="line-height: normal; min-height: 40px; min-width: 400px; height: 8%; margin-top: 8px; vertical-align:middle; width: 100%; font-size: larger; table-layout: fixed; inset-block-start: initial">            
            <span style="min-height: 40px; min-width: 60px; width:12%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="left" valign="middle">
                <asp:Button ID="bMerge" Width="12%" Height="8%" runat="server" ToolTip="Start" style="min-height: 40px; min-width: 40px; font-size: x-large" Text="Start"  OnClick="bMerge_Click" Enabled="true" />
            </span>
            <span style="min-height: 40px; min-width: 36px; width:10%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="right" valign="middle">
                <asp:Button ID="bStop" Width="10%" Height="8%" runat="server" ToolTip="Stop"  style="min-height: 40px; min-width: 36px; font-size: x-large" Text="Stop" OnClick="bStop_Click" Enabled="true" />
            </span>
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="left" valign="middle">
                <asp:Button ID="bContinue" Width="15%" Height="8%" runat="server" ToolTip="Continue" style="min-height: 40px; min-width: 56px; font-size: x-large" Text="Continue" OnClick="bContinue_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="left" valign="middle">
                <asp:Button ID="bChange" Width="15%" Height="8%" runat="server" ToolTip="Change Atou" style="min-height: 40px; min-width: 56px; font-size: x-large" Text="Change Atou Card" OnClick="bChange_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="left" valign="middle">
                <asp:Button ID="b20a" Width="15%" Height="8%" runat="server" ToolTip="Say marriage 20" style="min-height: 40px; min-width: 56px; font-size: x-large" Text="Marriage 20" OnClick="b20a_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="right" valign="middle">
                <asp:Button ID="b20b" Width="15%" Height="8%" runat="server" ToolTip="Say marriage 40"  style="min-height: 40px; min-width: 56px; font-size: x-large" Text="Marriage 40" OnClick="b20b_Click" Enabled="false" />&nbsp;                
            </span>            
            <span style="min-height: 40px; min-width: 36px; width:10%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="right" valign="middle">
                <asp:Button ID="bHelp" Width="10%" Height="8%" runat="server" ToolTip="Help"  style="min-height: 40px; min-width: 36px; font-size: x-large" Text="Help" OnClick="bHelp_Click" Enabled="true" />                
            </span>            
        </div>
        <div id="DivSchnapsStack" style="line-height: normal; min-height: 96px; min-width: 72px; height:10%; width: 100%; margin-top: 8px; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="imOut21" runat="server" ImageUrl="~/cardpics/e.gif" Width="15%" Height="10%" OnClick="ImOut1_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imOut20" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%"  OnClick="ImOut0_Click"  />
            </span>
            <asp:PlaceHolder ID="PlaceHolderAtouTalon" runat="server">
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px;  z-index: 10;  margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imAtou10" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" style="z-index: 1" />
            </span>
            <span style="min-height: 72px; min-width: 96px; height:8%; width:18%; margin-left: -6%; margin-top: 2%; z-index: 100; text-align: left; vertical-align: top; font-size: medium">                
                <asp:Image ID="imTalon" runat="server" ImageUrl="~/cardpics/t.gif" style="width:18%; margin-top: 2%; z-index: 110; tab-size: inherit" Width="18%" />
            </span>   
            </asp:PlaceHolder>
            <span style="visibility: visible; min-height: 96px; min-width: 96px; height:10%; width:20%; z-index: 10;  margin-top: 0px; text-align: left; font-size: medium">
                <asp:Image ID="ImageMerge" runat="server" ImageUrl="~/cardpics/mergeshort.gif" Width="20%" style="z-index: 2" BorderStyle="None" />&nbsp;
            </span>
            <asp:PlaceHolder ID="PlaceHolderComputerStitches" runat="server" Visible="false">
            <span style="visibility: visible; min-height: 96px; min-width: 96px; height:10%; width:18%; margin-left: 0px; margin-top: 0px;  z-index: 10;  margin-top: 0px; text-align: right; font-size: medium">
                <asp:ImageButton ID="ImageComputerStitch0a" runat="server" ImageUrl="~/cardpics/n1.gif" Width="15%" style="z-index: 2" BorderStyle="None" OnClick="ImageComputerStitch_Click" />
                <asp:ImageButton ID="ImageComputerStitch0b" runat="server" ImageUrl="~/cardpics/n1.gif" Width="15%" style="z-index: 2; margin-left: -12%; margin-top: 1px" BorderStyle="None" OnClick="ImageComputerStitch_Click" />
            </span>
            </asp:PlaceHolder>
        </div>
        <div id="DivPlayerStack" style="line-height: normal; min-height: 96px; min-width: 72px; height:10%; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="im0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im1" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im2" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%;  margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im3" runat="server" ImageUrl="~/cardpics/n0.gif"  Width="15%" Height="10%" OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im4" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%"  OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 120px; height:10%; width:25%; margin-left: 0px; margin-top: 0px; text-align: left; visibility: visible; font-size: medium">
                <asp:Table ID="tableTournement" runat="server" style="display:inline-table; font-size: large; vertical-align:top; text-align: right">
                    <asp:TableHeaderRow style="border-bottom: thick">
                        <asp:TableCell style="border-bottom: thick; border-right: medium">You</asp:TableCell>
                        <asp:TableCell style="border-bottom: thick; border-left: medium">Computer</asp:TableCell>
                    </asp:TableHeaderRow>
                </asp:Table>                
            </span>
        </div>        
        <div style="line-height: normal; vertical-align:middle; height: 8%; width: 100%; font-size: larger; margin-top: 8px; table-layout: fixed; inset-block-start: initial">
            <span style="width:6%; vertical-align:middle; text-align: left; font-size: x-large; height: 8%;" align="left" valign="middle">
                <asp:TextBox ID="tPoints" Width="6%" Height="8%"  runat="server" ToolTip="text message" style="min-height: 40px; min-width: 32px; font-size: x-large" Enabled="false">0</asp:TextBox>                
            </span>
            <span style="width:8%; vertical-align:middle; text-align: left; font-size: x-large; height: 8%;" align="right" valign="middle">
                <asp:Label ID="lPoints" Width="8%" Height="8%"  runat="server" ToolTip="Points" style="min-height: 40px; min-width: 40px; font-size: x-large;">Points</asp:Label>
            </span>
            <span style="width:75%; vertical-align:middle; text-align: left; font-size: larger; height: 8%;" align="middle" valign="middle">            
                <asp:TextBox ID="tMsg" runat="server" ToolTip="text message" Width="75%" Height="8%" style="font-size: larger">Short Information</asp:TextBox>
            </span>            
        </div>
        <pre id="preOut" style="width: 100%; height: 12%; visibility: visible; font-size: large; scroll-behavior: auto;" runat="server">
        </pre>
        <asp:PlaceHolder ID="PlaceHolderPlayerStitches" runat="server" Visible="false">
            <span style="min-height: 96px; min-width: 120px; height:10%; width:25%; margin-left: 0px; margin-top: 0px; text-align: left; visibility: visible; font-size: medium">    
                <asp:ImageButton ID="ImagePlayerStitch0a" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" OnClick="ImagePlayerStitch_Click" />
                <asp:ImageButton ID="ImagePlayerStitch0b" runat="server" ImageUrl="~/cardpics/n1.gif" Width="15%" style="z-index: 2; margin-left: -10%; margin-top: 1px" BorderStyle="None" OnClick="ImagePlayerStitch_Click" />            
            </span>
        </asp:PlaceHolder>
        <div align="left" style="text-align: left; width: 100%; height: 8%; visibility: inherit; background-color: #bfbfbf; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
            <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://@arkstar.work">darkstar.work</a>            
        </div>    
    </form>
</body>
</html>
