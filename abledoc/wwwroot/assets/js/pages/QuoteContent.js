$(document).ready(function () {
    var editval1 = $('#myedit').val();
    var deleteval1 = $('#deletebtn').val();


    var url = '/QuoteContent/ContentList';

    var table = $('#ContentTable').DataTable({

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
            aoData.push({ "name": "TypeSearch", "value": $("#TypeFilter").val() });
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

                "name": "Type",
                "bVisible": true,
                render: function (data, type, row, meta) {
                    return row[1];
                }

            },
            {
                "name": "Information",
                "orderable": true,
            },

            {
                "name": "typename",
                "orderable": true
            },
            {
                "name": "display_order",
                "orderable": true
            },
            {
                "bVisible": true,
                "class": "text-right",
                "orderable": false,
                "bVisible": ((editval1 != "No" || deleteval1 != "No") ? true : false),
                "render": function (data, type, row, meta) {

                    if (editval1 != "No") {
                        editBtn = '<a href="javascript:;"  class="btn  btn-sm editContent" data-id="' + row[0] + '"><i class="fa fa-edit"></i> ' + $("#myedit").val() + '</a>';
                    }
                    else {

                        editBtn = "";
                    }
                    if (deleteval1 != "No") {
                        deleteBtn = '<a href="javascript:;"  class="btn btn-danger btn-sm deleteContent" data-id="' + row[0] + '"><i class="fa fa-trash"></i> ' + $("#deletebtn").val() + '</a>';
                    }
                    else {

                        deleteBtn = "";
                    }
                    actionBtn = editBtn + " " + deleteBtn;
                    return actionBtn;
                },
            }



        ],
    });

    $('#TypeFilter').change(function (e) {

        $("#ContentTable").DataTable().ajax.reload();
    });




    $("#addNewContent").click(function () {

        $('#popupModal').load('/QuoteContent/Edit', function () {

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

    $(document).on("click", ".editContent", function () {
        id = $(this).attr("data-id");

        $('#popupModal').load('/QuoteContent/Edit?id=' + id, function () {
            var val = $("#Type").val();
            if (val == "Tax") {
                $(".tax-div").show();
                $(".language-div").hide();
            } else {
                $(".tax-div").hide();
                $(".language-div").show();
            }

            $.getJSON("/QuoteContent/checkcountryrequired", { type: val }, function (result) {
                if (result.data.country_required == 1) {
                    $(".country-dropdown").show();
                } else {
                    $(".country-dropdown").hide();
                }
            });

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
                    url: "/QuoteContent/Delete",
                    data: { id: id },
                    success: function (data) {


                        $.notify({
                            icon: 'add_alert',
                            title: '<strong>Success!</strong>',
                            message: data.data
                        }, {
                            type: 'success'
                        });

                        $("#ContentTable").DataTable().ajax.reload();

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
                //Swal.fire('Saved!', '', 'success')
            } else {
                // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
            }
        });


    });




    $(document).on("click", "#saveContent", function () {
        if (!$("#quotecontentform").valid()) {
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/QuoteContent/Create",
            data: $("#quotecontentform").serialize(),
            success: function (data) {

                //toastr.success(data.message);
                $.notify({
                    icon: 'add_alert',
                    title: '<strong>Success!</strong>',
                    message: data.message
                }, {
                    type: 'success'
                });

                $("#ContentTable").DataTable().ajax.reload();
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

    $(document).on("change", "#Language", function () {
        val = $(this).val();
        Type = $("#Type").val();

        $.getJSON("/QuoteContent/GetMaxDisplayOrder", { lang: val, type: Type }, function (result) {
            $("#display_order").val(result.data.display_order);

        });
    });

    $(document).on("change", "#Type", function () {
        val = $(this).val();

        if (val == "Tax") {
            $(".tax-div").show();
            $(".language-div").hide();
        } else {
            $(".tax-div").hide();
            $(".language-div").show();
        }

        $.getJSON("/QuoteContent/checkcountryrequired", { type: val }, function (result) {
            if (result.data.country_required == 1) {
                $(".country-dropdown").show();
            } else {
                $(".country-dropdown").hide();
            }
        });
    });

    $(document).on("change", "#country_code", function () {

        var country = $(this).val();
        $.ajax({
            type: "POST",
            url: "/QuoteContent/GetStateList",
            data: { country_code: $(this).val() },
            dataType: "JSON",
            success: function (data) {

                // console.log(data);
                $('#province').empty();
                $('#province').append($('<option>', {
                    value: '',
                    text: 'Select state'
                }));
                
                if (country != "") {
                    $.each(data.result, function (index, item) {

                        $('#province').append($('<option>', {
                            value: item.id,
                            text: item.state
                        }));
                    });
                }

            },
            error: function () {
                alert("error");
            }
        });
    });



});