var oTable;

var flag = $("#flag").val();

function ToDataTable() {
    //var editval1 = $('#myedit').val();
    var url = '/jobs/FileTreeFirstLevel';
    var flag = $('#flag').val();
    oTable = $('#FirstLevel').DataTable({
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
        "scrollX": false,
        "order": [],
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
                "width": "1%", "sortable": false, "class": "treetdp10",
                render: function (data, type, row, meta) {
                    var html = '<input type="checkbox" name="assignfiles[]" class="checkParent" value=' + row[0] + ' form="multiassign"> <span></span>';
                    html += '&nbsp; <a href="javascript:;" class="file-tree img"><i class="material-icons">folder</i></a>';//;'<img src="http://i.imgur.com/SD7Dz.png">';

                    return html;
                }
            },
            {
                "title": "File Name",
                "width": "20%",
                "name": "FileName",
                "orderable": true,
                "class": "treetdp90",
                render: function (data, type, row, meta) {
                    return row[2];
                }
            }

        ],
    });
}

function fnFormatSecondDetails(table_id) {

    var sOut = "<table id=\"SecondLevel" + table_id + "\" class='SecondLevel' width='100%' style='padding-left:20PX;'>";

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
    var flag = $('#flag').val();
    $(document).on('click', '#FirstLevel tbody td .img', function () {
        var nTr = $(this).parents('tr')[0];
        var nTds = this;
        var tr = $(this).closest('tr');
        var row = oTable.row(tr);

        if (row.child.isShown()) {
            //  This row is already open - close it

            nTds.innerHTML = '<i class="material-icons">folder</i>';//"http://i.imgur.com/SD7Dz.png";
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row

            nTds.innerHTML = '<i class="material-icons">folder_open</i>';//"http://i.imgur.com/d4ICC.png";
            row.child(fnFormatSecondDetails(iTableCounter)).show();
            tr.addClass('shown');
            // try datatable stuff
            var url = '/jobs/FileTreeSecondLevel';
            oInnerTable = $('#SecondLevel' + iTableCounter).DataTable({
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
                        "width": "1%", "sortable": false, "class": "treetd10",
                        render: function (data, type, row, meta) {
                            if (row[3] == "") {
                                var html = '<input type="checkbox" name="dwdfile[]" class="checkChild" value=' + row[0] + ' form="multiassign"> <span></span>';
                                html += "&nbsp; <a href='javascript:;' class='file-tree'><i class='material-icons'>text_snippet</i></a>";
                                return html;
                            } else {
                                var html = '<input type="checkbox" name="assignfiles[]" class="checkChild" value=' + row[0] + ' form="multiassign"> <span></span>';
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
                        "class": "treetd98",
                        render: function (data, type, row, meta) {
                            if (row[3] == "") {
                                return '<a class="a-1" href="/fileget?ID=' + row[0] + '&flag='+flag+'" title="' + row[2] + '">' + row[2] + '</a>';
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
                    {
                        "width": "1%",
                        "name": "Action",
                        "orderable": false,
                        "class": "td-actions text-right treetd1",
                        render: function (data, type, row, meta) {
                            if (row[3] == "") {
                                return '<button type="button" class="btn btn-danger" data-card-widget="collapse" onclick="DeleteFileVersion(' + row[0] + ',this)"><i class="material-icons">close</i></button>';
                            } else {
                                return "";
                            }

                        }

                    }

                ],
                "fnInitComplete": function (oSettings) {
                    $('.SecondLevel thead').hide();
                }
            });

            iTableCounter = iTableCounter + 1;
        }

    });

    $(document).on('click', '.SecondLevel tbody td .img', function () {

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
        var flag = $('#flag').val();
        if (row.child.isShown()) {
            //  This row is already open - close it
            nTds.innerHTML = '<i class="material-icons">folder</i>';//"http://i.imgur.com/SD7Dz.png";
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            nTds.innerHTML = '<i class="material-icons">folder_open</i>'; "http://i.imgur.com/d4ICC.png";

            row.child(fnFormatSecondDetails(iTableCounter)).show();
            tr.addClass('shown');
            // try datatable stuff
            var url = '/jobs/FileTreeThiredLevel';
            oInnerTable = $('#SecondLevel' + iTableCounter).DataTable({
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
                        "width": "1%", "sortable": false, "class": "treetd10",
                        render: function (data, type, row, meta) {
                            if (row[5] == "0") {
                                var html = '<input type="checkbox" name="dwdfile[]" class="checkChild1" value=' + row[0] + ' form="multiassign"> <span></span>';
                                html += "&nbsp; <a href='javascript:;' class='file-tree'><i class='material-icons'>text_snippet</i></a>";
                                
                                return html;
                            } else {
                                var html = '<input type="checkbox" name="assignfiles[]" class="checkChild1" value=' + row[0] + ' form="multiassign"> <span></span>';
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
                        "class": "treetd98",
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
                                return '<a class="a-1" href="/fileget?ID=' + row[0] + '&flag='+flag+'" title="' + row[2] + '">' + row[2] + '</a>';
                            }

                        }
                    },
                    {
                        "width": "1%",
                        "name": "Action",
                        "orderable": false,
                        "class": "td-actions text-right treetd1",
                        render: function (data, type, row, meta) {
                            if (row[5] == "1") {
                                return "";
                            } else if (row[5] == "2") {
                                return "";
                            } else if (row[5] == "3") {
                                return "";
                            } else if (row[5] == "4") {
                                return "";
                            }
                            else {
                                return '<button type="button" class="btn btn-danger" data-card-widget="collapse" onclick="DeleteFileVersion(' + row[0] + ',this)"><i class="material-icons">close</i></button>';
                            }

                        }

                    }

                ],
                "fnInitComplete": function (oSettings) {
                    $('.SecondLevel thead').hide();
                }
            });

            iTableCounter = iTableCounter + 1;
        }

    });

});

$(document).ready(function () {

    //$("#checkedAll").change(function () {
    //    if (this.checked) {
    //        $(".checkParent, .checkChild").each(function () {
    //            this.checked = true;
    //        });
    //    } else {
    //        $(".checkParent, .checkChild").each(function () {
    //            this.checked = false;
    //        });
    //    }
    //});

    $(document).on('click', '.checkParent', function () {
        if ($(this).is(":checked")) {
            //var isAllChecked = 0;
            //$(".checkParent").each(function () {
            //    if (!this.checked)
            //        isAllChecked = 1;
            //})
            $(this).closest("tr").next("tr").find(".checkChild").prop("checked", true);
            $(this).closest("tr").next("tr").find(".checkChild1").prop("checked", true);
            //if (isAllChecked == 0) {
            //    $("#checkedAll").prop("checked", true);
            //}
        } else {
            //$("#checkedAll").prop("checked", false);
            $(this).closest("tr").next("tr").find(".checkChild").prop("checked", false);
            $(this).closest("tr").next("tr").find(".checkChild1").prop("checked", false);
        }
    });

    $(document).on('click', '.checkChild', function () {
        if ($(this).is(":checked")) {

            //var isChildChecked = 0;
            //$(".checkChild").each(function () {
            //    if (!this.checked)
            //        isChildChecked = 1;
            //});
            //if (isChildChecked == 0) {
            //    $("#checkedAll").prop("checked", true);
            //}
            var isAllSiblingChecked = 0;
            $(this).closest("tr").nextAll("tr").find(".checkChild").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            $(this).closest("tr").prev("tr").find(".checkChild").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            if (isAllSiblingChecked == 0) {
                $(this).closest("table").closest("tr").prev("tr").find(".checkParent").prop("checked", true);
            }
            $(this).closest("tr").next("tr").find(".checkChild1").prop("checked", true);

        } else {
            var isAllSiblingChecked = 0;
            $(this).closest("tr").nextAll("tr").find(".checkChild").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            $(this).closest("tr").prev("tr").find(".checkChild").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            if (isAllSiblingChecked == 0) {
                $(this).closest("table").closest("tr").prev("tr").find(".checkParent").prop("checked", false);
            }
            $(this).closest("tr").next("tr").find(".checkChild1").prop("checked", false);
            //$("#checkedAll").prop("checked", false);
        }
    });

    $(document).on('click', '.checkChild1', function () {
        if ($(this).is(":checked")) {

            //var isChildChecked = 0;
            //$(".checkChild").each(function () {
            //    if (!this.checked)
            //        isChildChecked = 1;
            //});
            //if (isChildChecked == 0) {
            //    $("#checkedAll").prop("checked", true);
            //}
            var isAllSiblingChecked = 0;
            $(this).closest("tr").nextAll("tr").find(".checkChild1").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            $(this).closest("tr").prev("tr").find(".checkChild1").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            if (isAllSiblingChecked == 0) {
                $(this).closest("table").closest("tr").prev("tr").find(".checkChild").prop("checked", true);
            }
            var isAllSiblingChecked1 = 0;
            $(this).closest("table").closest("tr").nextAll("tr").find(".checkChild").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked1 = 1;
            });

            $(this).closest("table").closest("tr").prevAll("tr").find(".checkChild").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked1 = 1;
            });
            if (isAllSiblingChecked1 == 0) {
                $(this).closest("table").closest("tr").prev("tr").closest("table").closest("tr").prev("tr").find(".checkParent").prop("checked", true);
            }
           

        } else {
            var isAllSiblingChecked = 0;
            $(this).closest("tr").nextAll("tr").find(".checkChild1").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            $(this).closest("tr").prev("tr").find(".checkChild1").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            if (isAllSiblingChecked == 0) {
                $(this).closest("table").closest("tr").prev("tr").closest("table").closest("tr").prev("tr").find(".checkParent").prop("checked", false);
                $(this).closest("table").closest("tr").prev("tr").find(".checkChild").prop("checked", false);
            }

            //$("#checkedAll").prop("checked", false);
        }
    });

});


function DeleteFileVersion(ID, this1) {

    swal({
        title: 'Are you sure?',
        //text: 'Some text.',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes!',
        cancelButtonText: 'No.'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/File/DeleteFileVersion",
                data: { ID: ID },
                type: 'POST'
            }).done(function (responseData) {
                console.log('Done: ', responseData);
                var row = this1;
                //row.parents('td').hide();
                // row($(this).parents('tr')).hide();
                $(row).closest("tr").remove();

            }).fail(function () {
                console.log('Failed');
            });
        } else {
            // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
        }
    });

}
