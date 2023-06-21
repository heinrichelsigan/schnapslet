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

package at.area23.schnapslet.constenum;

import java.util.Calendar;
import java.util.Date;

/**
 * Constants contains application constants
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class Constants {

	public final static String APP_NAME = "Schnapslet";
	public final static String VERSION     = "v2.4.23";
	public final static String GITURL      = "https://github.com/heinrichelsigan/schnapslet";
	public final static String WIKIURL     = "https://github.com/heinrichelsigan/schnapslet/wiki";
	public final static String URLPREFIX   = "https://area23.at/schnapsen/cardpics/";
	public final static String URLPIC      = "https://area23.at/schnapsen/cardpics/";

	public final static String PARAM1_INDEX = "Index";
	public final static String PARAM2_POINTS = "Points";

	public final static String URL_PREFIX = "https://area23.at/schnapsen/cardpics/";

	public final static String IMG_JPG_SUFFIX = ".jpg";
	public final static String IMG_PNG_SUFFIX = ".png";
	public final static String IMG_BMP_SUFFIX = ".bmp";
	public final static String IMG_GIF_SUFFIX = ".gif";

	public final static String FILE_TXT_SUFFIX = ".txt";
	public final static String FILE_JSON_SUFFIX = ".json";

	public final static String COLOR_K     = "♦";  //  "&#9830;";    //  "&diams;"
	public final static String COLOR_H     = "♥";  //  "&#9829;";    //  "&hearts;"
	public final static String COLOR_P     = "♠";  //  "&#9824;";    //  "&spades;"
	public final static String COLOR_T     = "♣";  //  "&#9827;";    //  "&clubs;"
	public final static String COLOR_N     = "NOCOLOR";
	public final static String COLOR_E     = "EMPTYCOLOR";
	public final static String TAYLOR_SYM0 = "&#x2702;";       // ✂
	public final static String TAYLOR_SYM1 = "&#9986;";
	public final static String TAYLOR_SYM2 = "&#x2704;";       // ✄

	/**
	 * getSaveImageFileName
	 * gets application specific
	 * unique save file name for images (screenshots)
	 *
	 * @param compress true, if jpeg compression wanted,
	 *                 otherwise false if png wanted
	 * @return application specific unique save file name
	 */
	public static String getSaveImageFileName(boolean compress) {

		String datePart = getDateString();
		String timePart = getTimeString(true);
		String saveName = APP_NAME + "_" + datePart + "-" + timePart;
		String saveFullName = saveName + ((compress) ? IMG_JPG_SUFFIX : IMG_PNG_SUFFIX);

		return saveFullName;
	}

	/**
	 * getDateString
	 *
	 * @return String representing current date "YYYY-MM-DD"
	 */
	public static String getDateString() {
		Calendar calendar = Calendar.getInstance();

		String yearStr = String.valueOf(calendar.get(Calendar.YEAR));
		String monthStr = (calendar.get(Calendar.MONTH) < 10) ?
				"0" : "" + String.valueOf(calendar.get(Calendar.MONTH));
		String dayStr = (calendar.get(Calendar.DAY_OF_MONTH) < 10) ?
				"0" : "" + String.valueOf(calendar.get(Calendar.DAY_OF_MONTH));

		String dateString = yearStr + "-" + monthStr + "-" + dayStr;
		return dateString;
	}

	/**
	 * getTimeString
	 *
	 * @param showMillis true to show additionally milliseconds
	 * @return String representing current time "HHMMSS" or "HHMMSS_mmmm"
	 */
	public static String getTimeString(boolean showMillis) {
		Calendar calendar = Calendar.getInstance();

		String hourStr = (calendar.get(Calendar.HOUR_OF_DAY) < 10) ?
				"0" : "" + String.valueOf(calendar.get(Calendar.HOUR_OF_DAY));
		String minuteStr = (calendar.get(Calendar.MINUTE) < 10) ?
				"0" : "" + String.valueOf(calendar.get(Calendar.MINUTE));
		String secondStr = (calendar.get(Calendar.SECOND) < 10) ?
				"0" : "" + String.valueOf(calendar.get(Calendar.SECOND));
		String milliStr = (calendar.get(Calendar.MILLISECOND) < 10) ?
				"00" : ((calendar.get(Calendar.MILLISECOND) < 100) ?
				"0" : String.valueOf(calendar.get(Calendar.MILLISECOND)));

		String timeString = hourStr + minuteStr + secondStr;
		if (showMillis)
			timeString += "_" + milliStr;

		return timeString;
	}

	/**
	 * getTimeString
	 *
	 * @return String representing current time "HHMMSS"
	 */
	public static String getTimeString() {
		return getTimeString(false);
	}

}
