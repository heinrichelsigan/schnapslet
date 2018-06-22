package at.area23;

import at.area23.*;
import java.awt.*;

public class GetFrame {

	/**
	 * Finds the frame that encloses an applet, or if one can't be found,
	 * creates a new frame.
	 */
	public static Frame find(Container container) {
		Container theFrame = container;
		do {
			theFrame = theFrame.getParent();
		} while ((theFrame != null) && !(theFrame instanceof Frame));
		if (theFrame == null)
			theFrame = new Frame();
	    return (Frame) theFrame;
	}

}
