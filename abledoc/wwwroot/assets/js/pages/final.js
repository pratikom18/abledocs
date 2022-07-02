$(document).ready(function () {

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
    if ($.fn.dataTable.isDataTable('#FinalTable')) {
        $('#FinalTable').DataTable().destroy();
        ToDatatable();
    }
    else {
        ToDatatable();
    }

}

function ToDatatable() {
    var editval1 = 'yes';
    var sesion_id = $('#sessionid').val();
    var todayDate = $("#currentDate").val();
    var forDue2Days = $("#forDue2Days").val();

    var url = '/Phases/Phase4List';

    var table = $('#FinalTable')
        .DataTable({
        //"sDom": 'rtlfip',  // for set pagination dd and search button to Top
        "processing": true,
        "bServerSide": true,
        "bSort": true,
        "sAjaxSource": url,
        
        "bPaginate": true,
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

        //sarju
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            
            var txtColor = "#000";
            var bgcolor = "#6bbeff";
            var altProcessWaitingColor = '#ffebe0';
            var altProcessCompletedColor = '#D1E3C1';
            if (aData[5] == 1) {
                //$('td', nRow).css('background-color', bgcolor);
                //$('td', nRow).css('color', txtColor);
                $(nRow).find('td:eq(-5)').css('background-color', bgcolor);
                
            }
            else if (aData[4] == 0 || aData[4] == 2) {
                //$('td', nRow).css('background-color', altProcessWaitingColor);
                //$('td', nRow).css('color', txtColor);
              //  var redDeadline = "#ffb8b8";
                $(nRow).find('td:eq(-5)').css('background-color', altProcessWaitingColor);

            }
            else if (aData[4] == 1 || aData[4] == 3 || aData[4] == 4 || aData[4] == 5) {
                //$('td', nRow).css('background-color', altProcessCompletedColor);
                //$('td', nRow).css('color', txtColor);
              //  var redDeadline = "#ffb8b8";
                $(nRow).find('td:eq(-5)').css('background-color', altProcessCompletedColor);
            } 

            else if (aData[4] == 6) {
                //$('td', nRow).css('background-color', '#9370DB');
                //$('td', nRow).css('color', txtColor);
                $(nRow).find('td:eq(-5)').css('background-color', '#9370DB');
            }
            else if (aData[4] == 7) {
                //$('td', nRow).css('background-color', '#3f2a6b');
                //$('td', nRow).css('color', txtColor);
                $(nRow).find('td:eq(-5)').css('background-color', '#3f2a6b');
                $(nRow).find('td:eq(-5)').css('color', '#fff');
            }
            else {

                //$('td', nRow).css('background-color', '');
                //$('td', nRow).css('color', '');
                $(nRow).find('td:eq(-5)').css('background-color', '');
            }


            if (aData[3] <= todayDate) {
                var redDeadline = "#ffb8b8";
                $(nRow).find('td:eq(-3)').css('background-color', '');
            }
            else if (aData[3] <= forDue2Days) {

                var yellowDeadline = "#fffedb";
                $(nRow).find('td:eq(-3)').css('background-color', '');
            }
            else {

                $(nRow).find('td:eq(-3)').css('background-color', '#fff');
            }

        },

      

        "aoColumns": [
            {
                "width": "20%",
                "name": "EngagementNum",
                "title":"Engagement Num#",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return '<a class="a-1" href="/jobs/jobsinitial/' + row[7] + '">' + row[2] + '</a>';
                }
            },
            {
                "width": "10%",
                "name": "JobID",
                "title": "File ID",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[0];
                }
            },
       
            {
                "width": "30%",
                "name": "FileName",
                "title": "File Name",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[1]
                }
            },
           
            {
                "width": "10%",
                "name": "Deadline",
                "title": "DeadLine",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[3]
                }
            },
            {
                "width": "20%",
                "name": "AssignedTo",
                "title": "Assigned To",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[8];
                }
            },
            {
                "width": "10%",
                "class": "text-right",
                "title": "Actions",
                "orderable": false,
                "bVisible": (editval1 != "No" ? true : false),
                "render": function (data, type, row, meta) {    
                    if (editval1 != "No") {
                        return '<a href="/file?ID=' + row[0] + '"  class="btn  btn-sm"><i class="fa fa-edit"></i> Edit</a>';

                    }
                    else {

                        return '';
                    }
                },
            },
    
        ]
    });
}
