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

import at.area23.*;
import at.area23.schnapslet.*;
import java.lang.*;
import java.io.*;
import java.net.*;
import java.awt.*;

class card {
    
	boolean atou = false;
    char color = 'n';   // 4 colors and 'n' for unitialized
    int value = 0;      // 5 values and 0 for unitialized
    int intern = -1;    // 20 values for internal representation and (-1) for unitialized
    java.lang.String name = new String();  // Human readable classifier
    java.net.URL picture;  // picture 
	java.awt.Image cardimage = null;
	java.applet.Applet masterApplet = null;
	
    
    /**
     */
    public card() {
        super();
        this.color = 'n';
        this.value = 0;
        this.intern = -1;
        this.name = "nocard";        
    }
	
	public card(java.applet.Applet applet) {
        this();
        this.masterApplet = applet;   
    }
    

	
    public card(int num) {
		this();
        String tmpstr, namestr = new String();

        if ((num >= 0) && (num < 20)) {
            if (num < 5) { 
                this.color = 'h'; namestr = namestr + "Herz_";
            } else if (num < 10) {
                this.color = 'p'; namestr = namestr + "Pik_";
            } else if (num < 15) {
                this.color = 'k'; namestr = namestr + "Karo_";
            } else if (num < 20) {
                this.color = 't'; namestr = namestr + "Treff_";
            }
            switch (num%5) {
                case 0: this.value = 2;  namestr=namestr+"Jack"; break;
                case 1: this.value = 3;  namestr=namestr+"Queen"; break;
                case 2: this.value = 4;  namestr=namestr+"King"; break;
                case 3: this.value = 10; namestr=namestr+"Ten"; break;
                case 4: this.value = 11; namestr=namestr+"Ace"; break;            
                default: // never be here break;
            }
            this.intern = num;
            this.name = namestr;
        } 
        
		try {
        	tmpstr = new String("cardpics/" + this.color + this.value + ".gif");
			// this.cardimage = new Image();
			this.cardimage = JarIncludedImage(tmpstr);
		} catch (java.lang.Exception ex) {
			ex.printStackTrace();
			System.err.println(ex.toString()); 
		}
		
		try {
        	tmpstr = new String("https://area23.at/schnapsen/cardpics/" + this.color + this.value + ".gif");
         	System.err.println(tmpstr); 
			this.picture = new java.net.URL(tmpstr);
        } catch (java.net.MalformedURLException murle) { 
			System.err.println(murle); 
		} catch (Exception exi) {
			exi.printStackTrace();
		}		
		
        return;
    }	
	
    public card(java.applet.Applet applet, int num) {
        this(num);
		this.masterApplet = applet;   
        
        return;
    }

    public card(java.applet.Applet applet, int numv, char atoudef) {
        this(applet, numv);
        if (this.color == atoudef) {
            this.atou = true;
		}
    }

    public card(card aCard) {
        this();
        this.atou = aCard.atou;
        this.color = aCard.color;
        this.value = aCard.value;
        this.intern = aCard.intern;
        this.name = aCard.name;
        this.picture = aCard.picture;
		this.cardimage = aCard.cardimage;
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
        return this.color;
    }

	public void setAtou() {
		this.atou = true;
	}

    /**
     * liefert den Punkt Wert der Karte
     */
    public int getValue() {
        return this.value;
    }
    
	/**
     * liefert die URL des Kartensymbols
     */
    public java.net.URL getPictureUrl() {
        //return (picture);
		URL xy = null; 
		try {
			xy = new URL("https://area23.at/schnapsen/cardpics/" + this.color + this.value + ".gif");
		} catch (Exception e) { }
		return xy;
    }
	
	/**
     * liefert das Image des Kartensymbols
     */
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
    public boolean hitsValue(card otherCard) {
        if (this.color == otherCard.color) {
            if (this.getValue() > otherCard.getValue()) 
                return true;
        }
        return false;
    }
        

    /**
     * gibt an ob diese Karte die uebergebene Karte otherCard schlaegt
     * 
     * @param otherCard die andere Karte
     * @param active Der Flag active gibt an, welche Karte zuerst ausgespielt wurder.
     * Wurde die Karte selbst zuerst ausgespielt, so wird active true uebergeben.
     */
    public boolean hitsCard(card otherCard, boolean active) {
        if (this.color == otherCard.color) {
            if (this.getValue() > otherCard.getValue()) 
                return true;
            else return false;
        }
        if ((this.isAtou()) && (otherCard.isAtou()==false)) {
			return true;
		}
        if ((this.isAtou()==false) && (otherCard.isAtou())) {
			return false;
		}
        return active;
    }
 
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
   
}
