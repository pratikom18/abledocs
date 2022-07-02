/*$(".datepicker").datepicker({
    changeMonth: true,
    changeYear: true,
    dateFormat: 'yy-mm-dd'
});*/
var flag = $("#flag").val();

$(".datepicker").datetimepicker({
    format: "YYYY-MM-DD",
    icons: {
        time: "fa fa-clock-o",
        date: "fa fa-calendar",
        up: "fa fa-chevron-up",
        down: "fa fa-chevron-down",
        previous: "fa fa-chevron-left",
        next: "fa fa-chevron-right",
        today: "fa fa-screenshot",
        clear: "fa fa-trash",
        close: "fa fa-remove",
    },
});

var paramsGlobal;
var jobIDGlobal;

var jobID = document.getElementById("Add_JobID").value;
var params = jobID;
SetValues(params, jobID);

function SetValues(paramsVal, jobIDVal) {
    paramsGlobal = paramsVal;
    jobIDGlobal = jobIDVal;
}

function UpdateJobPanelDataInvoice(allFileId) {
    var fileIdInstance = allFileId.split(" ");
    var jobID = fileIdInstance[0];
    var params = "";
    for (var i = 1; i < fileIdInstance.length - 1; i++) {
        var description = document.getElementById("descriptionText_" + fileIdInstance[i]).value;
        var unitPrice = document.getElementById("priceText_" + fileIdInstance[i]).value;
        var quantity = document.getElementById("quantityText_" + fileIdInstance[i]).value;
        var e = document.getElementById("pricePerSelect_" + fileIdInstance[i]);
        var pricePer = e.options[e.selectedIndex].text;
        var taxe = document.getElementById("taxSelect");
        var tax = taxe.options[taxe.selectedIndex].text;

        params = params + fileIdInstance[i] + "|" + unitPrice + "|" + pricePer + "|" + quantity + "|" + tax + "|" + description + "|" + jobIDGlobal + "[]";

    }
    fileRefreshRequired = 0;

    var companyName = document.getElementById("companyName").value;
    var address1 = document.getElementById("ad1").value;
    var address2 = document.getElementById("ad2").value;
    var clientFN = document.getElementById("fn").value;
    var clientLN = document.getElementById("ln").value;
    var email = document.getElementById("email").value;
    var telephone = document.getElementById("telephone").value;
    var city = document.getElementById("city").value;
    var province = document.getElementById("province").value;
    var country = document.getElementById("country").value;
    var postalCode = document.getElementById("postalCode").value;
    var invoiceRow = document.getElementById("invoiceRow").value;
    var contractDate = document.getElementById("ContractDate").value;
    var paramsContact = invoiceRow + "|" + companyName + "|" + address1 + "|" + address2 + "|" + clientFN + "|" + clientLN + "|" + email + "|" + telephone + "|" + city + "|" +
        province + "|" + country + "|" + postalCode + "|" + contractDate + "|" + jobIDGlobal;
    
    currentInvoiceUpdateParams = paramsContact;
    console.log(params);
    AjaxInvoice("JobPanelUpdateInvoice", params);




}

function DeleteFileFromInvoice(ID, jobID) {
    swal({
        title: 'Are you sure?',
        //text: 'Some text.',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes!',
        cancelButtonText: 'No.'
    }).then((result) => {
        if (result.value) {
            var params = ID + "|" + jobID;
            AjaxInvoice("FileDeleteInvoice", params);
            //Swal.fire('Saved!', '', 'success')
        } else {
            // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
        }
    });
  
}

function GenerateDownloadInvoiceHandler(jobID) {
    AjaxInvoice("GenerateDownloadInvoiceJobSetupPage", jobID);
}

function AjaxInvoice(state, params) {
    $.ajax({
        url: '/Jobs/Job',
        data: { State: state, Params: params,JobID: jobIDGlobal,flag:flag },
        type: 'POST'
    }).done(function (responseData) {
        console.log('Done: ', responseData);

        if (state == "FileDeleteInvoice") {
           // toastr.success("Remove successfully.");
            $.notify({
                icon: 'add_alert',
                title: '<strong>Success!</strong>',
                message: "Remove Successfully."
            }, {
                type: 'success'
            });
        }
        if (state == "JobPanelUpdateInvoice") {
            $("#pageRefresh").load("/Jobs/Invoice?JobID=" + responseData.jobid + "&updateVal=1&flag="+flag+" #pageRefresh");
            //toastr.success("Update successfully.");
            $.notify({
                icon: 'add_alert',
                title: '<strong>Success!</strong>',
                message: "Update Successfully."
            }, {
                type: 'success'
            });

           // $("#invoiceGenerateButton").removeAttr("disabled");
            //$("#generateDownloadInvoiceButton").removeAttr("disabled");
            $("#invoiceGenerateButton").removeClass("ui-disabled");
            $("#generateDownloadInvoiceButton").removeClass("ui-disabled");
        } else {
            
            $("#pageRefresh").load("/Jobs/Invoice?JobID=" + jobIDGlobal + "&flag=" + flag +" #pageRefresh");
        }

        if (state == "JobPanelUpdateInvoice") {
            AjaxInvoice("JobInvoiceContactUpdate", currentInvoiceUpdateParams);

        }

        if (state == "GenerateInvoiceJobSetupPage") {
            //myWindow.blur();
            //$("#invoicePanelClose").click();
            //    window.location.href = "production.php?tab=TOBEINVOICED";
            window.location.href = "/invoices/tobeinvoiced";
        }
        else if (state == "GenerateDownloadInvoiceJobSetupPage") {
            var win = window.open('/pendinginvoices/GenerateInvoicePdf?Params=' + 0 + '|' + jobIDGlobal + '|' + 0 + '&InsertFlag=1&State=DownloadInvoice&flag='+flag, '_blank');
            //$("#invoicePanelClose").click();
            window.location.href = "/invoices/tobeinvoiced";
            //window.location.href = "production.php?tab=TOBEINVOICED";
            //$("#invoicePanelClose").click();
            //window.location.href = "production.php?tab=TOBEINVOICED";
        }


    }).fail(function () {
        console.log('Failed');
    });
}