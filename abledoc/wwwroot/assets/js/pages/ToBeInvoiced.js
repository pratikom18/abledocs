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
    $(document).on("click", ".jobEdit", function () {

        var database = $(this).attr("data-dn");
        var id = $(this).attr("data-id");


        $.ajax({
            type: "POST",
            url: "/phases/SetContactDatabase",
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
});

function search() {
    if ($.fn.dataTable.isDataTable('#ToBeInvoicedTable')) {
        $('#ToBeInvoicedTable').DataTable().destroy();
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

    var url = '/ToBeInvoiced/GetToBeInvoicedList';

    var table = $('#ToBeInvoicedTable').DataTable({
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
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            
            if (aData[2] <= todayDate) {
                var redDeadline = "#ffb8b8";
                $(nRow).find('td:eq(2)').css('background-color', redDeadline);
                $(nRow).find('td:eq(3)').css('background-color', redDeadline);
            }
            else if (aData[2] <= forDue2Days) {

                var yellowDeadline = "#fffedb";
                $(nRow).find('td:eq(2)').css('background-color', yellowDeadline);
                $(nRow).find('td:eq(3)').css('background-color', yellowDeadline);
            }
            else {

                $(nRow).find('td:eq(-2)').css('background-color', '#fff');
                $(nRow).find('td:eq(-1)').css('background-color', '#fff');
            }

            if (aData[8] > 180) {
                var yellowDeadline = "#ff9";
                $(nRow).find('td:eq(8)').css('background-color', yellowDeadline);
            }

        },
        "aoColumns": [
            {
                "name": "JobID",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return '<a class="a-1" href="/jobs/jobsinitial/' + row[0] + '?flag='+row[12]+'">' + row[0] + '</a>';
                    //return '<a href="javascript:;" class="JobID a-1 jobEdit" data-id="' + row[0] + '" data-dn="' + row[11] + '">' + row[0] + '</a>';
                    //return row[0]
                }
            },
            {
                "name": "EngagementNum",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[1]
                }
            },
            {
                "name": "Deadline",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[2];
                }
            },
            {
                "name": "JobType",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[3];
                }
            },
            {
                "width": "10%",
                "name": "Status",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[4]
                }
            },
            {
                "name": "fileC",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[5]
                }
            },
            {
                "name": "totalPages",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[6]
                }
            },
            {
                "name": "Notes",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[7]
                }
            },
            {
                "name": "Age",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[8];
                }
            },
            {
                "name": "JobQuotedAs",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[9];
                }
            },
            {
                "name": "hours",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[10];
                }
            },
        ]
    });
}

