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
import android.content.res.Resources;
import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.drawable.Drawable;
import android.graphics.drawable.PictureDrawable;
import android.util.TypedValue;

import java.lang.*;
import java.io.*;
import java.net.*;

/**
 * Card class represents a playing card in Game.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class Card {
    int intern = -1;    // 20 values for internal representation and (-1) for unitialized
    CARDVALUE cardValue = CARDVALUE.NONE;
    CARDCOLOR cardColor = CARDCOLOR.NONE;
    boolean atou = false;
    char color = 'n';   // 4 colors and 'n' for unitialized
    int value = -1; // 5 values and 0 for unitialized
    java.lang.String name = new String();  // Human readable classifier
    java.net.URL picture;  // picture 

    Resources r;
    Context context;

    /**
     * Constructor Card()
     */
    public Card() {
        super();
        this.intern = -1;
        this.name = "nocard";
        cardValue = CARDVALUE.NONE;
        cardColor = CARDCOLOR.NONE;
        this.color = (char)cardColor.getValue();
        this.value = cardValue.getValue();
    }

    /**
     *  Constructor Card
     * @param c us context of android app
     */
    public Card(Context c) {
        this();
        this.context = c;
        r = c.getResources();
    }

    /**
     * Constructor Card(int num)
     * @param num internal value -2 or -1 or between 0 and 19
     */
    public Card(int num) {
		this();
        String tmpstr;

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

        this.name = cardColor.toString() + "_" + cardValue.getName();;
        // System.err.println(namestr);

		try {
        	tmpstr = new String("http://www.area23.at/" + "cardpics/" + this.color + this.value + ".gif");
			this.picture = new java.net.URL(tmpstr);
		} catch (Exception exi) {
			exi.printStackTrace();
            System.err.println(exi.toString());
        }
		
        return;
    }

    /**
     * Constructor of Card
     * @param num internal value -2 or -1 or between 0 and 19
     * @param c us context of android app
     */
    public Card(int num, Context c) {
        this(num);
        this.context = c;
        r = c.getResources();
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
     * Constructor Card
     * @param numv internal value -2 or -1 or between 0 and 19
     * @param atoudef color of atou
     * @param c us context of android app
     */
    public Card(int numv, char atoudef, Context c) {
        this(numv, c);
        if (this.color == atoudef) {
            this.atou = true;
        }
    }

    /**
     * another Constructor of Card
     * @param aCardColor the current Card Color
     * @param aCardValue the current Card Value
     */
    public Card(CARDCOLOR aCardColor, CARDVALUE aCardValue) {

        this.cardColor = aCardColor;
        this.cardValue = aCardValue;

        int num = -1;
        if (aCardColor == CARDCOLOR.EMPTY) num = -2;
        if (aCardValue == CARDVALUE.NONE) num = -1;
        else if (aCardValue == CARDVALUE.JACK) num = 0;
        else if (aCardValue == CARDVALUE.QUEEN) num = 1;
        else if (aCardValue == CARDVALUE.KING) num = 2;
        else if (aCardValue == CARDVALUE.TEN) num = 3;
        else if (aCardValue == CARDVALUE.ACE) num = 4;

        if (cardColor == CARDCOLOR.PIK) num += 5;
        if (cardColor == CARDCOLOR.KARO) num += 10;
        if (cardColor == CARDCOLOR.TREFF) num += 15;

        this.intern = num;
        this.color = (char)aCardColor.getValue();
        this.value = aCardValue.getValue();

        this.name = cardColor.toString() + "_" + cardValue.getName();

        try {
            this.picture = new java.net.URL(new String("http://www.area23.at/" + "cardpics/" + this.color + this.value + ".gif"));
        } catch (Exception exi) {
            exi.printStackTrace();
            System.err.println(exi.toString());
        }
    }

    /**
     * another Constructor of Card
     * @param aCardColor the current Card Color
     * @param aCardValue the current Card Value
     * @param atoudef char of atoude
     */
    public Card(CARDCOLOR aCardColor, CARDVALUE aCardValue, char atoudef) {
        this(aCardColor, aCardValue);
        if (this.color == atoudef) {
            this.atou = true;
        }
    }

    /**
     * Constructor of Card
     * @param aCardColor the current Card Color
     * @param aCardValue the current Card Value
     * @param atoudef char of atoudef
     * @param c us context of android app
     */
    public  Card(CARDCOLOR aCardColor, CARDVALUE aCardValue, char atoudef, Context c) {
        this(aCardColor, aCardValue, atoudef);
        this.context = c;
        this.r = c.getResources();
    }

    /**
     * another Constructor of Card
     * @param aCardColor the current Card Color
     * @param aCardValue the current Card Value
     * @param atouColor Card Color of Atou
     */
    public Card(CARDCOLOR aCardColor, CARDVALUE aCardValue, CARDCOLOR atouColor) {
        this(aCardColor, aCardValue);
        if (this.color == (char)atouColor.getValue()) {
            this.atou = true;
        }
    }

    /**
     * another Constructor of Card
     * @param aCardColor the current Card Color
     * @param aCardValue the current Card Value
     * @param atouColor Card Color of Atou
     * @param c is context of android app
     */
    public Card(CARDCOLOR aCardColor, CARDVALUE aCardValue, CARDCOLOR atouColor, Context c) {
        this(aCardColor, aCardValue, atouColor);
        this.context = c;
        this.r = c.getResources();
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
        this.r = aCard.r;
        // cardImage = aCard.cardImage;
    }

    /**
     * Constructor
     * @param aCard a instanciated Card object
     * @param c us context of android app
     */
    public Card(Card aCard, Context c) {
        this(aCard);
        this.context = c;
        r = c.getResources();
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

    /**
     * setAtou() us to  set czrrebt card as arou
     */
	public void setAtou() {
		this.atou = true;
	}

    /**
     * getValue
     * @return points value of current card
     */
    public int getValue() {
        return (int)cardValue.getValue();
    }

    /**
     * getPictureUrl
     * @return a picture URL to ab image in !WWW
     */
    public java.net.URL getPictureUrl() {
        //return (picture);
		URL xy = null; 
		try {
			xy = new URL("http://www.area23.at/cardpics/" + this.color + this.value + ".gif");
		} catch (Exception e) { }
		return xy;
    }

    /**
     * getPictureUri
     * @return va picture Uri to ab image in !WWW
     */
    public android.net.Uri getPictureUri() {
        //return (picture);
        android.net.Uri xx = null;
        try {
            String myUri = "http://www.area23.at/cardpics/" + this.color + this.value + ".gif";
            xx = android.net.Uri.parse(myUri);
        } catch (Exception e) { }
        return xx;
    }

    /**
     * getResourcesInt
     * @return the RessourceID drom "drawable" as int for tge soecific card
     */
    public int getResourcesInt() {
        String tmp = this.color + String.valueOf(this.value);

        int drawableID = context.getResources().getIdentifier(tmp, "drawable", context.getPackageName());

        if (tmp.equals("e1") || tmp.equals("e0") || tmp.equals("e"))
            return R.drawable.e1;

        if (tmp.equals("n0") || tmp.equals("n"))
            return R.drawable.n0;

        return drawableID;
    }

    /**
     * isAtou
     * @return true, uf current card is currently ab Atou in that game
     */
    public boolean isAtou() {
        return this.atou;
    }

    /**
     * isValidCard
     * @return true, if the current card is valid, false, if ut's av enory ir none reference
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
     * hitsValue
     * @param otherCard
     * @return truem, if card hits value of otherCard
     */
    public boolean hitsValue(Card otherCard) {
        if (this.color == otherCard.color) {
            if (this.getValue() > otherCard.getValue()) 
                return true;
        }
        return false;
    }

    /**
     * hitsCard
     * @param otherCard
     * @param active - us card active card, uf no clear rule for hitting otherCard,
     * @return true, if current card hits otherCard, false otherwise
     */
    public boolean hitsCard(Card otherCard, boolean active) {
        if (this.color == otherCard.color) {
            if (this.getValue() > otherCard.getValue())
            return true;
            else
                return false;
        }
        if ((this.isAtou()) && (!otherCard.isAtou())) {
			return true;
		}
        if ((!this.isAtou()) && (otherCard.isAtou())) {
			return false;
		}
        return active;
    }

    public Drawable getDrawable() {

        String tmp = this.color + String.valueOf(this.value);
        android.util.TypedValue typVak = new TypedValue();
        typVak.resourceId = this.getResourcesInt();
        Resources.Theme theme =  context.getResources().newTheme();
        Drawable drawvle = context.getResources().getDrawable(this.getResourcesInt(), theme);

        return drawvle;
    }

    public  byte[] getBytes() {
        byte[] byBuf = null;
        try {
            InputStream is = context.getResources().openRawResource(this.getResourcesInt());
            BufferedInputStream bis = new BufferedInputStream(is);
            // a buffer large enough for our image can be byte[] byBuf = = new byte[is.available()];
            byBuf = new byte[10000]; // is.read(byBuf);  or something like that...
            int byteRead = bis.read(byBuf,0,10000);

            return byBuf;
            // img1 = Toolkit.getDefaultToolkit().createImage(byBuf);

        } catch(Exception e) {
            e.printStackTrace();
        }
        return byBuf;
    }

}
