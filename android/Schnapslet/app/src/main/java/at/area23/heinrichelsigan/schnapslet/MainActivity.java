package at.area23.heinrichelsigan.schnapslet;

import at.area23.heinrichelsigan.schnapslet.*;

import android.content.Context;
import android.content.res.Resources;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.os.Bundle;
import android.app.Activity;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Button;
import android.graphics.drawable.AnimationDrawable;
// import android.graphics.drawable.AnimatedImageDrawable;
import android.widget.ImageSwitcher;
import android.widget.ImageView;
import android.widget.TextView;
import android.content.res.Configuration;
import android.view.View;
import android.view.View.OnClickListener;
import java.net.URL;
// import com.google.cloud.translate.Translation;
// import com.google.cloud.translate.*;


public class MainActivity extends AppCompatActivity implements Runnable {

    private static final String API_KEY = "-----BEGIN PRIVATE KEY-----\nMIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQDDjZ+QmX6Zi514\nsFbIgT48HFuvXgWnmNbY7aBPW5gWq2kmISwxQcUG/JxdD2VasHiG66QAVgNHjQ8D\nRLyzPSmNUb4QVBMB4WHukqpBW97qG3Uhp4HnHYJ3Tg5XbHmjhFevxISG0ZLEni4C\nJMcNMTug6+VGDeNE/yISN42uhdiPsgTPIaGK/6FeG8KXLB9R501dYhiWprOuwhw5\nTXvAAaLyP+y/3N1/Q/4Po+WSusYqTUl1kNZ6/BvynmK4Bz+Ibakd59eBIn4xMOyK\nOxQuyC5GJbhRYjbcoEvbTzZy7CUk0nzrLunxzIucAr1SuOwJwIDz2yMM5wl/5nXY\nCm2RjzdnAgMBAAECggEAFWc50LTMI3gheyUpynZC3odoDZCn48kZstKHWkg3JDwM\nnSzCTn3ZV8NsRc86k6t+9Z1y7Mp9P2aT/xKV6LRICPyqZdUd43XMpzUMR20Lv+nT\nbySLVkVnkzFK5oyr35bLliRXMP5dJwH9HSTzWGFMGnfXN0yr1FBsZTwJWNGzez6a\nxX3tPFQXd4xwoZev+ZiEuaVgRGl6y1Va83QMw7rKOYA74NSBgMhZyhna+5O1fB3r\nH7mRsaCf+BI9HGYeu+mw9biJRBIHHqBcteT0I8wgXoxMews40elY5UrXYpHyfoV1\nSlYwLRcSaE4ugFO7zJIZGYrxE1Q6we6o6XuHsYCjyQKBgQDj/hOOJ89crQudFzm/\n1t8QHLWntQJzIU9NnazyXXT+coO3AX6qMDCwWy2o4gpku8gP4qqLErRLtCG+3f0T\nC6QHarLDhaONKIweArjJ7la9MsOqpeG9lZdOuzVxUWJCqTb75ykJBi/ickhDketb\nHJiGGTndU6YRIqc4atd4CKiO2wKBgQDbk2T9Nxm4TWvu5NRNYD9eMCVS8hFY5j0D\nU/Z4DDuO0ztktWVu+KQTMaMhn0iX+KjeuKt/ytfex8/uvbGx7cz9sUxP9GIZBKpB\nVTwNVr1Pt76YT5y+ngESlmueCVRQCFUYc//LCGeJh1s6PlmSM0ocV+8WvyrW9AUS\nYUx4g4ABZQKBgD/xyfBL8BfRHPnBQtwwWr29H6Ha3cYGqKRfPdt4JNEcsx6H18vJ\n2k4MNKEyTLH2DOWPsD9zTogRDIno3wsRb774yQyXlciIf8wG/Wb9ZuyHqWNaRRcU\nNqzJSvLuXX3O0fIS4mp6hsGfRe9VpMoYGhs6RgVyaZhSvM3RAX/UBdqTAoGAIC5A\n/c+GiHloWTHWX6S8hMxfnAF4Q2QzCvrSQ5PfYrZYnRDs1c/BFEMRGotis0sxTLsZ\n/3e2HaOBOQc6NM6aXZAPlCRIAEyruzmHvJi61CUk3OPGIDW+CIBdM2NApR4jgpr1\noUcRDZn159pdfEziDrdghh/sYmaPG7uA3qS/LPUCgYADPOzUYG45IPRb42R4qk0E\n5C83ekg5wz9PUsd6aZgRIvHZB3HgZ2p7bnHvMB0DBF+F4WPNB8zsY39lels/lC80\npDcK7XJtcm6ucbWJt0d8eyrxjlwGAzfcvOpubC/McVtW6Atj5+FVTi7dBvhqUSac\nzEXeRxpEeNilJzgNENDtAQ==\n-----END PRIVATE KEY-----\n";
    // Button bStart, bStop, bHelp,
    Button b20a, b20b,  bChange, bContinue;
    ImageView im0,im1,im2, im3, im4, imOut0, imOut1, imTalon, imAtou, imMerge;
    TextView tRest, tPoints, tMes, tDbg;
    Menu myMenu;

    AnimationDrawable frameAnimation;
    // android.graphics.drawable.AnimatedImageDrawable animatedGif;

    long errNum = 0; // Errors Ticker
    int ccard; // Computers Card played
    volatile Card emptyTmpCard;
    volatile boolean ready = false; // Ready to play
    volatile byte psaychange = 0;
    boolean pSaid = false; // Said something
    static java.lang.Runtime runtime = null;
    // URL emptyURL, backURL, talonURL, notURL;
    // static String emptyJarStr =	"cardpics/e.gif";
    // static String backJarStr =	"cardpics/verdeckt.gif";
    // static String notJarStr = 	"cardpics/n0.gif";
    // static String talonJarStr =	"cardpics/t.gif";
    Game aGame;

    /**
     * Override onCreate
     * @param savedInstanceState
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        String layoutMes = null;
        if(getResources().getDisplayMetrics().widthPixels > getResources().getDisplayMetrics().heightPixels)
        {
            layoutMes = new String(getString(R.string.landscape_mode));
            setContentView(R.layout.activity_main_vertical);
        }
        else
        {
            layoutMes = new String(getString(R.string.portrait_mode));
            setContentView(R.layout.activity_main);
        }


        im0 = (ImageView) findViewById(R.id.im0);
        im1 = (ImageView) findViewById(R.id.im1);
        im2 = (ImageView) findViewById(R.id.im2);
        im3 = (ImageView) findViewById(R.id.im3);
        im4 = (ImageView) findViewById(R.id.im4);
        // im4 = (ImageView) findViewById(R.id.im4);

        imOut0 =  (ImageView) findViewById(R.id.imOut0);
        imOut1 =  (ImageView) findViewById(R.id.imOut1);
        imTalon =  (ImageView) findViewById(R.id.imTalon);
        imAtou =  (ImageView) findViewById(R.id.imAtou);
        imTalon.setVisibility(View.INVISIBLE);
        imAtou.setVisibility(View.INVISIBLE);

        imMerge = (ImageView) findViewById(R.id.imMerge);
        imMerge.setBackgroundResource(R.drawable.anim_merge);
        // animatedGif = (android.graphics.drawable.AnimatedImageDrawable)imMerge.getBackground();
        // animatedGif.start();
        frameAnimation = (AnimationDrawable)imMerge.getBackground();
        frameAnimation.start();

        // bStart = (Button) findViewById(R.id.bStart);
        // bStop = (Button) findViewById(R.id.bStop);
        // bHelp = (Button) findViewById(R.id.bHelp);
        b20a =  (Button) findViewById(R.id.b20a);
        b20b =  (Button) findViewById(R.id.b20b);
        bChange = (Button) findViewById(R.id.bChange);
        bContinue = (Button) findViewById(R.id.bContinue);

        tMes = (TextView) findViewById(R.id.tMes);
        tMes.setVisibility(View.INVISIBLE);
        tPoints = (TextView) findViewById(R.id.tPoints);
        tRest = (TextView) findViewById(R.id.tRest);
        tDbg = (TextView) findViewById(R.id.tDbg);
        tDbg.setText(layoutMes);

        tMes.setVisibility(View.INVISIBLE);

        // bStop.setEnabled(false);
        bChange.setEnabled(false);
        bContinue.setEnabled(false);

        // bStart.setEnabled(true);
        // bHelp.setEnabled(true);

        addListenerOnClickables();

        initURLBase();
        resetButtons(0);
    }

    /**
     * Override onConfigurationChanged
     * @param newConfig
     */
    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);

        if (newConfig.orientation == Configuration.ORIENTATION_PORTRAIT)
        {
            setContentView(R.layout.activity_main);
            tDbg.setText(getString(R.string.portrait_mode));
        }
        if (newConfig.orientation == Configuration.ORIENTATION_LANDSCAPE)
        {
            setContentView(R.layout.activity_main_vertical);
            tDbg.setText(getString(R.string.landscape_mode));
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        myMenu = menu;
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        if (id == R.id.action_start) {
            if (aGame == null || !aGame.isGame)
                startGame();
            return true;
        }
        if (id == R.id.action_stop) {
            if (aGame != null && aGame.isGame)
                stopGame(2);
            return true;
        }
        if (id == R.id.action_help) {
            helpText();
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    /**
     * implements Runnable
     */
    public void run() {

    }

    /**
     * reset Buttons
     * @param level
     */
    void resetButtons(int level) {
        if (level >= 0 ) {
            b20a.setText(R.string.b20a_text);
            // b20a.setText("20 Ansagen");
            b20a.setEnabled(false);
            b20b.setText(R.string.b20b_text);
            // b20b.setText("40 Ansagen");
            b20b.setEnabled(false);
            bChange.setEnabled(false);
        }
        if (level >= 1) {
            if (aGame != null)
                aGame.shouldContinue = false;
            bContinue.setEnabled(false);

            if (imTalon.getVisibility() != View.VISIBLE)
                imTalon.setVisibility(View.VISIBLE);

            try {
                imTalon.setImageResource(R.drawable.t);
                imAtou.setImageResource(R.drawable.n0);
            } catch (Exception ex) {
                this.errHandler(ex);
            }
        }
        if (level >= 2) {
            try {
                imOut0.setImageResource(R.drawable.leer);
                imOut1.setImageResource(R.drawable.leer);
            } catch (Exception ex) {
                this.errHandler(ex);
            }
        }
    }


    /**
     * init all ImageView's with default empty values
     */
    public  void initURLBase() {
        im0.setImageResource(R.drawable.n0);
        im1.setImageResource(R.drawable.n0);
        im2.setImageResource(R.drawable.n0);
        im3.setImageResource(R.drawable.n0);
        im4.setImageResource(R.drawable.n0);
        imAtou.setImageResource(R.drawable.n0);
        imTalon.setImageResource(R.drawable.t);
        imOut0.setImageResource(R.drawable.leer);
        imOut1.setImageResource(R.drawable.leer);
        imTalon.setVisibility(View.INVISIBLE);
        imAtou.setVisibility(View.INVISIBLE);
    }

    /**
     * showTalonCard
     */
    void showTalonCard() {
        try {
            imTalon.setImageResource (R.drawable.t);
        } catch (Exception imTalonEx) {
            System.err.println(imTalonEx.toString());
            imTalonEx.printStackTrace();
        }
        imTalon.setVisibility(View.VISIBLE);
    }

    /**
     * showAtouCard
     */
    void showAtouCard() {
        try {
            imAtou.setImageResource(aGame.set[19].getResourcesInt());
        } catch (Exception exp) {
            this.errHandler(exp);
        }

    }

    /**
     * showPlayersCards
     */
    void showPlayersCards() {

        try {
            // String myStr = String.valueOf(aGame.gambler.hand[0].getResourcesInt()) + " ; " +
            //         String.valueOf(aGame.gambler.hand[1].getResourcesInt()) + " ; "+
            //         String.valueOf(aGame.gambler.hand[2].getResourcesInt()) + " ; ";
            // tDbg.setText(myStr);

            im0.setImageResource(aGame.gambler.hand[0].getResourcesInt());
            im1.setImageResource(aGame.gambler.hand[1].getResourcesInt());
            im2.setImageResource(aGame.gambler.hand[2].getResourcesInt());
            im3.setImageResource(aGame.gambler.hand[3].getResourcesInt());
            im4.setImageResource(aGame.gambler.hand[4].getResourcesInt());

        } catch (Exception exp) {
            this.errHandler(exp);
        }
    }

    /**
     * start game
     */
    void startGame() {

        if (myMenu != null) {
            myMenu.findItem(R.id.action_start).setEnabled(false);
        }
        aGame = null;

        // runtime = java.lang.Runtime.getRuntime();
        // runtime.runFinalization();
        // runtime.gc();


        aGame = new Game(getApplicationContext());
        tMes.setVisibility(View.INVISIBLE);

        // animatedGif.stop();
        frameAnimation.stop();
        imMerge.setVisibility(View.INVISIBLE);

        try {
            im0.setImageResource(aGame.gambler.hand[0].getResourcesInt());
            Thread.sleep(100);
            im1.setImageResource(aGame.gambler.hand[1].getResourcesInt());
            Thread.sleep(100);
            im2.setImageResource(aGame.gambler.hand[2].getResourcesInt());
            Thread.sleep(200);
            imAtou.setVisibility(View.VISIBLE);
            Thread.sleep(100);
            im3.setImageResource(aGame.gambler.hand[3].getResourcesInt());
            Thread.sleep(100);
            im4.setImageResource(aGame.gambler.hand[4].getResourcesInt());
        } catch (Exception ext) {
            this.errHandler(ext);
        }

        imAtou.setVisibility(View.VISIBLE);
        imTalon.setVisibility(View.VISIBLE);

        resetButtons(1);

        tDbg.setText("");
        tRest.setText(R.string.tPoints_text);

        emptyTmpCard = new Card(getApplicationContext()); // new Card(this, -1);
        tPoints.setText("" + String.valueOf(aGame.gambler.points));
        showAtouCard();
        showTalonCard();
        // bStop.setEnabled(true);
        if (myMenu != null) {
            myMenu.findItem(R.id.action_stop).setEnabled(true);
        }

        gameTurn(0);
    }

    /**
     * close that game
     */
    void closeGame() { //	Implementierung des Zudrehens
        if (aGame.isGame == false || aGame.gambler == null) {
            tMes.setVisibility(View.VISIBLE);
            tMes.setText(R.string.nogame_started);
            return;
        }
        tMes.setVisibility(View.VISIBLE);
        tMes.setText(R.string.player_closed_game);

        try {
            imTalon.setImageResource(R.drawable.leer);
            imTalon.setVisibility(View.INVISIBLE);
        } catch (Exception jbpvex) {
            this.errHandler(jbpvex);
        }

        try {
            imAtou.setImageResource(R.drawable.verdeckt);
        } catch (Exception jbpvex) {
            this.errHandler(jbpvex);
        }

        aGame.colorHitRule = true;
        aGame.isClosed = true;
        aGame.gambler.hasClosed = true;

        if (aGame.atouChanged == false) {
            aGame.atouChanged = true;
        }
        gameTurn(0);
    }

    /**
     * a turn in game
     * @param ixlevel level
     */
    void gameTurn(int ixlevel) {
        if (ixlevel < 1) {
            try {
                imOut0.setImageResource(R.drawable.e1);
                imOut1.setImageResource(R.drawable.e1);
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
            showPlayersCards();
            pSaid = false;
            aGame.said = 'n';
            aGame.csaid = 'n';
        }

        if (aGame.playersTurn) {
            // Wann kann man austauschen ?
            if (ixlevel < 1)
                if ((aGame.atouIsChangable(aGame.gambler)) && (pSaid == false)) {
                    psaychange += 1;
                    bChange.setEnabled(true);
                }
            // Gibts was zum Ansagen ?
            int a20 = aGame.gambler.has20();

            if (a20 > 0) {
                psaychange += 2;

                b20a.setText(aGame.printColor(aGame.gambler.handpairs[0]) + " " + getString(R.string.say_pair));
                b20a.setEnabled(true);

                if (a20 > 1) {
                    b20b.setText(aGame.printColor(aGame.gambler.handpairs[1]) + " " + getString(R.string.say_pair));
                    b20b.setEnabled(true);
                } else {
                    b20b.setText(R.string.no_second_pair);
                }
            }
            // Info
            tMes.setVisibility(View.VISIBLE);
            tMes.setText(R.string.toplayout_clickon_card);
        } else {
            // COMPUTERS TURN IMPLEMENTIEREN
            String outPutMessage = "";
            // boolean atouNowChanged = aGame.atouChanged;
            // if (aGame.atouIsChangable(aGame.computer))  aGame.changeAtou(aGame.computer); } /* old implementaton */
            ccard = aGame.computerStarts();

            if ((aGame.computer.playerOptions & PLAYEROPTIONS.CHANGEATOU.getValue()) == PLAYEROPTIONS.CHANGEATOU.getValue()) {
                this.showAtouCard();
                outPutMessage += getString(R.string.computer_changes_atou);
            }
            // if (atouNowChanged == false && aGame.atouChanged) { }

            if ((aGame.computer.playerOptions & PLAYEROPTIONS.SAYPAIR.getValue()) == PLAYEROPTIONS.SAYPAIR.getValue()) {
                outPutMessage = outPutMessage + getString(R.string.computer_says_pair, aGame.printColor(aGame.csaid));
            }

            tMes.setVisibility(View.VISIBLE);
            tMes.setText(outPutMessage);

            if ((aGame.computer.playerOptions & PLAYEROPTIONS.ANDENOUGH.getValue()) == PLAYEROPTIONS.ANDENOUGH.getValue()) {
                // if (aGame.computer.points > 65)
                twentyEnough(false);
                ready = false;

                return;
            }

            try {
                imOut1.setImageResource(aGame.computer.hand[ccard].getResourcesInt());
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
            // tMes.setVisibility(View.VISIBLE);
            // tMes.setText("Zum Antworten einfach auf die entsprechende Karte klicken");
        }

        ready = true;
        printMes();
    }

    /**
     * Continue turn
     */
    void continueTurn() {
        try {
            ready = true;

            if (aGame != null)
                aGame.shouldContinue = false;
            bContinue.setEnabled(false);

            tMes.setVisibility(View.INVISIBLE);
            gameTurn(0);
        } catch (Exception e) {
            this.errHandler(e);
        }
    }


    /**
     * say 20 or 40 and enough to finish game
     * @param who player or computer
     */
    void twentyEnough(boolean who) {
        int xj = 0;
        String andEnough = getString(R.string.twenty_and_enough);
        ready = false;

        if (who) {
            if (aGame.said == aGame.atouInGame) {
                andEnough = getString(R.string.fourty_and_enough);
            }
            try {
                for (xj = 0; xj < 5; xj++) {
                    if (aGame.gambler.hand[xj].color == aGame.said &&
                            aGame.gambler.hand[xj].value == 3)
                        imOut0.setImageResource(aGame.gambler.hand[xj].getResourcesInt());
                    if (aGame.gambler.hand[xj].color == aGame.said &&
                            aGame.gambler.hand[xj].value == 4)
                        imOut1.setImageResource(aGame.gambler.hand[xj].getResourcesInt());
                }
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }

            tsEnds(new String(andEnough + " " + getString(R.string.you_have_won_points, String.valueOf(aGame.gambler.points))), 1);

        } else {
            if (aGame.csaid == aGame.atouInGame) {
                andEnough = getString(R.string.fourty_and_enough);
            }
            try {
                for (xj = 0; xj < 5; xj++) {
                    if (aGame.computer.hand[xj].color == aGame.csaid &&
                            aGame.computer.hand[xj].value == 3)
                        imOut0.setImageResource(aGame.computer.hand[xj].getResourcesInt());
                    if (aGame.computer.hand[xj].color == aGame.csaid &&
                            aGame.computer.hand[xj].value == 4)
                        imOut1.setImageResource(aGame.computer.hand[xj].getResourcesInt());
                }
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }

            printMes();
            String tsEndMes1 = new String(andEnough + " " + getString(R.string.computer_has_won_points, String.valueOf(aGame.computer.points)));
            tsEnds(tsEndMes1, 1);
            // tsEnds(new String(andEnough + " Computer hat gewonnen mit " + String.valueOf(aGame.computer.points) + " Punkten !"), 1);
        }
        return;
    }

    /**
     * end current turn in game
     */
    void endTurn() {
        int tmppoints;
        /* IMPLEMENT COMPUTERS STRATEGIE HERE */
        if (aGame.playersTurn) {
            ccard = aGame.computersAnswer();
            try {
                imOut1.setImageResource(aGame.computer.hand[ccard].getResourcesInt());
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
        } else {
        }

        tmppoints = aGame.checkPoints(ccard);
        aGame.computer.hand[ccard] = emptyTmpCard;
        tPoints.setText("" + String.valueOf(aGame.gambler.points));

        if (tmppoints > 0) {
            tMes.setVisibility(View.VISIBLE);
            tMes.setText(getString(R.string.your_hit_points, String.valueOf(tmppoints)) + " " + getString(R.string.click_continue));
            if (aGame.isClosed && (aGame.computer.hasClosed)) {
                tsEnds(getString(R.string.computer_closing_failed), 1);
                return ;
            }
        } else {
            tMes.setVisibility(View.VISIBLE);
            tMes.setText(getString(R.string.computer_hit_points, String.valueOf(-tmppoints)) + " " + getString(R.string.click_continue));
            if ((aGame.isClosed) && (aGame.gambler.hasClosed)) {
                tsEnds(getString(R.string.closing_failed), 1);
                return ;
            }
        }

        // Assign new cards
        if (aGame.assignNewCard() == 1) {
            /* NOW WE HAVE NO MORE TALON */
            try {
                imTalon.setImageResource(R.drawable.e1);
                imTalon.setVisibility(View.INVISIBLE);
                imAtou.setImageResource(R.drawable.e1);
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
            tMes.setVisibility(View.VISIBLE);
            tMes.setText(getString(R.string.color_hit_force_mode));
        }
        tRest.setText(""+(19-aGame.index));
        printMes();
        // resetButtons(0);
        pSaid = false;
        aGame.said = 'n';
        aGame.csaid = 'n';

        if (aGame.playersTurn) {
            if (aGame.gambler.points > 65) {
                tsEnds(getString(R.string.you_have_won_points, String.valueOf(aGame.gambler.points)), 1);
                return;
            }
        } else {
            if (aGame.computer.points > 65) {
                tsEnds(getString(R.string.computer_has_won_points, String.valueOf(aGame.computer.points)), 1);
                return;
            }
        }

        if (aGame.movs >= 5) {
            if (aGame.isClosed) {
                if (aGame.gambler.hasClosed) {
                    tsEnds(getString(R.string.closing_failed), 1);
                }
                try {
                    if (aGame.computer.hasClosed) {
                        tsEnds(getString(R.string.computer_closing_failed), 1);
                    }
                } catch (Exception jbpvex) {
                    this.errHandler(jbpvex);
                }
                return ;
            } else {
                if (tmppoints > 0) {
                    tsEnds(getString(R.string.last_hit_you_have_won), 1);
                } else {
                    tsEnds(getString(R.string.computer_wins_last_hit), 1);
                }
                return;
            }
        }

        if (aGame != null)
            aGame.shouldContinue = true;
        bContinue.setEnabled(true);

        ready = false;
    }

    /**
     * stop current game
     * @param levela
     */
    void stopGame(int levela) {
        // bStop.setEnabled(false);
        if (myMenu != null) {
            myMenu.findItem(R.id.action_stop).setEnabled(false);
        }
        aGame.stopGame();

        resetButtons(levela);
        // bStart.setEnabled(true);
        if (myMenu != null) {
            myMenu.findItem(R.id.action_start).setEnabled(true);
        }

        showPlayersCards();
        aGame.destroyGame();

        // imMerge.setVisibility(View.VISIBLE);
        // frameAnimation.start();
        imTalon.setVisibility(View.INVISIBLE);
        imAtou.setVisibility(View.INVISIBLE);
        // java.lang.System.runFinalization();
        // java.lang.System.gc();
        // await Task.Delay(3000);
    }

    /**
     * tsEnds method for ending the current game
     * @param endMessage ending game message
     * @param ix level
     */
    void tsEnds(String endMessage, int ix) {
        tMes.setText(endMessage);
        tMes.setVisibility(View.VISIBLE);
        stopGame(ix);
        return ;
    }

    /**
     * add listeners on all clickables
     */
    public void addListenerOnClickables() {

        // imageView1.setOnClickListener() { }
        /*
        bStart = (Button) findViewById(R.id.bStart);
        bStart.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                bStart_Clicked(arg0);
            }
        });
        */
        /*
        bStop = (Button) findViewById(R.id.bStop);
        bStop.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                bStop_Clicked(arg0);
            }
        });
        */
        bChange = (Button) findViewById(R.id.bChange);
        bChange.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                bChange_Clicked(arg0);
            }
        });
        b20a = (Button) findViewById(R.id.b20a);
        b20a.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                b20a_Clicked(arg0);
            }
        });

        b20b = (Button) findViewById(R.id.b20b);
        b20b.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                b20b_Clicked(arg0);
            }
        });
        bContinue = (Button) findViewById(R.id.bContinue);
        bContinue.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                bContinue_Clicked(arg0);
            }
        });
        /*
        bHelp = (Button) findViewById(R.id.bHelp);
        bHelp.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                bHelp_Clicked(arg0);
            }
        });
        */

        im0 = (ImageView) findViewById(R.id.im0);
        im0.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                imageView_ClickEventHandler(arg0, 0);
            }
        });
        im1 = (ImageView) findViewById(R.id.im1);
        im1.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                imageView_ClickEventHandler(arg0, 1);
            }
        });
        im2 = (ImageView) findViewById(R.id.im2);
        im2.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                imageView_ClickEventHandler(arg0, 2);
            }
        });
        im3 = (ImageView) findViewById(R.id.im3);
        im3.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                imageView_ClickEventHandler(arg0, 3);
            }
        });
        im4 = (ImageView) findViewById(R.id.im4);
        im4.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                imageView_ClickEventHandler(arg0, 4);
            }
        });
        imAtou = (ImageView) findViewById(R.id.imAtou);
        imAtou.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                imageView_ClickEventHandler(arg0, 10);
            }
        });
    }

    /**
     * bStart_Clicked
     * @param arg0
     */
    public void bStart_Clicked(View arg0) {
        startGame();
    }

    /**
     * bStop_Clicked
     * @param arg0
     */
    public void bStop_Clicked(View arg0) {
        try {
            stopGame(2);
        } catch (Exception e) {
            this.errHandler(e);
        }
    }

    /**
     * b20a_Clicked
     * @param arg0
     */
    public void bChange_Clicked(View arg0) {
        try {
            aGame.changeAtou(aGame.gambler);
            bChange.setEnabled(false);
            showAtouCard();
            showPlayersCards();
            gameTurn(1);
        } catch (Exception e) {
            this.errHandler(e);
        }
    }

    /**
     * b20a_Clicked
     * @param arg0
     */
    public void b20a_Clicked(View arg0) {
        try {
            if ((pSaid) || (aGame.gambler.handpairs[0] == 'n')) {
                return;
            }
            if (aGame.gambler.handpairs[0] == aGame.atouInGame) {
                aGame.gambler.points += 40;
            } else {
                aGame.gambler.points += 20;
            }
            pSaid = true;
            resetButtons(0);
            aGame.said = aGame.gambler.handpairs[0];
            aGame.mqueue.insert(getString(R.string.you_say_pair,  aGame.printColor(aGame.said)));
            printMes();
            tPoints.setText("" + String.valueOf(aGame.gambler.points));
            if (aGame.gambler.points > 65) {
                twentyEnough(true);
            }
        } catch (Exception e) {
            this.errHandler(e);
        }
    }

    /**
     * b20b_Clicked
     * @param arg0
     */
    public void b20b_Clicked(View arg0) {
        try {
            if ((pSaid) || (aGame.gambler.handpairs[1]=='n')) {
                return;
            }
            if (aGame.gambler.handpairs[1] == aGame.atouInGame) {
                aGame.gambler.points += 40;
            }
            else {
                aGame.gambler.points += 20;
            }
            pSaid = true;
            resetButtons(0);
            aGame.said = aGame.gambler.handpairs[1];
            aGame.mqueue.insert(getString(R.string.you_say_pair,  aGame.printColor(aGame.said)));
            printMes();
            tPoints.setText("" + String.valueOf(aGame.gambler.points));
            if (aGame.gambler.points > 65) {
                twentyEnough(true);
            }
        } catch (Exception e) {
            this.errHandler(e);
        }
    }

    /**
     * bContinue_Clicked
     * @param arg0
     */
    public void bContinue_Clicked(View arg0) {
        continueTurn();
    }


    /**
     * EventHandler for all ImageViews
     * @param arg0 View, that fired click
     * @param ic image counter, that represents which ImageView is clicked
     */
    void imageView_ClickEventHandler(View arg0, int ic) {

        int j;
        // String c_array = "Player Array: ";
        try {
            if (!ready) {
                return;
            }

            if (ic == 10) {
                if (aGame.playersTurn && (!aGame.isClosed) &&  (!pSaid) && (aGame.index < 16)) {
                    closeGame();
                }
                return;
            }
            if (!aGame.gambler.hand[ic].isValidCard()) {
                aGame.mqueue.insert(getString(R.string.this_is_no_valid_card));
                printMes();
                return;
            }
            if (pSaid) {
                if ((aGame.said == aGame.gambler.hand[ic].getColor()) &&
                        (aGame.gambler.hand[ic].getValue() > 2) &&
                        (aGame.gambler.hand[ic].getValue() < 5)) {
                    ; // we can continue
                } else {
                    aGame.mqueue.insert(getString(R.string.you_must_play_pair_card));
                    printMes();
                    return ;
                }
            }
            if (aGame.colorHitRule && (!aGame.playersTurn)) {
                // CORRECT WAY ?
                if ((!aGame.gambler.isInColorHitsContextValid(ic,aGame.computer.hand[ccard]))) {

                    aGame.mqueue.insert(getString(R.string.you_must_play_color_hit_force_rules));
                    int tmpint = aGame.gambler.bestInColorHitsContext(aGame.computer.hand[ccard]);
                    // for (j = 0; j < 5; j++) {
                    //     c_array = c_array + aGame.gambler.colorHitArray[j] + " ";
                    // }
                    // aGame.mqueue.insert(c_array);

                    aGame.mqueue.insert(getString(R.string.best_card_would_be, aGame.gambler.hand[tmpint].getName()));
                    printMes();
                    return ;
                }
            }
            if (psaychange > 0) {
                resetButtons(0);
                psaychange = 0;
            }
            aGame.playedOut = aGame.gambler.hand[ic];
            // Besser Cards als Array
            switch (ic) {
                case 0:
                    im0.setImageResource(R.drawable.e);
                    break;
                case 1:
                    im1.setImageResource(R.drawable.e);
                    break;
                case 2:
                    im2.setImageResource(R.drawable.e);
                    break;
                case 3:
                    im3.setImageResource(R.drawable.e);
                    break;
                case 4:
                    im4.setImageResource(R.drawable.e);
                    break;
                default: tDbg.append("Assertion !");
            }

            imOut0.setImageResource(aGame.gambler.hand[ic].getResourcesInt());

        } catch (Exception e) {
            this.errHandler(e);
        }
        aGame.gambler.hand[ic] = emptyTmpCard;
        ready = false;
        endTurn();

    }


    /**
     * print message queue
     */
    void printMes() {
        tDbg.append(aGame.mqueue.fetch());
    }

    /**
     * Error handler
     * @param myErr java.lang.Throwable
     */
    void errHandler(java.lang.Throwable myErr) {
        tDbg.append("\nCRITICAL ERROR #" + String.valueOf((++errNum))  + " " + myErr.getMessage());
        tDbg.append(myErr.toString());
        tDbg.append("\nMessage: "+ myErr.getLocalizedMessage() + "\n");
        myErr.printStackTrace();
    }

    /**
     * Help button clicked
     * @param arg0
     */
    public void bHelp_Clicked(View arg0) {
        helpText();
    }

    /**
     * helpText() prints out help text
     */
    public void helpText() {
        tDbg.setText(R.string.help_text);
        // tDbg.setText("Schnapslet V 0.2 - Kartenspiel Schnapsen als android app von Heinrich Elsigan (heinrich.elsigan@area23.at)\n");
        // tDbg.append("Das Spiel ist so angelegt, dass man gegen den Computer spielt. Ist man am Zug, so kann man eine Karte ausspielen, indem man auf das Kartensymbol klickt. ");
        // tDbg.append("Andere Optionen, wie \"Atou austauschen\" oder \"Ein Paar Ansagen\" sind über die Buttons links oben moeglich; diese Optionen muessen gewaehlt werden, bevor man eine Karte auspielt! ");
        // tDbg.append("Ist der Computer am Zug, so spielt dieser eine Karte aus und man selbst kann dann durch Klick auf die eigenen Karten, stechen oder draufgeben! ");
        // tDbg.append("Die Regeln entsprechen dem oesterreichischen Schnapsen, Zudrehen ist implementiert. Man muss einfach auf die Atou Karte klicken.\n");

        try {
             Thread.currentThread().sleep(10);
        } catch (Exception exInt) {
            errHandler(exInt);
        }
        /*
        try {
            TranslateOptions options = TranslateOptions.newBuilder()
                    .setApiKey(API_KEY)
                    .build();
            Translate translate = options.getService();
            final Translation translation =
                    translate.translate("Hello World",
                            Translate.TranslateOption.targetLanguage("de"));
            String translated = translation.getTranslatedText();
            tDbg.append(translated + "\n");
        } catch (Exception exTranslation) {
            this.errHandler(exTranslation);
        }
        /* */

    }
}