// package symantec.itools.awt;

import java.beans.PropertyVetoException;

// 	05/31/97	LAB Updated to Java 1.1

/**
 * AlignStyle is an interface for components with labels that can be
 * aligned left, centered, or right.
 * @author Symantec
 */
public interface AlignStyle
{
    //--------------------------------------------------
    // constants
    //--------------------------------------------------

    /**
     * Defines the "left" label text alignment style.
     */
    public static final int ALIGN_LEFT = 0;

    /**
     * Defines the "center" label text alignment style.
     */
    public static final int ALIGN_CENTERED = 1;

    /**
     * Defines the "right" label text alignment style.
     */
    public static final int ALIGN_RIGHT = 2;


    //--------------------------------------------------
    // methods
    //--------------------------------------------------

    /**
     * Sets the new label alignment style.
     * @param style the new alignment style, one of ALIGN_LEFT,
     * ALIGN_CENTERED, or ALIGN_RIGHT
     * @exception PropertyVetoException
     * if the specified property value is unacceptable
     * @see #getAlignStyle
     * @see #ALIGN_LEFT
     * @see #ALIGN_CENTERED
     * @see #ALIGN_RIGHT
     */
    public void setAlignStyle(int style) throws PropertyVetoException;

    /**
     * Gets the current label alignment style.
     * @return the current alignment style, one of ALIGN_LEFT,
     * ALIGN_CENTERED, or ALIGN_RIGHT
     * @see #setAlignStyle
     * @see #ALIGN_LEFT
     * @see #ALIGN_CENTERED
     * @see #ALIGN_RIGHT
     */
    public int getAlignStyle();
}
