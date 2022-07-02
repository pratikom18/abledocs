$(document).ready(function () {






    $(document).on("click", "#saveBtn", function () {
        if (!$("#userForm").valid()) {
            return false;
        }
    });



    $(document).on("click", "#UpdatePasswordBtn", function (e) {
        e.preventDefault();
        if (!$("#passwordForm").valid()) {
            return false;
        }

        $.ajax({
            type: "POST",
            url: "Users/ChangePassword",
            data: $("#passwordForm").serialize(),
            success: function (data) {
                if (data.status == true) {
                    $.notify({
                        icon: 'add_alert',
                        title: '<strong>Success!</strong>',
                        message: data.message
                    }, {
                        type: 'success'
                    });
                    $("#bootstrap-modal1").modal("hide");
                } else {
                    $.notify({
                        icon: 'add_alert',
                        title: '<strong>Alert!</strong>',
                        message: data.message
                    }, {
                        type: 'danger'
                    });
                }
            },
            error: function () {
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
function Getuserprofile(id) {
  
    $('#popupUserProfileDetail').load('/Users/UserProfile?id=' + id, function () {
        $('#bootstrap-modal1').modal({
            show: true
        });
         $('#FirstName').focus();

    });

}

function ChangePassword(id) {

    $('#popupUserProfileDetail').load('/Users/ChangePassword?id=' + id, function () {
        $('#bootstrap-modal1').modal({
            show: true
        });
        $('#CurrentPassword').focus();

    });

}

