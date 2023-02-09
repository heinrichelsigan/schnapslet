@echo off

REM make sure, that java sdk tools are included in %PATH%
REM and that CLASSPATH is set correctly
REM or set it manually be commenting out next line:
echo starting make by using javac.exe jar.exe java.exe 
echo CLASSPATH = %CLASSPATH%  
echo REM JAVA CLASSPATH
echo 
echo REM when CLASSPATH  %CLASSPATH% is not correct change it, by setting it like:
echo set CLASSPATH=\"C:\Program Files\Java\jdk-18.0.2\lib\"
REM set CLASSPATH="C:\Program Files\Java\jdk-18.0.2\lib"

echo 
echo set MYCLASSPATH=%CLASSPATH%;.\;.\at\area23\;
set MYCLASSPATH=%CLASSPATH%;.\;.\at\area23\;
echo 
echo compiling with javac ...
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation at\area23\*.java
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation at\area23\schnapslet\*.java


echo 
echo set MYCLASSPATH=%CLASSPATH%;.\;.\at\area23;.\at\area23\schnapslet
set MYCLASSPATH=%CLASSPATH%;.\;.\at\area23;.\at\area23\schnapslet
echo ... continuing compelation of schnapslet.java ...

javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation card.java
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation player.java
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation game.java
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation schnapslet.java

echo 
echo ... building schnapsen.jar with jar.exe ...
jar cvf schnapsen.jar *.class at\area23\schnapslet\card.class at\area23\schnapslet\game.class at\area23\schnapslet\game$messageQueue.class at\area23\schnapslet\player.class at\area23\schnapslet\schnapslet.class at\area23\schnapslet\schnapslet$SymAction.class at\area23\schnapslet\schnapslet$SymMouse.class at\area23\*.class *.java at\area23\*.java at\area23\schnapslet\card.java at\area23\schnapslet\game.java at\area23\schnapslet\player.java at\area23\schnapslet\schnapslet.java cardpics\*.gif

REM 
REM appletviewer is obsolete
REM appletviewer appletviewer.exe index.htm

echo
echo "Nuild finished, press any key to launch schnapslet with java.exe"
pause
start java -classpath %MYCLASSPATH% schnapslet



