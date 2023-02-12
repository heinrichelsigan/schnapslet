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

import android.content.Context;
import android.content.Intent;
import android.content.OperationApplicationException;
import android.graphics.Typeface;
import android.os.Build;
import android.os.Bundle;
import android.speech.tts.TextToSpeech;
import android.speech.tts.UtteranceProgressListener;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import java.util.HashMap;
import java.util.Locale;

import at.area23.schnapslet.constenum.SCHNAPSOUNDS;

/**
 * BaseAppActivity extends AppCompatActivity
 * <p>
 * inherit all your Activity classes from that class
 * BaseAppActivity provides all lot of framework
 */
public class BaseAppActivity extends AppCompatActivity {

    protected volatile boolean started = false;
    protected volatile int startedTimes = 0;
    protected volatile int errNum = 0;
    protected volatile int startedDrag = 0;
    protected volatile int finishedDrop = 0;
    protected String tmp = "";

    protected Menu myMenu;

    protected HashMap<Integer, android.view.View> viewMap;
    protected android.view.View rootView = null;
    // Calling Application class (see application tag in AndroidManifest.xml)
    protected android.speech.tts.TextToSpeech text2Speach = null;
    protected String speakCallbackId;
    protected GlobalAppSettings globalVariable;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        globalVariable = (GlobalAppSettings) getApplicationContext();

        text2Speach = new TextToSpeech(getApplicationContext(), new TextToSpeech.OnInitListener() {
            @Override
            public void onInit(int status) {
                if (status != TextToSpeech.ERROR) {
                    text2Speach.setLanguage(globalVariable.geSystemLLocale());
                }
            }
        });
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
     *
     * @param item - the menu item, that has been selected
     * @return false to allow normal menu processing to proceed, true to consume it here.
     */
    public boolean onOptionsItemSelected(MenuItem item) {

        int mItemId = (item != null) ? item.getItemId() : -1;
        if (mItemId >= 0) {
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
                return setLanguage("de", item);
            }
            if (mItemId == R.id.action_french_cards) { // Overwrites application locale in GlobalAppSettings with french
                return setLanguage("fr", item);
            }
            if (mItemId == R.id.action_ukraine_cards) { //  uktainian
                return setLanguage("uk", item);
            }
            if (mItemId == R.id.action_polish_cards) { // Overwrites application locale in GlobalAppSettings with french
                return setLanguage("pl", item);
            }
        }

        return super.onOptionsItemSelected(item);
    }

    /**
     * setLocale        - change Locale incl. language in GlobalAppSettings globalVariable
     *
     * @param aLocale   - a locale
     * @param item      - a menu item
     * @return true in case of succcess, otherwise false
     */
    protected boolean setLocale(Locale aLocale, MenuItem item) {
        if (item != null) {
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

    /**
     * reset menu checkboxes from all checkable menu items
     */
    protected void menuResetCheckboxes() {
        if (myMenu != null) {
            for (int i = 0; i < myMenu.size(); i++) {
                MenuItem mItem = myMenu.getItem(i);
                if (mItem.isCheckable() && mItem.isChecked())
                    mItem.setChecked(false);
            }
        }
    }

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
        text2Speach.stop();
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
            locLang = globalVariable.geSystemLLocale();
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

    /**
     * saySchnapser text 2 speach
     * @param schnapserl default enum
     * @param text2Say special text to say
     */
    public void saySchnapser(SCHNAPSOUNDS schnapserl, String text2Say) {
        String sayPhrase = (text2Say != null) ? text2Say : schnapserl.saySpeach(getApplicationContext());
        if (sayPhrase != null) {
            // text2Speach.speak(sayPhrase, TextToSpeech.QUEUE_FLUSH, null);
            float floatRate = Float.parseFloat("1.15");
            float floatPitch = (float)(Math.E / 2);
            float floatVolume =  (float)Math.sqrt(Math.PI);

            speak(sayPhrase, globalVariable.geSystemLLocale(), floatRate, floatPitch, floatVolume, speakCallbackId);
            // .speak(text2Say, TextToSpeech.QUEUE_FLUSH, null);
        }
    }

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

            return ;
        }
        else
            throw new NullPointerException(getString(R.string.msg_null_pointer_toggle_text_view));
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

            return ;
        }
        else
            throw new NullPointerException(getString(R.string.msg_null_pointer_toggle_text_view));
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
        if (anyView != null) {
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
            return ;
        }
        else
            throw new NullPointerException(getString(R.string.msg_null_pointer_toggle_any_view));
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
        if (aMenuItem != null) {
            aMenuItem.setEnabled(enabled);
            return;
        }
        else
            throw new NullPointerException(getString(R.string.msg_null_pointer_toggle_menu_item));
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


    /**
     * showHelp
     * prints out help text
     */
    public void showHelp() {
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
     * openUrl
     * opens https://github.com/heinrichelsigan/schnapslet/wiki
     */
    public void openUrl() {
        Intent intent = new Intent();
        intent.setAction(Intent.ACTION_VIEW);
        String github = getString(R.string.github_uri);
        intent.setData(android.net.Uri.parse(github));
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
            CharSequence text = "CRITICAL ERROR #" + String.valueOf((errNum)) + " " + myErr.getMessage() + "\nMessage: " + myErr.getLocalizedMessage() + "\n";
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
}