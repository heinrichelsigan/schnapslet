﻿using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using SchnapsNet.ConstEnum;
using SchnapsNet.Models;
using SchnapsNet.Utils;

namespace SchnapsNet
{
    public partial class SchnapsNet : Area23BasePage
    {
        long errNum = 0; // Errors Ticker
        int ccard = -1; // Computers Card played
        Models.Card emptyTmpCard, playedOutCard0, playedOutCard1;
        volatile byte psaychange = 0;

        /// <summary>
        /// Gets or sets PreInnerText
        /// </summary>
        protected string PreInnerText
        {
            get => this.preOut.InnerText;
            set
            {
                this.preOut.InnerText = value;
                if (globalVariable == null)
                {
                    this.InitGlobalVariable();
                }
                if (globalVariable != null)
                {
                    this.globalVariable.InnerPreText = this.preOut.InnerText;
                }
            }
        }

        /// <summary>
        /// Gets or Sets AudioWav
        /// </summary>
        protected override string AudioWav
        {
            get => this.metaAudio.Content;
            set => this.metaAudio.Content = value;
        }

        // static String emptyJarStr = "/schnapsen/cardpics/e.gif";
        // static String backJarStr =  "/schnapsen/cardpics/verdeckt.gif";
        // static String notJarStr =   "/schnapsen/cardpics/n0.gif";
        // static String talonJarStr = "/schnapsen/cardpics/t.gif";
        // Thread t0;

        protected override void InitSchnaps()
        {
            base.InitSchnaps();

            this.PreInnerText = string.Empty;;
            // tMsg.Enabled = false;

            im0.ImageUrl = emptyURL.ToString();
            im1.ImageUrl = emptyURL.ToString();
            im2.ImageUrl = emptyURL.ToString();
            im3.ImageUrl = emptyURL.ToString();
            im4.ImageUrl = emptyURL.ToString();

            // imOut0.ImageUrl = emptyURL.ToString();
            imOut20.ImageUrl = emptyURL.ToString();
            // imOut1.ImageUrl = emptyURL.ToString();
            imOut21.ImageUrl = emptyURL.ToString();
            imTalon.ImageUrl = emptyTalonUri.ToString();
            imAtou10.ImageUrl = emptyURL.ToString();

            bMerge.Text = ResReader.GetRes("bStart_text", Locale);
            bStop.Text = ResReader.GetRes("bStop_text", Locale);
            bStop.Enabled = false;
            bStop.Visible = false;
            b20b.Text = ResReader.GetRes("b20b_text", Locale);
            b20b.Enabled = false;
            b20a.Text = ResReader.GetRes("b20a_text", Locale);
            b20a.Enabled = false;

            bChange.Text = ResReader.GetRes("bChange_text", Locale);
            bChange.Enabled = false;

            tPoints.Enabled = false;
            tPoints.Text = ResReader.GetRes("tPoints_text", Locale);
            bContinue.Text = ResReader.GetRes("bContinue_text", Locale);
            bContinue.Enabled = true;

            bHelp.Text = ResReader.GetRes("bHelp_text", Locale);
            bHelp.ToolTip = ResReader.GetRes("bHelp_text", Locale);

            // tRest.Enabled = false;
            // tRest.Text = ResReader.GetRes("tRest_text", Locale);            
            // lRest.Text = ResReader.GetRes("sRest", Locale);

            this.imOut20.ToolTip = ResReader.GetRes("imageMerge_ToolTip", Locale);
            this.imOut21.ToolTip = ResReader.GetRes("imageMerge_ToolTip", Locale);
            this.imMerge11.ToolTip = ResReader.GetRes("imageMerge_ToolTip", Locale);
            Reset_ImgOut(true);


            lPoints.Text = ResReader.GetRes("sPoints", Locale);
            lPoints.ToolTip = lPoints.Text;

            tMsg.Enabled = false;
            tMsg.Text = ResReader.GetRes("clickon_start", Locale);
            tMsg.Visible = true;

            ShowStitches(STITCH.NONE);
        }

        protected override void RefreshGlobalVariableSession()
        {
            base.RefreshGlobalVariableSession();

            //string saveFileName = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
            //    "Schnapsen_" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" +
            //    DateTime.Now.Day.ToString() + "_" + Context.Session.SessionID + "_" + 
            //    DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".json");
            // string jsonString = JsonConvert.SerializeObject(globalVariable);
            // System.IO.File.WriteAllText(saveFileName, jsonString);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (aGame != null && aGame.isGame)
            {
                lAtouIs.Text = aGame.PrintSymbol(CARDCOLOR_Extensions.ColorChar(aGame.set[19].CardColor));
                lAtouIs.ToolTip = ResReader.GetStringFormated("atou_is", Locale,
                    aGame.PrintColor(CARDCOLOR_Extensions.ColorChar(aGame.set[19].CardColor)));
            }
            else
            {
                lAtouIs.Text = ResReader.GetRes("symbol_n", Locale);
                lAtouIs.ToolTip = ResReader.GetRes("nogame_started", Locale);
            }
        }

        /// <summary>
        /// Page_Load Eventhandler fired, when Aspx Loads
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e"><see cref="EventArgs"/> e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!loaded) OnLoad(e);
        }


        protected void Perform_DotBorderCards_OnMarrgiage(char colorChar, int handPairNumber)
        {
            bool btnSet = false;
            ImageButton[] imBtns = new ImageButton[5] { im0, im1, im2, im3, im4 };
            int xj = 0;
            for (xj = 0; xj < aGame.gambler.HandCount; xj++)
            {
                char colorCh0 = CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xj].CardColor);
                if ((colorCh0 == aGame.said || colorCh0 == colorChar) &&
                        (aGame.gambler.hand[xj].CardValue == CARDVALUE.QUEEN ||
                            aGame.gambler.hand[xj].CardValue == CARDVALUE.KING))
                {
                    if (imBtns[xj].ImageUrl == aGame.gambler.hand[xj].PictureUrlString)
                    {
                        imBtns[xj].Style["border-style"] = "dotted";
                        imBtns[xj].Style["border-width"] = "2";
                        imBtns[xj].Style["border-color"] = "deeppink";
                        if (!btnSet)
                        {
                            if (handPairNumber == 0) this.b20a.Style["border-color"] = "purple";
                            if (handPairNumber == 1) this.b20b.Style["border-color"] = "purple";
                            btnSet = true;
                        }
                    }
                }
            }
        }

        protected void Perform_PairMarriage(char colorChar, int handPairNumber)
        {
            String sayPair;

            if (globalVariable != null && aGame == null)
            {
                aGame = globalVariable.Game;
            }

            if ((aGame.pSaid) || (aGame.gambler.handpairs[handPairNumber] == 'n'))
            {
                return;
            }

            aGame.said = aGame.gambler.handpairs[handPairNumber];
            if (aGame.gambler.handpairs[handPairNumber] == aGame.AtouInGame)
            {
                aGame.gambler.points += 40;
                sayPair = ResReader.GetRes("fourty_in_color", Locale) +
                    " " + aGame.PrintColor(aGame.said);
            }
            else
            {
                aGame.gambler.points += 20;
                sayPair = ResReader.GetRes("twenty_in_color", Locale) +
                    " " + aGame.PrintColor(aGame.said);
            }
            aGame.pSaid = true;
            ResetButtons(0);

            string msg0 = ResReader.GetStringFormated("you_say_pair", Locale, 
                aGame.PrintColor(aGame.said));
            SayTextMessage(msg0);

            tPoints.Text = aGame.gambler.points.ToString();
            if (aGame.gambler.points >= Constants.ENOUGH)
            {
                TwentyEnough(PLAYERDEF.HUMAN);
                return;
            }

            Perform_DotBorderCards_OnMarrgiage(aGame.said, handPairNumber);
        }

        protected void Reset_ImgOut(bool reset = true)
        {
            if (reset)
            {
                imOut20.Style["border-style"] = "none";
                imOut20.Style["border-width"] = "1";
                imOut20.Style["border-color"] = "none";
                imOut21.Style["border-style"] = "none";
                imOut21.Style["border-width"] = "1";
                imOut21.Style["border-color"] = "none";
            } 
            else
            {
                imOut20.Style["border-style"] = "dashed";
                imOut20.Style["border-width"] = "2";
                imOut20.Style["border-color"] = "purple";
                imOut21.Style["border-style"] = "dashed";
                imOut21.Style["border-width"] = "2";
                imOut21.Style["border-color"] = "deeppink";
            }
        }

        protected void Reset_PlayerCardsBorder()
        {
            im0.Style["border-width"] = "2";
            im0.Style["border-style"] = "groove";
            im0.Style["border-color"] = "azure";
            im1.Style["border-width"] = "2";
            im1.Style["border-style"] = "groove";
            im1.Style["border-color"] = "azure";
            im2.Style["border-width"] = "2";
            im2.Style["border-style"] = "groove";
            im2.Style["border-color"] = "azure";
            im3.Style["border-width"] = "2";
            im3.Style["border-style"] = "groove";
            im3.Style["border-color"] = "azure";
            im4.Style["border-width"] = "2";
            im4.Style["border-style"] = "groove";
            im4.Style["border-color"] = "azure";

            b20a.Style["border-color"] = "darkslategray";
            b20b.Style["border-color"] = "darkslategray";
        }

        protected void ShowPlayersCards(SCHNAPSTATE gameState)
        {
            int schnapStateVal = SCHNAPSTATE_Extensions.StateValue(gameState);
            if (schnapStateVal >= 16 && schnapStateVal < 22 && gameState != SCHNAPSTATE.GAME_START)
            {
                try
                {
                    im0.ImageUrl = aGame.gambler.hand[0].PictureUrlString;
                    im0.ToolTip = aGame.gambler.hand[0].FullName;
                    im1.ImageUrl = aGame.gambler.hand[1].PictureUrlString;
                    im1.ToolTip = aGame.gambler.hand[1].FullName;
                    im2.ImageUrl = aGame.gambler.hand[2].PictureUrlString;
                    im2.ToolTip = aGame.gambler.hand[2].FullName; 
                    im3.ImageUrl = aGame.gambler.hand[3].PictureUrlString;
                    im3.ToolTip = aGame.gambler.hand[3].FullName;
                    im4.ImageUrl = aGame.gambler.hand[4].PictureUrlString;
                    im4.ToolTip = aGame.gambler.hand[4].FullName;
                }
                catch (Exception exp)
                {
                    this.ErrHandler(exp);
                }
            }
            else
            {
                im0.ImageUrl = emptyURL.ToString();
                im0.ToolTip = string.Empty;
                im1.ImageUrl = emptyURL.ToString();
                im1.ToolTip = string.Empty;
                im2.ImageUrl = emptyURL.ToString();
                im2.ToolTip = string.Empty;
                im3.ImageUrl = emptyURL.ToString();
                im3.ToolTip = string.Empty;
                im4.ImageUrl = emptyURL.ToString();
                im4.ToolTip = string.Empty;
            }
        }

        /// <summary>
        /// showPlayedOutCards - shows playedOutCards => needed when changing locale and card deck
        /// </summary>
        protected void ShowPlayedOutCards()
        {
            if ((aGame != null && aGame.playedOut0 != null && playedOutCard0 != null &&
                aGame.playedOut0.ColorValue != playedOutCard0.ColorValue) ||
                    (aGame != null && aGame.playedOut0 != null && playedOutCard0 == null))
            {
                playedOutCard0 = aGame.playedOut0;
            }
            if (aGame == null && playedOutCard0 == null)
                playedOutCard0 = globalVariable.CardEmpty;
            // imOut0.ImageUrl = playedOutCard0.PictureUrlString;
            imOut20.ImageUrl = playedOutCard0.PictureUrlString;

            if ((aGame != null && aGame.playedOut1 != null && playedOutCard1 != null &&
                aGame.playedOut1.ColorValue != playedOutCard1.ColorValue) ||
                    (aGame != null && aGame.playedOut1 != null && playedOutCard1 == null))
            {
                playedOutCard1 = aGame.playedOut1;
            }
            if (playedOutCard1 == null)
                playedOutCard1 = globalVariable.CardEmpty;
            // imOut1.ImageUrl = playedOutCard1.PictureUrlString;
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
                    this.imMerge11.Visible = true;
                    try
                    {
                        this.imMerge11.ToolTip = ResReader.GetRes("imageMerge_ToolTip", Locale);
                        this.imOut20.ToolTip = string.Empty;
                        this.imOut21.ToolTip = string.Empty;
                        Reset_ImgOut(true);
                    }
                    catch (Exception exMergeVisible) {
                        this.ErrHandler(exMergeVisible);
                    }
                    // ImageMerge.Visible = true;
                }
                else
                {
                    this.imMerge11.Visible = false;
                    try
                    {
                        this.imOut20.ToolTip = string.Empty;
                        this.imOut21.ToolTip = string.Empty;
                    }
                    catch (Exception) { }
                    // ImageMerge.Visible = false;
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
                    spanAtouTalon.Visible = true;
                    spanAtouTalon.Style["visibility"] = "visible";
                    this.imAtou10.Visible = true;

                    if (gameState == SCHNAPSTATE.GAME_CLOSED)
                    {
                        imAtou10.ImageUrl = notURL.ToString();
                        imAtou10.ToolTip = ResReader.GetRes("imageAtou_AltText", Locale);
                    }
                    else
                    {
                        imAtou10.ImageUrl = aGame.set[19].PictureUrlString;
                        imAtou10.ToolTip = ResReader.GetRes("imageAtou_ToolTip", Locale);
                    }
                }
                else
                {
                    this.imAtou10.Visible = false;
                    this.imAtou10.ImageUrl = emptyURL.ToString();

                    spanAtouTalon.Style["visibility"] = "hidden";
                    if (schnapStateVal >= 20)
                        spanAtouTalon.Visible = false;
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
                this.spanAtouTalon.Visible = true;                

                int schnapStateVal = SCHNAPSTATE_Extensions.StateValue(gameState);
                if (schnapStateVal >= 15 && schnapStateVal < 20)
                {
                    spanAtouTalon.Style["visibility"] = "visible";

                    if (gameState == SCHNAPSTATE.GAME_START)
                        imTalon.ImageUrl = emptyTalonUri.ToString();
                    else
                        imTalon.ImageUrl = talonURL.ToString();
                    imTalon.Visible = true;
                }
                else
                {
                    imTalon.ImageUrl = emptyURL.ToString();
                    imTalon.ImageUrl = talonURL.ToString();
                    imTalon.Visible = false;

                    spanAtouTalon.Style["visibility"] = "hidden";
                    if (schnapStateVal >= 20)
                        spanAtouTalon.Visible = false;                    
                }
            }
            catch (Exception imTalonEx)
            {
                ErrHandler(imTalonEx);
            }
        }

        /// <summary>
        /// Show Stitches
        /// </summary>
        /// <param name="whichStitch"><see cref="STITCH" />, <see cref="STITCH.PLAYER"/> is implemented,
        /// but commented out, since too view space on page (don't know where to place it)</param>
        protected void ShowStitches(STITCH whichStitch)
        {
            if (aGame != null && aGame.gambler != null && aGame.computer != null)
            {
                if (whichStitch.Value() < -2)
                {
                    ImageComputerStitch0a.Visible = false;
                    ImageComputerStitch0b.Visible = false;
                    // ImagePlayerStitch0a.Visible = false;
                    // ImagePlayerStitch0b.Visible = false;
                    SpanComputerStitches.Style["visibility"] = "visible";
                    // PlaceHolderPlayerStitches.Visible = false;
                }
                else
                {
                    if (aGame.computer.cardStitches.Count > 0)
                    {
                        SpanComputerStitches.Style["visibility"] = "visible";
                        SpanComputerStitches.Visible = true;
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
                if (whichStitch == STITCH.HIDDEN)
                {
                    ImageComputerStitch0a.ImageUrl = notURL.ToString();
                    ImageComputerStitch0b.ImageUrl = notURL.ToString();
                    // ImagePlayerStitch0a.ImageUrl = notURL.ToString();
                    // ImagePlayerStitch0b.ImageUrl = notURL.ToString();
                }
                if (whichStitch == STITCH.COMPUTER && aGame.computer.stitchCount > 0)
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
                if (whichStitch == STITCH.PLAYER && aGame.gambler.stitchCount > 0)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="computerPlayedOut"></param>
        /// <param name="stage"></param>
        protected void ShowComputer20(Card computerPlayedOut, int stage)
        {
            for (int ci = 0; ci < aGame.computer.hand.Length; ci++)
            {
                if (computerPlayedOut.CardValue == CARDVALUE.QUEEN &&
                        aGame.computer.hand[ci].CardColor == computerPlayedOut.CardColor &&
                        aGame.computer.hand[ci].CardValue == CARDVALUE.KING)
                {
                    // imOut0.ImageUrl = aGame.computer.hand[ci].PictureUrlString;
                    imOut20.ImageUrl = aGame.computer.hand[ci].PictureUrlString;
                    break;
                }
                if (computerPlayedOut.CardValue == CARDVALUE.KING &&
                        aGame.computer.hand[ci].CardColor == computerPlayedOut.CardColor &&
                        aGame.computer.hand[ci].CardValue == CARDVALUE.QUEEN)
                {
                    // imOut0.ImageUrl = computerPlayedOut.PictureUrlString;
                    imOut20.ImageUrl = aGame.computer.hand[ci].PictureUrlString;
                    break;
                }
            }
            stage--;
            // imOut1.ImageUrl = computerPlayedOut.PictureUrlString;
            Reset_ImgOut(false);
            imOut21.ImageUrl = computerPlayedOut.PictureUrlString;            
        }

        /// <summary>
        /// Stop_Click EventHandler, when Player clicks on Stop Button
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Stop_Click(object sender, EventArgs e)
        {
            try
            {
                this.metaAudio.Content = "";
                this.metaLastAudio.Content = "";
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
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Change_Click(object sender, EventArgs e)
        {
            aGame.ChangeAtou(aGame.gambler);

            string msgChange = ResReader.GetRes("bChange_text", Locale);
            SetTextMessage(msgChange);

            bChange.Enabled = false;
            ShowAtouCard(aGame.schnapState);
            ShowPlayersCards(aGame.schnapState);
            GameTurn(1);
        }

        /// <summary>
        /// 1st button for pair marriage click EventHandler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void A20_Click(object sender, EventArgs e)
        {
            char colorSaid = aGame.gambler.handpairs[0];
            if (colorSaid != 'n')
                Perform_PairMarriage(colorSaid, 0);
        }

        /// <summary>
        /// 2nd button for pair marriage click EventHandler 
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void B20_Click(object sender, EventArgs e)
        {
            char colorSaid = aGame.gambler.handpairs[1];
            if (colorSaid != 'n')
                Perform_PairMarriage(colorSaid, 1);
        }


        /// <summary>
        /// Clicking on any Card Image EventHandler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ImageCard_Click(object sender, EventArgs e)
        {
            int ic = 0;

            // don't let player drag and drop cards, when he shouldn't
            if (aGame == null || (aGame != null && (!aGame.isReady || sender == null)))
            {
                return;
            }

            string senderStr = "";
            if (sender is WebControl)
            {
                senderStr = ((WebControl)sender).ClientID;
            }
            if (sender is System.Web.UI.WebControls.ImageButton)
                senderStr = ((System.Web.UI.WebControls.ImageButton)sender).ClientID;
            if (senderStr.StartsWith("imAtou"))
                senderStr = senderStr.Replace("imAtou", "");
            if (senderStr.StartsWith("imOut"))
                senderStr = senderStr.Replace("imOut", "");
            if (senderStr.StartsWith("imMerge"))
                senderStr = senderStr.Replace("imMerge", "");
            if (senderStr.StartsWith("im"))
                senderStr = senderStr.Replace("im", "");

            if (!Int32.TryParse(senderStr, out ic))
            {
                return;
            }

            try
            {
                if (ic == 10)
                {
                    if (aGame.playersTurn && !aGame.pSaid && aGame.CanCloseOrChange)
                    {
                        CloseGame(PLAYERDEF.HUMAN);
                        SayMsg(ResReader.GetRes("you_close_game", Locale));
                    }
                    return;
                }
                if (ic == 11 || ic == 20 || ic == 21)
                {
                    Continue_Click(sender, e);
                    return;
                }
                if (aGame.isGame == false || aGame.gambler == null || aGame.gambler.hand[ic] == null || !aGame.gambler.hand[ic].IsValidCard)
                {
                    String msgVC = ResReader.GetRes("this_is_no_valid_card", Locale);
                    SayTextMessage(msgVC, true, true);
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
                        String msgPlayPair = ResReader.GetRes("you_must_play_pair_card", Locale);
                        SayTextMessage(msgPlayPair, true, true);
                        return;
                    }
                }
                if (aGame.colorHitRule && (!aGame.playersTurn))
                {
                    ccard = globalVariable.CcCard;

                    // CORRECT WAY ?
                    if ((!aGame.gambler.IsValidInColorHitsContext(ic, aGame.computer.hand[ccard])))
                    {
                        String msgColorHitRule = ResReader.GetRes("you_must_play_color_hit_force_rules", Locale);
                        SayTextMessage(msgColorHitRule, true, false);
                        
                        int tmpint = aGame.gambler.PreferedInColorHitsContext(aGame.computer.hand[ccard]);
                        // for (j = 0; j < 5; j++) {
                        //     c_array = c_array + aGame.gambler.colorHitArray[j] + " ";
                        // }
                        // aGame.mqueue.insert(c_array);

                        String msgBestWouldBe = ResReader.GetStringFormated("best_card_would_be", Locale, 
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
                Reset_PlayerCardsBorder();
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
                    default:
                        break;
                }

                playedOutCard0 = aGame.gambler.hand[ic];
                aGame.playedOut0 = playedOutCard0;
                // imOut0.ImageUrl = aGame.gambler.hand[ic].PictureUrlString;
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
                ToggleTournament(true);
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

        /// <summary>
        /// Image Merge Click Eventhandler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Merge_Click(object sender, EventArgs e)
        {
            ToggleTournament(true);
            StartGame();
        }

        /// <summary>
        /// Image Computer Stitch Eventhandler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ImageComputerStitch_Click(object sender, EventArgs e)
        {
            if (aGame.computer.cardStitches.Count > 0)
            {
                if (ImageComputerStitch0a.ImageUrl == notURL.ToString() ||
                    ImageComputerStitch0b.ImageUrl == notURL.ToString())
                {
                    ShowStitches(STITCH.COMPUTER);
                }
                else
                    ShowStitches(STITCH.HIDDEN);
            }
        }
        
        /// <summary>
        /// Eventhandler, when clicking on players stitches
        /// (Implemented, but commented out, since no place for players stitches on page).
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e"><see cref="EventArgs"/> e</param>
        protected void ImagePlayerStitch_Click(object sender, EventArgs e)
        {
            ShowStitches(STITCH.PLAYER);
        }

        /// <summary>
        /// Toggle Continue
        /// </summary>
        /// <param name="continueEnabled">toggle continue</param>
        protected void ToggleContinue(bool continueEnabled = true)
        {
            aGame.shouldContinue = continueEnabled;
            Reset_ImgOut(true);

            imOut20.ToolTip = (continueEnabled) ? ResReader.GetRes("continue_ToolTip", Locale) : "";
            imOut21.ToolTip = (continueEnabled) ? ResReader.GetRes("continue_ToolTip", Locale) : "";
            bContinue.ToolTip = (continueEnabled) ? ResReader.GetRes("continue_ToolTip", Locale) : "";
            bContinue.Enabled = continueEnabled;
        }

        /// <summary>
        /// Toggle Tourament
        /// </summary>
        /// <param name="starts"></param>
        protected void ToggleTournament(bool starts = true)
        {
            if (starts)
            {
                if (aTournement.WonTournament != PLAYERDEF.UNKNOWN)
                {
                    globalVariable = new GlobalAppSettings(this.Context);
                    aTournement = new Tournament();
                    globalVariable.Tournement = aTournement;
                    this.Context.Session[Constants.APPNAME] = globalVariable;
                    DrawPointsTable();
                }
            }
            else
            {
                if (aTournement.WonTournament != PLAYERDEF.UNKNOWN)
                {
                    string endTournementMsg = "";
                    if (aTournement.WonTournament == PLAYERDEF.HUMAN)
                    {
                        if (aTournement.Taylor)
                        {
                            endTournementMsg = ResReader.GetRes("you_won_taylor", Locale);
                            DrawPointsTable(2, aTournement.WonTournament);
                        }
                        else
                        {
                            endTournementMsg = ResReader.GetRes("you_won_tournement", Locale);
                            DrawPointsTable(1, aTournement.WonTournament);
                        }
                    }
                    else if (aTournement.WonTournament == PLAYERDEF.COMPUTER)
                    {
                        if (aTournement.Taylor)
                        {
                            endTournementMsg = ResReader.GetRes("computer_won_taylor", Locale);
                            DrawPointsTable(2, aTournement.WonTournament);
                        }
                        else
                        {
                            endTournementMsg = ResReader.GetRes("computer_won_tournement", Locale);
                            DrawPointsTable(1, aTournement.WonTournament);
                        }
                    }
                    SetTextMessage(endTournementMsg);
                    // TODO: excited end animation
                }
            }
        }

        /// <summary>
        /// Draw Tournament points table
        /// </summary>
        /// <param name="displayBummerlOrTaylor"></param>
        /// <param name="whoWon"><see cref="PLAYERDEF"/></param>
        protected override void DrawPointsTable(short displayBummerlOrTaylor = 0, PLAYERDEF whoWon = PLAYERDEF.UNKNOWN)
        {
            base.DrawPointsTable(displayBummerlOrTaylor, whoWon);
        }

        /// <summary>
        /// Reset Buttons
        /// </summary>
        /// <param name="level"></param>
        protected void ResetButtons(int level)
        {
            if (level >= 0)
            {
                if (aGame != null)
                {
                    aGame.a20 = false;
                    aGame.b20 = false;
                    aGame.bChange = false;
                }

                b20a.Text = ResReader.GetRes("b20a_text", Locale);
                b20a.ToolTip = b20a.Text;
                b20a.Enabled = false;

                b20b.Text = ResReader.GetRes("b20b_text", Locale);
                b20b.ToolTip = b20b.Text;
                b20b.Enabled = false;

                bChange.Text = ResReader.GetRes("bChange_text", Locale);
                bChange.ToolTip = bChange.Text;
                bChange.Enabled = false;
            }

            if (level >= 1)
            {
                if (aGame != null)
                {
                    aGame.shouldContinue = false;
                }
                bContinue.Text = ResReader.GetRes("bContinue_text", Locale);
                bContinue.ToolTip = "";
                bContinue.Enabled = false;

                ShowAtouCard(SCHNAPSTATE.GAME_START);
                ShowTalonCard(SCHNAPSTATE.GAME_START);
                ShowMergeAnim(SCHNAPSTATE.GAME_START);
            }

            if (level > 3)
            {
                try
                {
                    Reset_ImgOut(true);
                    // imOut0.ImageUrl = emptyURL.ToString();
                    imOut20.ImageUrl = emptyURL.ToString();
                    // imOut1.ImageUrl = emptyURL.ToString();
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

        /// <summary>
        /// Stop Game
        /// </summary>
        /// <param name="tournementPts"></param>
        /// <param name="whoWon"><see cref="PLAYERDEF"/></param>
        /// <param name="endMessage">end message to say</param>
        protected void StopGame(int tournementPts, PLAYERDEF whoWon = PLAYERDEF.UNKNOWN, string endMessage = null)
        {
            if (!string.IsNullOrEmpty(endMessage))
            {
                SayTextMessage(endMessage);
                // SetTextMessage(endMessage);
            }
            aTournement.AddPointsRotateGiver(tournementPts, whoWon);
            bStop.Enabled = false;
            aGame.StopGame();

            ResetButtons(tournementPts);
            ShowStitches(STITCH.NONE);
            DrawPointsTable();
            lAtouIs.Text = ResReader.GetRes("symbol_n", Locale);
            lAtouIs.ToolTip = ResReader.GetRes("nogame_started", Locale);
            ShowPlayersCards(aGame.schnapState);
            aGame.Dispose();

            bMerge.Enabled = true;
            bMerge.Visible = true;
            bStop.Enabled = false;
            bStop.Visible = false;
            this.bContinue.Enabled = true;
            this.imOut20.ToolTip = ResReader.GetRes("imageMerge_ToolTip", Locale);
            this.imOut21.ToolTip = ResReader.GetRes("imageMerge_ToolTip", Locale);
            this.imMerge11.ToolTip = ResReader.GetRes("imageMerge_ToolTip", Locale);
            Reset_ImgOut(true);

            this.ToggleTournament(false);
        }

        /// <summary>
        /// Starts a new game
        /// </summary>
        protected void StartGame()
        {  
            /* Mischen */
            bMerge.Enabled = false;
            bMerge.Visible = false;

            aGame = null;
            // aGame = new Game(HttpContext.Current, PLAYERDEF.HUMAN);
            aGame = new Game(HttpContext.Current, aTournement.NextGameGiver); 
            aGame.isReady = true;
            tMsg.Visible = false;
            ResetButtons(1);
            this.PreInnerText = string.Empty;
            // tRest.Text = (19 - aGame.index).ToString();
            this.metaAudio.Content = "";
            this.metaLastAudio.Content = "";

            ShowStitches(STITCH.NONE);
            emptyTmpCard = new Card(-2, HttpContext.Current);
            tPoints.Text = "" + aGame.gambler.points;
            lAtouIs.Text = aGame.PrintSymbol(CARDCOLOR_Extensions.ColorChar(aGame.set[19].CardColor));
            lAtouIs.ToolTip = ResReader.GetStringFormated("atou_is", Locale,
                aGame.PrintColor(CARDCOLOR_Extensions.ColorChar(aGame.set[19].CardColor)));
            ShowAtouCard(aGame.schnapState);
            ShowTalonCard(aGame.schnapState);
            ShowMergeAnim(aGame.schnapState);
            bStop.Visible = true;
            bStop.Enabled = true;

            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
            GameTurn(0);
        }

        /// <summary>
        /// CloseGame - implements closing game => Zudrehens
        /// </summary>
        /// <param name="whoCloses">PLAYERDEF player or computer</param>
        protected void CloseGame(PLAYERDEF whoCloses)
        {
            if (aGame.isGame == false || aGame.gambler == null || aGame.colorHitRule)
            {
                SetTextMessage(ResReader.GetRes("nogame_started", Locale));
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

        /// <summary>
        /// TwentyEnough 
        /// </summary>
        /// <param name="whoWon"><see cref="PLAYERDEF"/></param>
        protected void TwentyEnough(PLAYERDEF whoWon)
        {
            int xj = 0;
            String andEnough = ResReader.GetRes("twenty_and_enough", Locale);
            aGame.isReady = false;

            if (whoWon == PLAYERDEF.HUMAN)
            {
                if (aGame.said == aGame.AtouInGame)
                {
                    andEnough = ResReader.GetRes("fourty_and_enough", Locale);
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
                            // imOut0.ImageUrl = aGame.gambler.hand[xj].PictureUrlString;
                            imOut20.ImageUrl = aGame.gambler.hand[xj].PictureUrlString;
                        }
                        if (colorCh0 == aGame.said &&
                                aGame.gambler.hand[xj].CardValue == CARDVALUE.KING)
                        {
                            playedOutCard1 = aGame.gambler.hand[xj];
                            aGame.playedOut1 = playedOutCard1;
                            // imOut1.ImageUrl = aGame.gambler.hand[xj].PictureUrlString;
                            imOut21.ImageUrl = aGame.gambler.hand[xj].PictureUrlString;
                        }
                    }
                }
                catch (Exception jbex)
                {
                    this.ErrHandler(jbex);
                }

                string sEnds11 = andEnough + " " + ResReader.GetStringFormated("you_win_with_points", Locale,
                    aGame.gambler.points.ToString());
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                StopGame(tPts, PLAYERDEF.HUMAN, sEnds11);
            }
            else // Computer won
            {
                if (aGame.csaid == aGame.AtouInGame)
                {
                    andEnough = ResReader.GetRes("fourty_and_enough", Locale);
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
                            // imOut0.ImageUrl = aGame.computer.hand[xj].PictureUrlString;
                            imOut20.ImageUrl = aGame.computer.hand[xj].PictureUrlString;
                        }
                        if (colorCh1 == aGame.csaid &&
                            aGame.computer.hand[xj].CardValue == CARDVALUE.KING)
                        {
                            playedOutCard1 = aGame.computer.hand[xj];
                            aGame.playedOut1 = playedOutCard1;
                            // imOut1.ImageUrl = aGame.computer.hand[xj].PictureUrlString;
                            imOut21.ImageUrl = aGame.computer.hand[xj].PictureUrlString;
                        }
                    }
                }
                catch (Exception enoughEx1)
                {
                    this.ErrHandler(enoughEx1);
                }

                PrintMsg();
                string sEnds12 = andEnough + " " + ResReader.GetStringFormated("computer_has_won_points", Locale, 
                    aGame.computer.points.ToString());
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.COMPUTER);
                StopGame(tPts, PLAYERDEF.COMPUTER, sEnds12);
                // stopGame(1, new String(andEnough + " Computer hat gewonnen mit " + String.valueOf(aGame.computer.points) + " Punkten !"));
            }
            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
            return;
        }

        [Obsolete("twentyEnough_Old(bool who) is obsolete and replaced with protected void twentyEnough(PLAYERDEF whoWon)", false)]
        void twentyEnough_Old(bool who)
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
                                // imOut0.ImageUrl = enoughQueenUrl.ToString();
                                imOut20.ImageUrl = enoughQueenUrl.ToString();
                                Uri enoughKingUrl = new Uri(
                                    "https://area23.at/schnapsen/cardpics/" + aGame.said + "4.gif");
                                // imOut1.ImageUrl = enoughKingUrl.ToString();
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
                                // imOut0.ImageUrl = enoughCQueenUrl.ToString();
                                imOut20.ImageUrl = enoughCQueenUrl.ToString();
                                Uri enoughCKingUrl = new Uri(
                                    "https://area23.at/schnapsen/cardpics/" + aGame.csaid + "4.gif");
                                // imOut1.ImageUrl = enoughCKingUrl.ToString();
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

        /// <summary>
        /// Game Turn, when a play move starts
        /// </summary>
        /// <param name="ixlevel"></param>
        protected void GameTurn(int ixlevel)
        {
            if (ixlevel < 1)
            {
                try
                {
                    // imOut0.ImageUrl = emptyURL.ToString();
                    imOut20.ImageUrl = emptyURL.ToString();
                    // imOut1.ImageUrl = emptyURL.ToString();
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

            ShowStitches(STITCH.HIDDEN);
            aGame.bChange = false;
            aGame.a20 = false;
            aGame.b20 = false;
            aGame.sayMarriage20 = ResReader.GetRes("b20a_text", Locale);
            aGame.sayMarriage40 = ResReader.GetRes("b20a_text", Locale);
            this.metaAudio.Content = "";
            this.metaLastAudio.Content = "";

            if (aGame.playersTurn)
            {
                string announceMsg = "";

                // Wann kann man austauschen ?
                if (ixlevel < 1)
                {
                    if (aGame.AtouIsChangable(aGame.gambler) && (!aGame.pSaid))
                    {
                        psaychange += 1;
                        bChange.Enabled = true;
                        aGame.bChange = true;
                        announceMsg = ResReader.GetRes("can_change_atou", Locale);
                    }
                }
                // Gibts was zum Ansagen ?
                int a20 = aGame.gambler.HasPair;
                if (a20 > 0)
                {                    
                    psaychange += 2;
                    b20a.Text = aGame.PrintSymbol(aGame.gambler.handpairs[0]) + " " +
                        ResReader.GetRes("say_pair", Locale);
                    aGame.sayMarriage20 = aGame.PrintColor(aGame.gambler.handpairs[0]) + " " +
                        ResReader.GetRes("say_pair", Locale);
                    aGame.a20 = true;
                    b20a.Enabled = true;
                    if (string.IsNullOrEmpty(announceMsg))
                        announceMsg = ResReader.GetStringFormated("can_say_pair", Locale,
                            aGame.PrintColor(aGame.gambler.handpairs[0]));

                    if (a20 > 1)
                    {
                        b20b.Text = aGame.PrintSymbol(aGame.gambler.handpairs[1]) + " " +
                            ResReader.GetRes("say_pair", Locale);
                        aGame.b20 = true;
                        aGame.sayMarriage40 = aGame.PrintColor(aGame.gambler.handpairs[1]) + " " +
                            ResReader.GetRes("say_pair", Locale);
                        b20b.Enabled = true;
                        announceMsg += " " + ResReader.GetStringFormated("can_say_pair", Locale,
                            aGame.PrintColor(aGame.gambler.handpairs[1]));
                    }
                    else
                    {
                        aGame.sayMarriage40 = ResReader.GetRes("no_second_pair", Locale);
                        b20b.Text = ResReader.GetRes("no_second_pair", Locale);
                    }                    
                }                
                
                // Say announce message
                if (!string.IsNullOrEmpty(announceMsg))
                    SayMsg(announceMsg);
                // Info 
                SetTextMessage(ResReader.GetRes("toplayout_clickon_card", Locale));
            }
            else
            {
                /* COMPUTERS TURN IMPLEMENTIEREN */
                string outPutMessage = "";
                ccard = aGame.ComputerStarts();
                globalVariable.CcCard = ccard;

                int bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.CHANGEATOU);
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    this.ShowAtouCard(aGame.schnapState);
                    outPutMessage += ResReader.GetRes("computer_changes_atou", Locale);
                }

                bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.SAYPAIR);
                bool computerSaid20 = false;
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    computerSaid20 = true;
                    String computerSaysPair = ResReader.GetStringFormated("computer_says_pair", Locale,
                        aGame.PrintColor(aGame.csaid));
                    outPutMessage = outPutMessage + " " + computerSaysPair;
                }
                if (outPutMessage == "")
                    outPutMessage = ResReader.GetRes("computer_plays_out", Locale);
                SayTextMessage(outPutMessage);

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
                    outPutMessage += ResReader.GetRes("computer_closed_game", Locale);
                    SayTextMessage(outPutMessage);
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

                    // imOut1.ImageUrl = aGame.computer.hand[ccard].PictureUrlString;
                    imOut21.ImageUrl = aGame.computer.hand[ccard].PictureUrlString;
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception jbpex)
                {
                    this.ErrHandler(jbpex);
                }

                String msgTxt33 = ResReader.GetRes("toplayout_clickon_card", Locale);
                // setTextMessage(msgTxt33);            
            }

            aGame.isReady = true;
            PrintMsg();
            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
        }

        /// <summary>
        /// End turn
        /// </summary>
        protected void EndTurn()
        {
            int tmppoints;
            String msgText = "";

            /* implement computers strategy here */
            if (aGame.playersTurn)
            {
                ccard = aGame.ComputersAnswer();
                globalVariable.CcCard = ccard;
                try
                {
                    playedOutCard1 = aGame.computer.hand[ccard];
                    // imOut1.ImageUrl = aGame.computer.hand[ccard].PictureUrlString;
                    imOut21.ImageUrl = aGame.computer.hand[ccard].PictureUrlString;
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception jbpvex)
                {
                    this.ErrHandler(jbpvex);
                }
            }

            ccard = globalVariable.ClearCcCard();

            tmppoints = aGame.CheckPoints(ccard);
            aGame.computer.hand[ccard] = globalVariable.CardEmpty;
            tPoints.Text = aGame.gambler.points.ToString();

            if (tmppoints > 0)
            {
                msgText = ResReader.GetStringFormated("your_hit_points", Locale, tmppoints.ToString());
                SayTextMessage(msgText);
                msgText += " " + ResReader.GetRes("click_continue", Locale);
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
                    string sEnds0 = ResReader.GetRes("computer_closing_failed", Locale);
                    StopGame(3, PLAYERDEF.HUMAN, sEnds0);
                    return;
                }
            }
            else
            {
                msgText = ResReader.GetStringFormated("computer_hit_points", Locale, (-tmppoints).ToString());
                SayTextMessage(msgText);
                msgText +=  " " + ResReader.GetRes("click_continue", Locale);
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
                    string sEnds1 = ResReader.GetRes("closing_failed", Locale);
                    StopGame(3, PLAYERDEF.COMPUTER, sEnds1);
                    return;
                }
            }

            // Assign new cards
            int assignCardState = aGame.AssignNewCard();
            if (assignCardState == 1)
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

                string msgChFrc = ResReader.GetRes("color_hit_force_mode", Locale);
                SetTextMessage(msgChFrc);
            }

            // tRest.Text = (19 - aGame.index).ToString();
            PrintMsg();

            // resetButtons(0);
            aGame.pSaid = false;
            aGame.said = 'n';
            aGame.csaid = 'n';

            if (aGame.gambler.points >= Constants.ENOUGH)
            {
                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                string sEnds3 = ResReader.GetStringFormated("you_win_with_points", Locale, aGame.gambler.points.ToString());
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                StopGame(tPts, PLAYERDEF.HUMAN, sEnds3);
                return;
            }
            if (aGame.computer.points >= Constants.ENOUGH)
            {
                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                string sEnds4 = ResReader.GetStringFormated("computer_has_won_points", Locale, aGame.computer.points.ToString());
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                StopGame(tPts, PLAYERDEF.COMPUTER, sEnds4);
                return;
            }

            if (aGame.schnapState == SCHNAPSTATE.ZERO_CARD_REMAINS || assignCardState == 5 || aGame.ZeroRemain)
            {
                if (aGame.isClosed) // close game => must have over 66 or loose
                {
                    if (aGame.gambler.hasClosed)
                    {
                        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                        string sEnds6 = ResReader.GetRes("closing_failed", Locale);
                        StopGame(3, PLAYERDEF.COMPUTER, sEnds6);
                    }
                    try
                    {
                        if (aGame.computer.hasClosed)
                        {
                            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                            string sEnds7 = ResReader.GetRes("computer_closing_failed", Locale);
                            StopGame(3, PLAYERDEF.HUMAN, sEnds7);
                        }
                    }
                    catch (Exception jbpvex)
                    {
                        this.ErrHandler(jbpvex);
                    }
                    return;
                }
                else
                {
                    if (tmppoints > 0)
                    {
                        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                        string sEnds8 = ResReader.GetRes("last_hit_you_have_won", Locale);
                        int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                        StopGame(tPts, PLAYERDEF.HUMAN, sEnds8);
                    }
                    else
                    {
                        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                        string sEnds9 = ResReader.GetRes("computer_wins_last_hit", Locale);
                        int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                        StopGame(tPts, PLAYERDEF.COMPUTER, sEnds9);
                    }
                    return;
                }
            }

            ToggleContinue(true);
            aGame.isReady = false;
            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
        }
        
        /// <summary>
        /// PrintMsg to PreOut
        /// </summary>
        protected void PrintMsg()
        {
            this.PreInnerText = aGame.FetchMsg();
            // preOut.InnerText = aGame.FetchMsg();

            string[] msgs = aGame.FetchMsgArray();

            for (int i = aGame.fetchedMsgCount; i < msgs.Length; i++)
            {
                Log(msgs[i]);
            }
            aGame.fetchedMsgCount = msgs.Length;
        }

        /// <summary>
        /// ErrHandler Exception Handler for Error
        /// </summary>
        /// <param name="myErr"></param>
        protected void ErrHandler(Exception myErr)
        {
            ++errNum;
            // Log(myErr);
            string errStr = (errNum < 10) ? "  " : (errNum < 100) ? " " : "";
            errStr += String.Format("\tException {0} ⇒ {1}\t{2}\t{3}\r\n",
                myErr.GetType(),
                myErr.Message,
                myErr.ToString().Replace("\r", "").Replace("\n", " "),
                myErr.StackTrace.Replace("\r", "").Replace("\n", " "));            
            aGame.mqueue.Enqueue(errStr);
            
            SetTextMessage(errStr, true, true);
        }

        /// <summary>
        /// HandleException
        /// </summary>
        /// <param name="myException"></param>
        protected override void HandleException(Exception myException)
        {
            base.HandleException(myException);
            ErrHandler(myException);
        }

        /// <summary>
        /// setTextMessage shows a new Toast dynamic message
        /// </summary>
        /// <param name="textMsg">text to display</param>
        /// <param name="queueMsg">if true, queue message in internal message queue</param>
        /// <param name="printMsg">if true, print and log message</param>
        protected void SetTextMessage(string textMsg, bool queueMsg = false, bool printMsg = false)
        {
            string msgSet = string.IsNullOrWhiteSpace(textMsg) ? "" : textMsg;
            tMsg.Visible = true;
            tMsg.Text = msgSet;

            if (!string.IsNullOrEmpty(msgSet))
            {
                Log(msgSet);

                if (aGame != null)
                {
                    aGame.statusMessage = msgSet;
                    if (queueMsg)
                    {
                        aGame.InsertMsg(msgSet);
                        if (printMsg)
                            PrintMsg();

                        return;
                    }
                }
            }
        }


        /// <summary>
        /// SayTextMessage speaks out and sets a text message
        /// </summary>
        /// <param name="sayMsg">message to say</param>
        /// <param name="queueMsg">if true, queue message in internal message queue</param>
        /// <param name="printMsg">if true, print and log message</param>
        public void SayTextMessage(string sayMsg, bool queueMsg = false, bool printMsg = false)
        {
            SetTextMessage(sayMsg, queueMsg, printMsg);
            SayMsg(sayMsg);
        }

        

    }
}