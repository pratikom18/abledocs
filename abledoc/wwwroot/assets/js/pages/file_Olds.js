$(".checkbox-ui").checkboxradio();

$(".checkbox-ui-woi").checkboxradio({
    icon: false,
});


$(document).ready(function () {
    $('.Checkout').click(function () {
        
        var $row = $(this).attr("data-link");
        var FileID = parseInt($("#ID").val());
        var Checkout_PageNumber = parseInt($("#jobsFiles_CurrentPage").val());
        var State = $('#Status').val();
        $.ajax({
            type: "POST",
            url: "/File/InsertCheckout",
            data: { FileID: FileID, Checkout_PageNumber: Checkout_PageNumber, State: State},
            dataType: "JSON",
            success: function (data) {
                
                if ($row == "Checkout") {
                    window.location.href = "/tag?ID=" + FileID;
                }
                else if ($row == "CheckoutToReview") {
                    window.location.href = "/review_files?ID=" + FileID;
                }
                else if ($row == "CheckoutToFinal") {
                    window.location.href = "/final_files?ID=" + FileID;
                }
                else if ($row == "CheckoutToQC") {
                    window.location.href = "/qcfiles?ID=" + FileID;
                }
 

            },
            error: function () {
                alert("error");
            }
        });
    })

    $('.SendTo').click(function () {
        var FileID = parseInt($("#ID").val());
        var $modal = $('#popupSendFile');
        $modal.load('/File/SendToFile?id=' + FileID, function () {
            $modal.modal();
        });
        $('#popupSendFile').on('shown.bs.modal', function () {
            $('#Status').select2();
            $('#SendTo').select2();
        })
    });

    $('.LinkPopupFile').click(function () {
        var JobID = parseInt($("#JobID").val());
        var FileID = parseInt($("#ID").val());
        var $modal = $('#popupSendFile');
        $modal.load('/File/LinkPopupFile?jobID=' + JobID + '&fileID=' + FileID, function () {
            $modal.modal();
        });
        $('#popupSendFile').on('shown.bs.modal', function () {
          
        })
    });

    //$('.btnConversation').click(function () {


    //    var FileID = parseInt($("#ID").val());
    //    var JobID = parseInt($("#JobID").val());
    //    var Conversation = $('#Conversation').val();
    //    $.ajax({
    //        type: "POST",
    //        url: "/File/InsertConversions",
    //        data: { FileID: FileID, JobID: JobID, Conversation: Conversation },
    //        dataType: "JSON",
    //        success: function (data) {
    //            window.location.href = "/file?ID=" + FileID;

    //        },
    //        error: function () {
    //            alert("error");
    //        }
    //    });
    //});

   
});

function AddFiles(type) {
    var FileID = parseInt($("#ID").val());
    var $modal = $('#popupSendFile');
    $modal.load('/File/DragDropFile?id=' + FileID + "&type=" + type, function () {
        $modal.modal();
        var box;
        box = document.getElementById("drop");
        box.addEventListener("dragenter", OnDragEnter, false);
        box.addEventListener("dragover", OnDragOver, false);
        box.addEventListener("drop", OnDrop, false);
        
        if (type == "REFERENCE") {
            $('.type').hide();
        }
        $('#Type').select2();
    });
    $('#popupSendFile').on('shown.bs.modal', function () {
        
    })
}

function DeleteFileVersion(ID) {
    
    $.ajax({
        type: "POST",
        url: "/File/DeleteFileVersion",
        data: { ID: ID},
        dataType: "JSON",
        success: function (data) {
            window.location.href = "/file?ID=" + FileID;

        },
        error: function () {
            alert("error");
        }
    });
}

function UpdateTagging(JobID) {
    var TaggingInstructions = $('#Tagging_Instructions').val();
    $.ajax({
        type: "POST",
        url: "/File/UpdateTagging",
        data: { JobID: JobID, TaggingInstructions: TaggingInstructions },
        dataType: "JSON",
        success: function (data) {
            window.location.href = "/file?ID=" + FileID;

        },
        error: function () {
            alert("error");
        }
    });
}


function fileTree(ID) {
    var $modal = $('.filetree');
    var CurrentPage = $("#CurrentPage").val();
    var inEndIndex = 10;
    $modal.load('/Jobs/FileTree?id=' + ID + "&CurrentPage=" + CurrentPage + "&inEndIndex=" + inEndIndex, function () {
        $('.example1').DataTable()
    });

}

function fileTreePaging(ID, CurrentPage) {
    var $modal = $('.filetree');
    $("#CurrentPage").val(CurrentPage);
    var inEndIndex = 10;
    $modal.load('/Jobs/FileTree?id=' + ID + "&CurrentPage=" + CurrentPage + "&inEndIndex=" + inEndIndex, function () {
        $('.example1').DataTable()
    });

}
var selectedFiles = [];

$(document).ready(function () {
   $("#uploads").click(function () {
        
        var data = new FormData();
        for (var i = 0; i < selectedFiles.length; i++) {
            if (selectedFiles[i].length == 1) {
                var files = selectedFiles[i][0]
                data.append(files.name, files);
            } else if (selectedFiles[i].length > 1) {
                var files = selectedFiles[i]
                for (var j = 0; j < files.length; j++) {
                    data.append(files[j].name, files[j]);
                }
            }

        }
        $.ajax({
            type: "POST",
            url: '@Url.Action("ProcessRequest","File")',
            contentType: false,
            processData: false,
            data: data,
            success: function (result) {
                alert(result);
            },
            error: function () {
                alert("There was error uploading files!");
            }
        });
    });
});
function OnDragEnter(e) {
    e.stopPropagation();
    e.preventDefault();
}

function OnDragOver(e) {
    e.stopPropagation();
    e.preventDefault();
}

function OnDrop(e) {
    e.stopPropagation();
    e.preventDefault();
    //selectedFiles = e.dataTransfer.files;
    selectedFiles.push(e.dataTransfer.files);

    $("#uploadedFiles").append("<li class='success' style='top:0px;height:75px;color:white;background-color:#6DA81E;background-image: -webkit-linear-gradient(top, #92D400, #6DA81E);'>" + e.dataTransfer.files[0].name + "<br/>" + e.dataTransfer.files[0].size +"</li>");

    //var greenUploadBackgroundColor = "#c5ffd4";
    //$('#uploadedFiles').css("overflow-y", "scroll");
    //$('#uploadedFiles').css("height", "200px");
    //$('#downloadBox').css("background-color", greenUploadBackgroundColor);
    ////$('#uploadText').css("z-index", "10000");
    //$('#uploadText').css("overflow", "hidden");
    //$('#uploadText').css("width", "90%");
    //$('#uploadText').css("left", "5%");
    //$('#uploadDragDrop').css("background-color", greenUploadBackgroundColor);
    //$('#listitem').css("color", "pink");

    //$(":file").parent().css("border-color", "transparent");


    //var greenUploadBackgroundColor = "#c5ffd4";

    //$('#downloadBox').css("background-color", greenUploadBackgroundColor);
    ////$('#downloadBox').css("background-color", greenUploadBackgroundColor);
    //$('#downloadBox').css("color", "#000");
    //$("#uploadedFiles").html("");

  
}

function ChangeCSS() {
    var greenUploadBackgroundColor = "#c5ffd4";
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

    //if ($("#Status").val() != "TOBEDELIVERED") {
    //    SourceFileUpload();
    //}


}

//$(document).ajaxComplete(function () {
//    if ($("#Status").val() != "TOBEDELIVERED") {
//        if ($("#uploadedFiles").children("li").hasClass("success")) {

//            var count = $("#uploadedFiles").children().length;

//            $('#uploadText').children().html("");
//            $('#uploadText').children().css("border", "transparent");
//            document.getElementById("drop").style.padding = "0px";


//            if (refreshFileFlag == 1) {
//                //    RefreshListSourceRef();
//            }
//        }
//    }
//});