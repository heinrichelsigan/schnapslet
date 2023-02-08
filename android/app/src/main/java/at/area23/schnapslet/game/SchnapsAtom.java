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

import java.net.URL;
import java.util.Locale;

/**
 * Card class represents a playing card in Game.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class SchnapsAtom {

    protected java.lang.String name = new String();  // Human readable classifier
    protected final String prefixUri = "http://www.area23.at/cardpics/";

	protected Locale locale;
	public boolean localeChanged = false;
   
    protected Resources r;
    protected Context context;
    

    /**
     * Constructor Card()
     */
    public SchnapsAtom() {
        super();        
        this.name = "ctorSchnapsAtom";
    }

    /**
     *  Constructor Card
     * @param c us context of android app
     */
    public SchnapsAtom(Context c) {
        this();
        this.context = c;
        r = c.getResources();
        initLocale();
    }

    /**
     * Constructor of Card
     * @param lcl new locale to set
     * @param c us context of android app
     */
    public SchnapsAtom(Locale lcl, Context c) {
        this();
        this.context = c;
        r = c.getResources();
        locale = lcl;
    }


    /**
     * initLocale
     */
    public void initLocale() {
        if (locale == null) {
            try {
                locale = context.getResources().getConfiguration().getLocales().get(0);
            } catch (Exception e) {
                locale = new Locale("en");
            }
        }
    }

    /**
     * getLocale
     * @return get current locale
     */
    public Locale getLocale() {
        initLocale();
        return locale;
    }

    /**
     *  setLocale set current locale
     * @param loc Locale
     */
	public void setLocale(Locale loc) {
        locale = loc;
        localeChanged = true;
    }


    /**
     * getLocaleString
     * @return getDisplayName() from current locale
     */
    public String getLocaleString() {
        return getLocale().getDisplayName();
    }
	
    /**
     * setLocale set current locale
     * @param locStr string representing new locale
     */	
	public void setLocaleString(String locStr) {
        setLocale(new Locale(locStr));
    }

    /**
     * getLocaleLanguage
     * @return current language from locale
     */
    public String getLocaleLanguage() {
        return getLocale().getLanguage();
    }

    /**
     * getPictureUrl
     * @return a picture URL to ab image in !WWW
     */
    public java.net.URL getPictureUrl(char colr, int val) {
		URL url = null;
		try {
            url = new URL(prefixUri + colr + val + ".gif");
		} catch (Exception exi) {
            exi.printStackTrace();
            // System.err.println(exi.toString());
        }
		return url;
    }

    /**
     * getPictureUri
     * @return va picture Uri to ab image in !WWW
     */
    public android.net.Uri getPictureUri(char colr, int val) {
        android.net.Uri uri = null;
        try {
            String myUri =  prefixUri + colr + val + ".gif";
            uri = android.net.Uri.parse(myUri);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return uri;
    }

}
