$(document).ready(function () {

    var editval1 = $('#myedit').val();

    var table = $('#UsersTable').DataTable({
        "lengthMenu": [
            [10, 25, 50, 100, -1],
            ['10', '25', '50', '100', 'Show All']
        ],
        "responsive": true,
        "ajax": "/users/usersList",
        "columns": [
            {
                "data": null, "sortable": true,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                "bVisible": true,
                "render": function (data, type, row, meta) {

                    return row.firstName + " " + row.lastName;
                },
            },
            {
                "data": "email",
            },
            {
                "data": "username",
            },
            {
                "bVisible": (editval1 != "No" ? true : false),
                "class": "text-right",
   				"orderable": false,
                "render": function (data, type, row, meta) {
                    if (editval1 != "No") {

                        return '<a href="javascript:;" data-id ="' + row.id + '" data-dn="' + row.databasename + '" class="btn  btn-sm developAlert"><i class="fa fa-edit"></i>' + $("#hdnEdit").val() +'</a>';     
                      
                    }
                    else {
                        return '';
                    }



                    // return '<a href="/users/edit/' + row.id + '"  class="btn btn-info btn-sm"><i class="fa fa-edit"></i>Edit</a>';
                    
                },
            }

        ]
    });

    $(".newUser").click(function () {

        $('#popupUserDetail').load('/Users/UserDetail?id=' + 0, function () {
            $('#bootstrap-modal').modal({
                show: true
            });

            $('#CurrentSupervisor').selectpicker('setStyle', 'btn btn-link');
            $('#Country').selectpicker('setStyle', 'btn btn-link');
            $('#UserRoleId').selectpicker('setStyle', 'btn btn-link');
            $('.filter-option').addClass('filter-option-1');
            $('#FirstName').focus();
        });
        //var $modal = $('#popupUserDetail');
        //$modal.load('/Users/UserDetail?id=' + 0, function () {
        //    $('#CurrentSupervisor').select2();
        //    $('#UserRoleId').select2();
        //    $modal.modal();
        //});
        //$('#popupUserDetail').on('shown.bs.modal', function () {
        //    $('#FirstName').focus();
        //})
    });

    $(document).on("click", ".developAlert", function () {

        var database = $(this).attr("data-dn");
        var id = $(this).attr("data-id");


        $.ajax({
            type: "POST",
            url: "/phases/SetContactDatabase",
            data: { databasename: database },
            dataType: "JSON",
            success: function (data) {

                $('#popupUserDetail').load('/Users/UserDetail?id=' + id , function () {
                    $('#bootstrap-modal').modal({
                        show: true
                    });
                    $('#CurrentSupervisor').selectpicker('setStyle', 'btn btn-link');
                    $('#Country').selectpicker('setStyle', 'btn btn-link');

                    $('#UserRoleId').selectpicker('setStyle', 'btn btn-link');
                    $('.filter-option').addClass('filter-option-1');
                    $('#FirstName').focus();

                });


            },
            error: function () {
                alert("error");
            }
        });


    });

});
