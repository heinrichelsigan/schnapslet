package at.area23.schnapslet;

import android.content.Context;
import androidx.test.ext.junit.runners.*;
import androidx.test.ext.junit.runners.AndroidJUnit4;
import androidx.test.*;
import androidx.test.platform.app.InstrumentationRegistry;
// import android.support.test.InstrumentationRegistry;
// import android.support.test.runner.AndroidJUnit4;

import org.junit.Test;
import org.junit.runner.RunWith;

import static org.junit.Assert.*;

/**
 * Instrumented test, which will execute on an Android device.
 *
 * @see <a href="http://d.android.com/tools/testing">Testing documentation</a>
 */
@RunWith(androidx.test.ext.junit.runners.AndroidJUnit4.class)
public class ExampleInstrumentedTest {
    @Test
    public void useAppContext() {
        // Context of the app under test.
        Context appContext = InstrumentationRegistry.getTargetContext();

        assertEquals("at.area23.schnapslet", appContext.getPackageName());
    }
}
