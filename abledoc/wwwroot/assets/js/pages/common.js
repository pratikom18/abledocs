$(document).ready(function () {

    $(document).on('click', '.btnConversation', function () {
        if ($('#Conversation').val() == "") {
            $('.conversation-error').show();
            return;
        } else {
            $('.conversation-error').hide();
        }


        var FileID = parseInt($("#ID").val());
        var JobID = parseInt($("#JobID").val());
        var Conversation = $('#Conversation').val();
        var flag = $('#flag').val();
        $.ajax({
            type: "POST",
            url: "/File/InsertConversions",
            data: { FileID: FileID, JobID: JobID, Conversation: Conversation, flag: flag },
            //dataType: "JSON",
            success: function (res) {

                //window.location.href = "/file?ID=" + FileID;
                $('.Conversation').load('/file/Conversations?id=' + FileID + '&flag=' + flag, function () {
                    $("#datatables").dataTable();
                });
            },
            error: function () {
                alert("error");
            }
        });
    });

    $(document).on('click', '.btnClientInstruction', function () {

        if ($('#Clientinstruction').val() == "") {
            $('.clientinstruction-error').show();
            return;
        } else {
            $('.clientinstruction-error').hide();
        }

        var FileID = parseInt($("#ID").val());
        var JobID = parseInt($("#JobID").val());
        var Status = $('#hdnStatus').val();
        var Clientinstruction = $('#Clientinstruction').val();
        var flag = $('#flag').val();
        $.ajax({
            type: "POST",
            url: "/File/InsertClientinstruction",
            data: { FileID: FileID, JobID: JobID, Clientinstruction: Clientinstruction, Status: Status, flag: flag },
            //dataType: "JSON",
            success: function (res) {

                //window.location.href = "/file?ID=" + FileID;
                $('.ClientInstruction').load('/file/ClientInstruction?id=' + FileID + '&flag=' + flag, function () {
                    $("#datatables1").dataTable();
                });
            },
            error: function () {
                alert("error");
            }
        });
    });



});

