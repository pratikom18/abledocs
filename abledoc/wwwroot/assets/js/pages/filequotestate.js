$(document).ready(function () {
    $('#FilePreviewForm').validate({
        rules: {
            Price: {
                required: true
            },
            Quantity: {
                required: true
            },
            PageCount: {
                required: true
            },
        },
        //messages: {
        //    RoleName: {
        //        required: "Price is required."
        //    }
        //},
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('div').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
});