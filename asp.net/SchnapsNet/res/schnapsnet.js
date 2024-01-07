/*
    schnapsnet.js ©
    2023-12-13 by Heinrich Elsigan
    https://area23.at/mono/SchnapsNet/res/schnapsnet.js
    https://darkstar.work/mono/SchnapsNet/res/schnapsnet.js
    2023-12-22 last change 
*/
var im0, im1, im2, im3, im4, imAtou10, imMerge11, imOut20, imOut21, imTalon, spanAtou, spanTalon, metaAudio, metaLastAudio;
var helpWin = null, schnapsState, schnapsUrl, soundDuration = 7200, urlAtou, audioContent, lastAudioContent;

function HelpOpen() {
    var Mleft = (screen.width / 2) - (720 / 2);
    var Mtop = (screen.height / 2) - (640 / 2);
    if (helpWin == null)
        helpWin = window.open('Help.aspx', 'helpWin',
            'height=640,width=720,location=yes,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=yes,toolbar=no,top=\' + Mtop + \', left=\' + Mleft + \'');
    else try {
        helpWin.focus();
    } catch (Exception) {
    }
}

function schnapsStateInit() {
    setAllVars();
    computerSaidPair();
    aAudioLoaded();    
}

function setAllVars() {
    im0 = document.getElementById("im0");
    im1 = document.getElementById("im1");
    im2 = document.getElementById("im2");
    im3 = document.getElementById("im3");
    im4 = document.getElementById("im4");

    imAtou10 = document.getElementById("imAtou10");
    if (imAtou10 != null && imAtou10.src != null)
        urlAtou = imAtou10.src;
    imMerge11 = document.getElementById("imMerge11");
    imOut20 = document.getElementById("imOut20");
    imOut21 = document.getElementById("imOut21");
    imTalon = document.getElementById("imTalon");

    spanAtou = document.getElementById("spanAtou");
    spanTalon = document.getElementById("spanTalon");

    metaAudio = document.getElementById("metaAudio");
    metaLastAudio = document.getElementById("metaLastAudio");
    audioContent = "";
}

function highLightOnOver(highLightId) {
    // alert("highLightOnOver(" + highLightId + ")");
    if (highLightId != null && document.getElementById(highLightId) != null) {
        if (document.getElementById(highLightId).style.borderStyle == "dotted" ||
            document.getElementById(highLightId).style.borderColor == "deeppink") {
            // change only color, when dotted
            document.getElementById(highLightId).style.borderColor = "blueviolet";
        }
        else {
            // set border-width: 1; border-style: dashed
            document.getElementById(highLightId).style.borderWidth = 2;
            document.getElementById(highLightId).style.borderStyle = "dashed";
            document.getElementById(highLightId).style.borderColor = "blueviolet";
        }
    }
}

function unLightOnOut(unLightId) {
    // alert("unLightOnOut(" + unLightId + ")");
    if (unLightId != null && document.getElementById(unLightId) != null) {
        if (document.getElementById(unLightId).style.borderStyle == "dotted" &&
            document.getElementById(unLightId).style.borderColor == "blueviolet") {
            // change only back to pair color, when dotted
            document.getElementById(unLightId).style.borderColor = "deeppink";
        }
        else {
            // if (document.getElementById(unLightId).style.borderStyle == "dashed" ||
            //    document.getElementById(unLightId).style.borderWidth == 1) {
            document.getElementById(unLightId).style.borderWidth = 2;
            document.getElementById(unLightId).style.borderStyle = "groove";
            document.getElementById(unLightId).style.borderColor = "azure";
            // }
        }
    }
}

function computerSaidPair() {
    if (imOut20 == null)
        imOut20 = document.getElementById("imOut20");
    if (imOut21 == null)
        imOut21 = document.getElementById("imOut21");
    if (imOut21 != null && imOut20 != null &&
        imOut20.style.borderColor == "purple" &&
        imOut20.style.borderStyle == "dashed") {
        setTimeout(function () {
            imOut20.style.borderColor = "none";
            imOut20.style.borderStyle = "none";
            imOut20.style.borderWidth = 1;
            imOut21.style.borderColor = "none";
            imOut21.style.borderStyle = "none";
            imOut21.style.borderWidth = 1;
            imOut20.src = "https://area23.at/schnapsen/cardpics/e.gif";
        }, 1250);
    }
    return;
}

function playSound(soundName) {
    const urlWindowLocation = new URL(window.location.toLocaleString());

    var dursec = parseInt(soundDuration);
    if (dursec < parseInt(4800))
        dursec = parseInt(4800);

    let soundUrlString = urlWindowLocation.origin + '/' + soundName;
    let soundUrl = new URL(soundUrlString);

    let sound = new Audio(soundName);

    sound.autoplay = true;
    sound.loop = false;

    setTimeout(function () { sound.play(); }, 100);

    setTimeout(function () {
        sound.loop = false;
        sound.pause();
        sound.autoplay = false;
        sound.currentTime = 0;
        if (metaAudio == null)
            metaAudio = document.getElementById('metaAudio');
        metaAudio.setAttribute("content", "");
        if (metaLastAudio == null)
            metaLastAudio = document.getElementById('metaLastAudio');
        metaLastAudio.setAttribute("content", "");
        try {
            sound.src = "";
            sound = null;
        } catch (exSnd) {
        }
        soundDuration = parseInt(7200);
    }, dursec);
}

function aAudioLoaded() {
    // if (metaId != null && document.getElementById(metaId) != null) {
    if (metaAudio == null)
        metaAudio = document.getElementById('metaAudio');

    if (metaLastAudio == null) 
        metaLastAudio = document.getElementById('metaLastAudio');

    if (metaAudio != null) {
        let aContent = metaAudio.getAttribute("content");
        if (aContent == null || aContent == "") {
            audioContent = "";
            lastAudioContent = "";
            return;
        }
        let aLastContent = metaLastAudio.getAttribute("content");


        if (audioContent == aContent || aContent == aLastContent) {
            // audioContent = "";
            return;
        }
        
        audioContent = aContent;
        metaLastAudio.setAttribute("content", audioContent);
        
        setTimeout(function () { playSound(aContent); }, 400);
    }
}

function audioOutputChanged() {
    // if (metaId != null && document.getElementById(metaId) != null) {
    var aAudioOutput = document.getElementById("metaAudio");
    if (aAudioOutput != null) {
        let aOutputName = aAudioOutput.getAttribute("name");
        if (aOutputName != null) {
            let aAudioValue = aAudioOutput.getAttribute("content");
            if (aAudioValue != null && aAudioValue != "") {
                alert("audioOutput.name = " + aOutputName);
                alert("audioOutput.value = " + aAudioValue);
                playSound(audioId.value);
            }
        }
    }
}