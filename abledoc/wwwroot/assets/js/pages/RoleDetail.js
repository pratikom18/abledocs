$(document).ready(function () {



    var validator = $('#userRoleForm').validate({
        rules: {
            Rolename: {
                required: true
            }
        },
        messages: {
            RoleName: {
                required: "Role name is required."
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
        if (!$("#userRoleForm").valid()) {
            return false;
        }
    });
});

