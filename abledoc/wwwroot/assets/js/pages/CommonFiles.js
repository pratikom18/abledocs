var allowSubmitFlag = 0;
var reloadPrompt = 0;
var pagename

$(document).ready(function () {

    if (window.location.pathname.indexOf('qcfiles') != -1) {
        pagename = 'qcfiles';
    } else if (window.location.pathname.indexOf('reviewfiles') != -1) {
        pagename = 'reviewfiles';
    } else if (window.location.pathname.indexOf('finalfiles') != -1) {
        pagename = 'finalfiles';
    } else if (window.location.pathname.indexOf('tag') != -1) {
        pagename = 'tag';
    }

    HidePhasesCheckedOutScreen();

    var box;
    box = document.getElementById("drop");
    box.addEventListener("dragenter", OnDragEnter, false);
    box.addEventListener("dragover", OnDragOver, false);
    box.addEventListener("drop", OnDrop, false);

    $(document).on('click', '.btn-minus', function () {
        var number = $(this).closest('div').find('input').val();
        if (number == "") {
            return;
        }
        number = parseInt(number) - 1;
        if (number == 0) {
            $(this).prop('disabled', true);
        }
        $(this).closest('div').find('input').val(number);

    });

    $(document).on('click', '.btn-plus', function () {

        var number = $(this).closest('div').find('input').val();
        if (number == "") {
            number = 0;
        }
        number = parseInt(number) + 1;
        if (parseInt(number) > 0) {
            $(this).closest('div').find('.btn-minus').prop('disabled', false);
        }
        $(this).closest('div').find('input').val(number)
    });

    $(document).on('click', '.DeleteUploadFile', function () {

        var ID = $(this).attr("data-id");
        var $row = $(this).closest('li')
        DeleteUploadFile(ID, $row)
    });
});

$("#uploadText").click(function () {
    // creating input on-the-fly
    var input = $(document.createElement("input"));
    input.attr("type", "file");

    input.attr("id", "fileToUpload");
    input.attr("onchange", "fileToUpload(this)");


    input.attr("multiple", "true");
    // add onchange handler if you wish to get the file :)
    input.trigger("click"); // opening dialog
    return true; //  navigation
});

var selectedFiles = [];

var selectedFiles1;
//start reference
function OnDragEnter(e) {
    e.stopPropagation();
    e.preventDefault();

}
function OnDragOver(e) {
    e.stopPropagation();
    e.preventDefault();
    $(e.target).css('background-color', '#D3D3D3');
}
function OnDragOut(e) {
    e.stopPropagation();
    e.preventDefault();
    $(e.target).css('background-color', '');
}
function OnDrop(e) {
    e.stopPropagation();
    e.preventDefault();
    selectedFiles1 = e.dataTransfer.files;
    //selectedFiles.push(e.dataTransfer.files);
    $(e.target).css('background-color', '');
    $("#uploadText").css('background-color', '');
    $("#uploadText i").css('background-color', '');
    $("#uploadDragDrop").css('background-color', '');
    // $("#uploadedFiles").append("<li class='success' style='top:0px;height:50px;color:white;background-color:#1883C6B3;background-image: -webkit-linear-gradient(top, #1883C6B3, #1883C6);'>" + e.dataTransfer.files[0].name + "<br/>" + e.dataTransfer.files[0].size + "</li>");

    var JobID = parseInt($("#JobID").val());
    var FileID = parseInt($("#ID").val());
    var uploadFlag = $("#uploadFlag").val();
    var flag = $('#flag').val();
    var data = new FormData();
    data.append("JobID", JobID)
    data.append("ID", FileID)
    data.append("FileType", $('#FileType').val());
    data.append("uploadFlag", uploadFlag);
    data.append("flag", flag);

    if (selectedFiles1.length > 1) {
        for (var i = 0; i < selectedFiles1.length; i++) {
            var files = selectedFiles1[i]//[0]
            data.append(files.name, files);
        }
    } else {
        var files = selectedFiles1[0];
        data.append(files.name, files);
    }


    $.ajax({
        type: "POST",
        url: '/qcfiles/upload',//Url.Action("", ""),
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            $(e.target).css('background-color', '');


            var list = ""
            $.each(result.uploadfile, function (index, item) {

                list += "<li class='success' style = 'top:0px;height:50px;color:white;background-color:#1883C6B3;background-image: -webkit-linear-gradient(top, #1883C6B3, #1883C6);'> <label class='col-md-12' style='margin-bottom: 0px;color:#fff;'>" + item.fileName + " </label> <label class='col-md-11' style='color:#fff;'>" + item.size + "</label><i class='material-icons DeleteUploadFile' data-id='" + item.id + "' data-flag='" + flag + "'>delete</i></li >";
            });
            //var filename = '<a href="/fileget?ID=' + result.path + '">' + result.name + '</a>';

            $("#uploadedFiles").append(list);

            $('#FirstLevel').DataTable().destroy();
            ToDataTable();
            ShowPhasesCheckedOutScreen();
            allowSubmitFlag = 1;
        },
        error: function () {
            alert("There was error uploading files!");
        }
    });


}


function fileToUpload(input) {
    var file = input.files;
    selectedFiles1 = input.files;
    if (file.length > 1) {
        for (var i = 0; i < file.length; i++) {
            var files = file[i];//[0];
            // selectedFiles1 = files;


            //  selectedFiles.push(files);
            $("#uploadText").css('background-color', '');
            $("#uploadText i").css('background-color', '');
            $("#uploadDragDrop").css('background-color', '');
            //   $("#uploadedFiles").append("<li class='success' style='top:0px;height:50px;color:white;background-color:#1883C6B3;background-image: -webkit-linear-gradient(top, #1883C6B3, #1883C6);'>" + files.name + "<br/>" + files.size + "</li>");


        }
    } else {
        var files = file[0];

        //  selectedFiles.push(files);
        $("#uploadText").css('background-color', '');
        $("#uploadText i").css('background-color', '');
        $("#uploadDragDrop").css('background-color', '');
        //$("#uploadedFiles").append("<li class='success' style='top:0px;height:50px;color:white;background-color:#1883C6B3;background-image: -webkit-linear-gradient(top, #1883C6B3, #1883C6);'>" + files.name + "<br/>" + files.size + "</li>");

    }

    var JobID = parseInt($("#JobID").val());
    var FileID = parseInt($("#ID").val());
    var uploadFlag = $("#uploadFlag").val();
    var flag = $("#flag").val();
    var data = new FormData();
    data.append("JobID", JobID)
    data.append("ID", FileID)
    data.append("FileType", $('#FileType').val());
    data.append("uploadFlag", uploadFlag);
    data.append("flag", flag);

    if (selectedFiles1.length > 1) {
        for (var i = 0; i < selectedFiles1.length; i++) {
            var files = selectedFiles1[i];
            data.append(files.name, files);
        }
    } else {
        var files = selectedFiles1[0];
        data.append(files.name, files);
    }


    $.ajax({
        type: "POST",
        url: '/qcfiles/upload',//Url.Action("", ""),
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            $(input.target).css('background-color', '');


            var list = ""
            $.each(result.uploadfile, function (index, item) {

                list += "<li class='success' style = 'top:0px;height:50px;color:white;background-color:#1883C6B3;background-image: -webkit-linear-gradient(top, #1883C6B3, #1883C6);'> <label class='col-md-12' style='margin-bottom: 0px;color:#fff;'>" + item.fileName + " </label> <label class='col-md-11' style='color:#fff;'>" + item.size + "</label><i class='material-icons DeleteUploadFile' data-id='" + item.id + "' data-flag='" + flag + "' >delete</i></li >";
            });
            //var filename = '<a href="/fileget?ID=' + result.path + '">' + result.name + '</a>';

            $("#uploadedFiles").append(list);

            $('#FirstLevel').DataTable().destroy();
            ToDataTable();
            ShowPhasesCheckedOutScreen();
            allowSubmitFlag = 1;
        },
        error: function () {
            alert("There was error uploading files!");
        }
    });


}


function CheckIn(phase) {
    var phaseVal = "";
    if (phase == 1) {
        phaseVal = "Assign";
    }
    else if (phase == 2) {
        phaseVal = "ToBeReviewed";
    }
    else if (phase == 4) {
        phaseVal = "ToBeFinalized";
    }
    else if (phase == 5) {
        phaseVal = "ToBeQualityControlled";
    }
    else if ((phase == 6 && pagename == 'qcfiles') || (phase == 6 && pagename == 'finalfiles')) {
        phaseVal = "Complete";
    }

    fileToCheckin = 1;
    timerStarted = 0;
    clearTimeout(t);
    $("#stop").hide();
    $("#start").show();

    var jobID = $("#JobID").val();
    var fileID = $("#ID").val();
    var fileState = $("#State").val();
    var fixFile = 0;
    var params = jobID + " | " + fileID + " | " + fileState + " | " + fixFile;


    var phaseValGlobal = phaseVal;
    if (timerStarted == 1) {
        AjaxState("TimerCompareState", params);
    }
    else if (timerStarted == 0) {
        if (fileToCheckin == 1) {
            if (pagename == 'qcfiles') {
                CheckInButtonQcFile(phaseValGlobal);
            } else if (pagename == 'reviewfiles') {
                CheckInButtonReview(phaseValGlobal);
            } else if (pagename == 'finalfiles') {
                CheckInButtonFinalFile(phaseValGlobal);
            } else if (pagename == 'tag') {
                CheckInButtonTag(phaseValGlobal);
            }
        }

    }

    function CheckInButtonQcFile(phaseVal) {
        timerStarted = 0;
        if (allowSubmitFlag == 1) {
            reloadPrompt = 1;
            //$('#checkoutTimer').mobiscroll('stop');
            //var timer = $("#checkoutTimer").val();
            var timer = $("#stopWatch").html();
            var comments = $("#PhaseComments").val();
            var file = $("#ID").val();
            var jobId = $("#JobID").val();
            var altTextStatus = $("#altTextStatusID").val();
            var flag = $('#flag').val();
            $.ajax({
                url: '/qcfiles/QcFileCheckin',
                data: { File: file, Comments: comments, Job: jobId, AltTextStatus: altTextStatus, Timer: timer, flag: flag },
                type: 'POST'
            }).done(function (responseData) {
                console.log('Done: ', responseData);
                AjaxState(phaseVal, file);
            }).fail(function () {
                console.log('Failed');
            });
        }
        else {
            alert('Please upload atleast one file!');
        }
    }

    function CheckInButtonReview(phaseVal) {
        timerStarted = 0;
        if (allowSubmitFlag == 1) {
            reloadPrompt = 1;
            //$('#checkoutTimer').mobiscroll('stop');
            //var timer = $("#checkoutTimer").val();
            var timer = $("#stopWatch").html();
            var comments = $("#PhaseComments").val();
            var file = $("#ID").val();
            var jobId = $("#JobID").val();
            var lastTagger = $("#LastTagger").val();

            var tagOrder = 0;// $("#TagOrder").val();
            var contentOrder = 0;// $("#ContentOrder").val();
            var tabbingOrder = 0;// $("#TabbingOrder").val();
            var incorrectTags = 0;// $("#IncorrectTags").val();
            var assembly = 0;// $("#Assembly").val();
            var figuresText = 0;// $("#FiguresText").val();
            var figuresMissed = 0;//$("#FiguresMissed").val();
            var headings = 0;// $("#Headings").val();
            var referencesMissed = 0;// $("#ReferencesMissed").val();
            var referencesType = 0;//$("#ReferencesType").val();
            var spaceIssues = 0;//$("#SpaceIssues").val();
            var formFieldProperties = 0;// $("#FormFieldProperties").val();
            var formFieldTooltips = 0;//$("#FormFieldTooltips").val();
            var formFieldType = 0;//$("#FormFieldType").val();
            var artifact = 0;// $("#Artifact").val();
            var tableStructure = 0;// $("#TableStructure").val();
            var listStructure = 0;//$("#ListStructure").val();
            var links = 0;// $("#Links").val();
            var languageAttributes = 0;// $("#LanguageAttributes").val();
            var sideBySide = 0;// $("#SideBySide").val();
            var csr = 0;// $("#CSR").val();
            var documentNaming = 0;// $("#DocumentNaming").val();

            var addComments = 0;

            if (tagOrder == 0 &&
                contentOrder == 0 &&
                tabbingOrder == 0 &&
                incorrectTags == 0 &&
                assembly == 0 &&
                figuresText == 0 &&
                figuresMissed == 0 &&
                headings == 0 &&
                referencesMissed == 0 &&
                referencesType == 0 &&
                spaceIssues == 0 &&
                formFieldProperties == 0 &&
                formFieldTooltips == 0 &&
                formFieldType == 0 &&
                artifact == 0 &&
                tableStructure == 0 &&
                listStructure == 0 &&
                links == 0 &&
                languageAttributes == 0 &&
                sideBySide == 0 &&
                csr == 0 &&
                documentNaming == 0 &&
                comments == "") {
                addComments = 0;
            }
            else {
                addComments = 1;
            }

            var data = new FormData();
            data.append("timer", timer);
            data.append("comments", comments);
            data.append("file", file);
            data.append("jobId", jobId);
            data.append("lastTagger", lastTagger);
            data.append("tagOrder", tagOrder);
            data.append("contentOrder", contentOrder);
            data.append("tabbingOrder", tabbingOrder);
            data.append("incorrectTags", incorrectTags);
            data.append("assembly", assembly);
            data.append("figuresText", figuresText);
            data.append("figuresMissed", figuresMissed);
            data.append("headings", headings);
            data.append("referencesMissed", referencesMissed);
            data.append("referencesType", referencesType);
            data.append("spaceIssues", spaceIssues);
            data.append("formFieldProperties", formFieldProperties);
            data.append("formFieldTooltips", formFieldTooltips);
            data.append("formFieldType", formFieldType);
            data.append("artifact", artifact);
            data.append("tableStructure", tableStructure);
            data.append("listStructure", listStructure);
            data.append("links", links);
            data.append("languageAttributes", languageAttributes);
            data.append("sideBySide", sideBySide);
            data.append("csr", csr);
            data.append("documentNaming", documentNaming);
            data.append("addComments", addComments);
            data.append("flag", $('#flag').val());


            $.ajax({
                type: 'POST',
                url: '/reviewfiles/ReviewCheckin',
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    AjaxState(phaseVal, file);
                },
                error: function () {
                    alert("Failed!");
                }
            });
        }
        else {
            alert('Please upload atleast one file!');
        }
    }

    function CheckInButtonFinalFile(phaseVal) {
        timerStarted = 0;
        if (allowSubmitFlag == 1) {
            reloadPrompt = 1;
            //$('#checkoutTimer').mobiscroll('stop');
            //var timer = $("#checkoutTimer").val();
            var timer = $("#stopWatch").html();
            var comments = $("#PhaseComments").val();
            var file = $("#ID").val();
            var jobId = $("#JobID").val();
            var altTextStatus = $("#altTextStatusID").val();

            $.ajax({
                url: '/finalfiles/FinalFileCheckin',
                data: { File: file, Comments: comments, Job: jobId, AltTextStatus: altTextStatus, Timer: timer },
                type: 'POST'
            }).done(function (responseData) {
                console.log('Done: ', responseData);
                AjaxState(phaseVal, file);
            }).fail(function () {
                console.log('Failed');
            });
        }
        else {
            alert('Please upload atleast one file!');
        }
    }

    function CheckInButtonTag(phaseVal) {
        timerStarted = 0;
        if (allowSubmitFlag == 1) {
            reloadPrompt = 1;

            var timer = $("#stopWatch").html();
            var comments = $("#PhaseComments").val();
            var file = $("#ID").val();
            var altTextStatus = $("#altTextStatusID").val();
            var page = $("#PageSlider").val();
            var assembly = $("#AseemblySlider").val();
            var totalPages = $("#TotalPages").val();
            var flag = $('#flag').val();
            $.ajax({
                url: '/tag/FileCheckin',
                data: { Timer: timer, File: file, Page: page, Assembly: assembly, AltTextStatus: altTextStatus,flag:flag },
                type: 'POST'
            }).done(function (responseData) {
                console.log('Done: ', responseData);
                AjaxState(phaseVal, file);
            }).fail(function () {
                console.log('Failed');
            });
        }
        else {
            alert('Please upload atleast one file!');
        }
    }
}
function AjaxState(state, params) {
    var flag = $('#flag').val();
    $.ajax({
        url: '/qcfiles/AjaxTimeState',
        data: { State: state, Params: params,flag:flag },
        type: 'POST'
    }).done(function (responseData) {
        console.log('Done: ', responseData);
        if (state == "QCTime" || state == "REVIEWTime" || state == "FINALTime" || state == "TAGGINGTime") {
            document.getElementById("stopWatch").innerHTML = responseData.data;
            TimerClockValues();
        }
        else if (state == 'Assign' || state == 'ToBeReviewed' || state == 'ToBeFinalized' || state == 'ToBeQualityControlled' || (state == 'Complete' && pagename == 'qcfiles') || (state == 'Complete' && pagename == 'finalfiles')) {
            if (pagename == 'qcfiles') {
                window.location.href = '/phases/phase3';
            } else if (pagename == 'reviewfiles') {
                window.location.href = '/phases/phase2';
            } else if (pagename == 'finalfiles') {
                window.location.href = '/phases/phase4';
            } else if (pagename == 'tag') {
                window.location.href = '/phases/phase1';
            }

        }
        else if (state == "TimerStartedState") {
            timerStarted = 1;
            $("#start").hide();
            $("#stop").show();
            timer();
        }
        else if (state == "TimerCompareState") {
            document.getElementById("stopWatch").innerHTML = responseData.data;
            TimerClockValues();

            if (fileToCheckin == 1) {
                if (pagename == 'qcfiles') {
                    CheckInButtonQcFile(phaseValGlobal);
                } else if (pagename == 'reviewfiles') {
                    CheckInButtonReview(phaseValGlobal);
                } else if (pagename == 'finalfiles') {
                    CheckInButtonFinalFile(phaseValGlobal);
                }
            }
            //alert(responseData);
        }
        //else if (state == "DownloadFileCheckoutScreen") {
        //    $("#downloadFileCheckoutScreen").html(responseData);
        //    $("#downloadFileCheckoutScreen").trigger("create");
        //    $("#downloadFileCheckoutScreen").trigger("update");
        //    TotalFileVersion();
        //    ChangeCSS();
        //}
    }).fail(function () {
        console.log('Failed');
    });
}

function HidePhasesCheckedOutScreen() {
    $("#phaseButton").hide();
}
function ShowPhasesCheckedOutScreen() {
    $("#phaseButton").show();
}

function UpdateAlt() {
    var altTextStatus = $("#altTextStatusID").val();
    var file = $("#ID").val();
    var params = file + "|" + altTextStatus;
    var flag = $('#flag').val();
    $.ajax({
        type: "POST",
        url: "/qcfiles/UpdateAltText",
        data: { Params: params,flag:flag },
        dataType: "JSON",
        success: function (data) {
            if (altTextStatus == 1 || altTextStatus == 3 || altTextStatus == 4 || altTextStatus == 5 || altTextStatus == 7) {
                $("#phase5Locked").parent().hide();
                $("#phase5LockedMessage").hide();
                $("#phase5Activated").parent().show();
            }
            else {

                $("#phase5Activated").parent().hide();
                $("#phase5Locked").parent().show();
                $("#phase5LockedMessage").show();
            }
        },
        error: function () {
            alert("error");
        }
    });
}

function DeleteUploadFile(ID, row, flag) {

    $.ajax({
        type: "POST",
        url: "/File/DeleteFileVersion",
        data: { ID: ID, flag: flag },
        dataType: "JSON",
        success: function (data) {
            row.hide();
            $('#FirstLevel').DataTable().destroy();
            ToDataTable();

        },
        error: function () {
            alert("error");
        }
    });
}

function CSRSave() {
    var code = $("#hdnCode").val();
    var notes = $("#clientsnotes").val();
    var flag = $('#flag').val();

    $.ajax({
        type: "POST",
        url: "/qcfiles/Updatecsrnotes",
        data: { code: code, notes: notes,flag:flag },
        dataType: "JSON",
        success: function (data) {

        },
        error: function () {
            alert("error");
        }
    });
}

function nonEngagement() {
    stop();
}