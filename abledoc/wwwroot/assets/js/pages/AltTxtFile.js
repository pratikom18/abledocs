$(document).ready(function () {

    $('#altTextStatusID').selectpicker('setStyle', 'btn btn-link');
    $('.filter-option').addClass('filter-option-1');

    var validator = $('#AltTextCreate').validate({
        rules: {

            altText: {
                required: true
            },
            pageNum: {
                required: true
            }

        },

        messages: { // custom messages for radio buttons and checkboxes
            altText: {
                required: "AltText is required."
            },
            pageNum: {
                required: "Page Num is required."
            }

        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        },
        submitHandler: function (form) {
            form.submit(); // form validation success, call ajax form submit
        }
    });

    $('.show-File-ALT-Text').click(function () {
        $(this).addClass('active');
        $('.File-ALT-Text').show();
        $('.show-Brandmarks').removeClass('active');
        $('.Brandmarks').hide();
    });

    $('.show-Brandmarks').click(function () {
        $(this).addClass('active');
        $('.Brandmarks').show();
        $('.show-File-ALT-Text').removeClass('active');
        $('.File-ALT-Text').hide();
    });
});
//-----------------------------------------------

//----------------------------------------------------------------
function FrequentLoadText(ID) {
    $('#altText').val($('#freqLoadID_' + ID).val())
}

function EditAltText(altID) {
    $("#altPage_" + altID).removeAttr("readonly");
    $("#altTextDetail_" + altID).removeAttr("readonly");

    $("#altEdit_" + altID).hide();
    $("#altSave_" + altID).show();
}
function SaveAltText(altID) {
    
    var altFileID = $("#altID_" + altID).val();
    var altPageNum = $("#altPage_" + altID).val();
    var altTextDetail = $("#altTextDetail_" + altID).val();
    $("#altPage_" + altID).attr("readonly", true);
    $("#altTextDetail_" + altID).attr("readonly", true);

    $("#altEdit_" + altID).show();
    $("#altSave_" + altID).hide();
    var flag =$('flag').val();
    $.ajax({
        type: "POST",
        url: "/phases/UpdateAltText",
        data: { FileID: altFileID, PageNum: altPageNum, AltText: altTextDetail, flag: flag },
        dataType: "JSON",
        success: function (data) {

        },
        error: function () {
            alert("error");
        }
    });
}

function AddAltText() {
    if (!$("#AltTextCreate").valid()) {
        return false;
    }
    var ClientID = $('#ClientID').val();
    var FileID = $('#FileID').val();
    var AltText = $('#altText').val();
    var PageNum = $('#pageNum').val();
    var saveAsFrequent = $("#saveAsFrequent").prop("checked");
    var flag = $('#flag').val();
    $.ajax({
        type: "POST",
        url: "/phases/Create",
        data: { FileID: FileID, ClientID: ClientID, AltText: AltText, PageNum: PageNum, saveAsFrequent: saveAsFrequent,flag:flag},
        dataType: "JSON",
        success: function (data) {
            window.location.href = "/alttxtfile?fileid=" + FileID+"&flag="+flag;
        },
        error: function () {
            alert("error");
        }
    });
}

function DeleteAltTxt(ID,flag) {
    var $modal = $('#popupDelete');
    $modal.load('/Phases/Delete?fileID=' + ID + '&flag='+flag, function () {
        $('#bootstrap-delete').modal({
            show: true
        });
    });
   
}

function FrameLoaded(count) {
    $("#imageIFrame_" + count).contents().find("img").attr("style", "width:200px");
}


function ReferenceFile(FileID) {
    var $modal = $('#popupSendFile');
    $modal.load('/Phases/referencefile?fileID=' + FileID, function () {
        $modal.modal();
        $('.example1').DataTable();
    });
  
}
function DoneAltText() {

    var timer = $("#stopWatch").html();
    var fileID = $('#FileID').val();
    var altTextStatus = $('#altTextStatusID').val();
    var params = fileID + "|" + altTextStatus + "|" + timer;
    var flag = $('#flag').val();
    $.ajax({
        type: "POST",
        url: "/phases/DoneAltText",
        data: { param1: params,flag:flag },
        dataType: "JSON",
        success: function (data) {
            window.location.href = "/phases/alttext";
        },
        error: function () {
            alert("error");
        }
    });
}