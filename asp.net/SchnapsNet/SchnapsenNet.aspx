<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchnapsenNet.aspx.cs" Inherits="SchnapsNet.SchnapsenNet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Schnapsen.Net</title>
    <script>
        var atouUrl = "";

        function sleep(ms) {
            setTimeout(codingCourse, 3000);
        }
            
        function codingCourse() {
            console.log("freeCodeCamp");
        }


        function schnapsStateInit() {
            const myUrl2 = new URL(window.location.toLocaleString());
            // alert(myUrl2);

            var myUrl = new URL(myUrl2);
            var initSchnapsState = myUrl.searchParams.get("initState");
            console.log(initSchnapsState);            

            schnapsStateSwitch(0);

            if (initSchnapsState != null) {                

                if (initSchnapsState == 4) {
                    initSchnapsState4();
                }
                if (initSchnapsState == 7) {
                    initSchnapsState7();
                }
                if (initSchnapsState == 8) {
                    initSchnapsState8();                    
                }   
                if (initSchnapsState == 15) {
                    schnapsStateRedirect();
                }
            }
        }

        function initSchnapsState4() {
            atouUrl = document.getElementById("imAtou10").src;
            // alert(atouUrl);
            setTimeout(allInvisibleInit, 100);
            setTimeout(playerCards1st3Visible, 1000);
            setTimeout(imOut20a3, 2000);
            setTimeout(imOut21Empty, 3000);
            setTimeout(atouCardVisible, 3250);
            setTimeout(playerCardsVisible, 4250);
            setTimeout(imOut21a2, 5250);
            setTimeout(talonCardVisible, 6250);
            setTimeout(imOut21Empty, 6500);            
            setTimeout(schnapsStateRedirect15, 7500);  
        }

        function initSchnapsState7() {
            atouUrl = document.getElementById("imAtou10").src;
            // alert(atouUrl);
            setTimeout(allInvisibleInit, 100);
            setTimeout(imOut20a3, 1000);
            setTimeout(playerCards1st3Visible, 2000);            
            setTimeout(imOut21Empty, 2250);
            setTimeout(atouCardVisible, 3000);
            setTimeout(imOut21a2, 4000);
            setTimeout(playerCardsVisible, 5000);
            setTimeout(imOut21Empty, 5250);
            setTimeout(talonCardVisible, 6500);
            setTimeout(schnapsStateRedirect15, 7500);
        }

        function initSchnapsState8() {
            atouUrl = document.getElementById("imAtou10").src;
            // alert(atouUrl);
            setTimeout(allInvisibleInit(), 100);
            // schnapsStateSwitch(3);
            setTimeout(playerCardsVisible, 1000);
            setTimeout(atouCardVisible, 2000);
            setTimeout(imOut21a2, 3000);
            setTimeout(imOut21Empty, 4000);
            setTimeout(talonCardVisible, 4250);
            setTimeout(schnapsStateRedirect15, 5000);
        }

        function schnapsStateSwitch(stage) {
            if (stage == null)
                stage = 0;

            if (stage == 0) {
                if (document.getElementById("imMerge11") != null)
                    document.getElementById("imMerge11").style.visibility = "visible";                
            }
            if (stage == 1) {
                allInvisibleInit();
            }
            if (stage == 2) {
                playerCardsVisible();
            }
            if (stage == 3) {
                playerCards1st3Visible();
            }
            if (stage == 10) {
                atouCardVisible();
            }
            if (stage == 11) {
                talonCardVisible();
            }
            if (stage == 200) {
                imOut20Empty();
            }
            if (stage == 201) {
                imOut20a3();
            }
            if (stage == 210) {
                imOut21Empty();
            }
            if (stage == 211) {
                imOut21a2();
            }
            if (stage == 1000) {
                schnapsStateRedirect15();
            }
        }


        function allInvisibleInit() {            
            if (document.getElementById("imOut21") != null)
                document.getElementById("imOut21").style.visibility = "hidden";
            if (document.getElementById("imOut20") != null)
                document.getElementById("imOut20").style.visibility = "hidden";
            if (document.getElementById("imMerge11") != null)
                document.getElementById("imMerge11").style.visibility = "hidden";            
            document.getElementById("im0").style.visibility = "hidden";
            document.getElementById("im1").style.visibility = "hidden";
            document.getElementById("im2").style.visibility = "hidden";
            document.getElementById("im3").style.visibility = "hidden";
            document.getElementById("im4").style.visibility = "hidden";
            document.getElementById("spanAtou").style.visibility = "hidden";
            document.getElementById("spanTalon").style.visibility = "hidden";
        }

        function playerCardsVisible() {
            document.getElementById("im0").style.visibility = "visible";
            document.getElementById("im1").style.visibility = "visible";
            document.getElementById("im2").style.visibility = "visible";
            document.getElementById("im3").style.visibility = "visible";
            document.getElementById("im4").style.visibility = "visible";
        }

        function playerCards1st3Visible() {
            document.getElementById("im0").style.visibility = "visible";
            document.getElementById("im1").style.visibility = "visible";
            document.getElementById("im2").style.visibility = "visible";
        }

        function atouCardVisible() {            
            if (document.getElementById("spanAtou") != null)
                document.getElementById("spanAtou").style.visibility = "visible";
            if (document.getElementById("imAtou10") != null) {
                document.getElementById("imAtou10").style.visibility = "visible";
                document.getElementById("imAtou10").src = atouUrl;
            }
        }

        function talonCardVisible() {
            if (document.getElementById("spanTalon") != null)
                document.getElementById("spanTalon").style.visibility = "visible";
            if (document.getElementById("imTalon") != null)
                document.getElementById("imTalon").style.visibility = "visible";
        }

        function imOut20Empty() {
            document.getElementById("imOut20").src = "https://area23.at/mono/SchnapsNet/cardpics/e.gif";
        }

        function imOut21Empty() {
            if (document.getElementById("imOut21") != null)
                document.getElementById("imOut21").style.visibility = "visible";
            if (document.getElementById("imOut20") != null)
                document.getElementById("imOut20").style.visibility = "visible";
            document.getElementById("imOut21").src = "https://area23.at/mono/SchnapsNet/cardpics/e.gif";
            document.getElementById("imOut20").src = "https://area23.at/mono/SchnapsNet/cardpics/e.gif";            
        }

        function imOut20a3() {            
            if (document.getElementById("imOut21") != null) {
                document.getElementById("imOut21").style.visibility = "visible";
                document.getElementById("imOut21").src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
            }                
            if (document.getElementById("imOut20") != null) {
                document.getElementById("imOut20").style.visibility = "visible";
                document.getElementById("imOut20").src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
            }                        
            if (document.getElementById("imAtou10") != null) {
                document.getElementById("imAtou10").src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
            }
            if (document.getElementById("spanAtou") != null)
                document.getElementById("spanAtou").style.visibility = "visible";
            if (document.getElementById("imAtou10") != null) {
                document.getElementById("imAtou10").style.visibility = "visible";
                document.getElementById("imAtou10").src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
            }            
        }

        function imOut21a2() {
            if (document.getElementById("imOut21") != null)
                document.getElementById("imOut21").style.visibility = "visible";
            if (document.getElementById("imOut20") != null)
                document.getElementById("imOut20").style.visibility = "visible";
            document.getElementById("imOut20").src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
            document.getElementById("imOut21").src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
        }

        function schnapsStateRedirect() {
            // alert("SchnapsenNet.aspx");
            window.location.href = "SchnapsenNet.aspx";
        }

        function schnapsStateRedirect15() {
            // alert("SchnapsenNet.aspx?initState=15");
            window.location.href = "SchnapsenNet.aspx?initState=15";
        }

    </script>
</head>
<body onload="schnapsStateInit()">
    <script>
        window.loaded = schnapsStateInit();
    </script>
    <form id="form1" runat="server">        
        <div id="DivSchnapsButtons" style="line-height: normal; min-height: 40px; min-width: 400px; height: 8%; margin-top: 8px; vertical-align:middle; width: 100%; font-size: larger; background-color: #fefb56; background-image: url('cardpics/schnapslet248.gif'); table-layout: fixed; inset-block-start: initial;">            
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: large" align="left" valign="middle">
                <asp:Button ID="bContinue" Width="15%" Height="8%" runat="server" ToolTip="Continue" style="min-height: 40px; min-width: 56px; font-size: large" Text="Continue" OnClick="bContinue_Click" Enabled="false" />&nbsp;
            </span>                        
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: large" align="left" valign="middle">
                <asp:Button ID="bChange" Width="15%" Height="8%" runat="server" ToolTip="Change Atou" style="min-height: 40px; min-width: 56px; font-size:large" Text="Change Atou Card" OnClick="bChange_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: large" align="left" valign="middle">
                <asp:Button ID="b20a" Width="15%" Height="8%" runat="server" ToolTip="Say marriage 20" style="min-height: 40px; min-width: 56px; font-size: large" Text="Marriage 20" OnClick="b20a_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: large" align="right" valign="middle">
                <asp:Button ID="b20b" Width="15%" Height="8%" runat="server" ToolTip="Say marriage 40"  style="min-height: 40px; min-width: 56px; font-size: large" Text="Marriage 40" OnClick="b20b_Click" Enabled="false" />&nbsp;                
            </span>   
            <span style="min-height: 40px; min-width: 60px; width:12%; height: 8%; vertical-align:middle; text-align: left; font-size: large" align="left" valign="middle">
                <asp:Button ID="bMerge" Width="12%" Height="8%" runat="server" ToolTip="Start" style="min-height: 40px; min-width: 40px; font-size: large" Text="Start"  OnClick="bMerge_Click" Visible="true" Enabled="true" />
                <asp:Button ID="bStop" Width="10%" Height="8%" runat="server" ToolTip="Stop"  style="min-height: 40px; min-width: 36px; font-size: large" Text="Stop" OnClick="bStop_Click" Visible="false" Enabled="true" />
            </span>
            <span style="min-height: 40px; min-width: 36px; width:10%; height: 8%; vertical-align:middle; text-align: left; font-size: large" align="right" valign="middle">
                <asp:Button ID="bHelp" Width="10%" Height="8%" runat="server" ToolTip="Help"  style="min-height: 40px; min-width: 36px; font-size: large" Text="Help" OnClick="bHelp_Click" Enabled="true" /> 
            </span>            
        </div>
        <div id="DivSchnapsStack" style="line-height: normal; min-height: 96px; min-width: 72px; height:10%; width: 100%; margin-top: 8px; font-size: medium; background-color: #fefb56; table-layout: fixed; inset-block-start: auto;">
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="imOut21" runat="server" ImageUrl="~/cardpics/e.gif" Width="15%" Height="10%" OnClick="ImOut_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imOut20" runat="server" ImageUrl="~/cardpics/e.gif" Width="15%" Height="10%"  OnClick="ImOut_Click"  />
            </span>
            <span id="spanMerge" runat="server" style="visibility: visible; min-height: 96px; min-width: 96px; height:10%; width:20%; margin-left: 0px; margin-top: 0px; z-index: 10; text-align: left; font-size: medium">
                <asp:ImageButton ID="imMerge11" runat="server" ImageUrl="~/cardpics/mergeshort.gif" Width="20%" style="z-index: 2" OnClick="imMerge11_Click" BorderStyle="None" />&nbsp;
            </span>
            <span id="spanAtou" runat="server" style="visibility: hidden; min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; z-index: 10; text-align: left; font-size: medium">
                <asp:ImageButton ID="imAtou10" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" style="z-index: 1" />
            </span>
            <span id="spanTalon" runat="server" style="visibility: hidden; min-height: 72px; min-width: 96px; height:8%; width:18%; margin-left: -6%; margin-top: 2%; z-index: 100; text-align: left; vertical-align: top; font-size: medium">                
                <asp:Image ID="imTalon" runat="server" ImageUrl="~/cardpics/t.gif" style="width:18%; margin-top: 2%; z-index: 110; tab-size: inherit" Width="18%" />
            </span>               
            <span id="spanComputerStitches" runat="server" visible="false" style="visibility: hidden; min-height: 96px; min-width: 96px; height:10%; width:18%; margin-left: 0px; margin-top: 0px;  z-index: 10; text-align: right; font-size: medium">
                <asp:ImageButton ID="ImageComputerStitch0a" runat="server" Visible="false" ImageUrl="~/cardpics/n1.gif" Width="15%" style="z-index: 2" BorderStyle="None" OnClick="ImageComputerStitch_Click" />
                <asp:ImageButton ID="ImageComputerStitch0b" runat="server" Visible="false" ImageUrl="~/cardpics/n1.gif" Width="15%" style="z-index: 2; margin-left: -12%; margin-top: 1px" BorderStyle="None" OnClick="ImageComputerStitch_Click" />
            </span>            
        </div>
        <div id="DivPlayerStack" style="line-height: normal; min-height: 96px; min-width: 72px; height:10%; width: 100%; font-size: medium; background-color: #fefb56; table-layout: fixed; inset-block-start: auto">
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
        <div style="line-height: normal; vertical-align:middle; height: 8%; width: 100%; font-size: larger; margin-top: 8px; background-color: #fefb56; table-layout: fixed; inset-block-start: initial">
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
        <pre id="preOut" style="width: 100%; height: 12%; visibility: visible; font-size: large; scroll-behavior: auto;" runat="server">
        </pre>
        <div align="left" style="text-align: left; width: 100%; height: 8%; visibility: inherit; background-color: #bfbfbf; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
            <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://darkstar.work">darkstar.work</a>            
        </div>    
    </form>
</body>
</html>
