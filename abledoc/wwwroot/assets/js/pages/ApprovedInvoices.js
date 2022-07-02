$(document).ready(function () {

    search();
    
    if ($('#txtSearch').val() != "") {

    }

    $('.btnSearch').click(function () {
        search();
    });

    $('#setdate').click(function () {
        ToDatatable();
    });

    $('#txtSearch').keypress(function (e) {
        if (e.keyCode == 13) {
            search();
        }

    });

});

function search() {
    if ($.fn.dataTable.isDataTable('#ApprovedInvoicesTable')) {
        $('#ApprovedInvoicesTable').DataTable().destroy();
        ToDatatable();
    }
    else {
        ToDatatable();
    }

}

function ToDatatable() {
    
    var sesion_id = $('#sessionid').val();
    var todayDate = $("#currentDate").val();
    var forDue2Days = $("#forDue2Days").val();

    var url = '/ApprovedInvoices/GetApprovedInvoicesList';

    var table = $('#ApprovedInvoicesTable').DataTable({
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
        "columnDefs": [
            { width: '20%', targets: 0 }
        ],
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "searchstr", "value": $('#txtSearch').val() });
            aoData.push({ "name": "state", "value": "InvoiceDateApprovedRange" });
            aoData.push({ "name": "startdate1", "value": $('#startdate').val() });
            aoData.push({ "name": "enddate1", "value": $('#enddate').val() });
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
        //"fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api();
            var data = data;

            var totalPreTaxAmount = 0;
            data.forEach(x => totalPreTaxAmount += parseFloat(x[8]))

            var totalTaxAmount = 0;
            data.forEach(x => totalTaxAmount += parseFloat(x[2]))

            var totalFullAmount = 0;
            data.forEach(x => totalFullAmount += parseFloat(x[5]))

            // Update footer
            $(api.column(4).footer()).html(
                '$' + totalPreTaxAmount.toFixed(2)
            );
            $(api.column(5).footer()).html(
                '$' + totalTaxAmount.toFixed(2)
            );
            $(api.column(6).footer()).html(
                '$' + totalFullAmount.toFixed(2)
            );
     

        },
           

        //},
        "aoColumns": [
            {
                "width": "5%",
                "name": "a.InvoiceID",
                "orderable": true,
                render: function (data, type, row, meta) {                   
                    return row[0]
                }
            },
            {
               "width": "30%",
                "name": "EngagementNum",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[6]
                }
            },
            {
                "width": "10%",
                "name": "LastUpdatedInv",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[7];
                }
            },
            {
                "width": "25%",
                "name": "CompanyName",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[4];
                }
            },
            {
                "width": "10%",
                "name": "Pre Tax Amount",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return "$" + parseFloat(row[8]).toFixed(2);
                }
            },
            {
                "width": "10%",
                "name": "Tax",
                "orderable": false,
                "class": "text-right",
                render: function (data, type, row, meta) {
                    return "$" +row[2]
                }
            },
            {
                "width": "10%",
                "name": "Full Amount",
                "orderable": false,
                "class":"text-right",
                render: function (data, type, row, meta) {
                    return "$" +parseFloat(row[3]).toFixed(2);
                    
                }
            } 
            //{
            //    "width": "10%",
            //    "bVisible": true,
            //    "render": function (data, type, row, meta) {
            //        return '<a href="javascript:;" id="VarianceOpen1" onClick="VarianceOpen(' + row[0] + ')"  class="btn btn-primary-1"><i class="fa fa-edit"></i> Variance</a>';
            //    }
            //}
        ]
    });
}

