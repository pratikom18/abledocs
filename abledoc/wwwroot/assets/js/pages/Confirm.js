$(document).ready(function () {
    $('.DenyCheckout').click(function () {
        $(".close").click();

    });
});
function ALTTextCheckout(ID,flag){
    
    $.ajax({
        type: "POST",
        url: "/Phases/InsertAltText",
        data: { FileID: ID,flag:flag},
        dataType: "JSON",
        success: function (data) {

            window.location.href = "/alttxtfile?fileID=" + ID+"&flag="+flag;
           
        },
        error: function () {
            alert("error");
        }
    });
}