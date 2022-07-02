var stopWatch2,
    seconds2 = 0,
    minutes2 = 0,
    hours2 = 0,
    t2;

$(document).ready(function () {
    $("#stop2").hide();
    
    stopWatch2 = $('#stopWatch2').val();

});

function TimerClockValues1() {
    
    var timeValue2 = $('#stopWatch2').val();
    if (timeValue2 == "") {
        hours2 = "0";
        minutes2 = "0";
        seconds2 = "0";
    } else {
        var timeValueSplit2 = timeValue2.split(":");
        hours2 = timeValueSplit2[0];
        minutes2 = timeValueSplit2[1];
        seconds2 = timeValueSplit2[2];
    }
   
}

TimerClockValues1();

hours2 = +hours2;
minutes2 = +minutes2;
seconds2 = +seconds2;

function add2() {
    seconds2++;
    if (seconds2 >= 60) {
        seconds2 = 0;
        minutes2++;
        if (minutes2 >= 60) {
            minutes2 = 0;
            hours2++;
        }
    }
    
    $('#stopWatch2').text((hours2 ? (hours2 > 9 ? hours2 : "0" + hours2) : "00") + ":" + (minutes2 ? (minutes2 > 9 ? minutes2 : "0" + minutes2) : "00") + ":" + (seconds2 > 9 ? seconds2 : "0" + seconds2))
    timer2();
}

function timer2() {
    t2 = setTimeout(add2, 1000);
}

/* Start button */
function Start2() {

    timer2();
    $("#stop2").show();
    $("#start2").hide();
}

/* Stop button */
function Stop2() {

    timerStarted2 = 0;
    clearTimeout(t2);
    $("#stop2").hide();
    $("#start2").show();
}

function DoneSecondTimer() {
    var timerValue = $("#stopWatch2").html();
    var message = $("#messageSecondTimer").val();
    var phaseState = "";
    var queryType = $("#queryTypeSelect").val();
    var jobID = $("#JobID_Timer").val();
    var fileID = $("#FileID_Timer").val();
    var params = timerValue + " | " + message + " | " + queryType + " | " + jobID + " | " + fileID;
    var flag = $('#flag').val();
    $.ajax({
        url: '/qcfiles/SecondTimer',
        data: { Params: params,flag:flag },
        type: 'POST'
    }).done(function (responseData) {
        $('.model-close').click();
    }).fail(function () {
        console.log('Failed');
    });
}

function CancelSecondTimer() {
    $('.model-close').click();
    Stop2();
    $('#stopWatch2').text("00" + ":" + "00" + ":" +"00")
}


