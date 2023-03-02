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
import android.content.ContentResolver;
import android.content.Context;
import android.content.Intent;
import android.content.OperationApplicationException;
import android.content.res.AssetFileDescriptor;
import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Typeface;
import android.graphics.drawable.BitmapDrawable;
import android.media.AudioAttributes;
import android.media.MediaPlayer;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.os.Handler;
import android.os.Looper;
import android.provider.MediaStore;
import android.speech.tts.TextToSpeech;
import android.speech.tts.UtteranceProgressListener;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import java.io.File;
import java.io.FileOutputStream;
import java.io.OutputStream;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;
import java.util.Locale;

import at.area23.schnapslet.constenum.Constants;
import at.area23.schnapslet.constenum.SCHNAPSOUNDS;

/**
 * BaseAppActivity extends AppCompatActivity
 * <p>
 * inherit all your Activity classes from that class
 * BaseAppActivity provides all lot of framework
 */
public class BaseAppActivity extends AppCompatActivity {

    protected volatile boolean started = false;
    protected volatile int ticks = 0;
    protected volatile int errNum = 0;
    protected volatile int startedDrag = 0;
    protected volatile int finishedDrop = 0;
    protected volatile Integer syncLocker  = null;
    protected String tmp = "";

    protected Menu myMenu;

    protected HashMap<Integer, android.view.View> viewMap;
    protected android.view.View rootView = null;
    // Calling Application class (see application tag in AndroidManifest.xml)
    protected android.speech.tts.TextToSpeech text2Speach = null;
    protected String speakCallbackId;
    protected ViewGroup rootViewGroup;
    protected GlobalAppSettings globalVariable;
    protected final static Handler playL8rHandler = new Handler(Looper.getMainLooper());

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        globalVariable = (GlobalAppSettings) getApplicationContext();
        if (rootView == null)
            rootView = getWindow().getDecorView().getRootView();
        rootView.setDrawingCacheEnabled(false);

        text2Speach = new TextToSpeech(getApplicationContext(), status -> {
            if (status != TextToSpeech.ERROR) {
                text2Speach.setLanguage(globalVariable.getSystemLLocale());
            }
        });
    }

    //region MenuSelection

    /**
     * setCardDeckFromSystemLocale
     *  sets card deck from default system language
     */
    public void setCardDeckFromSystemLocale() {
        String defaultLocale = "en";
        int localeItemId = R.id.action_english_cards;
        if (globalVariable != null) {
            defaultLocale = globalVariable.getSystemLLocale().getISO3Country().toLowerCase();
        }
        if (defaultLocale.equals("de"))
            localeItemId = R.id.action_german_cards;
        else if (defaultLocale.equals("en"))
            localeItemId = R.id.action_english_cards;
        else if (defaultLocale.equals("fe"))
            localeItemId = R.id.action_french_cards;
        else if (defaultLocale.equals("uk"))
            localeItemId = R.id.action_ukraine_cards;
        else if (defaultLocale.equals("us"))
            localeItemId = R.id.action_us_cards;

        if (myMenu != null && myMenu.findItem(localeItemId) != null) {
            myMenu.findItem(localeItemId).setChecked(true);
        }
    }

    /**
     * onCreateOptionsMenu
     *
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
                Menu mySubMenu = myMenu.findItem(R.id.action_carddeck).getSubMenu();
                menuResetCheckboxes(mySubMenu);
                return true;
            }
        } catch (Exception menuEx) {
            showException(menuEx);
        }

        return false;
    }

    /**
     * onOptionsItemSelected
     *
     * @param item - the menu item, that has been selected
     * @return false to allow normal menu processing to proceed, true to consume it here.
     */
    public boolean onOptionsItemSelected(MenuItem item) {

        int mItemId = (item != null) ? item.getItemId() : -1;
        if (mItemId >= 0) {
            // reset now all checkboxes for language menu items
            // menuResetCheckboxes(myMenu);

            if (mItemId == R.id.action_default_cards) {
                //Sets application locale in GlobalAppSettings from default app locale
                Locale primaryLocale = getApplicationContext().getResources().getConfiguration().getLocales().get(0);
                return setLocale(primaryLocale, item);
            }
            if (mItemId == R.id.action_english_cards) { // sets locale in GlobalAppSettings with english
                return setLanguage("en", item);
            }
            if (mItemId == R.id.action_german_cards) { // changes locale in GlobalAppSettings to Deutsch
                return setLanguage("de", item);
            }
            if (mItemId == R.id.action_french_cards) { // Overwrites application locale in GlobalAppSettings with french
                return setLanguage("fr", item);
            }
            if (mItemId == R.id.action_ukraine_cards) { //  ukrainian
                return setLanguage("uk", item);
            }
            if (mItemId == R.id.action_us_cards) { // Overwrites application locale in GlobalAppSettings with french
                return setLanguage("us", item);
            }
            if (mItemId == R.id.action_screenshot) {
                takeScreenShot(rootView, true);
                return true;
            }
            if (mItemId == R.id.action_sound) {
                toggleSoundOnOff();
                return true;
            }
            if (mItemId == R.id.action_exit) {
                exitGame();
                return true;
            }
        }

        return super.onOptionsItemSelected(item);
    }

    /**
     * reset menu checkboxes from all checkable menu items
     * @param recusriveMenu   - menu or submenu to enter recursion
     */
    protected void menuResetCheckboxes(Menu recusriveMenu) {
        if (recusriveMenu == null) {
            recusriveMenu = myMenu;
        }
        if (recusriveMenu != null) {
            for (int i = 0; i < recusriveMenu.size(); i++) {
                MenuItem mItem = recusriveMenu.getItem(i);
                if (mItem != null) {
                    if (mItem.isCheckable() && mItem.isChecked())
                        mItem.setChecked(false);
                    if (mItem.hasSubMenu()) {
                        Menu subMenu = mItem.getSubMenu();
                        menuResetCheckboxes(subMenu);
                    }
                }
            }
        }
    }


    /**
     * toggleSoundOnOff - toggle in submenu options Sound On/Off
     */
    public void toggleSoundOnOff() {
        boolean soundOnOff = globalVariable == null || globalVariable.getSound();
        MenuItem soundMenuItem = myMenu.findItem(R.id.action_sound);
        if (soundMenuItem != null) {
            soundMenuItem.setChecked(!soundOnOff);
            if (globalVariable != null) {
                globalVariable.setSound(!soundOnOff);
            }
        }
    }

    //endregion

    /**
     * setLocale - change Locale incl. language in GlobalAppSettings globalVariable
     *
     * @param aLocale   - a locale
     * @param item      - a menu item
     * @return true in case of succcess, otherwise false
     */
    protected boolean setLocale(Locale aLocale, MenuItem item) {
        if (item != null) {
            Menu mySubMenu = myMenu.findItem(R.id.action_carddeck).getSubMenu();
            menuResetCheckboxes(mySubMenu);
            item.setChecked(true);
        }
        if (!globalVariable.getLocale().getLanguage().equals(aLocale.getLanguage())) {
            //Overwrites application locale in GlobalAppSettings with english
            globalVariable.setLocale(aLocale);
        }
        return true;
    }

    /**
     * setLanguage      - change the language in GlobalAppSettings globalVariable
     *
     * param language   - new language to set
     * @param item      - a menu item
     * @return true in case of success, otherwise false
     */
    protected boolean setLanguage(String language, MenuItem item) {
        Locale newLocale = new Locale(language);
        return setLocale(newLocale, item);
    }

    //region speakSaySound

    /**
     * playMediaFromUri plays any sound media from an internet uri
     * @param url - the full quaöofoed url accessor
     */
    public void playMediaFromUri(String url) {
        boolean soundOn = globalVariable == null || globalVariable.getSound();
        if (soundOn) {
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
            } catch (Exception exi) {
                showError(exi, true);
            }
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
        boolean soundOn = globalVariable == null || globalVariable.getSound();
        if (soundOn) {
            try {
                Resources res = getResources();
                int resId = rId;
                if (rawName != null) {
                    resId = getSoundRId(rawName);
                }

                if (resId != rId) {
                    String RESOURCE_PATH = ContentResolver.SCHEME_ANDROID_RESOURCE + "://";
                    String path = RESOURCE_PATH + getPackageName() + File.separator + resId;
                    Uri soundUri = Uri.parse(path);
                    showMessage("RawSound: Uri=" + soundUri.toString() + " path=" + path);
                }

                final MediaPlayer mMediaPlayer = new MediaPlayer();
                mMediaPlayer.setVolume(1.0f, 1.0f);
                mMediaPlayer.setLooping(false);
                mMediaPlayer.setOnPreparedListener(mp -> {
                    // Toast.makeText(getApplicationContext(),
                    //         "start playing sound", Toast.LENGTH_SHORT).show();
                    mMediaPlayer.start();
                });
                mMediaPlayer.setOnErrorListener((mp, what, extra) -> {
                    // Toast.makeText(getApplicationContext(), String.format(Locale.US,
                    //         "Media error what=%d extra=%d", what, extra), Toast.LENGTH_LONG).show();
                    return false;
                });

                // 2. Load using content provider, passing file descriptor.
                ContentResolver resolver = getContentResolver();
                AssetFileDescriptor fd = res.openRawResourceFd(resId);
                mMediaPlayer.setDataSource(fd.getFileDescriptor(), fd.getStartOffset(), fd.getLength());
                fd.close();
                mMediaPlayer.prepareAsync();

                // See setOnPreparedListener above
                mMediaPlayer.start();

            } catch (Exception ex) {
                // showException(ex);
                showMessage(String.join("MediaPlayer: ", ex.getMessage()));
                Toast.makeText(this, ex.getMessage(), Toast.LENGTH_LONG).show();
            }
        }
    }

    /**
     * getSoundRId
     * @param rawSoundName - raw sound name
     * @return getRessources.getIdentifier(rawSoundName, ...):
     */
    public int getSoundRId(String rawSoundName) {
        // Build path using resource number
        int resID = getResources().getIdentifier(rawSoundName, "raw", getPackageName());
        return resID;
    }

    /**
     * playSound
     *  plays a sound
     * @param rawSoundName - resource name
     *      Syntax  :  android.resource://[package]/[res type]/[res name]
     *      Example : @<code>Uri.parse("android.resource://com.my.package/raw/sound1");</code>
     */
    public void playSound(String rawSoundName) {
        int resID = getSoundRId(rawSoundName);
        playRawSound(resID, rawSoundName);
    }

    // @SuppressLint("InlinedApi")
    /**
     * delayPlayScreenshot = new Runnable() -> { playSound("sound_screenshot"); }
     */
    protected final Runnable delayPlayScreenshot = () -> playSound("sound_screenshot");

    // @SuppressLint("InlinedApi")
    /**
     * delayPlayKissClickClick = new Runnable() -> { playSound("sound_kissclickclick"); }
     */
    protected final Runnable delayPlayKissClickClick = () -> playSound("sound_kissclickclick");

    /**
     * speak - say some text
     *
     * @param text String       - text string to speak
     * @param locLang Locale    - Locale for speak language
     * @param rate float        - floating point rate  (default 1)
     * @param pitch float       - floating point pitch (default 1)
     * @param volume float      - floating point speaker volume
     * @param callbackId String - speaker callback identifier as String
     */
    public void speak(String text, Locale locLang, float rate, float pitch, float volume, String callbackId) {
        boolean soundOn = globalVariable == null || globalVariable.getSound();
        text2Speach.stop();

        if (soundOn) {
            text2Speach.setOnUtteranceProgressListener(
                    new UtteranceProgressListener() {

                        @Override
                        public void onStart(String utteranceId) {
                        }

                        @Override
                        public void onDone(String utteranceId) {
                            showMessage("speak: UtteranceProgressListener.Done: " + utteranceId, false);
                        }

                        @Override
                        public void onError(String utteranceId) {
                            OperationApplicationException opAppEx =
                                    new OperationApplicationException("speak: UtteranceProgressListener.Error: " + utteranceId);
                            showException(opAppEx);
                            // showMessage("speak: UtteranceProgressListener.Error: " + utteranceId, false);
                        }
                    }
            );

            if (locLang == null)
                locLang = globalVariable.getSystemLLocale();
            text2Speach.setLanguage(locLang);

            text2Speach.setSpeechRate(rate);
            text2Speach.setPitch(pitch);

            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB_MR2) {
                Bundle ttsParams = new Bundle();
                ttsParams.putSerializable(android.speech.tts.TextToSpeech.Engine.KEY_PARAM_UTTERANCE_ID, callbackId);
                ttsParams.putSerializable(android.speech.tts.TextToSpeech.Engine.KEY_PARAM_VOLUME, volume);

                text2Speach.speak(text, android.speech.tts.TextToSpeech.QUEUE_FLUSH, ttsParams, callbackId);
            } else {
                HashMap<String, String> ttsParams = new HashMap<>();
                ttsParams.put(android.speech.tts.TextToSpeech.Engine.KEY_PARAM_UTTERANCE_ID, callbackId);
                ttsParams.put(android.speech.tts.TextToSpeech.Engine.KEY_PARAM_VOLUME, Float.toString(volume));

                text2Speach.speak(text, android.speech.tts.TextToSpeech.QUEUE_FLUSH, ttsParams);
            }
        }
    }

    /**
     * saySchnapser text 2 speach
     * @param schnapserl default enum
     * @param text2Say special text to say
     */
    public void saySchnapser(SCHNAPSOUNDS schnapserl, String text2Say) {
        String sayPhrase = (text2Say != null && text2Say.length() > 0) ?
                text2Say : schnapserl.saySpeach(getApplicationContext());

        if (sayPhrase != null) {
            float floatRate = Float.parseFloat("1.15");
            float floatPitch = (float)(Math.E / 2);
            float floatVolume =  (float)Math.sqrt(Math.PI);

            speak(sayPhrase, globalVariable.getSystemLLocale(),
                    floatRate, floatPitch, floatVolume, speakCallbackId);
        }
    }
    //endregion

    //region toggleEnabled

    /**
     * toggleEnabled - sets any TextView to enabled or disabled
     *
     * @param anytTextView TextView to change
     * @param enabled boolean => sets special enabled or not enabled state
     * @param text2Set String => replaces text of TextView with text2Set String
     * @param toolTip String => replaces toolTip of TextView with text2Set String
     *
     * @throws NullPointerException when anytTextView is null
     * */
    public void toggleEnabled(TextView anytTextView, boolean enabled, String text2Set, String toolTip) {

        if (anytTextView == null) {
            throw new NullPointerException(getString(R.string.msg_null_pointer_toggle_text_view));
        }
        if (anytTextView != null) {

            if (anytTextView.getVisibility() != View.VISIBLE)
                anytTextView.setVisibility(View.VISIBLE);

            anytTextView.setEnabled(enabled);

            if (text2Set != null && text2Set.length() > 0) {
                anytTextView.setText(text2Set);
                String toolTip2Set = (toolTip != null) ? toolTip : text2Set;
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O && toolTip2Set.length() > 1) {
                    anytTextView.setTooltipText(toolTip2Set);
                }
            }

            if (enabled) {
                anytTextView.setTextColor(getColor(R.color.colorDark));
                anytTextView.setTypeface(anytTextView.getTypeface(), Typeface.BOLD);
                anytTextView.setBackgroundColor(getColor(R.color.colorBackLight));

            }
            else {
                anytTextView.setTextColor(getColor(R.color.colorLighter));
                anytTextView.setTypeface(anytTextView.getTypeface(), Typeface.ITALIC);
                anytTextView.setBackgroundColor(getColor(R.color.colorBackButton));
            }
        }
    }

    /**
     * toggleEnabled - sets any Button to enabled or disabled
     *
     * @param aButton Button to change
     * @param enabled boolean => sets special enabled or not enabled state
     * @param text2Set String => replaces text of TextView with text2Set String
     * @param toolTip String => replaces toolTip of TextView with text2Set String
     *
     * @throws NullPointerException when anytTextView is null
     * */
    public void toggleEnabled(Button aButton, boolean enabled, String text2Set, String toolTip) {

        if (aButton == null) {
            throw new NullPointerException(getString(R.string.msg_null_pointer_toggle_text_view));
        }
        if (aButton != null) {

            if (aButton.getVisibility() != View.VISIBLE)
                aButton.setVisibility(View.VISIBLE);

            aButton.setEnabled(enabled);

            if (text2Set != null && text2Set.length() > 0) {
                aButton.setText(text2Set);
                String toolTip2Set = (toolTip != null) ? toolTip : text2Set;
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O && toolTip2Set.length() > 1) {
                    aButton.setTooltipText(toolTip2Set);
                }
            }

            if (enabled) {
                aButton.setTextColor(getColor(R.color.colorDark));
                aButton.setTypeface(aButton.getTypeface(), Typeface.BOLD);
                aButton.setBackgroundColor(getColor(R.color.colorBackLight));
            }
            else {
                aButton.setTextColor(getColor(R.color.colorLighter));
                aButton.setTypeface(aButton.getTypeface(), Typeface.ITALIC);
                aButton.setBackgroundColor(getColor(R.color.colorBackButton));
            }
        }
    }

    /**
     * toggleEnabled - sets any TextView to enabled or disabled
     *
     * @param anytTextView TextView to change
     * @param enabled boolean => sets special enabled or not enabled state
     * @param text2Set String => replaces text of TextView with text2Set String
     */
    public void toggleEnabled(TextView anytTextView, boolean enabled, String text2Set) {
        toggleEnabled(anytTextView, enabled, text2Set, text2Set);
    }

    /**
   * toggleEnabled - sets any View to enabled or disabled
   *
   * @param anyView View to change
   * @param enabled boolean => sets special enabled or not enabled state
   *
   * @throws NullPointerException when anyView is null
   */
    public void toggleEnabled(View anyView, boolean enabled) {
        if (anyView == null) {
            throw new NullPointerException(getString(R.string.msg_null_pointer_toggle_any_view));
        }

        if (anyView instanceof TextView)
            toggleEnabled(((TextView) anyView), enabled, null);
        else if (anyView instanceof Button)
            toggleEnabled(((Button) anyView), enabled, null, null);
        else {
            anyView.setEnabled(enabled);
            if (enabled)
                anyView.setBackgroundColor(getColor(R.color.colorBackLight));
            else
                anyView.setBackgroundColor(getColor(R.color.colorBackButton));
        }
    }

    /**
     * toggleMenuItem - sets a MenuItem to enabled or disabled
     *
     * @param aMenuItem MenuItem to change
     * @param enabled boolean => sets special enabled or not enabled state
     *
     * @throws NullPointerException when aMenuItem is null
     */
    public void toggleMenuItem(MenuItem aMenuItem, boolean enabled) {
        if (aMenuItem == null) {
            throw new NullPointerException(getString(R.string.msg_null_pointer_toggle_menu_item));
        }
        if (aMenuItem != null) {
            aMenuItem.setEnabled(enabled);
        }
    }

    /**
     * toggleMenuItem - sets a MenuItem to enabled or disabled
     * @param aMenu Menu to find item
     * @param itemId MenuItem id
     * @param enabled boolean => sets special enabled or not enabled state
     */
    public void toggleMenuItem(Menu aMenu, int itemId, boolean enabled) {
        if (aMenu != null && aMenu.findItem(itemId) != null) {
            aMenu.findItem(itemId).setEnabled(enabled);
        }
    }
    //endregion

    //region showHelpMessageErrorException

    /**
     * startAboutActivity
     * starts About Activity
     */
    public void startAboutActivity() {
        // try {
        //     Thread.currentThread().sleep(10);
        // } catch (Exception exInt) {
        //     errHandler(exInt);
        // }
        // tDbg.setText(R.string.help_text);
        Intent intent = new Intent(this, AboutActivity.class);
        startActivity(intent);
    }

    /**
     * showMessage shows a new Toast dynamic message
     *
     * @param text     to display
     * @param tooShort if set to yes, message inside toast widget appears only very shortly
     */
    public void showMessage(CharSequence text, boolean tooShort) {
        if (text != null && text != "") {
            Context context = getApplicationContext();
            Toast toast = Toast.makeText(context, text,
                    tooShort ? Toast.LENGTH_SHORT : Toast.LENGTH_LONG);
            toast.show();
        }
    }

    /**
     * showMessage shows a new Toast dynamic message
     *
     * @param text to display
     */
    public void showMessage(CharSequence text) {
        showMessage(text, false);
    }

    /**
     * showError simple dummy error handler
     *
     * @param myErr       java.lang.Throwable
     * @param showMessage triggers, that a Toast Widget shows the current Error / Exception
     */
    public void showError(java.lang.Throwable myErr, boolean showMessage) {
        if (myErr != null) {
            synchronized (this) {
                ++errNum;
            }
            CharSequence text = "CRITICAL ERROR #" + errNum + " " + myErr.getMessage() + "\nMessage: " + myErr.getLocalizedMessage() + "\n";
            if (showMessage)
                showMessage(text);
            myErr.printStackTrace();
        }
    }

    /**
     * showError simple dummy error handler
     *
     * @param myEx - tje exceütion, that has been thrown
     */
    public void showException(java.lang.Exception myEx) {
        showError(myEx, true);
    }

    //endregion

    /**
     * takeScreenShot
     *  takes a screenshot from a specific view or root view
     *  of current android app
     * @param view2Bmp android view on screen to grab / shot
     *                 if (view2Bmp == null)
     *                 getWindow().getDecorView().getRootView() will be assigned
     * @param compress compress saved image file
     *                 if (true) image will be saved as .jpg
     *                 otherwise (false) as .png
     */
    public void takeScreenShot(View view2Bmp, boolean compress) {

        // if (view2Bmp == null)
        view2Bmp = getWindow().getDecorView().getRootView();
        view2Bmp.buildDrawingCache(false);

        // Output directory path
        String path = getApplicationContext().getExternalFilesDir(null).getAbsolutePath();
        // path = (Build.VERSION.SDK_INT >= Build.VERSION_CODES.R) ? Environment.getStorageDirectory().toString() :
        //  Environment.getExternalStorageDirectory().toString();

        // save file name
        String saveName = Constants.getSaveImageFileName(compress);
        OutputStream fileOutputStream = null; // file Output Stream
        File file = new File(path, saveName);

        try {
            fileOutputStream = new FileOutputStream(file);
            // Canvas canvas = new Canvas(); view2Bmp.draw(canvas);
            // Paint paint = new Paint(); canvas.drawBitmap(0, 0, paint);
            Bitmap pictureBitmap = view2Bmp.getDrawingCache(false); // view2Bmp.getDrawingCache(true);
            Bitmap.CompressFormat bmpFormat = (compress) ?
                    Bitmap.CompressFormat.JPEG : Bitmap.CompressFormat.PNG;
            int qualitiy = (compress) ? 80 : 100;
            pictureBitmap.compress(bmpFormat, qualitiy, fileOutputStream);
            fileOutputStream.flush();   // flush output stream
            fileOutputStream.close();   // close output stream

            // insert written image file to media store
            MediaStore.Images.Media.insertImage(getContentResolver(),
                file.getAbsolutePath(), file.getName(), file.getName());

        } catch (Exception saveEcx) {
            if (fileOutputStream != null) {
                try {
                    fileOutputStream.close();
                } catch (Exception exx) {
                }
            }
            showError(saveEcx, true);
            saveEcx.printStackTrace();
        }
        finally {
            fileOutputStream = null;
        }

        try {
            view2Bmp.destroyDrawingCache();
        }
        catch (Exception destroyDrawingCacheEx) {
            destroyDrawingCacheEx.printStackTrace();
        }

        playL8rHandler.postDelayed(delayPlayScreenshot, 200);
    }

    /**
     * openUrl opens a  defined url as ACTION_VIEW Intent
     * @param urlString url to open
     */
    public void openUrl(String urlString) {

        Uri openUri;
        String url2Open = (urlString != null && urlString.length() > 1) ?
                urlString : getString(R.string.wiki_uri);
        try {
            openUri = android.net.Uri.parse(url2Open);
        } catch (Exception exUriParse) {
            exUriParse.printStackTrace();
            openUri = android.net.Uri.parse(getString(R.string.github_uri));
        }

        playL8rHandler.postDelayed(delayPlayKissClickClick, 100);

        Intent intent = new Intent();
        intent.setAction(Intent.ACTION_VIEW);
        intent.setData(openUri);
        startActivity(intent);
    }

    /**
     * exitGame() exit game
     */
    public void exitGame() {
        playL8rHandler.postDelayed(delayPlayKissClickClick, 100);
        finishAffinity();
        System.exit(0);
    }

}