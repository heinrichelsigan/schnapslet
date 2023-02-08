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
package at.area23.schnapslet;

import android.app.Application;
import android.content.ContentProvider;
import android.content.Context;
import android.net.Uri;
import android.os.Build;
import java.util.Locale;

public class GlobalAppSettings extends Application {
    private Locale systemLocale, locale;
    private final String prefixUrl = "https://area23.at/schnapsen/";
	private String pictureUrl = "https://area23.at/schnapsen/cardpics/";
	private Uri prefixUri = null;
    private Uri pictureUri = null;
    private Game game = null;
    Context context;

    private static Application sApplication;

    public static Application getApplication() {
        return sApplication;
    }

    public static Context getContext() {
        return getApplication().getApplicationContext();
    }

    /**
     * Called when the application is starting, before any activity, service,
     * or receiver objects (excluding content providers) have been created.
     */
    @Override
    public void onCreate() {
        super.onCreate();
        sApplication = this;
    }

    @Override
    public Context createDeviceProtectedStorageContext() {
        return super.createDeviceProtectedStorageContext();
    }

    public void setLocale(Locale setLocale) {
        locale = setLocale;
    }

    public void setLocale(String localeString) {
        locale = new Locale(localeString);
    }

    public void initLocale() {
        if (systemLocale == null) {
            try {
                systemLocale = getApplicationContext().getResources().getConfiguration().getLocales().get(0);
            } catch (Exception e) {
                systemLocale = new Locale("en");
            }
        }
        if (locale == null) {
            try {
                locale = getApplicationContext().getResources().getConfiguration().getLocales().get(0);
            } catch (Exception e) {
                locale = new Locale(systemLocale.getLanguage());
            }
        }
    }

    public Locale getLocale() {
        initLocale();
        return locale;
    }

    public Locale geSystemLLocale() {
        initLocale();
        return systemLocale;
    }

    public String getLocaleString() {
        return getLocale().getDisplayName();
    }

    public String getLocaleLanguage() {
        return getLocale().getLanguage();
    }

    public void setPictureUri(String baseUri) {
        try {
            this.pictureUri = Uri.parse(baseUri);
            this.pictureUrl = baseUri;
        } catch (Exception exi) {
            exi.printStackTrace();
        }
    }
	
    public void initPrefixUrl() {
        try {
            if (prefixUri == null)
                prefixUri = Uri.parse(prefixUrl);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }	

    public void initPictureUrl() {
        try {
            if (pictureUri == null)
                pictureUri = Uri.parse(pictureUrl);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public String getPictureUrl() {
        initPictureUrl();
        return this.pictureUrl;
    }
	

    public Uri getPictureUri() {
        initPictureUrl();
        return this.pictureUri;
    }

	public String getPrefixUrl() {
        initPrefixUrl();
        return this.prefixUrl;
    }
	
	public Uri getPrefixUri() {
        initPrefixUrl();
        return this.prefixUri;
    }

    public Game getGame() {
        return game;
    }

    public void setGame(Game aGame) {
        game = aGame;
    }
}