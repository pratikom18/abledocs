$(document).ready(function () {
    $('#btnDownload').click(function () {
       

    });
});

function DownloadExcel() {

    $.ajax({
        type: "GET",
        url: "/ApprovedTimesheet/Downloadxl",
        data: { 'pinvalue': pinvalue },
        dataType: "JSON",
        success: function (response) {

        },
        error: function () {
            alert("error");
        }
    });

}

function ALTTextCheckout(ID,flag){
    
    $.ajax({
        type: "POST",
        url: "/Phases/InsertAltText",
        data: { FileID: ID},
        dataType: "JSON",
        success: function (data) {

            window.location.href = "/alttxtfile?fileID=" + ID+"&flag="+flag;
           
        },
        error: function () {
            alert("error");
        }
    });
}