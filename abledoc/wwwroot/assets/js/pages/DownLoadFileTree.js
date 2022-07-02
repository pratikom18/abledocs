var oTable;

function ToDataTable() {
    var flag = $("#flag").val();
    var url = '/qcfiles/DownloadFileTreeFirstLevel';
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
            aoData.push({ "name": "ID", "value": $("#FileID").val() });
            aoData.push({ "name": "flag", "value": $("#flag").val() });
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
                "width": "1%", "data": "no", "sortable": false,
                render: function (data, type, row, meta) {
                    if (row[3] == "") {
                        return "<a href='javascript:;' class='file-tree'><i class='material-icons'>text_snippet</i></a>";
                    } else {
                        return '<a href="javascript:;" class="file-tree img"><i class="material-icons">folder</i></a>';//'<img src="http://i.imgur.com/SD7Dz.png">';
                    }

                }
            },
            {
                //"title": "FileName",
                "width": "20%",
                "name": "FileName",
                "orderable": false,
                render: function (data, type, row, meta) {
                    if (row[3] == "") {
                        return '<a class="a-1" href="/fileget?ID=' + row[0] + '&flag=' + flag+'" title="' + row[2] + '">' + row[2] + '</a>';
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
    var oInnerTable;
    var detailsTableHtml;
    var iTableCounter = 1;
    var flag = $('#flag').val();
    ToDataTable();

    $(document).on('click', '#FirstLevel tbody td .img', function () {
        var nTr = $(this).parents('tr')[0];
        var nTds = this;
        var tr = $(this).closest('tr');
        var row = oTable.row(tr);
        
        var tr = $(this).closest('tr');
       
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

            nTds.innerHTML = '<i class="material-icons">folder_open</i>';//"http://i.imgur.com/d4ICC.png";
            row.child(fnFormatSecondDetails(iTableCounter)).show();
            tr.addClass('shown');
            // try datatable stuff
            var url = '/qcfiles/DownloadFileTreeSecondLevel';
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
                    aoData.push({ "name": "flag", "value": $('#flag').val() });

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
                        "width": "1%", "data": "no", "sortable": false,
                        render: function (data, type, row, meta) {
                            if (row[3] == "") {
                                return "<a href='javascript:;' class='file-tree'><i class='material-icons'>text_snippet</i></a>";
                            } else {
                                return '<a href="javascript:;" class="file-tree img"><i class="material-icons">folder</i></a>';//'<img src="http://i.imgur.com/SD7Dz.png">';
                            }

                        }
                    },
                    {
                        //"title": "FileName",
                        "width": "20%",
                        "name": "FileName",
                        "orderable": false,
                        render: function (data, type, row, meta) {
                          
                            return '<a class="a-1" href="/fileget?ID=' + row[0] + '&flag=' + flag+'" title="' + row[2] + '">' + row[2] + '</a>';

                        }
                    }

                ],
            });

            iTableCounter = iTableCounter + 1;
        }

    });
});