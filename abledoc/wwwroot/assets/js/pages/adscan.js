$('#ADscanForm').validate({
    rules: {

        starturl: {
            required: true
        },

    },


    errorElement: 'span',
    errorPlacement: function (error, element) {
        error.addClass('invalid-feedback');
        element.closest('.form-group').append(error);
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass('is-invalid');
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass('is-invalid');
    },
    submitHandler: function (form) {
        form.submit(); // form validation success, call ajax form submit
    }
});



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

    $(document).on("click", ".adscanEdit", function () {

        var database = $(this).attr("data-dn");
        var id = $(this).attr("data-id");


        $.ajax({
            type: "POST",
            url: "/ADscan/SetContactDatabase",
            data: { databasename: database },
            dataType: "JSON",
            success: function (data) {

                window.location.href = "/adscan/edit/" + id;


            },
            error: function () {
                alert("error");
            }
        });


    });
});

function search() {
    if ($.fn.dataTable.isDataTable('#ADScanTable')) {
        $('#ADScanTable').DataTable().destroy();
        ToDatatable();
    }
    else {
        ToDatatable();
    }

}

function ToDatatable() {
    var url = '/ADscan/ADScanLists';

    var table = $('#ADScanTable').DataTable({
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
            //if (aData[4] == "Problem") {
            //    $(nRow).addClass("Problem");
            //}
            //else if (aData[4] == "Complete") {
            //    $(nRow).addClass("Complete");
            //}
            var percent_scanned = "";
            if (aData[6] > 0) {
                percent_scanned = parseFloat((100 * (parseFloat(aData[7]) + parseFloat(aData[8])) / parseFloat(aData[6])), 0);
                $(nRow).addClass("Progress_" + percent_scanned);
            }
        },

        "aoColumns": [
            
            {
                "name": "ID",
                "orderable": true,
                render: function (data, type, row, meta) {
                    
                    return '<a class="a-1" href="/adscan/edit/' + row[0] + '?flag='+row[11]+'">' + row[0] + '</a>';
                    //return '<a class="a-1 adscanEdit" href="javascript:;" data-id="' + row[0] + '" data-dn="' + row[9] + '">' + row[0] + '</a>';
                }
            },
            {
                "name": "clientID",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return '<a class="a-1" href="/clients/edit/' + row[1] + '?flag='+row[11]+'">' + row[2] + ' </a> :  '+ row[3] +' ';
                    //return '<a class="a-1 clientEdit" href="javascript:;" data-id="' + row[1] + '" data-dn="' + row[10] + '">' + row[2] + ' </a> :  ' + row[3] + '';
                }
            },
            {
                "name": "Status",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[4]
                }
            },
            {
                "name": "starturl",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return '<a class="a-1" href="'+ row[5] +'">' + row[5] + '</a>';
                    //return '<a href="' + row[5] + '"></a>';
                }
            },
            {
                "name": "Files_Found",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[6]
                }
            },

            {
                "name": "Files_Scanned",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[7]
                }
            },
            {
                "name": "Files_Error",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[8]
                }
            },

        ]
    });
}
