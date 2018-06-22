package at.area23;

import at.area23.*;

import java.awt.Component;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.MediaTracker;
import java.awt.Toolkit;
import java.net.URL;
import java.net.MalformedURLException;
import java.beans.PropertyVetoException;
import java.beans.PropertyChangeListener;
import java.beans.VetoableChangeListener;
import java.io.ObjectInputStream;
import java.io.IOException;
import java.util.ResourceBundle;
import java.text.MessageFormat;

//	01/29/97	TWB	Integrated changes from Windows
//	02/19/97	RKM	Changed default for centerMode to correspond with DES file
//					Changed setCenterMode to call repaint instead of invalidate
//					Changed setCenterMode to repaint only if value had changed
//
//	05/29/97	MSH	Updated to support Java 1.1
//  07/16/97    CAR marked fields transient as needed
//                  implemented readObject to load an Image after deserialization
//  07/17/97    CAR added add/remove property/vetoable ChangeListner methods
//  08/04/97    LAB Made lightweight.  Implemented setStyle and getStyle.  Deprecated
//					setCentered and getCentered.
//  08/27/97    CAR update setURL and setFileName to "erase" image if URL or String is null
//  08/04/97    LAB Deprecated URL property and replaced it with ImageURL property (Addresses
//					Mac Bug #7689).  Changed names of strings in PropertyChangeEvent handling
//					to follow Bean Spec naming conventions.

/**
 * ImageViewer component. Provides a platform-independent display of an Image.
 * @version 1.1, August 4, 1997
 * @author	Symantec
 */
public class ImageViewer extends Component implements java.io.Serializable
{
    /**
     * A constant indicating the image is to be tiled in the size of this component.
     */
    public static final int IMAGE_TILED = 0;
    /**
     * A constant indicating the image is to be centered in the size of this component.
     */
    public static final int IMAGE_CENTERED = 1;
    /**
     * A constant indicating the image is to be scaled to fit the size of this component.
     */
    public static final int IMAGE_SCALED_TO_FIT = 2;
    /**
     * A constant indicating the image is to be drawn normally in the upper left corner.
     */
    public static final int IMAGE_NORMAL = 3;

	/**
     * Create default image viewer.
     */
	public ImageViewer()
	{
	    image		= null;
	    fileName	= null;
	    url			= null;
		imageStyle	= IMAGE_CENTERED;
	}

    /**
     * Crate image viewer with filename.  The specified filename is used as
     * the image source.
     *
     * @param str name of file containing the image source
     * @exception MalformedURLException Thrown if URL cannot be generated from filename
     *
     */
	public ImageViewer(String str) throws MalformedURLException
	{
		this();

		try
		{
	    	setFileName(str);
	    }
	    catch(PropertyVetoException e){}
	}

    /**
     * Create image viewer with URL.  The specified URL is used as
     * the image source.
     *
     * @param url the URL of the image to be displayed
     */
	public ImageViewer(URL url)
	{
		this();

		try
		{
			 setURL(url);
		}
		catch ( PropertyVetoException e ) { }
	}

    /**
     * Create image viewer with image.  The specified image is used as
     * the image source
     *
     * @param img the image to be displayed
     */
	public ImageViewer(Image img)
	{
		this();

	    try
		{
			setImage(img);
		}
		catch ( PropertyVetoException e ) { }
	}

	/**
     * Specify or change the image filename.
     *
     * @param str name of file containing image source
     *
     * @exception PropertyVetoException
     * if the specified property value is unacceptable
     */
	public void setFileName(String str) throws PropertyVetoException
	{
		String oldValue = fileName;

	    try
	    {
	    	vetos.fireVetoableChange("fileName", oldValue, str );

	       fileName = str;
	       if(fileName != null && fileName != "")
	           setURL(new URL(fileName));
	       else
	           setURL(null);

	    }
	    catch(MalformedURLException e)
	    {
	        //System.out.println("malformed URL");
	        fileName = oldValue;
	    }
	    repaint();

	    changes.firePropertyChange("fileName", oldValue, str );
	}

	/**
     * Obtain the filename associated with the current image.
     *
     * @return the name of the file, if any, associated with this image.  If
     * no file is associated with this image, returns null
     */
	public String getFileName()
	{
	    return fileName;
	}

    /**
     * Specify or change the image URL.
     *
     * @param aUrl the URL of the image to be displayed
     *
     * @exception PropertyVetoException
     * if the specified property value is unacceptable
     */
	public void setImageURL(URL aUrl) throws PropertyVetoException
	{
		URL oldValue = url;
		vetos.fireVetoableChange("imageURL", oldValue, aUrl );

	    url = aUrl;
	    fileName = null;
		Image loadedImage = null;
		if(url != null){
		    loadedImage = getToolkit().getImage(url);
		}
		setImage(loadedImage);
		repaint();
		changes.firePropertyChange("imageURL", oldValue, aUrl );
	}

   /**
     * Obtain the URL associated with the current image.  If the image
     * was specified by file name, or URL, it will have a URL which
     * indicates its source.  Images created using the constructor
     * with an Image parameter will have no associated URL.
     *
     * @return the name of the URL, if any, associated with this image.  If
               no URL is associated with this image, returns null
     */
	public URL getImageURL()
	{
	    return url;
	}

    /**
     * @deprecated
     * @see #setImageURL
     * @exception PropertyVetoException
     * if the specified property value is unacceptable
     */
	public void setURL(URL aUrl) throws PropertyVetoException
	{
		setImageURL(aUrl);
	}

   /**
     * @deprecated
     * @see #getImageURL
     */
	public URL getURL()
	{
	    return getImageURL();
	}

    /**
     * @deprecated
     * @see #setStyle
     * @exception PropertyVetoException
     * if the specified property value is unacceptable
     */
    public void setCenterMode(boolean flag) throws PropertyVetoException
    {
		if(flag)
		{
			if(getStyle() != IMAGE_CENTERED)
				setStyle(IMAGE_CENTERED);
		}
		else
		{
			if(getStyle() != IMAGE_NORMAL)
				setStyle(IMAGE_NORMAL);
		}
    }

    /**
     * @deprecated
     * @see #getStyle
     */
    public boolean getCenterMode()
    {
        return (getStyle() == IMAGE_CENTERED);
    }

    /**
     * Sets the new panel image style.
     * @param newStyle the new panel image style, one of
     * IMAGE_TILED, IMAGE_CENTERED, or IMAGE_SCALED_TO_FIT
     * @exception PropertyVetoException
     * if the specified property value is unacceptable
     * @see #getStyle
     * @see #IMAGE_TILED
     * @see #IMAGE_CENTERED
     * @see #IMAGE_SCALED_TO_FIT
     * @see #IMAGE_NORMAL
     */
	public void setStyle(int newStyle) throws PropertyVetoException
	{
		if (newStyle != imageStyle)
		{
			Integer oldValue = new Integer(imageStyle);
			Integer newValue = new Integer(newStyle);

			vetos.fireVetoableChange("style", oldValue, newValue);

			imageStyle = newStyle;
	        repaint();

	        changes.firePropertyChange("style", oldValue, newValue);
		}
	}

    /**
     * Gets the current panel image style.
     * @return the current panel image style, one of
     * IMAGE_TILED, IMAGE_CENTERED, or IMAGE_SCALED_TO_FIT
     * @see #setStyle
     * @see #IMAGE_TILED
     * @see #IMAGE_CENTERED
     * @see #IMAGE_SCALED_TO_FIT
     */
	public int getStyle()
	{
		return imageStyle;
	}

    /**
     * Set or change the current image.  Call this method if you want to
     * specify directly the image to display.
     *
     * @param img the image to be displayed
     *
     * @exception PropertyVetoException
     * if the specified property value is unacceptable
     */
	public void setImage(Image img) throws PropertyVetoException
	{
	    fileName = null;
	    Image oldValue = image;

	    vetos.fireVetoableChange( "image", oldValue, img );

		if(image != null)
		{
			image.flush();
			image = null;
		}
	    image    = img;

        if (img != null)
        {
            MediaTracker tracker;

            try
            {
                tracker = new MediaTracker(this);
                tracker.addImage(image, 0);
                tracker.waitForID(0);
            }
            catch(InterruptedException e) { }
        }
        else
      		repaint();

        changes.firePropertyChange( "image", oldValue, img );
	}

    /**
     * Obtain the image currently being displayed.
     *
     * @return the image currently displayed or null if no image
     */
	public Image getImage()
	{
		return image;
	}

	/**
	 * Paints this component using the given graphics context.
     * This is a standard Java AWT method which typically gets called
     * by the AWT to handle painting this component. It paints this component
     * using the given graphics context. The graphics context clipping region
     * is set to the bounding rectangle of this component and its [0,0]
     * coordinate is this component's top-left corner.
     *
     * @param g the graphics context used for painting
     * @see java.awt.Component#repaint
     * @see java.awt.Component#update
	 */
	public void paint(Graphics g)
	{
		super.paint(g);

		Dimension dim = size();
		if (image != null)
		{

			int imageWidth = image.getWidth(this);
			int imageHeight = image.getHeight(this);

			switch(imageStyle)
			{
				case IMAGE_CENTERED:
				default:
				{
					g.drawImage
						(image,
						 (dim.width - imageWidth) / 2,
						 (dim.height - imageHeight) / 2,
						 imageWidth,
						 imageHeight,
						 this);

					break;
				}

				case IMAGE_TILED:
				{
					//Calculate number of images that should be drawn horizontally
					int numHImages = dim.width / imageWidth;

					//Don't forget remainders
					if (dim.width % imageWidth != 0)
						numHImages++;

					//Calculate number of images that should be drawn vertically
					int numVImages = dim.height / imageHeight;

					//Don't forget remainders
					if (dim.height % imageHeight != 0)
						numVImages++;

					int h;
					int v = 0;
					for (int vCount = 0;vCount < numVImages;vCount++)
					{
						h = 0;
						for (int hCount = 0;hCount < numHImages;hCount++)
						{
							g.drawImage(image, h, v, imageWidth, imageHeight, this);

							//Increment to next column
							h += imageWidth;
						}

						//Increment to next row
						v += imageHeight;
					}

					break;
				}


				case IMAGE_SCALED_TO_FIT:
				{
					g.drawImage(image, 0, 0, dim.width, dim.height, this);

					break;
				}

				case IMAGE_NORMAL:
				{
					g.drawImage(image, 0, 0, this);
					break;
				}
			}//switch
		}
		else
		{
		    g.clearRect(0, 0, dim.width, dim.height);
		}
	}

    /**
	 * Returns the recommended dimensions to properly display this component.
     * This is a standard Java AWT method which gets called to determine
     * the recommended size of this component.
     *
     * @return  If no image has been loaded, a dimension of 10 by 10 is returned.
     *          If an image has been loaded, the height and width of the image
     *          is returned.
     *
     * @see #minimumSize
	 */
	public Dimension preferredSize()
	{
		if (image != null)
	        return (new Dimension(image.getWidth(this), image.getHeight(this)));
		else
	        return new Dimension(10, 10);
	}


    /**
	 * Returns the minimum dimensions to properly display this component.
     * This is a standard Java AWT method which gets called to determine
     * the minimum size of this component.
     *
     * @return  If no image has been loaded, a dimension of 10 by 10 is returned.
     *          If an image has been loaded, the height and width of the image
     *          is returned.
     * @see #preferredSize
	 */
	public Dimension minimumSize()
    {
        return preferredSize();
    }

	/**
	 * Adds a listener for all event changes.
	 * @param PropertyChangeListener listener the listener to add.
	 * @see #removePropertyChangeListener
	 */
	public void addPropertyChangeListener(PropertyChangeListener listener)
	{
	    //super.addPropertyChangeListener(listener);
	    changes.addPropertyChangeListener(listener);
	}

	/**
	 * Removes a listener for all event changes.
	 * @param PropertyChangeListener listener the listener to remove.
	 * @see #addPropertyChangeListener
	 */
	public void removePropertyChangeListener(PropertyChangeListener listener)
	{
	    //super.removePropertyChangeListener(listener);
	    changes.removePropertyChangeListener(listener);
	}

	/**
	 * Adds a vetoable listener for all event changes.
	 * @param VetoableChangeListener listener the listener to add.
	 * @see #removeVetoableChangeListener
	 */
	public void addVetoableChangeListener(VetoableChangeListener listener)
	{
	     //super.addVetoableChangeListener(listener);
		vetos.addVetoableChangeListener(listener);
	}

	/**
	 * Removes a vetoable listener for all event changes.
	 * @param VetoableChangeListener listener the listener to remove.
	 * @see #addVetoableChangeListener
	 */
	public void removeVetoableChangeListener(VetoableChangeListener listener)
	{
	    //super.removeVetoableChangeListener(listener);
	    vetos.removeVetoableChangeListener(listener);
	}

    private void readObject(ObjectInputStream stream) throws IOException, ClassNotFoundException {
        stream.defaultReadObject();

        errors = ResourceBundle.getBundle("ErrorsBundle");

        if (url != null) {
    	    image = getToolkit().getImage(url);

    	    MediaTracker tracker = new MediaTracker(this);
    	    tracker.addImage(image, 0);
    	    try {
    	        tracker.waitForAll();
    	    }
    	    catch(InterruptedException e) {
	        Object[] args = { url };
	        throw new IOException(MessageFormat.format(errors.getString("ErrorLoadingImageForURL"), args));
    	    }
	    }
    }

   /**
     * Image that this viewer is displaying.
     */
	transient protected Image image;

    /**
     * Name of file, if any, associated with this image.
     */
	protected String fileName;

    /**
     * URL of the image being displayed.
     */
	protected URL url;

    /**
     * Determines how to draw the image.
     */
	protected int imageStyle;

    /**
     * Error strings.
     */
    transient protected ResourceBundle errors;

	private VetoableChangeSupport vetos   = new VetoableChangeSupport(this);
    private PropertyChangeSupport changes = new PropertyChangeSupport(this);

}
