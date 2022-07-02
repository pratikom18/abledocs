function LinkFiles(JobID, FileID,flag) {
    var row = '';
    
    $(".linkpopupfile input[type=checkbox]:checked").each(function () {
        if (row == '') {
            row = $(this).val();
        } else {
            row = row + ',' + $(this).val();
        }
    });
    if (row == '') {
        return;
    }
    $.ajax({
        type: "POST",
        url: "/File/InsertLinkPopupFile",
        data: { FileID: FileID, JobID: JobID, RefFileID: row,flag:flag },
        dataType: "JSON",
        success: function (data) {
            window.location.href = "/file?ID=" + FileID+"&flag="+flag;

        },
        error: function () {
            alert("error");
        }
    });
}