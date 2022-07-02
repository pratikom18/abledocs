$(document).ready(function () {
    $('.show-File').click(function () {
        $(this).addClass('active');
        $('.show-Error').removeClass('active');
        $('#uploadFlag').val('file');
        $('.uploadLabel').text('Version');
        $("#uploadedFiles").empty();
    });
    $('.show-Error').click(function () {
        $(this).addClass('active');
        $('.show-File').removeClass('active');
        $('#uploadFlag').val('error');
        $('.uploadLabel').text('Error Reports File');
        $("#uploadedFiles").empty();
    });

});
