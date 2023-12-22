/*
    schnapsennet.js ©
    2023-12-21 by Heinrich Elsigan
    https://area23.at/mono/SchnapsNet/res/schnapsennet.js
    https://darkstar.work/mono/SchnapsNet/res/schnapsennet.js
    2023-12-22 last change 
*/
var atouUrl = "";

function schnapsStateInit() {
    const myUrl2 = new URL(window.location.toLocaleString());
    // alert(myUrl2);

    var myUrl = new URL(myUrl2);
    var initSchnapsState = myUrl.searchParams.get("initState");
    console.log(initSchnapsState);

    schnapsStateSwitch(0);

    if (initSchnapsState != null) {

        if (initSchnapsState == 4) {
            initSchnapsState4();
        }
        if (initSchnapsState == 7) {
            initSchnapsState7();
        }
        if (initSchnapsState == 8) {
            initSchnapsState8();
        }
        if (initSchnapsState == 15) {
            schnapsStateRedirect();
        }
    }
}

function initSchnapsState4() {
    atouUrl = document.getElementById("imAtou10").src;
    // alert(atouUrl);
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
    atouUrl = document.getElementById("imAtou10").src;
    // alert(atouUrl);
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
    atouUrl = document.getElementById("imAtou10").src;
    // alert(atouUrl);
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
        if (document.getElementById("imMerge11") != null)
            document.getElementById("imMerge11").style.visibility = "visible";
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
    if (document.getElementById("imOut21") != null)
        document.getElementById("imOut21").style.visibility = "hidden";
    if (document.getElementById("imOut20") != null)
        document.getElementById("imOut20").style.visibility = "hidden";
    if (document.getElementById("imMerge11") != null)
        document.getElementById("imMerge11").style.visibility = "hidden";
    document.getElementById("im0").style.visibility = "hidden";
    document.getElementById("im1").style.visibility = "hidden";
    document.getElementById("im2").style.visibility = "hidden";
    document.getElementById("im3").style.visibility = "hidden";
    document.getElementById("im4").style.visibility = "hidden";
    document.getElementById("spanAtou").style.visibility = "hidden";
    document.getElementById("spanTalon").style.visibility = "hidden";
}

function playerCardsVisible() {
    document.getElementById("im0").style.visibility = "visible";
    document.getElementById("im1").style.visibility = "visible";
    document.getElementById("im2").style.visibility = "visible";
    document.getElementById("im3").style.visibility = "visible";
    document.getElementById("im4").style.visibility = "visible";
}

function playerCards1st3Visible() {
    document.getElementById("im0").style.visibility = "visible";
    document.getElementById("im1").style.visibility = "visible";
    document.getElementById("im2").style.visibility = "visible";
}

function atouCardVisible() {
    if (document.getElementById("spanAtou") != null)
        document.getElementById("spanAtou").style.visibility = "visible";
    if (document.getElementById("imAtou10") != null) {
        document.getElementById("imAtou10").style.visibility = "visible";
        document.getElementById("imAtou10").src = atouUrl;
    }
}

function talonCardVisible() {
    if (document.getElementById("spanTalon") != null)
        document.getElementById("spanTalon").style.visibility = "visible";
    if (document.getElementById("imTalon") != null)
        document.getElementById("imTalon").style.visibility = "visible";
}

function imOut20Empty() {
    document.getElementById("imOut20").src = "https://area23.at/mono/SchnapsNet/cardpics/e.gif";
}

function imOut21Empty() {
    if (document.getElementById("imOut21") != null)
        document.getElementById("imOut21").style.visibility = "visible";
    if (document.getElementById("imOut20") != null)
        document.getElementById("imOut20").style.visibility = "visible";
    document.getElementById("imOut21").src = "https://area23.at/mono/SchnapsNet/cardpics/e.gif";
    document.getElementById("imOut20").src = "https://area23.at/mono/SchnapsNet/cardpics/e.gif";
}

function imOut20a3() {
    if (document.getElementById("imOut21") != null) {
        document.getElementById("imOut21").style.visibility = "visible";
        document.getElementById("imOut21").src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
    }
    if (document.getElementById("imOut20") != null) {
        document.getElementById("imOut20").style.visibility = "visible";
        document.getElementById("imOut20").src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
    }
    if (document.getElementById("imAtou10") != null) {
        document.getElementById("imAtou10").src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
    }
    if (document.getElementById("spanAtou") != null)
        document.getElementById("spanAtou").style.visibility = "visible";
    if (document.getElementById("imAtou10") != null) {
        document.getElementById("imAtou10").style.visibility = "visible";
        document.getElementById("imAtou10").src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
    }
}

function imOut21a2() {
    if (document.getElementById("imOut21") != null)
        document.getElementById("imOut21").style.visibility = "visible";
    if (document.getElementById("imOut20") != null)
        document.getElementById("imOut20").style.visibility = "visible";
    document.getElementById("imOut20").src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
    document.getElementById("imOut21").src = "https://area23.at/mono/SchnapsNet/cardpics/n1.gif";
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
    unLightId.st
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

