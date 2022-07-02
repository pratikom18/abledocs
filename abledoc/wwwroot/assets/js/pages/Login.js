//import { error } from "jquery";

function setFormValidation(id) {
    $(id).validate({
        highlight: function (element) {
            $(element).closest('.form-group').removeClass('has-success').addClass('has-danger');
            $(element).closest('.form-check').removeClass('has-success').addClass('has-danger');
        },
        success: function (element) {
            $(element).closest('.form-group').removeClass('has-danger').addClass('has-success');
            $(element).closest('.form-check').removeClass('has-danger').addClass('has-success');
        },
        errorPlacement: function (error, element) {
            $(element).closest('.form-group').append(error);
        },
    });
}

$(document).ready(function () {
    setFormValidation('#login-form');
});

$(document).ready(function () {

   
    $(document).on("click", "#btnlogin", function () {
        SignIN();
    });

    $('#txtPassword').keypress(function (e) {
        if (e.keyCode == 13) {
            SignIN();
        }
    });
    $('#txtUserName').keypress(function (e) {
        if (e.keyCode == 13) {
            SignIN();
        }
    });
});

function SignIN() {
    if ($("#login-form").valid()) {
        $('#btnlogin').addClass('disabled')
        $('.loding-1').show();
        $('.loding-2').hide();
        var username = $("#txtUserName").val();
        var password = $("#txtPassword").val();
       // var location = $("#txtLocation").val();
        var keepme = $('#remember').is(':checked') ? true : false;
        $.ajax({
            type: "POST",
            url: "login/signin",
            data: { 'UserName': username, 'Password': password, 'Remember': keepme},
            datatype: "JSON",
            success: function (data) {

                if (data.messageKey == "True") {
                    location.href = "/";
                }
                else {
                    $("#message").html(data.message)
                    $("#message").show();
                    $('#btnlogin').removeClass('disabled')
                    $('.loding-1').hide();
                    $('.loding-2').show();
                }
            },
            error: function () {
                
                $('#btnlogin').removeClass('disabled')
                $('.loding-1').hide();
                $('.loding-2').show();
            }
        });
    }
}