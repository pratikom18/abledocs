$(document).ready(function () {

    var editval1 = $('#myedit').val();

    var url = '/Countries/CountriesList';

    var table = $('#CountriesTable').DataTable({

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
            aoData.push({ "name": "CountrySearch", "value": $("#CountryFilter").val() });
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

                "name": "Code",
                "orderable": true,


            },
            {

                "name": "Country",
                "bVisible": true,
                render: function (data, type, row, meta) {
                    return row[2];
                }

            },
            {

                "name": "phone_prefix",
                "orderable": true,

            },
            {

                "name": "currency",
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

    $('#CountryFilter').change(function (e) {
        
        $("#CountriesTable").DataTable().ajax.reload();
    });


    $('#CountryFilter1').change(function (e) {
        //alert($(this).val());
        $.ajax({
            type: "GET",
            url: "/Countries/DefaultSMTP",
            data: { code: $(this).val() },
            success: function (data) {


                $.notify({
                    icon: 'add_alert',
                    title: '<strong>Success!</strong>',
                    message: 'Successfully Default SMTP '
                }, {
                    type: 'success'
                });

               

            },
            error: function () {
                //toastr.error("Ajax Error found");
               
            }
        });
    });
   


    $("#addNewContent").click(function () {
        
        $('#popupModal').load('/Countries/Edit', function () {
            
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

        $('#popupModal').load('/Countries/Edit?id=' + id, function () {
            
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
        if (confirm("Are you sure ?")) {
            $.ajax({
                type: "GET",
                url: "/Countries/Delete",
                data: { id: id },
                success: function (data) {


                    $.notify({
                        icon: 'add_alert',
                        title: '<strong>Success!</strong>',
                        message: data.data
                    }, {
                        type: 'success'
                    });

                    $("#CountriesTable").DataTable().ajax.reload();

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
        }

    });




$(document).on("click", "#saveContent", function () {
    if (!$("#Countriesform").valid()) {
        return false;
    }
    $.ajax({
        type: "POST",
        url: "/Countries/Create",
        data: $("#Countriesform").serialize(),
        success: function (data) {

            //toastr.success(data.message);
            $.notify({
                icon: 'add_alert',
                title: '<strong>Success!</strong>',
                message: data.message
            }, {
                type: 'success'
            });

            $("#CountriesTable").DataTable().ajax.reload();
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