#/bin/sh

# make sure, that java sdk tools are included in %PATH%
# and that CLASSPATH is set correctly
# or set it manually be commenting out next line:
echo starting make by using javac.exe jar.exe java.exe 
echo CLASSPATH = $CLASSPATH
echo 
# echo set CLASSPATH=\"C:\Program Files\Java\jdk-18.0.2\lib\"
# set CLASSPATH="C:\Program Files\Java\jdk-18.0.2\lib"
CLASSPATH=/usr/share/java/libintl-0.23.2.jar:/usr/share/java/libintl.jar:/usr/share/java/java-atk-wrapper.jar:/usr/share/java/gettext.jar:/usr/lib/jvm/java-25-openjdk-amd64/lib/jrt-fs.jar:/usr/lib/jvm/java-25-openjdk-amd64/lib/

echo 
MYCLASSPATH=$CLASSPATH:at/area23/:at/area23/schnapsen/:at/area23/schnapsen/cardpics/:./
# MYCLASSPATH="$CLASSPATH;./;./at/area23/;./at/area23/schnapsen/;./at/area23/schnapsen/cardpics/;" 
echo 
echo "... compiling with javac ... with classpath = $CLASSPATH myclp = $MYCLASSPATH"

javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/Context.java
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/GetFrame.java
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/ImagePanel.java
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/Card.java     
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/Player.java
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/Game.java     
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/Schnapslet.java


echo ... continuing compelation of schnapslet.java ...

sleep 2
# echo 
# echo ... building schnapsen.jar with jar.exe ...


echo "... creating schnapser.jar"
jar --create --verbose --file schnapser.jar --manifest META-INF/MANIFEST.MF --main-class at/area23/schnapsen/Schnapslet at/area23/schnapsen/Schnapslet.class at/area23/schnapsen/ImagePanel.class at/area23/schnapsen/Context.class at/area23/schnapsen/GetFrame.class at/area23/schnapsen/Card.class at/area23/schnapsen/Game.class at/area23/schnapsen/Game$messageQueue.class at/area23/schnapsen/Player.class at/area23/schnapsen/Schnapslet$SymAction.class at/area23/schnapsen/Schnapslet$SymMouse.class at/area23/schnapsen/Schnapslet.class  at/area23/schnapsen/cardpics/*.gif cardpics/*.gif

echo "--- sleep 3 ..."
sleep 3

echo "... building area23Schnapslet.java at/Area23Schnapslet.java ... jar borh archives ..."
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation  area23Schnapslet.java  
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/Area23Schnapslet.java  

jar --create --verbose --file area23Schnapser.jar --manifest META-INF/MANIFEST.MF --main-class at/Area23Schnapslet @classes.list
jar --create --verbose --file area23Schnapsen.jar --manifest META-INF/MANIFEST.MF --main-class area23Schnapslet.class @classes.list 


sleep 2
# appletviewer is obsolete
# appletviewer appletviewer.exe index.htm

echo
echo "Nuild finished, press any key to launch schnapslet with java.exe"
sleep 2
java -jar area23Schnapser.jar --main-class at/Area23Schnapslet.class



