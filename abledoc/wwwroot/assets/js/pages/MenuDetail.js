$(document).ready(function () {



    var validator = $('#menuForm').validate({
        rules: {
            MenuName: {
                required: true
            },
            PageUrl: {
                required: true
            }, IconName: {
                required: true
            }

        },
        messages: {
            MenuName: {
                required: "Menu name is required."
            }
            , PageUrl: {
                required: "Page url is required."
            },
            IconName: {
                required: "Icon name is required."
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
        }
    });



    $(document).on("click", "#saveBtn", function () {
        if (!$("#menuForm").valid()) {
            return false;
        }
    });
});

