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
    if ($.fn.dataTable.isDataTable('#ClosedJobTable')) {
        $('#ClosedJobTable').DataTable().destroy();
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

    var url = '/ClosedJobs/GetJobStatusList';

    var table = $('#ClosedJobTable').DataTable({
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
                $(nRow).find('td:eq(-2)').css('background-color', redDeadline);
                $(nRow).find('td:eq(-1)').css('background-color', redDeadline);
            }
            else if (aData[2] <= forDue2Days) {

                var yellowDeadline = "#fffedb";
                $(nRow).find('td:eq(-2)').css('background-color', yellowDeadline);
                $(nRow).find('td:eq(-1)').css('background-color', yellowDeadline);
            }
            else {

                $(nRow).find('td:eq(-2)').css('background-color', '#fff');
                $(nRow).find('td:eq(-1)').css('background-color', '#fff');
            }

        },
        "aoColumns": [
            {
                "width": "20%",
                "name": "JobID",
                //"title":"Job ID",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return '<a class="a-1" href="/jobs/jobsinitial/' + row[0] + '?flag='+row[5]+'" >' + row[0] + '</a>';
                    //return '<a class="a-1 contactEdit" href="javascript:;" data-id="' + row[0] + '" data-database="' + row[4] + '">' + row[0] + '</a>';
                    
                      //return row[0]
                }
            },
            {
                "width": "30%",
                "name": "EngagementNum",
                //"title": "Engagement Num#",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[1]
                }
            },
            
            {
                "width": "30%",
                "name": "Deadline",
                //"title": "DeadLine",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[2];
                }
            },
            {
                "width": "20%",
                "name": "JobType",
                //"title": "Job Type",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[3];
                }
            },
        ]
    });
}
