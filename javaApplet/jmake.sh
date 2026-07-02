#/bin/sh

# make sure, that java sdk tools are included in %PATH%
# and that CLASSPATH is set correctly
# or set it manually be commenting out next line:
echo starting make by using javac.exe jar.exe java.exe 
echo CLASSPATH = $CLASSPATH
echo 
# echo set CLASSPATH=\"C:\Program Files\Java\jdk-18.0.2\lib\"
# set CLASSPATH="C:\Program Files\Java\jdk-18.0.2\lib"

echo 
set MYCLASSPATH=$CLASSPATH:./:./at/area23/:./at/area23/schnapsen/:./at/area23/schnapsen/cardpics/
# MYCLASSPATH="$CLASSPATH;./;./at/area23/;./at/area23/schnapsen/;./at/area23/schnapsen/cardpics/;" 
echo 
echo compiling with javac ...

javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/Context.java
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/GetFrame.java
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/ImagePanel.java
# javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/*.java
# javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/Card.java
# javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/Game.java
# javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/Player.java
# javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/Schnapslet.java

javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation at/area23/schnapsen/*.java


echo ... continuing compelation of schnapslet.java ...


# echo 
# echo ... building schnapsen.jar with jar.exe ...

# jar --create --verbose --file schnapsen.jar --manifest META-INF/MANIFEST.MF at/area23/ImagePanel.class at/area23/GetFrame.class at/area23/Context.class at/area23/schnapsen/Card.class at/area23/schnapsen/Game.class at/area23/schnapsen/Game$messageQueue.class at/area23/schnapsen/Player.class at/area23/schnapsen/Schnapslet$SymAction.class at/area23/schnapsen/Schnapslet$SymMouse.class at/area23/schnapsen/Schnapslet.class at/area23/schnapsen/Schnapslet.class  at/area23/schnapsen/cardpics cardpics

echo "... creating schnapser.jar"
jar --create --verbose --file schnapser.jar --main-class at/area23/schnapsen/Schnapslet.class at/area23/schnapsen/ImagePanel.class at/area23/schnapsen/Context.class at/area23/schnapsen/GetFrame.class at/area23/schnapsen/Card.class at/area23/schnapsen/Game.class at/area23/schnapsen/Game$messageQueue.class at/area23/schnapsen/Player.class at/area23/schnapsen/Schnapslet$SymAction.class at/area23/schnapsen/Schnapslet$SymMouse.class at/area23/schnapsen/Schnapslet.class  at/area23/schnapsen/cardpics/*.gif cardpics/*.gif

sleep 1

javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation area23Schnapsen.java

jar --create --verbose --file area23Schnapser.jar --main-class area23Schnapslet.class area23Schnapslet$SymAction.class area23Schnapslet$SymMouse.class area23Schnapslet.class at/area23/schnapsen/Schnapslet.class at/area23/schnapsen/ImagePanel.class at/area23/schnapsen/Context.class at/area23/schnapsen/GetFrame.class at/area23/schnapsen/Card.class at/area23/schnapsen/Game.class at/area23/schnapsen/Game$messageQueue.class at/area23/schnapsen/Player.class at/area23/schnapsen/cardpics/*.gif cardpics/*.gif



# appletviewer is obsolete
# appletviewer appletviewer.exe index.htm

echo
echo "Nuild finished, press any key to launch schnapslet with java.exe"
sleep 2
java -jar area23Schnapser.jar --main-class area23Schnapslet.class



