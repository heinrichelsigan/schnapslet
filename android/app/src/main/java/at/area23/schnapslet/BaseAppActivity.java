/*
*
* @author           Heinrich Elsigan
* @version          V 1.3.4
* @since            JDK 1.2.1
*

*/
/*
   Copyright (C) 2000 - 2018 Heinrich Elsigan

   Schnapslet java applet is free software; you can redistribute it and/or
   modify it under the terms of the GNU Library General Public License as
   published by the Free Software Foundation; either version 2 of the
   License, or (at your option) any later version.
   See the GNU Library General Public License for more details.

*/

package at.area23.schnapslet;

import android.content.ContentResolver;
import android.content.Context;
import android.content.Intent;
import android.content.res.AssetFileDescriptor;
import android.content.res.Resources;
import android.media.AudioAttributes;
import android.media.MediaPlayer;
import android.net.Uri;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;


import java.io.File;
import java.lang.reflect.Field;
import java.util.HashMap;
import java.util.Locale;
import java.util.concurrent.atomic.AtomicInteger;

import at.area23.schnapslet.constenum.SOUNDS;

/**
 * BaseAppActivity extends AppCompatActivity
 *
 * inherit all your Activity classes from that class
 * BaseAppActivity provides all lot of framework
 */
public class BaseAppActivity extends AppCompatActivity implements Runnable {

	volatile boolean endRecursion = false;
    protected volatile boolean started = false;
    protected volatile int startedTimes = 0;
    protected volatile int errNum = 0;
    protected volatile int startedDrag = 0;
    protected volatile int finishedDrop = 0;
    protected String tmp = "";
    protected String parentErrMsg = "";

    protected Menu myMenu;
    protected HashMap<Integer, android.view.View> viewMap;
    protected android.view.View rootView = null;
    // protected android.widget.TextView mSoundName = new android.widget.TextView();
    protected volatile boolean atomicSoundLock = false;

    public GlobalAppSettings globalVariable;
    protected volatile at.area23.schnapslet.constenum.SOUNDS sound2Play = SOUNDS.NONE;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        // RessourceViewHashMap(rootView, viewMap);
    }


    /**
     * hashMapViewRecursivley - adds recursivley all child views to a ressource hash map
	 * 	 	 
     * @param aView - current view in the hierarchy to be parsed
	 * @param viewHashMap - Hashmap, that contains ressource Ids als keys and views with children as values
	 * @param mapMsg - a full informational message, what views have been added to ressource view HashMap
	 * @param loopCnt - totally loop calls for that recursion method
	 * @param rDepth - recursion depth (maybe you neeed that to avoid stack overflows
	 * // TODO: at here CancellactinToken, that cancels full recursion, when 
	 * // operation takes more than a defined time interval or when recursive depth level is too hug
	 * 	 
	 * @return number of loop calls of tecursive method
     */
    protected int hashMapViewRecursivley(
        android.view.View aView,
        java.util.HashMap<Integer, android.view.View> viewHashMap,
        String mapMsg,
        AtomicInteger loopCnt,
		int rDepth
		) {

		int childCount = 0;
        int viewId = aView.getId();

        if (!(viewHashMap.containsKey(viewId))) {
            viewHashMap.put(viewId, aView);
            mapMsg += rDepth + "\t:" + "view(" + viewId + ") \t -> " + this.getAll4RId(viewId) + "\r\n";
        }

        if (aView instanceof ViewGroup) {

            ViewGroup viewGroup = (ViewGroup)aView;
            int childrenSize = viewGroup.getChildCount();

            for (childCount = 0; childCount < childrenSize; childCount++) {
			
                int childId = viewGroup.getChildAt(childCount).getId();
                View childView  = (View)(viewGroup.getChildAt(childCount));
				
                if (!(viewHashMap.containsKey(childId))) {
                    viewHashMap.put(childId, childView);
                    mapMsg += rDepth + "\t:" + "view(" + viewId + ") \t -> child(" + childId + ") \t -> " +
                            this.getAll4RId(viewId) + "\r\n";
                }
                loopCnt.set(loopCnt.incrementAndGet());
                int recCount = hashMapViewRecursivley(childView, viewHashMap, mapMsg, loopCnt, ++rDepth);
            }
        }

        return loopCnt.intValue();
    }


    /**
     * InitViewHashMap - init a Hashmap, containing ressource Id as key, view as value for all view groups in current application with children
     * @param rootView - the root view of curremt window, current activity
     * @param viewHashMap - HashMap(unique ressource Id => android view with childrem)
     *
	 * @return false to allow normal menu processing to proceed, true to consume it here.
     */
    protected int RessourceViewHashMap(
        android.view.View rootView,
		java.util.HashMap<Integer, android.view.View> viewHashMap
		) {
        
		String mapMsg = "";
		
		if (viewHashMap == null) { // init new HashMap, when null and not initialized 
			viewHashMap = new java.util.HashMap<Integer, android.view.View>();
			mapMsg += "Initializing new viewMap ctor.\r\n";
		} else { // Clear Ressource Hash Map first to avoid duplicated entries in HashMap, when method is called more than once
			mapMsg += "Clearing existing viewMap with " + viewHashMap.size() + " entries.\r\n";
			viewHashMap.clear();
        }		
		if (rootView == null) {
			mapMsg += "rootView is null, ";
			rootView = getWindow().getDecorView().getRootView();
			mapMsg += "getting rootView from decor view of current window.\r\n";
        }

        AtomicInteger loops = new AtomicInteger();
        loops.set(0);

        int runnedCycles = hashMapViewRecursivley(rootView, viewHashMap, mapMsg, loops, 0);
		if (runnedCycles != 0 && viewHashMap.size() > 0) {
			mapMsg = "HashMap builded with " + viewHashMap.size() + " R.Ids as keys after running " + runnedCycles + " total cycles.\r\n" + mapMsg;
			showMessage(mapMsg, false);
		} else {
			mapMsg += "HashMap not builed after running " + runnedCycles + " cycles.\r\n";
			showMessage(mapMsg, true);
		}
		
		return runnedCycles;
    }


    /**
     * getRNameFromId
     * @param id the ressource Identifier of an graphical view element (android.view)
     * @param objectById referebce to the created instance at runtime
     * @return R.id reflected name
     */
    public String getRNameFromId(int id, Object objectById) {
        // objectById = null;
        Class clss = null;
        try {
            clss = Class.forName(getPackageName() + ".R$id");
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        }

        Class<R.id> c = R.id.class;

        android.R.id rObj = new android.R.id();
        // id object = new id();
        // R.id object = new R.id()
        Field[] fields;
        try {
            fields =  clss.getDeclaredFields();

            for (Field field : fields) { // Iterate through whatever fields R.id has
                field.setAccessible(true);
                String reflectedName = field.getName();
                String reflectedString = reflectedName;
                int resId = 0;
                try {
                    resId = (int) field.get(null);
                    reflectedString = this.getResources().getString(resId);
                } catch (NullPointerException ne) {
                    showError(ne, false);
                }
                catch (Exception e) {
                    showError(e, false);
                }

                try {
                    objectById = (Object) field.get(rObj);
                } catch (IllegalAccessException e) {
                    showError(e,false);
                }
                /*
                int nyRUd = getApplicationContext().getResources().getIdentifier(
                    reflectedName, "id", getApplicationContext().getPackageName());
                */
                if (id == resId) {
                    return reflectedName;
                }
            }
        } catch (Exception exf) {
            showError(exf, false);
        }
        return null;
    }


    /**
     * getAll4RId - get all names, types, meta-data, xml for an existing ressource Id
     * @param rId - the ressource Id
     * @return - combined string information for that ressource Id
     */
    public String getAll4RId(int rId) {
        if (rId == -1) {
            return "\r\nrId = -1 detected";
        }
        String allStr = "";
        try {
            allStr += "\r\ngetString(" + rId + ") = " + getApplicationContext().getResources().getString(rId) + "\r\n";
        } catch (Exception exi) { showError(exi, false); }
        try {
            allStr += "\r\nResource Name = " + getApplicationContext().getResources().getResourceName(rId) + "\r\n";
        } catch (Exception exi) { showError(exi, false); }
        // try {
        //     allStr += "\r\ngetXml(" + rId + ") = " + getApplicationContext().getResources().getXml(rId) +  "\r\n";
        // } catch (Exception exi) { showError(exi, false); }
        return allStr;
    }

    /**
     * onCreateOptionsMenu 
     * @param menu - the menu item, that has been selected
	 * @return true, if menu successfully created, otherwise false
     */
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        myMenu = menu;
        int menuId = -1;

        try {
            menuId = getApplicationContext().getResources().getIdentifier(
                    "menu_main",
                    "menu",
                    getApplicationContext().getPackageName());

            if (menuId >= 0) {
                getMenuInflater().inflate(menuId, menu);
                menuResetCheckboxes();
                return true;
            }
        } catch (Exception menuEx) {
            showException(menuEx);
        }

        return false;
    }

    /**
     * onOptionsItemSelected
     * @param  item - the menu item, that has been selected
	 * @return false to allow normal menu processing to proceed, true to consume it here.
     */
    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int  mItemId = (item != null) ?  item.getItemId() : -1;
		if (mItemId >= 0) {
            if (mItemId == R.id.action_help) {
                helpText();
                return true;
            }
            // reset now all checkboxes for language menu items
            menuResetCheckboxes();

            if (mItemId == R.id.action_default_cards) {
                //Sets application locale in GlobalAppSettings from default app locale
                Locale primaryLocale = getApplicationContext().getResources().getConfiguration().getLocales().get(0);
                return setLocale(primaryLocale, item);
            }
            if (mItemId == R.id.action_english_cards) { // sets locale in GlobalAppSettings with english
                return setLanguage("en", item);
            }
            if (mItemId == R.id.action_german_cards) { // changes locale in GlobalAppSettings to Deutsch
                return  setLanguage("de", item);
            }
            if (mItemId == R.id.action_french_cards) { // Overwrites application locale in GlobalAppSettings with french
                return setLanguage("fr", item);
            }
            if (mItemId == R.id.action_ukraine_cards) { //  uktainian
                return setLanguage("uk", item);
            }

			boolean consumedNoFwd = actionMenuItem(mItemId, item, myMenu);
			if (consumedNoFwd) 
				return true;
        }
	
        return super.onOptionsItemSelected(item);
    }
	
	/**
     * actionMenuItem - you must implement that method for your purpose
     * @param itemId - ressource Id of menu item
     * @param item - MenuItem item entity
     * @param parentMenu - parent Menu instance, where the menu item belongs to
	 * @return true --> Event Consumed here, now It should not be forwarded for other event
	 * 			false --> Forward for other event to get consumed
     */
	public boolean actionMenuItem(int itemId, MenuItem item, Menu parentMenu) {
		
		// we fall through by default
		return false;
	}

    /**
     * reset menu checkboxes from all checkable menu items
     */
    protected void menuResetCheckboxes() {
        if (myMenu != null) {
            myMenu.findItem(R.id.action_default_cards).setChecked(false);
            myMenu.findItem(R.id.action_english_cards).setChecked(false);
            myMenu.findItem(R.id.action_german_cards).setChecked(false);
            myMenu.findItem(R.id.action_french_cards).setChecked(false);
            myMenu.findItem(R.id.action_ukraine_cards).setChecked(false);
        }
    }



    /**
     * setLanguage
     * @param language - the langzage
     * @return true in case of succcess, otherwise false
     */
    protected boolean setLanguage(String language, MenuItem item) {
        return setLocale(new Locale(language), item);
    }

    /**
     * setLanguage
     * @param aLocale - a locale
     * @param item - a menu item
     * @return true in case of succcess, otherwise false
     */
    protected boolean setLocale(Locale aLocale, MenuItem item) {
        if (item != null) {
            item.setChecked(true);
        }
        if (globalVariable.getLocale().getLanguage() != aLocale.getLanguage()) {
            //Overwrites application locale in GlobalAppSettings with english
            globalVariable.setLocale(aLocale);
        }
        return true;
    }

    /**
     * implements Runnable
     */
    public void run() {

    }

    /**
     * helpText() prints out help text
     */
    public void helpText() {
        Intent intent = new Intent(this, AboutActivity.class);
        startActivity(intent);
    }

    /**
     * showMessage shows a new Toast dynamic message
     * @param text to display
     * @param shortTime if set to yes, message inside toast widget appears only very shortly
     */
    public void showMessage(CharSequence text, boolean shortTime) {
        if (text != null && text != "") {
            Context context = getApplicationContext();
            int duration = shortTime ? Toast.LENGTH_SHORT : Toast.LENGTH_LONG;
            Toast toast = Toast.makeText(context, text, duration);
            toast.show();
        }
    }

    /**
     * setTextMessage shows a new Toast dynamic message
     * @param text to display
     */
    protected void setTextMessage(CharSequence text) { showMessage(text, true); }

	/**
     * showMessage shows a new Toast dynamic message
     * @param text to display
     */
    public void showMessage(CharSequence text) { showMessage(text, false); }

    /**
     * showError simple dummy error handler
     * @param throwableExc java.lang.Throwable
     * @param showMessage triggers, that a Toast Widget shows the current Error / Exception
     * @return - error Text Message from throwableExc
     */
    public String showError(java.lang.Throwable throwableExc, boolean showMessage) {
        CharSequence errTextMsg = null;
        if (throwableExc != null) {
            errTextMsg = "\nERROR #" + ++errNum + " " + throwableExc.getMessage() + "\nMessage: " +
                    throwableExc.getLocalizedMessage() + "\n\t " + throwableExc + "\n";
            if (showMessage)
                showMessage(errTextMsg);
            else
                parentErrMsg += "\r\n" + errTextMsg;
            throwableExc.printStackTrace();
        }
        return (errTextMsg != null) ? errTextMsg.toString() : parentErrMsg;
    }

    /**
     * showError simple dummy error handler
     * @param myEx - tje exceütion, that has been thrown
     * @return - error Text Message from throwableExc
     */
    public void showException(java.lang.Exception myEx) {
        parentErrMsg += "\r\n" + showError(myEx, false);
    }


    /**
     * playMediaFromUri plays any sound media from an internet uri
     * @param url - the full quaöofoed url accessor
     */
    public void playMediaFromUri(String url) {
        MediaPlayer mediaPlayer = new MediaPlayer();
        mediaPlayer.setAudioAttributes(
                new AudioAttributes.Builder()
                        .setContentType(AudioAttributes.CONTENT_TYPE_MUSIC)
                        .setUsage(AudioAttributes.USAGE_MEDIA)
                        .build()
        );
        try {
            mediaPlayer.setDataSource(url);
            mediaPlayer.prepare(); // might take long! (for buffering, etc)
            mediaPlayer.start();
        } catch (Exception mediaFromUriExc) {
            showException(mediaFromUriExc);
        }
    }

    /**
     * Play sound file stored in res/raw/ directory
     * @param rawName - resource name or resource id
     *   Name
     *     Syntax  :  android.resource://[package]/[res type]/[res name]
     *     Example : @<code>Uri.parse("android.resource://com.my.package/raw/sound1");</code>
     *   Resource id
     *     Syntax  : android.resource://[package]/[resource_id]
     *     Example : @<code>Uri.parse("android.resource://com.my.package/" + R.raw.sound1); </code>
     *
     */
    public void playRawSound(int rId, String rawName) {
        try {
            String RESOURCE_PATH = ContentResolver.SCHEME_ANDROID_RESOURCE + "://";

            Resources res = getResources();
            String path;
            int resID = rId;

            if (rId <= 0) {
                // Build path using resource number
                resID = getResources().getIdentifier(rawName, "raw", getPackageName());
                path = RESOURCE_PATH + getPackageName() + File.separator + resID;

                Uri soundUri = Uri.parse(path);
            }

            final MediaPlayer mMediaPlayer = new MediaPlayer();
            mMediaPlayer.setVolume(1.0f, 1.0f);
            mMediaPlayer.setLooping(false);
            mMediaPlayer.setOnPreparedListener(new MediaPlayer.OnPreparedListener() {
                @Override
                public void onPrepared(MediaPlayer mp) {
                    mMediaPlayer.start();
                }
            });
            mMediaPlayer.setOnErrorListener(new MediaPlayer.OnErrorListener() {
                @Override
                public boolean onError(MediaPlayer mp, int what, int extra) {
                    parentErrMsg += "\r\n" + String.format(Locale.US,
                            "Media error what=%d extra=%d", what, extra);
                    // Toast.makeText(getApplicationContext(), String.format(Locale.US,
                    //        "Media error what=%d extra=%d", what, extra), Toast.LENGTH_LONG).show();
                    return false;
                }
            });

            // 2. Load using content provider, passing file descriptor.
            ContentResolver resolver = getContentResolver();
            AssetFileDescriptor fd = res.openRawResourceFd(resID);
            mMediaPlayer.setDataSource(fd.getFileDescriptor(), fd.getStartOffset(), fd.getLength());
            fd.close();
            mMediaPlayer.prepareAsync();

            // See setOnPreparedListener above
            mMediaPlayer.start();

        } catch (Exception mediaRawExc) {
            showException(mediaRawExc);
        }
    }

    public void playSoundWav(SOUNDS sounds) {
        playRawSound(-2, sounds.getName());
    }


    public void playDragCard() { playDragCard(true); }
    public void playDragCard(boolean fromRawOrInetUrl) {
        if (fromRawOrInetUrl)
            playRawSound(R.raw.windows_restore, SOUNDS.DRAG_CARD.getName());
        else
            playMediaFromUri("https://github.com/heinrichelsigan/Archon/blob/master/app/src/main/res/raw/takestone.wav?raw=true");
    }

    public void playDropCard() {
        playRawSound(R.raw.windows_recycle, SOUNDS.DROP_CARD.getName());
    }


    public void playChangeAtou() {
        playRawSound(R.raw.windows_hardware_remove, SOUNDS.CHANGE_ATOU.getName());
    }

    public void playSay20() {
        playRawSound(R.raw.windows_hardware_insert, SOUNDS.SAY_20.getName());
    }

    public void playSay40() {
        playRawSound(R.raw.windows_logon, SOUNDS.SAY_40.getName());
    }


    public void playCloseGame() {
        playRawSound(R.raw.windows_ding, SOUNDS.CLOSE_GAME.getName());
    }

    public void playColorHitRule() {
        playRawSound(R.raw.windows_ding, SOUNDS.COLOR_HIT_RULE.getName());
    }

    public void playAndEnough() {
        playRawSound(R.raw.kbd_keytap, SOUNDS.AND_ENOUGH.getName());
    }


    public void playMergeCards() {
        playRawSound(R.raw.windows_notify_messaging, SOUNDS.MERGE_CARDS.getName());
    }

    public void playChangeCardDeck() {
        playRawSound(R.raw.windows_notify_email, SOUNDS.CHANGE_CARDDECK.getName());
    }


    public void playPlayerHits() {
        playRawSound(R.raw.windows_background, SOUNDS.PLAYER_HITS.getName());
    }

    public void playComputerHits() {
        playRawSound(R.raw.windows_error, SOUNDS.COMPUTER_HITS.getName());
    }


    public void playPlayerWin() {
        playRawSound(R.raw.windows_critical_stop, SOUNDS.PLAYER_WIN.getName());
    }

    public void playComputerWin() {
        playRawSound(R.raw.windows_critical_stop, SOUNDS.COMPUTER_WIN.getName());
    }

}