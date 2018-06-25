package at.area23.heinrichelsigan.schnapslet;

import at.area23.heinrichelsigan.schnapslet.card;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.app.Activity;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.content.res.Configuration;
import android.view.View;
import android.view.View.OnClickListener;
import java.net.URL;

public class MainActivity extends AppCompatActivity {

    Button bMerge, bStop, bHelp, b20a, b20b,  bChange, bcont;
    ImageView im0,im1,im2, im3, im4, imOut0, imOut1, imTalon, imAtou;
    TextView tRest, tPoints, tMes, tDbg;

    // final static String PROTO = "http";
    // final static String HOST  = "^www.area23.at";
    // final static int    PORT  = 80;
    long errNum = 0; // Errors Ticker
    int ccard; // Computers Card played
    volatile card emptyTmpCard;
    volatile boolean ready = false; // Ready to play
    volatile byte psaychange = 0;
    boolean pSaid = false; // Said something
    static java.lang.Runtime runtime = null;
    // URL emptyURL, backURL, talonURL, notURL;
    // static String emptyJarStr =	"cardpics/e.gif";
    // static String backJarStr =	"cardpics/verdeckt.gif";
    // static String notJarStr = 	"cardpics/n0.gif";
    // static String talonJarStr =	"cardpics/t.gif";
    game aGame;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        // count = 0;
        tDbg = (TextView) findViewById(R.id.tDbg);

        if(getResources().getDisplayMetrics().widthPixels>getResources().getDisplayMetrics().
                heightPixels)
        {
            tDbg.setText("Screen switched to Landscape mode");
            setContentView(R.layout.activity_main_vertical);
        }
        else
        {
            tDbg.setText("Screen switched to Portrait mode");
            setContentView(R.layout.activity_main);
        }

        im0 = (ImageView) findViewById(R.id.im0);
        im1 = (ImageView) findViewById(R.id.im1);
        im2 = (ImageView) findViewById(R.id.im2);
        im3 = (ImageView) findViewById(R.id.im3);
        im4 = (ImageView) findViewById(R.id.im4);
        im4 = (ImageView) findViewById(R.id.im4);
        imOut0 =  (ImageView) findViewById(R.id.imOut0);
        imOut1 =  (ImageView) findViewById(R.id.imOut1);
        imTalon =  (ImageView) findViewById(R.id.imTalon);
        imAtou =  (ImageView) findViewById(R.id.imAtou);

        bMerge = (Button) findViewById(R.id.bMerge);
        bStop = (Button) findViewById(R.id.bStop);
        bHelp = (Button) findViewById(R.id.bHelp);
        b20a =  (Button) findViewById(R.id.b20a);
        b20b =  (Button) findViewById(R.id.b20b);
        bChange = (Button) findViewById(R.id.bChange);
        bcont = (Button) findViewById(R.id.bcont);


        tMes = (TextView) findViewById(R.id.tMes);
        tPoints = (TextView) findViewById(R.id.tPoints);
        tRest = (TextView) findViewById(R.id.tRest);

        tMes.setVisibility(View.INVISIBLE);

        bStop.setEnabled(false);
        bChange.setEnabled(false);

        bMerge.setEnabled(true);
        bHelp.setEnabled(true);

        addListenerOnClickables();




        initURLBase();
        resetButtons(1);
    }


    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);

        if (newConfig.orientation == Configuration.ORIENTATION_PORTRAIT)
        {
            setContentView(R.layout.activity_main);
            tDbg.setText("Layout PORTRAIT");
        }
        if (newConfig.orientation == Configuration.ORIENTATION_LANDSCAPE)
        {
            setContentView(R.layout.activity_main_vertical);
            tDbg.setText("Layout LANDSCAPE");
        }
    }


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
    }

    void showTalonCard() {
        try {
            imTalon.setImageResource (R.drawable.t);
        } catch (Exception imTalonEx) {
            System.err.println(imTalonEx.toString());
            imTalonEx.printStackTrace();
        }
        imTalon.setVisibility(View.VISIBLE);
    }

    void showAtouCard() {
        try {
            imAtou.setImageResource(aGame.set[19].getResInt());
        } catch (Exception exp) {
            this.errHandler(exp);
        }

    }

    void resetButtons(int level) {
        if (level >= 0 ) {
            b20a.setText("20 Ansagen");
            b20a.setEnabled(false);
            b20b.setText("40 Ansagen");
            b20b.setEnabled(false);
            bChange.setEnabled(false);
        }
        if (level >= 1) {
            bcont.setEnabled(false);
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

    void showPlayersCards() {

        try {
            // String myStr = String.valueOf(aGame.gambler.hand[0].getResInt()) + " ; " +
            //         String.valueOf(aGame.gambler.hand[1].getResInt()) + " ; "+
            //         String.valueOf(aGame.gambler.hand[2].getResInt()) + " ; ";
            // tDbg.setText(myStr);

            im0.setImageResource(aGame.gambler.hand[0].getResInt());
            im1.setImageResource(aGame.gambler.hand[1].getResInt());
            im2.setImageResource(aGame.gambler.hand[2].getResInt());
            im3.setImageResource(aGame.gambler.hand[3].getResInt());
            im4.setImageResource(aGame.gambler.hand[4].getResInt());

        } catch (Exception exp) {
            this.errHandler(exp);
        }
    }

    void startGame() {	// Mischen
        bMerge.setEnabled(false);
        aGame = null;

        // runtime = java.lang.Runtime.getRuntime();
        // runtime.runFinalization();
        // runtime.gc();

        aGame = new game();
        tMes.setVisibility(View.INVISIBLE);
        resetButtons(1);

        tDbg.setText("Neues Spiel started ...");
        tRest.setText("10");

        emptyTmpCard = new card(); // new card(this, -1);
        tPoints.setText("" + String.valueOf(aGame.gambler.points));
        showAtouCard();
        showTalonCard();
        bStop.setEnabled(true);

        gameTurn(0);
    }

    void closeGame() { //	Implementierung des Zudrehens
        if (aGame.isGame == false || aGame.gambler == null) {
            tMes.setVisibility(View.VISIBLE);
            tMes.setText("Kein Spiel gestartet!");
            return;
        }
        tMes.setVisibility(View.VISIBLE);
        tMes.setText("Spieler dreht zu !");

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

    void gameTurn(int ixlevel) {
        if (ixlevel < 1) {
            try {
                imOut0.setImageResource(R.drawable.leer);
                imOut1.setImageResource(R.drawable.leer);
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
                b20a.setText(aGame.printColor(aGame.gambler.handpairs[0]) +" ansagen");
                b20a.setEnabled(true);

                if (a20 > 1) {
                    b20b.setText(aGame.printColor(aGame.gambler.handpairs[1])+" ansagen");
                    b20b.setEnabled(true);
                } else {
                    b20b.setText("kein 2. Paar");
                }
            }
            // Info
            tMes.setVisibility(View.VISIBLE);
            tMes.setText("Zum Auspielen einfach auf die entsprechende Karte klicken");
        } else {
            // COMPUTERS TURN IMPLEMENTIEREN
            if (aGame.atouIsChangable(aGame.computer)) {
                aGame.changeAtou(aGame.computer);
                this.showAtouCard();
                tMes.setVisibility(View.VISIBLE);
                tMes.setText("COMPUTER TAUSCHT ATOU AUS !!!");
                aGame.mqueue.insert("Computer tauscht Atou aus !");
            }
            ccard = aGame.computerStarts();
            if (aGame.csaid != 'n') {
                tMes.setVisibility(View.VISIBLE);;
                tMes.setText("COMPUTER SAGT PAAR IN " + aGame.printColor(aGame.csaid) + " AN !!!");
                aGame.mqueue.insert("Computer sagt Paar in " + aGame.printColor(aGame.csaid) + " an !");
                if (aGame.computer.points > 65) {
                    twentyEnough(false);
                }
            }
            try {
                imOut1.setImageResource(aGame.computer.hand[ccard].getResInt());
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
            // tMes.setVisibility(View.VISIBLE);
            // tMes.setText("Zum Antworten einfach auf die entsprechende Karte klicken");
        }

        ready = true;
        printMes();
    }

    void twentyEnough(boolean who) {
        int xj = 0;
        String andEnough = "20 und genug !";
        ready = false;


        if (aGame.said == aGame.atouInGame) {
            andEnough = "40 und genug !";
        }

        if (who) {
            try {
                for (xj = 0; xj < 5; xj++) {
                    if (aGame.gambler.hand[xj].color == aGame.said &&
                            aGame.gambler.hand[xj].value == 3)
                        imOut0.setImageResource(aGame.gambler.hand[xj].getResInt());
                    if (aGame.gambler.hand[xj].color == aGame.said &&
                            aGame.gambler.hand[xj].value == 4)
                        imOut1.setImageResource(aGame.gambler.hand[xj].getResInt());
                }
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }

            tsEnds(new String(andEnough + " Sie haben gewonnen mit " + String.valueOf(aGame.gambler.points) + " Punkten !"), 1);

        } else {

            try {
                for (xj = 0; xj < 5; xj++) {
                    if (aGame.computer.hand[xj].color == aGame.said &&
                            aGame.computer.hand[xj].value == 3)
                        imOut0.setImageResource(aGame.computer.hand[xj].getResInt());
                    if (aGame.computer.hand[xj].color == aGame.said &&
                            aGame.computer.hand[xj].value == 4)
                        imOut1.setImageResource(aGame.computer.hand[xj].getResInt());
                }
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }

            printMes();
            tsEnds(new String(andEnough+"Computer hatgewonnen mit " + String.valueOf(aGame.computer.points) + " Punkten !"), 1);
        }
        return;
    }

    void endTurn() {
        int tmppoints;
        /* IMPLEMENT COMPUTERS STRATEGIE HERE */
        if (aGame.playersTurn) {
            ccard = aGame.computersAnswer();
            try {
                imOut1.setImageResource(aGame.computer.hand[ccard].getResInt());
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
            tMes.setText("Ihr Stich mit Punkten " + String.valueOf(tmppoints) + " ! Klicken Sie auf Weiter !");
            if (aGame.isClosed && (aGame.computer.hasClosed)) {
                tsEnds("Zudrehen des Computers fehlgeschlagen, sie haben gewonnen !", 1);
                return ;
            }
        } else {
            tMes.setVisibility(View.VISIBLE);
            tMes.setText("Computer sticht " + String.valueOf((-tmppoints)) + " ! Klicken Sie auf Weiter !");
            if ((aGame.isClosed) && (aGame.gambler.hasClosed)) {
                tsEnds("Zudrehen fehlgeschlagen, Computer hat gewonnen !", 1);
                return ;
            }
        }

        // Assign new cards
        if (aGame.assignNewCard() == 1) {
            /* NOW WE HAVE NO MORE TALON */
            try {
                imTalon.setImageResource(R.drawable.leer);
                imTalon.setVisibility(View.INVISIBLE);
                imAtou.setImageResource(R.drawable.leer);
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
            tMes.setVisibility(View.VISIBLE);
            tMes.setText("Keine Karten im Talon -> Farb- und Stichzwang !");
        }
        tRest.setText(""+(19-aGame.index));
        printMes();
        // resetButtons(0);
        pSaid = false;
        aGame.said = 'n';
        aGame.csaid = 'n';

        if (aGame.playersTurn) {
            if (aGame.gambler.points > 65) {
                tsEnds("Sie haben gewonnen mit " + String.valueOf(aGame.gambler.points) + " Punkten !", 1);
                return;
            }
        } else {
            if (aGame.computer.points > 65) {
                tsEnds("Computer hat gewonnen mit " + String.valueOf(aGame.computer.points) + " Punkten !", 1);
                return;
            }
        }

        if (aGame.movs >= 5) {
            if (aGame.isClosed) {
                if (aGame.gambler.hasClosed) {
                    tsEnds("Zudrehen fehlgeschlagen, Computer hat gewonnen !", 1);
                }
                if (aGame.computer.hasClosed) {
                    tsEnds("Computers Zudrehen fehlgeschlagen, Sie haben gewonnen !", 1);
                }
                return ;
            } else {
                if (tmppoints > 0) {
                    tsEnds("Letzter Stich: Sie haben gewonnen !", 1);
                } else {
                    tsEnds("Letzter Stich: Computer hat gewonnen !", 1);
                }
                return;
            }
        }

        bcont.setEnabled(true);
        ready = false;
    }

    void stopGame(int levela) {
        bStop.setEnabled(false);
        aGame.stopGame();

        resetButtons(levela);
        bMerge.setEnabled(true);

        showPlayersCards();
        aGame.destroyGame();
        // java.lang.System.runFinalization();
        // java.lang.System.gc();
    }

    void tsEnds(String endMessage, int ix) {
        tMes.setText(endMessage);
        tMes.setVisibility(View.VISIBLE);
        stopGame(ix);
        return ;
    }

    /* Add all EventHandlers on Clickabl elements */

    public void addListenerOnClickables() {

        // imageView1.setOnClickListener() { }
        bMerge = (Button) findViewById(R.id.bMerge);
        bMerge.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                bMerge_Clicked(arg0);
            }
        });
        bStop = (Button) findViewById(R.id.bStop);
        bStop.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                bStop_Clicked(arg0);
            }
        });
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
        bcont = (Button) findViewById(R.id.bcont);
        bcont.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                bcont_Clicked(arg0);
            }
        });
        bHelp = (Button) findViewById(R.id.bHelp);
        bHelp.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                bHelp_Clicked(arg0);
            }
        });

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

    /* Event Handler Buttons */

    public void bMerge_Clicked(View arg0) {
        startGame();
    }

    public void bStop_Clicked(View arg0) {
        try {
            stopGame(2);
        } catch (Exception e) {
            this.errHandler(e);
        }
    }

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
            aGame.mqueue.insert("Spieler sagt Paar in " + aGame.printColor(aGame.said) + " an !");
            printMes();
            tPoints.setText("" + String.valueOf(aGame.gambler.points));
            if (aGame.gambler.points > 65) {
                twentyEnough(true);
            }
        } catch (Exception e) {
            this.errHandler(e);
        }
    }

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
            aGame.mqueue.insert("Spieler sagt Paar in " + aGame.printColor(aGame.said) + " an !");
            printMes();
            tPoints.setText("" + String.valueOf(aGame.gambler.points));
            if (aGame.gambler.points > 65) {
                twentyEnough(true);
            }
        } catch (Exception e) {
            this.errHandler(e);
        }
    }

    public void bcont_Clicked(View arg0) {
        try {
            ready = true;
            bcont.setEnabled(false);
            tMes.setVisibility(View.INVISIBLE);
            gameTurn(0);
        } catch (Exception e) {
            this.errHandler(e);
        }
    }

    /* EventHandler for all ImageViews */

    void imageView_ClickEventHandler(View arg0, int ic) {

        int j;
        String c_array = "Player Array: ";
        try {
            if (ready == false) {
                return;
            }

            if (ic == 10) {
                if (aGame.playersTurn && (aGame.isClosed == false) && (pSaid == false) && (aGame.index < 16)) {
                    closeGame();
                }
                return;
            }
            if (aGame.gambler.hand[ic].isValidCard() == false) {
                aGame.mqueue.insert("Das ist keine gültige Karte !");
                printMes();
                return;
            }
            if (pSaid) {
                if ((aGame.said == aGame.gambler.hand[ic].getColor()) &&
                        (aGame.gambler.hand[ic].getValue() > 2) &&
                        (aGame.gambler.hand[ic].getValue() < 5)) {
                    ; // we can continue
                } else {
                    aGame.mqueue.insert("Sie muessen eine Karte vom Paar ausspielen !");
                    printMes();
                    return ;
                }
            }
            if (aGame.colorHitRule && (aGame.playersTurn == false)) {
                // CORRECT WAY ?
                if ((aGame.gambler.isInColorHitsContextValid(ic,aGame.computer.hand[ccard])) == false) {
                    aGame.mqueue.insert("Farb und Stichzwang muss eingehalten werden !");
                    int tmpint = aGame.gambler.bestInColorHitsContext(aGame.computer.hand[ccard]);
                    for (j=0; j<5; j++) {
                        c_array=c_array + aGame.gambler.colorHitArray[j] + " ";
                    }
                    aGame.mqueue.insert(c_array);
                    aGame.mqueue.insert("Beste Karte wäre: "+tmpint);
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

            imOut0.setImageResource(aGame.gambler.hand[ic].getResInt());

        } catch (Exception e) {
            this.errHandler(e);
        }
        aGame.gambler.hand[ic] = emptyTmpCard;
        ready = false;
        endTurn();

    }

    /* tDbg setting text */

    void printMes() {
        tDbg.setText(aGame.mqueue.fetch());
    }

    void errHandler(java.lang.Throwable myErr) {
        tDbg.setText("\nCRITICAL ERROR #" + String.valueOf((++errNum))  + " " + myErr.getMessage());
        tDbg.append(myErr.toString());
        tDbg.append("\nLmessage: "+ myErr.getLocalizedMessage() + "\n");
        myErr.printStackTrace();
    }

    public void bHelp_Clicked(View arg0) {
        tDbg.setText("-------------------------------------------------------------------------\n");
        tDbg.append("Schnapslet V 0.2 - Pre Alpha Release \n");
        tDbg.append("Implementierung des Kartenspiel Schnapsen als einfaches java.awt.Applet\n");
        tDbg.append("von Heinrich Elsigan (heinrich.elsigan@area23.at)\n\n");
        tDbg.append("Funktionsweise:\n");
        tDbg.append("Das Spiel ist so angelegt, dass man gegen den Computer spielt.\n");
        tDbg.append("Ist man am Zug, so kann man eine Karte ausspielen, indem man auf das\n");
        tDbg.append("Kartensymbol klickt. Andere Optionen, wie \"Atou austauschen\" oder \n");
        tDbg.append("\"Ein Paar Ansagen\" sind über die Buttons links oben moeglich; diese\n");
        tDbg.append("Optionen muessen gewaehlt werden, bevor man eine Karte auspielt !\n");
        tDbg.append("Ist der Computer am Zug, so spielt dieser eine Karte aus und man selbst\n");
        tDbg.append("kann dann durch Klick auf die eigenen Karten, stechen oder draufgeben!\n");
        tDbg.append("Die Regeln entsprechen dem oesterreichischen Schnapsen, allerdings gibt\n");
        tDbg.append("es bei der Implementierung des Farb- und Stichzwangs noch kleine Bugs!\n");
        tDbg.append("Zudrehen ist implementiert. Man muss einfach auf die Atou Karte klicken.\n");
        tDbg.append("Ideen, Vorschläge, Verbesserungen werden gerne angenommen !\n");
        tDbg.append("-------------------------------------------------------------------------\n");
    }

}