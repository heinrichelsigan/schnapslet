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

import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Button;
import android.content.Intent;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.res.ResourcesCompat;
import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.constraintlayout.widget.*;
import androidx.fragment.app.DialogFragment;

import at.area23.schnapslet.models.Game;

/**
 * AboutActivity class implements help text view.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class AboutActivity extends BaseAppActivity {

    Button backButton, learnMoreButton;
    TextView helpTextView, builtWithTextView;

    /**
     * Override onCreate
     * @param savedInstanceState saved application state of current instance
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        globalVariable = (GlobalAppSettings) getApplicationContext();
        setContentView(R.layout.activity_about);

        backButton = (Button) findViewById(R.id.backButton);
        learnMoreButton =  (Button) findViewById(R.id.learnMoreButton);
        helpTextView = (TextView) findViewById(R.id.helpTextView);
        builtWithTextView = (TextView) findViewById(R.id.builtWithTextView);

        addListenerOnClickables();

        Game thatGame = globalVariable.getGame();
        if (thatGame != null) // TODO: replace with constant
            thatGame.phoneDirection = 255;

    }

    /**
     * onCreateOptionsMenu
     * @param menu options menu
     * @return always (boolean) true for overridden member function
     */
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        myMenu = menu;
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        if (id == R.id.action_start) {
            finish();
            return true;
        }
        if (id == R.id.action_stop) {
            finish();
            return true;
        }
        if (id == R.id.action_help) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }


    /**
     * add listeners on all clickables
     */
    public void addListenerOnClickables() {
        backButton = (Button) findViewById(R.id.backButton);
        backButton.setOnClickListener(arg0 -> backButton_Clicked());

        learnMoreButton = (Button) findViewById(R.id.learnMoreButton);
        learnMoreButton.setOnClickListener(arg0 -> learnMoreButton_Clicked());
    }


    /**
     * backButton_Clicked finish about activity
     */
    public void backButton_Clicked() {
        // finish activity
        finish();
    }


    /**
     * learnMoreButton_Clicked_Clicked
     */
    public void learnMoreButton_Clicked() {

        Intent intent = new Intent();
        intent.setAction(Intent.ACTION_VIEW);
        String github = getString(R.string.github_uri);
        intent.setData(android.net.Uri.parse(github));
        startActivity(intent);

    }


}