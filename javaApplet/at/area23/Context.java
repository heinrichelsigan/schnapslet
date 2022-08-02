/*
	Class:          Animator
	Package:        ioutil
	Author:         Symantec
    Version:        1.0, June 15, 1997
    Modifier:       Heinrich Elsigan
  	Last Updated:   20. Februar 2001
	
	
	Project builded with Symantec Visual Cafe 3.0
    Symantec Corporation, Visual Cafe 1998
 */

package at.area23;

import at.area23.*;
import java.applet.Applet;
import java.net.URL;

/**
 * This class tracks information relating to the Browser/Applet Viewer that
 * the program is currently running under.
 * it is used in conjunction with class symantec.itools.net.RelativeURL to
 * provide URLs that are relative to an applet or application document base.
 * For applets the document base is the URL of the document that the applet is
 * embedded in. For applications the document base is the same as the user.dir
 * system property.
 */
public class Context
{
	private static Applet applet;
	private static URL documentBase;
	private static boolean initialized = false;

    /**
     * Do not use, this is an all-static class.
     */
    public Context() {
    }

    /**
     * Returns true if this is being used with an applet,
     * otherwise this is being used with an application.
     * @see #isApplication
     * @see #getApplet
     */
	public static boolean isApplet()
	{
		return applet != null;
	}

    /**
     * Returns true if this is being used with an application,
     * otherwise this is being used with an applet.
     * @see #isApplet
     */
	public static boolean isApplication()
	{
		return applet == null;
	}

    /**
     * Notes the applet this class is being used with.
     * @param a the applet to use this class with.
     * @see #getApplet
     */
	public static void setApplet(Applet a)
	{
		applet = a;
        initialized = true;
	}

    /**
     * Returns the current applet, or null if being used with an application.
     * @see #setApplet
     */
	public static Applet getApplet()
	{
		return applet;
	}

	/**
	 * Manually sets the current document base.
	 * If the document base is not manually specified it will be automatically
	 * determined.
     * @param db the new document base
     * @see #getDocumentBase
     */
	public static void setDocumentBase(URL db)
	{
        // Hack because Netscape Communicator 4.0 Preview 4 for Windows
        // will not look up documents off of the local file system based
        // on the URL returned from java.applet.Applet.getDocumentBase()
        if (System.getProperty("java.vendor").startsWith("Netscape") &&
            db.getHost() == "" &&
            System.getProperty("os.name").startsWith("Windows")) {

            String f = db.getFile();
            String hd = f.substring(0, f.indexOf(':') + 1);
            f = f.substring(f.indexOf(':') + 1);
            try {
                documentBase = new URL(db.getProtocol(), hd, f);
            }
            catch (java.net.MalformedURLException e) { }
        }

        else {
    		documentBase = db;
	    }

        initialized = true;
	}

	private static void initializeApp()
	{
        StringBuffer p = new StringBuffer(System.getProperty("user.dir"));
        int         pl = p.length();

        // If the system file separator isn't the URL file separator convert it.
        try
        {
            char ps = (System.getProperty("file.separator")).charAt(0);
            if(ps != '/')
                for(int counter = 0; counter < pl; counter++)
                {
                    if(ps == p.charAt(counter)) p.setCharAt(counter, '/');
                }
        }
        catch (StringIndexOutOfBoundsException e) { }

        try {
            documentBase = new URL("file:///" + p + "/");
        }
        catch (java.net.MalformedURLException e) { }
	}

    /**
     * Returns the current document base.
     * For applets the document base is the URL of the document that the
     * applet is embedded in. For applications the document base is the same as
     * the user.dir system property.
     * @see #setDocumentBase
     */
	public static URL getDocumentBase()
	{
		if (! initialized) {
            initializeApp();
            initialized = true;
        }

		if(documentBase != null)
		{
			return documentBase;
		}
		else if (applet != null)
		{
			return applet.getDocumentBase();
		}

		return null;
	}
}