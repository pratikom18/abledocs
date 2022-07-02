$(document).ready(function () {

    var assignval1 = $('#assign').val();
    var permissionval1 = $('#permission').val();
    var editval1 = $('#myedit').val();
  //  var RoleID = $('#hdnRoleID').val();

    var table = $('#ClientsTable').DataTable({
        "lengthMenu": [
            [10, 25, 50, 100, -1],
            ['10', '25', '50', '100', 'Show All']
        ],
        "responsive": true,
        "ajax": "/role/UserRoleList",
        "columns": [
            {
                "data": null, "sortable": true,
                "width": "5%",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                "bVisible": true,
                "width": "10%",
                "render": function (data, type, row, meta) {

                    return row.roleName;
                },
            },
            {
                "width": "20%",
                //"bVisible": true,
                "bVisible": (assignval1 != "No" ? true : false),
                "class": "text-center",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    //return '<a href="javascript:;"  onClick="assignRole(' + row.roleId + ')"  class="btn btn-sm">Assign</a>';
                    var assignButton = "";
                  
                    if (assignval1 != "No") {
                        assignButton = '<a href="javascript:;"  onClick="assignRole(' + row.roleId + ')"  class="btn btn-sm">' + $("#hdnAssign").val() + '</a>';
                        //editButton = '<button type="button" data-id="' + row[0] + '" data-database="' + row[9] + '" class="btn  btn-sm contactEdit"><i class="fa fa-edit"></i> Edit</button>';

                    }
                 

                    var ActionButton = assignButton;
                    return ActionButton;
                },
            },
            {
                "width": "30%",
                //"bVisible": true,
                "bVisible": (permissionval1 != "No" ? true : false),
                "class": "text-center",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    //return '<a href="javascript:;" onClick="permissionSetting(' + row.roleId + ')"  class="btn btn-sm">Permission</a>';
                    var permissionButton = "";

                    if (permissionval1 != "No") {
                        permissionButton = '<a href="javascript:;" onClick="permissionSetting(' + row.roleId + ')"  class="btn btn-sm">' + $("#hdnPermission").val() + '</a>';
                        //editButton = '<button type="button" data-id="' + row[0] + '" data-database="' + row[9] + '" class="btn  btn-sm contactEdit"><i class="fa fa-edit"></i> Edit</button>';

                    }
                    var ActionButton = permissionButton;
                    return ActionButton;
                },
            },
            {
                "data": "displayOrder",
                "width": "10%"
            },
            {
                "width": "10%",
                "bVisible": true,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    if (row.isActive) {
                        return '<input type="checkbox" name="isactive" checked>';
                    } else {
                        return '<input type="checkbox" name="isactive">';
                    }
                    
                },
            },
            {
                "width": "15%",
                "bVisible": (editval1 != "No" ? true : false),
                "class": "text-right",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    if (editval1 != "No") {
                      
                        return '<a href="javascript:;" id="edit1" onClick="developAlert(' + row.roleId + ')"  class="btn btn-sm"><i class="fa fa-edit"></i>' + $("#myedit").val() + '</a>';
                    }
                    else {
                       // table.columns([5]).visible(false);
                        return '';
                    }
                                      
                },
            }

        ]
    });


    $(".newUserRole").click(function () {
        $('#popupRoleDetail').load('/Role/RoleDetail?id=' + 0, function () {
            $('#bootstrap-modal').modal({
                show: true
            });
            $('#RoleName').focus();

        });

    });

   
    $(function () {
       
      //  $('#edit1').hide();
        var $editval = $('#myedit').val();

       // alert(s);
       // UserRolesTable.columns("displayOrder").bVisible
    });



});





function developAlert(id) {
    // return alert("This section is under development");
    $('#popupRoleDetail').load('/Role/RoleDetail?id=' + id, function () {
        $('#bootstrap-modal').modal({
            show: true
        });
        $('#RoleName').focus();

    });
}

function assignRole(id) {
    // return alert("This section is under development");
    $('#popupRoleDetail').load('/Role/RolePermissionList?roleid=' + id, function () {
        $('#bootstrap-modal').modal({
            show: true
        });

    });
    

}

function permissionSetting(id) {
    $('#popupRoleDetail').load('/Role/SettingPermissionList?roleid=' + id, function () {
        $('#bootstrap-modal').modal({
            show: true
        });

    });
}

