package at.area23.heinrichelsigan.schnapslet;

import at.area23.heinrichelsigan.schnapslet.*;

import android.content.Context;
import android.content.res.Resources;
// import android.support.v4.app.FragmentActivity;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Button;
import android.content.Intent;
import android.widget.TextView;
import android.view.View;
import android.view.View.OnClickListener;

/**
 * AboutActivity class implements help text view.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class AboutActivity extends AppCompatActivity {

    Button backButton, learnMoreButton;
    TextView helpTextView, builtWithTextView;
    Menu myMenu;


    /**
     * Override onCreate
     * @param savedInstanceState
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_about);

        backButton = (Button) findViewById(R.id.backButton);
        learnMoreButton =  (Button) findViewById(R.id.learnMoreButton);
        helpTextView = (TextView) findViewById(R.id.helpTextView);
        builtWithTextView = (TextView) findViewById(R.id.builtWithTextView);

        addListenerOnClickables();

    }

    /**
     * onCreateOptionsMenu
     * @param menu
     * @return true|false
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
        backButton.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                backButton_Clicked(arg0);
            }
        });

        learnMoreButton = (Button) findViewById(R.id.learnMoreButton);
        learnMoreButton.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View arg0) {
                learnMoreButton_Clicked(arg0);
            }
        });

    }


    /**
     * backButton_Clicked finish about activity
     * @param arg0 current View
     */
    public void backButton_Clicked(View arg0) {
        // finish activity
        finish();
    }


    /**
     * learnMoreButton_Clicked_Clicked
     * @param arg0
     */
    public void learnMoreButton_Clicked(View arg0) {

        helpTextView.setText(R.string.help_text);

        Intent intent = new Intent();
        intent.setAction(Intent.ACTION_VIEW);
        String github = getString(R.string.github_uri);
        intent.setData(android.net.Uri.parse(github));
        startActivity(intent);
    }


}