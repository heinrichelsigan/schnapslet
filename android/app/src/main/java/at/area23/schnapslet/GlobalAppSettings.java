/*
 *
 * @author           Heinrich Elsigan
 * @version          V 1.3.4
 * @since            JDK 1.2.1
 *
 */
/*
   Copyright (C) 2000 - 2021 Heinrich Elsigan

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

public class GlobalAppSettings extends Application {
    private Locale locale;
    private String prefixUri = "http://www.area23.at/cardpics/";
    private Uri pictureUri = null;
    private boolean appSetsChanged = false;
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
        appSetsChanged = true;
    }

    @Override
    public Context createDeviceProtectedStorageContext() {
        return super.createDeviceProtectedStorageContext();
    }

    public void setLocale(Locale setLocale) {
        locale = setLocale;
        appSetsChanged = true;
    }

    public void setLocale(String localeString) {
        setLocale(new Locale(localeString));
    }

    public void initLocale() {
        if (locale == null) {
            try {
                locale = getApplicationContext().getResources().getConfiguration().getLocales().get(0);
            } catch (Exception e) {
                locale = new Locale("en");
            }
        }
    }

    public Locale getLocale() {
        initLocale();
        return locale;
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
            this.prefixUri = baseUri;
            appSetsChanged = true;
        } catch (Exception exi) {
            exi.printStackTrace();
        }
    }

    public void initPictureUrl() {
        try {
            if (pictureUri == null)
                pictureUri = Uri.parse(prefixUri);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public String getPictureUrlPrefix() {
        initPictureUrl();
        return this.prefixUri;
    }

    public Uri getPictureUri() {
        initPictureUrl();
        return this.pictureUri;
    }

    /**
     * hasChanged - GlobalAppSettings has changed
     * @return true in case of uncommitted change on GlobalAppSettings, otherwise false
     */
    public boolean hasChanged() {
        if (appSetsChanged) {
            appSetsChanged = false;
            return true;
        }
        return false;
    }
}