/*
    schnapsnet.js ©
    2023-12-13 by Heinrich Elsigan
    https://area23.at/mono/SchnapsNet/res/schnapsnet.js
    https://darkstar.work/mono/SchnapsNet/res/schnapsnet.js
    2023-12-22 last change 
*/
var im0, im1, im2, im3, im4, imAtou10, imMerge11, imOut20, imOut21, imTalon, spanAtou, spanTalon;
var helpWin = null, schnapsState, schnapsUrl, soundDuration = 7200, urlAtou;

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
    initStateParamFromUrl();
    // schnapsStateSwitch(0);

    if (schnapsState == null) { 
        aAudioLoaded();
    }
}

function setAllVars() {
    schnapsState = 0;
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
}

function initStateParamFromUrl() {
    const urlWindowLocation = new URL(window.location.toLocaleString());
    schnapsUrl = new URL(urlWindowLocation);
    try {
        schnapsState = schnapsUrl.searchParams.get("initState");
    } catch (Exception) {

    }
    // console.log(schnapsState);
}

function allInvisibleInit() {
    if (imOut21 != null)
        imOut21.style.visibility = "hidden";
    if (imOut20 != null)
        imOut20.style.visibility = "hidden";
    if (imMerge11 != null)
        imMerge11.style.visibility = "hidden";

    if (im0 != null)
        im0.style.visibility = "hidden";
    if (im1 != null)
        im1.style.visibility = "hidden";
    if (im2 != null)
        im2.style.visibility = "hidden";
    if (im3 != null)
        im3.style.visibility = "hidden";
    if (im4 != null)
        im4.style.visibility = "hidden";

    if (spanAtou != null) {
        if (imAtou10 != null)
            imAtou10.style.visibility = "hidden";
        spanAtou.style.visibility = "hidden";
    }
    if (spanTalon != null) {
        if (imTalon != null)
            imTalon.style.visibility = "hidden";
        spanTalon.style.visibility = "hidden";
    }
}

function schnapsStateRedirect() {
    // alert("SchnapsenNet.aspx");
    window.location.href = "SchnapsNet.aspx";
}


function highLightOnOver(highLightId) {

    if (highLightId != null && document.getElementById(highLightId) != null) {

        if (document.getElementById(highLightId).style.borderStyle == "dotted" ||
            document.getElementById(highLightId).style.borderColor == "deeppink") {
            // change only color, when dotted
            document.getElementById(highLightId).style.borderColor = "blueviolet";
            return; 
        }                

        if ((document.getElementById("b20a") != null && document.getElementById("b20a").style.borderColor == "purple") ||
            (document.getElementById("b20b") != null && document.getElementById("b20b").style.borderColor == "purple"))
            return; // don't highlight other cards in case of pair marriage

        // set border-width: 2; border-style: dashed
        document.getElementById(highLightId).style.borderWidth = 2;
        document.getElementById(highLightId).style.borderStyle = "dashed";
        document.getElementById(highLightId).style.borderColor = "blueviolet";
    }
}

function unLightOnOut(unLightId) {

    if (unLightId != null && document.getElementById(unLightId) != null) {

        if (document.getElementById(unLightId).style.borderStyle == "dotted" &&
            document.getElementById(unLightId).style.borderColor == "blueviolet") {
            // change only back to pair color, when dotted
            document.getElementById(unLightId).style.borderColor = "deeppink";
            return; 
        }

        // if (document.getElementById(unLightId).style.borderStyle == "dashed" ||
        //    document.getElementById(unLightId).style.borderWidth == 1) {
        document.getElementById(unLightId).style.borderWidth = 2;
        document.getElementById(unLightId).style.borderStyle = "groove";
        document.getElementById(unLightId).style.borderColor = "azure";
        // }
    }
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
    var aAudio = document.getElementById('aAudio');
    if (aAudio != null) {
        let aHref = aAudio.getAttribute("href");
        if (aHref == null || aHref == "")
            return;

        setTimeout(function () { playSound(aHref); }, 400);
    }
}

function audioOutputChanged() {
    // if (metaId != null && document.getElementById(metaId) != null) {
    var aAudioOutput = document.getElementById("audioOutput");
    if (aAudioOutput != null) {
        let aOutputName = aAudioOutput.getAttribute("name");
        if (aOutputName != null) {
            let aAudioValue = aAudioOutput.getAttribute("value");
            if (aAudioValue != null) {
                alert("audioOutput.name = " + aOutputName);
                alert("audioOutput.value = " + aAudioValue);
                playSound(audioId.value);
            }
        }
    }
}