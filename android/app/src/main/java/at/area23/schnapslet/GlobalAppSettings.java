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
import android.content.Context;
import android.net.Uri;

import java.util.Locale;

import at.area23.schnapslet.constenum.Constants;
import at.area23.schnapslet.models.*;
import at.area23.schnapslet.constenum.DIALOGS;

public class GlobalAppSettings extends Application {

    private static Application sApplication;

    private boolean soundOn = true;
    private String pictureUrl = "https://area23.at/schnapsen/cardpics/";
    private Uri pictureUri = null;
    private Locale systemLocale, locale;
    private DIALOGS dialogOpened = DIALOGS.None;
    private Card emptyCard = null;
    private Card noneCard = null;
    private Tournament tournament = null;
    private Game game = null;
    private Context context;

    /**
     * Called when the application is starting, before any activity, service,
     * or receiver objects (excluding content providers) have been created.
     */
    @Override
    public void onCreate() {
        super.onCreate();
        sApplication = this;
    }

    //region ApplicationContext
    public static Application getApplication() {
        return sApplication;
    }

    @Override
    public Context createDeviceProtectedStorageContext() {
        return super.createDeviceProtectedStorageContext();
    }

    public static Context getContext() {
        return getApplication().getApplicationContext();
    }
    //endregion

    //region LocaleLanguage
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

    public Locale getSystemLLocale() {
        initLocale();
        return systemLocale;
    }

    public String getLocaleString() {
        return getLocale().getDisplayName();
    }

    public String getLocaleLanguage() {
        return getLocale().getLanguage();
    }

    public void setLocale(Locale setLocale) {
        locale = setLocale;
    }

    public void setLocale(String localeString) {
        locale = new Locale(localeString);
    }
    //endregion

    //region pictureUrl
    public void initPictureUrl() {
        try {
            if (pictureUrl == null || pictureUrl.length() < 1) {
                pictureUrl = Constants.URL_PREFIX;
            }
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

    public void setPictureUri(String baseUri) {
        try {
            this.pictureUri = Uri.parse(baseUri);
            this.pictureUrl = baseUri;
        } catch (Exception exi) {
            exi.printStackTrace();
        }
    }
    //endregion

    public Game getGame() { return game; }

    public void setGame(Game aGame) { game = aGame;}

    public Tournament getTournament() {
        if (tournament == null)
            tournament = new Tournament(getContext());
        return tournament;
    }
    /**
     * setTournamentGame sets current tournament & game to statefull GlobalAppSettings
     * @param aTournament Tournament current tournament
     * @param aGame Game current game
     */
    public void setTournamentGame(Tournament aTournament, Game aGame) {
        tournament = aTournament;
        game = aGame;
    }
    public Card cardEmpty() {
        if (emptyCard == null)
            emptyCard = new Card(-2, getApplication().getApplicationContext());
        return emptyCard;
    }

    public Card cardNone() {
        if (noneCard == null)
            noneCard = new Card(-1, getApplication().getApplicationContext());
        return noneCard;
    }

    public DIALOGS getDialog() { return dialogOpened; }

    public void setDialog(DIALOGS dia) { dialogOpened = dia; }

    public boolean getSound() { return soundOn; }
    public void setSound(boolean onOff) { soundOn = onOff; }

}
