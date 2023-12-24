/*
    schnapsennet.js ©
    2023-12-21 by Heinrich Elsigan
    https://area23.at/mono/SchnapsNet/res/schnapsennet.js
    https://darkstar.work/mono/SchnapsNet/res/schnapsennet.js
    2023-12-22 last change 
*/
var im0, im1, im2, im3, im4, imAtou10, imMerge11, imOut20, imOut21, imTalon, spanAtou, spanTalon, urlAtou;
var schnapsState, schnapsUrl;

function schnapsStateInit() {
    setAllVars();
    initStateParamFromUrl();
    schnapsStateSwitch(0);

    if (schnapsState != null) {

        if (schnapsState == 4) {
            initSchnapsState4();
        }
        if (schnapsState == 7) {
            initSchnapsState7();
        }
        if (schnapsState == 8) {
            initSchnapsState8();
        }
        if (schnapsState == 15) {
            schnapsStateRedirect();
        }
    }
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
}

function initStateParamFromUrl() {
    const urlWindowLocation = new URL(window.location.toLocaleString());    
    schnapsUrl = new URL(urlWindowLocation);
    schnapsState = schnapsUrl.searchParams.get("initState");
    // console.log(schnapsState);
}

function initSchnapsState4() {    
    setTimeout(allInvisibleInit, 100);
    setTimeout(playerCards1st3Visible, 1000);
    setTimeout(imOut20a3, 2000);
    setTimeout(imOut21Empty, 3000);
    setTimeout(atouCardVisible, 3250);
    setTimeout(playerCardsVisible, 4250);
    setTimeout(imOut21a2, 5250);
    setTimeout(talonCardVisible, 6250);
    setTimeout(imOut21Empty, 6500);
    setTimeout(schnapsStateRedirect15, 7500);
}

function initSchnapsState7() {    
    setTimeout(allInvisibleInit, 100);
    setTimeout(imOut20a3, 1000);
    setTimeout(playerCards1st3Visible, 2000);
    setTimeout(imOut21Empty, 2250);
    setTimeout(atouCardVisible, 3000);
    setTimeout(imOut21a2, 4000);
    setTimeout(playerCardsVisible, 5000);
    setTimeout(imOut21Empty, 5250);
    setTimeout(talonCardVisible, 6500);
    setTimeout(schnapsStateRedirect15, 7500);
}

function initSchnapsState8() {
    setTimeout(allInvisibleInit(), 100);
    // schnapsStateSwitch(3);
    setTimeout(playerCardsVisible, 1000);
    setTimeout(atouCardVisible, 2000);
    setTimeout(imOut21a2, 3000);
    setTimeout(imOut21Empty, 4000);
    setTimeout(talonCardVisible, 4250);
    setTimeout(schnapsStateRedirect15, 5000);
}

function schnapsStateSwitch(stage) {
    if (stage == null)
        stage = 0;

    if (stage == 0) {
        if (imMerge11 != null)
            imMerge11.style.visibility = "visible";
    }
    if (stage == 1) {
        allInvisibleInit();
    }
    if (stage == 2) {
        playerCardsVisible();
    }
    if (stage == 3) {
        playerCards1st3Visible();
    }
    if (stage == 10) {
        atouCardVisible();
    }
    if (stage == 11) {
        talonCardVisible();
    }
    if (stage == 200) {
        imOut20Empty();
    }
    if (stage == 201) {
        imOut20a3();
    }
    if (stage == 210) {
        imOut21Empty();
    }
    if (stage == 211) {
        imOut21a2();
    }
    if (stage == 1000) {
        schnapsStateRedirect15();
    }
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

function playerCardsVisible() {
    if (im0 != null && im1 != null && im2 != null && im3 != null && im4 != null) {
        im0.style.visibility = "visible";
        im1.style.visibility = "visible";
        im2.style.visibility = "visible";
        im3.style.visibility = "visible";
        im4.style.visibility = "visible";
    }
}

function playerCards1st3Visible() {
    if (im0 != null && im1 != null && im2 != null) {
        im0.style.visibility = "visible";
        im1.style.visibility = "visible";
        im2.style.visibility = "visible";
    }
}

function atouCardVisible() {
    if (spanAtou != null)
        spanAtou.style.visibility = "visible";
    if (imAtou10 != null && urlAtou != null) {
        spanAtou.src = urlAtou;
        spanAtou.style.visibility = "visible";
        spanAtou.src = urlAtou;
    }
}

function talonCardVisible() {
    if (spanTalon != null) 
        spanTalon.style.visibility = "visible";
    if (imTalon != null)
        imTalon.style.visibility = "visible";
}

function imOut20Empty() {
    imOut20 = document.getElementById("imOut20");
    if (imOut20 != null) {
        imOut20.src = "https://area23.at/mono/SchnapsNet/cardpics/e.gif";
        imOut20.style.visibility = "visible";
    }
}

function imOut21Empty() {
    if (imOut21 != null) {
        imOut21.style.visibility = "visible";
        imOut21.src = "https://area23.at/mono/SchnapsNet/cardpics/e.gif";
    }
    if (imOut20 != null) {
        imOut20.style.visibility = "visible";
        imOut20.src = "https://area23.at/mono/SchnapsNet/cardpics/e.gif";
    }
}

function imOut20a3() {    
    imOut20 = document.getElementById("imOut20");
    imOut21 = document.getElementById("imOut21");    
    imAtou10 = document.getElementById("imAtou10");
    spanAtou = document.getElementById("spanAtou");    

    if (imOut21 != null) {
        imOut21.style.visibility = "visible";
        imOut21.src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
    }
    if (imOut20 != null) {
        imOut20.style.visibility = "visible";
        imOut20.src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
    }
    if (imAtou10 != null) {
        imAtou10.src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
        imAtou10.style.visibility = "visible";
    }
    if (spanAtou != null && imAtou10 != null) {        
        imAtou10.style.visibility = "visible";
        imAtou10.src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
        spanAtou.style.visibility = "visible";        
    }
}

function imOut21a2() {
    if (imOut21 != null) {
        imOut21.src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
        imOut21.style.visibility = "visible";
    }
    if (imOut20 != null) {
        imOut20.src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
        imOut20.style.visibility = "visible";
    }    
}

function schnapsStateRedirect() {
    // alert("SchnapsenNet.aspx");
    window.location.href = "SchnapsenNet.aspx";
}

function schnapsStateRedirect15() {
    // alert("SchnapsenNet.aspx?initState=15");
    window.location.href = "SchnapsenNet.aspx?initState=15";
}

function highLightOnOver(highLightId) {
    // alert("highLightOnOver(" + highLightId + ")");
    if (highLightId != null && document.getElementById(highLightId) != null) {
        if (document.getElementById(highLightId).style.borderStyle == "dotted" ||
            document.getElementById(highLightId).style.borderWidth == 2) {
            // do nothing when dotted
        }
        else {
            // set border-width: 1; border-style: dashed
            document.getElementById(highLightId).style.borderWidth = 1;
            document.getElementById(highLightId).style.borderStyle = "dashed";
        }
    }
}

function unLightOnOut(unLightId) {    
    // alert("unLightOnOut(" + unLightId + ")");
    if (unLightId != null && document.getElementById(unLightId) != null) {
        if (document.getElementById(unLightId).style.borderStyle == "dotted" ||
            document.getElementById(unLightId).style.borderWidth == 2) {
            // do nothing when dotted
        }
        else {
            // if (document.getElementById(highLightId).style.borderStyle == "dashed" ||
            //    document.getElementById(highLightId).style.borderWidth == 1) {
            document.getElementById(unLightId).style.borderWidth = 1;
            document.getElementById(unLightId).style.borderStyle = "none";
            // }
        }
    }
}

function playSound(soundName) {
    var dursec = 2600;
    dursec = parseInt(3000);
    if (dursec < parseInt(3000))
        dursec = parseInt(3000);

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
        soundDuration = parseInt(2600);
    }, dursec);
}


function metaAudioNameChanged(metaId) {
    // if (metaId != null && document.getElementById(metaId) != null) {
    var audioId = document.getElementById(metaAudioId);
    if (audioId != null && audioId.name != null) {
        alert("audioId.name = " + audioId.name);
        playSound(audioId.name);
    }   
}


function aAudioChanged() {
    // if (metaId != null && document.getElementById(metaId) != null) {
    var aAudioId = document.getElementById(aAudio);
    if (aAudioId != null) {
        if (aAudioId.name != null) {
            alert("audioId.name = " + audioId.name);
            playSound(audioId.name);
        }
    }
}
