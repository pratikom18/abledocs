$(document).ready(function () {

    var editval1 = $('#myedit').val();

    var table = $('#ClientsTable').DataTable({
        "lengthMenu": [
            [10, 25, 50, 100, -1],
            ['10', '25', '50', '100', 'Show All']
        ],
        "responsive": true,
        "ajax": "/menu/MenuList",
        "columns": [
            {
                "data": null, "sortable": true,
                "width": "10%",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                "bVisible": true,
                "width": "10%",
                "render": function (data, type, row, meta) {

                    return row.menuName;
                },
            },
            {
                "bVisible": true,
                "width": "30%",
                "render": function (data, type, row, meta) {

                    return row.pageUrl;
                },
            },
            {
                "data": "displayOrder",
                "width": "10%"
            },
            {
                "width": "10%",
                "bVisible": true,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    if (row.isActive) {
                        return '<input type="checkbox" name="isactive" checked>';
                    } else {
                        return '<input type="checkbox" name="isactive">';
                    }

                },
            },
            {
                "width": "10%",
                "bVisible": true,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    if (row.IsHeaderMenu) {
                        return '<input type="checkbox" name="isHeaderMenu" checked>';
                    } else {
                        return '<input type="checkbox" name="isHeaderMenu">';
                    }

                },
            },
            {
                "width": "20%",
                "bVisible": (editval1 != "No" ? true : false),
                "class": "text-right",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    
                    if (editval1 != "No") {
                        return '<a href="javascript:;" onClick="developAlert(' + row.menuMasterId + ')"  class="btn btn-sm"><i class="fa fa-edit"></i> ' + $("#myedit").val() + '</a>';
                    }
                    else {
                      //  table.columns([6]).visible(false);
                        return '';
                    }
                   
                    // return '<a href="/users/edit/' + row.id + '"  class="btn btn-info btn-sm"><i class="fa fa-edit"></i>Edit</a>';
                    
                },
            }

        ]
    });

    $(".newMenu").click(function () {
        
        $('#MenuDetail').load('/Menu/MenuDetail?id=' + 0, function () {
            $('#bootstrap-modal').modal({
                show: true
            });
            $('#ParentId').selectpicker('setStyle', 'btn btn-link');
            $('.filter-option').addClass('filter-option-1');
            $('#MenuName').focus();
           
        });
        
    });

});

function developAlert(id) {
    $('#MenuDetail').load('/Menu/MenuDetail?id=' + id, function () {
        $('#bootstrap-modal').modal({
            show: true
        });
        $('#ParentId').selectpicker('setStyle', 'btn btn-link');
        $('.filter-option').addClass('filter-option-1');
        $('#MenuName').focus();

    });
}
