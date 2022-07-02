

$(document).ready(function () {

    $(document).on('keypress', '.txtproduction', function (e) {
        
        if (e.keyCode == 13) {
            e.preventDefault();
            reloadTable();

        }

    });

    $(document).on('keypress', '.txtproduction1', function (e) {

        if (e.keyCode == 13) {
            e.preventDefault();
            reloadTable1();

        }

    });

    var todayDate = $("#currentDate").val().trim();
    var forDue2Days = $("#forDue2Days").val().trim();

    var url = '/Production/ProductionList';
    var url1 = '/Production/ProductionCurrentUserList';

    var userlist = "";
   
    $.ajax({
        type: "POST",
        url: "/Production/GetAssignedUsersList",
        dataType: "JSON",
        success: function (result) {
            //console.log(result.data);
            $('.dropdown-content').html('');
            userlist = result.data;//jQuery.parseJSON(result.data); // parse the data
           
        },
        error: function () {
            alert("error");
        }
    });
    function ToDataTable() {
        var table = $('#ProductionTable').DataTable({
            //"sDom": 'rtlfip',  // for set pagination dd and search button to Top
            "processing": true,
            "bServerSide": true,
            "bSort": true,
            "sAjaxSource": url,
            "aLengthMenu": [
                [10, 25, 50, 100, -1],
                ['10 rows', '25 rows', '50 rows', '100 rows', 'Show All']
            ],
            "iDisplayLength": 10,
            "bLengthChange": true,
            "bDestroy": true,
            "sEmptyTable": "Loading data from server",
            "searching": true,
            "paging": true,
            //"scrollY": height,
            "scrollX": true,
            "order": [],
            "autoWidth": false,
            "responsive": true,
            //"columnDefs": [{
            //    "targets": 0,
            //    "orderable": false
            //}],
            //"aaSorting": [],// for set deafult 1st colunm sorting option to false

            "fnServerData": function (sSource, aoData, fnCallback) {
                aoData.push({ "name": "Status", "value": $("#status").val() });
                aoData.push({ "name": "SearchBy", "value": $("#searchby").val() });
                aoData.push({ "name": "txtSearch", "value": $("#txtSearch").val() });
                $.ajax({
                    "dataType": 'json',
                    "type": "POST",
                    "url": sSource,
                    "data": aoData,
                    "success": fnCallback
                });
            },
            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                $(nRow).attr('id', aData[0]) // or whatever you choose to set as the id
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                
                //if (aData[20] <= todayDate.trim()) {
                //    var redDeadline = "#ffb8b8";
                //    $(nRow).find('td:eq(-6)').css('background-color', redDeadline);
                //}
                //else if (aData[20] <= forDue2Days.trim()) {

                //    var yellowDeadline = "#fffedb";
                //    $(nRow).find('td:eq(-6)').css('background-color', "");
                //    $(nRow).find('td:eq(-6)').css('background-color', yellowDeadline);
                //}
                //else {

                //    $(nRow).find('td:eq(-6)').css('background-color', '#fff');
                //}

            },
            "aoColumns": [
                {
                    "data": "no",
                    "sortable": false,
                    "width": "5%",
                    render: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                
                {
                    "name": "FileID",
                    "width": "5%",
                    "bVisible": true,
                    "render": function (data, type, row, meta) {

                        return '<a class="a-1" href="/file?ID=' + row[0] + '&flag=' + row[18] + '" target="_blank">' + row[0] + '</a>';
                        //return '<a class="a-1 tagEdit" href="javascript:;" data-id="' + row[0] + '" data-dn="' + row[17] + '">' + row[0] + '</a>';
                    },
                },

                {
                    "name": "Code",
                    "width": "5%",
                    "bVisible": true,
                    "render": function (data, type, row, meta) {
                        return '<a class="a-1" href="/clients/edit/' + row[11] + '?flag=' + row[18] + '" target="_blank">' + row[1] + '</a>';
                        //return '<a class="a-1 clientEdit" href="javascript:;" data-id="' + row[11] + '" data-dn="' + row[17] + '">' + row[1] + '</a>';
                    },
                },
                {
                    "name": "JobID",
                    "width": "5%",
                    "orderable": true,
                    "render": function (data, type, row, meta) {

                        return '<a href="/jobs/jobsinitial/' + row[2] + '?flag=' + row[18] + '" target="_blank" class="JobID a-1" data-id="' + row[2] + '">' + row[2] + '</a>';
                        //return '<a href="javascript:;" class="JobID a-1 jobEdit" data-id="' + row[2] + '" data-dn="' + row[17] + '">' + row[2] + '</a>';
                    },
                },
                {
                    "name": "Filename",
                    "width": "25%",
                    "orderable": true,
                    "render": function (data, type, row, meta) {

                        return '<a href="javascript:;" class="wordbreak a-1" onclick="OpenFilePreview(' + row[19] + ',' + row[18] + '); return false;" data-flag="' + row[18] + '">' + row[3] + '</a>';
                    },
                },
                {
                    "name": "Pages",
                    "width": "5%",
                    "orderable": true,
                    "render": function (data, type, row, meta) {

                        return row[4];
                    },
                },
                {
                    "name": "Deadline", "orderable": true,
                    "width": "10%",
                    render: function (data, type, row, meta) {
                        var style = "";
                        if (row[20] <= todayDate) {
                            var redDeadline = "#ffb8b8";
                            style = "background-color:" + redDeadline;
                        }
                        else if (row[20] <= forDue2Days) {
                            var yellowDeadline = "#fffedb";
                            style += "background-color:" + yellowDeadline;
                        }
                        else {
                            style += "background-color:#fff";
                        }
                        return '<div style="' + style + '">' + row[5] + '</div>';
                        //return row[5];

                    }
                },
                {
                    "name": "JobType",
                    "width": "5%",
                    "orderable": true,
                    render: function (data, type, row, meta) {
                        return row[6];
                    }
                },
                {
                    "name": "Progress",
                    "width": "10%",
                    "orderable": false,
                    "bVisible": ($("#status").val() == "Phase 1") ? true : false,
                    render: function (data, type, row, meta) {
                        finalpercent = 0;
                        if (row[4] > 0) {
                            finalpercent = (parseInt(row[7]) + parseInt(row[8])) * 42.5 / parseInt(row[4]);
                        }

                        if (row[12] == '1') {
                            finalpercent += 5;
                        }

                        if (row[13] == '1') {
                            finalpercent += 5;
                        }

                        if (row[14] == '1') {
                            finalpercent += 5;
                        }

                        return '<div class="progress position-relative" style="height: 25px;font-size:20px"><div class="progress-bar bg-success" role = "progressbar" style = "width: ' + finalpercent + '%" aria - valuenow="60" aria - valuemin="0" aria - valuemax="100" ></div><small class="justify-content-center d-flex position-absolute w-100">' + Math.round(finalpercent) + '%</small></div>';
                    }
                },

                {
                    "name": "CurrentCheckout",
                    "width": "5%",
                    "orderable": true,
                    "bVisible": ($("#status").val() != "Deliverables") ? true : false,
                    render: function (data, type, row, meta) {
                        return row[9];
                    }
                },
                {
                    "name": "AssignedTo",
                    "width": "10%",
                    "orderable": false,
                    "bVisible": ($("#status").val() != "Deliverables") ? true : false,
                    render: function (data, type, row, meta) {

                        var options = '<option></option>';
                        '<input type="hidden" id="FileID" name="assignfiles[]" value="' + row[0] + '">';
                        var html = '<select required name="user" class="form-control dropdown-content assignedChange" file-id="' + row[0] + '" data-flag="' + row[18] + '">';

                        $.each(userlist, function (i, v) {   // get data using each loop
                            if (row[10] == v.id) {
                                selected = 'selected';
                            } else {
                                selected = '';
                            }
                            options += "<option " + selected + " value=" + v.id + ">" + v.firstName + " " + v.lastName[0] + ".</option>";
                        });
                        html += options + '</select>';

                        $('.dropdown-content').html(html);

                        return html;
                    }
                },
                {
                    "name": "assignfiles",
                    "width": "5%",
                    "bVisible": ($("#status").val() != "Deliverables") ? true : false,
                    "orderable": false,
                    "render": function (data, type, row, meta) {
                        //if (row[16] == "1") {
                        return '<input type="checkbox" name="assignfiles[]" class="assignfiles" value="' + row[0] + '" form="multiassign" data-flag="' + row[18] + '"> <span></span>';
                        //}
                        //else {
                        //    return "";
                        //}
                    },
                },
                {
                    "name": "LastCheckOut",
                    "width": "5%",
                    "orderable": false,
                    "bVisible": ($("#status").val() != "Deliverables" && $("#status").val() != "Phase 1") ? true : false,
                    render: function (data, type, row, meta) {
                        return row[15];
                    }
                },




            ],
        });

    }

    ToDataTable();

    /*var tableOld = $('#ProductionTableOLD').DataTable({
        //"sDom": 'rtlfip',  // for set pagination dd and search button to Top
        "processing": true,
        "bServerSide": true,
        "bSort": true,
        "sAjaxSource": url,
        "aLengthMenu": [
            [10, 25, 50, 100, -1],
            ['10 rows', '25 rows', '50 rows', '100 rows', 'Show All']
        ],
        "iDisplayLength": 10,
        "bLengthChange": true,
        "bDestroy": true,
        "sEmptyTable": "Loading data from server",
        "searching": true,
        "paging": true,
        //"scrollY": height,
        "scrollX": true,
        "order": [],
        "autoWidth": false,
        "responsive": true,
        //"columnDefs": [{
        //    "targets": 0,
        //    "orderable": false
        //}],
        //"aaSorting": [],// for set deafult 1st colunm sorting option to false

        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "Status", "value": $("#status").val() });
            aoData.push({ "name": "SearchBy", "value": $("#searchby").val() });
            aoData.push({ "name": "txtSearch", "value": $("#txtSearch").val() });
            $.ajax({
                "dataType": 'json',
                "type": "POST",
                "url": sSource,
                "data": aoData,
                "success": fnCallback
            });
        },
        "fnCreatedRow": function (nRow, aData, iDataIndex) {
            $(nRow).attr('id', aData[0]) // or whatever you choose to set as the id
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('#progress').hide();
            $('#checkout').hide();
            $('.showassign').hide();
            $('#last_check_in').hide();
            
            $('td:eq(-1)', nRow).hide();
            $('td:eq(-2)', nRow).hide();
            $('td:eq(-3)', nRow).hide();
            $('td:eq(-4)', nRow).hide();
            $('td:eq(-12)', nRow).hide();
            showassign = false;
            if ($("#status").val() == "Phase 1") {
                $('#progress').show();
                $('td:eq(-4)', nRow).show();
                $('td:eq(-12)', nRow).show();
                $(nRow).find('td:eq(-4)').css('padding', '2px;');
                 showassign = true;
            } 
            if ($("#status").val() != 'Deliverables') {
                $('td:eq(-12)', nRow).show();
                showassign = true;
            }
            if ($("#status").val() != 'Deliverables') {
                $('#checkout').show();
                $('td:eq(-3)', nRow).show();
                if (showassign == true) {
                    $('.showassign').show();
                    $('td:eq(-2)', nRow).show();
                } else {
                    $('#last_check_in').show();
                    $('td:eq(-1)', nRow).show();
                }
            }
            
            
            if (aData[5] <= todayDate) {
                var redDeadline = "#ffb8b8";
                $(nRow).find('td:eq(-6)').css('background-color', redDeadline);
            }
            else if (aData[5] <= forDue2Days) {

                var yellowDeadline = "#fffedb";
                $(nRow).find('td:eq(-6)').css('background-color', yellowDeadline);
            }
            else {

                $(nRow).find('td:eq(-6)').css('background-color', '#fff');
            }

        },
        "aoColumns": [
            {
                "data": "no", "sortable": false,
                "width": "5%",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                
                "name": "assignfiles",
                "width": "5%",
                "bVisible": true,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    //if (row[16] == "1") {
                    return '<input type="checkbox" name="assignfiles[]" class="assignfiles" value="' + row[0] + '" form="multiassign" data-flag="' + row[18] + '"> <span></span>';
                    //}
                    //else {
                    //    return "";
                    //}
                },
            },
            {
                
                "name": "FileID",
                "width": "5%",
                "bVisible": true,
                "render": function (data, type, row, meta) {

                    return '<a class="a-1" href="/file?ID=' + row[0] + '&flag=' + row[18]+'" target="_blank">' + row[0] + '</a>';
                    //return '<a class="a-1 tagEdit" href="javascript:;" data-id="' + row[0] + '" data-dn="' + row[17] + '">' + row[0] + '</a>';
                },
            },

            {
                
                "name": "Code",
                "width": "5%",
                "bVisible": true,
                "render": function (data, type, row, meta) {
                    return '<a class="a-1" href="/clients/edit/' + row[11] + '?flag=' + row[18] +'" target="_blank">' + row[1] + '</a>';
                    //return '<a class="a-1 clientEdit" href="javascript:;" data-id="' + row[11] + '" data-dn="' + row[17] + '">' + row[1] + '</a>';
                },
            },
            {
                
                "name": "JobID",
                "width": "5%",
                "orderable": true,
                "render": function (data, type, row, meta) {

                    return '<a href="/jobs/jobsinitial/' + row[2] + '?flag=' + row[18]+'" target="_blank" class="JobID a-1" data-id="' + row[2] +'">' + row[2] + '</a>';
                    //return '<a href="javascript:;" class="JobID a-1 jobEdit" data-id="' + row[2] + '" data-dn="' + row[17] + '">' + row[2] + '</a>';
                },
            },
            {
                
                "name": "Filename",
                "width": "25%",
                "orderable": true,
                "render": function (data, type, row, meta) {

                    return '<a href="javascript:;" class="wordbreak a-1" onclick="OpenFilePreview(' + row[19] + ',' + row[18] +'); return false;" data-flag="' + row[18]+'">' + row[3] + '</a>';
                },
            },
            {
                
                "name": "Pages",
                "width": "5%",
                "orderable": true,
                "render": function (data, type, row, meta) {

                    return row[4];
                },
            },
            {
                "name": "Deadline", "orderable": true,
                "width": "10%",
                render: function (data, type, row, meta) {
                    return row[5];
                }
            },
            {
               
                "name": "JobType",
                "width": "5%",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[6];
                }
            },
            {
               
                "name": "Progress",
                "width": "10%",
                "orderable": false,
                render: function (data, type, row, meta) {
                     finalpercent = 0;
                    if (row[4] > 0) {
                        finalpercent = (parseInt(row[7]) + parseInt(row[8])) * 42.5 / parseInt(row[4]);
                    }

                    if (row[12] == '1') {
                        finalpercent += 5;
                    }

                    if (row[13] == '1') {
                        finalpercent += 5;
                    }

                    if (row[14] == '1') {
                        finalpercent += 5;
                    }

                    return '<div class="progress position-relative" style="height: 25px;font-size:20px"><div class="progress-bar bg-success" role = "progressbar" style = "width: ' + finalpercent + '%" aria - valuenow="60" aria - valuemin="0" aria - valuemax="100" ></div><small class="justify-content-center d-flex position-absolute w-100">' + Math.round(finalpercent) + '%</small></div>';
                }
            },
            
            {
               
                "name": "CurrentCheckout",
                "width": "5%",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[9];
                }
            },
            {
                
                "name": "AssignedTo",
                "width": "10%",
                "orderable": true,
                render: function (data, type, row, meta) {

                    var options = '<option></option>';
                    '<input type="hidden" id="FileID" name="assignfiles[]" value="' + row[0] + '">';
                    var html = '<select required name="user" class="form-control dropdown-content assignedChange" file-id="' + row[0] + '" data-flag="'+row[18]+'">';

                    $.each(userlist, function (i, v) {   // get data using each loop
                        if (row[10] == v.id) {
                            selected = 'selected';
                        } else {
                            selected = '';
                        }
                        options += "<option " + selected +" value=" + v.id + ">" + v.firstName + " " + v.lastName[0] + ".</option>";
                    });
                    html += options + '</select>';
                  
                    $('.dropdown-content').html(html);
                
                    return html;
                }
            },
            {
                
                "name": "LastCheckOut",
                "width": "5%",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[15];
                }
            },
            
            


        ],
    });*/

    var table1 = $('#ProductionTable1').DataTable({
        //"sDom": 'rtlfip',  // for set pagination dd and search button to Top
        "processing": true,
        "bServerSide": true,
        "bSort": false,
        "bInfo": false,
        "sAjaxSource": url1,
        "aLengthMenu": [],
        "iDisplayLength": 10,
        "bLengthChange": false,
        "bDestroy": true,
        "sEmptyTable": "Loading data from server",
        "searching": false,
        "paging": false,
        //"scrollY": height,
        "scrollX": true,
        "order": [],
        "autoWidth": false,
        "responsive": false,
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "Status", "value": $("#status").val() });
            aoData.push({ "name": "txtSearch", "value": $("#txtSearch1").val() });
            $.ajax({
                "dataType": 'json',
                "type": "POST",
                "url": sSource,
                "data": aoData,
                "success": fnCallback
            });
        },
        "drawCallback": function (settings) {
            table1Count();
        },
        "aoColumns": [
            {
                //"title": $("#hdnActions").val(),
                "name": "FileID",
                "width": "5%",
                "bVisible": true,
                "render": function (data, type, row, meta) {

                    return '<a href="/file?ID=' + row[0] + '&flag=' + row[4]+'" target="_blank" class="current a-1">' + row[1] + '<hr class="hr-1"/> Engagement #: ' + row[2] + '<hr class="hr-1"/> Deadline : ' + row[3] + '</a>';
                },
            },
        ],
    });
   
    function table1Count() {
            var rowcount = $('#ProductionTable1 .current').length;
            $('.display-count').text(rowcount);
    }

    function reloadTable() {
        //table.ajax.reload();
        $('#ProductionTable').DataTable().destroy();
        ToDataTable();
    }

    function reloadTable1() {
        table1.ajax.reload();
    }
    $('.btnSearch').click(function (e) {

        reloadTable();
    });

    $('.btnSearch1').click(function (e) {

        reloadTable1();
    });

    var autoreload = setInterval(reloadTable, 100000);
    
    $(document).on("click", ".assignfiles", function () {
        clearInterval(autoreload);
        var len = $('input[name="assignfiles[]"]:checked').length;
        if ($(this).is(":checked")) {
            $(this).parent().find('span').show().text(len);
        } else {
            $(this).parent().find('span').hide().text("");
        }
        
        
    });
    
    $('.statusBtn').click(function (e) {
        var val = $(this).attr('rel');

        $(".statusBtn").addClass("btn-primary-1").removeClass("btn-success");
        $(this).removeClass("btn-primary-1").addClass("btn-success");
        if (val == "Deliverables") {
            $("#MultDiv").hide();
            $("#MultDiv1").hide();
        } else {
            if (val == "Phase 3") {
                $("#MultDiv1").hide();
                $("#MultDiv").show();
            } else {
                $("#MultDiv1").show();
                $("#MultDiv").hide();
            }
        }
        //var column = table.column(9);


        //column.visible(!column.visible());
        
        $('.display-name').text(val);

        $("#status").val(val);
       // table.draw();
        reloadTable();
        table1.draw();
    });

    $(document).on("change", ".assignedChange", function () {
        assignedUser = $(this).val();
      
        FileID = $(this).attr("file-id");   
        var flag = $(this).attr("data-flag");   
        $.ajax({
            type: "POST",
            url: "production/AssignUpdate",
            data: { FileID: FileID, assignedUser: assignedUser, flag: flag },
            success: function (data) {
                
               // toastr.success(data.message);
                $.notify({
                    icon: 'add_alert',
                    title: '<strong>Success!</strong>',
                    message: data.message
                }, {
                    type: 'success'
                });
            },
            error: function () {
               // toastr.error("Ajax Error found");
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

    $(document).on("change", ".assignedChangeBatch", function () {
        assignedUser = $(this).val();
        var update_success = false;

        var assignfiles = $('input:checked').map(function () {
            return $(this).val();
        }).get();
        //console.log(assignfiles);
        if (assignfiles != "") {
            
            $("input:checked").each(function () {
                var assignfiles1 = $(this).val();
                var flag = $(this).attr('data-flag');
                
                jqxhr = $.ajax({
                    type: "POST",
                    url: "production/MultiAssignUpdate",
                    data: { assignfiles: assignfiles1, assignedUser: assignedUser, flag: flag },
                    success: function (data) {
                        update_success = true;
                    },
                    error: function () {
                        //toastr.error("Ajax Error found");
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
            
            jqxhr.always(function () {
                $.notify({
                    icon: 'add_alert',
                    title: '<strong>Success!</strong>',
                    message: "Update Successfully."
                }, {
                    type: 'success'
                });
                // table.draw();
                reloadTable();

            });
                
            

        } else {
            //toastr.error("Select Multi Assign");
            $.notify({
                icon: 'add_alert',
                title: '<strong>Alert!</strong>',
                message: "Select Multi Assign"
            }, {
                type: 'danger'
            });
        }

        //var assignfiles = $('input:checked').map(function () {
        //    return $(this).val();
        //}).get();
        ////console.log(assignfiles);
        //if (assignfiles != "") {
        //    $.ajax({
        //        type: "POST",
        //        url: "production/MultiAssignUpdate",
        //        data: { assignfiles: assignfiles, assignedUser: assignedUser },
        //        success: function (data) {

        //           // toastr.success(data.message);
                    
        //            $.notify({
        //                icon: 'add_alert',
        //                title: '<strong>Success!</strong>',
        //                message: data.message
        //            }, {
        //                type: 'success'
        //            });
        //            table.draw();
        //        },
        //        error: function () {
        //            //toastr.error("Ajax Error found");
        //            $.notify({
        //                icon: 'add_alert',
        //                title: '<strong>Alert!</strong>',
        //                message: "Ajax Error found"
        //            }, {
        //                type: 'danger'
        //            });
        //        }
        //    });
        //} else {
        //    //toastr.error("Select Multi Assign");
        //    $.notify({
        //        icon: 'add_alert',
        //        title: '<strong>Alert!</strong>',
        //        message: "Select Multi Assign"
        //    }, {
        //        type: 'danger'
        //    });
        //}
    });

    $(document).on("click", "#multiassigndate", function () {
        var assignfiles = $('input:checked').map(function () {

            return $(this).val();

        }).get();
        //console.log(assignfiles);

        if (assignfiles != "") {
            // return alert("This section is under development");

            manageModel('/production/DeadlineDate');

            /*var $modal = $('#popup');
            $modal.load('/production/DeadlineDate?assignfiles=' + assignfiles, function () {
                $(".datepicker").datepicker({
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: 'yy-mm-dd'
                });
                $('.timepicker').mobiscroll().time({
                    lang: "EN",
                    theme: 'bootstrap', mode: 'scroller', timeFormat: 'HH:ii',
                    timeWheels: 'hhiiA',
                });
                $modal.modal();
            });
            $('#popup').on('shown.bs.modal', function () {
                
            })*/

        } else {
            //toastr.error("Select Multi Assign");
            $.notify({
                icon: 'add_alert',
                title: '<strong>Alert!</strong>',
                message: "Select Multi Assign"
            }, {
                type: 'danger'
            });
        }
       // reloadTable();
        
    });

    $('#assigndateform').validate({
        rules: {

            Deadline: {
                required: true
            },
            DeadlineTime: {
                required: true
            },
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

    //$(document).on("click", ".tagEdit", function () {

    //    var database = $(this).attr("data-dn");
    //    var id = $(this).attr("data-id");


    //    $.ajax({
    //        type: "POST",
    //        url: "/phases/SetContactDatabase",
    //        data: { databasename: database },
    //        dataType: "JSON",
    //        success: function (data) {

    //            window.location.href = "/file?ID=" + id;


    //        },
    //        error: function () {
    //            alert("error");
    //        }
    //    });


    //});

    //$(document).on("click", ".jobEdit", function () {

    //    var database = $(this).attr("data-dn");
    //    var id = $(this).attr("data-id");


    //    $.ajax({
    //        type: "POST",
    //        url: "/phases/SetContactDatabase",
    //        data: { databasename: database },
    //        dataType: "JSON",
    //        success: function (data) {

    //            window.location.href = "/jobs/jobsinitial/" + id;


    //        },
    //        error: function () {
    //            alert("error");
    //        }
    //    });


    //});

    //$(document).on("click", ".clientEdit", function () {

    //    var database = $(this).attr("data-dn");
    //    var id = $(this).attr("data-id");


    //    $.ajax({
    //        type: "POST",
    //        url: "/phases/SetContactDatabase",
    //        data: { databasename: database },
    //        dataType: "JSON",
    //        success: function (data) {

    //            window.location.href = "/clients/edit/" + id;


    //        },
    //        error: function () {
    //            alert("error");
    //        }
    //    });


    //});
});

/*$(document).on("click", "#saveDate", function () {
    if (!$('#assigndateform').valid()) {
        return false;
    }
    
        $.ajax({
            type: "POST",
            url: "production/DeadlineDateUpdate",
            data: $("#assigndateform").serialize(),
            success: function (data) {

                //toastr.success(data.message);
                $.notify({
                    icon: 'add_alert',
                    title: '<strong>Success!</strong>',
                    message: data.message
                }, {
                    type: 'success'
                });

                $("#ProductionTable").DataTable().ajax.reload();
                $("#bootstrap-modal").modal("hide");
            },
            error: function () {
                //toastr.error("Ajax Error found");
                $.notify({
                    icon: 'add_alert',
                    title: '<strong>Alert!</strong>',
                    message: "Ajax Error found"
                }, {
                    type: 'danger'
                });
            }
        });
    

});*/

$(document).on("click", "#saveDate", function () {
    if (!$('#assigndateform').valid()) {
        return false;
    }
    $("input:checked").each(function () {
        var assignfiles1 = $(this).val();
        var flag = $(this).attr('data-flag');
        $.ajax({
            type: "POST",
            url: "production/DeadlineDateUpdate",
            data: $("#assigndateform").serialize() + "&assignfiles=" + assignfiles1 + "&flag=" + flag,
            success: function (data) {

                //toastr.success(data.message);
                
            },
            error: function () {
                //toastr.error("Ajax Error found");
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
    $.notify({
        icon: 'add_alert',
        title: '<strong>Success!</strong>',
        message: "Update Successfully"
    }, {
        type: 'success'
    });

    $("#ProductionTable").DataTable().ajax.reload();
    $("#bootstrap-modal").modal("hide");
   
});

var manageModel = function (url) {
    loadModel(url);
}

var loadModel = function (url) {
    var assignfiles = $('input:checked').map(function () {

        return $(this).val();

    }).get();

    $.post(url, {
        assignfiles: assignfiles,
        
    }, function (response) {
            
            $("#popup").html(response);
            //$("#popup").modal();
            $('#bootstrap-modal').modal({
                show: true
            });
            $(".datepicker").datetimepicker({
                format: "YYYY-MM-DD",
                icons: {
                    time: "fa fa-clock-o",
                    date: "fa fa-calendar",
                    up: "fa fa-chevron-up",
                    down: "fa fa-chevron-down",
                    previous: "fa fa-chevron-left",
                    next: "fa fa-chevron-right",
                    today: "fa fa-screenshot",
                    clear: "fa fa-trash",
                    close: "fa fa-remove",
                },
            }),
               /* $(".timepicker").datetimepicker({
                    format: "h:mm A",
                    icons: {
                        time: "fa fa-clock-o",
                        date: "fa fa-calendar",
                        up: "fa fa-chevron-up",
                        down: "fa fa-chevron-down",
                        previous: "fa fa-chevron-left",
                        next: "fa fa-chevron-right",
                        today: "fa fa-screenshot",
                        clear: "fa fa-trash",
                        close: "fa fa-remove",
                    },
                });
            $(".datepicker").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'yy-mm-dd'
            });
            */
            $('.timepicker').mobiscroll().time({
                lang: "EN",
                theme: 'bootstrap', mode: 'scroller', timeFormat: 'HH:ii',
                timeWheels: 'hhiiA',
            });
        
        
    });
}

function unique(list) {
    var result = [];
    $.each(list, function (i, e) {
        if ($.inArray(e, result) == -1) result.push(e);
    });
    return result;
}

/*$(document).on("click", "#downloadBtn", function () {
    var assignfiles = $('input:checked').map(function () {

        return $(this).val();

    }).get();

    var Jobids = $('input:checked').map(function () {

        return $(this).closest("tr").find(".JobID").attr("data-id");

    }).get();

    Jobids = unique(Jobids);
    
    
    if (assignfiles != "") {
        if (Jobids.length == 1) {
            window.location.href = "/Production/DownloadFiles?ID=" + Jobids + "&IDS=" + assignfiles;
        } else {
            alert("You can't select different jobs files");
        }
        // return alert("This section is under development");

        

        

    } else {
        //toastr.error("Select Multi Assign");
        $.notify({
            icon: 'add_alert',
            title: '<strong>Alert!</strong>',
            message: "Select Multi Assign"
        }, {
            type: 'danger'
        });
    }

});*/
$(document).on("click", "#downloadBtn", function () {

    var assignfiles = $('input:checked').map(function () {

        return $(this).val();

    }).get();
    var flag = $('input:checked').map(function () {

        return $(this).attr('data-flag');

    }).get();
   


    if (assignfiles != "") {
        /*if (Jobids.length == 1) {
            window.location.href = "/Production/DownloadFiles?ID=" + Jobids + "&IDS=" + assignfiles;
        } else {
            alert("You can't select different jobs files");
        }*/
        // return alert("This section is under development");

        /*$("input:checked").each(function () {
            var assignfiles1 = $(this).val();
            var flag = $(this).attr('data-flag');
            var JobID = $(this).closest("tr").find(".JobID").attr("data-id");

            window.location.href = "/Production/DownloadFiles?ID=" + JobID + "&IDS=" + assignfiles1 + "&flag=" + flag;


        });*/
        window.location.href = "/Production/DownloadFilesMulti?IDS=" + assignfiles + "&flag=" + flag;




    } else {
        //toastr.error("Select Multi Assign");
        $.notify({
            icon: 'add_alert',
            title: '<strong>Alert!</strong>',
            message: "Select Multi Assign"
        }, {
            type: 'danger'
        });
    }

});

function OpenFilePreview(id, flag = 0) {
    var pdfH = window.innerHeight;
    

    $('#popupQuoteDetail').load('/Jobs/filepreview?FileID=' + id + '&pdfH=' + pdfH+'&flag='+flag, function () {
        $('.modal-dialog').css('max-width', '80%');
        
        $('#bootstrap-modal').modal({
            show: true
        });


    });
}