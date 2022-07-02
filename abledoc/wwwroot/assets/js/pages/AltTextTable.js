﻿$(document).ready(function () {

    search();
    if ($('#txtSearch').val() != "") {

    }
   
    
    $('.btnSearch').click(function () {
        search();

    });

    $('#txtSearch').keypress(function (e) {
        if (e.keyCode == 13) {
            search();
        }

    });
});

function search() {
    if ($.fn.dataTable.isDataTable('#AltTextTable')) {
        $('#AltTextTable').DataTable().destroy();
        ToDatatable();
    }
    else {
        ToDatatable();
    }

}

function ToDatatable() {
   // var editval1 = 'yes';
    var editval1 = $('#myConfirm').val();
    var url = '/Phases/alttextList';

    var table = $('#AltTextTable').DataTable({
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
        "scrollX": true,
        "order": [],
        "autoWidth": false,
        "responsive": true,
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "searchstr", "value": $('#txtSearch').val() });
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

        "aoColumns": [
            {
                "width": "20%",
                "name": "EngagementNum",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[3];//'<a href="/jobs/jobsinitial/' + row[1] + '">' + row[3] + '</a>';
                }
            },
            {
                "width": "10%",
                "name": "JobID",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[0];//'<a href="/file?ID=' + row[0] + '">' + row[0] + '</a>';
                }
            },
            
            {
                "width": "30%",
                "name": "FileName",
                "orderable": true,
                render: function (data, type, row, meta) {
                    //return row[2]
                  //  return row[2];//'<a href="javascript:;" onclick="ApproveCheckout(' + row[0]+')">' + row[2] + '</a>';
                    return '<a href="javascript:;" class="wordbreak a-1" onclick="OpenFilePreview(' + row[8] + ',' + row[7] + '); return false;" data-flag="' + row[7] + '">' + row[2] + '</a>';
                }
            },
            {
                "width": "20%",
                "name": "Deadline",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[4]
                }
            },
            {
                "width": "10%",
                "name": "AssignedTo",
                "orderable": false,
                render: function (data, type, row, meta) {
                    return row[5]
                }
            },
            {
                "width": "10%",
                "bVisible": (editval1 != "No" ? true : false),
                "class": "text-right",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    return '<a href="javascript:;" onclick="ApproveCheckout(' + row[0] + ',' + row[7] + ')" class="btn  btn-sm">  ' + $("#myConfirm").val() + ' </a>'; 
                },
            },

        ]
    });
}

function ApproveCheckout(ID,flag) {
    $('#popupSendFile').load('/Phases/Confirm?fileID=' + ID+'&flag='+flag, function () {
        $('#bootstrap-modal').modal({
            show: true
        });
       
    });
   
}
function OpenFilePreview(id, flag = 0) {

    var pdfH = window.innerHeight;


    $('#popupQuoteDetail').load('/Jobs/filepreview?FileID=' + id + '&pdfH=' + pdfH + '&flag=' + flag, function () {
        $('.modal-dialog').css('max-width', '80%');

        $('#bootstrap-modal').modal({
            show: true
        });


    });
}
