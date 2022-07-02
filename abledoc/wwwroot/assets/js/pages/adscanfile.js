$(document).ready(function () {

    $('#clientID').selectpicker('setStyle', 'btn btn-link');
    $('.filter-option').addClass('filter-option-1');

    $('#ADscanfilesForm').validate({
        rules: {

            url: {
                required: true
            },

        },


        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        },
        submitHandler: function (form) {
            form.submit(); // form validation success, call ajax form submit
        }
    });

    $(".btnAddurl").click(function () {
        var $modal = $('#popupAdScanDetail');
        $modal.load('/adscan/AdscanFileUrl?id=' + $('#ID').val()+'&flag='+$('#flag').val(), function () {
            $('#bootstrap-modal').modal({
                show: true
            });
        });
        
    });

    $("#btnstartcrawl").click(function () {
        var id = $('#hdnadscanid').val();
        Crawladscan(id);
    });

    $("#btndelete").click(function () {
        var id = $('#hdnadscanid').val();
        Deleteadscan(id);
    });

    $("#btnreport").click(function () {
        var accesskey = $('#hdnadscanaccesskey').val();
        Reportadscan(accesskey);
    });

    $("#btnCreateJob").click(function () {
        var clientid = $('#hdnadscanclientid').val();
        var id = $('#hdnadscanid').val();
        Createjob(clientid,id);
    });

    $("#btngotojob").click(function () {
        var jobid = $('#hdnadscanjobid').val();
        Gotojob(jobid);
    });


});

function Crawladscan(ID) {
    var flag = $('#flag').val();
    $.ajax({
        type: "POST",
        url: "/ADscan/updateCrawl",
        data: { ID: ID,flag:flag },
        dataType: "JSON",
        success: function (data) {
            
            window.location.href = "/adscan";
        },
        error: function () {
            alert("error");
        }
    });
}

function Deleteadscan(ID) {
    var flag = $('#flag').val();
    $.ajax({
        type: "POST",
        url: "/ADscan/deleteCrawl",
        data: { ID: ID, flag: flag },
        dataType: "JSON",
        success: function (data) {

            window.location.href = "/adscan/";


        },
        error: function () {
            alert("error");
        }
    });
}

function Createjob(clientID,ID) {
    var flag = $('#flag').val();
    $.ajax({
        type: "POST",
        url: "/ADscan/adscanCreatejob",
        data: { clientID: clientID, ID: ID, flag: flag },
        dataType: "JSON",
        success: function (data) {
            window.location.href = "/jobs/JobsInitial/" + data.jobid + "?flag=" + flag;
        },
        error: function () {
            alert("error");
        }
    });
}

function Gotojob(JobID) {
    var flag = $('#flag').val();
    $.ajax({
        type: "GET",
        success: function (data) {
            
            window.location.href = "/jobs/JobsInitial/" + JobID + "?flag=" + flag;
        },
        error: function () {
            alert("error");
        }
    });
}



function Reportadscan(accesskey) {

            alert(accesskey);
   
}



