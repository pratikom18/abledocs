var _alphabetSearch = '';

$.fn.dataTable.ext.search.push(function (settings, searchData) {
    if (!_alphabetSearch) {
        return true;
    }

    if (searchData[0].charAt(0) === _alphabetSearch) {
        return true;
    }

    return false;
});
$(document).ready(function () {

    var editval1 = $('#myedit').val();
    var deleteval1 = $('#mydelete').val();
    var loginuserDatabase = $('#loginuserDatabase').val();

    var url = '/contacts/ContactList';

    var table = $('#contactsTable').DataTable({

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
        "order": [[2, "asc"]],
        "autoWidth": false,
        "columnDefs": [{
            "targets": 0,
            "orderable": false
        }],
        //"aaSorting": [],// for set deafult 1st colunm sorting option to false
        responsive: true,
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "AlphaSearch", "value": $("#AlphaSearch").val() });
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
                "data": "id", "sortable": false,
                "width": "10%",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                "width": "10%",
                "name": "Code",
                "bVisible": true,
                render: function (data, type, row, meta) {
                    if (row[7] != "") {
                        Code = row[7] + "-" + row[8] + "-" + row[1];
                    } else {
                        Code = row[1];
                    }

                    return Code;
                }

            },
            {
                "width": "10%",
                "name": "Company",
                "orderable": true,


            },
            {
                "width": "10%",
                "name": "FirstName",
                "orderable": true,

            },

            {
                "width": "20%",
                "name": "Email",
                "orderable": true
            },
            {
                "width": "10%",
                "name": "City",
                "orderable": true
            },
            {
                "width": "10%",
                "name": "Country",
                "orderable": true
            },
            {
                "width": "20%",
                "bVisible": (editval1 != "No" ? true : false),
                "orderable": false,
                "class": "text-right",
                "render": function (data, type, row, meta) {
                    var editButton = "";
                    var deleteButton = "";
                    if (editval1 != "No") {
                       editButton = '<a href="/contacts/edit/' + row[0] + '?flag='+row[10]+'"  class="btn  btn-sm"><i class="fa fa-edit"></i>'+$("#hdnEdit").val()+'</a>';
                        //editButton = '<button type="button" data-id="' + row[0] + '" data-database="' + row[9] + '" class="btn  btn-sm contactEdit"><i class="fa fa-edit"></i> Edit</button>';
                     
                    }
                    if (deleteval1 != "No") {
                        
                        deleteButton = '<button type="button" data-id="' + row[0] + '" data-database="' + row[9] + '" class="btn btn-danger  btn-sm contactDelete" data-flag="' + row[10] + '"><i class="fa fa-trash"></i> ' + $("#hdnDelete").val() +'</button>';

                    }

                    var ActionButton = editButton + " " + deleteButton;
                    return ActionButton;

                    // return '<a href="/clients/edit/' + row[0] + '"  class="btn btn-info btn-sm" style="visibility: hidden"><i class="fa fa-edit"></i>Edit</a>';
                    //return '<a href="javascript:;" onClick="developAlert()" class="btn btn-info btn-sm"><i class="fa fa-edit"></i>Edit</a>';

                },
            }



        ],
    });


    $(document).on("click", ".contactDelete", function () {

        var database = $(this).attr("data-database");
        var id = $(this).attr("data-id");
        var flag = $(this).attr("data-flag");
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
                    type: "POST",
                    url: "/contacts/DeleteContact",
                    data: { id:id,flag: flag },
                    dataType: "JSON",
                    success: function (data) {
                        
                        if (data.result == true) {
                            table.ajax.reload();
                            Swal.fire(data.msg, '', 'success')
                        } else {
                            Swal.fire(data.msg, '', 'error')
                        }


                    },
                    error: function () {
                        alert("error");
                    }
                });
                
            } else {
                // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
            }
        });


        


    });

    $('.tooltips').tooltip();

    $(".alphabet").on('click', 'span', function () {
        $(".alphabet").find('.active').removeClass('active');
        $(this).addClass('active');



        var AlphaSearch = $(this).html();
        if ($(this).html() == "All") {
            AlphaSearch = "";
        }

        $("#AlphaSearch").val(AlphaSearch);
        table.draw();
    });




    
    //$("#Telephone").inputmask({ "mask": $("#phone_format").val() });
    //$("#Cell").inputmask({ "mask": $("#phone_format").val() });
    


});

function developAlert() {
    return alert("This section is under development");
}