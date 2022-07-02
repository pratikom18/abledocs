$(document).ready(function () {
    $(document).on('click', '.permission', function (e) {
        var userid = $('#hdnUserID').val();
        var settingid = $(this).attr('alt-menuid');
        //if ($(this).prop('checked')) {
        //    $(this).closest("tr").find(".view").attr("checked", "checked").parent().addClass("checked");
        //}
        var view = $(this).closest("tr").find(".view").prop('checked');
       
        $.ajax({
            type: "POST",
            url: "/home/UpsertSettingPermission",
            data: { 'userid': userid, 'settingid': settingid, 'view': view },
            datatype: "JSON",
            success: function (data) {
                //BoostrapMessage();
            },
            error: function () {
            }
        });
    });

    $(document).on('click', '.AllView', function (e) {

        var userid = $('#hdnUserID').val();

        var view = false;
        if ($(this).is(":checked")) {
            view = true;
        }

        $("#RoleAssignTable tbody tr .view").each(function () {
            var $this = $(this);

            if (view) {
                $this.prop("checked", true);
            } else {
                $this.prop("checked", false);
            }

            var settingid = parseInt($this.attr('alt-menuid'));

            $.ajax({
                type: "POST",
                url: "/home/UpsertSettingPermission",
                data: { 'userid': userid, 'settingid': settingid, 'view': view },
                datatype: "JSON",
                success: function (data) {
                    //BoostrapMessage();
                },
                error: function () {
                }
            });
        });

    });
});

function BoostrapMessage() {
    toastr.success('Record updated sccussefully.');
}