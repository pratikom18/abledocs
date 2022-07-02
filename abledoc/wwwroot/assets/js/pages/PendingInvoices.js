$(document).ready(function () {
    ToDatatable();
});


function ToDatatable() {

    var url = '/PendingInvoices/GetPendingInvoicesList';

    var table = $('#PendingInvoicesTable').DataTable({
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
            aoData.push({ "name": "State", "value": "InvoiceDateRange" });
            aoData.push({ "name": "param1", "value": "" });

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
                "name": "EngagementNum",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[2]
                }
            },
            {
                "name": "LastUpdatedInv",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[3];
                }
            },
            {
                "name": "Pre Tax Amount",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[9] +row[4];
                }
            },
            {
                "name": "Tax",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[9] +row[5]
                }
            },
            {
                "name": "Full Amount",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return row[9]+row[6]
                }
            },


            {
                "bVisible": true,
                "orderable": false,
                "class": "disabled-sorting text-right",
                //"bVisible": true,
                //"bVisible": (approvebtn != "No" ? true : false),
                "render": function (data, type, row, meta) {
                   
                    if (row[6] == row[7] && row[8] == 0) {
                        return '<a href="javascript:;" id="ApproveInvoice" onClick="ApproveInvoice(event,' + row[1] + ',' + row[0] + ',' + row[11] + ')"  class="btn btn-success">' + $("#myapprove").val() + '</a>';
                    }
                    else{

                        return '<a href="javascript:;" id="VarianceOpen" onClick="VarianceOpen(' + row[1] + ',' + row[7] + ',' + row[6] + ',' + row[0] + ',' + row[11] + ')"  class="btn btn-primary-1"> ' + $("#myvariance").val() + '</button>';

                    }

                   


                }
            }


        ]
    });
}

function VarianceOpen(jobID, quoteAmount, invoiceAmount, invoiceID, flag ) {
    panelOpen = 1;
    var pdfH = window.innerHeight;
    var params = jobID + "|" + pdfH + "|" + quoteAmount + "|" + invoiceAmount + "|" + invoiceID + "|" + flag;
    var $modal = $('#popupSendFile');
    $modal.load('/PendingInvoices/VarianceState?Params=' + params, function () {
        $('#bootstrap-modal').modal({
            show: true
        });
    });
}

function SaveVarianceComment(jobID,flag) {
    var message = $("#varianceComment").val();
    var params = jobID + " | " + message;
    AjaxState("SaveVarianceComment", params, flag);
}

function VarianceSendBack(jobID, invoiceID,flag) {
    var params = jobID + " | " + invoiceID;
    AjaxState("VarianceSendBack", params,flag);
}

function AjaxState(state, params,flag) {
    $.ajax({
        url: '/pendinginvoices/AjaxState',
        data: { state: state, param: params,flag:flag },
        type: 'POST'
    }).done(function (responseData) {
        if (state == "VarianceSendBack") {
            window.location.href = "/invoices/pendinginvoices";
        }
    });
}

function ReIssueInvoice(invoiceID, jobID, invoiceIDQB,flag) {
    var params = invoiceID + "|" + jobID + "|" + invoiceIDQB;
   // AjaxInvoice("InvoiceEmailSend", localParams);
}

function ApproveVariance(event, jobID, invoiceID,flag) {
    UnifiedApproveInvoice(event, jobID, invoiceID,flag);
}

function ApproveInvoice(event, jobID, invoiceID,flag) {
    UnifiedApproveInvoice(event, jobID, invoiceID,flag);
}
function UnifiedApproveInvoice(event, jobID, invoiceID,flag) {
    $(event.target).addClass('ui-disabled');
    var localMessage = "Fetching data & posting to QuickBooks...";
    // MessagePackage("show", localMessage);
   // alert("Under Development", localMessage);
    var params = invoiceID + "|" + jobID;
    AjaxInvoice("InvoiceEmail", params,flag);
}

function AjaxInvoice(state, params,flag) {
    $.ajax({
        url: '/pendinginvoices/GenerateInvoicePdf',
        data: { State: state, Params: params, InsertFlag:1,flag:flag },
        type: 'GET'
    }).done(function (responseData) {
        console.log('Done: ', responseData);
        if (state == "InvoiceEmail") {
            
            var responseLocal = responseData.data;
            var responseLocalSplit = responseLocal.split("|");
            var invoiceIDQBLocal = responseLocalSplit[2];
            AjaxInvoice("InvoiceEmailSend", responseData.data);
            //if (invoiceIDQBLocal != 0 && invoiceIDQBLocal != "") {
            //    //var localMessage = "Preparing email and sending it to the client...";
            //    $.notify({
            //        icon: 'add_alert',
            //        title: '<strong>Success!</strong>',
            //        message: "Preparing email and sending it to the client..."
            //    }, {
            //        type: 'success'
            //    });

            //    AjaxInvoice("InvoiceEmailSend", responseData.data);
            //}
            //else {
            //    //var localMessage = "ALERT: QB ISSUE, please contact the team...";
            //    $.notify({
            //        icon: 'add_alert',
            //        title: '<strong>Success!</strong>',
            //        message: "ALERT: QB ISSUE, please contact the team..."
            //    }, {
            //        type: 'danger'
            //    });
            //    setTimeout(() => { window.location.href = "/invoices/pendinginvoices"; }, 3000);
               
            //    //MessagePackage("show", localMessage);
            //    //PanelCheck();
            //    //localMessage = "Refreshing pending invoices list...";
            //    //SlackProcess("InvoiceDateRange", localMessage);
            //}
        }
        else if (state == "InvoiceEmailSend") {
            //var localMessage = "Refreshing pending invoices list...";
            //MessagePackage("show", localMessage);
            //PanelCheck();
            //AjaxState("InvoiceDateRange", 0);
            window.location.href = "/invoices/pendinginvoices";
        }
    });
}
