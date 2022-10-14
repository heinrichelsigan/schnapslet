/*
*
* @author           Heinrich Elsigan
* @version          V 1.3.4
* @since            JDK 1.2.1
*

*/
/*
   Copyright (C) 2000 - 2018 Heinrich Elsigan

   Schnapslet java applet is free software; you can redistribute it and/or
   modify it under the terms of the GNU Library General Public License as
   published by the Free Software Foundation; either version 2 of the
   License, or (at your option) any later version.
   See the GNU Library General Public License for more details.

*/
package at.area23.schnapslet;

// import android.app.Activity;
import android.content.ClipData;
import android.content.ContentResolver;
import android.content.Context;
import android.content.Intent;
import android.content.res.Configuration;
import android.content.res.Resources;
import android.graphics.drawable.AnimationDrawable;
import android.graphics.drawable.Drawable;
import android.graphics.drawable.AnimatedImageDrawable;
import android.graphics.ImageDecoder;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
// import android.support.annotation.DrawableRes;
// import android.support.v4.content.ContextCompat;
// import android.support.v4.content.res.ResourcesCompat;
// import android.support.v13.view.DragAndDropPermissionsCompat;
// import android.support.v13.view.DragStartHelper;
// import android.support.v7.app.AppCompatActivity;
import android.view.DragEvent;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.DragShadowBuilder;
import android.view.View.OnDragListener;
import android.view.View.OnTouchListener;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.res.ResourcesCompat;
import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.constraintlayout.widget.*;
import androidx.fragment.app.DialogFragment;

import java.util.HashMap;
import java.util.Locale;

/**
 * MainActivity class implements the MainActivity.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class MainActivity extends AppCompatActivity implements Runnable {

    private static final String API_KEY = "-----BEGIN PRIVATE KEY-----\nMIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQDDjZ+QmX6Zi514\nsFbIgT48HFuvXgWnmNbY7aBPW5gWq2kmISwxQcUG/JxdD2VasHiG66QAVgNHjQ8D\nRLyzPSmNUb4QVBMB4WHukqpBW97qG3Uhp4HnHYJ3Tg5XbHmjhFevxISG0ZLEni4C\nJMcNMTug6+VGDeNE/yISN42uhdiPsgTPIaGK/6FeG8KXLB9R501dYhiWprOuwhw5\nTXvAAaLyP+y/3N1/Q/4Po+WSusYqTUl1kNZ6/BvynmK4Bz+Ibakd59eBIn4xMOyK\nOxQuyC5GJbhRYjbcoEvbTzZy7CUk0nzrLunxzIucAr1SuOwJwIDz2yMM5wl/5nXY\nCm2RjzdnAgMBAAECggEAFWc50LTMI3gheyUpynZC3odoDZCn48kZstKHWkg3JDwM\nnSzCTn3ZV8NsRc86k6t+9Z1y7Mp9P2aT/xKV6LRICPyqZdUd43XMpzUMR20Lv+nT\nbySLVkVnkzFK5oyr35bLliRXMP5dJwH9HSTzWGFMGnfXN0yr1FBsZTwJWNGzez6a\nxX3tPFQXd4xwoZev+ZiEuaVgRGl6y1Va83QMw7rKOYA74NSBgMhZyhna+5O1fB3r\nH7mRsaCf+BI9HGYeu+mw9biJRBIHHqBcteT0I8wgXoxMews40elY5UrXYpHyfoV1\nSlYwLRcSaE4ugFO7zJIZGYrxE1Q6we6o6XuHsYCjyQKBgQDj/hOOJ89crQudFzm/\n1t8QHLWntQJzIU9NnazyXXT+coO3AX6qMDCwWy2o4gpku8gP4qqLErRLtCG+3f0T\nC6QHarLDhaONKIweArjJ7la9MsOqpeG9lZdOuzVxUWJCqTb75ykJBi/ickhDketb\nHJiGGTndU6YRIqc4atd4CKiO2wKBgQDbk2T9Nxm4TWvu5NRNYD9eMCVS8hFY5j0D\nU/Z4DDuO0ztktWVu+KQTMaMhn0iX+KjeuKt/ytfex8/uvbGx7cz9sUxP9GIZBKpB\nVTwNVr1Pt76YT5y+ngESlmueCVRQCFUYc//LCGeJh1s6PlmSM0ocV+8WvyrW9AUS\nYUx4g4ABZQKBgD/xyfBL8BfRHPnBQtwwWr29H6Ha3cYGqKRfPdt4JNEcsx6H18vJ\n2k4MNKEyTLH2DOWPsD9zTogRDIno3wsRb774yQyXlciIf8wG/Wb9ZuyHqWNaRRcU\nNqzJSvLuXX3O0fIS4mp6hsGfRe9VpMoYGhs6RgVyaZhSvM3RAX/UBdqTAoGAIC5A\n/c+GiHloWTHWX6S8hMxfnAF4Q2QzCvrSQ5PfYrZYnRDs1c/BFEMRGotis0sxTLsZ\n/3e2HaOBOQc6NM6aXZAPlCRIAEyruzmHvJi61CUk3OPGIDW+CIBdM2NApR4jgpr1\noUcRDZn159pdfEziDrdghh/sYmaPG7uA3qS/LPUCgYADPOzUYG45IPRb42R4qk0E\n5C83ekg5wz9PUsd6aZgRIvHZB3HgZ2p7bnHvMB0DBF+F4WPNB8zsY39lels/lC80\npDcK7XJtcm6ucbWJt0d8eyrxjlwGAzfcvOpubC/McVtW6Atj5+FVTi7dBvhqUSac\nzEXeRxpEeNilJzgNENDtAQ==\n-----END PRIVATE KEY-----\n";
    Button b20a, b20b,  bChange, bContinue; // bStart, bStop, bHelp;
    ImageView im0,im1,im2, im3, im4,
            imageCOut0, imageCOut1, imOut0, imOut1, imAtou, imTalon, imMerge;
    TextView tRest, tPoints, tMes, tDbg;
    Menu myMenu;

    AnimationDrawable frameAnimation;
    AnimatedImageDrawable animatedGif;

    LinearLayout playerCard0, playerCard1, playerCard2, playerCard3, playerCard4,
        linearLayoutCCard0, linearLayoutCCard1, playedCard0, playedCard1, atouCard, talonCard;

    long errNum = 0; // Errors Ticker
    int ccard; // Computers Card played
    volatile Card touchedCard;
    volatile Card draggedCard;
    final Card assignedCard = null;
    volatile boolean ready = false, droppedCard = false, dragged20 = false;
    volatile byte psaychange = 0;
    boolean pSaid = false; // Said something
    static java.lang.Runtime runtime = null;
    HashMap<Integer, Drawable> dragNDropMap = new HashMap<>();
    HashMap<Integer, LinearLayout> toFields = new HashMap<>();
    Game aGame;
    // Calling Application class (see application tag in AndroidManifest.xml)
    GlobalAppSettings globalVariable;

    private final static Handler resetBackgroundHandler = new Handler(Looper.getMainLooper());

    /**
     * runResetBackground new Runnable() -> { resetBackground(); }
     */
    private final Runnable runResetBackground = new Runnable() {
        @Override
        // @SuppressLint("InlinedApi")
        public void run() {
            mergeCardAnim(true);
        }
    };

    /**
     * Override onCreate
     * @param savedInstanceState saved instance state
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Bundle bundle = getIntent().getExtras();
        if (bundle != null) {
        }

        setContentView(R.layout.activity_main);

        String layoutMes;
        if (getResources().getDisplayMetrics().widthPixels > getResources().getDisplayMetrics().heightPixels) {
            layoutMes = getString(R.string.landscape_mode);
            setContentView(R.layout.activity_main_vertical);
        } else {
            layoutMes = getString(R.string.portrait_mode);
            setContentView(R.layout.activity_main);
        }

        globalVariable = (GlobalAppSettings) getApplicationContext();

        linearLayoutCCard0 = (LinearLayout) findViewById(R.id.linearLayoutCCard0);
        linearLayoutCCard1 = (LinearLayout) findViewById(R.id.linearLayoutCCard1);
        playedCard0 = (LinearLayout) findViewById(R.id.playedCard0);
        playedCard1 = (LinearLayout) findViewById(R.id.playedCard1);
        atouCard = (LinearLayout) findViewById(R.id.atouCard);
        talonCard = (LinearLayout) findViewById(R.id.talonCard);
        playerCard0 = (LinearLayout) findViewById(R.id.playerCard0);
        playerCard1 = (LinearLayout) findViewById(R.id.playerCard1);
        playerCard2 = (LinearLayout) findViewById(R.id.playerCard2);
        playerCard3 = (LinearLayout) findViewById(R.id.playerCard3);
        playerCard4 = (LinearLayout) findViewById(R.id.playerCard4);

        im0 = (ImageView) findViewById(R.id.im0);
        im1 = (ImageView) findViewById(R.id.im1);
        im2 = (ImageView) findViewById(R.id.im2);
        im3 = (ImageView) findViewById(R.id.im3);
        im4 = (ImageView) findViewById(R.id.im4);

        imageCOut0 = (ImageView) findViewById(R.id.imageCOut0);
        imageCOut1 = (ImageView) findViewById(R.id.imageCOut1);
        imOut0 = (ImageView) findViewById(R.id.imOut0);
        imOut1 = (ImageView) findViewById(R.id.imOut1);
        imTalon = (ImageView) findViewById(R.id.imTalon);
        imAtou = (ImageView) findViewById(R.id.imAtou);
        imTalon.setVisibility(View.INVISIBLE);
        imAtou.setVisibility(View.INVISIBLE);

        imMerge = (ImageView) findViewById(R.id.imMerge);
        imMerge.setBackgroundResource(R.drawable.anim_merge);

        // bStart = (Button) findViewById(R.id.bStart); bStop = (Button) findViewById(R.id.bStop); bHelp = (Button) findViewById(R.id.bHelp);
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
        // bStop.setEnabled(false); bContinue.setEnabled(false); bStart.setEnabled(true); bHelp.setEnabled(true);
        bChange.setEnabled(false);

        mergeCardAnim(true);

        frameAnimation = (AnimationDrawable)imMerge.getBackground();
        frameAnimation.start();

        addListenerOnClickables();
        // initURLBase();
        resetButtons(0);
    }

    /**
     * Override onConfigurationChanged
     * @param newConfig the new configuration
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
        menuResetCheckboxes();
        return true;
    }

    /**
     * Handles action bar item clicks in option menu.
     * @param item in options menu, that has been clicked
     */
    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
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
        // reset now all checkboxes for language menu items
        menuResetCheckboxes();

        if (id == R.id.action_default_cards) {
            //Sets application locale in GlobalAppSettings from default app locale
            Locale primaryLocale = getApplicationContext().getResources().getConfiguration().getLocales().get(0);
            return setLocale(primaryLocale, item);
        }
        if (id == R.id.action_english_cards) { // sets locale in GlobalAppSettings with english
            return setLanguage("en", item);
        }
        if (id == R.id.action_german_cards) { // changes locale in GlobalAppSettings to Deutsch
            return  setLanguage("de", item);
        }
        if (id == R.id.action_french_cards) { // Overwrites application locale in GlobalAppSettings with french
            return setLanguage("fr", item);
        }
        if (id == R.id.action_ukraine_cards) { //  uktainian
            return setLanguage("uk", item);
        }
        if (id == R.id.action_polish_cards) { // Overwrites application locale in GlobalAppSettings with french
            return setLanguage("pl", item);
        }

        return super.onOptionsItemSelected(item);
    }


    /**
     * setLanguage
     * @param language - the langzage
     * @return true in case of succcess, otherwise false
     */
    protected  boolean setLanguage(String language, MenuItem item) {
        return setLocale(new Locale(language), item);
    }

    /**
     * setLanguage
     * @param aLocale - a locale
     * @param item - a menu item
     * @return true in case of succcess, otherwise false
     */
    protected  boolean setLocale(Locale aLocale, MenuItem item) {
        if (item != null) {
            item.setChecked(true);
        }
        if (globalVariable.getLocale().getLanguage() != aLocale.getLanguage()) {
            //Overwrites application locale in GlobalAppSettings with english
            globalVariable.setLocale(aLocale);
            showAtouCard();
            showPlayersCards();
        }
        return true;
    }

    /**
     * reset menu checkboxes from all checkable menu items
     */
    protected void menuResetCheckboxes() {
        if (myMenu != null) {
            myMenu.findItem(R.id.action_default_cards).setChecked(false);
            myMenu.findItem(R.id.action_english_cards).setChecked(false);
            myMenu.findItem(R.id.action_german_cards).setChecked(false);
            myMenu.findItem(R.id.action_french_cards).setChecked(false);
            myMenu.findItem(R.id.action_ukraine_cards).setChecked(false);
            myMenu.findItem(R.id.action_polish_cards).setChecked(false);
        }
    }

    /**
     * implements Runnable
     */
    public void run() {

    }

    /**
     * reset Buttons
     * @param level of reset
     *  0 disable Mariage and Change Atou button
     *  1 perform level 0 and enable Continue, reset atou and talon card
     *  2 perform 0 + 1 and reset play out position cards
     */
    protected void resetButtons(int level) {

        if (level >= 0 ) {
            b20a.setText(R.string.b20a_text);
            b20a.setEnabled(false);
            b20b.setText(R.string.b20b_text);
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
                imOut0.setImageResource(R.drawable.e);
                imOut1.setImageResource(R.drawable.e);
            } catch (Exception ex) {
                this.errHandler(ex);
            }
        }
    }

    /**
     * init all ImageView's with default empty values
     */
    @Deprecated
    public void initURLBase() {
        im0.setImageResource(R.drawable.n0);
        im1.setImageResource(R.drawable.n0);
        im2.setImageResource(R.drawable.n0);
        im3.setImageResource(R.drawable.n0);
        im4.setImageResource(R.drawable.n0);
        imAtou.setImageResource(R.drawable.n0);
        imTalon.setImageResource(R.drawable.t);
        imOut0.setImageResource(R.drawable.e);
        imOut1.setImageResource(R.drawable.e);
        imTalon.setVisibility(View.INVISIBLE);
        imAtou.setVisibility(View.INVISIBLE);
    }

    /**
     * mergeCardAnim
     * @param startMergeAnim true to start merge anim, false to stop
     */
    protected void mergeCardAnim(boolean startMergeAnim) {
        if (startMergeAnim) {
            // animatedGif = null;
            imMerge.setVisibility(View.VISIBLE);
            try {
                ImageDecoder.Source source =
                        ImageDecoder.createSource(getResources(), R.drawable.anim_merge);
                Drawable animDraw = ImageDecoder.decodeDrawable(source);

                imMerge.setImageDrawable(animDraw);
                if (animDraw instanceof AnimatedImageDrawable) {
                    animatedGif = ((AnimatedImageDrawable) animDraw);
                }
            } catch (Exception exCraw) {
                this.errHandler(exCraw);
            }
            if (animatedGif != null)
                animatedGif.start();
        }
        if (!startMergeAnim) {
            if (animatedGif != null)
                animatedGif.stop();
            imMerge.setVisibility(View.INVISIBLE);
        }
    }


    /**
     * showTalonCard
     */
    protected void showTalonCard() {
        try {
            imTalon.setImageResource (R.drawable.t);
        } catch (Exception imTalonEx) {
            System.err.println(imTalonEx);
            imTalonEx.printStackTrace();
        }
        imTalon.setVisibility(View.VISIBLE);
    }

    /**
     * showAtouCard
     */
    protected void showAtouCard() {
        try {
            // imAtou.setImageResource(aGame.set[19].getResourcesInt());
            imAtou.setImageDrawable(aGame.set[19].getDrawable());
        } catch (Exception exp) {
            this.errHandler(exp);
        }

    }

    /**
     * showPlayersCards
     */
    protected void showPlayersCards() {
        try {
            Drawable normalShape = ResourcesCompat.getDrawable(getResources(), R.drawable.shape, null);

            Card handCard = (aGame.gambler != null && aGame.gambler.hand[0].isValidCard()) ?
                    aGame.gambler.hand[0] : aGame.emptyTmpCard;
            playerCard0.setVisibility(View.VISIBLE);
            playerCard0.setBackground(normalShape);

            im0.setBackground(normalShape);
            im0.setImageDrawable(handCard.getDrawable());
            im0.setVisibility(View.VISIBLE);

            handCard =  (aGame.gambler != null && aGame.gambler.hand[1].isValidCard()) ?
                    aGame.gambler.hand[1] : aGame.emptyTmpCard;
            playerCard1.setVisibility(View.VISIBLE);
            playerCard1.setBackground(normalShape);

            im1.setBackground(normalShape);
            im1.setImageDrawable(handCard.getDrawable());
            im1.setVisibility(View.VISIBLE);

            handCard = (aGame.gambler != null && aGame.gambler.hand[2].isValidCard()) ?
                aGame.gambler.hand[2] :  aGame.emptyTmpCard;
            playerCard2.setVisibility(View.VISIBLE);
            playerCard2.setBackground(normalShape);
            // im2.setImageResource(handCard.getResourcesInt());
            im2.setBackground(normalShape);
            im2.setImageDrawable(handCard.getDrawable());
            im2.setVisibility(View.VISIBLE);

            handCard = (aGame.gambler != null && aGame.gambler.hand[3].isValidCard()) ?
                    aGame.gambler.hand[3] : aGame.emptyTmpCard;
            playerCard3.setVisibility(View.VISIBLE);
            playerCard3.setBackground(normalShape);
            // im3.setImageResource(aGame.gambler.hand[3].getResourcesInt());
            // im3.setImageDrawable(aGame.gambler.hand[3].getDrawable());
            im3.setBackground(normalShape);
            im3.setImageDrawable(handCard.getDrawable());
            im3.setVisibility(View.VISIBLE);

            handCard = (aGame.gambler != null && aGame.gambler.hand[4].isValidCard()) ?
                 aGame.gambler.hand[4] : aGame.emptyTmpCard;
            playerCard4.setVisibility(View.VISIBLE);
            playerCard4.setBackground(normalShape);
            // im4.setImageResource(aGame.gambler.hand[4].getResourcesInt());
            // im4.setImageDrawable(aGame.gambler.hand[4].getDrawable());
            im4.setBackground(normalShape);
            im4.setImageDrawable(handCard.getDrawable());
            im4.setVisibility(View.VISIBLE);

        } catch (Exception exp) {
            this.errHandler(exp);
        }
    }

    /**
     * start game
     */
    protected void startGame() {

        if (myMenu != null) {
            myMenu.findItem(R.id.action_start).setEnabled(false);
        }
        aGame = null;

        // runtime = java.lang.Runtime.getRuntime();
        // runtime.runFinalization();
        // runtime.gc();

        aGame = new Game(getApplicationContext());
        tMes.setVisibility(View.INVISIBLE);

        frameAnimation.stop();
        mergeCardAnim(false);

        try {
            // im0.setImageResource(aGame.gambler.hand[0].getResourcesInt());
            im0.setImageDrawable(aGame.gambler.hand[0].getDrawable());
            // im1.setImageResource(aGame.gambler.hand[1].getResourcesInt());
            im1.setImageDrawable(aGame.gambler.hand[1].getDrawable());
            Thread.sleep(100);
            // im2.setImageResource(aGame.gambler.hand[2].getResourcesInt());
            im2.setImageDrawable(aGame.gambler.hand[2].getDrawable());
            imAtou.setVisibility(View.VISIBLE);
            Thread.sleep(100);
            // im3.setImageResource(aGame.gambler.hand[3].getResourcesInt());
            im3.setImageDrawable(aGame.gambler.hand[3].getDrawable());
            // im4.setImageResource(aGame.gambler.hand[4].getResourcesInt());
            im4.setImageDrawable(aGame.gambler.hand[4].getDrawable());
        } catch (Exception ext) {
            this.errHandler(ext);
        }

        atouCard.setVisibility(View.VISIBLE);
        imAtou.setVisibility(View.VISIBLE);
        imTalon.setVisibility(View.VISIBLE);

        resetButtons(1);

        tDbg.setText("");
        tRest.setText(R.string.tPoints_text);

        // emptyTmpCard = new Card(-2, getApplicationContext()); // new Card(this, -1);
        tPoints.setText(String.valueOf(aGame.gambler.points));
        showAtouCard();
        showTalonCard();

        if (myMenu != null) {
            myMenu.findItem(R.id.action_stop).setEnabled(true);
        }

        gameTurn(0);
    }

    /**
     * close that game
     */
    protected void closeGame() { //	Implementierung des Zudrehens
        if (!aGame.isGame || aGame.gambler == null) {
            tMes.setVisibility(View.VISIBLE);

            tMes.setText(R.string.nogame_started);
            return;
        }
        tMes.setVisibility(View.VISIBLE);
        setTextMessage(getString(R.string.player_closed_game));
        tMes.setText(R.string.player_closed_game);

        try {
            imTalon.setImageResource(R.drawable.t);
            imTalon.setVisibility(View.VISIBLE);
        } catch (Exception jbpvex) {
            this.errHandler(jbpvex);
        }

        try {
            imAtou.setImageResource(R.drawable.n0);
        } catch (Exception jbpvex) {
            this.errHandler(jbpvex);
        }

        aGame.colorHitRule = true;
        aGame.isClosed = true;
        aGame.gambler.hasClosed = true;

        if (!aGame.atouChanged) {
            aGame.atouChanged = true;
        }
        gameTurn(0);
    }

    /**
     * a turn in game
     * @param ixlevel level
     */
    protected void gameTurn(int ixlevel) {
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
                if ((aGame.atouIsChangable(aGame.gambler)) && (!pSaid)) {
                    psaychange += 1;
                    bChange.setEnabled(true);
                }
            // Gibts was zum Ansagen ?
            int a20 = aGame.gambler.has20();

            if (a20 > 0) {
                psaychange += 2;
                String sayText = aGame.printColor(aGame.gambler.handpairs[0]) + " " + getString(R.string.say_pair);
                b20a.setText(sayText);
                b20a.setEnabled(true);

                if (a20 > 1) {
                    sayText = aGame.printColor(aGame.gambler.handpairs[1]) + " " + getString(R.string.say_pair);
                    b20b.setText(sayText);
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
                setTextMessage(getString(R.string.computer_changes_atou));
                outPutMessage += getString(R.string.computer_changes_atou);
            }
            // if (atouNowChanged == false && aGame.atouChanged) { }

            if ((aGame.computer.playerOptions & PLAYEROPTIONS.SAYPAIR.getValue()) == PLAYEROPTIONS.SAYPAIR.getValue()) {
                setTextMessage(getString(R.string.computer_says_pair, aGame.printColor(aGame.csaid)));
                outPutMessage = outPutMessage + getString(R.string.computer_says_pair, aGame.printColor(aGame.csaid));
            }

            tMes.setVisibility(View.VISIBLE);
            tMes.setText(outPutMessage);

            if ((aGame.computer.playerOptions & PLAYEROPTIONS.ANDENOUGH.getValue()) == PLAYEROPTIONS.ANDENOUGH.getValue()) {
                twentyEnough(false);
                ready = false;
                return;
            }

            try {
                // imOut1.setImageResource(aGame.computer.hand[ccard].getResourcesInt());
                imOut1.setImageDrawable(aGame.computer.hand[ccard].getDrawable());
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
    protected void continueTurn() {
        try {
            ready = true;
            dragged20 = false;
            droppedCard = false;

            if (aGame == null || !aGame.isGame) {
                startGame();
                return;
            }

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
    protected void twentyEnough(boolean who) {
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
                        // imOut0.setImageResource(aGame.gambler.hand[xj].getResourcesInt());
                        imOut0.setImageDrawable(aGame.gambler.hand[xj].getDrawable());
                    if (aGame.gambler.hand[xj].color == aGame.said &&
                            aGame.gambler.hand[xj].value == 4)
                        // imOut1.setImageResource(aGame.gambler.hand[xj].getResourcesInt());
                        imOut1.setImageDrawable(aGame.gambler.hand[xj].getDrawable());
                }
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }

            tsEnds(andEnough + " " + getString(R.string.you_have_won_points, String.valueOf(aGame.gambler.points)), 1);

        } else {
            if (aGame.csaid == aGame.atouInGame) {
                andEnough = getString(R.string.fourty_and_enough);
            }
            try {
                for (xj = 0; xj < 5; xj++) {
                    if (aGame.computer.hand[xj].color == aGame.csaid &&
                            aGame.computer.hand[xj].value == 3)
                        // imOut0.setImageResource(aGame.computer.hand[xj].getResourcesInt());
                        imOut0.setImageDrawable(aGame.computer.hand[xj].getDrawable());
                    if (aGame.computer.hand[xj].color == aGame.csaid &&
                            aGame.computer.hand[xj].value == 4)
                        // imOut1.setImageResource(aGame.computer.hand[xj].getResourcesInt());
                        imOut1.setImageDrawable(aGame.computer.hand[xj].getDrawable());
                }
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }

            printMes();
            String tsEndMes1 = (andEnough + " " + getString(R.string.computer_has_won_points, String.valueOf(aGame.computer.points)));
            tsEnds(tsEndMes1, 1);
            // tsEnds(new String(andEnough + " Computer hat gewonnen mit " + String.valueOf(aGame.computer.points) + " Punkten !"), 1);
        }
        return;
    }

    /**
     * end current turn in game
     */
    protected void endTurn() {
        int tmppoints;
        String msgText;

        /* implement computers strategy here */
        if (aGame.playersTurn) {
            ccard = aGame.computersAnswer();
            try {
                // imOut1.setImageResource(aGame.computer.hand[ccard].getResourcesInt());
                imOut1.setImageDrawable(aGame.computer.hand[ccard].getDrawable());
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
        }

        tmppoints = aGame.checkPoints(ccard);
        aGame.computer.hand[ccard] = aGame.emptyTmpCard;
        tPoints.setText(String.valueOf(aGame.gambler.points));

        if (tmppoints > 0) {
            tMes.setVisibility(View.VISIBLE);
            msgText = getString(R.string.your_hit_points, String.valueOf(tmppoints)) + " " + getString(R.string.click_continue);
            setTextMessage(msgText);
            tMes.setText(msgText);
            if (aGame.isClosed && (aGame.computer.hasClosed)) {
                tsEnds(getString(R.string.computer_closing_failed), 1);
                return;
            }
        } else {
            tMes.setVisibility(View.VISIBLE);
            msgText = getString(R.string.computer_hit_points, String.valueOf(-tmppoints)) + " " + getString(R.string.click_continue);
            setTextMessage(msgText);
            tMes.setText(msgText);
            if ((aGame.isClosed) && (aGame.gambler.hasClosed)) {
                tsEnds(getString(R.string.closing_failed), 1);
                return;
            }
        }

        // Assign new cards
        if (aGame.assignNextCard(assignedCard)) {
            /* NOW WE HAVE NO MORE TALON */
            try {
                imTalon.setImageResource(R.drawable.e1);
                imTalon.setVisibility(View.INVISIBLE);
                imAtou.setImageResource(R.drawable.e1);
                atouCard.setVisibility(View.INVISIBLE);
                imAtou.setVisibility(View.INVISIBLE);
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
            tMes.setVisibility(View.VISIBLE);
            setTextMessage(getString(R.string.color_hit_force_mode));
            tMes.setText(getString(R.string.color_hit_force_mode));
        }
        tRest.setText(String.valueOf((19-aGame.index)));
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

        imTalon.setOnClickListener(this::bContinue_Clicked);

        ready = false;
    }

    /**
     * stop current game
     * @param levela level of stop
     */
    protected void stopGame(int levela) {
        if (myMenu != null) {
            myMenu.findItem(R.id.action_stop).setEnabled(false);
        }
        aGame.stopGame();

        resetButtons(levela);

        bContinue.setEnabled(true);
        if (myMenu != null) {
            myMenu.findItem(R.id.action_start).setEnabled(true);
        }

        showPlayersCards();
        aGame.destroyGame();

        imTalon.setImageResource(R.drawable.t);
        imTalon.setVisibility(View.VISIBLE);
        atouCard.setVisibility(View.VISIBLE);
        imAtou.setVisibility(View.VISIBLE);
        atouCard.setVisibility(View.VISIBLE);
        imAtou.setImageResource(R.drawable.n0);

        mergeCardAnim(true);
    }

    /**
     * tsEnds method for ending the current game
     * @param endMessage ending game message
     * @param ix level
     */
    private void tsEnds(String endMessage, int ix) {
        tMes.setText(endMessage);
        setTextMessage(endMessage);
        tMes.setVisibility(View.VISIBLE);
        stopGame(ix);
    }

    /**
     * add listeners on all clickables
     */
    public void addListenerOnClickables() {

        playedCard0 = (LinearLayout) findViewById(R.id.playedCard0);
        playedCard0.setOnDragListener((view, dragEvent) -> layoutView_OnDragHandler(view, dragEvent, -2));
        playedCard1 = (LinearLayout) findViewById(R.id.playedCard1);
        playedCard1.setOnDragListener((view, dragEvent) -> layoutView_OnDragHandler(view, dragEvent, -2));

        playerCard0 = (LinearLayout) findViewById(R.id.playerCard0);
        playerCard1 = (LinearLayout) findViewById(R.id.playerCard1);
        playerCard2 = (LinearLayout) findViewById(R.id.playerCard2);
        playerCard3 = (LinearLayout) findViewById(R.id.playerCard3);
        playerCard4 = (LinearLayout) findViewById(R.id.playerCard4);


        bChange = (Button) findViewById(R.id.bChange);
        bChange.setOnClickListener(this::bChange_Clicked);
        b20a = (Button) findViewById(R.id.b20a);
        b20a.setOnClickListener(this::b20a_Clicked);

        b20b = (Button) findViewById(R.id.b20b);
        b20b.setOnClickListener(this::b20b_Clicked);
        bContinue = (Button) findViewById(R.id.bContinue);
        bContinue.setOnClickListener(this::bContinue_Clicked);

        im0 = (ImageView) findViewById(R.id.im0);
        im0.setOnTouchListener((view, motionEvent) -> image_OnTouchListener(view, motionEvent, 0));

        im1 = (ImageView) findViewById(R.id.im1);
        im1.setOnTouchListener((view, motionEvent) -> image_OnTouchListener(view, motionEvent, 1));

        im2 = (ImageView) findViewById(R.id.im2);
        im2.setOnTouchListener((view, motionEvent) -> image_OnTouchListener(view, motionEvent, 2));

        im3 = (ImageView) findViewById(R.id.im3);
        im3.setOnTouchListener((view, motionEvent) -> image_OnTouchListener(view, motionEvent, 3));

        im4 = (ImageView) findViewById(R.id.im4);
        im4.setOnTouchListener((view, motionEvent) -> image_OnTouchListener(view, motionEvent, 4));

        imAtou = (ImageView) findViewById(R.id.imAtou);
        imAtou.setOnClickListener(arg0 -> imageView_ClickEventHandler(arg0, 10));
    }

    /**
     * b20a_Clicked
     * @param arg0 view that was clicked
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
     * @param arg0 view that was clicked
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
            setTextMessage(getString(R.string.you_say_pair,  aGame.printColor(aGame.said)));
            aGame.mqueue.insert(getString(R.string.you_say_pair,  aGame.printColor(aGame.said)));
            printMes();
            tPoints.setText(String.valueOf(aGame.gambler.points));
            if (aGame.gambler.points > 65) {
                twentyEnough(true);
            }
        } catch (Exception e) {
            this.errHandler(e);
        }
    }

    /**
     * b20b_Clicked
     * @param arg0 view that was clicked
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
            setTextMessage(getString(R.string.you_say_pair,  aGame.printColor(aGame.said)));
            aGame.mqueue.insert(getString(R.string.you_say_pair,  aGame.printColor(aGame.said)));
            printMes();
            tPoints.setText(String.valueOf(aGame.gambler.points));
            if (aGame.gambler.points > 65) {
                twentyEnough(true);
            }
        } catch (Exception e) {
            this.errHandler(e);
        }
    }

    /**
     * bContinue_Clicked
     * @param arg0 view that was clicked
     */
    public void bContinue_Clicked(View arg0) {
        continueTurn();
        imTalon.setOnClickListener(null);
    }

    /**
     * OnTouchListener
     * @param view current view
     * @param motionEvent the motion Event
     * @param ic additional indexer
     * @return true|false
     */
    protected boolean image_OnTouchListener(View view, MotionEvent motionEvent, int ic) {

        if (motionEvent.getAction() == MotionEvent.ACTION_DOWN) {

            if (!aGame.gambler.hand[ic].isValidCard()) {
                setTextMessage(getString(R.string.this_is_no_valid_card));
                aGame.mqueue.insert(getString(R.string.this_is_no_valid_card));
                printMes();
                return false;
            }

            atouCard = (LinearLayout) findViewById(R.id.atouCard);
            atouCard.setOnDragListener(null);

            ClipData data = ClipData.newPlainText("", "");
            DragShadowBuilder shadowBuilder = new View.DragShadowBuilder(view);

            int viewId = view.getId();
            String tmp;

            for (int i = 0; i < 5; i++){
                tmp = "im" + i;
                int myID = getApplicationContext().getResources().getIdentifier(tmp, "id", getApplicationContext().getPackageName());
                if (viewId == myID) {
                    touchedCard = aGame.gambler.hand[i];
                    break;
                }
            }

            if ((aGame.atouIsChangable(aGame.gambler)) && (!pSaid) &&
                    touchedCard.cardValue == CARDVALUE.JACK && touchedCard.isAtou()) {
                atouCard = (LinearLayout) findViewById(R.id.atouCard);
                atouCard.setOnDragListener((view1, dragEvent) -> layoutView_OnDragHandler(view1, dragEvent, -1));
            }

            playerCard0 = (LinearLayout) findViewById(R.id.playerCard0);
            playerCard0.setOnDragListener(null);
            playerCard1 = (LinearLayout) findViewById(R.id.playerCard1);
            playerCard1.setOnDragListener(null);
            playerCard2 = (LinearLayout) findViewById(R.id.playerCard2);
            playerCard2.setOnDragListener(null);
            playerCard3 = (LinearLayout) findViewById(R.id.playerCard3);
            playerCard3.setOnDragListener(null);
            playerCard4 = (LinearLayout) findViewById(R.id.playerCard4);
            playerCard4.setOnDragListener(null);

            if (touchedCard.cardValue == CARDVALUE.QUEEN || touchedCard.cardValue == CARDVALUE.KING) {

                for (int idx = 0; idx < 5; idx++) {
                    Card dropCard = aGame.gambler.hand[idx];
                    if (dropCard.isValidCard() &&
                            (dropCard.cardColor == touchedCard.cardColor) &&
                            (dropCard.getValue() + touchedCard.getValue() == 7)) {

                        switch (idx) {
                            case 0:
                                playerCard0.setOnDragListener((view12, dragEvent) -> layoutView_OnDragHandler(view12, dragEvent, 0));
                                break;
                            case 1:
                                playerCard1.setOnDragListener((view13, dragEvent) -> layoutView_OnDragHandler(view13, dragEvent, 1));
                                break;
                            case 2:
                                playerCard2.setOnDragListener((view14, dragEvent) -> layoutView_OnDragHandler(view14, dragEvent, 2));
                                break;
                            case 3:
                                playerCard3.setOnDragListener((view15, dragEvent) -> layoutView_OnDragHandler(view15, dragEvent, 3));
                                break;
                            case 4:
                                playerCard4.setOnDragListener((view16, dragEvent) -> layoutView_OnDragHandler(view16, dragEvent, 4));
                                break;
                            default:
                                break;
                        }
                    }
                }

            }

            // view.startDragAndDrop(data, shadowBuilder, view, 0);
            view.startDragAndDrop(data, shadowBuilder, view, 0);
            view.setVisibility(View.INVISIBLE);
            return true;
        } else {
            return false;
        }
    }

    /**
     * OnDragHandler
     * @param view current view
     * @param dragEvent current drag event
     * @param ic additional index to specify event
     * @return true|false
     */
    protected boolean layoutView_OnDragHandler(View view, DragEvent dragEvent, int ic) {

        Drawable enterShape = getResources().getDrawable(R.drawable.shape_droptarget, getApplicationContext().getTheme());
        Drawable normalShape = getResources().getDrawable(R.drawable.shape, getApplicationContext().getTheme());

        if (touchedCard == null)
            return false;

        Drawable savedBackground;
        int action = dragEvent.getAction();
        switch (dragEvent.getAction()) {
            case DragEvent.ACTION_DRAG_STARTED:
                // do nothing
                savedBackground = view.getBackground();
                if (!dragNDropMap.containsKey(view.getId())) {
                    dragNDropMap.put(view.getId(), savedBackground);
                }

                break;
            case DragEvent.ACTION_DRAG_ENTERED:
                savedBackground = view.getBackground();
                if (!dragNDropMap.containsKey(view.getId())) {
                    dragNDropMap.put(view.getId(), savedBackground);
                }
                view.setBackground(enterShape);
                break;
            case DragEvent.ACTION_DRAG_EXITED:

                if (dragNDropMap.containsKey(view.getId())) {
                    Drawable dr = dragNDropMap.get(view.getId());
                    if (dr != null) {
                        view.setBackground(enterShape);
                        view.setBackground(dr);
                        dragNDropMap.remove(view.getId());
                    }
                }

                view.setBackground(normalShape);
                break;
            case DragEvent.ACTION_DROP:
                // Dropped, reassign View to ViewGroup
                View dropView = (View) dragEvent.getLocalState();
                ViewGroup owner = (ViewGroup) dropView.getParent();
                // owner.removeView(dropView);
                LinearLayout container = (LinearLayout) view;

                dragged20 = false;
                droppedCard = true;

                int viewID = dropView.getId();
                int lcId = container.getId();

                if (lcId == R.id.atouCard || ic == -1) {
                    draggedCard = touchedCard;
                    touchedCard = null;
                    // container.addView(view, 0);
                    view.setVisibility(View.VISIBLE);
                    bChange_Clicked(view);
                    showPlayersCards();
                    return  true;
                }
                if (lcId == R.id.playedCard0 || lcId == R.id.playedCard1 || ic == -2) {
                    // container.addView(view, 0);
                    view.setVisibility(View.VISIBLE);
                    draggedCard = touchedCard;
                    touchedCard = null;

                    String tmp;
                    for (int i = 0; i < 5; i++){
                        tmp = "im" + i;
                        int myID = getApplicationContext().getResources().getIdentifier(tmp, "id", getApplicationContext().getPackageName());
                        if (viewID == myID) {
                            imageView_ClickEventHandler(view, i);
                            return true;
                        }
                    }

//                    switch (viewID) {
//                        case R.id.im0:
//                            imageView_ClickEventHandler(view, 0);
//                            break;
//                        case R.id.im1:
//                            imageView_ClickEventHandler(view, 1);
//                            break;
//                        case R.id.im2:
//                            imageView_ClickEventHandler(view, 2);
//                            break;
//                        case R.id.im3:
//                            imageView_ClickEventHandler(view, 3);
//                            break;
//                        case R.id.im4:
//                            imageView_ClickEventHandler(view, 4);
//                            break;
//                        default:
//                            // assert(0);
//                            break;
//                    }
//                    return true;
                }

                if (lcId == R.id.playerCard0 || lcId == R.id.playerCard1 || lcId == R.id.playerCard2 ||
                        lcId == R.id.playerCard3 || lcId == R.id.playerCard4) {
                    if (touchedCard.cardValue != CARDVALUE.QUEEN && touchedCard.cardValue != CARDVALUE.KING) {
                        view.setVisibility(View.VISIBLE);
                        return false;
                    }
                    Card dropCard = null;
                    String tmp;
                    for (int i = 0; i < 5; i++){
                        tmp = "playerCard" + i;
                        int myID = getApplicationContext().getResources().getIdentifier(tmp, "id", getApplicationContext().getPackageName());
                        if (lcId == myID) {
                            dropCard = aGame.gambler.hand[i];
                        }
                    }
                    switch (lcId) {
                        case R.id.playerCard0:
                            dropCard = aGame.gambler.hand[0];
                            break;
                        case R.id.playerCard1:
                            dropCard = aGame.gambler.hand[1];
                            break;
                        case R.id.playerCard2:
                            dropCard = aGame.gambler.hand[2];
                            break;
                        case R.id.playerCard3:
                            dropCard = aGame.gambler.hand[3];
                            break;
                        case R.id.playerCard4:
                            dropCard = aGame.gambler.hand[4];
                            break;
                        default:
                            break;
                    }
                    if ((dropCard.cardColor == touchedCard.cardColor) &&
                            (dropCard.getValue() + touchedCard.getValue() == 7)) {

                        if (dropCard.isAtou()) {
                        // if (aGame.gambler.handpairs[0] == aGame.atouInGame) {
                            aGame.gambler.points += 40;
                        } else {
                            aGame.gambler.points += 20;
                        }
                        pSaid = true;
                        resetButtons(0);
                        aGame.said = dropCard.getColor();
                        setTextMessage(getString(R.string.you_say_pair,  aGame.printColor(aGame.said)));
                        aGame.mqueue.insert(getString(R.string.you_say_pair,  aGame.printColor(aGame.said)));
                        printMes();
                        tPoints.setText(String.valueOf(aGame.gambler.points));
                        if (aGame.gambler.points > 65) {
                            twentyEnough(true);
                        }

                        dragged20 = true;

                        switch (viewID) {
                            case R.id.im0:
                                imageView_ClickEventHandler(view, 0);
                                im0.setImageResource(R.drawable.e);
                                break;
                            case R.id.im1:
                                imageView_ClickEventHandler(view, 1);
                                im1.setImageResource(R.drawable.e);
                                break;
                            case R.id.im2:
                                imageView_ClickEventHandler(view, 2);
                                im2.setImageResource(R.drawable.e);
                                break;
                            case R.id.im3:
                                imageView_ClickEventHandler(view, 3);
                                im3.setImageResource(R.drawable.e);
                                break;
                            case R.id.im4:
                                imageView_ClickEventHandler(view, 4);
                                im4.setImageResource(R.drawable.e);
                                break;
                            default:
                                // assert(0);
                                break;
                        }

                        view.setVisibility(View.VISIBLE);
                        return true;
                    }

                    view.setVisibility(View.VISIBLE);
                    showPlayersCards();
                    return false;

                }

                // container.addView(view, 0);
                // container.addView(view);
                view.setVisibility(View.VISIBLE);
                break;
            case DragEvent.ACTION_DRAG_LOCATION:
            case DragEvent.ACTION_DRAG_ENDED:

                if (dragNDropMap.containsKey(view.getId())) {
                    Drawable dr = dragNDropMap.get(view.getId());
                    if (dr != null) {
                        view.setBackground(enterShape);
                        view.setBackground(dr);
                        dragNDropMap.remove(view.getId());
                    }
                }

                if (!dragged20 || !droppedCard)
                    showPlayersCards();
            default:
                break;
        }
        return true;
    }

    /**
     * EventHandler for all ImageViews
     * @param arg0 View, that fired click
     * @param ic image counter, that represents which ImageView is clicked
     */
    protected void imageView_ClickEventHandler(View arg0, int ic) {

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
                setTextMessage(getString(R.string.this_is_no_valid_card));
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
                    setTextMessage(getString(R.string.you_must_play_pair_card));
                    aGame.mqueue.insert(getString(R.string.you_must_play_pair_card));
                    printMes();
                    return ;
                }
            }
            if (aGame.colorHitRule && (!aGame.playersTurn)) {
                // CORRECT WAY ?
                if ((!aGame.gambler.isInColorHitsContextValid(ic,aGame.computer.hand[ccard]))) {
                    setTextMessage(getString(R.string.you_must_play_color_hit_force_rules));
                    aGame.mqueue.insert(getString(R.string.you_must_play_color_hit_force_rules));
                    int tmpint = aGame.gambler.bestInColorHitsContext(aGame.computer.hand[ccard]);
                    // for (j = 0; j < 5; j++) {
                    //     c_array = c_array + aGame.gambler.colorHitArray[j] + " ";
                    // }
                    // aGame.mqueue.insert(c_array);

                    aGame.mqueue.insert(getString(R.string.best_card_would_be, aGame.gambler.hand[tmpint].getName()));
                    printMes();
                    showPlayersCards();
                    return ;
                }
            }
            if (psaychange > 0) {
                resetButtons(0);
                psaychange = 0;
            }
            aGame.playedOut = aGame.gambler.hand[ic];
            // Besser Cards als Array
            String tmp = "im" + ic;
            int myID = getApplicationContext().getResources().getIdentifier(tmp, "id", getApplicationContext().getPackageName());
            ImageView cardPlayed = (ImageView) findViewById(myID);
            cardPlayed.setImageResource(R.drawable.e);

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

            // imOut0.setImageResource(aGame.gambler.hand[ic].getResourcesInt());
            imOut0.setImageDrawable(aGame.gambler.hand[ic].getDrawable());

        } catch (Exception e) {
            this.errHandler(e);
        }
        aGame.gambler.hand[ic] = aGame.emptyTmpCard;
        ready = false;
        endTurn();

    }


    /*
     * Future design
     */
    protected void SetImageDrawable(ImageView imageView, Card card) {
        if (aGame != null && card == null)
            card = aGame.emptyTmpCard;


        String tmp = card.color + String.valueOf(card.value);
        imageView.setImageDrawable(card.getDrawable());

        imageView.setTag(0, tmp);
        imageView.setTag(1, imageView.getVisibility());
    }



    /**
     * setTextMessage shows a new Toast dynamic message
     * @param text to display
     */
    private void setTextMessage(CharSequence text) {
        if (text != null && text != "") {
            Context context = getApplicationContext();
            int duration = Toast.LENGTH_SHORT;
            Toast toast = Toast.makeText(context, text, duration);
            toast.show();
        }
    }

    /**
     * print message queue
     */
    private void printMes() {
        tDbg.append(aGame.mqueue.fetch());
    }

    /**
     * Error handler
     * @param myErr java.lang.Throwable
     */
    private void errHandler(java.lang.Throwable myErr) {
        tDbg.append("\nCRITICAL ERROR #" + ++errNum + " " + myErr.getMessage());
        tDbg.append(myErr.toString());
        tDbg.append("\nMessage: "+ myErr.getLocalizedMessage() + "\n");
        myErr.printStackTrace();
    }

    /**
     * helpText() prints out help text
     */
    public void helpText() {
        Intent intent = new Intent(this, AboutActivity.class);
        startActivity(intent);
    }

}