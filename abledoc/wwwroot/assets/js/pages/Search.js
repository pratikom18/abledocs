$(document).ready(function () {
    if ($('#txtSearch').val() != "") {
        search();
    }

    $('.btnSearch').click(function () {
        search();
           
    });
    
    $('#txtSearch').keypress(function (e) {
        if (e.keyCode == 13) {
            search();
        }

    });

    $(document).on("click", ".fileEdit", function () {

        var database = $(this).attr("data-dn");
        var id = $(this).attr("data-id");


        $.ajax({
            type: "POST",
            url: "/phases/SetContactDatabase",
            data: { databasename: database },
            dataType: "JSON",
            success: function (data) {

                window.location.href = "/file?ID=" + id;


            },
            error: function () {
                alert("error");
            }
        });


    });

    $(document).on("click", ".jobsEdit", function () {

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
    if ($("#SearchList").val() != "" || $("#hdnSearch").val() != "") {
        $('.searchlist-error').css('display', 'none');
        if ($('#txtSearch').val() != "") {
            $('.search-error').css('display', 'none');
            if ($.fn.dataTable.isDataTable('#SearchTable')) {
                $('#SearchTable').DataTable().destroy();
                ToDatatable();
            }
            else {
                ToDatatable();
            }
        } else {
            $('.search-error').css('display', 'block');
        }
    } else {
        $('.searchlist-error').css('display', 'block');
    }
    
}

function ToDatatable() {
    var url = '/search/searchList';

    var table = $('#ClientsTable').DataTable({
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
            aoData.push({ "name": "filter", "value": $('#SearchList').val() });
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
                
                "name": "ID",
                "width": "10%",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return '<a class="a-1" href="/file?ID=' + row[0] + '&flag='+row[9]+'">' + row[0] + '</a>';
                    //return '<a class="a-1 fileEdit" href="javascript:;" data-id="' + row[0] + '" data-dn="' + row[8] + '">' + row[0] + '</a>';
                }
            },
            {
                
                "name": "JobID",
                "width": "10%",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return '<a class="a-1" href="/jobs/jobsinitial/' + row[1] + '?flag='+row[9]+'">' + row[1] + '</a>';
                    //return '<a class="a-1 jobsEdit" href="javascript:;" data-id="' + row[1] + '" data-dn="' + row[8] + '">' + row[1] + '</a>';
                }
            },
            {
                
                "name": "Client",
                "width": "20%",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[2]
                }
            },
            {
                
                "name": "Contact",
                "width": "20%",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[3]
                }
            },
            {
                
                "name": "EngagementNum",
                "width": "20%",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[4]
                }
            },
            {
               
                "name": "EntityType",
                "width": "5%",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[5]
                }
            },
            {
               
                "name": "Status",
                "width": "5%",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[6]
                }
            },
            {
                
                "name": "FileName",
                "width": "10%",
                "orderable": true,
                render: function (data, type, row, meta) {
                  //  return row[7]
                    return '<span class="wordbreak">' + row[7] + '</span>';
                }
            }
        ]
    });
}

//function ToDatatable()
//{
//    var table = $('#SearchTable').DataTable({
//        "destroy": "true",
//        "retrieve": "true",
//        "lengthMenu": [
//            [10, 25, 50, 100, -1],
//            ['10', '25', '50', '100', 'Show All']
//        ],
//        "ajax": "/search/searchList?searchstr=" + $("#txtSearch").val(),
//        "columns": [
//            {
//                "bVisible": true,
//                "render": function (data, type, row, meta) {

//                    // return '<a href="/users/edit/' + row.id + '"  class="btn btn-info btn-sm"><i class="fa fa-edit"></i>Edit</a>';
//                    return '<a href="/file?ID=' + row.id + '">' + row.id + '</a>';
//                },
//            },
//            {
//                "bVisible": true,
//                "render": function (data, type, row, meta) {

//                    // return '<a href="/users/edit/' + row.id + '"  class="btn btn-info btn-sm"><i class="fa fa-edit"></i>Edit</a>';
//                    return '<a href="/job_initial?ID=' + row.jobID + '">' + row.jobID + '</a>';
//                },
//            },
//            { "data": "engagementNum" },
//            { "data": "entityType" },
//            { "data": "status" },
//            { "data": "fileName" }
//        ]
//    });
//}