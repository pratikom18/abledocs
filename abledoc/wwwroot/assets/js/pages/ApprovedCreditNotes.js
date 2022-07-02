$(document).ready(function () {

    search();

    if ($('#txtSearch').val() != "") {

    }

    $('.btnSearch').click(function () {
        search();
    });

    $('#setdate').click(function () {
        ToDatatable();
    });

    $('#txtSearch').keypress(function (e) {
        if (e.keyCode == 13) {
            search();
        }

    });

});

function search() {
    if ($.fn.dataTable.isDataTable('#ApprovedCreditNotesTable')) {
        $('#ApprovedCreditNotesTable').DataTable().destroy();
        ToDatatable();
    }
    else {
        ToDatatable();
    }

}

function ToDatatable() {

    var sesion_id = $('#sessionid').val();
    var todayDate = $("#currentDate").val();
    var forDue2Days = $("#forDue2Days").val();

    var url = '/ApprovedCreditNotes/GetApprovedCreditNotesList';

    var table = $('#ApprovedCreditNotesTable').DataTable({
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
        "columnDefs": [
            { width: '20%', targets: 0 }
        ],
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "searchstr", "value": $('#txtSearch').val() });
            aoData.push({ "name": "state", "value": "CreditNoteDateApprovedRange" });
            aoData.push({ "name": "startdate1", "value": $('#startdate').val() });
            aoData.push({ "name": "enddate1", "value": $('#enddate').val() });
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
        //"fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
       


        //},
        "aoColumns": [
            {
                "name": "InvoiceID",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[0]
                   // return '<a href="javascript:;" id="CreditNote"  onclick="CreditNoteOpen(' + row[2] + ',' + row[1] + ')">' + row[2] + '</a>';
                }
            },
            {
                "name": "JobID",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[6]
                }
            },
            {
                "name": "Date",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[6];
                }
            },
            {
                "name": "Client",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[4];
                }
            },
            {
                "name": "Pre Tax Amount",
                "orderable": true,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return "$" + parseFloat(row[7]).toFixed(2);
                }
            },
            {
                "name": "Tax",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return "$" + row[2]
                }
            },
            {
                "name": "Full Amount",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return "$" + parseFloat(row[3]).toFixed(2);

                }
            }
           
        ]
    });
}

