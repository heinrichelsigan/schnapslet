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
package at.area23.heinrichelsigan.schnapslet;

import at.area23.heinrichelsigan.schnapslet.Card;
import java.lang.*;
import java.io.*;
import java.net.*;
// import java.awt.*;

public class Card {
    
	boolean atou = false;
    char color = 'n';   // 4 colors and 'n' for unitialized
    int value = 0;      // 5 values and 0 for unitialized
    int intern = -1;    // 20 values for internal representation and (-1) for unitialized
    CARDVALUE cardValue = CARDVALUE.NONE;
    CARDCOLOR cardColor = CARDCOLOR.NONE;
    java.lang.String name = new String();  // Human readable classifier
    java.net.URL picture;  // picture 
	//
	// java.applet.Applet masterApplet = null;


    /**
     * Constructor Card()
     */
    public Card() {
        super();
        // this.color = 'n';
        // this.value = 0;
        this.intern = -1;
        this.name = "nocard";
        cardValue = CARDVALUE.NONE;
        cardColor = CARDCOLOR.NONE;
    }
	

    /**
     * Constructor Card(int num)
     * @param num internal value -2 or -1 or between 0 and 19
     */
    public Card(int num) {
		this();
        String tmpstr, namestr = new String();

        if (num == -2) {
            this.cardColor = CARDCOLOR.EMPTY;
            this.cardValue = CARDVALUE.EMPTY;
        } else if (num == -1) {
            this.cardColor = CARDCOLOR.NONE;
            this.cardValue = CARDVALUE.NONE;
        } else  if ((num >= 0) && (num < 20)) {

            if (num >= 0 && num < 5)
                this.cardColor = CARDCOLOR.HERZ;
            else if (num < 10)
                this.cardColor = CARDCOLOR.PIK;
            else if (num < 15)
                this.cardColor = CARDCOLOR.KARO;
            else if (num < 20)
                this.cardColor = CARDCOLOR.TREFF;

            switch (num % 5) {
                case 0:
                    this.cardValue = CARDVALUE.JACK;
                    break;
                case 1:
                    this.cardValue = CARDVALUE.QUEEN;
                    break;
                case 2:
                    this.cardValue = CARDVALUE.KING;
                    break;
                case 3:
                    this.cardValue = CARDVALUE.TEN;
                    break;
                case 4:
                    this.cardValue = CARDVALUE.ACE;
                    break;
                default: // never be here break;
                    break;
            }
        }

        this.intern = num;
        this.value = (int)cardValue.getValue();
        this.color = (char)cardColor.getValue();

        namestr = cardColor.toString() + "_" + cardValue.getName();
        this.name = namestr;
        System.err.println(namestr);

        /*
		try {
        	tmpstr = new String("cardpics/" + this.color + this.value + ".gif");
			// this.cardimage = new Image();
			// this.cardimage = JarIncludedImage(tmpstr);
		} catch (java.lang.Exception ex) {
			ex.printStackTrace();
			System.err.println(ex.toString()); 
		}
		*/

		try {
        	tmpstr = new String("http://www.area23.at/" + "cardpics/" + this.color + this.value + ".gif");
            System.err.println(tmpstr);
        	// System.err.println(tmpstr);
			this.picture = new java.net.URL(tmpstr);
		} catch (Exception exi) {
			exi.printStackTrace();
            System.err.println(exi.toString());
        }
		
        return;
    }	
	



    /**
     * Constructor Card(int numv, char atoudef)
     * @param numv internal value -2 or -1 or between 0 and 19
     * @param atoudef color of atou
     */
	public Card(int numv, char atoudef) {
        this(numv);
        if (this.color == atoudef) {
            this.atou = true;
		}
    }



    /**
     * Constructor Card(Card aCard)
     * @param aCard - a instanciated Card object
     */
    public Card(Card aCard) {
        this();
        this.atou = aCard.atou;
        this.color = aCard.color;
        this.value = aCard.value;
        this.intern = aCard.intern;
        this.name = aCard.name;
        this.picture = aCard.picture;
        this.cardValue = aCard.cardValue;
        this.cardColor = aCard.cardColor;
		// this.cardimage = aCard.cardimage;
    }
        
    /**
     * Liefert den menschlichen Bezeichner der Karte
     */
    public String getName() {
        return this.name;
    }
 
    /**
     * liefert die Farbe der Karte
     */
    public char getColor() {
        return (char)cardColor.getValue();
    }

	public void setAtou() {
		this.atou = true;
	}

    /**
     * liefert den Punkt Wert der Karte
     */
    public int getValue() {
        return (int)cardValue.getValue();
    }
    
	/**
     * liefert die URL des Kartensymbols
     */
    public java.net.URL getPictureUrl() {
        //return (picture);
		URL xy = null; 
		try {
			xy = new URL("http://www.area23.at/cardpics/" + this.color + this.value + ".gif");
		} catch (Exception e) { }
		return xy;
    }

    public android.net.Uri getPictureUri() {
        //return (picture);
        android.net.Uri xx = null;
        try {
            String myUri = "http://www.area23.at/cardpics/" + this.color + this.value + ".gif";
            xx = android.net.Uri.parse(myUri);
        } catch (Exception e) { }
        return xx;
    }

    public int getResInt() {
        String tmp = this.color + String.valueOf(this.value);
        if (tmp.equals("k2"))
            return R.drawable.k2;
        if (tmp.equals("k3"))
            return R.drawable.k3;
        if (tmp.equals("k4"))
            return R.drawable.k4;
        if (tmp.equals("k10"))
            return R.drawable.k10;
        if (tmp.equals("k11"))
            return R.drawable.k11;
        if (tmp.equals("p2"))
            return R.drawable.p2;
        if (tmp.equals("p3"))
            return R.drawable.p3;
        if (tmp.equals("p4"))
            return R.drawable.p4;
        if (tmp.equals("p10"))
            return R.drawable.p10;
        if (tmp.equals("p11"))
            return R.drawable.p11;
        if (tmp.equals("t2"))
            return R.drawable.t2;
        if (tmp.equals("t3"))
            return R.drawable.t3;
        if (tmp.equals("t4"))
            return R.drawable.t4;
        if (tmp.equals("t10"))
            return R.drawable.t10;
        if (tmp.equals("t11"))
            return R.drawable.t11;
        if (tmp.equals("h2"))
            return R.drawable.h2;
        if (tmp.equals("h3"))
            return R.drawable.h3;
        if (tmp.equals("h4"))
            return R.drawable.h4;
        if (tmp.equals("h10"))
            return R.drawable.h10;
        if (tmp.equals("h11"))
            return R.drawable.h11;

        if (tmp.equals("e1") || tmp.equals("e0"))
            return R.drawable.h11;

        return R.drawable.n0;
    }

	/**
     * liefert das Image des Kartensymbols

	public java.awt.Image getImage() {
		if (this.cardimage != null) {
			return this.cardimage;
		}
		
		java.awt.Image img1 = null;
		String imgstr = new String("cardpics/" + this.color + this.value + ".gif");
		try {
			InputStream is = getClass().getResourceAsStream(imgstr);
			BufferedInputStream bis = new BufferedInputStream(is);
			// a buffer large enough for our image can be byte[] byBuf = = new byte[is.available()];
			byte[] byBuf = new byte[10000]; // is.read(byBuf);  or something like that... 
			int byteRead = bis.read(byBuf,0,10000);
			img1 = Toolkit.getDefaultToolkit().createImage(byBuf);
		} catch(Exception e) {
			e.printStackTrace();
		}
		return img1;
	}  
	     */
    
    /**
     * Zeigt an, ob es sich bei dieser Karte um ein Atou handelt.
     */
    public boolean isAtou() {
        return this.atou;
    }
    
    /**
     * Zeigt an, ob es sich bei der Karte um eine regulaere Karte handelt.
     */
    public boolean isValidCard() {
        char c1 = this.color;
        int v1 = this.value;
        int i1 = this.intern;
        if ((i1 < 0) || (i1 >= 20)) return false;
        if ((c1 == 'h') || (c1 == 'p') || (c1 == 't') || (c1 == 'k')) {
            if (((v1 >= 2) && (v1 <= 4)) || (v1 == 10) || (v1 == 11)) {
                return true;
			}
        }
        return false;
    }    

    /**
     * gibt an ob diese Karte ohne Kontext des Spiels eine andere schlaegt
     * 
     * @param otherCard
     */
    public boolean hitsValue(Card otherCard) {
        if (this.color == otherCard.color) {
            if (this.getValue() > otherCard.getValue()) 
                return true;
        }
        return false;
    }
        

    /**
     * gibt an ob diese Karte die ueergebene Karte otherCard schlaegt
     * 
     * otherCard die andere Karte
     * active Der Flag active gibt an, welche Karte zuerst ausgespielt wurder.
     * Wurde die Karte selbst zuerst ausgespielt, so wird active true uebergeben.
     */
    public boolean hitsCard(Card otherCard, boolean active) {
        if (this.color == otherCard.color) {
            if (this.getValue() > otherCard.getValue()) 
                return true;
            else return false;
        }
        if ((this.isAtou()) && (otherCard.isAtou() == false)) {
			return true;
		}
        if ((this.isAtou() == false) && (otherCard.isAtou())) {
			return false;
		}
        return active;
    }
 
	/*
	public java.awt.Image JarIncludedImage(String imgstr) {
		java.awt.Image img1 = null;
		try {
			InputStream is = getClass().getResourceAsStream(imgstr);
			BufferedInputStream bis = new BufferedInputStream(is);
			// a buffer large enough for our image can be byte[] byBuf = = new byte[is.available()];
			byte[] byBuf = new byte[10000]; // is.read(byBuf);  or something like that... 
			int byteRead = bis.read(byBuf,0,10000);
			img1 = Toolkit.getDefaultToolkit().createImage(byBuf);
		} catch(Exception e) {
			e.printStackTrace();
		}
		return img1;
	} 
	*/
   
}
