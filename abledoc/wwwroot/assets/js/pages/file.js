$(".checkbox-ui").checkboxradio();

$(".checkbox-ui-woi").checkboxradio({
    icon: false,
});

$(document).ready(function () {
    $('#hdnStatusName').val($('#Status').find(":selected").text());
    $('.Checkout').click(function () {
        
        var $row = $(this).attr("data-link");
        var FileID = parseInt($("#ID").val());
        var Checkout_PageNumber = parseInt($("#jobsFiles_CurrentPage").val());
        var State = $('#Status').val();
        var flag = $('#flag').val();
        $.ajax({
            type: "POST",
            url: "/File/InsertCheckout",
            data: { FileID: FileID, Checkout_PageNumber: Checkout_PageNumber, State: State,flag:flag},
            dataType: "JSON",
            success: function (data) {
                
                if ($row == "Checkout") {
                    window.location.href = "/tag?ID=" + FileID + "&flag=" + flag;
                }
                else if ($row == "CheckoutToReview") {
                    window.location.href = "/reviewfiles?ID=" + FileID + "&flag=" + flag;
                }
                else if ($row == "CheckoutToFinal") {
                    window.location.href = "/finalfiles?ID=" + FileID + "&flag=" + flag;
                }
                else if ($row == "CheckoutToQC") {
                    window.location.href = "/qcfiles?ID=" + FileID + "&flag=" + flag;
                }
 

            },
            error: function () {
                alert("error");
            }
        });
    })

    $('.SendTo').click(function () {
        var FileID = parseInt($("#ID").val());
        var flag = $('#flag').val();

        var $modal = $('#popupSendFile');
        $modal.load('/File/SendToFile?id=' + FileID + '&flag=' + flag, function () {
            $('#bootstrap-modal').modal({
                show: true
            });
            $('#Status').selectpicker('setStyle', 'btn btn-link');
            $('#SendTo').selectpicker('setStyle', 'btn btn-link');
            $('.filter-option').addClass('filter-option-1');
        });
       
    });

    $('.LinkPopupFile').click(function () {
        var JobID = parseInt($("#JobID").val());
        var FileID = parseInt($("#ID").val());
        var flag = $("#flag").val();
        var $modal = $('#popupSendFile');
        $modal.load('/File/LinkPopupFile?jobID=' + JobID + '&fileID=' + FileID+'&flag='+flag, function () {
            $('#bootstrap-modal').modal({
                show: true
            });
            $('.linkpopupfile').dataTable();
        });
       
    });

    //var box;
    //box = document.getElementById("drop");
    //box.addEventListener("dragenter", OnDragEnter, false);
    //box.addEventListener("dragover", OnDragOver, false);
    //box.addEventListener("dragout", OnDragOut, false);
    //box.addEventListener("drop", OnDrop, false);

    //var box1;
    //box1 = document.getElementById("drop1");
    //box1.addEventListener("dragenter", OnDragEnter1, false);
    //box1.addEventListener("dragover", OnDragOver1, false);
    //box1.addEventListener("dragout", OnDragOut1, false);
    //box1.addEventListener("drop", OnDrop1, false);
    $('#Status').change(function () {
        
        var oldStatus = $('#hdnoldStatus').val();
        var newStatus = $(this).val();
        var oldStatus1 = $('#hdnStatusName').val();
        var newStatus1 = $('#Status').find(":selected").text();
        var $modal = $('#popupSendFile');
        $modal.load('/File/ConfirmStatus?oldStatus=' + oldStatus + '&newStatus=' + newStatus + '&oldStatus1=' + oldStatus1.replace(/ /g, "-") + '&newStatus1=' + newStatus1.replace(/ /g, "-"), function () {
            $('#bootstrap-Status').modal({
                show: true
            });
        });
    });

    $(document).on("click", ".DenyCheckoutStatus", function () {
        $(".close").click();
        $('#Status').val($('#oldStatus').val());
        $('.selectpicker').selectpicker('refresh');
    });

    $(document).on("click", "#ConfirmStatus", function () {
        $(".close").click();
        $("#fileSave").click();
        $('#hdnoldStatus').val($('#newStatus').val());
        $('#hdnStatusName').val($('#newStatus1').val());
    });

});

function AddFiles(type) {
    var FileID = parseInt($("#ID").val());
    var $modal = $('#popupSendFile');
    $modal.load('/File/DragDropFile?id=' + FileID + "&type=" + type, function () {
        $('#bootstrap-modal').modal({
            show: true
        });
        $('#Type').selectpicker('setStyle', 'btn btn-link');
        $('.filter-option').addClass('filter-option-1');

        var box;
        box = document.getElementById("drop");
        box.addEventListener("dragenter", OnDragEnter, false);
        box.addEventListener("dragover", OnDragOver, false);
        box.addEventListener("drop", OnDrop, false);
        
        if (type == "REFERENCE") {
            $('.type').hide();
        }
    });
   
}

//function DeleteFileVersion(ID) {
    
//    $.ajax({
//        type: "POST",
//        url: "/File/DeleteFileVersion",
//        data: { ID: ID},
//        dataType: "JSON",
//        success: function (data) {
//            window.location.href = "/file?ID=" + FileID;

//        },
//        error: function () {
//            alert("error");
//        }
//    });
//}

function UpdateTagging(JobID) {
    var TaggingInstructions = $('#Tagging_Instructions').val();
    var FileID = $('#ID').val();
    var flag = $('#flag').val();
    $.ajax({
        type: "POST",
        url: "/File/UpdateTagging",
        data: { JobID: JobID, TaggingInstructions: TaggingInstructions,flag:flag },
        dataType: "JSON",
        success: function (data) {
            window.location.href = "/file?ID=" + FileID+"&flag="+flag;

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

function fileTreeDataTable(ID) {
    var $modal = $('.FileTree');

    $modal.load('/Jobs/FileTreeDataTable?id=' + ID, function () {
        ToDataTable();
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
    //selectedFiles = e.dataTransfer.files;
    selectedFiles.push(e.dataTransfer.files);
    $(e.target).css('background-color', '');
    $("#uploadText").css('background-color', '');
    $("#uploadText i").css('background-color', '');
    $("#uploadDragDrop").css('background-color', '');
    $("#uploadedFiles").append("<li class='success' style='top:0px;height:50px;color:white;background-color:#1883C6B3;background-image: -webkit-linear-gradient(top, #1883C6B3, #1883C6);'>" + e.dataTransfer.files[0].name + "<br/>" + e.dataTransfer.files[0].size + "</li>");

    //var JobID = parseInt($("#JobID").val());
    //var FileID = parseInt($("#ID").val());
    //var FileType = $("#Type").val();

    //var data = new FormData();
    //data.append("jobID", JobID)
    //data.append("fileID", FileID)

    
    //    data.append("state", "REFERENCE")
    //    data.append("qcType", "")
   

    //for (var i = 0; i < selectedFiles.length; i++) {
    //    if (selectedFiles[i].length == 1) {
    //        var files = selectedFiles[i][0]
    //        data.append(files.name, files);
    //    } else if (selectedFiles[i].length > 1) {
    //        var files = selectedFiles[i]
    //        for (var j = 0; j < files.length; j++) {
    //            data.append(files[j].name, files[j]);
    //        }
    //    }

    //}
    
    //$.ajax({
    //    type: "POST",
    //    url: '/File/ProcessRequest',//Url.Action("", ""),
    //    contentType: false,
    //    processData: false,
    //    data: data,
    //    success: function (result) {
    //        $(e.target).css('background-color', '');
            
    //        var filename = '<a href="/fileget?ID=' + result.path + '">' + result.name+'</a>';

    //        $('table.ReferenceFilesTable > tbody > tr:first').before('<tr><td>' + filename + '</td><td>Reference</td><td>' + result.date+'</td></tr>');
    //       // window.location.href = "/file?ID=" + FileID;
    //    },
    //    error: function () {
    //        alert("There was error uploading files!");
    //    }
    //});

  
}
//end reference

//start version files
function OnDragEnter1(e) {
    e.stopPropagation();
    e.preventDefault();

}
function OnDragOver1(e) {
    e.stopPropagation();
    e.preventDefault();
    $(e.target).css('background-color', '#D3D3D3');
}
function OnDragOut1(e) {
    e.stopPropagation();
    e.preventDefault();
    $(e.target).css('background-color', '');
}
function OnDrop1(e) {
    e.stopPropagation();
    e.preventDefault();
    //selectedFiles = e.dataTransfer.files;
    selectedFiles.push(e.dataTransfer.files);

    var JobID = parseInt($("#JobID").val());
    var FileID = parseInt($("#ID").val());
    var FileType = $("#Type").val();
    var flag = $('#flag').val();

    var data = new FormData();
    data.append("jobID", JobID)
    data.append("fileID", FileID)

        data.append("state", FileType)
        data.append("qcType", "")
    data.append("flag", flag);

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
        url: '/File/ProcessRequest',//Url.Action("", ""),
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            $(e.target).css('background-color', '');

            var filename = '<a href="/fileget?ID=' + result.path + '&flag='+flag+'">' + result.name + '</a>';

            $('table.FileVersionsTable > tbody > tr:first').before('<tr><td>' + filename + '</td><td>' + FileType+'</td><td>' + result.date + '</td></tr>');
            // window.location.href = "/file?ID=" + FileID;
        },
        error: function () {
            alert("There was error uploading files!");
        }
    });

    //$("#uploadedFiles").append("<li class='success' style='top:0px;height:75px;color:white;background-color:#6DA81E;background-image: -webkit-linear-gradient(top, #92D400, #6DA81E);'>" + e.dataTransfer.files[0].name + "<br/>" + e.dataTransfer.files[0].size +"</li>");


}
//end version files
