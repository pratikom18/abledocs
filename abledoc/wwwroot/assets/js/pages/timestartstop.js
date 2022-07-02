$(document).ready(function () {
    $("#stop").hide();

    window.setInterval(function () {

        var timer = $("#stopWatch").html();
        var file = $("#ID").val();
        var phase = $("#State").val();
        var params = file + "|" + timer + "|" + phase;

        if (timerStarted == 1) {

            var jobID = $("#JobID").val();
            var fileID = $("#ID").val();
            var fileState = $("#State").val();
            var fixFile = 0;

            var params = jobID + " | " + fileID + " | " + fileState + " | " + fixFile;
            AjaxStateTimer("TimerCompareState", params);

        }
    }, 30000);
});
var timerStarted = 0;

var seconds = 0, minutes = 0, hours = 0, t;

function TimerClockValues() {
    var timeValue = document.getElementById("stopWatch").innerHTML;
    var timeValueSplit = timeValue.split(":");
    hours = parseInt(timeValueSplit[0]);
    minutes = parseInt(timeValueSplit[1]);
    seconds = parseInt(timeValueSplit[2]);
}

TimerClockValues();

hours = +hours;
minutes = +minutes;
seconds = +seconds;

function add() {
    
    seconds++;
    if (seconds >= 60) {
        seconds = 0;
        minutes++;
        if (minutes >= 60) {
            minutes = 0;
            hours++;
        }
    }

    $("#stopWatch").text((hours ? (hours > 9 ? hours : "0" + hours) : "00") + ":" + (minutes ? (minutes > 9 ? minutes : "0" + minutes) : "00") + ":" + (seconds > 9 ? seconds : "0" + seconds));
    timer();
}
function timer() {
    t = setTimeout(add, 1000);
}
/* Start button */
function start() {

    var jobID = $("#JobID").val();
    var fileID = $("#ID").val();
    var fileState = $("#State").val();
    var fixFile = 0;
    var lastTimerTotal = $("#LastTimerTotal").val();
    var params = jobID + " | " + fileID + " | " + fileState + " | " + fixFile + " | " + lastTimerTotal;

    // Activate new timer function
    AjaxStateTimer("TimerStartedState", params);


}

/* Stop button */
function stop() {

    timerStarted = 0;
    clearTimeout(t);
    $("#stop").hide();
    $("#start").show();

    var jobID = $("#JobID").val();
    var fileID = $("#ID").val();
    var fileState = $("#State").val();
    var fixFile = 0;
    var params = jobID + " | " + fileID + " | " + fileState + " | " + fixFile;
    AjaxStateTimer("TimerCompareState", params);
}

function AjaxStateTimer(state, params) {
    var flag = $('#flag').val();
    $.ajax({
        url: '/phases/StateTimer',
        data: { State: state, Params1: params,flag:flag },
        type: 'POST'
    }).done(function (responseData) {
        if (state == "TimerStartedState") {
            timerStarted = 1;
            $("#start").hide();
            $("#stop").show();
            timer();
        }
        else if (state == "TimerCompareState") {
            
            document.getElementById("stopWatch").innerHTML = responseData.message;
            TimerClockValues();
        }
    }).fail(function () {
        console.log('Failed');
    });
}
