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
    if ($.fn.dataTable.isDataTable('#QuoteTable')) {
        $('#QuoteTable').DataTable().destroy();
        ToDatatable();
    }
    else {
        ToDatatable();
    }

}

function ToDatatable() {
    var url = '/Quotes/GetQuotesList';

    var table = $('#QuoteTable').DataTable({
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
                "name": "EngagementNum",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return '<a href="javascript:;" onclick="VarianceOpen(' + row[5] + ', ' + row[0] + ',' + row[7]+')">' + row[1] + '</a>';
                }
            },
            {
                "name": "JobID",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return '<a href="javascript:;" onclick="QuoteButtonClicked(' + row[0] + ',' + row[7] +');">' + row[0] + '</a>';
                  //  return row[0]
                }
            },

            {
                "name": "clients_contacts.FirstName,clients_contacts.LastName",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[2] + " " + row[3]
                }
            },
            {
                "name": "Quote Date",
                "orderable": true,
                render: function (data, type, row, meta) {
                    return row[4]
                }
            }
        ]
    });
}

function VarianceOpen(QTID,JobID,flag) {
    // return alert("This section is under development");
    var $modal = $('#popupQuoteDetail');
    $modal.load('/Jobs/Quote?JobID=' + JobID + "&QTID=" + QTID+"&flag="+flag, function () {
        $('#bootstrap-modal').modal({
            show: true
        });
        $(".description").select2({
            placeholder: "Select",
            theme: "material"
        });

        $(".select2-selection__arrow")
            .addClass("material-icons")
            .html("arrow_drop_down");
        $('#addPricePerUnit option:selected').text();
        $('#defaultAddPricePerUnit').selectpicker('setStyle', 'btn btn-link');
        //$('#addPricePerUnit').selectpicker('setStyle', 'btn btn-link');
        //$('.pricePerSelect').selectpicker('setStyle', 'btn btn-link');
        
        $('.filter-option').addClass('filter-option-1');
        $('.modal-dialog').css('max-width', '80%');
        $('.quotelist').addClass('col-md-6');
        
        if (flag == false) {
            $('.quotelist').attr('aria-current', 'page');
        }
       
    });
   
}

function QuoteButtonClicked(JobID, flag) {
    var $modal = $('#popupQuoteDetail');
    $modal.load('/Jobs/Quote?JobID=' + JobID+'&flag='+flag , function () {
        $('#bootstrap-modal').modal({
            show: true
        });
        $(".description").select2({
            placeholder: "Select",
            theme: "material"
        });

        $(".select2-selection__arrow")
            .addClass("material-icons")
            .html("arrow_drop_down");
        $('#addPricePerUnit option:selected').text();
        $('#defaultAddPricePerUnit').selectpicker('setStyle', 'btn btn-link');
        //$('#addPricePerUnit').selectpicker('setStyle', 'btn btn-link');
        //$('.pricePerSelect').selectpicker('setStyle', 'btn btn-link');

        $('.filter-option').addClass('filter-option-1');
        $('.modal-dialog').css('max-width', '80%');
        //$('.quotelist').addClass('col-md-6');
        if (flag == false) {
            $('.quotelist').attr('aria-current', 'page');
        }
    });
   
}
