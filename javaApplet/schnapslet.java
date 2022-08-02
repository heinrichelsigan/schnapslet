/*
*
* @author           Heinrich Elsigan
* @version          V 0.2
* @since            JDK 1.2.1
*
*/
/*
   Copyright (C) 2000 - 2002 Heinrich Elsigan

   Schnapslet java applet is free software; you can redistribute it and/or
   modify it under the terms of the GNU Library General Public License as
   published by the Free Software Foundation; either version 2 of the
   License, or (at your option) any later version.
   See the GNU Library General Public License for more details.

*/
// package at.area23.schnapslet;

import at.area23.ImageViewer;
import at.area23.AlignStyle;
import at.area23.BevelStyle;
import at.area23.ColorUtils;
import at.area23.Context;
import at.area23.ErrorsBundle;
import at.area23.GeneralUtils;
import at.area23.GetFrame;
import at.area23.ImageViewer;
import at.area23.PropertyChangeSupport;
import at.area23.VetoableChangeSupport;
import at.area23.ResBundle;
import at.area23.ResBundle_de;
// import at.area23.schnapslet.*;
import java.awt.*;
import java.net.*;
import java.util.*;
import java.io.*;
import java.lang.*;
import java.applet.*;
import java.beans.*;

public class schnapslet extends Applet implements Runnable
{
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
	java.awt.Font schnapsFont;
    Thread t0;
    
	//{{DECLARE_CONTROLS
	java.awt.TextArea tDbg = new java.awt.TextArea();
	java.awt.Panel pHand = new java.awt.Panel();
	ImageViewer im1 = new ImageViewer();
	ImageViewer im0 = new ImageViewer();
	ImageViewer im2 = new ImageViewer();
	ImageViewer im3 = new ImageViewer();
	ImageViewer im4 = new ImageViewer();
	java.awt.Panel pGame = new java.awt.Panel();
	ImageViewer imOut0 = new ImageViewer();
	ImageViewer imOut1 = new ImageViewer();
	ImageViewer imTalon = new ImageViewer();
	ImageViewer imAtou = new ImageViewer();
	java.awt.Panel pAction = new java.awt.Panel();
	java.awt.Button bMerge = new java.awt.Button();
	java.awt.Button bStop = new java.awt.Button();
	java.awt.Button b20b = new java.awt.Button();
	java.awt.Button b20a = new java.awt.Button();
	java.awt.Button bChange = new java.awt.Button();
	java.awt.TextField tPoints = new java.awt.TextField();
	java.awt.Button bcont = new java.awt.Button();
	java.awt.Button pHelp = new java.awt.Button();
	java.awt.TextField tRest = new java.awt.TextField();
	java.awt.TextField tMes = new java.awt.TextField();
	java.awt.TextField tsMes = new java.awt.TextField();
	//}}
	
	protected void initURLBase() {
		try { 
			notURL =   new URL("http://www.area23.at/" + "cardpics/n0.gif");
		    emptyURL = new URL("http://www.area23.at/" + "cardpics/e.gif");
		    backURL =  new URL("http://www.area23.at/" + "cardpics/verdeckt.gif");
			// backURL =  new URL(this.getCodeBase() + "cardpics/verdeckt.gif");
		    talonURL = new URL("http://www.area23.at/" + "cardpics/t.gif");
		} catch (Exception error) { 
		}
	}
	
	 public static void main(String args[]) {
        Frame appletFrame = new Frame("schnapslet");
        appletFrame.setLayout(new GridLayout(1,1));
        appletFrame.setSize(504,432);
        appletFrame.setVisible(true);
        Applet myApplet = new schnapslet();
        appletFrame.add(myApplet);
        myApplet.init();
        myApplet.start();
 
        // issues getCodeBase
        // cannot find main
        // getImage
        // store high scores
    }
	
	public void init()
	{
		// Take out this line if you don't use symantec.itools.net.RelativeURL or symantec.itools.awt.util.StatusScroller
		// symantec.itools.lang.Context.setApplet(this);
		initURLBase();

		//{{INIT_CONTROLS
		setLayout(null);
		schnapsFont = new java.awt.Font("Dialog", Font.PLAIN, 9);
		setFont(schnapsFont);
		setSize(504,420);
		tDbg.setEditable(false);
		add(tDbg);
		tDbg.setBounds(0,312,504,108);
		tDbg.setFont(new Font("Dialog", Font.PLAIN, 10));
		pHand.setLayout(null);
		add(pHand);
		pHand.setBounds(0,132,384,108);
		
		try { 
			im0.setImage(setJarIncludedImage(notJarStr)); 
			im0.setImageURL(notURL);
		} catch (Exception im0ex) {
			System.err.println(im0ex.toString());
			im0ex.printStackTrace();
		}
		pHand.add(im0);
		im0.setBounds(12,12,72,96);		
		
		try {
			// im1.setImage(setJarIncludedImage(notJarStr)); 
			im1.setImageURL(notURL); 
		} catch (Exception im1ex) {
			System.err.println(im1ex.toString());
			im1ex.printStackTrace();
		}
		pHand.add(im1);
		im1.setBounds(84,12,72,96);

		try {
			// im2.setImage(setJarIncludedImage(notJarStr)); 
			im2.setImageURL(notURL); 
		} catch (Exception im2ex) {
			System.err.println(im2ex.toString());
			im2ex.printStackTrace();			
		}
		pHand.add(im2);
		im2.setBounds(156,12,72,96);
		
		try {
			// im3.setImage(setJarIncludedImage(notJarStr)); 
			im3.setImageURL(notURL); 
		} catch (Exception im3ex) {
			System.err.println(im3ex.toString());
			im3ex.printStackTrace();			
		}
		pHand.add(im3);
		im3.setBounds(228,12,72,96);
    
		try {
			// im4.setImage(setJarIncludedImage(notJarStr)); 
			im4.setImageURL(notURL); 
		} catch (Exception im4ex) {
			System.err.println(im4ex.toString());
			im4ex.printStackTrace();
		}
		pHand.add(im4);
		im4.setBounds(300,12,72,96);
		
		pGame.setLayout(null);
		add(pGame);
		pGame.setBounds(0,12,384,132);
		
		try {
			// imOut0.setImage(setJarIncludedImage(emptyJarStr)); 
			imOut0.setImageURL(emptyURL); 
			// imOut0.setImageURL(new java.net.URL(this.getCodeBase() + "cardpics/e.gif")); 
		} catch (Exception imOutEx) {
			System.err.println(imOutEx.toString());
			imOutEx.printStackTrace();			
		}	
		pGame.add(imOut0);
		imOut0.setBounds(96,24,72,96);
		
		try {
			// imOut1.setImage(setJarIncludedImage(emptyJarStr)); 
			imOut1.setImageURL(emptyURL); 
			// imOut1.setImageURL(new java.net.URL(this.getCodeBase() + "cardpics/e.gif")); 
		} catch (Exception imOutEx) {
			System.err.println(imOutEx.toString());
			imOutEx.printStackTrace();
		}
		pGame.add(imOut1);
		imOut1.setBounds(12,12,72,96);
		
		try {
			// imTalon.setImage(setJarIncludedImage(talonJarStr)); 
			imTalon.setImageURL(talonURL);
			// imTalon.setImageURL(new java.net.URL(this.getCodeBase() + "cardpics/talon.gif")); 
		} catch (Exception imTalonEx) {
			System.err.println(imTalonEx.toString());
			imTalonEx.printStackTrace();
		}
		imTalon.setVisible(true);			
		pGame.add(imTalon);
		imTalon.setBounds(204,24,96,72);
    
		try {
			// imAtou.setImage(setJarIncludedImage(notJarStr)); 
			imAtou.setImageURL(notURL); 
			//imAtou.setImageURL(new java.net.URL(this.getCodeBase() + "cardpics/n0.gif")); 
		} catch (Exception imAtouEx) {
			System.err.println(imAtouEx.toString());
			imAtouEx.printStackTrace();
		}
		pGame.add(imAtou);
		imAtou.setBounds(276,12,72,96);
		
		pAction.setLayout(null);
		add(pAction);
		pAction.setBounds(384,12,120,240);
		
		bMerge.setLabel("Start");
		pAction.add(bMerge);
		bMerge.setBackground(java.awt.Color.lightGray);
		bMerge.setBounds(12,12,96,24);
		bMerge.setFont(schnapsFont);
		
		bStop.setLabel("Ende");
		bStop.setEnabled(false);
		pAction.add(bStop);
		bStop.setBackground(java.awt.Color.lightGray);
		bStop.setBounds(12,36,96,24);
		bStop.setFont(schnapsFont);
		
		b20b.setLabel("40 Ansagen");
		pAction.add(b20b);
		b20b.setBackground(java.awt.Color.lightGray);
		b20b.setBounds(12,144,96,24);
        b20b.setFont(schnapsFont);
        b20b.setEnabled(false);
        
		b20a.setLabel("20 Ansagen");
		pAction.add(b20a);
		b20a.setBackground(java.awt.Color.lightGray);
		b20a.setBounds(12,120,96,24);
		b20a.setFont(schnapsFont);
		b20a.setEnabled(false);
		
		bChange.setLabel("Atou tauschen");
		bChange.setEnabled(false);
		pAction.add(bChange);
		bChange.setBackground(java.awt.Color.lightGray);
		bChange.setBounds(12,168,96,24);
		bChange.setFont(schnapsFont);
		
		tPoints.setEditable(false);
		tPoints.setText("0");
		// tPoints.setEnabled(false);
		tPoints.setFont(schnapsFont);
		pAction.add(tPoints);
		tPoints.setBounds(72,84,36,26);
		
		bcont.setLabel("WEITER");
		bcont.setEnabled(false);
		pAction.add(bcont);
		bcont.setBackground(java.awt.Color.lightGray);
		bcont.setBounds(12,204,96,24);
		bcont.setFont(schnapsFont);
		
		pHelp.setLabel("Hilfe");
		pAction.add(pHelp);
		pHelp.setBackground(java.awt.Color.lightGray);
		pHelp.setBounds(12,60,97,24);
		pHelp.setFont(schnapsFont);
		
		tRest.setEditable(false);
		tRest.setText("10");
		// tRest.setEnabled(false);
		pAction.add(tRest);
		tRest.setBounds(12,84,36,26);
		tRest.setFont(schnapsFont);
		
		tMes.setEditable(false);
		tMes.setText("Um ein neues Spiel zu beginnen klicken Sie auf Start");
		// tMes.setEnabled(false);
		// tMes.setEnabled(false);
		add(tMes);
		tMes.setFont(schnapsFont);
		tMes.setBounds(12,250,480,26);
		
		tsMes.setEditable(false);
		// tsMes.setEnabled(false);
		add(tsMes);
		tsMes.setBackground(java.awt.Color.white);
		tsMes.setForeground(java.awt.Color.red);
		tsMes.setFont(schnapsFont);
		tsMes.setBounds(12,276,480,28);
		tsMes.setVisible(false);
		//}}
		t0 = new Thread(this);
		t0.start();

		
		//{{REGISTER_LISTENERS
		SymAction lSymAction = new SymAction();
		bStop.addActionListener(lSymAction);
		bMerge.addActionListener(lSymAction);
		bChange.addActionListener(lSymAction);
		SymMouse aSymMouse = new SymMouse();
		im0.addMouseListener(aSymMouse);
		b20a.addActionListener(lSymAction);
		b20b.addActionListener(lSymAction);
		im1.addMouseListener(aSymMouse);
		im2.addMouseListener(aSymMouse);
		im3.addMouseListener(aSymMouse);
		im4.addMouseListener(aSymMouse);
		tMes.addMouseListener(aSymMouse);
		bcont.addActionListener(lSymAction);
		pHelp.addActionListener(lSymAction);
		imAtou.addMouseListener(aSymMouse);
		//}}
	}
	

	class SymAction implements java.awt.event.ActionListener {
		public void actionPerformed(java.awt.event.ActionEvent event) {
			Object object = event.getSource();
			if (object == bStop)
				bStop_ActionPerformed(event);
			else if (object == bMerge)
				bMerge_ActionPerformed(event);
			else if (object == bChange)
				bChange_ActionPerformed(event);
			else if (object == b20a)
				b20a_ActionPerformed(event);
			else if (object == b20b)
				b20b_ActionPerformed(event);
			else if (object == bcont)
				bcont_ActionPerformed(event);
			else if (object == pHelp)
				pHelp_ActionPerformed(event);
		}
	}
	

	
	//     try { im0.setImage(setJarIncludedImage("cardpics/n0.gif")); } catch (Exception e) {  System.err.println(e.toString()); }
	Image setJarIncludedImage(String imgstr) {
		Image img = null;
		try {
			InputStream is = getClass().getResourceAsStream(imgstr);
			BufferedInputStream bis = new BufferedInputStream(is);
			// a buffer large enough for our image can be byte[] byBuf = = new byte[is.available()];
			byte[] byBuf = new byte[10000];  // is.read(byBuf);  or something like that...
			int byteRead = bis.read(byBuf, 0, 10000);
			img = Toolkit.getDefaultToolkit().createImage(byBuf);
 	 	} catch(Exception e) {
			e.printStackTrace();
 		}
		return img;
	}

	void errHandler(java.lang.Throwable myErr) {
	    tDbg.append("\nCRITICAL ERROR #" + (++errNum));
	    tDbg.append("\nMessage: " + myErr.getMessage());
	    tDbg.append("\nString: " + myErr.toString());
	    tDbg.append("\nLmessage: "+ myErr.getLocalizedMessage() + "\n");
	    myErr.printStackTrace();
	}
	
	void resetButtons(int level) {
        if (level >= 0 ) {
            b20a.setLabel("20 Ansagen");
            b20a.setEnabled(false);
            b20b.setLabel("40 Ansagen");
            b20b.setEnabled(false);
            bChange.setEnabled(false);
        }
        if (level >= 1) {
            bcont.setEnabled(false);
	        if (imTalon.isVisible()==false)
	            imTalon.setVisible(true);  
            try {
 				// imTalon.setImage(setJarIncludedImage(talonJarStr)); 
                imTalon.setImageURL(talonURL);
				// imAtou.setImage(setJarIncludedImage(emptyJarStr)); 
				imAtou.setImageURL(emptyURL);
            } catch (java.beans.PropertyVetoException pvex) {
                this.errHandler(pvex);
            } catch (Exception ex) {
				this.errHandler(ex);
			}
        }
        if (level >= 2) {        
            try {
				// imOut0.setImage(setJarIncludedImage(emptyJarStr));
                imOut0.setImageURL(emptyURL);
				// imOut1.setImage(setJarIncludedImage(emptyJarStr));
                imOut1.setImageURL(emptyURL);
            } catch (java.beans.PropertyVetoException pvex) {
                this.errHandler(pvex);
            } catch (Exception ex) {
				this.errHandler(ex);
            }
        }
	}
	
	void bStop_ActionPerformed(java.awt.event.ActionEvent event) {	// to do: code goes here.
		bStop_ActionPerformed_Interaction1(event);
	}

	void bStop_ActionPerformed_Interaction1(java.awt.event.ActionEvent event) {
		try {
	        stopGame(2);
		} catch (Exception e) {
			this.errHandler(e);
		}
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

	void bMerge_ActionPerformed(java.awt.event.ActionEvent event) {	// to do: code goes here.
		bMerge_ActionPerformed_Interaction1(event);
	}
	
	
	void bMerge_ActionPerformed_Interaction1(java.awt.event.ActionEvent event) {
        startGame();
	}

    void startGame() {	/* Mischen */
        bMerge.setEnabled(false);
        runtime = java.lang.Runtime.getRuntime();
        aGame = null;
        runtime.runFinalization();        
        runtime.gc();

        aGame = new game(this);
        tsMes.setVisible(false);
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
			tsMes.setVisible(true);
			tsMes.setText("Kein Spiel gestartet!");
			return;
		}
		tsMes.setVisible(true);
		tsMes.setText("Spieler dreht zu !");
        
		try {
			// imTalon.setImage(setJarIncludedImage(emptyJarStr));
            imTalon.setImageURL(emptyURL);
            imTalon.setVisible(false);
        } catch (java.beans.PropertyVetoException jbpvex) {
            this.errHandler(jbpvex);
        }                   
		
		try {	
			// imAtou.setImage(setJarIncludedImage(backJarStr));
            imAtou.setImageURL(backURL);
        } catch (java.beans.PropertyVetoException jbpvex) {
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
    
    void tsEnds(String endMessage, int ix) {
        tsMes.setText(endMessage);
        tsMes.setVisible(true);
        stopGame(ix);
        return ;
    }
        
    
    void twentyEnough(boolean who) {
        int xj = 0;
        String andEnough = "20 und genug !";
        ready = false;
        if (who) {
            try {
                while(aGame.gambler.hand[xj++].color != aGame.said) ;
				while(aGame.gambler.hand[xj++].getValue() < 3) ;
				// imOut0.setImage(aGame.gambler.hand[xj-1].getImage());
                imOut0.setImageURL(aGame.gambler.hand[xj-1].getPictureUrl());
                // imOut1.setImage(aGame.gambler.hand[xj].getImage());
				imOut1.setImageURL(aGame.gambler.hand[xj].getPictureUrl());				
                if (aGame.said == aGame.atouInGame) {
					andEnough = "40 und genug !";
				}
            } catch (java.beans.PropertyVetoException jbpvex) {
                this.errHandler(jbpvex);
            }            
            tsEnds(new String(andEnough+" Sie haben gewonnen mit " + aGame.gambler.points + " Punkten !"), 1);       
        } else {
            try {
                while(aGame.computer.hand[xj++].color != aGame.csaid) ;
				while(aGame.computer.hand[xj++].getValue() < 3) ;
				// imOut0.setImage(aGame.computer.hand[xj-1].getImage());
                imOut0.setImageURL(aGame.computer.hand[xj-1].getPictureUrl());
				// imOut1.setImage(aGame.computer.hand[xj].getImage());				
				imOut1.setImageURL(aGame.computer.hand[xj].getPictureUrl());
                if (aGame.csaid == aGame.atouInGame) { 
					andEnough="40 und genug !";
				}
            } catch (java.beans.PropertyVetoException jbpvex) {
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
				// imOut1.setImage(aGame.computer.hand[ccard].getImage());
                imOut1.setImageURL(aGame.computer.hand[ccard].getPictureUrl());
            } catch (java.beans.PropertyVetoException jbpvex) {
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
				// imTalon.setImage(setJarIncludedImage(emptyJarStr)); 
                imTalon.setImageURL(emptyURL);
                imTalon.setVisible(false);
				
				// imAtou.setImage(setJarIncludedImage(emptyJarStr)); 
                imAtou.setImageURL(emptyURL);
            } catch (java.beans.PropertyVetoException jbpvex) {
                this.errHandler(jbpvex);
            }              
            tsMes.setVisible(true);
            tsMes.setText("Keine Karten im Talon -> Farb- und Stichzwang !");
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
        
    
    void gameTurn(int ixlevel) {
        if (ixlevel < 1) {
            try {
				// imOut0.setImage(setJarIncludedImage(emptyJarStr)); 
				imOut0.setImageURL(emptyURL);
				// imOut1.setImage(setJarIncludedImage(emptyJarStr)); 
                imOut1.setImageURL(emptyURL);
            } catch (java.beans.PropertyVetoException jbpvex) {
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
                b20a.setLabel(aGame.printColor(aGame.gambler.handpairs[0])+" ansagen");
                b20a.setEnabled(true);
                if (a20 > 1) {
                    b20b.setLabel(aGame.printColor(aGame.gambler.handpairs[1])+" ansagen");
                    b20b.setEnabled(true);
                } else { 
					b20b.setLabel("kein 2. Paar");
				}
            }
            // Info 
            tMes.setText("Zum Auspielen einfach auf die entsprechende Karte klicken");                
        } else { 
			/* COMPUTERS TURN IMPLEMENTIEREN */
            if (aGame.atouIsChangable(aGame.computer)) {
                aGame.changeAtou(aGame.computer);
			    this.showAtouCard();
			    tsMes.setVisible(true);
			    tsMes.setText("COMPUTER TAUSCHT ATOU AUS !!!");
			    aGame.mqueue.insert("Computer tauscht Atou aus !");
			}
            ccard = aGame.computerStarts();
            if (aGame.csaid != 'n') {
			    tsMes.setVisible(true);
			    tsMes.setText("COMPUTER SAGT PAAR IN " + aGame.printColor(aGame.csaid) + " AN !!!");                
                aGame.mqueue.insert("Computer sagt Paar in " + aGame.printColor(aGame.csaid) + " an !");                			    
                if (aGame.computer.points > 65) {
					twentyEnough(false);
				}
            }            
            try {
				imOut1.setImage(aGame.computer.hand[ccard].getImage());
                imOut1.setImageURL(aGame.computer.hand[ccard].getPictureUrl());
            } catch (java.beans.PropertyVetoException jbpvex) {
                this.errHandler(jbpvex);
            }
            tMes.setText("Zum Antworten einfach auf die entsprechende Karte klicken");
        }
        ready = true; 
        printMes();
	}
    
    void printMes() {
        tDbg.append(aGame.mqueue.fetch());   
    }
    
	void showPlayersCards() {

		try {
			im0.setImageURL(aGame.gambler.hand[0].getPictureUrl());
	        im1.setImageURL(aGame.gambler.hand[1].getPictureUrl());
	        im2.setImageURL(aGame.gambler.hand[2].getPictureUrl());
    	    im3.setImageURL(aGame.gambler.hand[3].getPictureUrl());
	        im4.setImageURL(aGame.gambler.hand[4].getPictureUrl());
	    } catch (java.beans.PropertyVetoException exp) {
             this.errHandler(exp);
	    }
	}
	
	void showTalonCard() {
		try {
			// imTalon.setImage(setJarIncludedImage(talonJarStr)); 
			imTalon.setImageURL(talonURL);
			// imTalon.setImageURL(new java.net.URL(this.getCodeBase() + "cardpics/talon.gif")); 
		} catch (Exception imTalonEx) {
			System.err.println(imTalonEx.toString());
			imTalonEx.printStackTrace();
		}
		imTalon.setVisible(true);	
	}
	
	void showAtouCard() {

	    try {
	        imAtou.setImageURL(aGame.set[19].getPictureUrl());
	    } catch (java.beans.PropertyVetoException exp) {
            this.errHandler(exp);
	    }
		
	}

	void bChange_ActionPerformed(java.awt.event.ActionEvent event) {	// to do: code goes here.
		bChange_ActionPerformed_Interaction1(event);
	}

	void bChange_ActionPerformed_Interaction1(java.awt.event.ActionEvent event) {
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

	class SymMouse extends java.awt.event.MouseAdapter {
		public void mouseClicked(java.awt.event.MouseEvent event) {
			Object object = event.getSource();
			if (object == im0) imageMouseEventHandler(event, 0);
			else if (object == im1) imageMouseEventHandler(event, 1);
			else if (object == im2) imageMouseEventHandler(event, 2);
			else if (object == im3)imageMouseEventHandler(event, 3);
			else if (object == im4) imageMouseEventHandler(event, 4);
			else if (object == imAtou) imageMouseEventHandler(event, 10);
		}
	}


	void b20a_ActionPerformed(java.awt.event.ActionEvent event) { 	// to do: code goes here.
		b20a_ActionPerformed_Interaction1(event);
	}

	void b20a_ActionPerformed_Interaction1(java.awt.event.ActionEvent event) {
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

	void b20b_ActionPerformed(java.awt.event.ActionEvent event) {
		b20b_ActionPerformed_Interaction1(event);
	}

	void b20b_ActionPerformed_Interaction1(java.awt.event.ActionEvent event) {
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
	
	void imageMouseEventHandler(java.awt.event.MouseEvent event, int ic) {
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
                aGame.mqueue.insert("Das ist keine gueltige Karte !");
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
                    aGame.mqueue.insert("Beste Karte waere: "+tmpint);
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
					// im0.setImage(setJarIncludedImage(emptyJarStr));  
					im0.setImageURL(emptyURL); 
					break;
			    case 1: 
					// im1.setImage(setJarIncludedImage(emptyJarStr)); 
					im1.setImageURL(emptyURL); 
					break;			    
			    case 2: 
					// im2.setImage(setJarIncludedImage(emptyJarStr));  
					im2.setImageURL(emptyURL); 
					break;			    
			    case 3: 
					// im3.setImage(setJarIncludedImage(emptyJarStr)); 
					im3.setImageURL(emptyURL); 
					break;			    
			    case 4: 
					// im4.setImage(setJarIncludedImage(emptyJarStr)); 
					im4.setImageURL(emptyURL); 
					break;			    
			    default: tDbg.append("Assertion !");
			}
			
			// imOut0.setImage(aGame.gambler.hand[ic].getImage());
			imOut0.setImageURL(aGame.gambler.hand[ic].getPictureUrl());
		
		} catch (Exception e) {
            this.errHandler(e);		    
    	}
		aGame.gambler.hand[ic] = emptyTmpCard;
		ready = false;
		endTurn();
	}
	
	public void run() {
	    
	}

	void bcont_ActionPerformed(java.awt.event.ActionEvent event) {
		bcont_ActionPerformed_Interaction1(event);
	}

	void bcont_ActionPerformed_Interaction1(java.awt.event.ActionEvent event) {
		try {
		  ready = true;
			bcont.setEnabled(false);
			tsMes.setVisible(false);
			gameTurn(0);
		} catch (Exception e) {
            this.errHandler(e);		    
		}
	}

	void pHelp_ActionPerformed(java.awt.event.ActionEvent event) {	 
		pHelp_ActionPerformed_Interaction1(event);
	}

	void pHelp_ActionPerformed_Interaction1(java.awt.event.ActionEvent event) {
		try {
		    tDbg.append("-------------------------------------------------------------------------\n");
		    tDbg.append("Schnapslet V 0.2 - Pre Alpha Release \n");
		    tDbg.append("Implementierung des Kartenspiel Schnapsen als einfaches java.awt.Applet\n");
		    tDbg.append("von Heinrich Elsigan (heinrich.elsigan@area23.at)\n\n");
		    tDbg.append("Funktionsweise:\n");
		    tDbg.append("Das Spiel ist so angelegt, dass man gegen den Computer spielt.\n");
		    tDbg.append("Ist man am Zug, so kann man eine Karte ausspielen, indem man auf das\n");
		    tDbg.append("Kartensymbol klickt. Andere Optionen, wie \"Atou austauschen\" oder \n");
		    tDbg.append("\"Ein Paar Ansagen\" sind ueber die Buttons links oben moeglich; diese\n");
		    tDbg.append("Optionen muessen gewaehlt werden, bevor man eine Karte auspielt !\n");
		    tDbg.append("Ist der Computer am Zug, so spielt dieser eine Karte aus und man selbst\n");
		    tDbg.append("kann dann durch Klick auf die eigenen Karten, stechen oder draufgeben!\n");
		    tDbg.append("Die Regeln entsprechen dem oesterreichischen Schnapsen, allerdings gibt\n");
		    tDbg.append("es bei der Implementierung des Farb- und Stichzwangs noch kleine Bugs!\n");
		    tDbg.append("Zudrehen ist implementiert. Man muss einfach auf die Atou Karte klicken.\n");
		    tDbg.append("Ideen, Vorschlaege, Verbesserungen werden gerne angenommen !\n");
            tDbg.append("-------------------------------------------------------------------------\n");		    
            
			aGame.gambler.points = 0;
            aGame.computer.points = 0;
            
		} catch (Exception e) {
		}
	}

}



