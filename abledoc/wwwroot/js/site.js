// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
    $('body').on('shown.bs.modal', '#form-modal,#myModal', function () {
        $('input:visible:enabled:first', this).focus();
    })
});

showInPopup = (url, title, size, param1 , param2) => {
    $.ajax({
        type: 'GET',
        url: url,
        data: { param1: param1, param2: param2 },
        success: function (res) {
            if (size == "big") {
                $('.modal-dialog').css('max-width', '80%');
            } else {
               $('.modal-dialog').css('max-width', '500px');
            }
            //$('.select2').select2();
            
            $('#form-modal .modal-body-main').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
            $('.select-dropdown').selectpicker('setStyle', 'btn btn-link');
            $('.filter-option').addClass('filter-option-1');

            $(".modal-select2").select2({
                theme: "material"
            });

            $(".select2-selection__arrow")
                .addClass("material-icons")
                .html("arrow_drop_down");

            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}

jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    if (res.reload == true) {
                        location.reload();
                    }
                    $('#' + res.divId).html(res.html);
                    //$('.select2').select2();

                    $('#form-modal .modal-body-main').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                   // toastr.success(res.message);
                    $.notify({
                        icon: 'add_alert',
                        title: '<strong>Success!</strong>',
                        message: res.message
                    }, {
                        type: 'success'
                    });
                   // location.reload();
                }
                else
                    $('#form-modal .modal-body-main').html(res.html);
                $(".modal-select2").select2({
                    theme: "material"
                });

                $(".select2-selection__arrow")
                    .addClass("material-icons")
                    .html("arrow_drop_down");
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

jQueryAjaxDelete = form => {
    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all').html(res.html);
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}
