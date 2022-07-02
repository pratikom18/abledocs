//$("#timesheetWeekRange").datepicker(
//    {
//        beforeShowDay: function (date) {
//            if (date.getDay() == 6) {
//                return [true];
//            } else {
//                return [false];
//            }
//        }
//    }
//);

var allIDGlobal = [];
var timesheetJson = [];
var timesheetIDGlobal = 0;
var timesheetIDGlobal1 = 0;
var dateRangeGlobal = "";
var loggedInUser1 = 0;




$(document).ready(function () {

  


    $('#queryTypeSelect').selectpicker('setStyle', 'btn btn-link');
    $('.filter-option').addClass('filter-option-1');
   
    $(".sendForProcessing").prop('disabled', true);
//    $("#timesheetWeekRange").datepicker({
//        format: "YYYY-MM-DD",
//        icons: {
//            time: "fa fa-clock-o",
//            date: "fa fa-calendar",
//            up: "fa fa-chevron-up",
//            down: "fa fa-chevron-down",
//            previous: "fa fa-chevron-left",
//            next: "fa fa-chevron-right",
//            today: "fa fa-screenshot",
//            clear: "fa fa-trash",
//            close: "fa fa-remove",
//        },
//         beforeShowDay: function (date) {
//            if (date.getDay() == 6) {
//                return [true];
//            } else {
//                return [false];
//            }
//        }
//    });
    $(document).on("change", "#timesheetWeekRange", function () {
        var loggedInUser = $("#userID").val();
        var dateRange = $("#timesheetWeekRange").val();
        var timesheetID = 0;
        var mode = "";

        $('#weekTimesheetTableDiv').load('/approvedtimesheet/UserWorkedDataWeekly?loggedInUser=' + loggedInUser + '&dateRange=' + dateRange + '&timesheetID=' + timesheetID + '&mode=' + mode + '&flag=timesheet&updatedSupervisionFlag=0', function () {
            $('#datatables1').DataTable();
        });
    });

    $("#btnSubmit").click(function () {
        var loggedInUser = $("#userID").val();
        var dateRange = $("#timesheetWeekRange").val();
        var timesheetID = 0;
        var mode = "";
        $("#weekTimesheetTableDiv2").show();
        $("#weekTimesheetTableDiv3").show();
        
        $('#weekTimesheetTableDiv').load('/approvedtimesheet/UserWorkedDataWeekly?loggedInUser=' + loggedInUser + '&dateRange=' + dateRange + '&timesheetID=' + timesheetID + '&mode=' + mode + '&flag=timesheet&updatedSupervisionFlag=0', function () {
            $('#datatables1').DataTable();
        });

        
    });

    $(".AddTimer").click(function () {

        $('#popupModal').load('/Timesheet/AddTime', function () {
            $('.datepicker').datepicker({
                multidate: true,
                format: "YYYY-MM-DD",
                icons: {
                    time: "fa fa-clock-o",
                    date: "fa fa-calendar",
                    up: "fa fa-chevron-up",
                    down: "fa fa-chevron-down",
                    previous: "fa fa-chevron-left",
                    next: "fa fa-chevron-right",
                    today: "fa fa-screenshot",
                    clear: "fa fa-trash",
                    close: "fa fa-remove",
                },
            });

            $('#bootstrap-AddTime').modal({
                show: true
            });
           
            $('#queryTypeSelect').selectpicker('setStyle', 'btn btn-link');
            $('.filter-option').addClass('filter-option-1');
        });

    });

    $(".sendForProcessing").click(function () {
        $('#popupModal').load('/Timesheet/sendForProcessing', function () {
            $('#bootstrap-sendForProcessing').modal({
                show: true
            });

        });

    });
   
});

function DownloadExcel() {

    var loggedInUser = $('#loggedInUser_id').val();
    var dateRange = $('#dateRange_id').val();
    var timesheetID = $('#timesheetID_id').val();
    var mode = $('#mode_id').val();
    window.location.href = '/approvedtimesheet/Downloadxl?loggedInUser=' + loggedInUser + '&dateRange=' + dateRange + '&timesheetID=' + timesheetID + '&mode=&flag=timesheet'


}

function AddTimeToTimesheet() {
    // Get the Type of Addition
    var typeAddition = $("#queryTypeSelect").val();

    // Get the number of hours entered and put them in the override time
    var hours = $("#addTimeNumber").val();

    var message = $("#messageAddTimer").val();

    var dates = $("#addTimerDateExtra").val();
    var currentTimesheetGlobal = $("#currentTimesheetGlobal").val();

    var params = typeAddition + " | " + hours + " | " + message + " | " + dates + " | " + currentTimesheetGlobal;
    
    $.ajax({
        url: '/Timesheet/AddTimeToTimesheet',
        data: { param: params },
        type: 'POST'
    }).done(function (responseData) {
        console.log('Done: ', responseData);
        updateHoure();

    }).fail(function () {
        console.log('Failed');
    });
    //alert(params);

}

function FormTimesheetForUpdating() {
    timesheetJson = [];
    
    for (var i = 0; i < allIDGlobal.length; i++) {
        var hour = $("#jfcFile_" + allIDGlobal[i]).val();
        var comment = $("#jfcFile_" + allIDGlobal[i] + "_Textarea").val();
        var commentSupervisor = $("#jfcFile_" + allIDGlobal[i] + "_TextareaSupervisor").val();
        var flag = $("#jfcFile_" + allIDGlobal[i] + "_databasename").val();
        timesheetJson.push({ 'ID': allIDGlobal[i], 'Hour': hour, 'Comment': comment, 'SupervisorComment': commentSupervisor, 'flag': flag });
    }
    var timesheetJsonEncode = JSON.stringify(timesheetJson);
    return timesheetJsonEncode;
}

function SaveTimesheetUpdate() {
    var timesheetID = $("#timesheetIDParam").val();
    $(".sendForProcessing").prop('disabled', false);

    currentTimesheetGlobal = $('#currentTimesheetGlobal').val();
    if (timesheetID != 0) {
        sentBackFlag = 1;
    }
    getAllID()

    var timesheetJsonEncode = FormTimesheetForUpdating();
    //alert(timesheetJsonEncode);
    TimesheetUpdate(timesheetJsonEncode);
    //updateHoure();
}

function updateHoure() {

    var loggedInUser = $("#userID").val();
    var dateRange = $("#timesheetWeekRange").val();
    var timesheetID = 0;
    var mode = "";

    $('#weekTimesheetTableDiv').load('/approvedtimesheet/UserWorkedDataWeekly?loggedInUser=' + loggedInUser + '&dateRange=' + dateRange + '&timesheetID=' + timesheetID + '&mode=' + mode + '&flag=timesheet&updatedSupervisionFlag=0', function () {
        $('#datatables1').DataTable();
    });

}

function getAllID() {
    
    $("#datatables1 tbody tr").find('.jfcFile').each(function () {
        var id = $(this).attr('data-id');
        allIDGlobal.push(id);

    });
}

function TimesheetUpdate($params) {

    $timesheetArray = JSON.parse($params);
    for ($i = 0; $i < $timesheetArray.length; $i++) {
        $rowID = $timesheetArray[$i]['ID'];
        $rowIDExplode = $rowID.split("-");
        $tableName = $rowIDExplode[0];
        $onlyRowID = $rowIDExplode[1];
        $onlyTableName = "";
        if ($tableName == "jobsfilescheckouts") {
            $onlyTableName = "timesheet_jobs";
        } else if ($tableName == "secondtimer") {
            $onlyTableName = "second_timer";
        }

        $.ajax({
            url: '/supervisor/UpdateSupervisor',
            data: { flag: $tableName, Comment: $timesheetArray[$i]['Comment'], OverrideTime: $timesheetArray[$i]['Hour'], SupervisorComment: $timesheetArray[$i]['SupervisorComment'], ID: $onlyRowID, flagdn: $timesheetArray[$i]['flag'] },
            type: 'POST'
        }).done(function (responseData) {
            console.log('Done: ', responseData);
            //$('.close').click();
            //search();

        }).fail(function () {
            console.log('Failed');
        });

    }

}

function SendTimesheetForProcessing() {
    var timesheetID = $("#timesheetIDParam").val();
    var actualTimeGlobal = 0;
    if (timesheetID == "") {
        //alert('asdasd');
        
        //getAllID();
        var timesheetJsonEncode = FormTimesheetForUpdating();
        var totalHour = $("#billableHours").val();
        //var billableDuration = $("#field_billableDuration").val();
        var billableDuration = $("#timesheetWeekRange1").val();
        var userID = $("#userID").val();
        //var originalHour = $("#field_originalHours").val();
        var supervisorID = $("#supervisorID").val();

       // var params = timesheetJsonEncode + " | " + totalHour + " | " + billableDuration + " | " + userID + " | " + supervisorID + " | " + actualTimeGlobal;

        var param = totalHour + " | " + billableDuration + " | " + userID + " | " + supervisorID + " | " + actualTimeGlobal;

        $.ajax({
            url: '/Timesheet/SendTimesheetForProcessing',
            data: { param: param },
            type: 'POST'
        }).done(function (responseData) {
            console.log('Done: ', responseData);
            
            UpdateTimeSheet(timesheetJsonEncode, responseData.message)
            window.location.href = "/timesheet";

        }).fail(function () {
            console.log('Failed');
        });
        
    }
    else {
        var totalHour = $("#billableHours").val();
        var param = timesheetID + " | " + totalHour;
        $.ajax({
            url: '/timesheet/SendTimesheetForProcessingAgain',
            data: { param: param },
            type: 'POST'
        }).done(function (responseData) {
            console.log('Done: ', responseData);


        }).fail(function () {
            console.log('Failed');
        });
    }
}

function UpdateTimeSheet($params, LastID) {
    //debugger
    $timesheetArray = JSON.parse($params);
    for ($i = 0; $i < $timesheetArray.length; $i++) {
        $rowID = $timesheetArray[$i]['ID'];
        $rowIDExplode = $rowID.split("-");
        $tableName = $rowIDExplode[0];
        $onlyRowID = $rowIDExplode[1];
        $onlyTableName = "";
        if ($tableName == "jobsfilescheckouts") {
            $onlyTableName = "timesheet_jobs";
        } else if ($tableName == "secondtimer") {
            $onlyTableName = "second_timer";
        }

        $.ajax({
            url: '/timesheet/UpdateTimeSheet',
            data: { flag: $tableName, ID: $onlyRowID, LastID: LastID, flagdn: $timesheetArray[$i]['flag']},
            type: 'POST'
        }).done(function (responseData) {
            console.log('Done: ', responseData);
           

        }).fail(function () {
            console.log('Failed');
        });

    }

}


