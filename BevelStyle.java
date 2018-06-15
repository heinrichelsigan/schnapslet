package symantec.itools.awt;

import java.beans.PropertyVetoException;

// 	05/31/97	LAB Updated to Java 1.1

/**
 * BevelStyle is an interface for components with borders that can be drawn
 * different ways. The "raised" style makes the component appears raised above
 * the surrounding area, the "lowered" style makes the component appear lowered,
 * the "line" style draws a simple line around the component, and components with
 * the "none" style have nothing drawn around them.
 * @author Symantec
 */
public interface BevelStyle
{
    //--------------------------------------------------
    // constants
    //--------------------------------------------------

    /**
     * Defines the "lowered" shape border style. This makes the component appear
     * lower than the surrounding area.
     */
    public static final int BEVEL_LOWERED = 0;

    /**
     * Defines the "raised" shape border style. This makes the component appear
     * raised above the surrounding area.
     */
    public static final int BEVEL_RAISED = 1;

   /**
     * Defines the "line" shape border style. This draws a simple line around the
     * component.
     */
    public static final int BEVEL_LINE = 2;

   /**
     * Defines the "none" shape border style. This indicates the component will have
     * nothing drawn around its border.
     */
    public static final int BEVEL_NONE = 3;


    //--------------------------------------------------
    // methods
    //--------------------------------------------------

    /**
     * Sets the new border style.
     * @param style the new border style, one of BEVEL_LOWERED, BEVEL_RAISED,
     * BEVEL_LINE, or BEVEL_NONE
     * @exception PropertyVetoException
     * if the specified property value is unacceptable
     * @see #getBevelStyle
     * @see #BEVEL_LOWERED
     * @see #BEVEL_RAISED
     * @see #BEVEL_LINE
     * @see #BEVEL_NONE
     */
    public void setBevelStyle(int style) throws PropertyVetoException;
    /**
     * Gets the current border style.
     * @return the current border style, one of BEVEL_LOWERED, BEVEL_RAISED,
     * BEVEL_LINE, or BEVEL_NONE
     * @see #setBevelStyle
     * @see #BEVEL_LOWERED
     * @see #BEVEL_RAISED
     * @see #BEVEL_LINE
     * @see #BEVEL_NONE
     */
    public int getBevelStyle();
}
