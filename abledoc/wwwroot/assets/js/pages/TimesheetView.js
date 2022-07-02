
$(document).ready(function () {

   // $('#TableTotalHours').dataTable();
    ToDatatable();
   

    $(document).on('click', '#TotalHours', function () {
        $('.TotalHours').show();
        $('.TotalWeeks').hide();
        $('#TotalHours').addClass('btn-success');
        $('#TotalHours').removeClass('btn-primary-1');
        $('#TotalWeeks').removeClass('btn-success');
        $('#TotalWeeks').addClass('btn-primary-1');
        ToDatatable();
    });

    $(document).on('click', '#TotalWeeks', function () {
        $('.TotalWeeks').show();
        $('.TotalHours').hide();
        $('#TotalWeeks').addClass('btn-success');
        $('#TotalWeeks').removeClass('btn-primary-1');
        $('#TotalHours').removeClass('btn-success');
        $('#TotalHours').addClass('btn-primary-1');
        //$('#TableTotalWeeks').dataTable();
        ToDatatable1();
    });

    $(document).on('click', '#btnReturn', function () {
        $('.TotalHours').show();
        $('.TotalWeeks').hide();
        $('.weeksperemployee').empty();
        $('.view1').show();
        ToDatatable();
    });

    $(document).on('click', '.VarianceOpen', function () {
        var ID = $(this).attr("data-id");
        var dateRange = $(this).attr("data-date");
        var userID = $(this).attr("data-userid");
        daysperemployee(ID, dateRange, userID)
    });
});

function totalweek(userid) {
    $('.view1').hide();
   // var res = name.replace(" ", "_");
    $('.weeksperemployee').load('/approvedtimesheet/totalweek?ID=' + userid , function () {
        //$('#TableTotalHours1').dataTable();
        ToDatatable2();
    });
}

function daysperemployee(ID, dateRange, userID) {

    dateRange = dateRange.replace(" to ", ",");
    dateRangeGlobal = dateRange;

    timesheetIDGlobal = ID;
    timesheetIDGlobal1 = ID;
    loggedInUser1 = userID;
    var loggedInUser = userID;
    var dateRange = dateRangeGlobal;
    var timesheetID = timesheetIDGlobal;
    var mode = "Supervision";

    //$('#approvedtimesheet').load('/approvedtimesheet/UserWorkedDataWeekly?loggedInUser=' + loggedInUser + '&dateRange=' + dateRange + '&timesheetID=' + timesheetID + '&mode=' + mode + '&flag=supervise&updatedSupervisionFlag=0', function () {
    //    $('#bootstrap-modal').modal({
    //        show: true
    //    });

    //    $('#datatables1').DataTable();
    //});

    $('.DaysPerEmployee').load('/approvedtimesheet/UserWorkedDataWeeklyNew1?loggedInUser=' + loggedInUser + '&dateRange=' + dateRange + '&timesheetID=' + timesheetID + '&mode=' + mode + '&flag=supervise&updatedSupervisionFlag=0', function () {
        $('#modal-daysperemaployee').modal({
            show: true
        });
    });
}

function ToDatatable() {
    var url = '/approvedtimesheet/getTotalHours';

    var table = $('#TableTotalHours').DataTable({
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
            aoData.push({ "name": "supervisorID", "value": 0 });
            aoData.push({ "name": "monthVal", "value": "" });
            aoData.push({ "name": "yearVal", "value": "" });
            aoData.push({ "name": "ApprovedFinal", "value": 0 });
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
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
           
        },
        "aoColumns": [
           
            {
                "name": "FirstName",
                "width": "15%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return "<a href='javascripts:;' onclick='totalweek(" + row[0] + ");'>" + row[1]+"</a>";
                }
            },
            {

                "name": "Totalhours",
                "width": "15%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return parseFloat(row[2]) + parseFloat(row[3]) + parseFloat(row[4]) + parseFloat(row[5]) + parseFloat(row[6]) + parseFloat(row[7]) + parseFloat(row[8])
                }
            },
            {

                "name": "TotalContracthours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return parseFloat(row[3])
                }
            },
            {

                "name": "TotalOThours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[2]
                }
            },
            {
                "name": "TotalPersonaltimehours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[4]
                }
            },
            {
                "name": "TotalStatholidayhours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[5]
                }
            },
            {
                "name": "TotalTraininghours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[6]
                }
            },
            {
                "name": "TotalMeetingshours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[7]
                }
            },
            {
                "name": "TotalMeetingshours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[8]
                }
            },

        ]
    });
}

function ToDatatable1() {
    var url = '/approvedtimesheet/getTotalWeeks';

    var table = $('#TableTotalWeeks').DataTable({
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
            aoData.push({ "name": "supervisorID", "value": 0 });
            aoData.push({ "name": "monthVal", "value": "" });
            aoData.push({ "name": "yearVal", "value": "" });
            aoData.push({ "name": "ApprovedFinal", "value": 0 });
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
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
           
        },
        "aoColumns": [
            {
                "name": "Weekintheyear",
                "width": "10%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[9]
                }
            },
            {
                "name": "FirstName",
                "width": "10%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[1]
                }
            },
            {

                "name": "Totalhours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return parseFloat(row[2]) + parseFloat(row[3]) + parseFloat(row[4]) + parseFloat(row[5]) + parseFloat(row[6]) + parseFloat(row[7]) + parseFloat(row[8])
                }
            },
            {

                "name": "TotalContracthours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return parseFloat(row[3])
                }
            },
            {

                "name": "TotalOThours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[2]
                }
            },
            {
                "name": "TotalPersonaltimehours",
                "width": "10%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[4]
                }
            },
            {
                "name": "TotalStatholidayhours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[5]
                }
            },
            {
                "name": "TotalTraininghours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[6]
                }
            },
            {
                "name": "TotalMeetingshours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[7]
                }
            },
            {
                "name": "TotalMeetingshours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[8]
                }
            },

        ]
    });
}

function ToDatatable2() {
    var url = '/approvedtimesheet/getTotalWeeksbyUser';

    var table = $('#TableTotalHours1').DataTable({
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
            aoData.push({ "name": "supervisorID", "value": 0 });
            aoData.push({ "name": "monthVal", "value": "" });
            aoData.push({ "name": "yearVal", "value": "" });
            aoData.push({ "name": "ApprovedFinal", "value": 0 });
            aoData.push({ "name": "userID", "value": $("#hdnUserID").val() });
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
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

        },
        "aoColumns": [
            {
                "name": "Weekintheyear",
                "width": "15%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return '<a href="javascript:;" id="VarianceOpen" data-id="' + row[10] + '" data-date="' + row[9] + '" data-userid="' + row[0] + '"  class="VarianceOpen a-1">' + row[9] + '</a>';              
                    //return "<a href='javascripts:;' onclick='daysperemployee();'>" + row[9]+"</a>"
                }
            },
            //{
            //    "name": "FirstName",
            //    "width": "10%",
            //    "orderable": false,
            //    render: function (data, type, row, meta) {
            //        return row[1]
            //    }
            //},
            {

                "name": "Totalhours",
                "width": "15%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return parseFloat(row[2]) + parseFloat(row[3]) + parseFloat(row[4]) + parseFloat(row[5]) + parseFloat(row[6]) + parseFloat(row[7]) + parseFloat(row[8])
                }
            },
            {

                "name": "TotalContracthours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return parseFloat(row[3])
                }
            },
            {

                "name": "TotalOThours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[2]
                }
            },
            {
                "name": "TotalPersonaltimehours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[4]
                }
            },
            {
                "name": "TotalStatholidayhours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[5]
                }
            },
            {
                "name": "TotalTraininghours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[6]
                }
            },
            {
                "name": "TotalMeetingshours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[7]
                }
            },
            {
                "name": "TotalMeetingshours",
                "width": "10%",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[8]
                }
            },

        ]
    });
}