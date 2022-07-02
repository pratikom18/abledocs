var counter = $("#addition_contact_counter").val();
var cloneContact = function (thisObj) {

    $(".contact-select2").select2("destroy");
    // $('.select-dropdown').selectpicker('setStyle', '');
    counter++;
    var parentObj = $('.cloneContact1');//$(thisObj).parents('.cloneContact');
    var cloneObj = parentObj.clone();
    
    cloneObj.find('.RateType').attr("id", "TypeFilter_" + counter + "_RateType");
    cloneObj.find('.RateType').attr("name", "TypeFilter[" + counter + "][RateType]");
    cloneObj.find('.RateType').val("");

    cloneObj.find('.MultiRate').attr("id", "TypeFilter_" + counter + "_Rate");
    cloneObj.find('.MultiRate').attr("name", "TypeFilter[" + counter + "][Rate]");
    cloneObj.find('.MultiRate').val("");
    cloneObj.addClass("cloneContact");
    cloneObj.removeClass("cloneContact1");


    if (counter > 0) {
        cloneObj.find(".addBtn").hide();
        cloneObj.find(".removeBtn").show();
    }
    
    cloneObj.show();
    $(".cloneContact:last").after(cloneObj);
    $('.select-dropdown').selectpicker('setStyle', 'btn btn-link');
    $('.filter-option').addClass('filter-option-1');

    $(".RateType").select2({
        placeholder: "Select",
        theme: "material"
    });

    $(".select2-selection__arrow")
        .addClass("material-icons")
        .html("arrow_drop_down");

    // $(".select2-container").css("width", "auto");

}
var removeContact = function (thisObj) {
    counter--;
    var multiID = $(thisObj).parents('.cloneContact').find('.removeBtn').attr('data-id');

    if (multiID != 0) {
        $.ajax({
            url: '/Clients/DeleteMultirate',
            data: { multiID: multiID, flag: $('#flag').val() },
            type: 'POST'
        }).done(function (responseData) {


        }).fail(function () {
            console.log('Failed');
        });
    }
    
    $(thisObj).parents('.cloneContact').remove();
}


$(document).ready(function () {

    $("#AddLegacy").click(function () {
       // alert("hi");
        var input = $("<input>").attr("type", "hidden").attr("name", "AddLegacy").val("1");
        $('#clientsForm').append(input).submit();
    });

    var editval1 = $('#myedit').val();

    var validator = $('#clientForm').validate({
        rules: {

            Company: {
                required: true
            }

        },

        messages: { // custom messages for radio buttons and checkboxes
            Company: {
                required: "Company name is required."
            }

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

    $(document).on("click", "#saveBtn", function () {
        if (!$("#clientForm").valid()) {
            return false;
        }
    });

    $(".addUrl").click(function () {

        $('#popupModal').load('/Clients/AddUrl?id=' +  $("#ID").val()+'&flag='+$('#flag').val(), function () {
            $('#bootstrap-modal').modal({
                show: true
            });
            
            

        });

    });


    $(".addGateway").click(function () {

        $('#popupModal').load('/Clients/AddGateway?id=' + $("#ID").val() + '&flag=' + $('#flag').val(), function () {
            $('#bootstrap-modal').modal({
                show: true
            });
           

        });

    });


    $(".addContact").click(function () {

        $('#popupModal').load('/Clients/AddContact?id=' + $("#ID").val() + '&flag=' + $('#flag').val(), function () {
            $('#bootstrap-modal').modal({
                show: true
            });
            

        });

    });

    $("#Country").on("change", function () {

        $.ajax({
            type: "POST",
            url: "/Clients/GetStateList",
            data: { country_code: $(this).val() },
            dataType: "JSON",
            success: function (data) {

               // console.log(data);
                $('#Province').empty();
                $('#Province').append($('<option>', {
                    value: '',
                    text:'Select state'
                }));
                $.each(data.result, function (index, item) {

                    $('#Province').append($('<option>', {
                        value: item.state,
                        text: item.state
                    }));
                });
                

            },
            error: function () {
                alert("error");
            }
        });
    });

    $(document).on("change", ".updatemultirate", function () {
        
        var $row = $(this).closest('.cloneContact');
        var RateType =  $row.find('.RateType').val();
        var MultiRate = parseFloat( $row.find('.MultiRate').val());
        var multiID = parseInt( $row.find('.removeBtn').attr('data-id')); 
        var clientID = $('#ID').val();
        var flag= $('#flag').val();

        $.ajax({
            url: '/Clients/UpdateMultirate',
            data: { multiID: multiID, clientID: clientID, RateType: RateType, MultiRate: MultiRate, flag: flag },
            type: 'POST'
        }).done(function (responseData) {
                
            if (multiID == 0) {
                $row.find('.removeBtn').attr('data-id', responseData.id);
            }

        }).fail(function () {
            console.log('Failed');
        });

    });



    var url = '/clients/ClientList';

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
        //"scrollY": height,
        "scrollX": true,
        "order": [],
        "autoWidth": false,
        "columnDefs": [{
            "targets": 0,
            "orderable": false
        }],
        responsive: true,
        //"aaSorting": [],// for set deafult 1st colunm sorting option to false

        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "AlphaSearch", "value": $("#AlphaSearch").val() });
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
                "width": "10%",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {

                "name": "Code",
                "width": "20%",
                "bVisible": true,
                render: function (data, type, row, meta) {
                    if (row[6] != "") {
                        Code = row[6] + "-" + row[7] + "-" + row[1];
                    } else {
                        Code = row[1];
                    }

                    return Code;
                }

            },
            {
                "width": "20%",
                "name": "Company",
                "orderable": true,


            },

            {
                "width": "20%",
                "name": "Email",
                "orderable": true
            },
            {
                "width": "10%",
                "name": "City",
                "orderable": true
            },
            {
                "width": "10%",
                "name": "Country",
                "orderable": true
            },
            {
                // "bVisible": true,
                "width": "10%",
                "bVisible": (editval1 != "No" ? true : false),
                "class": "text-right",
                "orderable": false,
                "render": function (data, type, row, meta) {

                    if (editval1 != "No") {
                        return '<a href="/clients/edit/' + row[0] + '?flag='+row[9]+'"  class="btn  btn-sm"><i class="fa fa-edit"></i> ' + $("#hdnEdit").val() + '</a>';
                        //return '<button type="button" data-id="' + row[0] + '" data-database="' + row[8] + '" data-flag="'+row[9]+'" class="btn  btn-sm clientEdit"><i class="fa fa-edit"></i> Edit</button>';
                    }
                    else {
                        // table.columns([6]).visible(false);
                        return '';
                    }

                },
            }



        ],
    });

    $(document).on("click", ".clientEdit", function () {

        var database = $(this).attr("data-database");
        var id = $(this).attr("data-id");


        $.ajax({
            type: "POST",
            url: "/clients/SetContactDatabase",
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



    $(".alphabet").on('click', 'span', function () {
        $(".alphabet").find('.active').removeClass('active');
        $(this).addClass('active');
        var AlphaSearch = $(this).html();
        if ($(this).html() == "All") {
            AlphaSearch = "";
        }

        $("#AlphaSearch").val(AlphaSearch);
        table.draw();
    });

    $('.tooltips').tooltip();


    $('.client-datatables').DataTable({
        //"pagingType": "full_numbers",

        /*"lengthMenu": [
            [10, 25, 50, -1],
            [10, 25, 50, "All"]
        ],*/
        "searching": false,
        "paging": false,
        "info": false,
        responsive: true,
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Search records",
        }
    });

});

function upsertCSR(id) {
    
    var code = $("#Code").val();
    var clientid = $("#ID").val();
    $('#popupModal').load('/Clients/AddCSR?id=' + id + "&code=" + code + "&clientid=" + clientid + '&flag=' + $('#flag').val(), function () {
            $('#bootstrap-csr').modal({
                show: true
            });


        });

}
function deleteCSR(id, this1) {
    //var code = $("#Code").val();
    //var clientid = $("#ID").val();
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
                url: '/Clients/DeleteCSR',
                data: { id: id, flag: $('#flag').val() },
                type: 'POST'
            }).done(function (responseData) {
                console.log('Done: ', responseData);
                var row = this1;
                //row.parents('td').hide();
               // row($(this).parents('tr')).hide();
                $(row).closest("tr").remove();

            }).fail(function () {
                console.log('Failed');
            });
        } else {
            // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
        }
    });
}

$(document).on("click", ".deleteGateway", function () {
    var obj = $(this);
    var subdomain = $(this).attr("data-subdomain");
    var ClientID = $(this).attr("data-clientid");
    var flag = $("#flag").val();
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
                type: "POST",
                url: "/Gateways/DeleteGateways",
                data: { ClientID: ClientID, subdomain: subdomain, flag: flag },
                dataType: "JSON",
                success: function (data) {
                    obj.closest("tr").remove();
                    Swal.fire('Deleted Successfully!', '', 'success')


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

