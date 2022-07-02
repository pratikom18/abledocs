//import { for } from "core-js/fn/symbol";
var editval1 = $('#myedit').val();

$('#gotoJob').keypress(function (e) {
    if (e.which == 13) {
        job = $('#gotoJob').val();
        console.log('Go to Job ' + job);
        window.open("/jobs/jobsinitial/" + job);
        return false;
    }
});

$('#AddNewJobFromClientCode').keypress(function (e) {
    if (e.which == 13) {
        client = $('#AddNewJobFromClientCode').val();

        // console.log('Start job for client ' + client);
        window.open("/jobs/AddJobforClient?ClientCode=" + client);
        return false;
    }
});

function ToDataTable() {
    var editval1 = $('#myedit').val();
    var url = '/jobs/JobList';

    var table = $('#JobsTable').DataTable({
      
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
        //"scrollY": height,
        "scrollX": true,
        "order": [],
        "autoWidth": false,
        "responsive": true,
        //"columnDefs": [{
        //    "targets": 0,
        //    "orderable": false
        //}],
        //"aaSorting": [],// for set deafult 1st colunm sorting option to false

        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "Status", "value": $("#status").val() });
            aoData.push({ "name": "TodayTask", "value": $("#today_task").val() });
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
                "title": $("#hdnNo").val(),
                "width": "5%", "data": "no", "sortable": false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                "width": "10%",
                "name": "JobID",
                "title": $("#hdnJobID").val(),
                "orderable": true,
                "render": function (data, type, row, meta) {
                    if (row[14] != "") {
                        Code = row[14] + "-" + row[15] + "-" + row[0];
                    } else {
                        Code = row[0];
                    }

                    //return '<a href=/jobs/jobsinitial/' + row[1] + '>' + Code + "-" + row[1] + '</a>';
                    return Code + "-" + row[1];
                },
            },

            {
                "title": $("#hdnDeadLine").val(),
                "width": "5%", "name": "Deadline", "orderable": true,
                render: function (data, type, row, meta) {
                    return row[2];
                }
            },
            {
                "title": $("#hdnType").val(),
                "width": "5%",
                "name": "JobType",
                "orderable": true,
                "class":"text-center",
                render: function (data, type, row, meta) {
                    var style = "";
                    if (row[3] == "RUSH") {
                        style = "background-color:#FF9800;";
                    }
                    else if (row[3] == "FIXED") {
                        style += "background-color:#1986E6;color:#fff";
                    }
                    else {
                        style += "background-color:#fff";
                    }
                    return '<div style="' + style + '">' + row[3] + '</div>';
                }
            },
            {
                "title": $("#hdnFiles").val(),
                "width": "5%",
                "name": "Files",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[4];
                }
            },
            {
                "title": $("#hdnPages").val(),
                "width": "5%",
                "name": "Pages",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[5];
                }
            },
            {
                "title": $("#hdnCurrency").val(),
                "width": "5%",
                "name": "Currency",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[6];
                }
            },
            {
                "title": $("#hdnQuoted").val(),
                "width": "10%",
                "name": "QuotedValue",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[7];
                }
            },
            {
                "title": $("#hdnQuotedHours").val(),
                "width": "5%",
                "name": "QuotedHours",
                "orderable": true,
                "visible": $("#status").val() == "OPEN" || $("#status").val() == "TOBEDELIVERED" || $("#status").val() == "DELIVERED",
                render: function (data, type, row, meta) {
                    return row[8];
                }
            },
            {
                "title": $("#hdnHoursTaken").val(),
                "width": "5%",
                "name": "hours",
                "orderable": true,
                "visible": $("#status").val() == "OPEN" || $("#status").val() == "TOBEDELIVERED" || $("#status").val() == "DELIVERED",
                render: function (data, type, row, meta) {
                    return row[9];
                }
            },
            {
                "title": $("#hdnProgress").val(),
                "width": "40%",
                "padding": "2px",
                "name": "FinalPercent",
                "orderable": true,
                "visible": $("#status").val() == "OPEN",
                "render": function (data, type, row, meta) {

                    return '<div class="progress position-relative" style="height: 45px;font-size:20px"><div class="progress-bar bg-success" role = "progressbar" style = "width: ' + row[12] + '%" aria - valuenow="60" aria - valuemin="0" aria - valuemax="100" ></div><small class="justify-content-center d-flex position-absolute w-100">' + row[10] + '</small></div>';//'<div class="" style="width: 98%; background-color: #eee; border: 1px solid #ccc; text-align: left;"><div style="display: inline-block; position: relative; left: 0px; width:' + row[12] +'%;  background-color: #a1c283;">'+row[10]+'</div></div>';
                },
            },
            {
                "title": $("#hdnNotes").val(),
                "width": "15%",
                "name": "Notes",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[16];
                }
            },
            {
                "title": $("#hdnActions").val(),
                // "bVisible": true,
                "width": "5%",
                "class": "text-right",
                "orderable": false,
                "bVisible": (editval1 != "No" ? true : false),
                "render": function (data, type, row, meta) {

                    if ($("#status").val() == "TOBEDELIVERED") {
                        btnName = '<i class="fa fa-angle-double-right"></i> ' + $("#hdnSendtoclient").val() + '';
                    } else {
                        btnName = '<i class="fa fa-edit"></i> ' + $("#hdnEdit").val()+'';
                    }

                    if (editval1 != "No") {
                        
                        return '<a href="/jobs/jobsinitial/' + row[1] + '?flag=' + row[17] +'"  class="btn  btn-sm"> ' + btnName +' </a>';
                        //return '<button type="button" data-id="' + row[1] + '" data-database="' + row[17] + '" class="btn  btn-sm jobEdit"><i class="fa fa-edit"></i> Edit</button>';
                    }
                    else {
                        // table.columns([6]).visible(false);
                        return '';
                    }

                }
            },

        ],
    });
}

$(document).ready(function () {

    var todayDate = $("#currentDate").val();
    var forDue2Days = $("#forDue2Days").val();

    ToDataTable();

    $('.statusBtn').click(function (e) {
        var val = $(this).attr('rel');

        $(".statusBtn").addClass("btn-primary-1").removeClass("btn-success");
        $(this).removeClass("btn-primary-1").addClass("btn-success");

       
        $("#status").val(val);
        reloadTable();

        var data = 'currenttab=' + val;
        loc = $('<a>', { href: window.location })[0];
        $.post('/Jobs/', data);
        if (history.pushState) {
            if (data != "") {
                history.pushState(null, null, loc.pathname + '?' + data);
            } else {
                history.pushState(null, null, loc.pathname);
            }
        }

    });
    $('#today_task').change(function (e) {
        if (this.checked) {
            $(this).val(1);
        } else {
            $(this).val(0);
        }
        reloadTable();
    });

    function reloadTable() {
       
        $('#JobsTable').DataTable().destroy();
        ToDataTable();
    }
    reloadTable();
    setInterval(reloadTable, 10000000);
});


$(document).on("click", ".jobEdit", function () {

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