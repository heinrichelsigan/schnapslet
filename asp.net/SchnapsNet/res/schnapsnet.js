/*
    schnapsnet.js ©
    2023-12-13 by Heinrich Elsigan
    https://area23.at/mono/SchnapsNet/res/schnapsnet.js
    https://darkstar.work/mono/SchnapsNet/res/schnapsnet.js
    2023-12-22 last change 
*/
let helpWin = null;

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
            document.getElementById(highLightId).style.borderColor = "deeppink";
            return; 
        }

        // if (document.getElementById(highLightId).style.borderStyle == "dashed" ||
        //    document.getElementById(highLightId).style.borderWidth == 1) {
        document.getElementById(unLightId).style.borderWidth = 2;
        document.getElementById(unLightId).style.borderStyle = "groove";
        document.getElementById(highLightId).style.borderColor = "azure";
        // }
    }
}
