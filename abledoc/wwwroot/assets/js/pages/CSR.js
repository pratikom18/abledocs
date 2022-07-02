$(document).ready(function () {

    ToDatatable();

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
});


function ToDatatable() {

    var url = '/csr/GetCSRList';

    var table = $('#CSRTable').DataTable({
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
                "name": "Client Code",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return '<a class="a-1" href="/clients/edit/' + row[0] + '?flag='+row[12]+'">' + row[1] + '</a>';
                    //return row[0]
                    //return '<a class="a-1 clientEdit" href="javascript:;" data-id="' + row[0] + '" data-dn="' + row[11] + '">' + row[1] + '</a>';
                }
            },
            {
                "name": "Company",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[2];
                }
            },
            
            {
                "name": "Remediation Requirements",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[3]
                }
            },
            {
                "name": "Common Alt",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[4];
                }
            },
            {
                "name": "Metadata/Author(s)",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[5];
                }
            },
            {
                "name": "Unsecured (FINAL)",
                "orderable": true,
                render: function (data, type, row, meta) {
                    if (row[6] == "True") {
                        return '<input type="checkbox" name="assignfiles[]" class="assignfiles" value="1" form="multiassign" checked> <span></span>'
                    } else {
                        return '<input type="checkbox" name="assignfiles[]" class="assignfiles" value="0" form="multiassign"> <span></span>'
                    }
                    
                }
            },
            {
                "name": "Secured (FINAL-ua or FINAL-uae)",
                "orderable": false,
                render: function (data, type, row, meta) {
                    if (row[7] == "True") {
                        return '<input type="checkbox" name="assignfiles[]" class="assignfiles" value="1" form="multiassign" checked> <span></span>'
                    } else {
                        return '<input type="checkbox" name="assignfiles[]" class="assignfiles" value="0" form="multiassign"> <span></span>'
                    }
                }
            },
            {
                "name": "PDF/A (FINAL-a)",
                "orderable": false,
                render: function (data, type, row, meta) {
                    if (row[8] == "True") {
                        return '<input type="checkbox" name="assignfiles[]" class="assignfiles" value="1" form="multiassign" checked> <span></span>'
                    } else {
                        return '<input type="checkbox" name="assignfiles[]" class="assignfiles" value="0" form="multiassign"> <span></span>'
                    }
                }
            },
            {
                "name": "PAC Reports",
                "orderable": true,
                render: function (data, type, row, meta) {
                    if (row[9] == "True") {
                        return '<input type="checkbox" name="assignfiles[]" class="assignfiles" value="1" form="multiassign" checked> <span></span>'
                    } else {
                        return '<input type="checkbox" name="assignfiles[]" class="assignfiles" value="0" form="multiassign"> <span></span>'
                    }
                }
            },
            {
                "name": "Notes",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[10];
                }
            },
            
        ]
    });
}

function UploadFile() {


    var files = $("#importexcelfile").get(0).files;

    if (files.length > 0) {
        var data = new FormData();
        data.append("file", files[0]);
        $.ajax({
            type: "POST",
            url: '/csr/uploadcsr',
            contentType: false,
            processData: false,
            data: data,
            success: function (result) {

                //alert(result);
            },
            error: function () {
                alert("There was error uploading files!");
            }
        });
    }

}

