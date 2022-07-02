$(document).ready(function () {
    ToDatatable();
});


function ToDatatable() {

    var url = '/PendingCreditNotes/GetPendingCreditNotesList';

    var table = $('#PendingCreditNotesTable').DataTable({
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
            aoData.push({ "name": "State", "value": "CreditNoteDateRange" });
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
                "name": "InvoiceID",
                "orderable": true,
                render: function (data, type, row, meta) {
                   // return row[2]
                   // return '<a href="/PendingCreditNotes/creditnote/' + row[2] + '">' + row[2] + '</a>';
                    return '<a class="a-1" href="javascript:;" id="CreditNote"  onclick="CreditNoteOpen(' + row[2] + ',' + row[1] + ')">' + row[2] + '</a>';
                }
            },
            {
                "name": "Date",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[4];
                }
            },
            {
                "name": "Amount",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return "$" +row[3];
                }
            },
    
            {
                "bVisible": true,
                "class": "text-right",
                "orderable": false,
                "render": function (data, type, row, meta) {
                  
                    return '<a href="javascript:;" id="ApproveCreditNote" onClick="ApproveCreditNote(' + row[2] + ')"  class="btn btn-success"> Approve</a>';
         
                }
            }

        ]
    });
}

//function VarianceOpen(jobID, quoteAmount, invoiceAmount, invoiceID, flag = 0) {
//    panelOpen = 1;
//    var pdfH = window.innerHeight;
//    var params = jobID + "|" + pdfH + "|" + quoteAmount + "|" + invoiceAmount + "|" + invoiceID + "|" + flag;
//    var $modal = $('#popupSendFile');
//    $modal.load('/PendingInvoices/VarianceState?Params=' + params, function () {
//        $('#bootstrap-modal').modal({
//            show: true
//        });
//    });
//}

//function SaveVarianceComment(jobID) {
//    var message = $("#varianceComment").val();
//    var params = jobID + " | " + message;
//    AjaxState("SaveVarianceComment", params);
//}

//function ReIssueInvoice(invoiceID, jobID, invoiceIDQB) {
//    var params = invoiceID + "|" + jobID + "|" + invoiceIDQB;
//    AjaxInvoice("InvoiceEmailSend", localParams);
//}

//function VarianceSendBack(jobID, invoiceID) {
//    var params = jobID + " | " + invoiceID;
//    AjaxState("VarianceSendBack", params);
//}

//function ApproveVariance(invoiceID) {
//    $(event.target).addClass('ui-disabled');
//    var localMessage = "Fetching data & posting to QuickBooks...";
//    MessagePackage("show", localMessage);
//    var params = invoiceID + "|" + jobID;
//    AjaxInvoice("InvoiceEmail", params);
//}
