var counter = $("#addition_contact_counter").val();
var flag = $("#flag").val();
var cloneContact = function (thisObj) {

    $(".contact-select2").select2("destroy");
   // $('.select-dropdown').selectpicker('setStyle', '');
    counter++;
    var parentObj = $(thisObj).parents('.cloneContact');
    var cloneObj = parentObj.clone();

    cloneObj.find('.id').attr("id", "JobDelivery_" + counter + "_id");
    cloneObj.find('.id').attr("name", "JobDelivery[" + counter + "][id]");
    cloneObj.find('.id').val("");

    cloneObj.find('.deliveryContact').attr("id", "JobDelivery_" + counter + "_ContactID");
    cloneObj.find('.deliveryContact').attr("name", "JobDelivery[" + counter + "][ContactID]");
    cloneObj.find('.deliveryContact').val("");

    cloneObj.find('.ContactType').attr("id", "JobDelivery_" + counter + "_ContactType");
    cloneObj.find('.ContactType').attr("name", "JobDelivery[" + counter + "][ContactType]");
    cloneObj.find('.ContactType').val("");



    if (counter > 0) {
        cloneObj.find(".addBtn").hide();
        cloneObj.find(".removeBtn").show();
    }
    $(".cloneContact:last").after(cloneObj);
    $('.select-dropdown').selectpicker('setStyle', 'btn btn-link');
    $('.filter-option').addClass('filter-option-1');
    
    $(".ContactType").select2({
        placeholder: "Contact Type",
        theme: "material"
    });
    $(".deliveryContact").select2({
        placeholder: "Contact",
        theme: "material"
    });
    $(".select2-selection__arrow")
        .addClass("material-icons")
        .html("arrow_drop_down");

   // $(".select2-container").css("width", "auto");

}
var removeContact = function (thisObj) {
    counter--;
    $(thisObj).parents('.cloneContact').remove();
}


//$('.timepicker').timepicker({
//    showPeriod: true,
//    showLeadingZero: true
//});
$(".checkbox-ui").checkboxradio();

$(".checkbox-ui-woi").checkboxradio({
    icon: false,
});

$(document).on('dp.change','#field_Deadline', function (e) {
    var deadline = e.date.format(e.date._f);
   var olddeadline = $("#oldDeadline").val();
    if (olddeadline != deadline) {
        setTimeout(function () {
            var params = deadline;
            AjaxJob('UpdateDeadlineManual', params);
        }, 1000);
    }
})

$(document).on("change", "#ClientID", function () {
    clientID = $(this).val();
    JobID = $("#JobID").val();
    
    $.ajax({
        type: 'GET',
        url: "/Jobs/ClientChange",
        data: { 'ClientID': clientID, 'JobID': JobID, 'flag': flag },
        success: function (res) {
            if (res.reload == true) {
                location.reload();
            }
            $('#' + res.divId).html(res.html);
            //$("#deliveryContact").html(res.html2);
            $('.select2').select2();

            // location.reload();

        },
        error: function (err) {
            console.log(err)
        }
    })
});

var UID = "";
var restrictionMode = "";


function RestrictionCheck() {
    if (restrictionMode == "Allowed") {
        RestrictionCheckAllowed();
    }
    else if (restrictionMode == "Restricted") {
        RestrictionCheckRestricted();
    }
}
function RestrictionCheckRestricted() {
    var countFlag = 0;
    UID = "";
    restrictionMode = "Restricted";
    $('input:checkbox.allowedUserClass').each(function () {
        var checkVal = (this.checked ? $(this).val() : "");
        $(this).parent().find("label").removeClass("restricted-active");
        $(this).parent().find("label").removeClass("allowed-active");
        if (checkVal == "on") {
            var userID = $(this).attr('id');
            var userIDSplit = userID.split("_");
            UID = UID + userIDSplit[1] + "|";
            countFlag = 1;
            $(this).parent().find("label").addClass("restricted-active");
        }
    });
    if (countFlag == 1) {
        document.getElementById("RestrictedUID").value = UID;
        document.getElementById("Mode").value = "Restricted";
    }
    else {
        document.getElementById("RestrictedUID").value = "";
        document.getElementById("Mode").value = "";
    }
}

function RestrictionCheckAllowed() {
    var countFlag = 0;
    UID = "";
    restrictionMode = "Allowed";
    var countChecked = 0;
    var totalUsersRestriction = $("#TotalUsersRestriction").val();
    $('input:checkbox.allowedUserClass').each(function () {
        var checkVal = (this.checked ? $(this).val() : "");
        $(this).parent().find("label").removeClass("restricted-active");
        $(this).parent().find("label").removeClass("allowed-active");
        if (checkVal == "") {
            var userID = $(this).attr('id');
            var userIDSplit = userID.split("_");
            UID = UID + userIDSplit[1] + "|";
            countFlag = 1;

        }
        else {
            $(this).parent().find("label").addClass("allowed-active");
            if ($(this).is(':checked')) {
                // alert($(this).attr('id'))
                // $(this).parent().find("label").addClass("allowed-active");
            }
            countChecked++;
        }
    });
    if (countFlag == 1 && countChecked > 0) {
        document.getElementById("RestrictedUID").value = UID;
        document.getElementById("Mode").value = "Allowed";
    }
    else {
        document.getElementById("RestrictedUID").value = "";
        document.getElementById("Mode").value = "";
    }
}


function RestrictedColor() {

    document.getElementById("Mode").value = "Restricted";
    $("#restrictedToggle").parent().find("label").addClass("restricted-active");

    $("#allowedToggle").parent().find("label").removeClass("allowed-active");

    $("input[type='checkbox'].allowedUserClass").checkboxradio({ theme: "n" });
    $("input[type='radio'].modeClass").checkboxradio({ theme: "n" });

    RestrictionCheckRestricted();
}
function AllowedColor() {
    $("#restrictedToggle").parent().find("label").removeClass("restricted-active");

    $("#allowedToggle").parent().find("label").addClass("allowed-active");

    $("input[type='checkbox'].allowedUserClass").checkboxradio({ theme: "o" });
    $("input[type='radio'].modeClass").checkboxradio({ theme: "o" });
    document.getElementById("Mode").value = "Allowed";

    RestrictionCheckAllowed();
}

if ($('#Mode').val() == "Allowed") {
    restrictionMode = "Allowed";
    AllowedColor();
}
if ($('#Mode').val() == "Restricted") {
    restrictionMode = "Restricted";
    RestrictedColor();
}
if ($('#Mode').val() == "") {
    restrictionMode = "Allowed";
    AllowedColor();
}


function QuotePopup(JobID) {
    $('#popupQuoteDetail').load('/Jobs/Quote?JobID=' + JobID+'&flag='+flag, function (data) {
        
        $('.modal-dialog').css('max-width', '80%');
        $('#bootstrap-modal').modal({
            show: true
        });


    });
    
    /*var $modal = $('#popupQuoteDetail');
    $modal.load('/Jobs/Quote?JobID=' + JobID, function () {

        $('.select2').select2();
        $('.modal-dialog').css('max-width', '80%');

        $modal.modal();
    });
    $('#popupQuoteDetail').on('shown.bs.modal', function () {
        
    })*/
}

function InvoicePopup(JobID) {

    // return alert("This section is under development");

    $('#popupQuoteDetail').load('/Jobs/Invoice?JobID=' + JobID + '&flag=' + flag, function () {
        $('.modal-dialog').css('max-width', '80%');
        $('#bootstrap-modal').modal({
            show: true
        });


    });

    //var $modal = $('#popupQuoteDetail');
    //$modal.load('/Jobs/Invoice?JobID=' + JobID, function () {

    //    $('.select2').select2();
    //    $('.modal-dialog').css('max-width', '80%');

    //    $modal.modal();
    //});
    //$('#popupQuoteDetail').on('shown.bs.modal', function () {
        
    //})
}

function SendToProduction() {

    var allID = "";
    var assignedTo = document.getElementById("ATHidden").value;
    if (assignedTo == "") {
        assignedTo = 0;
    }
    JobID = $("#JobID").val();
    allID = allID + JobID + "|";
    allID = allID + assignedTo + "|";
    //allID = allID + "62 ";
    $(".traverseLI").each(function (index) {
        //console.log( index + ": " + $( this ).text() );

        allID = allID + $(this).attr('id') + "|";

    });

    AjaxJob("ToProduction", allID);
}

function POUpload(JobID, Status = "otherFile", PO = 0) {
    if (filesCountAjax > 0 || Status == "sourceFile") {
        $('#popupQuoteDetail').load('/Jobs/POUpload?JobID=' + JobID + '&Status=' + Status + '&flag=' + flag, function () {
            //$('.modal-dialog').css('max-width', '30%');
            $('#bootstrap-modal').modal({
                show: true
            });
            var title = "PO Upload";
            $("#PODiv").show();
            if (Status == "sourceFile") {
                title = "Source File";
                $("#PODiv").hide();
            } else if (Status == "referenceFile") {
                title = "Reference File";
                $("#PODiv").hide();
            } else if (Status == "otherFile" && PO == 0) {
                title = "Other File";
                $("#PODiv").hide();
            }
            $(".modal-title").text(title);
            var box;
            box = document.getElementById("drop");
            box.addEventListener("dragenter", OnDragEnter, false);
            box.addEventListener("dragover", OnDragOver, false);
            box.addEventListener("drop", OnDrop, false);


        });
    } else {
        alert('Please upload at least one Source File prior to uploading Other Files');
        //$('#errorReportCheck').attr('checked',false);
    }
    
}

var selectedFiles = [];
//start reference
function OnDragEnter(e) {
    e.stopPropagation();
    e.preventDefault();

}
function OnDragOver(e) {
    e.stopPropagation();
    e.preventDefault();
    $(e.target).css('background-color', '#D3D3D3');
}
function OnDragOut(e) {
    e.stopPropagation();
    e.preventDefault();
    $(e.target).css('background-color', '');
}

function OnDrop(e) {
    e.stopPropagation();
    e.preventDefault();

    var file = e.dataTransfer.files;

    if (file.length > 1) {
        for (var i = 0; i < file.length; i++) {
            var files = file[i];
            // debugger;
            selectedFiles.push(files);
            $("#uploadText").css('background-color', '');
            $("#uploadText i").css('background-color', '');
            $("#uploadDragDrop").css('background-color', '');
            $("#uploadedFiles").append("<li class='success' style='top:0px;height:50px;color:white;background-color:#1883C6B3;background-image: -webkit-linear-gradient(top, #1883C6B3, #1883C6);'>" + files.name + "<br/>" + files.size + "</li>");


        }
    } else {
        var files = file[0];
        selectedFiles.push(files);
        $("#uploadText").css('background-color', '');
        $("#uploadText i").css('background-color', '');
        $("#uploadDragDrop").css('background-color', '');
        $("#uploadedFiles").append("<li class='success' style='top:0px;height:50px;color:white;background-color:#1883C6B3;background-image: -webkit-linear-gradient(top, #1883C6B3, #1883C6);'>" + files.name + "<br/>" + files.size + "</li>");

    }

    //if (selectedFiles1.length > 1) {
    //    for (var i = 0; i < e.dataTransfer.files; i++) {
    //        var files = e.dataTransfer.files[i];//[0]
    //        selectedFiles[files];
    //        $(e.target).css('background-color', '');
    //        $("#uploadText").css('background-color', '');
    //        $("#uploadText i").css('background-color', '');
    //        $("#uploadDragDrop").css('background-color', '');
    //        $("#uploadedFiles").append("<li class='success' style='top:0px;height:50px;color:white;background-color:#1883C6B3;background-image: -webkit-linear-gradient(top, #1883C6B3, #1883C6);'>" + files.name + "<br/>" + files.size + "</li>");

    //    }
    //} else {
    //    var files = e.dataTransfer.files;
    //    selectedFiles[files];
    //    $(e.target).css('background-color', '');
    //    $("#uploadText").css('background-color', '');
    //    $("#uploadText i").css('background-color', '');
    //    $("#uploadDragDrop").css('background-color', '');
    //    $("#uploadedFiles").append("<li class='success' style='top:0px;height:50px;color:white;background-color:#1883C6B3;background-image: -webkit-linear-gradient(top, #1883C6B3, #1883C6);'>" + files.name + "<br/>" + files.size + "</li>");

    //}

}

function OpenFilePreview(id,flag = "") {
    var pdfH = window.innerHeight;
    // return alert("This section is under development");
    /*var $modal = $('#popupQuoteDetail');
    $modal.load('/Jobs/filepreview?FileID=' + id + '&pdfH=' + pdfH, function () {

        $('.select2').select2();
        $('.modal-dialog').css('max-width', '80%');

        $modal.modal();
    });
    $('#popupQuoteDetail').on('shown.bs.modal', function () {
        //$('#FirstName').focus();
    })*/

    $('#popupQuoteDetail').load('/Jobs/filepreview?FileID=' + id + '&pdfH=' + pdfH + '&flag=' + flag, function () {
        $('.modal-dialog').css('max-width', '80%');
        /*$(".select2").select2({
            theme: "material"
        });

        $(".select2-selection__arrow")
            .addClass("material-icons")
            .html("arrow_drop_down");*/
        
        $('#bootstrap-modal').modal({
            show: true
        });


    });
}



function FileQuotation(id,flag = "") {

    var pdfH = window.innerHeight;
    
    /*var $modal = $('#popupQuoteDetail');
    $modal.load('/Jobs/FileQuoteState?FileID=' + id + '&pdfH=' + pdfH, function () {

        $('.select2').select2();
        $('.modal-dialog').css('max-width', '90%');
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

        var $modal1 = $('.pdfView');
        
    });
    $('#popupQuoteDetail').on('shown.bs.modal', function () {
        //$('#FirstName').focus();
    });*/
    
    $('#popupQuoteDetail').load('/Jobs/FileQuoteState?FileID=' + id + '&pdfH=' + pdfH + '&flag=' + flag, function () {
        $('.modal-dialog').css('max-width', '90%');
        $(".modal-select2").select2({
            theme: "material"
        });

        $(".select2-selection__arrow")
            .addClass("material-icons")
            .html("arrow_drop_down");
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
        });
        $('.timepicker').mobiscroll().time({
            lang: "EN",
            theme: 'bootstrap', mode: 'scroller', timeFormat: 'HH:ii',
            timeWheels: 'hhiiA',
        });
        $('#bootstrap-modal').modal({
            show: true
        });


    });
}


$("#save_and_close").bind('click', function (event) {
    $("#currenttab").val("open");
    $("#JobForm").submit();
});

function SaveVarianceComment(jobID) {
    var message = $("#varianceComment").val();
    var params = jobID + " | " + message;
    AjaxJob("SaveVarianceComment", params);
}
var JobID = $("#JobID").val();

function DeleteFileFromOtherList(ID) {
    var params = ID + " | " + JobID;
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
            AjaxJob("DeleteFileFromOtherList", params);
            //Swal.fire('Saved!', '', 'success')
        } else {
            // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
        }
    });
    
    //alert(ID);
}
function DeleteFileFromReferenceList(ID) {
    var params = ID + " | " + JobID;
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
            AjaxJob("DeleteFileFromReferenceList", params);
            //Swal.fire('Saved!', '', 'success')
        } else {
            // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
        }
    });
    
    //alert(ID);
}
function DeleteFileFromFileList(ID) {
    var params = ID + " | " + JobID;
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
            AjaxJob("DeleteFileFromFileList", params);
            //Swal.fire('Saved!', '', 'success')
        } else {
            // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
        }
    });
    
    //alert(ID);
}


function SavePanelData(fileID) {
    if (!$("#FilePreviewForm").valid()) {
        return false;
    }
    var params = "";
    var fieldDeadline = document.getElementById("field_DeadlineFile").value;
    var fieldDeadlineTime = document.getElementById("field_DeadlineTimeFile").value;
    var fieldPageCount = document.getElementById("field_PageCount").value;
    var fieldPrice = document.getElementById("field_Price").value;
    var e = document.getElementById("field_PricePer");
    var fieldPricePer = e.options[e.selectedIndex].text;
    var quantity = document.getElementById("field_Quantity").value;
    var old_deadline_date = $("#old_deadline_date").val();
    
    params = params + fileID + " " + fieldDeadline + " " + fieldDeadlineTime + " " + fieldPageCount + " " + fieldPrice + " " + fieldPricePer + " " + quantity + " " + old_deadline_date + " ";
    
    AjaxJob("PanelSave", params);
}

function UpdateAlt() {
    var altTextStatus = $("#altTextStatusID").val();
    var file = $("#Checkout_FileID").val();
    var params = file + "|" + altTextStatus;
    AjaxJob("UpdateAltText", params);
}

function AjaxJob(state, params) {
    $.ajax({
        url: '/Jobs/Job',
        data: { State: state, Params: params, JobID: JobID, flag: flag },
        type: 'POST'
    }).done(function (responseData) {
        //console.log('Done: ', responseData);


        if (state == "SaveVarianceComment") {
            //toastr.success("Update successfully.");
            $.notify({
                icon: 'add_alert',
                title: '<strong>Success!</strong>',
                message: "Update successfully."
            }, {
                type: 'success'
            });
        }
        if (state == "DeleteFileVersion" || state == "DeleteFileFromOtherList" || state == "DeleteFileFromReferenceList" || state == "DeleteFileFromFileList" || state == "PanelSave") {

            $("#filesRefresh").html(responseData.html);

            sourceDatatable();

            //fileTotalPageCount();
            //SourceFileUpload();

            $.getScript("/js/upload.js");

            if (state == "PanelSave") {
                //toastr.success("File Update successfully.");
                $.notify({
                    icon: 'add_alert',
                    title: '<strong>Success!</strong>',
                    message: "File Update successfully."
                }, {
                    type: 'success'
                });
                AjaxJob("UpdateJobDeadline", JobID);
               // location.reload();
            } else {
               // toastr.success("File deleted successfully.");
                $.notify({
                    icon: 'add_alert',
                    title: '<strong>Success!</strong>',
                    message: "File deleted successfully."
                }, {
                    type: 'success'
                });
            }
            



        }
        if (state == "UpdateJobDeadline") {
            location.reload();

        }
        if (state == "UpdateDeadlineManual") {
            $("#filesRefresh").html(responseData.html);

            sourceDatatable();

            //fileTotalPageCount();
            $("#oldDeadline").val(params);
           // $.getScript("/js/upload.js");
            //location.reload();

        }
        if (state == "ToProduction") {
            $.notify({
                icon: 'add_alert',
                title: '<strong>Success!</strong>',
                message: "Send to production successfully."
            }, {
                type: 'success'
            });
            
            window.open("/production", "_self");

        }

        


    }).fail(function () {
        console.log('Failed');
    });
}

$("#upl111").on('change', function (event) {
    event.preventDefault();
   

    var form_data = new FormData($('#upload')[0]);
    $.ajax({
        url: "/Jobs/upload",//$("#upload").attr('action'),
        dataType: 'JSON',
        cache: false,
        contentType: false,
        processData: false,
        data: form_data, //$(this).serialize(),                      
        type: 'post',
        success: function (response) {
            
            if (response.status == "FileUpload") {

                $("#filesRefresh").html(response.html);

                sourceDatatable();

                fileTotalPageCount();

                toastr.success("File Uploaded successfully.");




            }
        },
        
        error: function (data) {

            //toastr.warning("","There may a error on uploading. Try again later");    
            //alert("There may a error on uploading. Try again later");
            //$(".loader").hide();
            //return false;
        }
    });


    return false;


});

sourceDatatable();


function sourceDatatable() {
    //$(".filesDatatable").DataTable({
    //    "responsive": true,
    //    "autoWidth": false,
    //});
    $('#otherFileTable').DataTable({
        "lengthMenu": [
            [10, 25, 50, 100, -1],
            ['10', '25', '50', '100', 'Show All']
        ],
        "responsive": true,
        "displayLength": 10,
        "searching": false,
        //"paging": false,
        "info": false,
        "ajax": "/Jobs/OtherFilesList?JobID=" + JobID + '&flag=' + flag,
        "columns": [

            {
                "bVisible": true,
                "render": function (data, type, row, meta) {

                    return '<a class="a-1" href="javascript:;" onclick="OpenFilePreview(' + row.id + ','+flag+'); return false;" data-ajax="false">' + row.id + " - " + row.filename + '</a >';
                },
            },
            {
                "width": "10%",
                "bVisible": true,
                "render": function (data, type, row, meta) {
                    
                    if ($("#hdnCountry").val() == "True") {
                        return '<button type="button" class="btn btn-danger btn-sm" onclick="DeleteFileFromOtherList(' + row.id + '); return false;"><i class="fa fa-trash" ></i></button >';
                    } else {
                        return '<button type="button" class="btn btn-danger btn-sm" onclick="DeleteFileFromOtherList(' + row.id + '); return false;" disabled><i class="fa fa-trash" ></i></button >';

                    }
                   

                },
            }

        ],
        "footerCallback": function (row, data, start, end, display) {
            
            $("#filesCountAjaxOther").val(data.length);
            if (data.length > 0) {
                fileTotalPageCount();
            }
        }
        
    });
    $('#referenceFileTable').DataTable({
        "lengthMenu": [
            [10, 25, 50, 100, -1],
            ['10', '25', '50', '100', 'Show All']
        ],
        "responsive": true,
        "displayLength": 10,
        "searching": false,
       // "paging": false,
        "info": false,
        "ajax": "/Jobs/ReferenceFilesList?JobID=" + JobID+"&flag="+flag,
        "columns": [
            {
                "bVisible": true,
                "render": function (data, type, row, meta) {

                    return '<a class="a-1" href="javascript:;" onclick="OpenFilePreview(' + row.id + ','+flag+'); return false;" data-ajax="false">' + row.id + " - " + row.filename + '</a >';
                },
            },
            {
                "width": "10%",
                "bVisible": true,
                "render": function (data, type, row, meta) {
                    
                    if ($("#hdnCountry").val() == "True") {
                        return '<button type="button" class="btn btn-danger btn-sm" onclick="DeleteFileFromReferenceList(' + row.id + '); return false;"><i class="fa fa-trash" ></i></button >';
                    }
                    else {
                        return '<button type="button" class="btn btn-danger btn-sm" onclick="DeleteFileFromReferenceList(' + row.id + '); return false;" disabled><i class="fa fa-trash"></i></button >';
                    }
                    

                },
            }

        ],
        "footerCallback": function (row, data, start, end, display) {

            $("#filesCountAjaxReference").val(data.length);
            if (data.length > 0) {
                fileTotalPageCount();
            }
        }
       
    });
    var groupColumn = 1;
    var table = $('#sourceTable').DataTable({
        "lengthMenu": [
            [10, 25, 50, 100, -1],
            ['10', '25', '50', '100', 'Show All']
        ],
        "columnDefs": [
            { "visible": false, "targets": groupColumn }
        ],
        "responsive": true,
        "order": [[groupColumn, 'asc']],
        "displayLength": 10,
        "searching": false,
        //"paging": false,
        "info": false,
        "ajax": "/Jobs/SourceFilesList?JobID=" + JobID+"&flag="+flag,
        "columns": [
            
            {
                "bVisible": true,
                "render": function (data, type, row, meta) {

                    return '<a class="a-1 traverseLI" id="' + row.id +'" href="javascript:;" onclick="FileQuotation(' + row.id +','+flag+'); return false;" data-ajax="false">'+row.id + " - " + row.filename+'</a >';
                },
            },
            { "data": "deadline" },
            {
                "width": "10%",
                "bVisible": true,
                "render": function (data, type, row, meta) {

                    return '<button type="button" class="btn btn-danger btn-sm" onclick="DeleteFileFromFileList('+row.id+'); return false;"><i class="fa fa-trash" ></i></button >';

                },
            }

        ],
        "footerCallback": function (row, data, start, end, display) {
            $("#filesCountAjax").val(data.length);
            if (data.length > 0) {
                fileTotalPageCount();
            }
        },
        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;

            api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                if (last !== group) {
                    $(rows).eq(i).before(
                        '<tr class="group"><td colspan="5">' + group + '</td></tr>'
                    );

                    last = group;
                }
            });
        }
    });

    // Order by the grouping
    $('#sourceTable tbody').on('click', 'tr.group', function () {
        var currentOrder = table.order()[0];
        if (currentOrder[0] === groupColumn && currentOrder[1] === 'asc') {
            table.order([groupColumn, 'desc']).draw();
        }
        else {
            table.order([groupColumn, 'asc']).draw();
        }
    });
    fileTotalPageCount();
    
}




function fileTotalPageCount() {
    filesCountAjaxReference = $("#filesCountAjaxReference").val();
    filesCountAjaxOther = $("#filesCountAjaxOther").val();
    filesCountAjax = $("#filesCountAjax").val();
    if (filesCountAjax == "") {
        filesCountAjax = 0;
    }
    if (filesCountAjaxOther == "") {
        filesCountAjaxOther = 0;
    }
    if (filesCountAjaxReference == "") {
        filesCountAjaxReference = 0;
    }
   // alert(filesCountAjax + " - " + filesCountAjaxOther + " - " + filesCountAjaxReference);
    totalFiles = parseInt(filesCountAjax) + parseInt(filesCountAjaxReference) + parseInt(filesCountAjaxOther);

    $("#fileCountSpanID").html("<span style='position: relative;top:-2px;border:1px solid #1883C6;background-color: #1883C6;padding: 5px;font-size: 10px;border-radius: 20px;color: #fff;'>" + totalFiles + "</span>");
    //var dueDate = document.getElementById("dueDate").value;

    //document.getElementById("field_Deadline").value = dueDate;

    var totalPages = $("#totalPagesCount").val();//document.getElementById("totalPagesCount").value;
    if (totalPages == "") {
        totalPages = 0;
    }
    $("#totalPagesElement").html("<span style='position: relative; top: -2px; border: 1px solid #1883C6; background-color: #1883C6; padding: 5px; font-size: 10px; border-radius: 20px; color: #fff;'>" + totalPages + "</span>");
}

var greenUploadBackgroundColor = "#c5ffd4";
var blueUploadBackgroundColor = "#c1ddff";
var yellowUploadBackgroundColor = "#fffcc5";
var refreshFileFlag = 0;

h = window.innerHeight;
$("#fileQuotePanel").css("height", h);
$("#jobQuotePanel").css("height", h);
$("#jobInvoicePanel").css("height", h);
$("#jobCreditNotePanel").css("height", h);

$(".ui-panel").css("min-height", h);
$(".ui-panel").css("max-height", h);

//ChangeCSS();

$(document).ajaxComplete(function () {
    if ($("#Status").val() != "TOBEDELIVERED") {
        if ($("#uploadedFiles").children("li").hasClass("success")) {

            var count = $("#uploadedFiles").children().length;

            $('#uploadText').children().html("");
            $('#uploadText').children().css("border", "transparent");
            document.getElementById("drop").style.padding = "0px";


            if (refreshFileFlag == 1) {
                //    RefreshListSourceRef();
            }
        }
    }
});

function ChangeCSS() {
    var collapsableText = document.getElementsByClassName("downloadClassH3");
    for (var i = 0; i < collapsableText.length; i++) {
        collapsableText[i].style.borderRadius = "0px";
        var aLink = collapsableText[i].getElementsByTagName("a");
        aLink[0].style.padding = "2px";
        aLink[0].style.paddingLeft = "40px";
    }
    $('.fileLabel').css("padding", "5px");
    $('.fileLabel').css("padding-left", "40px");
    $('.fileLabel').parent().css("margin", "0px");
    $('#field_POText').parent().css("width", "50%");

    $('#downloadBox').css("overflow-y", "hidden");
    $('#uploadedFiles').css("overflow-y", "scroll");
    $('#uploadedFiles').css("height", "200px");
    $('#downloadBox').css("background-color", greenUploadBackgroundColor);
    //$('#uploadText').css("z-index", "10000");
    $('#uploadText').css("overflow", "hidden");
    $('#uploadText').css("width", "90%");
    $('#uploadText').css("left", "5%");
    //$('#uploadText').css("height","150px");

    $(":file").parent().css("border-color", "transparent");

    if ($("#Status").val() != "TOBEDELIVERED") {
        SourceFileUpload();
    }


}

function SourceFileUpload() {
    referenceFlag = 0;
    otherFlag = 0;
    $('#Add_FileID').val("");
    document.getElementById("uploadText").innerHTML = '<div style="height: auto; border: 10px #333 dashed; padding-top: 5%; padding-bottom: 5%; margin: auto; color: #000;"><br/>Drop<br/><br/><span style="font-size: 20px;">Source</span><br/><br/>Files Here</div>';
    document.getElementById("uploadFlag").value = "sourceFile";
    $('#downloadBox').css("background-color", greenUploadBackgroundColor);
    $('#sourceUploadTab').css("background-color", greenUploadBackgroundColor);
    $('#sourceUploadTab').css("color", "#000");
    $('#otherUploadTab').css("background-color", "#333");
    $('#otherUploadTab').css("color", "#fff");
    $('#referenceUploadTab').css("background-color", "#333");
    $('#referenceUploadTab').css("color", "#fff");
    $("#uploadedFiles").html("");
    //document.getElementById("downloadBox").style.backgroundColor="#fff";
    //document.getElementById("uploadText").style.backgroundColor="#00A1DE";

}

function OtherFileUpload() {
    if (filesCountAjax > 0) {
        referenceFlag = 0;
        otherFlag = 1;
        fileIDForRef = $("#fileid").val();//$('ul#filesListView li:nth-child(2)').attr('id');
        $('#Add_FileID').val(fileIDForRef);
        document.getElementById("uploadText").innerHTML = '<div style="height: auto; border: 10px #333 dashed; padding-top: 5%; padding-bottom: 5%; margin: auto; color: #000;"><br/>Drop<br/><br/><span style="font-size: 20px;">Other</span><br/><br/>Files Here</div>';
        document.getElementById("uploadFlag").value = "otherFile";
        $('#downloadBox').css("background-color", yellowUploadBackgroundColor);
        $('#otherUploadTab').css("background-color", yellowUploadBackgroundColor);
        $('#otherUploadTab').css("color", "#000");
        $('#sourceUploadTab').css("background-color", "#333");
        $('#sourceUploadTab').css("color", "#fff");
        $('#referenceUploadTab').css("background-color", "#333");
        $('#referenceUploadTab').css("color", "#fff");
        $("#uploadedFiles").html("");
        //document.getElementById("downloadBox").style.backgroundColor="#fff";
        //document.getElementById("uploadText").style.backgroundColor="#00A1DE";
    }
    else {
        alert('Please upload at least one Source File prior to uploading Other Files');

    }
}

function ReferenceFileUpload() {
    //if(c==0)
    //{
    //alert(filesCountAjax);
    if (filesCountAjax > 0) {
        referenceFlag = 1;
        otherFlag = 0;
        fileIDForRef = $("#fileid").val();//$('ul#filesListView li:nth-child(2)').attr('id');
        $('#Add_FileID').val(fileIDForRef);
        document.getElementById("uploadText").innerHTML = '<div style="height: auto; border: 10px #333 dashed; padding-top: 5%; padding-bottom: 5%; margin: auto; color: #000;"><br/>Drop<br/><br/><span style="font-size: 20px;">Reference</span><br/><br/>Files Here</div>';
        document.getElementById("uploadFlag").value = "referenceFile";
        $('#downloadBox').css("background-color", blueUploadBackgroundColor);
        $('#referenceUploadTab').css("background-color", blueUploadBackgroundColor);
        $('#referenceUploadTab').css("color", "#000");
        $('#otherUploadTab').css("background-color", "#333");
        $('#otherUploadTab').css("color", "#fff");
        $('#sourceUploadTab').css("background-color", "#333");
        $('#sourceUploadTab').css("color", "#fff");
        $("#uploadedFiles").html("");
        //document.getElementById("downloadBox").style.backgroundColor = "#ccc";
        //document.getElementById("uploadText").style.backgroundColor = "#333";
        //c++;
        //alert(document.getElementById("uploadFlag").value);
    }
    else {
        alert('Please upload at least one Source File prior to uploading Reference Files');
        //$('#errorReportCheck').attr('checked',false);
    }
    //}
    /*
    else
    {
        referenceFlag = 0;
        $('#Add_FileID').val("");
        document.getElementById("uploadText").innerHTML = "Upload Source Files";
        document.getElementById("uploadFlag").value = "sourceFile";
        document.getElementById("downloadBox").style.backgroundColor="#fff";
        document.getElementById("uploadText").style.backgroundColor="#00A1DE";
        c++;
    }

    if(c==2)
    {
        c=0;
    }
*/
}

function fileTree(ID, checkbox = 0) {
    var $modal = $('.filetree');
    var CurrentPage = $("#CurrentPage").val();
    var inEndIndex = 10;

    $modal.load('/Jobs/FileTree?id=' + ID + "&CurrentPage=" + CurrentPage + "&inEndIndex=" + inEndIndex + "&checkbox=" + checkbox + "&flag=" + flag, function () {
        $('.example1').DataTable()
    });
   
}

function fileTreePaging(ID, CurrentPage) {
    var $modal = $('.filetree');
    $("#CurrentPage").val(CurrentPage);
    var inEndIndex = 10;
    $modal.load('/Jobs/FileTree?id=' + ID + "&CurrentPage=" + CurrentPage + "&inEndIndex=" + inEndIndex + "&flag=" + flag, function () {
        $('.example1').DataTable()
    });

}
function fileTreeDataTable(ID) {
    var $modal = $('.FileTree');
   
    $modal.load('/Jobs/FileTreeDataTable?id=' + ID + "&flag=" + flag , function () {
        ToDataTable($modal);
    });

}

function fileTreeDataTableToBeDeliverd(ID) {
    var $modal = $('.FileTreeDeliverd');

    $modal.load('/Jobs/FileTreeDataTableDeliverd?id=' + ID + "&flag=" + flag, function () {
        ToDataTableDeliverd();
        ToDataTableCompliance();
    });

}

function ApproveCancelOption(jobID, clientID) {
    jobIDOldGlobalFooter = jobID;
    var params = jobID + " | " + clientID;
    var cancelledVal = $("#field_CancelOptions").val();
    
    if (cancelledVal == 0) {
        AjaxCancelJob("FilesToBeCopied", params);

    } else if (cancelledVal == 1) {
        AjaxCancelJob("JustCancelJob", jobID);

    }
}

function CopyAndCreateJob(jobID, clientID) {
    var params = jobID + " | " + clientID;
   // alert(params);
    AjaxCancelJob("CopyAndCreateJob", clientID);

}

function filecopieDatatable() {
    $('#otherFileCopy').DataTable({
        "lengthMenu": [
            [10, 25, 50, 100, -1],
            ['10', '25', '50', '100', 'Show All']
        ],
        "responsive": true,
        "order": [],

        "displayLength": -1,
        "ajax": "/Jobs/OtherFilesList?JobID=" + JobID + "&flag=" + flag,
        "columns": [

            {
                "bVisible": true,
                "render": function (data, type, row, meta) {

                    return '<label for="' + row.id +'"> <input type="checkbox" checked class="copyCheckbox" name="' + row.id + '" id="' + row.id +'"> ' + row.filename + '</label>';
                },
            },
            

        ],
        

    });
    $('#referenceFileCopy').DataTable({
        "lengthMenu": [
            [10, 25, 50, 100, -1],
            ['10', '25', '50', '100', 'Show All']
        ],
        "responsive": true,
        "order": [],
        "displayLength": -1,
        "ajax": "/Jobs/ReferenceFilesList?JobID=" + JobID + "&flag=" + flag,
        "columns": [

            {
                "bVisible": true,
                "render": function (data, type, row, meta) {

                    return '<label for="' + row.id + '"> <input type="checkbox" checked class="copyCheckbox" name="' + row.id + '" id="' + row.id + '"> ' + row.filename + '</label>';
                },
            },
            

        ],
       

    });
   
    $('#sourceCopy').DataTable({
        "lengthMenu": [
            [10, 25, 50, 100, -1],
            ['10', '25', '50', '100', 'Show All']
        ],
        "responsive": true,
        "order": [],
        "displayLength": -1,
        "ajax": "/Jobs/SourceFilesList?JobID=" + JobID + "&flag=" + flag,
        "columns": [

            {
                "bVisible": true,
                "render": function (data, type, row, meta) {

                    return '<label for="' + row.id + '"> <input type="checkbox" checked class="copyCheckbox" name="' + row.id + '" id="' + row.id + '"> ' + row.filename + '</label>';
                },
            },
            

        ],
        
       
    });

   


}

function FilesToBeCopied(JobID) {
    
    /*var $modal = $('#popupQuoteDetail');
    $modal.load("/Jobs/FilesToBeCopied?JobID=" + JobID, function () {
        
        $('.modal-dialog').css('max-width', '50%');
        filecopieDatatable();
        
        $modal.modal();
        
    });
    $('#popupQuoteDetail').on('shown.bs.modal', function () {
        $('.copyCheckbox').prop('checked', true);
    })*/
    $('#popupQuoteDetail').load("/Jobs/FilesToBeCopied?JobID=" + JobID + "&flag=" + flag, function () {
        $('.modal-dialog').css('max-width', '50%');
        filecopieDatatable();
        
        $('#bootstrap-modal').modal({
            show: true,
        });
       


    });
}
var JobID = $("#JobID").val();

function AjaxCancelJob(state, params) {
    var paramsStatic = params;
    $.ajax({
        url: '/Jobs/CancelJob',
        data: { State: state, Params: params,flag:flag },
        type: 'POST'
    }).done(function (responseData) {
       // console.log('Done: ', responseData);
        if (state == "FilesToBeCopied") {
            $('#form-modal').modal('hide');
            
            FilesToBeCopied(JobID);
        }else if (state == "CopyAndCreateJob") {
            var newJobID = responseData.jobidnew;
            var oldJobID = JobID;
            var allIDs = "";
            $(".copyCheckbox").each(function () {
                var $this = $(this);
                if ($this.is(":checked")) {
                } else {
                    allIDs += $this.attr("id") + " | ";
                }
            });
            var params = oldJobID + " | " + newJobID + " | " + allIDs;
            
           AjaxCancelJob("CopyAndCreateJobStep2", params);
        } else if (state == "CopyAndCreateJobStep2") {
            window.location.reload(true);
        } else if (state == "JustCancelJob") {
            window.location.reload(true);
        }



    }).fail(function () {
        console.log('Failed');
    });
}



$(document).on("click", "#download_all_files", function () {

    $('#popupQuoteDetail').load('/Jobs/Downloadall?JobID=' + JobID+'&flag='+flag, function () {
        $('.modal-dialog').css('max-width', '20%');
        $(".checkbox-ui").checkboxradio();

        $(".checkbox-ui-woi").checkboxradio({
            icon: false,
        });
        $('#bootstrap-modal').modal({
            show: true
        });


    });

   

});

function download() {
    var ID = parseInt($("#JobID").val());
    var allIDs = "";
    $('input[name="dwdfile[]"]').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            if (allIDs == "") {
                allIDs = $this.attr('value');
            } else {
                allIDs = allIDs + "," + $this.attr('value');
            }
        }
    });
    
    if (allIDs != "") {
        window.location.href = "/Jobs/DownloadFiles?ID=" + ID + "&IDS=" + allIDs+"&flag="+flag;
    } else {
        alert("Please select files in folders.");
    }

   

}

$(document).on("click", "#sourceBtn", function () {
    value = $(this).attr("rel");
    uploadCategory(value, $(this));
    
});
$("#sourceBtn").click();


$(document).on("click", "#othersBtn", function () {
    value = $(this).attr("rel");
    if (filesCountAjax > 0) {
        uploadCategory(value, $(this));
    }else {
        alert('Please upload at least one Source File prior to uploading Other Files');
        //$('#errorReportCheck').attr('checked',false);
    }

});

$(document).on("click", "#referenceBtn", function () {
    value = $(this).attr("rel");
    if (filesCountAjax > 0) {
        uploadCategory(value, $(this));
    }else {
        alert('Please upload at least one Source File prior to uploading Reference Files');
        //$('#errorReportCheck').attr('checked',false);
    }

});
function uploadCategory(value, $this) {

        $(".uploadCategory").addClass("btn-primary-1").removeClass("btn-outline-success");
        $this.removeClass("btn-primary-1").addClass("btn-outline-success");
        $(".source-file-div").addClass("hide");
        $(".reference-file-div").addClass("hide");
        $(".others-file-div").addClass("hide");

        if (value == "source") {
            $(".source-file-div").removeClass("hide");

        } else if (value == "reference") {
            $(".reference-file-div").removeClass("hide");
        } else if (value == "others") {
            $(".others-file-div").removeClass("hide");
        }
    
}

function DoDelivery(batchCount = 0) {
    var checkedFiles = "";
    //var email = $('#selectedEmail').val();
    var justEmail = $('#justEmail').val();
    var billingEmail = $('#billingEmail').val();
    var justName = $('#justName').val();
    var billingName = $('#billingName').val();
    var JobID = $("#JobID").val();

    var deliveryName = $("#deliveryName").val();
    var deliveryEmail = $("#deliveryEmail").val();

    var subject = $("#subjectmail").val();

    var emailContacts = "";
    //var selectedFirstName = $('#selectedFirstName').val();
    emailContacts = billingEmail + "=" + billingName + "," + justEmail + "=" + justName + "," + deliveryEmail + "=" + deliveryName;
    //alert(email);
    var params = "";
    if (justEmail == "" && billingEmail == "") {
        alert("Please select the Contact to Deliver the Information to");
        //$('#errorMessageForDeliveryLink').click();
    }
    else {
        var deliveryMessage = "";
        if (batchCount == 0) {
            deliveryMessage = $('#deliveryMessageID').val();
            /*$('input:checkbox.fileCheckbox').each(function ()
                        {
                            var checkVal = (this.checked ? $(this).val() : "");
                            //var checkVal = (this.dataCacheval ? $(this).val() : "");
                            //alert(this.dataCacheval.val());
                            if (checkVal == "on")
                            {
                                checkedFiles = checkedFiles + $(this).attr('id') + "|";
                            }
                            //var sThisVal = (this.checked ? $(this).attr('id') : "");
            });*/
            /*var checkedFiles = $('input:checkbox.fileCheckbox:checked').map(function () {

                return $(this).val();

            }).get();*/

            $('input:checkbox.fileCheckbox').each(function () {
                var checkVal = (this.checked ? $(this).val() : "");
                //var checkVal = (this.dataCacheval ? $(this).val() : "");
                //alert(this.dataCacheval.val());
                if (checkVal) {
                    checkedFiles = checkedFiles + "file_"+checkVal + "|";
                }
                //var sThisVal = (this.checked ? $(this).attr('id') : "");
            })
        }
        else {
            // this section is pending for approval
            deliveryMessage = $('#deliveryMessageID_batch_' + batchCount).val();
            $('input:checkbox.fileCheckbox_batch_' + batchCount).each(function () {
                var checkVal = (this.checked ? $(this).val() : "");
                //var checkVal = (this.dataCacheval ? $(this).val() : "");
                //alert(this.dataCacheval.val());
                if (checkVal == "on") {
                    checkedFiles = checkedFiles + $(this).attr('id') + "|";
                }
                //var sThisVal = (this.checked ? $(this).attr('id') : "");
            });
        }

        params = JobID + "|" + emailContacts + "|" + deliveryMessage + "|" + checkedFiles;
        

        $.ajax({
            url: '/Jobs/GenerateDelivery',
            data: { Params: params, JobID: JobID, flag: flag, subject: subject },
            type: 'POST'
        }).done(function (responseData) {
            //console.log('Done: ', responseData);

            $.notify({
                icon: 'add_alert',
                title: '<strong>Success!</strong>',
                message: "Update successfully."
            }, {
                type: 'success'
            });

            window.open("/Jobs/?currenttab=TOBEDELIVERED", "_self");

        }).fail(function () {
            console.log('Failed');
        });
        //AjaxState("GenerateDelivery", params);
        //alert(checkedFiles);
    }
}

$(document).on("change", ".quantity,.price,.taxDropdown", function () {
    var currency_symbol = $("#currency_symbol").val();//"$";
    var quantity = $(this).closest("tr").find(".quantity").val();
    var price = $(this).closest("tr").find(".price").val();
    fileTotalAmount = (parseFloat(quantity) * parseFloat(price));
    $(this).closest("tr").find(".fileTotalAmount").text(currency_symbol+""+fileTotalAmount.toFixed(2));
    $(this).closest("tr").find(".fileTotalAmount").attr("data-val", fileTotalAmount.toFixed(2));

    var subTotal = 0;

    $(".fileTotalAmount").each(function () {
        //var stval = parseFloat($(this).text().replace(',', ''));
        var stval = parseFloat($(this).attr("data-val").replace(',', ''));
        subTotal += isNaN(stval) ? 0 : stval;
    });
    $(".subtotal").text(currency_symbol + "" + subTotal.toFixed(2));
    $(".subtotal").attr("data-val", + subTotal.toFixed(2));
   tax = $(".taxDropdown").val();
    taxTotal = (tax * subTotal.toFixed(2)) / 100;
    
    $(".tax").text(currency_symbol+""+taxTotal.toFixed(2));
    $(".tax").attr("data-val",taxTotal.toFixed(2));
    var totalAmount = parseFloat(taxTotal.toFixed(2)) + parseFloat(subTotal.toFixed(2));
    
    $(".totalAmount").text(currency_symbol + "" + totalAmount.toFixed(2))
    $(".totalAmount").attr("data-val", totalAmount.toFixed(2))
});

$(document).ready(function () {

    $('.clientDetailshover').tooltip({
        content: function (result) {
            $.ajax({
                url: '/Jobs/clientdetails',
                type: 'post',
                async: false,
                data: { id: $("#ClientID").val(),flag:flag },
                success: function (response) {

                    result(response.details);
                }
            });
        },
        classes: {
            "ui-tooltip": "highlight"
        },
        //show: "slideDown",
        position: { my: 'left center', at: 'right+50 center' },
        open: function (event, ui) {
            ui.tooltip.hover(function () {
                $(this).fadeTo("slow", 0.5);
            });
        }
    });

    //$('.clientDetailshover').tooltip({
    //    classes: {
    //        "ui-tooltip": "highlight"
    //    },
    //    show: {duration: 500 },
    //    position: { my: 'left center', at: 'right+50 center' },
    //    content: function (result) {

    //        $.ajax({
    //            url: '/Jobs/clientdetails',
    //            type: 'post',
    //            async: false,
    //            data: { id: $("#ClientID").val() },
    //            success: function (response) {
                    
    //                result(response.details);
    //            }
    //        });
    //    }
    //});
    //$(".clientDetailshover").mouseout(function () {
    //    // re-initializing tooltip
    //   // $(this).attr('title', 'Please wait...');
    //    $(this).tooltip();
    //    $('.ui-tooltip').hide();
    //});

    $('.contactDetailshover').tooltip({
        content: function (result) {
            $.ajax({
                url: '/Jobs/contactdetails',
                type: 'post',
                async: false,
                data: { id: $("#ContactID").val(), flag: flag },
                success: function (response) {

                    result(response.details);
                }
            });
        },
        classes: {
            "ui-tooltip": "highlight"
        },
        //show: "slideDown",
        position: { my: 'left center', at: 'right+50 center' },
        open: function (event, ui) {
            ui.tooltip.hover(function () {
                $(this).fadeTo("slow", 0.5);
            });
        }
    });

    //$('.contactDetailshover').tooltip({
    //    classes: {
    //        "ui-tooltip": "highlight"
    //    },
    //    show: {duration: 500 },
    //    position: { my: 'left center', at: 'right+50 center' },
    //    content: function (result) {

    //        /*$.post('/Jobs/contactdetails', {
    //            id: $("#ContactID").val()
    //        }, function (data) {
    //            result(data.details);
    //        });*/
    //        $.ajax({
    //            url: '/Jobs/contactdetails',
    //            type: 'post',
    //            async: false,
    //            data: { id: $("#ContactID").val() },
    //            success: function (response) {

    //                result(response.details);
    //            }
    //        });
    //    }
    //});

    //$(".contactDetailshover").mouseout(function () {
    //    // re-initializing tooltip
    //  //  $(this).attr('title', 'Please wait...');
    //    $(this).tooltip();
    //    $('.ui-tooltip').hide();
    //});

}); 





