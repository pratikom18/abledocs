$(document).ready(function () {
    $('.DenyCheckout').click(function () {
        $(".close").click();

    });
});
function ALTTextDelete(ID,flag) {

    $.ajax({
        type: "POST",
        url: "/Phases/DeleteAltText",
        data: { FileID: ID,flag:flag },
        dataType: "JSON",
        success: function (data) {
            $(".close").click();
            $('.altID_' + ID).hide();
        },
        error: function () {
            alert("error");
        }
    });
}