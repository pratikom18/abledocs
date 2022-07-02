
//$(document).ready(function () {
$(document).on("click", "#saveBtn", function () {
    if (!$("#bugReportForm").valid()) {
        return false;
    }

});
    function SaveData() {

        $('#saveBtn').click(function () {
            var Summary = ($("#Summary").val());
            var Severity = $("#Severity").select2();
            var Priority = $('#Priority').select2();
            var Description = $('#Description').val();
            var Steps_To_Reproduce = $('#Steps_To_Reproduce').val();

            $.ajax({
                type: "POST",
                url: "/BugReport/Index",
                data: { Summary: Summary, Severity: Severity, Priority: Priority, Description: Description, Steps_To_Reproduce: Steps_To_Reproduce },
                dataType: "JSON",
                success: function (data) {
                    alert('Thank you - your bug has been submitted');

                },
                error: function () {
                    alert("error");
                }
            });
        });
    }
//});
$(document).ready(function () {
    $("#Submit").click(function (e) {
        e.preventDefault();
            $('#bugReportForm').submit();

    });


});