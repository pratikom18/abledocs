var counter = $("#addition_contact_counter").val();
var cloneContact = function (thisObj) {

    $(".contact-select2").select2("destroy");
    counter++;
    var parentObj = $(thisObj).parents('.cloneContact');
    var cloneObj = parentObj.clone();

    cloneObj.find('.id').attr("id", "JobDelivery_" + counter + "_id");
    cloneObj.find('.id').attr("name", "JobDelivery[" + counter + "][id]");
    cloneObj.find('.id').val("");

    cloneObj.find('.deliveryContact').attr("id", "JobDelivery_" + counter + "_ContactID");
    cloneObj.find('.deliveryContact').attr("name", "JobDelivery[" + counter + "][ContactID]");
    cloneObj.find('.deliveryContact').val("");

    

    if (counter > 0) {
        cloneObj.find(".addBtn").hide();
        cloneObj.find(".removeBtn").show();
    }
    $(".cloneContact:last").after(cloneObj);

    $(".contact-select2").select2();
    
    $(".select2-container").css("width", "auto");
    
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

$(document).on("change", "#ClientID", function () {
    clientID = $(this).val();
    JobID = $("#JobID").val();
    
    $.ajax({
        type: 'GET',
        url: "/Jobs/ClientChange",
        data: { 'ClientID': clientID, 'JobID': JobID},
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
    
    // return alert("This section is under development");
    var $modal = $('#popupQuoteDetail');
    $modal.load('/Jobs/Quote?JobID=' + JobID, function () {
       
        $('.select2').select2();
        $('.modal-dialog').css('max-width', '80%');

        $modal.modal();
    });
    $('#popupQuoteDetail').on('shown.bs.modal', function () {
        //$('#FirstName').focus();
    })
}

function InvoicePopup(JobID) {

    // return alert("This section is under development");
    var $modal = $('#popupQuoteDetail');
    $modal.load('/Jobs/Invoice?JobID=' + JobID, function () {

        $('.select2').select2();
        $('.modal-dialog').css('max-width', '80%');

        $modal.modal();
    });
    $('#popupQuoteDetail').on('shown.bs.modal', function () {
        //$('#FirstName').focus();
    })
}


$("#save_and_close").bind('click', function (event) {
    $("#currenttab").val("open");
    $("#JobForm").submit();
});

function SaveVarianceComment(jobID) {
    var message = $("#varianceComment").val();
    var params = jobID + " | " + message;
    AjaxState("SaveVarianceComment", params);
}
var JobID = $("#JobID").val();

function DeleteFileFromOtherList(ID) {
    var params = ID + " | " + JobID;
    if (confirm("Are you sure ?")) {
        AjaxState("DeleteFileFromOtherList", params);
        
    }
    //alert(ID);
}
function DeleteFileFromReferenceList(ID) {
    var params = ID + " | " + JobID;
    if (confirm("Are you sure ?")) {
        AjaxState("DeleteFileFromReferenceList", params);
    }
    //alert(ID);
}
function DeleteFileFromFileList(ID) {
    var params = ID + " | " + JobID;
    if (confirm("Are you sure ?")) {
        AjaxState("DeleteFileFromFileList", params);
    }
    //alert(ID);
}

function AjaxState(state, params) {
    $.ajax({
        url: '/Jobs/Job',
        data: { State: state, Params: params },
        type: 'POST'
    }).done(function (responseData) {
        console.log('Done: ', responseData);


        if (state == "SaveVarianceComment") {
            toastr.success("Update successfully.");
        }
        if (state == "DeleteFileVersion" || state == "DeleteFileFromOtherList" || state == "DeleteFileFromReferenceList" || state == "DeleteFileFromFileList") {
           
            $("#filesRefresh").html(responseData.html);
            
            sourceDatatable();
           
            fileTotalPageCount();
            toastr.success("File deleted successfully.");
            
            
            
        }
        

    }).fail(function () {
        console.log('Failed');
    });
}
   
sourceDatatable();
fileTotalPageCount();

    function sourceDatatable() {
        $(".filesDatatable").DataTable({
            "responsive": true,
            "autoWidth": false,
        });
        var groupColumn = 1;
        var table = $('#sourceTable').DataTable({
            "columnDefs": [
                { "visible": false, "targets": groupColumn }
            ],
            "order": [[groupColumn, 'asc']],
            "displayLength": 25,
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
    }




function fileTotalPageCount() {
    filesCountAjaxReference = $("#filesCountAjaxReference").val();
    filesCountAjaxOther = $("#filesCountAjaxOther").val();
    filesCountAjax = $("#filesCountAjax").val();
    //alert(filesCountAjax + " - " + filesCountAjaxOther + " - " + filesCountAjaxReference);
    totalFiles = parseInt(filesCountAjax) + parseInt(filesCountAjaxReference) + parseInt(filesCountAjaxOther);

    $("#fileCountSpanID").html("<span style='position: relative; top: -2px; border: 1px solid #fff; background-color: #fff; padding: 5px; font-size: 10px; border-radius: 20px; color: #000;'>" + totalFiles + "</span>");
    var dueDate = document.getElementById("dueDate").value;

    //document.getElementById("field_Deadline").value = dueDate;
    var totalPages = document.getElementById("totalPagesCount").value;
    $("#totalPagesElement").html("<span style='position: relative; top: -2px; border: 1px solid #fff; background-color: #fff; padding: 5px; font-size: 10px; border-radius: 20px; color: #000;'>" + totalPages + "</span>");
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

ChangeCSS();

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
    $('#uploadText').css("z-index", "10000");
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
        fileIDForRef = $('ul#filesListView li:nth-child(2)').attr('id');
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
        fileIDForRef = $('ul#filesListView li:nth-child(2)').attr('id');
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