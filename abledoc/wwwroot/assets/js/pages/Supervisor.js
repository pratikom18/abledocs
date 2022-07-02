var allIDGlobal = [];
var timesheetJson = [];
var timesheetIDGlobal = 0;
var timesheetIDGlobal1 = 0;
var dateRangeGlobal = "";
var loggedInUser1 = 0;
$(document).ready(function () {

    search();

    $(document).on('click', '.VarianceOpen', function () {
        var ID = $(this).attr("data-id");
        var dateRange = $(this).attr("data-date");
        var userID = $(this).attr("data-userid");
        VarianceOpen(ID, dateRange, userID)
    });

});

function search() {
    if ($.fn.dataTable.isDataTable('#SupervisorTable')) {
        $('#SupervisorTable').DataTable().destroy();
        ToDatatable();
    }
    else {
        ToDatatable();
    }

}

function ToDatatable() {
    var url = '/Supervisor/SupervisorList';

    var table = $('#SupervisorTable').DataTable({
        //"sDom": 'rtlfip',  // for set pagination dd and search button to Top
        "processing": true,
        "bServerSide": true,
        "bSort": true,
        "sAjaxSource": url,
        "aLengthMenu": [
            [10, 25, 50, 100, -1],
            ['10 rows', '25 rows', '50 rows', '100 rows', 'Show All']
        ],
        "iDisplayLength": 10,
        "bLengthChange": true,
        "bDestroy": true,
        "sEmptyTable": "Loading data from server",
        "searching": true,
        "paging": true,
        "scrollX": true,
        "order": [],
        "autoWidth": false,
        "responsive": true,
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "searchstr", "value": $('#txtSearch').val() });
            $.ajax({
                "dataType": 'json',
                "type": "POST",
                "url": sSource,
                "data": aoData,
                "success": fnCallback
            });
        },
        "fnCreatedRow": function (nRow, aData, iDataIndex) {
            $(nRow).attr('id', aData[0]) // or whatever you choose to set as the id
        },

        "aoColumns": [
            //{
            //    "width": "5%",
            //    "title": "No",
            //    "data": "id", "sortable": false,
            //    render: function (data, type, row, meta) {
            //        return meta.row + meta.settings._iDisplayStart + 1;
            //    }
            //},
            {
                "width": "15%",
                "name": "TimesheetID",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return '<a href="javascript:;" id="VarianceOpen" data-id="' + row[4] + '" data-date="' + row[2] + '" data-userid="' + row[5] + '"  class="VarianceOpen a-1">' + row[0]+'</a>';              
                  
                }
            },
            {
                "width": "20%",
                "name": "FullName",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[1];
                }
            },

            {
                "width": "20%",
                "name": "BillableDuration",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[2]
                }
            },
            {
                "width": "20%",
                "name": "TotalHourRounded",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return parseFloat(row[3]).toFixed(2) + " " + "Hrs" 
                }
            },
            {
                "width": "20%",
                "title": "Actions",
                "bVisible": true,
                "class": "text-right",
                "orderable": false,
                //"bVisible": (editval1 != "No" ? true : false),
                "render": function (data, type, row, meta) {
                    if (row[6] == row[7]) {
                        return '<a href="javascript:;" onclick="ApproveTimesheet('+row[4]+'); return false;"  class="btn btn-success">Approve</a>';              
                    } else {
                        return '<a href="javascript:;" id="VarianceOpen" data-id="' + row[4] + '" data-date="' + row[2] + '" data-userid="' + row[5] + '"  class="btn btn-primary-1 VarianceOpen">Review</a>';              
                    }
                    
                    //return '<a href=""  class="btn  btn-sm"><i class="fa fa-edit"></i> Variance</a>';                   
                },
            }

        ]
    });
}

function VarianceOpen(ID, dateRange, userID) {

    dateRange = dateRange.replace(" to ", ",");
    dateRangeGlobal = dateRange;

    timesheetIDGlobal = ID;
    timesheetIDGlobal1 = ID;
    loggedInUser1 = userID;
    var loggedInUser = userID;
    var dateRange = dateRangeGlobal;
    var timesheetID = timesheetIDGlobal;
    var mode = "Supervision";

    $('#approvedtimesheet').load('/approvedtimesheet/UserWorkedDataWeekly?loggedInUser=' + loggedInUser + '&dateRange=' + dateRange + '&timesheetID=' + timesheetID + '&mode=' + mode +'&flag=supervise&updatedSupervisionFlag=0', function () {
        $('#bootstrap-modal').modal({
            show: true
        });

        $('#datatables1').DataTable();
    });

}

function FormTimesheetForUpdating() {
    timesheetJson = [];
    for (var i = 0; i < allIDGlobal.length; i++) {
        var hour = $("#jfcFile_" + allIDGlobal[i]).val();
        var comment = $("#jfcFile_" + allIDGlobal[i] + "_Textarea").val();
        var commentSupervisor = $("#jfcFile_" + allIDGlobal[i] + "_TextareaSupervisor").val();
        var flag = $("#jfcFile_" + allIDGlobal[i] + "_databasename").val();
        timesheetJson.push({ 'ID': allIDGlobal[i], 'Hour': hour, 'Comment': comment, 'SupervisorComment': commentSupervisor,'flag':flag });
    }
    var timesheetJsonEncode = JSON.stringify(timesheetJson);
    return timesheetJsonEncode;
}

function SaveTimesheetUpdate(timesheetID = 0) {
    currentTimesheetGlobal = timesheetID;
    if (timesheetID != 0) {
        sentBackFlag = 1;
    }
    getAllID()

    var timesheetJsonEncode = FormTimesheetForUpdating();
    //alert(timesheetJsonEncode);
    TimesheetUpdate(timesheetJsonEncode);
    updateHoure();
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
        } else if($tableName == "secondtimer") {
            $onlyTableName = "second_timer";
        }

        $.ajax({
            url: '/supervisor/UpdateSupervisor',
            data: { flag: $tableName, Comment: $timesheetArray[$i]['Comment'], OverrideTime: $timesheetArray[$i]['Hour'], SupervisorComment: $timesheetArray[$i]['SupervisorComment'], ID: $onlyRowID,flagdn: $timesheetArray[$i]['flag'] },
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

function SendBackTimesheet(ID = "") {
    var params = "";
    if (ID == "") {
        params = timesheetIDGlobal;
    }
    else {
        params = ID;
    }

    $.ajax({
        url: '/supervisor/SendBackTimesheet',
        data: { timesheetID: params},
        type: 'POST'
    }).done(function (responseData) {
        console.log('Done: ', responseData);
        $('.close').click();
        search();

    }).fail(function () {
        console.log('Failed');
    });
}

function ApproveTimesheet(ID = "") {
    var params = "";
    if (ID == "") {
        params = timesheetIDGlobal;
    }
    else {
        params = ID;
    }
    $.ajax({
        url: '/supervisor/ApproveTimesheet',
        data: { timesheetID: params },
        type: 'POST'
    }).done(function (responseData) {
        console.log('Done: ', responseData);
        $('.close').click();
        search();

    }).fail(function () {
        console.log('Failed');
    });
}

function updateHoure() {

    var mode = "Supervision";

    $('.updateApprovedTimesheet').load('/approvedtimesheet/UserWorkedDataWeekly?loggedInUser=' + loggedInUser1 + '&dateRange=' + dateRangeGlobal + '&timesheetID=' + timesheetIDGlobal1 + '&mode=' + mode + '&flag=supervise&updatedSupervisionFlag=1', function () {
        $('#datatables1').DataTable();
    });

}