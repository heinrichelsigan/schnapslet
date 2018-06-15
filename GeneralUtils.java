// package symantec.itools.util;

import java.lang.IllegalArgumentException;
import java.util.ResourceBundle;
import java.text.MessageFormat;


public final class GeneralUtils {

    public GeneralUtils() {
    }


     public static String frameTarget_self = "_self";

     public static String frameTarget_parent = "_parent";

     public static String frameTarget_top = "_top";

     public static String frameTarget_blank = "_blank";

	public static boolean objectsEqual(Object objectA,Object objectB) {
		if (objectA == null)
			return (objectB == null);
		return objectA.equals(objectB);
	}

	public static void checkValidPercent(double percent) throws IllegalArgumentException {
		if(percent > 1 || percent < 0){
		    Object[] args = { new Double(percent) };
			throw new IllegalArgumentException(MessageFormat.format(errors.getString("InvalidPercent2"), args));
		}
	}

	public static String removeCharAtIndex(String string, int index) {
		if(string == null || string == "")
			return string;

		int length		= string.length();

		if(index > length || index < 0)
			return string;

		String left	= index > 0 ? string.substring(0, index) : "";
		String right	= index + 1 < length ? string.substring(index + 1) : "";

		return left + right;
	}

    static protected ResourceBundle errors = ResourceBundle.getBundle("ErrorsBundle");

}
