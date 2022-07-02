$(document).ready(function () {
    $("#PageSlider").on('change', function postinput() {
        var matchvalue = $(this).val(); // this.value
        var file = $("#FileID").val();
        var cat = 'TAGGING';
        $('#PageSliderVal').val(matchvalue);
        var flag = $('#flag').val();
        $.ajax({
            url: '/tag/FileProgress',
            data: { Page: matchvalue, File: file, Cat: cat, flag: flag },
            type: 'POST'
        }).done(function (responseData) {
            console.log('Done: ', responseData);
        }).fail(function () {
            console.log('Failed');
        });
    });

    $("#AseemblySlider").on('change', function postinput() {
        var matchvalue = $(this).val(); // this.value
        var file = $("#FileID").val();
        var cat = 'ASSEMBLY';
        $('#AseemblySliderVal').val(matchvalue);
        var flag = $('#flag').val();
        $.ajax({
            url: '/tag/FileProgress',
            data: { Page: matchvalue, File: file, Cat: cat, flag: flag },
            type: 'POST'
        }).done(function (responseData) {
            console.log('Done: ', responseData);
        }).fail(function () {
            console.log('Failed');
        });
    });

});

function ChangeFlag(flagVal) {
    var file = $("#FileID").val();
    if (flagVal == 1) {
        AjaxState("FormsFlag", file);
    }
    else if (flagVal == 2) {
        AjaxState("LinkingFlag", file);
    }
    else if (flagVal == 3) {
        AjaxState("ReviewFlag", file);
    }
}