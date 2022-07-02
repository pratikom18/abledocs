$(document).ready(function () {
    $('#Country').selectpicker('setStyle', 'btn btn-link');
    $('#Province').selectpicker('setStyle', 'btn btn-link');
    $('#IsSecDeliveryContact').selectpicker('setStyle', 'btn btn-link');
    $('#IsSecBillingContact').selectpicker('setStyle', 'btn btn-link');
    $('#Language').selectpicker('setStyle', 'btn btn-link');
    $('#ClientID').selectpicker('setStyle', 'btn btn-link');
    $('.filter-option').addClass('filter-option-1');
    $('#CreateADScanUser').click(function () {
        
        //var CreateADScanUser1 = {
        //    Email: $("#Email").val(),
        //    CompanyID: $("#ClientID").val(),
        //    First_Name: $("#FirstName").val(),
        //    Last_Name: $("LastName").val(),
        //    Company_Address: $("#Address1").val(),
        //    Phone_Num: $("#Telephone").val(),
        //    Postal_Code: $("#PostalCode").val(),
        //    City: $("#City").val(),
        //    Province: $("#Province").val(),
        //    Country: $("#Country").val(),
        //    ADO_Contact_ID: $("#ID").val(),
        //    Lang: $("#Language").val(),
        //    Role:"Owner"
        //}
        //var CreateADScanUser = JSON.stringify(CreateADScanUser1);
        
        $.ajax({
            type: "POST",
            url: "/contacts/createadscanuser",
            data: {
                Email: $("#Email").val(),
                CompanyID: $("#ClientID").val(),
                First_Name: $("#FirstName").val(),
                Last_Name: $("LastName").val(),
                Company_Address: $("#Address1").val(),
                Phone_Num: $("#Telephone").val(),
                Postal_Code: $("#PostalCode").val(),
                City: $("#City").val(),
                Province: $("#Province").val(),
                Country: $("#Country").val(),
                ADO_Contact_ID: $("#ID").val(),
                Lang: $("#Language").val(),
                Role: "Owner",
                flag:$('#flag').val()
            },
            dataType: "json",
            success: function (data) {
                $("#CreateADScanUser").hide();
                $("#ResetADScanUser").show();
                alert(data.msg)
            }
        });
        //$.ajax({
        //    type: "POST",
        //    url: "/contacts/createadscanuser",
        //    data: JSON.stringify(CreateADScanUser),
        //    contentType: 'application/json',
        //    dataType: "JSON",
        //    success: function (data) {
        //        $("#CreateADScanUser").hide();
        //        $("#ResetADScanUser").show();
        //        alert(data.msg)

        //    },
        //    error: function () {
        //        alert("error");
        //    }
        //});
    })

    $('#ResetADScanUser').click(function () {
       var flag = $("#flag").val();
        $.ajax({
            type: "POST",
            url: "/contacts/resetadscanuser",
            data: { ID: $("#ID").val(), flag: flag},
            dataType: "JSON",
            success: function (data) {
                alert(data.msg)
            },
            error: function () {
                alert("error");
            }
        });
    })

    $(document).on("change", "#ClientID", function () {
        var ClientID = $(this).val();
        var contactid = $("#ID").val();
        var flag = $("#flag").val();
        if (contactid == 0) {
            $.ajax({
                type: "GET",
                url: "/contacts/getClientAddress",
                data: { ClientID: ClientID, flag: flag },
                dataType: "JSON",
                success: function (data) {
                    //console.log(data)
                    $("#Address1").val(data.result.address1).trigger('change');
                    $("#Address2").val(data.result.address2).trigger('change');
                    
                    $('#Country')
                        .val(data.result.country)
                        .trigger('change');
                    
                    setTimeout(function () {   //calls click event after a certain time
                        $("#Province").val(data.result.province).trigger('change');
                    }, 500);
                    $("#City").val(data.result.city).trigger('change');
                    $("#PostalCode").val(data.result.postalCode).trigger('change');
                },
                error: function () {
                    alert("error");
                }
            });

        }

    });

    $(document).on("click", ".contactDelete", function () {

        var database = $(this).attr("data-database");
        var id = $(this).attr("data-id");
        var flag = $(this).attr("data-flag");
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
                    url: "/contacts/DeleteContact",
                    data: { id: id, flag: flag },
                    dataType: "JSON",
                    success: function (data) {

                        if (data.result == true) {
                            window.location.href = "/contacts";
                            Swal.fire(data.msg, '', 'success')
                        } else {
                            Swal.fire(data.msg, '', 'error')
                        }


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

    $('#contactForm').validate({
        rules: {

            FirstName: {
                required: true
            },
            LastName: {
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
                    text: 'Select state'
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
});