@echo on

REM make sure, that java sdk tools are included in %PATH%

javac.exe -Xlint:unchecked -Xlint:deprecation at\area23\*.java
javac.exe -Xlint:unchecked -Xlint:deprecation card.java
javac.exe -Xlint:unchecked -Xlint:deprecation player.java
javac.exe -Xlint:unchecked -Xlint:deprecation game.java
javac.exe -Xlint:unchecked -Xlint:deprecation schnapslet.java

REM jar cvf schnapsen.jar *.class at\area23\*.class at\area23\*.java *.java cardpics\*.gif
REM appletviewer appletviewer.exe index.html  

java schnapslet
