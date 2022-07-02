var invJobIDGlobal = "";
var userIDGlobal = 0;
var dateRangeGlobal = 0;
var timesheetIDGlobal = 0;
var monthGlobalFlag = 0;
var yearGlobalFlag = 0;
var monthGlobalVal = 0;
var yearGlobalVal = 0;
var ApprovedFinal = 0

$(document).ready(function () {

    userIDGlobal = $("#loggedInUser").val();

    search();

    $(document).on('click', '.VarianceOpen', function () {
        var ID= $(this).attr("data-id");
        var dateRange= $(this).attr("data-date");
        var userID= $(this).attr("data-userid");
        VarianceOpen(ID, dateRange, userID)
    });

});


function ToPendingButton() {
    var allChecked = "";
    $('input:checkbox.timesheetCheckBox').each(function () {
        if (this.checked) {
            if (allChecked == "") {
                allChecked = $(this).val();
            } else {
                allChecked = allChecked + "," + $(this).val();
            }

        }
    });

    if (allChecked != "") {
        updateApprovedFinal("ToPending", allChecked);
    }
}

function ToResolvedButton() {
    var allChecked = "";
    $('input:checkbox.timesheetCheckBox').each(function () {
        if (this.checked) {
            if (allChecked == "") {
                allChecked = $(this).val();
            } else {
                allChecked = allChecked + "," + $(this).val();
            }
        }
    });

    if (allChecked != "") {
        updateApprovedFinal("ToResolved", allChecked);
    }
}

function updateApprovedFinal(state, params) {
    $.ajax({
        url: '/approvedtimesheet/updateapprovedFinal',
        data: { State: state, Params: params },
        type: 'POST'
    }).done(function (responseData) {
        console.log('Done: ', responseData);
        search();

    }).fail(function () {
        console.log('Failed');
    });
}
function SelectMonth() {
    monthGlobalVal = $("#monthSelect option:selected").val();
    if (monthGlobalVal == "") {
        monthGlobalFlag = 0;
    }
    else {
        monthGlobalFlag = 1;
    }
    //FilterShowHide();
}

function SelectYear() {
    yearGlobalVal = $("#yearSelect option:selected").val();
    if (yearGlobalVal == "") {
        yearGlobalFlag = 0;
    }
    else {
        yearGlobalFlag = 1;
    }
    //FilterShowHide();
}


function FilterButton(id) {
    ApprovedFinal = id;
    if (id == 0) {
        $('#Pending').addClass('btn-success');
        $('#Pending').removeClass('btn-primary-1');
        $('#Resolved').removeClass('btn-success');
        $('#Resolved').addClass('btn-primary-1');
    }
    else if (id == 1) {
        $('#Resolved').addClass('btn-success')
        $('#Resolved').removeClass('btn-primary-1')
        $('#Pending').removeClass('btn-success');
        $('#Pending').addClass('btn-primary-1');
    }

    search();
}

function search() {
    $('#ApprovedTimesheet').DataTable().destroy();
    ToDatatable();
}

function ToDatatable() {
    var url = '/approvedtimesheet/getApprovedTimesheet';

    var table = $('#ApprovedTimesheet').DataTable({
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
            aoData.push({ "name": "supervisorID", "value": userIDGlobal });
            aoData.push({ "name": "monthVal", "value": monthGlobalVal });
            aoData.push({ "name": "yearVal", "value": yearGlobalVal });
            aoData.push({ "name": "ApprovedFinal", "value": ApprovedFinal });
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
            $('.showassign').hide();
            $('td:eq(-10)', nRow).hide();
            // if ($('#loggedInUser').val() == 8) {
            if ($("#hdnShow").val() == "true") {
                $('.showassign').show();
                $('td:eq(-10)', nRow).show();
            }
        },
        "aoColumns": [
            {

                "name": "assignfiles",
                "width": "5%",
                "bVisible": true,
                "orderable": false,
                "render": function (data, type, row, meta) {

                    return '<input type="checkbox" name="assignfiles[]" class="timesheetCheckBox" value=' + row[0] + ' form="multiassign"> <span></span>';
                },
            },
            {

                "name": "TimesheetID",
                "width": "10%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return '<a href="javascript:;" class="VarianceOpen a-1" data-id="' + row[0] + '" data-date="' + row[2] + '" data-userid="' + row[1] + '">' + row[3] + '</a>';
                }
            },
            {

                "name": "FirstName",
                "title": "User",
                "width": "15%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[4];
                }
            },
            {

                "name": "BillableDuration",
                "width": "10%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[2]
                }
            },
            {

                "name": "Hours",
                "width": "10%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[5]
                }
            },
            {

                "name": "OTHours",
                "width": "10%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[6]
                }
            },
            {
                "name": "VacationHours",
                "width": "10%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[7]
                }
            },
            {
                "name": "PersonalHours",
                "width": "10%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[8]
                }
            },
            {
                "name": "StatutoryHours",
                "width": "10%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[9]
                }
            },
            {
                "name": "TotalHours",
                "width": "10%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[10]
                }
            },

        ]
    });
}

function VarianceOpen(ID, dateRange, userID) {
    
    dateRange = dateRange.replace(" to ", ",");
    dateRangeGlobal = dateRange;
   
    timesheetIDGlobal = ID;
    var loggedInUser = userID;
    var dateRange = dateRangeGlobal;
    var timesheetID = timesheetIDGlobal;
    var mode = "Supervision";

    $('#approvedtimesheet').load('/approvedtimesheet/UserWorkedDataWeekly?loggedInUser=' + loggedInUser + '&dateRange=' + dateRange + '&timesheetID=' + timesheetID + '&mode=' + mode + '&flag=approvedtimesheet&updatedSupervisionFlag=0', function () {
        $('#bootstrap-modal').modal({
            show: true
        });
        $('#datatables1').DataTable();

    });
   
    
}

function AjaxState(state, params) {
    $.ajax({
        url: '/api/ajax_invoice_dashboard.php',
        data: { State: state, Params: params },
        type: 'POST'
    }).done(function (responseData) {
        console.log('Done: ', responseData);
        if (state == "VarianceState") {
            $("#variancePanel").html("");
            $("#variancePanel").append(responseData);
            $("#variancePanel").trigger('create');
            $("#variancePanel").trigger("update");
        }
        if (state == "VarianceApproveState") {
            //var dataInvJob = responseData.split("|");
            //location.reload();
            invJobIDGlobal = responseData;
            $("#variancePanelClose").trigger("click");
            var dateRange = $('#invoiceDateRange').val();
            var params = dateRange;
            AjaxState("InvoiceDateRange", params);
            AjaxState("InvoiceDateApprovedRange", 0);
            AjaxInvoice("InvoiceEmail", responseData);
        }
        if (state == "TimesheetDateRangeApproved") {
            $("#timesheetListDiv").html("");
            $("#timesheetListDiv").append(responseData);
            $("#timesheetListDiv").trigger('create');
            $("#timesheetListDiv").trigger("update");
            $(".timesheetRowClass > div").css("width", "100%");
        }
        if (state == "InvoiceDateApprovedRange") {
            $("#invoiceApprovedListDiv").html("");
            $("#invoiceApprovedListDiv").append(responseData);
            $("#invoiceApprovedListDiv").trigger('create');
            $("#invoiceApprovedListDiv").trigger("update");
            $(".invoiceRowClass > div").css("width", "100%");
        }
        if (state == "InvoiceDate") {
            $("#timesheetListDiv").html("");
            $("#timesheetListDiv").append(responseData);
            $("#timesheetListDiv").trigger('create');
            $("#timesheetListDiv").trigger("update");
            $(".invoiceRowClass > div").css("width", "100%");
        }
        if (state == "InvoiceDateApproved") {
            $("#invoiceApprovedListDiv").html("");
            $("#invoiceApprovedListDiv").append(responseData);
            $("#invoiceApprovedListDiv").trigger('create');
            $("#invoiceApprovedListDiv").trigger("update");
            $(".invoiceRowClass > div").css("width", "100%");
        }

    }).fail(function () {
        console.log('Failed');
    });
}