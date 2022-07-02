$(document).ready(function () {

    var editval1 = $('#myedit').val();

    var url = '/Unit/UnitList';

    var table = $('#UnitTable').DataTable({

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
        "columnDefs": [{
            "targets": 0,
            "orderable": false
        }],
        //"aaSorting": [],// for set deafult 1st colunm sorting option to false

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

                "data": "id", "sortable": false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {

                "name": "typename",
                "orderable": true,


            },
            {

                "name": "display_order",
                "orderable": true,


            },
           
            
            
            {
                "bVisible": true,
                "class": "text-right",
                "orderable": false,
                "bVisible": (editval1 != "No" ? true : false),
                "render": function (data, type, row, meta) {

                    if (editval1 != "No") {
                        return '<a href="javascript:;"  class="btn  btn-sm editContent" data-id="' + row[0] + '"><i class="fa fa-edit"></i> ' + $("#myedit").val() + '</a>';
                    }
                    else {
                        
                        return '';
                    }
                    

                },
            }



        ],
    });

  

   


    $("#addNewContent").click(function () {
        
        $('#popupModal').load('/Unit/edit', function () {
            
            $(".select2").select2({
                theme: "material"
            });

            $(".select2-selection__arrow")
                .addClass("material-icons")
                .html("arrow_drop_down");
            $('#bootstrap-modal').modal({
                show: true
            });



        });

    });

    $(document).on("click",".editContent", function () {
        id = $(this).attr("data-id");

        $('#popupModal').load('/Unit/edit?id=' + id, function () {
            
            $(".select2").select2({
                theme: "material"
            });

            $(".select2-selection__arrow")
                .addClass("material-icons")
                .html("arrow_drop_down");
            //$(".modal-title").text("Update");
            $('#bootstrap-modal').modal({
                show: true
            });



        }); 
    });
   
       
  
    $(document).on("click", ".deleteContent", function () {
        id = $(this).attr("data-id");


        //if (confirm("Are you sure ?")) {
        //    $.ajax({
        //        type: "GET",
        //        url: "/Unit/Delete",
        //        data: { id: id },
        //        success: function (data) {


        //            $.notify({
        //                icon: 'add_alert',
        //                title: '<strong>Success!</strong>',
        //                message: data.data
        //            }, {
        //                type: 'success'
        //            });

        //            $("#UnitTable").DataTable().ajax.reload();

        //        },
        //        error: function () {
        //            //toastr.error("Ajax Error found");
        //            $.notify({
        //                icon: 'add_alert',
        //                title: '<strong>Alert!</strong>',
        //                message: "Ajax Error found"
        //            }, {
        //                type: 'danger'
        //            });
        //        }
        //    });
        //}


        swal({
            title: 'Are you sure?',
            //text: 'Some text.',
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#DD6B55',
            confirmButtonText: 'Yes!',
            cancelButtonText: 'No.'
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    type: "GET",
                    url: "/Unit/Delete",
                    data: { id: id },
                    success: function (data) {
                        Swal.fire(data.data, '', 'success')
                        $("#UnitTable").DataTable().ajax.reload();

                    },
                    error: function () {
                        alert("error");
                    }
                });

            } else {
                // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
            }
        });

    });




$(document).on("click", "#saveContent", function () {
    if (!$("#Unitform").valid()) {
        return false;
    }
    $.ajax({
        type: "POST",
        url: "/Unit/Create",
        data: $("#Unitform").serialize(),
        success: function (data) {

            //toastr.success(data.message);
            $.notify({
                icon: 'add_alert',
                title: '<strong>Success!</strong>',
                message: data.message
            }, {
                type: 'success'
            });

            $("#UnitTable").DataTable().ajax.reload();
            $("#bootstrap-modal").modal("hide");
        },
        error: function () {
            //toastr.error("Ajax Error found");
            $.notify({
                icon: 'add_alert',
                title: '<strong>Alert!</strong>',
                message: "Ajax Error found"
            }, {
                type: 'danger'
            });
        }
    });

});

    

   
   


});