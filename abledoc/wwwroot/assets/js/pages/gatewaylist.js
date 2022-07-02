
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


    $(document).on("click", ".clientEdit", function () {

        var database = $(this).attr("data-dn");
        var id = $(this).attr("data-id");


        $.ajax({
            type: "POST",
            url: "/phases/SetContactDatabase",
            data: { databasename: database },
            dataType: "JSON",
            success: function (data) {

                window.location.href = "/clients/edit/" + id;


            },
            error: function () {
                alert("error");
            }
        });


    });

    $(document).on("click", ".gatewayEdit", function () {

        var database = $(this).attr("data-dn");
        var id = $(this).attr("data-id");


        $.ajax({
            type: "POST",
            url: "/Gateways/SetContactDatabase",
            data: { databasename: database },
            dataType: "JSON",
            success: function (data) {

                window.location.href = "/gateways/edit?subdomain=" + id;


            },
            error: function () {
                alert("error");
            }
        });


    });
});

function search() {
    if ($.fn.dataTable.isDataTable('#GatewayTable')) {
        $('#GatewayTable').DataTable().destroy();
        ToDatatable();
    }
    else {
        ToDatatable();
    }

}

function ToDatatable() {
    var editval1 = $('#hdnEdit').val();
    var url = '/Gateways/GatewayList';

    var table = $('#GatewayTable').DataTable({
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
                "width": "5%",
                "data": "id", "sortable": false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                "width": "15%",
                "name": "ClientID",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return '<a class="a-1" href="/clients/edit/' + row[0] + '?flag=' + row[7] + '">' + row[1] + '</a>';
                    //return '<a class="a-1 clientEdit" href="javascript:;" data-id="' + row[0] + '" data-dn="' + row[6] + '">' + row[1] + ' </a> ';
                }
            },
            {
                "width": "20%",
                "name": "subdomain",
                "orderable": true,
                render: function (data, type, row, meta) {
                    //return row[1]
                    return row[2];
                }
            },

            {
                "width": "20%",
                "name": "ClientName",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[3]
                }
            },
            {
                "width": "20%",
                "name": "PortalName",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[4]
                }
            },
            {
                "width": "20%",
                "bVisible": true,
                "class": "text-right",
                "orderable": false,
                //"bVisible": (editval1 != "No" ? true : false),
                "bVisible": (editval1 != "No" ? true : false),
                "render": function (data, type, row, meta) {
                   // return '<a href="/gateways/edit?subdomain=' + row[2] + '&flag=' + row[7]+'"  class="btn  btn-sm"><i class="fa fa-edit"></i> Edit</a>';
                    //return '<button class="btn  btn-sm gatewayEdit" data-id="' + row[2] + '" data-dn="' + row[5] + '"><i class="fa fa-edit"></i> Edit</button>';
                    //if (editval1 != "No") {

                    //}
                    //else {
                    //    // table.columns([6]).visible(false);
                    //    return '';
                    //}

                    if (editval1 != "No") {

                        return '<a href="/gateways/edit?subdomain=' + row[2] + '&flag=' + row[7] +'"  class="btn  btn-sm"><i class="fa fa-edit"></i> ' + $("#hdnEdit").val() + '</a>';

                    }
                    else {
                        return '';
                    }
                },
            }

        ]
    });
}
