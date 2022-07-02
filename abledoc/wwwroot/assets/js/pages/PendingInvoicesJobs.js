$(document).ready(function () {

    search();
    if ($('#txtSearch').val() != "") {

    }

    $('.btnSearch').click(function () {
        search();

    });

    $('#txtSearch').keypress(function (e) {
        if (e.keyCode == 13) {
            search();
        }

    });
});

function search() {
    if ($.fn.dataTable.isDataTable('#PendingInvoicesJobsTable')) {
        $('#PendingInvoicesJobsTable').DataTable().destroy();
        ToDatatable();
    }
    else {
        ToDatatable();
    }

}

function ToDatatable() {
    var url = '/PendingInvoicesJobs/GetPendingInvoicesList';

    var table = $('#PendingInvoicesJobsTable').DataTable({
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
           
            {
                "width": "100%",
                "name": "EngagementNum",
                //"title":"Engagement Num#",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return '<a href="/jobs/jobsinitial/' + row[1] + '?flag='+row[4]+'">' + row[2] + '</a>';
                    //return '<a class="contactEdit" href="javascript:;" data-id="' + row[1] + '" data-database="' + row[3] + '">' + row[2] + '</a>';
                }
            },          
        ]
    });
}

$(document).on("click", ".contactEdit", function () {

    var database = $(this).attr("data-database");
    var id = $(this).attr("data-id");



    $.ajax({
        type: "POST",
        url: "/clients/SetContactDatabase",
        data: { databasename: database },
        dataType: "JSON",
        success: function (data) {

            window.location.href = "/jobs/jobsinitial/" + id;


        },
        error: function () {
            alert("error");
        }
    });


});

