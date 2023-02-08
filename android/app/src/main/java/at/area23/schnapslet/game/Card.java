/*
 *
 * @author           Heinrich Elsigan root@darkstar.work
 * @version          V 1.6.9
 * @since            API 26 Android Oreo 8.1
 *
 */
/*
   Copyright (C) 2000 - 2023 Heinrich Elsigan root@darkstar.work

   Schnapslet java applet is free software; you can redistribute it and/or
   modify it under the terms of the GNU Library General Public License as
   published by the Free Software Foundation; either version 2 of the
   License, or (at your option) any later version.
   See the GNU Library General Public License for more details.

*/
package at.area23.schnapslet.game;

import android.content.Context;
import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.Drawable;
import android.util.TypedValue;

import java.io.BufferedInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.util.Locale;

import at.area23.schnapslet.GlobalAppSettings;
import at.area23.schnapslet.constenum.CARDCOLOR;
import at.area23.schnapslet.constenum.CARDVALUE;

/**
 * Card class represents a playing card in Game.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class Card  extends SchnapsAtom {
    int intern = -1;    // 20 values for internal representation and (-1) for unitialized

    public CARDVALUE cardValue = CARDVALUE.NONE;
    public CARDCOLOR cardColor = CARDCOLOR.NONE;
    public boolean atou = false;
    public char color = 'n';   // 4 colors and 'n' for unitialized
    public int value = -1; // 5 values and 0 for unitialized
    // java.lang.String name = new String();  // Human readable classifier
    java.net.URL picture;  // picture

    // Resources r;
    // Context context;
    // Calling Application class (see application tag in AndroidManifest.xml)
    GlobalAppSettings globalVariable;
    // Locale globalAppVarLocale;

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
        initLocale();
        globalVariable = (GlobalAppSettings) c;
    }

    /**
     * Constructor Card(int num)
     * @param num internal value -2 or -1 or between 0 and 19
     */
    public Card(int num) {
		this();

        if (num == -2) {
            this.cardColor = CARDCOLOR.EMPTY;
            this.cardValue = CARDVALUE.EMPTY;
        } else if (num == -1) {
            this.cardColor = CARDCOLOR.NONE;
            this.cardValue = CARDVALUE.NONE;
        } else  if ((num >= 0) && (num < 20)) {

            if (num >= 0 && num < 5)
                this.cardColor = CARDCOLOR.HEARTS;
            else if (num < 10)
                this.cardColor = CARDCOLOR.SPADES;
            else if (num < 15)
                this.cardColor = CARDCOLOR.DIAMONDS;
            else if (num < 20)
                this.cardColor = CARDCOLOR.CLUBS;

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

        // TODO: Multilanguage
        this.name = cardColor.toString() + "_" + cardValue.getName();
        // System.err.println(namestr);
        this.picture = this.getPictureUrl(this.color, this.value);
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
        initLocale();
        globalVariable = (GlobalAppSettings) c;
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
     * @param c context of android app
     */
    public Card(int numv, char atoudef, Context c) {
        this(numv, c);
        if (this.color == atoudef) {
            this.atou = true;
        }
        globalVariable = (GlobalAppSettings) c;
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

        if (cardColor == CARDCOLOR.SPADES) num += 5;
        if (cardColor == CARDCOLOR.DIAMONDS) num += 10;
        if (cardColor == CARDCOLOR.CLUBS) num += 15;

        this.intern = num;
        this.color = (char)aCardColor.getValue();
        this.value = aCardValue.getValue();

        this.name = cardColor.toString() + "_" + cardValue.getName();
        this.picture = this.getPictureUrl(this.color, this.value);
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
        // translate here
        // java.util.Locale primaryLocale = context.getResources().getConfiguration().getLocales().get(0);
        // String locale = primaryLocale.getDisplayName();
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
        initLocale();
        globalVariable = (GlobalAppSettings) c;
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
        initLocale();
        globalVariable = (GlobalAppSettings) c;
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
        this.context = aCard.context;
        initLocale();
        globalVariable = (GlobalAppSettings) this.context;
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
        initLocale();
        globalVariable = (GlobalAppSettings) c;
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
     * setAtou() us to  set czrrebt card as atou
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
     * isAtou
     * @return true, uf current card is currently ab Atou in that game
     */
    public boolean isAtou() { return this.atou; }

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
     * @param otherCard the other card, which my card hits (or not)
     * @return true, if card hits value of otherCard
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
     * @param otherCard the other card, which my card hits (or not)
     * @param active is current card active card, if no clear rule for hitting otherCard
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


     /* getDrawableFromUrl
     * @return Drawable Bitmap on the net, that represents custom card deck from a prefix url
     */
    protected Drawable getDrawableFromUrl() {
        Bitmap bmp = null;
        try {
            HttpURLConnection connection = (HttpURLConnection) getPictureUrl(this.color, this.value).openConnection();
            connection.connect();
            InputStream input = connection.getInputStream();
            bmp = BitmapFactory.decodeStream(input);
        } catch (IOException e) {
            e.printStackTrace();
        }
        return new BitmapDrawable(context.getResources(), bmp);
    }

    /**
     * getResourcesInt
     * @return the RessourceID drom "drawable" as int for the soecific card
     */
    public int getResourcesInt() {
        String tmp = this.color + String.valueOf(this.value);

        if (this.cardColor == CARDCOLOR.EMPTY || tmp.startsWith("e") ||
                tmp.equals("e1") || tmp.equals("e0") || tmp.equals("e"))
            return at.area23.schnapslet.R.drawable.e1;

        if (this.cardColor == CARDCOLOR.NONE || tmp.startsWith("n") ||
                tmp.equals("n0") || tmp.equals("n"))
            return at.area23.schnapslet.R.drawable.n0;

        int drawableID = context.getResources().getIdentifier(
                tmp, "drawable", context.getPackageName());

        // Get menu set locale, that is global stored in app context
        if (!getLocaleString().equals(globalVariable.getLocale().getDisplayName())) {
            setLocale(globalVariable.getLocale());
        }

        locale = getLocale();
        String langLocaleString = getLocaleString();
        String langNoCntry = getLocaleLanguage();

        if (langNoCntry.equals((new Locale("en")).getLanguage()) ||
                langNoCntry.equals((new Locale("fr")).getLanguage()) ||
                langNoCntry.equals((new Locale("de")).getLanguage()) ||
                langNoCntry.equals((new Locale("pl")).getLanguage()) ||
                langNoCntry.equals((new Locale("uk")).getLanguage())) {
            // get language country region specific card deck card symbol
            int drawableLangId = context.getResources().getIdentifier(
                    langNoCntry + "_" + tmp,
                    "drawable", context.getPackageName());
            if (drawableLangId > 0)
                return drawableLangId;

        }
        return drawableID;
    }

    /**
     * getDrawable
     * @return Drawable, that contains card symbol e.g. for heart ace => R.drawable.h11
     */
    public Drawable getDrawable() {
        android.util.TypedValue typVal = new TypedValue();
        typVal.resourceId = this.getResourcesInt();
        Resources.Theme theme =  context.getResources().newTheme();

        return context.getResources().getDrawable(typVal.resourceId, theme);
    }


    /**
     * getBytes get bytes of drawable ressource
     * @return byte[]
     */
    public byte[] getBytes() {
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
