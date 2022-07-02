var oTableCompliance;
var flag = $("#flag").val();


function ToDataTableCompliance() {
    

    var url = '/jobs/FileTreeComplianceReportFirstLevel';
    oTableCompliance = $('#FirstLevelReportTree').DataTable({
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
                        var html = '<input type="checkbox" name="assignfiles[]" class="checkParentComplianceReport" value=' + row[0] + ' form="multiassign"> <span></span>';
                        html += "&nbsp; <a href='javascript:;' class='file-tree'><i class='material-icons'>text_snippet</i></a>";

                        return html;
                    } else {
                        var html = '<input type="checkbox" name="assignfiles[]" class="checkParentComplianceReport" value=' + row[0] + ' form="multiassign"> <span></span>';
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
                    

                }
            },


        ],
        fnInitComplete: function () {
            if ($(this).find('tbody tr').length <= 1) {
                $(this).parent().hide();
            }
        } 
        
    });

    
}

function fnFormatSecondReportTreeDetails(table_id) {

    var sOut = "<table id=\"SecondLevelReportTree" + table_id + "\" class='SecondLevelReportTree' width='100%' style='padding-left:20PX;'>";

    sOut += "</table>";
    return sOut;
}


$(document).ready(function () {

    var oInnerTable;
    var detailsTableHtml;
    var iTableCounter = 1;



    $(document).on('click', '#FirstLevelReportTree tbody td .img', function () {

        var sTable = $(this).closest('tr').parents('table');
        var sTableID = sTable.attr('id');

        var nTr = $(this).parents('tr')[0];
        var nTds = this;
        var tr = $(this).closest('tr');
        var row = $('#' + sTableID).DataTable().row(tr);
        var id = nTr.id;
        var fileid = $("#JobID").val();//tr.attr('fileid');
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

            row.child(fnFormatSecondReportTreeDetails(iTableCounter)).show();
            tr.addClass('shown');
            // try datatable stuff
            var url = '/jobs/FileTreeComplianceReportSecondLevel';
            oInnerTable = $('#SecondLevelReportTree' + iTableCounter).DataTable({
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

                            var html = '<input type="checkbox" name="dwdfile[]" class="checkChildComplianceReport fileCheckbox" value=' + row[0] + ' form="multiassign"> <span></span>';
                            html += "&nbsp; <a href='javascript:;' class='file-tree'><i class='material-icons'>text_snippet</i></a>";

                            return html;



                        }
                    },
                    {
                        //"title": "FileName",
                        "width": "20%",
                        "name": "FileName",
                        "orderable": false,
                        "class": "treetd80",
                        render: function (data, type, row, meta) {

                            return '<a href="javascript:;" class="fileclickComplianceReport a-1">' + row[2] + '</a>';


                        }
                    },


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

   

    $(document).on('click', '.checkParentComplianceReport', function () {
        if ($(this).is(":checked")) {
            //var isAllChecked = 0;
            //$(".checkParentComplianceReport").each(function () {
            //    if (!this.checked)
            //        isAllChecked = 1;
            //})
            $(this).closest("tr").next("tr").find(".checkChildComplianceReport").prop("checked", true);
            //if (isAllChecked == 0) {
            //    $("#checkedAll").prop("checked", true);
            //}
        } else {
            //$("#checkedAll").prop("checked", false);
            $(this).closest("tr").next("tr").find(".checkChildComplianceReport").prop("checked", false);
            
        }
    });

    $(document).on('click', '.checkChildComplianceReport', function () {
        if ($(this).is(":checked")) {

            //var isChildChecked = 0;
            //$(".checkChildComplianceReport").each(function () {
            //    if (!this.checked)
            //        isChildChecked = 1;
            //});
            //if (isChildChecked == 0) {
            //    $("#checkedAll").prop("checked", true);
            //}
            var isAllSiblingChecked = 0;
            $(this).closest("tr").nextAll("tr").find(".checkChildComplianceReport").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            $(this).closest("tr").prev("tr").find(".checkChildComplianceReport").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            if (isAllSiblingChecked == 0) {
                $(this).closest("table").closest("tr").prev("tr").find(".checkParentComplianceReport").prop("checked", true);
            }
            

        } else {
            var isAllSiblingChecked = 0;
            $(this).closest("tr").nextAll("tr").find(".checkChildComplianceReport").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            $(this).closest("tr").prev("tr").find(".checkChildComplianceReport").each(function () {
                if (!$(this).is(":checked"))
                    isAllSiblingChecked = 1;
            });

            if (isAllSiblingChecked == 0) {
                $(this).closest("table").closest("tr").prev("tr").find(".checkParentComplianceReport").prop("checked", false);
            }
            
            //$("#checkedAll").prop("checked", false);
        }
    });

    
    $(document).on('click', '.fileclickComplianceReport', function () {
        $(this).closest("tr").find(".checkChildComplianceReport").click();
    });

   

});