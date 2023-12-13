using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SchnapsNet.ConstEnum;
using SchnapsNet.Models;
using SchnapsNet.Utils;

namespace SchnapsNet
{
    public partial class Schnapsen3er : Area23BasePage
    {
        long errNum = 0; // Errors Ticker
        int ccard = -1; // Computers Card played
        Models.Card emptyTmpCard, playedOutCard0, playedOutCard1;
        volatile byte psaychange = 0;

        public override void InitSchnaps()
        {
            base.InitSchnaps();

            preOut.InnerText = "";
            // tMsg.Enabled = false;

            im0.ImageUrl = emptyURL.ToString();
            im1.ImageUrl = emptyURL.ToString();
            im2.ImageUrl = emptyURL.ToString();
            im3.ImageUrl = emptyURL.ToString();
            im4.ImageUrl = emptyURL.ToString();

            imOut20.ImageUrl = emptyURL.ToString();
            imOut21.ImageUrl = emptyURL.ToString();
            imTalon.ImageUrl = emptyTalonUri.ToString();
            imTalon.Visible = true;
            imAtou10.ImageUrl = emptyURL.ToString();

            bMerge.Text = ResReader.GetValue("bStart_text", Locale.TwoLetterISOLanguageName);
            bStop.Text = ResReader.GetValue("bStop_text", Locale.TwoLetterISOLanguageName);
            bStop.Enabled = false;
            b20a.Text = ResReader.GetValue("b20a_text", Locale.TwoLetterISOLanguageName);
            b20a.Enabled = false;

            

            tPoints.Enabled = false;
            tPoints.Text = ResReader.GetValue("tPoints_text", Locale.TwoLetterISOLanguageName);
            bContinue.Text = ResReader.GetValue("bContinue_text", Locale.TwoLetterISOLanguageName);
            bContinue.Enabled = false;

            bHelp.Text = ResReader.GetValue("bHelp_text", Locale.TwoLetterISOLanguageName);
            bHelp.ToolTip = ResReader.GetValue("bHelp_text", Locale.TwoLetterISOLanguageName);

            // tRest.Enabled = false;
            // tRest.Text = ResReader.GetValue("tRest_text", Locale.TwoLetterISOLanguageName);            
            // lRest.Text = ResReader.GetValue("sRest", Locale.TwoLetterISOLanguageName);

            this.imOut20.ToolTip = ResReader.GetValue("imageMerge_ToolTip", Locale.TwoLetterISOLanguageName);
            this.imOut21.ToolTip = ResReader.GetValue("imageMerge_ToolTip", Locale.TwoLetterISOLanguageName);
            this.imMerge11.ToolTip = ResReader.GetValue("imageMerge_ToolTip", Locale.TwoLetterISOLanguageName);

            lPoints.Text = ResReader.GetValue("sPoints", Locale.TwoLetterISOLanguageName);

            tMsg.Enabled = false;
            tMsg.Text = ResReader.GetValue("clickon_start", Locale.TwoLetterISOLanguageName);
            tMsg.Visible = true;

            ShowStitches(-3);
        }

        public override void RefreshGlobalVariableSession()
        {
            base.RefreshGlobalVariableSession();

            string saveFileName = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
    "Schnapsen_" +
    DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" +
    DateTime.Now.Day.ToString() + "_"
     + Context.Session.SessionID + "_" + DateTime.Now.Hour + DateTime.Now.Minute +
     DateTime.Now.Second +
    ".json");

            // string jsonString = JsonConvert.SerializeObject(globalVariable);
            // System.IO.File.WriteAllText(saveFileName, jsonString);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (globalVariable == null)
            {
                if (!Page.IsPostBack)
                {
                    InitSchnaps();
                }

                if (this.Context.Session[Constants.APPNAME] == null)
                {
                    string initMsg = "New connection started from " + Request.UserHostAddress + " " + Request.UserHostName + " with " + Request.UserAgent + "!";
                    Log(initMsg);
                    string preMsg = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss \t");
                    string appPath = HttpContext.Current.Request.ApplicationPath;
                    Log("AppPath=" + appPath + " logging to " + Logger.LogFile);
                    globalVariable = new Utils.GlobalAppSettings(this.Context, this.Session);
                    aTournement = new Tournament();
                    globalVariable.Tournement = aTournement;
                    this.Context.Session[Constants.APPNAME] = globalVariable;
                }
                else
                {
                    globalVariable = (GlobalAppSettings)this.Context.Session[Constants.APPNAME];
                }
            }

            if (aTournement == null)
                aTournement = globalVariable.Tournement;
            if (aGame == null)
                aGame = globalVariable.Game;
            
            DrawPointsTable();

            ShowStateSchnapsStack();            
            
        }

        protected void ShowPlayersCards(SCHNAPSTATE gameState)
        {
            int schnapStateVal = SCHNAPSTATE_Extensions.StateValue(gameState);
            if (schnapStateVal >= 15 && schnapStateVal < 22 && gameState != SCHNAPSTATE.GAME_START)
            {
                try
                {
                    imOut20.Style["visibility"] = "visible";
                    imOut21.Style["visibility"] = "visible";
                    im0.Style["visibility"] = "visible";
                    im1.Style["visibility"] = "visible";
                    im2.Style["visibility"] = "visible";
                    im3.Style["visibility"] = "visible";
                    im4.Style["visibility"] = "visible";
                    im0.ImageUrl = aGame.gambler.hand[0].PictureUrlString;
                    im1.ImageUrl = aGame.gambler.hand[1].PictureUrlString;
                    im2.ImageUrl = aGame.gambler.hand[2].PictureUrlString;
                    im3.ImageUrl = aGame.gambler.hand[3].PictureUrlString;
                    im4.ImageUrl = aGame.gambler.hand[4].PictureUrlString;
                }
                catch (Exception exp)
                {
                    this.ErrHandler(exp);
                }
            }
            else
            {
                if ((SCHNAPSTATE_Extensions.StateValue(gameState) < 4) ||
                    (gameState == SCHNAPSTATE.NONE))
                {
                    imOut20.Style["visibility"] = "hidden";
                    imOut21.Style["visibility"] = "hidden";

                    if ((gameState == SCHNAPSTATE.NONE) ||
                        (gameState == SCHNAPSTATE.GAME_START))
                    {
                        imOut20.Style["visibility"] = "visible";
                        imOut21.Style["visibility"] = "visible";
                    }

                    im0.Style["visibility"] = "hidden";
                    im0.ImageUrl = emptyURL.ToString();
                    im1.Style["visibility"] = "hidden";
                    im1.ImageUrl = emptyURL.ToString();
                    im2.Style["visibility"] = "hidden";
                    im2.ImageUrl = emptyURL.ToString();
                    im3.Style["visibility"] = "hidden";
                    im3.ImageUrl = emptyURL.ToString();
                    im4.Style["visibility"] = "hidden";
                    im4.ImageUrl = emptyURL.ToString();
                }
                else if ((gameState == SCHNAPSTATE.PLAYER_2ND_2) ||
                        (gameState == SCHNAPSTATE.PLAYER_1ST_3) ||
                        (gameState == SCHNAPSTATE.PLAYER_1ST_5) ||
                        (gameState == SCHNAPSTATE.GIVE_PLAYER) ||
                        (gameState == SCHNAPSTATE.PLAYER_FIST) ||
                        (gameState == SCHNAPSTATE.PLAYER_TAKES))
                {
                    imOut20.Style["visibility"] = "hidden";
                    imOut21.Style["visibility"] = "hidden";
                    im0.ImageUrl = aGame.gambler.hand[0].PictureUrlString;
                    im0.Style["visibility"] = "hidden";
                    im1.ImageUrl = aGame.gambler.hand[1].PictureUrlString;
                    im1.Style["visibility"] = "hidden";
                    im2.ImageUrl = aGame.gambler.hand[2].PictureUrlString;
                    im2.Style["visibility"] = "hidden";
                    im3.ImageUrl = aGame.gambler.hand[3].PictureUrlString;
                    im3.Style["visibility"] = "hidden";
                    im4.ImageUrl = aGame.gambler.hand[4].PictureUrlString;
                    im4.Style["visibility"] = "hidden";
                }                  
            }
        }

        /// <summary>
        /// showPlayedOutCards - shows playedOutCards => needed when changing locale and card deck
        /// </summary>
        protected void showPlayedOutCards()
        {
            if ((aGame != null && aGame.playedOut0 != null && playedOutCard0 != null &&
                aGame.playedOut0.ColorValue != playedOutCard0.ColorValue) ||
                    (aGame != null && aGame.playedOut0 != null && playedOutCard0 == null))
            {
                playedOutCard0 = aGame.playedOut0;
            }
            if (aGame == null && playedOutCard0 == null)
                playedOutCard0 = globalVariable.CardEmpty;
            imOut20.ImageUrl = playedOutCard0.PictureUrlString;

            if ((aGame != null && aGame.playedOut1 != null && playedOutCard1 != null &&
                aGame.playedOut1.ColorValue != playedOutCard1.ColorValue) ||
                    (aGame != null && aGame.playedOut1 != null && playedOutCard1 == null))
            {
                playedOutCard1 = aGame.playedOut1;
            }
            if (playedOutCard1 == null)
                playedOutCard1 = globalVariable.CardEmpty;
            imOut21.ImageUrl = playedOutCard1.PictureUrlString;
        }

        protected void ShowMergeAnim(SCHNAPSTATE gameState)
        {
            try
            {
                if (gameState == SCHNAPSTATE.GAME_START || gameState == SCHNAPSTATE.NONE ||
                gameState == SCHNAPSTATE.MERGE_COMPUTER || gameState == SCHNAPSTATE.MERGE_PLAYER ||
                gameState == SCHNAPSTATE.MERGING_CARDS)
                {
                    this.spanMerge.Visible = true;
                    this.spanMerge.Style["visibility"] = "visible";
                    this.imMerge11.Visible = true;
                    this.imMerge11.Style["visibility"] = "visible";
                    // PlaceHolderMerge.Visible = true;
                }
                else
                {
                    this.spanMerge.Style["visibility"] = "hidden";
                    imMerge11.Visible = false;
                    // PlaceHolderMerge.Visible = false;
                }
            }
            catch (Exception mergeAnimEx)
            {
                this.ErrHandler(mergeAnimEx);
            }
        }

        protected void ShowAtouCard(SCHNAPSTATE gameState)
        {
            try
            {
                int schnapStateVal = SCHNAPSTATE_Extensions.StateValue(gameState);                

                if (schnapStateVal >= 10 && schnapStateVal < 20)
                {
                    this.spanAtou.Visible = true;
                    this.spanAtou.Style["visibility"] = "visible";
                    imAtou10.Visible = true;

                    if (gameState == SCHNAPSTATE.GAME_CLOSED)
                    {
                        imAtou10.ImageUrl = notURL.ToString();
                        imAtou10.ToolTip = ResReader.GetValue("imageAtou_AltText", globalVariable.ISO2Lang);
                    }
                    else
                    {
                        imAtou10.ImageUrl = aGame.set[19].PictureUrlString;
                        imAtou10.ToolTip = ResReader.GetValue("imageAtou_ToolTip", globalVariable.ISO2Lang);
                    }                    
                }
                else
                {
                    this.spanAtou.Style["visibility"] = "hidden";
                    this.imAtou10.ToolTip = "";
                    this.imAtou10.ImageUrl = emptyURL.ToString();

                    if (gameState == SCHNAPSTATE.GAME_START ||
                        gameState == SCHNAPSTATE.MERGING_CARDS ||
                        gameState == SCHNAPSTATE.MERGE_PLAYER ||
                        gameState == SCHNAPSTATE.MERGE_COMPUTER ||
                        gameState == SCHNAPSTATE.TALON_CONSUMED ||
                        gameState == SCHNAPSTATE.ZERO_CARD_REMAINS ||
                        gameState == SCHNAPSTATE.NONE)
                    {
                        this.imAtou10.Visible = false;                        
                    }
                    else if (
                        gameState == SCHNAPSTATE.PLAYER_1ST_3 ||
                        gameState == SCHNAPSTATE.COMPUTER_1ST_3 ||
                        gameState == SCHNAPSTATE.PLAYER_1ST_5 ||
                        gameState == SCHNAPSTATE.GIVE_PLAYER ||
                        gameState == SCHNAPSTATE.PLAYER_TAKES ||
                        gameState == SCHNAPSTATE.PLAYER_FIST)                        
                    {
                        imAtou10.ImageUrl = aGame.set[19].PictureUrlString;
                        this.imAtou10.Visible = true;
                    }

                }
            }
            catch (Exception exAtou1)
            {
                this.ErrHandler(exAtou1);
            }
        }

        protected void ShowTalonCard(SCHNAPSTATE gameState)
        {
            try
            {
                this.spanTalon.Visible = true;                
                int schnapStateVal = SCHNAPSTATE_Extensions.StateValue(gameState);
                if (schnapStateVal >= 15 && schnapStateVal < 20)
                {
                    imTalon.ImageUrl = talonURL.ToString();
                    imTalon.Style["visibility"] = "visible";
                    this.spanTalon.Style["visibility"] = "visible";
                    this.spanTalon.Style["margin-left"] = "-6%";
                }
                else
                {
                    if (gameState == SCHNAPSTATE.GAME_START || gameState == SCHNAPSTATE.NONE)
                        imTalon.ImageUrl = emptyTalonUri.ToString();
                    else 
                        imTalon.ImageUrl = talonURL.ToString();

                    imTalon.Style["visibility"] = "hidden";
                    this.spanTalon.Style["margin-left"] = "0px";
                    this.spanTalon.Style["visibility"] = "hidden";                    
                }
            }
            catch (Exception imTalonEx)
            {
                ErrHandler(imTalonEx);
            }
        }

        protected void ShowFistOrHand(SCHNAPSTATE gameState)
        {
            im0.ImageUrl = emptyURL.ToString();
            im1.ImageUrl = emptyURL.ToString();
            im2.ImageUrl = emptyURL.ToString();
            im3.ImageUrl = emptyURL.ToString();
            im4.ImageUrl = emptyURL.ToString();

            imOut20.ImageUrl = notURL.ToString();
            imOut21.ImageUrl = notURL.ToString();
            imOut21.Style["visibility"] = "visible";
            imOut20.Style["visibility"] = "visible";

            int schnapStateVal = SCHNAPSTATE_Extensions.StateValue(gameState);
            if (schnapStateVal > 1 && schnapStateVal <= 8)
            {
                imOut20.ImageUrl = "https://area23.at/mono/SchnapsNet/cardpics/" + "a0.gif";
                imOut20.ToolTip = ResReader.GetValue("image_take", globalVariable.ISO2Lang);
                imOut21.ImageUrl = "https://area23.at/mono/SchnapsNet/cardpics/" + "f0.gif";
                imOut21.ToolTip = ResReader.GetValue("image_fist", globalVariable.ISO2Lang);
            }
        }

        protected void ShowStitches(int whichStitch)
        {
            if (aGame != null && aGame.gambler != null && aGame.computer != null)
            {
                if (whichStitch < -2)
                {
                    ImageComputerStitch0a.Visible = false;
                    ImageComputerStitch0b.Visible = false;
                    // ImagePlayerStitch0a.Visible = false;
                    // ImagePlayerStitch0b.Visible = false;
                    // spanComputerStitches.Visible = false;
                    spanComputerStitches.Style["visibility"] = "hidden";
                    // PlaceHolderPlayerStitches.Visible = false;
                }
                else
                {
                    if (aGame.computer.cardStitches.Count > 0)
                    {
                        spanComputerStitches.Visible = true;
                        spanComputerStitches.Style["visibility"] = "visible";
                        ImageComputerStitch0a.Visible = true;
                        ImageComputerStitch0b.Visible = true;
                    }
                    if (aGame.gambler.cardStitches.Count > 0)
                    {
                        // PlaceHolderPlayerStitches.Visible = true;
                        // ImagePlayerStitch0a.Visible = true;
                        // ImagePlayerStitch0b.Visible = true;
                    }
                }
                if (whichStitch == -2)
                {
                    ImageComputerStitch0a.ImageUrl = notURL.ToString();
                    ImageComputerStitch0b.ImageUrl = notURL.ToString();
                    // ImagePlayerStitch0a.ImageUrl = notURL.ToString();
                    // ImagePlayerStitch0b.ImageUrl = notURL.ToString();
                }
                if (whichStitch == -1 && aGame.computer.stitchCount > 0)
                {
                    if (aGame.computer.stitchCount > 0 && aGame.computer.cardStitches.Count > 0)
                    {
                        if (aGame.computer.cardStitches.Keys.Contains(0))
                        {
                            TwoCards stitch0 = aGame.computer.cardStitches[0];
                            if (stitch0 != null && stitch0.Card1st != null && stitch0.Card2nd != null)
                            {
                                ImageComputerStitch0a.ImageUrl = stitch0.Card1st.PictureUri.ToString();
                                ImageComputerStitch0b.ImageUrl = stitch0.Card2nd.PictureUri.ToString();
                            }
                        }
                    }
                }
                if (whichStitch == 0 && aGame.gambler.stitchCount > 0)
                {
                    if (aGame.gambler.stitchCount > 0 && aGame.gambler.cardStitches.Count > 0)
                    {
                        if (aGame.gambler.cardStitches.Keys.Contains(0))
                        {
                            TwoCards stitchPlayer0 = aGame.gambler.cardStitches[0];
                            if (stitchPlayer0 != null && stitchPlayer0.Card1st != null && stitchPlayer0.Card2nd != null)
                            {
                                // ImagePlayerStitch0a.ImageUrl = stitchPlayer0.Card1st.PictureUri.ToString();
                                // ImagePlayerStitch0b.ImageUrl = stitchPlayer0.Card2nd.PictureUri.ToString();
                            }
                        }
                    }
                }
            }
        }

        protected void ShowComputer20(Card computerPlayedOut, int stage)
        {
            for (int ci = 0; ci < aGame.computer.hand.Length; ci++)
            {
                if (computerPlayedOut.CardValue == CARDVALUE.QUEEN &&
                        aGame.computer.hand[ci].CardColor == computerPlayedOut.CardColor &&
                        aGame.computer.hand[ci].CardValue == CARDVALUE.KING)
                {
                    imOut20.ImageUrl = aGame.computer.hand[ci].PictureUrlString;
                    // imCOut1.ImageUrl = computerPlayedOut.getPictureUrl();
                    break;
                }
                if (computerPlayedOut.CardValue == CARDVALUE.KING &&
                        aGame.computer.hand[ci].CardColor == computerPlayedOut.CardColor &&
                        aGame.computer.hand[ci].CardValue == CARDVALUE.QUEEN)
                {
                    // imCOut0.ImageUrl = computerPlayedOut.PictureUrlString;
                    imOut20.ImageUrl = aGame.computer.hand[ci].PictureUrlString;
                    break;
                }
            }
            stage--;
            imOut21.ImageUrl = computerPlayedOut.PictureUrlString;
        }


        protected void Stop_Click(object sender, EventArgs e)
        {
            try
            {
                StopGame(7, PLAYERDEF.COMPUTER);
            }
            catch (Exception e23)
            {
                this.ErrHandler(e23);
            }
        }

        /// <summary>
        /// bChange_Click - change atou click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Change_Click(object sender, EventArgs e)
        {
            preOut.InnerText += "bChange_Click\r\n";
            aGame.ChangeAtou(aGame.gambler);

            string msgChange = ResReader.GetValue("bChange_text", globalVariable.ISO2Lang);
            SetTextMessage(msgChange);
            
            ShowAtouCard(aGame.schnapState);
            ShowPlayersCards(aGame.schnapState);
            GameTurn(1);
        }

        /// <summary>
        /// A20_Click - say marriage in first pair
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void A20_Click(object sender, EventArgs e)
        {
            if (globalVariable != null && aGame == null)
            {
                aGame = globalVariable.Game;
            }
            preOut.InnerText += "A20_Click\r\n";
            try
            {
                if ((aGame.pSaid) || (aGame.gambler.handpairs[0] == 'n'))
                {
                    return;
                }
                String sayPair;
                aGame.said = aGame.gambler.handpairs[0];
                if (aGame.gambler.handpairs[0] == aGame.AtouInGame)
                {
                    aGame.gambler.points += 40;
                    sayPair = ResReader.GetValue("fourty_in_color", globalVariable.ISO2Lang) +
                        " " + aGame.PrintColor(aGame.said);
                }
                else
                {
                    aGame.gambler.points += 20;
                    sayPair = ResReader.GetValue("twenty_in_color", globalVariable.ISO2Lang) +
                        " " + aGame.PrintColor(aGame.said);
                }
                aGame.pSaid = true;
                ResetButtons(0);

                string msg0 = string.Format(
                    ResReader.GetValue("you_say_pair", globalVariable.ISO2Lang),
                    aGame.PrintColor(aGame.said));
                SetTextMessage(msg0);
                aGame.InsertMsg(msg0);
                PrintMsg();

                tPoints.Text = aGame.gambler.points.ToString();
                if (aGame.gambler.points >= Constants.ENOUGH)
                {
                    TwentyEnough(PLAYERDEF.HUMAN);
                }
            }
            catch (Exception ex22)
            {
                this.ErrHandler(ex22);
            }
        }

        /// <summary>
        /// 2nd Button for pair marriage click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void B20_Click(object sender, EventArgs e)
        {
            string msg = "B20_Click";
            preOut.InnerText += "\r\n" + msg;
            try
            {
                if ((aGame.pSaid) || (aGame.gambler.handpairs[1] == 'n'))
                {
                    return;
                }
                String sayPair;
                aGame.said = aGame.gambler.handpairs[1];
                if (aGame.gambler.handpairs[1] == aGame.AtouInGame)
                {
                    aGame.gambler.points += 40;
                    sayPair = ResReader.GetValue("fourty_in_color", globalVariable.ISO2Lang) +
                        " " + aGame.PrintColor(aGame.said);
                }
                else
                {
                    aGame.gambler.points += 20;
                    sayPair = ResReader.GetValue("fourty_in_color", globalVariable.ISO2Lang) +
                        " " + aGame.PrintColor(aGame.said);
                }
                aGame.pSaid = true;
                ResetButtons(0);

                string msg0 = string.Format(ResReader.GetValue("you_say_pair", globalVariable.ISO2Lang),
                    aGame.PrintColor(aGame.said));
                SetTextMessage(sayPair);

                aGame.InsertMsg(msg0);
                PrintMsg();

                tPoints.Text = aGame.gambler.points.ToString();
                if (aGame.gambler.points >= Constants.ENOUGH)
                {
                    TwentyEnough(PLAYERDEF.HUMAN);
                }
            }
            catch (Exception ex33)
            {
                this.ErrHandler(ex33);
            }
        }


        /// <summary>
        /// EventHandler when clicking on a Card Image
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ImageCard_Click(object sender, EventArgs e)
        {
            int ic = 0;

            // don't let player drag and drop cards, when he shouldn't
            if (aGame != null && (!aGame.isReady || sender == null))
            {
                return;
            }

            string senderStr = "";
            if (sender is WebControl)
            {
                senderStr = ((WebControl)sender).ClientID;
                preOut.InnerText += "ImageCard_Click: \r\nsender = " + senderStr.ToString() + " e = " + e.ToString() + "\r\n";
            }
            if (sender is System.Web.UI.WebControls.ImageButton)
                senderStr = ((System.Web.UI.WebControls.ImageButton)sender).ClientID;
            if (senderStr.StartsWith("imAtou"))
                senderStr = senderStr.Replace("imAtou", "");
            else if (senderStr.Contains("imOut"))
                senderStr = senderStr.Replace("imOut", "");
            else 
               senderStr = senderStr.Replace("im", "");

            if (!Int32.TryParse(senderStr, out ic))
            {
                return;
            }

            try
            {
                if (ic == 20 || ic == 21)
                {
                    ImOut_Click(sender, e);
                    return;
                }
                if (ic == 11)
                {
                    Continue_Click(sender, e);
                    return;
                }
                if (ic == 10)
                {
                    if (aGame.playersTurn && !aGame.pSaid && aGame.CanCloseOrChange)
                    {
                        CloseGame(PLAYERDEF.HUMAN);
                    }
                    return;
                }
                if (!aGame.gambler.hand[ic].IsValidCard)
                {
                    String msgVC = ResReader.GetValue("this_is_no_valid_card", globalVariable.ISO2Lang);
                    SetTextMessage(msgVC);
                    aGame.InsertMsg(msgVC);
                    PrintMsg();
                    return;
                }
                if (aGame.pSaid)
                {
                    int cardVal = CARDVALUE_Extensions.CardValue(aGame.gambler.hand[ic].CardValue);
                    if ((aGame.said == CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[ic].CardColor)) &&
                            (cardVal > 2) &&
                            (cardVal < 5))
                    {
                        ; // we can continue
                    }
                    else
                    {
                        String msgPlayPair = ResReader.GetValue("you_must_play_pair_card", globalVariable.ISO2Lang);
                        SetTextMessage(msgPlayPair);
                        aGame.InsertMsg(msgPlayPair);
                        PrintMsg();
                        return;
                    }
                }
                if (aGame.colorHitRule && (!aGame.playersTurn))
                {
                    if (ccard < 0 && Session["ccard"] != null)
                    {
                        ccard = (int)Session["ccard"];
                    }

                    // CORRECT WAY ?
                    if ((!aGame.gambler.IsValidInColorHitsContext(ic, aGame.computer.hand[ccard])))
                    {
                        String msgColorHitRule = ResReader.GetValue("you_must_play_color_hit_force_rules", globalVariable.ISO2Lang);
                        SetTextMessage(msgColorHitRule);
                        aGame.InsertMsg(msgColorHitRule);
                        int tmpint = aGame.gambler.PreferedInColorHitsContext(aGame.computer.hand[ccard]);
                        // for (j = 0; j < 5; j++) {
                        //     c_array = c_array + aGame.gambler.colorHitArray[j] + " ";
                        // }
                        // aGame.mqueue.insert(c_array);

                        String msgBestWouldBe = string.Format(ResReader.GetValue("best_card_would_be", globalVariable.ISO2Lang),
                            aGame.gambler.hand[tmpint].Name);
                        aGame.InsertMsg(msgBestWouldBe);
                        PrintMsg();
                        ShowPlayersCards(aGame.schnapState);
                        return;
                    }
                }
                if (psaychange > 0)
                {
                    ResetButtons(0);
                    psaychange = 0;
                }
                aGame.playedOut = aGame.gambler.hand[ic];
                switch (ic)
                {
                    case 0:
                        im0.ImageUrl = emptyURL.ToString();
                        break;
                    case 1:
                        im1.ImageUrl = emptyURL.ToString();
                        break;
                    case 2:
                        im2.ImageUrl = emptyURL.ToString();
                        break;
                    case 3:
                        im3.ImageUrl = emptyURL.ToString();
                        break;
                    case 4:
                        im4.ImageUrl = emptyURL.ToString();
                        break;
                    default: preOut.InnerText += "\r\nAssertion: ic = " + ic + "\r\n"; break;
                }

                playedOutCard0 = aGame.gambler.hand[ic];
                aGame.playedOut0 = playedOutCard0;
                imOut20.ImageUrl = aGame.gambler.hand[ic].PictureUrlString;

            }
            catch (Exception e156)
            {
                this.ErrHandler(e156);
            }
            aGame.gambler.hand[ic] = globalVariable.CardEmpty;
            aGame.isReady = false;
            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
            EndTurn();

        }

        /// <summary>
        /// EventHandler, when clicking on Continue
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Continue_Click(object sender, EventArgs e)
        {            
            if (aGame == null || !aGame.isGame)
            {
                ToggleTorunament(true);
                StartGame();
                return;
            }
            if (aGame.shouldContinue)
            {
                ToggleContinue(false);
                tMsg.Visible = false;
                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                GameTurn(0);
            }
        }

        protected void ToggleContinue(bool continueEnabled = true)
        {
            aGame.shouldContinue = continueEnabled;
            imOut20.ToolTip = (continueEnabled) ? ResReader.GetValue("continue_ToolTip", globalVariable.ISO2Lang) : "";
            imOut21.ToolTip = (continueEnabled) ? ResReader.GetValue("continue_ToolTip", globalVariable.ISO2Lang) : "";
            bContinue.ToolTip = (continueEnabled) ? ResReader.GetValue("continue_ToolTip", globalVariable.ISO2Lang) : "";
            bContinue.Enabled = continueEnabled;
        }


        protected void ImMerge11_Click(object sender, EventArgs e)
        {
            if (aGame != null && aGame.schnapState == SCHNAPSTATE.MERGE_COMPUTER)
            {
                aGame.schnapState = SCHNAPSTATE.PLAYER_TAKES;
                imOut20.ImageUrl = emptyURL.ToString();
                imOut21.ImageUrl = emptyURL.ToString();

                aGame.schnapsStack.Clear();
                aGame.schnapsStack.Push(SCHNAPSTATE.GIVE_TALON);
                aGame.schnapsStack.Push(SCHNAPSTATE.PLAYER_TAKES);

                ShowMergeAnim(aGame.schnapState);
                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);

                int schnapsTempParamVal = SCHNAPSTATE.PLAYER_TAKES.GetValue();
                string rawUrl = RawUrlInit(schnapsTempParamVal);
                this.Response.Redirect(rawUrl);

                return;
            }

            Merge_Click(sender, e);
        }

        protected void ImOut_Click(object sender, EventArgs e)
        {
            string senderString = "";
            if (sender != null && sender is System.Web.UI.WebControls.ImageButton)
            {
                senderString = ((System.Web.UI.WebControls.ImageButton)sender).ID.ToString();
            }

            if (senderString.StartsWith("imOut") &&
                (aGame.schnapState == SCHNAPSTATE.MERGE_COMPUTER))
            {
                if (senderString == "imOut21")
                {
                    aGame.schnapState = SCHNAPSTATE.PLAYER_FIST;
                    imOut20.ImageUrl = emptyURL.ToString();
                    imOut21.ImageUrl = emptyURL.ToString();

                    aGame.schnapsStack.Clear();
                    aGame.schnapsStack.Push(SCHNAPSTATE.GIVE_TALON);
                    aGame.schnapsStack.Push(SCHNAPSTATE.PLAYER_FIST);

                    ShowMergeAnim(aGame.schnapState);
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);

                    int schnapsTempParamVal = SCHNAPSTATE.PLAYER_FIST.GetValue();
                    string rawUrl = RawUrlInit(schnapsTempParamVal);
                    this.Response.Redirect(rawUrl);
                }
                if (senderString == "imOut20")
                {
                    aGame.schnapState = SCHNAPSTATE.PLAYER_TAKES;
                    imOut20.ImageUrl = emptyURL.ToString();
                    imOut21.ImageUrl = emptyURL.ToString();

                    aGame.schnapsStack.Clear();
                    aGame.schnapsStack.Push(SCHNAPSTATE.GIVE_TALON);
                    aGame.schnapsStack.Push(SCHNAPSTATE.PLAYER_TAKES);

                    ShowMergeAnim(aGame.schnapState);
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);

                    int schnapsTempParamVal = SCHNAPSTATE.PLAYER_TAKES.GetValue();
                    string rawUrl = RawUrlInit(schnapsTempParamVal);
                    this.Response.Redirect(rawUrl);
                }
                return;
            }
            this.Continue_Click(sender, e);
        }

        void ResetButtons(int level)
        {
            if (level >= 0)
            {
                if (aGame != null)
                {
                    aGame.a20 = false;
                    aGame.b20 = false;
                    aGame.bChange = false;
                }

                b20a.Text = ResReader.GetValue("b20a_text", globalVariable.ISO2Lang);
                b20a.ToolTip = b20a.Text;
                b20a.Enabled = false;                
            }

            if (level >= 1)
            {
                if (aGame != null)
                {
                    aGame.shouldContinue = false;
                }
                bContinue.Text = ResReader.GetValue("bContinue_text", globalVariable.ISO2Lang);
                bContinue.ToolTip = bContinue.Text;
                bContinue.Enabled = false;

                ShowAtouCard(SCHNAPSTATE.GAME_START);
                ShowTalonCard(SCHNAPSTATE.GAME_START);
                ShowMergeAnim(SCHNAPSTATE.GAME_START);
            }

            if (level > 3)
            {
                try
                {
                    // imOut0.ImageUrl = emptyURL.ToString();
                    // imOut1.ImageUrl = emptyURL.ToString();
                    imOut20.ImageUrl = emptyURL.ToString();
                    imOut21.ImageUrl = emptyURL.ToString();
                    playedOutCard0 = globalVariable.CardEmpty;
                    playedOutCard1 = globalVariable.CardEmpty;
                    aGame.playedOut0 = playedOutCard0;
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception exL2)
                {
                    this.ErrHandler(exL2);
                }
            }
            if (aGame != null)
            {
                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
            }
        }

        public override void DrawPointsTable(short displayBummerlOrTaylor = 0, PLAYERDEF whoWon = PLAYERDEF.UNKNOWN)
        {
            base.DrawPointsTable(displayBummerlOrTaylor, whoWon);            
        }

        void StopGame(int tournementPts, PLAYERDEF whoWon = PLAYERDEF.UNKNOWN, string endMessage = null)
        {
            if (!string.IsNullOrEmpty(endMessage))
            {
                SetTextMessage(endMessage);
            }
            aTournement.AddPointsRotateGiver(tournementPts, whoWon);
            aGame.StopGame();

            ResetButtons(tournementPts);
            ShowStitches(-3);
            DrawPointsTable();

            ShowPlayersCards(aGame.schnapState);
            aGame.Dispose();

            bMerge.Enabled = true;
            bMerge.Visible = true;
            bStop.Enabled = false;
            bStop.Visible = false;
            this.bContinue.Enabled = true;
            this.imOut20.ToolTip = ResReader.GetValue("imageMerge_ToolTip", globalVariable.ISO2Lang);
            this.imOut21.ToolTip = ResReader.GetValue("imageMerge_ToolTip", globalVariable.ISO2Lang);
            this.imMerge11.ToolTip = ResReader.GetValue("imageMerge_ToolTip", globalVariable.ISO2Lang);

            ToggleTorunament(false);
        }

        void StartGame()
        {  /* Mischen */
            bMerge.Enabled = false;
            bMerge.Visible = false;
            bStop.Visible = true;
            bStop.Enabled = true;

            aGame = null;
            aGame = new Game(HttpContext.Current, aTournement.NextGameGiver);
            aGame.isReady = true;
            tMsg.Visible = false;
            ResetButtons(1);
            preOut.InnerText = "";
            // tRest.Text = (19 - aGame.index).ToString();

            ShowStitches(-3);
            emptyTmpCard = new Card(-2, HttpContext.Current);
            tPoints.Text = "" + aGame.gambler.points;

            if (aTournement.NextGameGiver == PLAYERDEF.HUMAN)
            {
                aGame.schnapState = SCHNAPSTATE.MERGE_COMPUTER;
                RefreshGlobalVariableSession();
                ShowFistOrHand(aGame.schnapState);
                return;
            }
            else if (aTournement.NextGameGiver == PLAYERDEF.COMPUTER)
            {
                aGame.schnapState = SCHNAPSTATE.MERGE_PLAYER;               
                
                aGame.schnapsStack.Clear();
                aGame.schnapsStack.Push(SCHNAPSTATE.GIVE_TALON);
                aGame.schnapsStack.Push(SCHNAPSTATE.GIVE_PLAYER);

                // ShowMergeAnim(aGame.schnapState);
                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);

                int schnapsTempParamVal = SCHNAPSTATE.GIVE_PLAYER.GetValue();
                string rawUrl = RawUrlInit(schnapsTempParamVal);
                this.Response.Redirect(rawUrl);
                return;
            }

            ShowMergeAnim(aGame.schnapState);

            ShowAtouCard(aGame.schnapState);
            ShowTalonCard(aGame.schnapState);

            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
            GameTurn(0);
        }

        void ShowStateSchnapsStack()
        {
            bool finishGivingWithTalon = false;
            bool enterStage = Request.RawUrl.Contains("initState=15");
            
            if (aGame == null || !aGame.isGame)
            {
                bContinue.Enabled = true;
                imOut20.Style["visibility"] = "hidden";
                imOut21.Style["visibility"] = "hidden";                
                im0.Style["visibility"] = "hidden";
                im1.Style["visibility"] = "hidden";
                im2.Style["visibility"] = "hidden";
                im3.Style["visibility"] = "hidden";
                im4.Style["visibility"] = "hidden";
            }

            if (Request.RawUrl.Contains("initState=4") || Request.RawUrl.Contains("initState=7") || Request.RawUrl.Contains("initState=8"))
            {
                bContinue.Enabled = false;
                bMerge.Enabled = false;
                bStop.Enabled = false;
            }

            if (aGame != null && (aGame.schnapsStack != null && aGame.schnapsStack.Count > 0) || enterStage)
            {
                SCHNAPSTATE myState = aGame.schnapState;
                if (enterStage)
                {
                    ; // finishGivingWithTalon = true;
                }
                else
                {
                    myState = aGame.schnapsStack.Pop();
                    aGame.schnapState = myState;
                    switch (myState)
                    {
                        case SCHNAPSTATE.PLAYER_TAKES:
                        case SCHNAPSTATE.PLAYER_FIST:
                        case SCHNAPSTATE.GIVE_PLAYER:
                            ShowPlayersCards(myState);
                            ShowAtouCard(myState);
                            // Show1st3Computer(myState);
                            // Show2nd2Computer(myState);
                            ShowTalonCard(myState);

                            aGame.schnapState = SCHNAPSTATE.GAME_STARTED;
                            RefreshGlobalVariableSession();

                            ShowMergeAnim(aGame.schnapState);
                            ShowAtouCard(myState);
                            ShowTalonCard(myState);

                            bMerge.Enabled = false;
                            bMerge.Visible = false;
                            bStop.Enabled = true;
                            bStop.Visible = true;
                            break;
                        case SCHNAPSTATE.PLAYER_1ST_3:
                        case SCHNAPSTATE.PLAYER_1ST_5:
                        case SCHNAPSTATE.PLAYER_2ND_2:
                            ShowPlayersCards(myState);
                            break;
                        case SCHNAPSTATE.GIVE_ATOU:
                            ShowAtouCard(myState); break;
                        case SCHNAPSTATE.GIVE_TALON:
                        case SCHNAPSTATE.GAME_STARTED:
                            finishGivingWithTalon = true;
                            break;
                        default:
                            string schnapsStateDbgS = myState.ToString();
                            break;
                    }
                }
                // finishGivingWithTalon = (finishGivingWithTalon || (aGame.schnapsStack != null && aGame.schnapsStack.Count == 0));
                if (finishGivingWithTalon)
                {
                    aGame.schnapState = SCHNAPSTATE.GAME_STARTED;
                    RefreshGlobalVariableSession();
                    
                    ShowMergeAnim(aGame.schnapState);
                    ShowAtouCard(myState);
                    ShowTalonCard(myState);

                    bMerge.Enabled = false;
                    bMerge.Visible = false;
                    bStop.Enabled = true;
                    bStop.Visible = true;


                    GameTurn(0);
                    return;
                }

                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                // Thread.Sleep(500);
                int schnapsTempParamVal = aGame.schnapsStack.Peek().GetValue();
                string rawUrl = RawUrlInit(schnapsTempParamVal);
                // this.Response.Redirect(rawUrl);
            }            
        }

        public string RawUrlInit(int schnapsInitParam)
        {            
            string rawUrl = Request.RawUrl;
            if (rawUrl.Contains("initState="))
            {
                int stateReplIdx = rawUrl.LastIndexOf("initState=");
                rawUrl = rawUrl.Substring(0, stateReplIdx);

                rawUrl = rawUrl + "initState=" + schnapsInitParam;
            }
            else
            {
                rawUrl = rawUrl + "?initState=" + schnapsInitParam;
            }

            return rawUrl;
        }

        /// <summary>
        /// CloseGame - implements closing game => Zudrehens
        /// </summary>
        /// <param name="whoCloses">PLAYERDEF player or computer</param>
        void CloseGame(PLAYERDEF whoCloses)
        {
            if (aGame.isGame == false || aGame.gambler == null || aGame.isClosed || aGame.colorHitRule)
            {
                SetTextMessage(ResReader.GetValue("nogame_started", globalVariable.ISO2Lang));
                return;
            }   

            aGame.CloseGame(whoCloses);

            SetTextMessage(aGame.statusMessage);
            ShowTalonCard(aGame.schnapState);
            ShowAtouCard(aGame.schnapState);
            ShowMergeAnim(aGame.schnapState);

            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
            if (whoCloses == PLAYERDEF.HUMAN)
            {
                GameTurn(0);
            }
        }

        protected void TwentyEnough(PLAYERDEF whoWon)
        {
            int xj = 0;
            String andEnough = ResReader.GetValue("twenty_and_enough", globalVariable.ISO2Lang);
            aGame.isReady = false;

            if (whoWon == PLAYERDEF.HUMAN)
            {
                if (aGame.said == aGame.AtouInGame)
                {
                    andEnough = ResReader.GetValue("fourty_and_enough", globalVariable.ISO2Lang);
                }
                try
                {
                    for (xj = 0; xj < aGame.gambler.HandCount; xj++)
                    {
                        char colorCh0 = CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xj].CardColor);
                        if (colorCh0 == aGame.said &&
                                aGame.gambler.hand[xj].CardValue == CARDVALUE.QUEEN)
                        {
                            playedOutCard0 = aGame.gambler.hand[xj];
                            aGame.playedOut0 = playedOutCard0;
                            imOut20.ImageUrl = aGame.gambler.hand[xj].PictureUrlString;
                        }
                        if (colorCh0 == aGame.said &&
                                aGame.gambler.hand[xj].CardValue == CARDVALUE.KING)
                        {
                            playedOutCard1 = aGame.gambler.hand[xj];
                            aGame.playedOut1 = playedOutCard1;
                            imOut21.ImageUrl = aGame.gambler.hand[xj].PictureUrlString;
                        }
                    }
                }
                catch (Exception jbex)
                {
                    this.ErrHandler(jbex);
                }

                string sEnds11 = andEnough + " " + string.Format(
                    ResReader.GetValue("you_have_won_points", globalVariable.ISO2Lang),
                    aGame.gambler.points.ToString());
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                StopGame(tPts, PLAYERDEF.HUMAN, sEnds11);
            }
            else // Computer won
            {
                if (aGame.csaid == aGame.AtouInGame)
                {
                    andEnough = ResReader.GetValue("fourty_and_enough", globalVariable.ISO2Lang);
                }
                try
                {
                    for (xj = 0; xj < aGame.computer.HandCount; xj++)
                    {
                        char colorCh1 = CARDCOLOR_Extensions.ColorChar(aGame.computer.hand[xj].CardColor);
                        if (colorCh1 == aGame.csaid &&
                            aGame.computer.hand[xj].CardValue == CARDVALUE.QUEEN)
                        {
                            playedOutCard0 = aGame.computer.hand[xj];
                            aGame.playedOut0 = playedOutCard0;
                            imOut20.ImageUrl = aGame.computer.hand[xj].PictureUrlString;
                        }
                        if (colorCh1 == aGame.csaid &&
                            aGame.computer.hand[xj].CardValue == CARDVALUE.KING)
                        {
                            playedOutCard1 = aGame.computer.hand[xj];
                            aGame.playedOut1 = playedOutCard1;
                            imOut21.ImageUrl = aGame.computer.hand[xj].PictureUrlString;
                        }
                    }
                }
                catch (Exception enoughEx1)
                {
                    this.ErrHandler(enoughEx1);
                }

                PrintMsg();
                string sEnds12 = andEnough + " " + string.Format(
                    ResReader.GetValue("computer_has_won_points", globalVariable.ISO2Lang),
                    aGame.computer.points.ToString());
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.COMPUTER);
                StopGame(tPts, PLAYERDEF.COMPUTER, sEnds12);
                // StopGame(1, new String(andEnough + " Computer hat gewonnen mit " + String.valueOf(aGame.computer.points) + " Punkten !"));
            }
            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
            return;
        }

        [Obsolete("TwentyEnough_Old(bool who) is obsolete and replaced with protected void TwentyEnough(PLAYERDEF whoWon)", false)]
        void TwentyEnough_Old(bool who)
        {
            int xking = 0;
            int xqueen = 0;
            bool xfinished = false;
            String andEnough = "20 und genug !";
            aGame.isReady = false;
            if (who)
            {
                try
                {
                    while ((xqueen < aGame.gambler.HandCount) && !xfinished)
                    {
                        if ((aGame.gambler.hand[xqueen] != null))
                        {
                            char queenColor = CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xqueen].CardColor);
                            int queenValue = CARDVALUE_Extensions.CardValue(aGame.gambler.hand[xqueen].CardValue);
                            if ((queenColor == aGame.said) && (queenValue == 3 || queenValue == 4))
                            {
                                Uri enoughQueenUrl = new Uri(
                                    "https://area23.at/schnapsen/cardpics/" + aGame.said + "3.gif");                                
                                imOut20.ImageUrl = enoughQueenUrl.ToString();
                                Uri enoughKingUrl = new Uri(
                                    "https://area23.at/schnapsen/cardpics/" + aGame.said + "4.gif");
                                imOut21.ImageUrl = enoughKingUrl.ToString();
                                xfinished = true;
                                break;
                            }
                        }
                        xqueen++;
                    }

                    if (aGame.said == aGame.AtouInGame)
                    {
                        andEnough = "40 und genug !";
                    }
                }
                catch (Exception jbpvex)
                {
                    this.ErrHandler(jbpvex);
                }
                string anEnPairMsg = andEnough + " Sie haben gewonnen mit " + aGame.gambler.points + " Punkten !";
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                StopGame(tPts, PLAYERDEF.HUMAN, andEnough);
            }
            else
            {
                try
                {
                    xking = 0;
                    xfinished = false;
                    while ((xking < aGame.computer.HandCount) && !xfinished)
                    {
                        if (aGame.computer.hand[xking] != null)
                        {
                            char queenColor = CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xking].CardColor);

                            if ((queenColor == aGame.csaid) &&
                                (aGame.computer.hand[xking].CardValue == CARDVALUE.QUEEN ||
                                    aGame.computer.hand[xking].CardValue == CARDVALUE.KING))
                            {
                                Uri enoughCQueenUrl = new Uri(
                                    "https://area23.at/schnapsen/cardpics/" + aGame.csaid + "3.gif");
                                imOut20.ImageUrl = enoughCQueenUrl.ToString();
                                Uri enoughCKingUrl = new Uri(
                                    "https://area23.at/schnapsen/cardpics/" + aGame.csaid + "4.gif");
                                imOut21.ImageUrl = enoughCKingUrl.ToString();
                                xfinished = true;
                                break;
                            }
                        }
                        xking++;
                    }

                    if (aGame.csaid == aGame.AtouInGame)
                    {
                        andEnough = "40 und genug !";
                    }
                }
                catch (Exception jbpvex)
                {
                    this.ErrHandler(jbpvex);
                }
                PrintMsg();
                string msg40 = andEnough + "Computer hat gewonnen mit " + aGame.computer.points + " Punkten !";
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.COMPUTER);
                StopGame(tPts, PLAYERDEF.COMPUTER, msg40);
            }
            return;
        }

        void GameTurn(int ixlevel)
        {
            if (ixlevel < 1)
            {
                try
                {
                    imOut20.ImageUrl = emptyURL.ToString();
                    imOut21.ImageUrl = emptyURL.ToString();
                    playedOutCard0 = globalVariable.CardEmpty;
                    playedOutCard1 = globalVariable.CardEmpty;
                    aGame.playedOut0 = playedOutCard0;
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception jbpvex)
                {
                    this.ErrHandler(jbpvex);
                }
                ShowPlayersCards(aGame.schnapState);
                aGame.pSaid = false;
                aGame.said = 'n';
                aGame.csaid = 'n';
            }

            ShowStitches(-2);
            aGame.bChange = false;
            aGame.a20 = false;
            aGame.b20 = false;
            aGame.sayMarriage20 = ResReader.GetValue("b20a_text", globalVariable.ISO2Lang);
            aGame.sayMarriage40 = ResReader.GetValue("b20a_text", globalVariable.ISO2Lang);

            if (aGame.playersTurn)
            {
                // Wann kann man austauschen ?
                if (ixlevel < 1)
                {
                    if (aGame.AtouIsChangable(aGame.gambler) && (!aGame.pSaid))
                    {
                        psaychange += 1;
                        aGame.bChange = true;
                    }
                }
                // Gibts was zum Ansagen ?
                int a20 = aGame.gambler.HasPair;
                if (a20 > 0)
                {
                    psaychange += 2;
                    b20a.Text = aGame.PrintColor(aGame.gambler.handpairs[0]) + " " +
                        ResReader.GetValue("say_pair", globalVariable.ISO2Lang);
                    aGame.sayMarriage20 = aGame.PrintColor(aGame.gambler.handpairs[0]) + " " +
                        ResReader.GetValue("say_pair", globalVariable.ISO2Lang);
                    aGame.a20 = true;
                    b20a.Enabled = true;                    
                }
                // Info 
                SetTextMessage(ResReader.GetValue("toplayout_clickon_card", globalVariable.ISO2Lang));
            }
            else
            {
                /* COMPUTERS TURN IMPLEMENTIEREN */
                string outPutMessage = "";
                ccard = aGame.ComputerStarts();
                Session["ccard"] = ccard;

                int bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.CHANGEATOU);
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    this.ShowAtouCard(aGame.schnapState);
                    outPutMessage += ResReader.GetValue("computer_changes_atou", globalVariable.ISO2Lang);
                }

                bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.SAYPAIR);
                bool computerSaid20 = false;
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    computerSaid20 = true;
                    String computerSaysPair = string.Format(
                        ResReader.GetValue("computer_says_pair", globalVariable.ISO2Lang),
                        aGame.PrintColor(aGame.csaid));
                    outPutMessage += (string.IsNullOrEmpty(outPutMessage) ? "" : " ") + computerSaysPair;
                }
                if (outPutMessage == "")
                    outPutMessage = ResReader.GetValue("computer_plays_out", globalVariable.ISO2Lang);
                SetTextMessage(outPutMessage);

                bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.ANDENOUGH);
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    TwentyEnough(PLAYERDEF.COMPUTER);
                    aGame.isReady = false;
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    return;
                }

                bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.CLOSESGAME);
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    aGame.isClosed = true;
                    outPutMessage += ResReader.GetValue("computer_closed_game", globalVariable.ISO2Lang);
                    SetTextMessage(outPutMessage);
                    CloseGame(PLAYERDEF.COMPUTER);
                }


                try
                {
                    playedOutCard1 = aGame.computer.hand[ccard];
                    if (computerSaid20)
                    {
                        // TODO: implement it
                        ShowComputer20(playedOutCard1, 4);
                    }

                    imOut21.ImageUrl = aGame.computer.hand[ccard].PictureUrlString;
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception jbpex)
                {
                    this.ErrHandler(jbpex);
                }

                String msgTxt33 = ResReader.GetValue("toplayout_clickon_card", globalVariable.ISO2Lang);
                // SetTextMessage(msgTxt33);            
            }

            aGame.isReady = true;
            PrintMsg();
            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
        }

        void EndTurn()
        {
            int tmppoints;
            String msgText = "";

            /* implement computers strategy here */
            if (aGame.playersTurn)
            {
                ccard = aGame.ComputersAnswer();
                Session["ccard"] = ccard;
                try
                {
                    playedOutCard1 = aGame.computer.hand[ccard];
                    imOut21.ImageUrl = aGame.computer.hand[ccard].PictureUrlString;
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception jbpvex)
                {
                    this.ErrHandler(jbpvex);
                }
            }

            if (Session["ccard"] != null)
            {
                ccard = (int)Session["ccard"];
                Session["ccard"] = null;
            }

            tmppoints = aGame.CheckPoints(ccard);
            aGame.computer.hand[ccard] = globalVariable.CardEmpty;
            tPoints.Text = aGame.gambler.points.ToString();

            if (tmppoints > 0)
            {
                msgText = string.Format(ResReader.GetValue("your_hit_points", globalVariable.ISO2Lang),
                    tmppoints.ToString()) + " " +
                    ResReader.GetValue("click_continue", globalVariable.ISO2Lang);

                SetTextMessage(msgText);

                TwoCards stitchPlayer = new TwoCards(aGame.playedOut, aGame.playedOut1);
                if (!aGame.gambler.cardStitches.Keys.Contains(aGame.gambler.stitchCount))
                {
                    aGame.gambler.cardStitches.Add(aGame.gambler.stitchCount, stitchPlayer);
                    aGame.gambler.stitchCount++;
                }

                if (aGame.isClosed && (aGame.computer.hasClosed))
                {
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    string sEnds0 = ResReader.GetValue("computer_closing_failed", globalVariable.ISO2Lang);
                    StopGame(3, PLAYERDEF.HUMAN, sEnds0);
                    return;
                }
            }
            else
            {
                msgText = string.Format(ResReader.GetValue("computer_hit_points", globalVariable.ISO2Lang),
                    (-tmppoints).ToString()) + " " +
                    ResReader.GetValue("click_continue", globalVariable.ISO2Lang);
                SetTextMessage(msgText);

                TwoCards stitchComputer = new TwoCards(aGame.playedOut, aGame.playedOut1);
                if (!aGame.computer.cardStitches.Keys.Contains(aGame.computer.stitchCount))
                {
                    aGame.computer.cardStitches.Add(aGame.computer.stitchCount, stitchComputer);
                    aGame.computer.stitchCount++;
                }

                if ((aGame.isClosed) && (aGame.gambler.hasClosed))
                {
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    string sEnds1 = ResReader.GetValue("closing_failed", globalVariable.ISO2Lang);
                    StopGame(3, PLAYERDEF.COMPUTER, sEnds1);
                    return;
                }
            }

            // Assign new cards
            if (aGame.AssignNewCard() == 1)
            {
                /* NOW WE HAVE NO MORE TALON */
                try
                {
                    ShowTalonCard(aGame.schnapState);
                    ShowAtouCard(aGame.schnapState);
                    ShowMergeAnim(aGame.schnapState);
                }
                catch (Exception jbpvex)
                {
                    this.ErrHandler(jbpvex);
                }

                string msgChFrc = ResReader.GetValue("color_hit_force_mode", globalVariable.ISO2Lang);
                SetTextMessage(msgChFrc);
            }

            // tRest.Text = (19 - aGame.index).ToString();
            PrintMsg();

            // ResetButtons(0);
            aGame.pSaid = false;
            aGame.said = 'n';
            aGame.csaid = 'n';

            if (aGame.playersTurn)
            {
                if (aGame.gambler.points >= Constants.ENOUGH)
                {
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    string sEnds3 = string.Format(
                        ResReader.GetValue("you_have_won_points", globalVariable.ISO2Lang),
                        aGame.gambler.points.ToString());
                    int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                    StopGame(tPts, PLAYERDEF.HUMAN, sEnds3);
                    return;
                }
            }
            else
            {
                if (aGame.computer.points >= Constants.ENOUGH)
                {
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    string sEnds4 = string.Format(
                        ResReader.GetValue("computer_has_won_points", globalVariable.ISO2Lang),
                        aGame.computer.points.ToString());
                    int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                    StopGame(tPts, PLAYERDEF.COMPUTER, sEnds4);
                    return;
                }
            }

            if (aGame.schnapState == SCHNAPSTATE.ZERO_CARD_REMAINS)
            {
                if (aGame.isClosed) // close game => must have over 66 or loose
                {
                    if (aGame.gambler.hasClosed)
                    {
                        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                        string sEnds6 = ResReader.GetValue("closing_failed", globalVariable.ISO2Lang);
                        StopGame(3, PLAYERDEF.COMPUTER, sEnds6);
                    }
                    try
                    {
                        if (aGame.computer.hasClosed)
                        {
                            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                            string sEnds7 = ResReader.GetValue("computer_closing_failed", globalVariable.ISO2Lang);
                            StopGame(3, PLAYERDEF.HUMAN, sEnds7);
                        }
                    }
                    catch (Exception jbpvex)
                    {
                        this.ErrHandler(jbpvex);
                    }
                    return;
                }

                if (tmppoints > 0)
                {
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    string sEnds8 = ResReader.GetValue("last_hit_you_have_won", globalVariable.ISO2Lang);
                    int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                    StopGame(tPts, PLAYERDEF.HUMAN, sEnds8);
                }
                else
                {
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    string sEnds9 = ResReader.GetValue("computer_wins_last_hit", globalVariable.ISO2Lang);
                    int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                    StopGame(tPts, PLAYERDEF.COMPUTER, sEnds9);
                }
                return;
            }

            ToggleContinue(true);
            aGame.isReady = false;
            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
        }


        void PrintMsg()
        {
            preOut.InnerText = aGame.FetchMsg();
            string[] msgs = aGame.FetchMsgArray();
            
            for (int i = aGame.fetchedMsgCount; i < msgs.Length; i++) 
            {
                Log(msgs[i]);
            }
            aGame.fetchedMsgCount = msgs.Length;
        }

        void ErrHandler(Exception myErr)
        {
            preOut.InnerText += "\r\nCRITICAL ERROR #" + (++errNum);
            preOut.InnerText += "\nMessage: " + myErr.Message;
            preOut.InnerText += "\nString: " + myErr.ToString();
            preOut.InnerText += "\nLmessage: " + myErr.StackTrace + "\n";
        }

        /// <summary>
        /// SetTextMessage shows a new Toast dynamic message
        /// </summary>
        /// <param name="textMsg">text to display</param>
        void SetTextMessage(string textMsg)
        {
            string msgSet = string.IsNullOrWhiteSpace(textMsg) ? "" : textMsg;
            if (aGame != null)
                aGame.statusMessage = msgSet;

            tMsg.Visible = true;
            tMsg.Text = msgSet;
            Log(msgSet);
        }

        public void Help_Click(object sender, EventArgs e)
        {
            preOut.InnerHtml = "-------------------------------------------------------------------------\n";
            preOut.InnerText += ResReader.GetValue("help_text", globalVariable.ISO2Lang) + "\n";
            preOut.InnerHtml += "-------------------------------------------------------------------------\n";
        }

        protected void Merge_Click(object sender, EventArgs e)
        {
            ToggleTorunament(true);
            StartGame();
        }

        protected void ToggleTorunament(bool starts = true)
        {
            if (starts)
            {
                if (aTournement.WonTournament != PLAYERDEF.UNKNOWN)
                {
                    globalVariable = new GlobalAppSettings(this.Context, this.Session);
                    aTournement = new Tournament();
                    globalVariable.Tournement = aTournement;
                    this.Context.Session[Constants.APPNAME] = globalVariable;
                    DrawPointsTable();
                }
            }
            else // toggle at the end
            {
                if (aTournement.WonTournament != PLAYERDEF.UNKNOWN)
                {
                    string endTournementMsg = "";
                    if (aTournement.WonTournament == PLAYERDEF.HUMAN)
                    {
                        if (aTournement.Taylor)
                        {
                            endTournementMsg = ResReader.GetValue("you_won_taylor", globalVariable.ISO2Lang);
                            DrawPointsTable(2, aTournement.WonTournament);
                        }
                        else
                        {
                            endTournementMsg = ResReader.GetValue("you_won_tournement", globalVariable.ISO2Lang);
                            DrawPointsTable(1, aTournement.WonTournament);
                        }
                    }
                    else if (aTournement.WonTournament == PLAYERDEF.COMPUTER)
                    {
                        if (aTournement.Taylor)
                        {
                            endTournementMsg = ResReader.GetValue("computer_won_taylor", globalVariable.ISO2Lang);
                            DrawPointsTable(2, aTournement.WonTournament);
                        }
                        else
                        {
                            endTournementMsg = ResReader.GetValue("computer_won_tournement", globalVariable.ISO2Lang);
                            DrawPointsTable(1, aTournement.WonTournament);
                        }
                    }
                    SetTextMessage(endTournementMsg);
                    // TODO: excited end animation
                }
            }
        }


        protected void ImageComputerStitch_Click(object sender, EventArgs e)
        {
            ShowStitches(-1);
        }

        protected void ImagePlayerStitch_Click(object sender, EventArgs e)
        {
            ShowStitches(0);
        }
    }
}