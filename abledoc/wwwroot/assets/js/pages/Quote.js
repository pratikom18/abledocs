//$(".select2").select2();
var flag = $("#flag").val();

$(".checkbox-ui").checkboxradio();
var paramsGlobal;
var jobIDGlobal;

var jobID = document.getElementById("Add_JobID").value;
var params = jobID;
SetValues(params, jobID);

function SetValues(paramsVal, jobIDVal) {
    paramsGlobal = paramsVal;
    jobIDGlobal = jobIDVal;
}

function DeleteFileFromQuote(fileID, jobID) {
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
            var params = fileID + "|" + jobID;
            AjaxState("FileDeleteQuote", params);
        } else {
            // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
        }
    });
}
function DeleteAllFileFromQuote(jobID) {

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
            var params = jobID;
            AjaxState("AllFileDeleteQuote", params);
            //Swal.fire('Saved!', '', 'success')
        } else {
            // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
        }
    });
}

function AddEmptyFileFunc() {
    // Getting File Description, Quantity, Unit
    var description = $('#descriptionTextAdd').val();
    var quantity = $('#quantityTextAdd').val();
    var unit = $("#addPricePerUnit option:selected").text();
    var price = $('#priceTextAdd').val();
    var params = jobIDGlobal + "|" + description + "|" + quantity + "|" + unit + "|" + price;
    AjaxState("AddEmptyFileState", params);
    //alert(description + " " + quantity + " " + unit);
}

function AddNotes(jobID) {
    var textData = document.getElementById("addNotesText").value;
    params = jobID + "|" + textData + "|";
    fileRefreshRequired = 0;
    AjaxState("AddNotes", params);
    fileRefreshRequired = 0;
    //JobQuotation(jobID);
}

function AddOffering(jobID) {
    var textData = document.getElementById("addOfferingText").value;
    params = jobID + "|" + textData + "|";
    fileRefreshRequired = 0;
    AjaxState("AddOffering", params);
    fileRefreshRequired = 0;
    //JobQuotation(jobID);
}

function SaveQuoteText(ID) {
    var textData = document.getElementById("qData_" + ID).value;
    var params = "";
    params = ID + "|" + textData + "|";
    AjaxState("SaveQuoteText", params);
}

function DeleteQuoteText(ID, jobID) {
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
            var textData = document.getElementById("qData_" + ID).value;
            var params = "";
            params = ID + "|" + textData + "|" + jobID + "|";


            AjaxState("DeleteQuoteText", params);
            //Swal.fire('Saved!', '', 'success')
        } else {
            // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
        }
    });

}

function DeleteAllOffering(jobID) {
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
            var params = jobID;
            fileRefreshRequired = 0;
            AjaxState('DeleteAllOffering', params);
            //Swal.fire('Saved!', '', 'success')
        } else {
            // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
        }
    });

}

function DeleteAllNotes(jobID) {
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
            var params = jobID;
            fileRefreshRequired = 0;
            AjaxState('DeleteAllNotes', params);
            //Swal.fire('Saved!', '', 'success')
        } else {
            // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
        }
    });


}


function SetDefaultButton(jobID) {

    var errorFlag = 0;
    var defaultUnit = $("#defaultAddPricePerUnit").val();
    var defaultPrice = $("#defaultAddPrice").val();
    var defaultQuantity = $("#defaultAddQuantity").val();

    var params = jobID + "|" + defaultUnit + "|" + defaultPrice + "|" + defaultQuantity;

    if (defaultUnit == "Select_Unit") {
        errorFlag = 1;
    }
    else if (defaultPrice == "") {
        errorFlag = 1;
    }

    if (errorFlag == 1) {
        $("#defaultErrorMessage").show();
    }
    else {
        $("#defaultErrorMessage").hide();
        AjaxState("DefaultUnitPriceSet", params);
    }

}

function PriceAutoFill(unitTypeSelect, hourlyRate, pageRate, fieldID, priceFlagVal, pageCount) {
    // Get the selected value
    //alert(unitTypeSelect + "-" + hourlyRate + "-" + pageRate + "-" + fieldID + "-" + priceFlagVal + "-" + pageCount)
  /*  var e = document.getElementById(unitTypeSelect);
    var unitType = e.options[e.selectedIndex].value;

    var rowIDSplit = fieldID.split("_");
    var rowID = rowIDSplit[1];

    if (unitType == "Hour") {
        document.getElementById(fieldID).value = hourlyRate;
        document.getElementById("field_Quantity").value = 0;
    }
    else if (unitType == "Page") {
        document.getElementById(fieldID).value = pageRate;
        document.getElementById("field_Quantity").value = pageCount;
    }
    else if (unitType == "Document") {
        document.getElementById(fieldID).value = 0;
        document.getElementById("field_Quantity").value = 0;
    }*/
}

function UpdateJobPanelData(allFileId) {
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
        var description_id = document.getElementById("description_id_" + fileIdInstance[i]).value;
        params = params + fileIdInstance[i] + "|" + unitPrice + "|" + pricePer + "|" + quantity + "|" + tax + "|" + description + "|" + jobID + "|" + description_id + "[]";
    }
    //console.log(params);
    AjaxState("JobPanelUpdate", params);


}

function JobQuotation(jobID, updateVal = 0) {
    var params = "";
    // Get the selected contact name

    params = params + jobID + " " + updateVal;
    //alert(params);
    AjaxState("JobQuoteState", params);
    //$("#quoteUpdateButton").click();
}

function AjaxState(state, params) {
    $.ajax({
        url: '/Jobs/Job',
        data: { State: state, Params: params, JobID: jobIDGlobal,flag:flag },
        type: 'POST'
    }).done(function (responseData) {
        console.log('Done: ', responseData);


        //if (state == "DeleteQuoteText" || state == "AddOffering" || state == "AddNotes" || state == "AddEmptyFileState" || state == "FileDeleteQuote") {
        //    $("#pageRefresh").load("/Jobs/Quote?JobID=" + responseData.jobid+" #pageRefresh");
        //}
        if (state == "JobPanelUpdate") {
            AjaxState("QuoteDone", jobIDGlobal);

        } else if (state == "QuoteDone") {
            $("#pageRefresh").load("/Jobs/Quote?JobID=" + responseData.jobid + "&updateVal=1&flag="+flag+" #pageRefresh");

            //JobQuotation(jobIDGlobal, 1);

            $.notify({
                icon: 'add_alert',
                title: '<strong>Success!</strong>',
                message: "Update successfully."
            }, {
                type: 'success'
            });
        }

        if (state == "SaveQuoteText") {
            $.notify({
                icon: 'add_alert',
                title: '<strong>Success!</strong>',
                message: responseData.message
            }, {
                type: 'success'
            });

        } else {
            $("#pageRefresh").load("/Jobs/Quote?JobID=" + responseData.jobid + "&flag="+flag+" #pageRefresh");
        }

        if (state == "FileDeleteQuote") {
            $.notify({
                icon: 'add_alert',
                title: '<strong>Success!</strong>',
                message: "Remove Successfully."
            }, {
                type: 'success'
            });
        }
        if (state == "AllFileDeleteQuote") {
            $.notify({
                icon: 'add_alert',
                title: '<strong>Success!</strong>',
                message: "Remove All Successfully."
            }, {
                type: 'success'
            });
        }

    }).fail(function () {
        console.log('Failed');
    });
}
$(document).ready(function () {
    $(document).on("change", ".description", function () {
       // debugger
        var $row = $(this).closest('tr');
        var name = $(this).val();
        name = name.replace(" ", "_");
        $.ajax({
            url: '/jobs/getdescriptionbyname',
            data: { name: name },
            type: 'POST'
        }).done(function (responseData) {
            //alert(responseData.data.productPrice);
            $row.find(".description_id").val(responseData.data.id);
            $row.find('.price').val(responseData.data.productPrice).trigger("change");
            

        }).fail(function () {
            console.log('Failed');
        });

        $.ajax({
            type: "POST",
            url: "/jobs/getunitlist",
            data: { name: name },
            dataType: "JSON",
            success: function (responseData) {

                //console.log(responseData);
                $row.find('.pricePerSelect').empty();
                $row.find('.pricePerSelect').append($('<option>', {
                    value: '',
                    text: 'Select'
                }));
                $.each(responseData.data, function (index, item) {
                    
                    $row.find('.pricePerSelect').append($('<option>', {
                        value: item.typecode,
                        text: item.typename
                    }));
                });


            },
            error: function () {
                alert("error");
            }
        });

    });

    
});

