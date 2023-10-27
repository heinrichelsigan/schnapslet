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
package at.area23.schnapslet.sound;

import android.content.ContentResolver;
import android.content.Context;
import android.content.res.AssetFileDescriptor;
import android.content.res.Resources;
import android.media.AudioAttributes;
import android.media.MediaPlayer;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.provider.MediaStore;
import android.speech.tts.TextToSpeech;
import android.speech.tts.UtteranceProgressListener;
import java.util.HashMap;
import java.util.Locale;


/**
 * SoundPlayer
 * <p>
 * Sound and media player and say speach accessor
 * </p>
 */
public class SoundPlayer {

    public volatile boolean started = false;
    static Context context;
    static SoundPlayer soundPlayer;
    protected volatile short playing = 0;
    // Calling Application class (see application tag in AndroidManifest.xml)
    protected android.speech.tts.TextToSpeech text2Speach = null;
    protected String speakCallbackId;


    public SoundPlayer(Context c) {
        started = true;
        context = c;
        Locale sysLocale = c.getApplicationContext().getResources().getConfiguration().getLocales().get(0);
        text2Speach = new TextToSpeech(c.getApplicationContext(), status -> {
            if (status != TextToSpeech.ERROR) {
                text2Speach.setLanguage(sysLocale);
            }
        });
    }

    public SoundPlayer setContext(Context c) {
        context = c;
        return this;
    }

    public static SoundPlayer initContext(Context c) {
        if (soundPlayer == null)
            soundPlayer = new SoundPlayer(c);
        else
            soundPlayer.setContext(c);
        return soundPlayer;
    }

    /**
     * playMediaFromUri plays any sound media from an internet uri
     *
     * @param url     - the full quaöofoed url accessor
     * @param soundOn - if false, no sound playing
     * @return boolean, true if sound was played, false if sound not played
     */
    public boolean playMediaFromUri(String url, boolean soundOn) {

        if (!soundOn)
            return false;

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
            // TODO: handle Exception here
            return false;
        }
        return true;
    }

    /**
     * playRawSound
     * Plays sound file stored in res/raw/ directory
     *
     * @param resRawId - resource id of R.raw.{soundResourceName}
     * @param soundOn  - if false, no sound playing
     * @return boolean, true if sound was played, false if sound wasn't played
     */
    public boolean playRawSound(int resRawId, boolean soundOn) {

        if (!soundOn)
            return false;

        try {
            Resources res = context.getResources();
            int resId = resRawId;

            final MediaPlayer mMediaPlayer = new MediaPlayer();
            mMediaPlayer.setVolume(1.0f, 1.0f);
            mMediaPlayer.setLooping(false);
            mMediaPlayer.setOnPreparedListener(mp -> {
                mMediaPlayer.start();
            });
            mMediaPlayer.setOnErrorListener((mp, what, extra) -> {
                // Toast.makeText(getApplicationContext(), String.format(Locale.US,
                //         "Media error what=%d extra=%d", what, extra), Toast.LENGTH_LONG).show();
                return false;
            });

            // 2. Load using content provider, passing file descriptor.
            ContentResolver resolver = context.getContentResolver();
            AssetFileDescriptor fd = res.openRawResourceFd(resId);
            mMediaPlayer.setDataSource(fd.getFileDescriptor(), fd.getStartOffset(), fd.getLength());
            fd.close();
            mMediaPlayer.prepareAsync();

            // See setOnPreparedListener above
            mMediaPlayer.start();

        } catch (Exception ex) {
            // showException(ex);
            // What shell we do with the drunken saylor
            return false;
        }
        return true;
    }


    /**
     * playSound
     * plays a sound
     *
     * @param rawSoundName - resource name
     * @param soundOn      - if false, no sound playing
     * @return boolean, true if sound was played, otherwise false
     */
    public boolean playSound(String rawSoundName, boolean soundOn) {
        if (!soundOn)
            return false;
        int resID = context.getResources().getIdentifier(rawSoundName, "raw", context.getPackageName());
        return (resID >= 0) ? playRawSound(resID, soundOn) : false;
    }


    /**
     * speak - say some text
     *
     * @param text       String       - text string to speak
     * @param locLang    Locale    - Locale for speak language
     * @param rate       float        - floating point rate  (default 1)
     * @param pitch      float       - floating point pitch (default 1)
     * @param volume     float      - floating point speaker volume
     * @param callbackId String - speaker callback identifier as String
     * @param soundOn    boolean   - true, if should speak, false for sound of silence
     * @return boolean, true if spoken, otherwise false
     */
    public boolean speak(String text, Locale locLang, float rate, float pitch, float volume,
                         String callbackId, boolean soundOn) {

        text2Speach.stop();
        if (!soundOn)
            return false;

        text2Speach.setOnUtteranceProgressListener(
                new UtteranceProgressListener() {

                    @Override
                    public void onStart(String utteranceId) {
                        playing = 2;
                    }

                    @Override
                    public void onDone(String utteranceId) {
                        playing = 0;
                    }

                    @Override
                    public void onError(String utteranceId) {
                        playing = 255;
                    }
                }
        );

        if (locLang == null)
            locLang = context.getResources().getConfiguration().getLocales().get(0);
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

        if (playing == 255) {
            playing = 0;
            return false;
        }

        return true;
    }

    /**
     * sayText text 2 speach
     *
     * @param text2Say       special text to say
     * @param languageLocale Locale for language to say text
     * @param soundOn        - if false, no sound playing
     * @return boolean, true if spoken out, otherwise false
     */
    public boolean sayText(String text2Say, Locale languageLocale, boolean soundOn) {

        if (text2Say != null && text2Say.length() > 0) {
            float floatRate = Float.parseFloat("1.15");
            float floatPitch = (float) (Math.E / 2);
            float floatVolume = (float) Math.sqrt(Math.PI);

            return speak(text2Say, languageLocale,
                    floatRate, floatPitch, floatVolume, speakCallbackId, soundOn);
        }
        return false;
    }

}