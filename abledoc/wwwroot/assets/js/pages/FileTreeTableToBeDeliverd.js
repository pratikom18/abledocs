﻿var oTableDeliverd;
var flag = $("#flag").val();

function ToDataTableDeliverd() {
    var groupColumnDate = 3;
    //var editval1 = $('#myedit').val();
    var url = '/jobs/FileTreeFirstLevel';
    oTableDeliverd = $('#FirstLevelDeliverd').DataTable({
        //"sDom": 'rtlfip',  // for set pagination dd and search button to Top
        "processing": true,
        "bServerSide": true,
        "bSort": true,
        "sAjaxSource": url,
        "aLengthMenu": [
            [10, 25, 50, 100, -1],
            ['10 rows', '25 rows', '50 rows', '100 rows', 'Show All']
        ],
        "aoColumnDefs": [
            { "bVisible": false, "targets": groupColumnDate }
        ],
        "iDisplayLength": 10,
        "bLengthChange": true,
        "bDestroy": true,
        "sEmptyTable": "Loading data from server",
        "searching": true,
        "paging": true,
        //"scrollY": height,
        "scrollX": false,
        "order": [[groupColumnDate, 'desc']],
        "autoWidth": false,
        "responsive": true,
        //"columnDefs": [{
        //    "targets": 0,
        //    "orderable": false
        //}],
        //"aaSorting": [],// for set deafult 1st colunm sorting option to false

        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "ID", "value": $("#JobID").val() });
            aoData.push({ "name": "flag", "value": flag });
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

        },
        "aoColumns": [
            {
                "width": "1%", "sortable": false, "class": "treetd20",
                render: function (data, type, row, meta) {
                    var html = '<input type="checkbox" name="assignfiles[]" class="checkParentDeliverd" value=' + row[0] + ' form="multiassign"> <span></span>';
                    html += '&nbsp; <a href="javascript:;" class="file-tree img"><i class="material-icons">folder</i></a>';//;'<img src="http://i.imgur.com/SD7Dz.png">';

                    return html;
                }
            },
            {
               // "title": "FileName",
                "width": "20%",
                "name": "FileName",
                "orderable": true,
                "class": "treetd80",
                render: function (data, type, row, meta) {
                    return row[2];
                }
            },
            {
                
                "width": "20%",
                "name": "Deadline",
                "orderable": true,
                "bVisible": false, 
                "class": "treetd80",
                render: function (data, type, row, meta) {
                    return row[3];
                }
            },
            {

                "width": "20%",
                "name": "Deadline",
                "orderable": true,
                "bVisible": false,
                "class": "treetd80",
                render: function (data, type, row, meta) {
                    return row[3];
                }
            },
            

        ],
        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;

            api.column(groupColumnDate, { page: 'current' }).data().each(function (group, i) {
                if (last !== group) {
                    $(rows).eq(i).before(
                        '<tr class="group"><td colspan="2">' + group + '</td></tr>'
                    );

                    last = group;
                }
            });
        }
    });
    // Order by the grouping
    $('#FirstLevelDeliverd tbody').on('click', 'tr.group', function () {
        var currentOrder = oTableDeliverd.order()[0];
        if (currentOrder[0] === groupColumnDate && currentOrder[1] === 'asc') {
            oTableDeliverd.order([groupColumnDate, 'desc']).draw();
        }
        else {
            oTableDeliverd.order([groupColumnDate, 'asc']).draw();
        }
    });
}

function fnFormatSecondDetailsDeliverd(table_id) {

    var sOut = "<table id=\"SecondLevelDeliverd" + table_id + "\" class='SecondLevelDeliverd' width='100%' style='padding-left:20PX;'>";

    sOut += "</table>";
    return sOut;
}

$(document).ready(function () {

    //$(document).on('click', '.assignfiles', function () {

    //    if ($(this).is(':checked')) {
    //        $('.assignfiles').prop('checked', true);
    //    } else {
    //        $('.assignfiles').prop('checked', false);
    //    }

    //});

    var oInnerTable;
    var detailsTableHtml;
    var iTableCounter = 1;

    $(document).on('click', '#FirstLevelDeliverd tbody td .img', function () {
        var nTr = $(this).parents('tr')[0];
        var nTds = this;
        var tr = $(this).closest('tr');
        var row = oTableDeliverd.row(tr);

        if (row.child.isShown()) {
            //  This row is already open - close it

            nTds.innerHTML = '<i class="material-icons">folder</i>';//"http://i.imgur.com/SD7Dz.png";
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row

            nTds.innerHTML = '<i class="material-icons">folder_open</i>';//"http://i.imgur.com/d4ICC.png";
            row.child(fnFormatSecondDetailsDeliverd(iTableCounter)).show();
            tr.addClass('shown');
            // try datatable stuff
            var url = '/jobs/FileTreeSecondLevel';
            oInnerTable = $('#SecondLevelDeliverd' + iTableCounter).DataTable({
                //"sDom": 'rtlfip',  // for set pagination dd and search button to Top
                "processing": true,
                "bServerSide": true,
                "bSort": true,
                "sAjaxSource": url,
                "aLengthMenu": [
                    [10, 25, 50, 100, -1],
                    ['10 rows', '25 rows', '50 rows', '100 rows', 'Show All']
                ],
                "iDisplayLength": -1,
                "bLengthChange": false,
                "bDestroy": true,
                "sEmptyTable": "Loading data from server",
                "searching": false,
                "paging": false,
                //"scrollY": height,
                "scrollX": false,
                "order": [],
                "autoWidth": false,
                "bInfo": false,
                "responsive": true,

                "fnServerData": function (sSource, aoData, fnCallback) {
                    aoData.push({ "name": "ID", "value": nTr.id });
                    aoData.push({ "name": "ReferenceNotAllow", "value": true });
                    aoData.push({ "name": "flag", "value": flag });
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
                    $(nRow).attr('fileid', aData[1])
                    $(nRow).attr('State', aData[3])
                    $(nRow).attr('QcType', aData[5])
                },
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                },
                "aoColumns": [
                    {
                        "width": "1%", "sortable": false, "class": "treetd20",
                        render: function (data, type, row, meta) {
                            if (row[3] == "") {
                                var html = '<input type="checkbox" name="assignfiles[]" class="checkChildDeliverd" value=' + row[0] + ' form="multiassign"> <span></span>';
                                html += "&nbsp; <a href='javascript:;' class='file-tree'><i class='material-icons'>text_snippet</i></a>";
                                return html;
                            } else {
                                var html = '<input type="checkbox" name="assignfiles[]" class="checkChildDeliverd" value=' + row[0] + ' form="multiassign"> <span></span>';
                                html += '&nbsp; <a href="javascript:;" class="file-tree img"><i class="material-icons">folder</i></a>';//'<img src="http://i.imgur.com/SD7Dz.png">';
                                return html;
                            }

                        }
                    },
                    {
                        //"title": "FileName",
                        "width": "98%",
                        "name": "FileName",
                        "orderable": false,
                        "class": "treetd80",
                        render: function (data, type, row, meta) {
                            if (row[3] == "") {
                               // return '<a href="/fileget?ID=' + row[0] + '" title="' + row[2] + '">' + row[2] + '</a>';
                                return '<a href="javascript:;" title="' + row[2] + '" class="fileclick a-1">' + row[2] + '</a>';
                            } else {
                                if (row[3] == "REFERENCE") {
                                    return "Reference";
                                } else if (row[3] == "SOURCE") {
                                    return "Source";
                                } else if (row[3] == "TAGGING") {
                                    return "Phase 1";
                                } else if (row[3] == "REVIEW") {
                                    return "Phase 2";
                                } else if (row[3] == "FINAL") {
                                    return "Phase 4";
                                } else if (row[3] == "QC") {
                                    return "Phase 3";
                                }

                            }

                        }
                    },
                    

                ],
                "fnInitComplete": function (oSettings) {
                    $('.SecondLevelDeliverd thead').hide();
                }
            });

            iTableCounter = iTableCounter + 1;
        }

    });

    $(document).on('click', '.SecondLevelDeliverd tbody td .img', function () {

        var sTable = $(this).closest('tr').parents('table');
        var sTableID = sTable.attr('id');

        var nTr = $(this).parents('tr')[0];
        var nTds = this;
        var tr = $(this).closest('tr');
        var row = $('#' + sTableID).DataTable().row(tr);
        var id = nTr.id;
        var fileid = tr.attr('fileid');
        var state = tr.attr('state');
        var qctype = tr.attr('qctype');

        if (row.child.isShown()) {
            //  This row is already open - close it
            nTds.innerHTML = '<i class="material-icons">folder</i>';//"http://i.imgur.com/SD7Dz.png";
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            nTds.innerHTML = '<i class="material-icons">folder_open</i>'; "http://i.imgur.com/d4ICC.png";

            row.child(fnFormatSecondDetailsDeliverd(iTableCounter)).show();
            tr.addClass('shown');
            // try datatable stuff
            var url = '/jobs/FileTreeThiredLevel';
            oInnerTable = $('#SecondLevelDeliverd' + iTableCounter).DataTable({
                //"sDom": 'rtlfip',  // for set pagination dd and search button to Top
                "processing": true,
                "bServerSide": true,
                "bSort": true,
                "sAjaxSource": url,
                "aLengthMenu": [
                    [10, 25, 50, 100, -1],
                    ['10 rows', '25 rows', '50 rows', '100 rows', 'Show All']
                ],
                "iDisplayLength": -1,
                "bLengthChange": false,
                "bDestroy": true,
                "sEmptyTable": "Loading data from server",
                "searching": false,
                "paging": false,
                //"scrollY": height,
                "scrollX": false,
                "order": [],
                "autoWidth": false,
                "bInfo": false,
                "responsive": true,

                "fnServerData": function (sSource, aoData, fnCallback) {
                    aoData.push({ "name": "ID", "value": fileid });
                    aoData.push({ "name": "State", "value": state });
                    aoData.push({ "name": "QcType", "value": qctype });
                    aoData.push({ "name": "flag", "value": flag });
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
                    $(nRow).attr('fileid', aData[1])
                    $(nRow).attr('State', aData[3])
                    $(nRow).attr('QcType', aData[5])
                },
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                },
                "aoColumns": [
                    {
                        "width": "1%", "sortable": false, "class": "treetd20",
                        render: function (data, type, row, meta) {
                            if (row[5] == "0") {
                                var html = '<input type="checkbox" name="assignfiles[]" class="checkChildDeliverd1 fileCheckbox" value=' + row[0] + ' form="multiassign"> <span></span>';
                                html += "&nbsp; <a href='javascript:;' class='file-tree'><i class='material-icons'>text_snippet</i></a>";
                                
                                return html;
                            } else {
                                var html = '<input type="checkbox" name="assignfiles[]" class="checkChildDeliverd1 fileCheckbox" value=' + row[0] + ' form="multiassign"> <span></span>';
                                html += '&nbsp; <a href="javascript:;" class="file-tree img"><i class="material-icons">folder</i></a>';//'<img src="http://i.imgur.com/SD7Dz.png">';
                                
                                return html;
                            }

                        }
                    },
                    {
                        //"title": "FileName",
                        "width": "20%",
                        "name": "FileName",
                        "orderable": false,
                        "class": "treetd80",
                        render: function (data, type, row, meta) {
                            if (row[5] == "1") {
                                return "Working Copies";
                            } else if (row[5] == "2") {
                                return "PAC Reports";
                            } else if (row[5] == "3") {
                                return "Unsecured";
                            } else if (row[5] == "4") {
                                return "Secured";
                            }
                            else {
                                //return '<a href="/fileget?ID=' + row[0] + '" title="' + row[2] + '">' + row[2] + '</a>';
                                return '<a href="javascript:;" title="' + row[2] + '" class="fileclick1 a-1">' + row[2] + '</a>';
                            }

                        }
                    },
                    

                ],
                "fnInitComplete": function (oSettings) {
                    $('.SecondLevelDeliverd thead').hide();
                }
            });

            iTableCounter = iTableCounter + 1;
        }

    });

});

$(document).ready(function () {

    //$("#checkedAll").change(function () {
    //    if (this.checked) {
    //        $(".checkParentDeliverd, .checkChildDeliverd").each(function () {
    //            this.checked = true;
    //        });
    //    } else {
    //        $(".checkParentDeliverd, .checkChildDeliverd").each(function () {
    //            this.checked = false;
    //        });
    //    }
    //});

    $(document).on('click', '.checkParentDeliverd', function () {
        if ($(this).is(":checked")) {
            //var isAllChecked = 0;
            //$(".checkParentDeliverd").each(function () {
            //    if (!this.checked)
            //        isAllChecked = 1;
            //})
            $(this).closest("tr").next("tr").find(".checkChildDeliverd").prop("checked", true);
            $(this).closest("tr").next("tr").find(".checkChildDeliverd1").prop("checked", true);
            //if (isAllChecked == 0) {
            //    $("#checkedAll").prop("checked", true);
            //}
        } else {
            //$("#checkedAll").prop("checked", false);
            $(this).closest("tr").next("tr").find(".checkChildDeliverd").prop("checked", false);
            $(this).closest("tr").next("tr").find(".checkChildDeliverd1").prop("checked", false);
        }
    });

    $(document).on('click', '.checkChildDeliverd', function () {
        if ($(this).is(":checked")) {

            //var isChildChecked = 0;
            //$(".checkChildDeliverd").each(function () {
            //    if (!this.checked)
            //        isChildChecked = 1;
            //});
            //if (isChildChecked == 0) {
            //    $("#checkedAll").prop("checked", true);
            //}
            var isAllSiblingChecked = 0;
            $(this).closest("tr").nextAll("tr").find(".checkChildDeliverd").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            $(this).closest("tr").prev("tr").find(".checkChildDeliverd").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            if (isAllSiblingChecked == 0) {
                $(this).closest("table").closest("tr").prev("tr").find(".checkParentDeliverd").prop("checked", true);
            }
            $(this).closest("tr").next("tr").find(".checkChildDeliverd1").prop("checked", true);

        } else {
            var isAllSiblingChecked = 0;
            $(this).closest("tr").nextAll("tr").find(".checkChildDeliverd").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            $(this).closest("tr").prev("tr").find(".checkChildDeliverd").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            if (isAllSiblingChecked == 0) {
                $(this).closest("table").closest("tr").prev("tr").find(".checkParentDeliverd").prop("checked", false);
            }
            $(this).closest("tr").next("tr").find(".checkChildDeliverd1").prop("checked", false);
            //$("#checkedAll").prop("checked", false);
        }
    });

    $(document).on('click', '.checkChildDeliverd1', function () {
        if ($(this).is(":checked")) {

            //var isChildChecked = 0;
            //$(".checkChildDeliverd").each(function () {
            //    if (!this.checked)
            //        isChildChecked = 1;
            //});
            //if (isChildChecked == 0) {
            //    $("#checkedAll").prop("checked", true);
            //}
            var isAllSiblingChecked = 0;
            $(this).closest("tr").nextAll("tr").find(".checkChildDeliverd1").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            $(this).closest("tr").prev("tr").find(".checkChildDeliverd1").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            if (isAllSiblingChecked == 0) {
                $(this).closest("table").closest("tr").prev("tr").find(".checkChildDeliverd").prop("checked", true);
            }
            var isAllSiblingChecked1 = 0;
            $(this).closest("table").closest("tr").nextAll("tr").find(".checkChildDeliverd").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked1 = 1;
            });

            $(this).closest("table").closest("tr").prevAll("tr").find(".checkChildDeliverd").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked1 = 1;
            });
            if (isAllSiblingChecked1 == 0) {
                $(this).closest("table").closest("tr").prev("tr").closest("table").closest("tr").prev("tr").find(".checkParentDeliverd").prop("checked", true);
            }
           

        } else {
            var isAllSiblingChecked = 0;
            $(this).closest("tr").nextAll("tr").find(".checkChildDeliverd1").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            $(this).closest("tr").prev("tr").find(".checkChildDeliverd1").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            if (isAllSiblingChecked == 0) {
                $(this).closest("table").closest("tr").prev("tr").closest("table").closest("tr").prev("tr").find(".checkParentDeliverd").prop("checked", false);
                $(this).closest("table").closest("tr").prev("tr").find(".checkChildDeliverd").prop("checked", false);
            }

            //$("#checkedAll").prop("checked", false);
        }
    });
    $(document).on('click', '.fileclick', function () {
        $(this).closest("tr").find(".checkChildDeliverd").click();
    });

    $(document).on('click', '.fileclick1', function () {
        $(this).closest("tr").find(".checkChildDeliverd1").click();
    });

});
