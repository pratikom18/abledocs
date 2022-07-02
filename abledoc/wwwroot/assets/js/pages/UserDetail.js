$(document).ready(function () {



    var validator = $('#userForm').validate({
        rules: {
            FirstName: {
                required: true
            },
            LastName: {
                required: true
            },
            Username: {
                required: true
            },
            Password: {
                required: true
            },
            Email: {
                required: true
            },
            Title: {
                required: true
            },
            UserRoleId: {
                required: true
            }
        },
        messages: {
            FirstName: {
                required: "First name is required."
            },
            LastName: {
                required: "Last name is required."
            },
            Username: {
                required: "User name is required."
            },
            Password: {
                required: "Password is required."
            },
            Email: {
                required: "Email is required."
            },
            Title: {
                required: "Title is required."
            },
            UserRoleId: {
                required: "Title is required."
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
        if (!$("#userForm").valid()) {
            return false;
        }
    });
});

