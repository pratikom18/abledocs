
$(document).ready(function () {
    $('.show-Wc').click(function () {
        $(this).addClass('active');
        $('.show-PdfUa').removeClass('active');
        $('.show-Unsecured').removeClass('active');
        $('.show-Secured').removeClass('active');
        $('#uploadFlag').val('wcFile');
        $('.uploadLabel').text('Phase 3 Working Copies');
    });
    $('.show-PdfUa').click(function () {
        $(this).addClass('active');
        $('.show-Wc').removeClass('active');
        $('.show-Unsecured').removeClass('active');
        $('.show-Secured').removeClass('active');
        $('#uploadFlag').val('pacFile');
        $('.uploadLabel').text('PDF/UA Compliance Reports');
        $("#uploadedFiles").empty();
    });
    $('.show-Unsecured').click(function () {
        $(this).addClass('active');
        $('.show-Wc').removeClass('active');
        $('.show-PdfUa').removeClass('active');
        $('.show-Secured').removeClass('active');
        $('#uploadFlag').val('unsecuredFile');
        $('.uploadLabel').text('Unsecured Files');
        $("#uploadedFiles").empty();
    });
    $('.show-Secured').click(function () {
        $(this).addClass('active');
        $('.show-Wc').removeClass('active');
        $('.show-PdfUa').removeClass('active');
        $('.show-Unsecured').removeClass('active');
        $('#uploadFlag').val('securedFile');
        $('.uploadLabel').text('Secured');
        $("#uploadedFiles").empty();
    });
});



