$(document).ready(function () {
    $(document).on('click', '.upsert', function (e) {
        var RoleID = $('#hdnRoleID').val();
        var MenuID = $(this).attr('alt-menuid');
        //if ($(this).prop('checked')) {
        //    $(this).closest("tr").find(".view").attr("checked", "checked").parent().addClass("checked");
        //}
        var view = $(this).closest("tr").find(".view").prop('checked');
        var add = $(this).closest("tr").find(".add").prop('checked');
        var update = $(this).closest("tr").find(".update").prop('checked');
        var del = $(this).closest("tr").find(".delete").prop('checked');

        $.ajax({
            type: "POST",
            url: "/role/UpsertRolePermission",
            data: { 'roleid': RoleID, 'menuid': MenuID, 'view': view, 'add': add, 'update': update, 'delete': del },
            datatype: "JSON",
            success: function (data) {
                //BoostrapMessage();
            },
            error: function () {
            }
        });
    });

    $(document).on('click', '.AllView', function (e) {
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

        });

        chkChecked();
        
    });

    $(document).on('click', '.AllAdd', function (e) {
              
        var view = false;
        if ($(this).is(":checked")) {
            view = true;
        }

        $("#RoleAssignTable tbody tr .add").each(function () {
            var $this = $(this);

            if (view) {
                $this.prop("checked", true);
            } else {
                $this.prop("checked", false);
            }

        });
        chkChecked();
    });

    $(document).on('click', '.AllEdit', function (e) {

        var view = false;
        if ($(this).is(":checked")) {
            view = true;
        }

        $("#RoleAssignTable tbody tr .update").each(function () {
            var $this = $(this);

            if (view) {
                $this.prop("checked", true);
            } else {
                $this.prop("checked", false);
            }

        });
        chkChecked();
    });

    $(document).on('click', '.AllDelete', function (e) {

        var view = false;
        if ($(this).is(":checked")) {
            view = true;
        }

        $("#RoleAssignTable tbody tr .delete").each(function () {
            var $this = $(this);

            if (view) {
                $this.prop("checked", true);
            } else {
                $this.prop("checked", false);
            }

        });

        chkChecked();
       
    });

    $(document).on('click', '.All', function (e) {

        var view = false;
        if ($(this).is(":checked")) {
            view = true;
            $('.AllView').prop("checked", true);
            $('.AllAdd').prop("checked", true);
            $('.AllEdit').prop("checked", true);
            $('.AllDelete').prop("checked", true);
        } else {
            view = false;
            $('.AllView').prop("checked", false);
            $('.AllAdd').prop("checked", false);
            $('.AllEdit').prop("checked", false);
            $('.AllDelete').prop("checked", false);
        }

        $("#RoleAssignTable tbody tr").each(function () {
            var $this = $(this);


            if (view) {
                $this.find(".view").prop("checked", true);
                $this.find(".add").prop("checked", true);
                $this.find(".update").prop("checked", true);
                $this.find(".delete").prop("checked", true);
                $this.find(".allChk").prop("checked", true);
            } else {
                $this.find(".view").prop("checked", false);
                $this.find(".add").prop("checked", false);
                $this.find(".update").prop("checked", false);
                $this.find(".delete").prop("checked", false);
                $this.find(".allChk").prop("checked", false);
            }

        });

        chkChecked();
    });

    $(document).on('click', '.allChk', function (e) {

        var RoleID = $('#hdnRoleID').val();
        var view = false;
        if ($(this).is(":checked")) {
            view = true;
        }

        if (view) {
            $(this).closest("tr").find(".view").prop("checked", true);
            $(this).closest("tr").find(".add").prop("checked", true);
            $(this).closest("tr").find(".update").prop("checked", true);
            $(this).closest("tr").find(".delete").prop("checked", true);
        } else {
            $(this).closest("tr").find(".view").prop("checked", false);
            $(this).closest("tr").find(".add").prop("checked", false);
            $(this).closest("tr").find(".update").prop("checked", false);
            $(this).closest("tr").find(".delete").prop("checked", false);
        }

        var MenuID = $(this).closest("tr").find(".view").attr('alt-menuid');
      
        var view1 = view;
        var add = view;
        var update = view;
        var del = view;

        $.ajax({
            type: "POST",
            url: "/role/UpsertRolePermission",
            data: { 'roleid': RoleID, 'menuid': MenuID, 'view': view1, 'add': add, 'update': update, 'delete': del },
            datatype: "JSON",
            success: function (data) {
                //BoostrapMessage();
            },
            error: function () {
            }
        });

    });
});

function chkChecked() {
    var RoleAssignList = [];
    $("#RoleAssignTable tbody tr").each(function () {
        var $this = $(this);
        var RoleAssign = {};

        var RoleID = parseInt($('#hdnRoleID').val());
        var view1 = $this.find(".view").prop('checked');
        var add = $this.find(".add").prop('checked');
        var update = $this.find(".update").prop('checked');
        var del = $this.find(".delete").prop('checked');
        var MenuID = parseInt($this.find(".view").attr('alt-menuid'));

        $.ajax({
            type: "POST",
            url: "/role/UpsertRolePermission",
            data: { 'roleid': RoleID, 'menuid': MenuID, 'view': view1, 'add': add, 'update': update, 'delete': del },
            datatype: "JSON",
            success: function (data) {
                //BoostrapMessage();
            },
            error: function () {
            }
        });

        //var RoleAssign = {};
        //RoleAssign.roleid = parseInt($('#hdnRoleID').val());
        //RoleAssign.isview = $this.find(".view").prop('checked');
        //RoleAssign.isadd = $this.find(".add").prop('checked');
        //RoleAssign.isupdate = $this.find(".update").prop('checked');
        //RoleAssign.isdelete = $this.find(".delete").prop('checked');
        //RoleAssign.menuid = parseInt($this.find(".view").attr('alt-menuid'));
        
         //RoleAssign.roleid: parseInt(RoleID), menuid: parseInt(MenuID), isview: view, isadd: add, isupdate: update, isdelete: del 
        //RoleAssignList.push(RoleAssign);

    });
    //RoleAssignList = alert(JSON.stringify({ 'RoleAssignList': RoleAssignList }))
    //$.ajax({
    //    contentType: 'application/json; charset=utf-8',
    //    dataType: 'json',
    //    type: 'POST',
    //    url: '/role/updateAssign',
    //    data: '{RoleAssignList:' + JSON.stringify(RoleAssignList) + '}',//JSON.stringify({ 'RoleAssignList': RoleAssignList }),
    //    success: function (data) {

    //    }
    //})
}

function BoostrapMessage() {
    toastr.success('Record updated sccussefully.');
}