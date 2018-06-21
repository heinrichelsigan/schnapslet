package at.area23.heinrichelsigan.schnapslet;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.app.Activity;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.view.View;
import android.view.View.OnClickListener;
import java.net.URL;

public class MainActivity extends AppCompatActivity {

    Button bMerge, bStop, bHelp, b20a, b20b,  bChange, bcont;
    ImageView im0,im1,im2, im3, im4, imOut0, imOut1, imTalon, imAtou;
    TextView tRest, tPoints, tMes, tDbg;

    int count = 0;
    final static String PROTO = "http";
    final static String HOST  = "^www.area23.at";
    final static int    PORT  = 80;
    long errNum = 0; // Errors Ticker
    int ccard; // Computers Card played
    volatile card emptyTmpCard;
    volatile boolean ready = false; // Ready to play
    volatile byte psaychange = 0;
    boolean pSaid = false; // Said something
    static java.lang.Runtime runtime = null;
    URL emptyURL, backURL, talonURL, notURL;
    static String emptyJarStr =	"cardpics/e.gif";
    static String backJarStr =	"cardpics/verdeckt.gif";
    static String notJarStr = 	"cardpics/n0.gif";
    static String talonJarStr =	"cardpics/t.gif";
    game aGame;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        // count = 0;

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

        tDbg = (TextView) findViewById(R.id.tDbg);
        tMes = (TextView) findViewById(R.id.tMes);
        tPoints = (TextView) findViewById(R.id.tPoints);
        tRest = (TextView) findViewById(R.id.tRest);

        tMes.setVisibility(View.INVISIBLE);
        initURLBase();

        addListenerOnButton();
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

    public void addListenerOnButton() {

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
                im0_Clicked(arg0);
            }
        });
        im1 = (ImageView) findViewById(R.id.im1);
        im1.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                im1_Clicked(arg0);
            }
        });
        im2 = (ImageView) findViewById(R.id.im2);
        im2.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                im2_Clicked(arg0);
            }
        });
        im3 = (ImageView) findViewById(R.id.im3);
        im3.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                im3_Clicked(arg0);
            }
        });
        im4 = (ImageView) findViewById(R.id.im4);
        im4.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                im4_Clicked(arg0);
            }
        });
        imAtou = (ImageView) findViewById(R.id.imAtou);
        imAtou.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                imAtou_Clicked(arg0);
            }
        });
    }

    void showTalonCard() {
        try {
            // imTalon.setImage(setJarIncludedImage(talonJarStr));
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


    void errHandler(java.lang.Throwable myErr) {
        tDbg.setText("");
        tDbg.append("\nCRITICAL ERROR #" + (++errNum));
        tDbg.append("\nMessage: " + myErr.getMessage());
        tDbg.append("\nString: " + myErr.toString());
        tDbg.append("\nLmessage: "+ myErr.getLocalizedMessage() + "\n");
        myErr.printStackTrace();
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
            // if (imTalon.isVisible()==false)
            //    imTalon.setVisible(true);
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
            String myStr = String.valueOf(aGame.gambler.hand[0].getResInt()) + " ; " +
                    String.valueOf(aGame.gambler.hand[1].getResInt()) + " ; "+
                    String.valueOf(aGame.gambler.hand[2].getResInt()) + " ; ";
            tDbg.setText(myStr);

            im0.setImageResource(aGame.gambler.hand[0].getResInt());
            im1.setImageResource(aGame.gambler.hand[1].getResInt());
            im2.setImageResource(aGame.gambler.hand[2].getResInt());
            im3.setImageResource(aGame.gambler.hand[3].getResInt());
            im4.setImageResource(aGame.gambler.hand[4].getResInt());

            /*
            android.net.Uri myUri = aGame.gambler.hand[0].getPictureUri();

            im0.setImageURI(myUri);
            android.net.Uri myUri1 = aGame.gambler.hand[1].getPictureUri();
            im1.setImageURI(myUri1);
            im2.setImageURI(aGame.gambler.hand[2].getPictureUri());
            im3.setImageURI(aGame.gambler.hand[3].getPictureUri());
            im4.setImageURI(aGame.gambler.hand[4].getPictureUri());
            */

        } catch (Exception exp) {
            this.errHandler(exp);
        }
    }


    void startGame() {	// Mischen
        bMerge.setEnabled(false);
        runtime = java.lang.Runtime.getRuntime();
        aGame = null;
        runtime.runFinalization();
        runtime.gc();

        aGame = new game();
        tMes.setVisibility(View.INVISIBLE);
        resetButtons(1);
        tDbg.setText("");
        tRest.setText("10");

        emptyTmpCard = new card(); // new card(this, -1);
        tPoints.setText(""+aGame.gambler.points);
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
            // imTalon.setImage(setJarIncludedImage(emptyJarStr));
            imTalon.setImageResource(R.drawable.leer);
            imTalon.setVisibility(View.INVISIBLE);
        } catch (Exception jbpvex) {
            this.errHandler(jbpvex);
        }

        try {
            // imAtou.setImage(setJarIncludedImage(backJarStr));
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
            // Info
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
            tMes.setText("Zum Antworten einfach auf die entsprechende Karte klicken");
        }

        ready = true;
        printMes();
    }


    void twentyEnough(boolean who) {
        int xj = 0;
        String andEnough = "20 und genug !";
        ready = false;
        if (who) {
            try {
                while(aGame.gambler.hand[xj++].color != aGame.said) ;
                // imOut0.setImage(aGame.gambler.hand[xj-1].getImage());
                imOut0.setImageResource(aGame.gambler.hand[xj-1].getResInt());
                // imOut1.setImage(aGame.gambler.hand[xj].getImage());
                imOut1.setImageResource(aGame.gambler.hand[xj].getResInt());
                if (aGame.said == aGame.atouInGame) {
                    andEnough = "40 und genug !";
                }
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
            tsEnds(new String(andEnough+" Sie haben gewonnen mit " + aGame.gambler.points + " Punkten !"), 1);
        } else {
            try {
                while(aGame.computer.hand[xj++].color != aGame.csaid) ;
                // imOut0.setImage(aGame.computer.hand[xj-1].getImage());
                imOut0.setImageResource(aGame.computer.hand[xj-1].getResInt());
                // imOut1.setImage(aGame.computer.hand[xj].getImage());
                imOut1.setImageResource(aGame.computer.hand[xj].getResInt());
                if (aGame.csaid == aGame.atouInGame) {
                    andEnough="40 und genug !";
                }
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
            printMes();
            tsEnds(new String(andEnough+"Computer hatgewonnen mit " + aGame.computer.points + " Punkten !"), 1);
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
        tPoints.setText("" + aGame.gambler.points);

        if (tmppoints > 0) {
            tMes.setText("Ihr Stich mit Punkten " + tmppoints + " ! Klicken Sie auf Weiter !");
            if (aGame.isClosed && (aGame.computer.hasClosed)) {
                tsEnds("Zudrehen des Computers fehlgeschlagen, sie haben gewonnen !", 1);
                return ;
            }
        } else {
            tMes.setText("Computer sticht " + (-tmppoints) + " ! Klicken Sie auf Weiter !");
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
                tsEnds("Sie haben gewonnen mit " + aGame.gambler.points + " Punkten !", 1);
                return;
            }
        } else {
            if (aGame.computer.points > 65) {
                tsEnds("Computer hat gewonnen mit " + aGame.computer.points + " Punkten !", 1);
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


    void printMes() {
        tDbg.append(aGame.mqueue.fetch());
    }

    void tsEnds(String endMessage, int ix) {
        tMes.setText(endMessage);
        tMes.setVisibility(View.VISIBLE);
        stopGame(ix);
        return ;
    }

    void stopGame(int levela) {
        bStop.setEnabled(false);
        aGame.stopGame();
        resetButtons(levela);
        showPlayersCards();
        aGame.destroyGame();
        java.lang.System.runFinalization();
        java.lang.System.gc();
        bMerge.setEnabled(true);
    }

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
            tPoints.setText("" + aGame.gambler.points);
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
            tPoints.setText("" + aGame.gambler.points);
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

    public void im0_Clicked(View arg0) {
        imageMouseEventHandler(arg0,0);
        tDbg.setText("");
        tDbg.append("Image0 Clicked\n");
        tDbg.append("-------------------------------------------------------------------------\n");
    }

    public void im1_Clicked(View arg0) {
        imageMouseEventHandler(arg0,1);
        tDbg.setText("");
        tDbg.append("Image1 Clicked\n");
        tDbg.append("-------------------------------------------------------------------------\n");
    }

    public void im2_Clicked(View arg0) {
        imageMouseEventHandler(arg0,2);
        tDbg.setText("");
        tDbg.append("Image2 Clicked\n");
        tDbg.append("-------------------------------------------------------------------------\n");
    }

    public void im3_Clicked(View arg0) {
        imageMouseEventHandler(arg0,3);
        tDbg.setText("");
        tDbg.append("Image3 Clicked\n");
        tDbg.append("-------------------------------------------------------------------------\n");
    }

    public void im4_Clicked(View arg0) {
        imageMouseEventHandler(arg0,4);
        tDbg.setText("");
        tDbg.append("Image4 Clicked\n");
        tDbg.append("-------------------------------------------------------------------------\n");
    }

    public void imAtou_Clicked(View arg0) {
        imageMouseEventHandler(arg0,10);
        tDbg.setText("");
        tDbg.append("Image4 Clicked\n");
        tDbg.append("-------------------------------------------------------------------------\n");
    }

    void imageMouseEventHandler(View arg0, int ic) {

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

            // imOut0.setImage(aGame.gambler.hand[ic].getImage());
            imOut0.setImageResource(aGame.gambler.hand[ic].getResInt());

        } catch (Exception e) {
            this.errHandler(e);
        }
        aGame.gambler.hand[ic] = emptyTmpCard;
        ready = false;
        endTurn();

    }


    public void bHelp_Clicked(View arg0) {
        tDbg.append("-------------------------------------------------------------------------\n");
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