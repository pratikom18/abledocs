var counter = $("#addition_phone_counter").val();
var counter1 = $("#addition_email_counter").val();
//debugger;
var clonephone = function (thisObj) {

    $(".phone-select2").select2("destroy");
   // $('.select-dropdown').selectpicker('setStyle', '');
    counter++;
    var parentObj = $(thisObj).parents('.clonephone');
    var cloneObj = parentObj.clone();

    cloneObj.find('.phone1').attr("id", "TypeFilter_" + counter + "_Phone1");
    cloneObj.find('.phone1').attr("name", "TypeFilter[" + counter + "][Phone1]");
    cloneObj.find('.phone1').val("");

    cloneObj.find('.PhoneType').attr("id", "TypeFilter_" + counter + "_PhoneType");
    cloneObj.find('.PhoneType').attr("name", "TypeFilter[" + counter + "][PhoneType]");
    cloneObj.find('.PhoneType').val("");


    if (counter > 0) {
        cloneObj.find(".addBtn").hide();
        cloneObj.find(".removeBtn").show();
    }
    $(".clonephone:last").after(cloneObj);
    $('.select-dropdown').selectpicker('setStyle', 'btn btn-link');
    $('.filter-option').addClass('filter-option-1');
    
    $(".PhoneType").select2({
        placeholder: "Select",
        theme: "material"
    });
   
    $(".select2-selection__arrow")
        .addClass("material-icons")
        .html("arrow_drop_down");

   // $(".select2-container").css("width", "auto");

}
var removeContact = function (thisObj) {
    counter--;
    $(thisObj).parents('.clonephone').remove();
}


var cloneemail = function (thisObj) {

    $(".email-select2").select2("destroy");
    // $('.select-dropdown').selectpicker('setStyle', '');
    counter1++;
    var parentObj = $(thisObj).parents('.cloneemail');
    var cloneObj = parentObj.clone();

    cloneObj.find('.Email1').attr("id", "TypeEmail_" + counter1 + "_Email1");
    cloneObj.find('.Email1').attr("name", "TypeEmail[" + counter1 + "][Email1]");
    cloneObj.find('.Email1').val("");

    cloneObj.find('.EmailType').attr("id", "TypeEmail_" + counter1 + "_EmailType");
    cloneObj.find('.EmailType').attr("name", "TypeEmail[" + counter1 + "][EmailType]");
    cloneObj.find('.EmailType').val("");


    if (counter1 > 0) {
        cloneObj.find(".addBtn").hide();
        cloneObj.find(".removeBtn").show();
    }
    $(".cloneemail:last").after(cloneObj);
    $('.select-dropdown').selectpicker('setStyle', 'btn btn-link');
    $('.filter-option').addClass('filter-option-1');

    $(".EmailType").select2({
        placeholder: "Select",
        theme: "material"
    });

    $(".select2-selection__arrow")
        .addClass("material-icons")
        .html("arrow_drop_down");

    // $(".select2-container").css("width", "auto");

}
var removeemail = function (thisObj) {
    counter1--;
    $(thisObj).parents('.cloneemail').remove();
}


$(document).ready(function () {

    var stage = $('#stage').val();
    var id = $('#' + stage).attr("data-id");
       
    $('label.pipelinestage').each(function (index) {
        var i = $(this).attr('data-id');

        if (i > id) {
            $(this).css("background-color", "#61C786");
        } else {
            $(this).css("background-color", "#08a742");

        }
    });

    //$('.select2').select2();
    $(document).on("change", "#clientid", function () {
        clientID = $(this).val();
        $('.contact-list').load('/adsales/contactdropdown?id=' + clientID+"&flag="+$('#flag').val(), function () {
            //$('.select2').select2();
            $('#contactid').selectpicker('setStyle', 'btn btn-link');
        });

        $('.currency-list').load('/adsales/currencydropdown?id=' + clientID, function () {
            //$('.select2').select2();
            $('#currency').selectpicker('setStyle', 'btn btn-link');
        });
    });


    $(document).on("change", "#pipeline", function () {
        clientID = $(this).val();
        $('.Pipeline_stage').load('/adsales/pipelinestage?typecode=' + clientID, function () {
            //$('.select2').select2();
            $('#stage').val($('#hdnstage').val());
        });
    });

    $(document).on("click", ".pipelinestage", function () {
       var id = $(this).attr("data-id");
        var stage = $(this).attr("id");
        $('#stage').val(stage);
        $('label.pipelinestage').each(function (index) {
            var i = $(this).attr('data-id');
            
            if (i > id) {
                $(this).css("background-color", "#61C786");
            } else {
                $(this).css("background-color", "#08a742");
                
            }
        });
    });

    $('.pipelinestage').tooltip({
        content: function (result) {
            result('<p>' + $(this).attr("data-value") + '</p>')
        },
        classes: {
            "ui-tooltip": "highlight"
        },
        //show: "slideDown",
        position: { my: 'left center', at: 'right+10 center' },
        open: function (event, ui) {
            ui.tooltip.hover(function () {
                $(this).fadeTo("slow", 0.5);
            });
        }
    });
});







