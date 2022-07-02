$(function(){

   
    var ul = $('#upload ul');

    $('#drop a').click(function(){
        // Simulate a click on the file input button
        // to show the file browser dialog
        $(this).parent().find('input').click();
    });

    // Initialize the jQuery File Upload plugin
    $('#upload').fileupload({
        crossDomain: true,

        // If you need to send along cookies (e.g. for authentication), set the withCredentials $.ajax() setting as fileupload widget option:
        xhrFields: { withCredentials: true },

        // This element will accept file drag/drop uploading
        dropZone: $('#drop'),

        // This function is called when a file is added to the queue;
        // either via the browse button, or via drag/drop:
        add: function (e, data) {

            var tpl = $('<li class="working"><input type="text" value="0" data-width="48" data-height="48"'+' data-fgColor="transparent" data-readOnly="1" data-bgColor="transparent" /><p></p><span></span></li>');

            // Append the file name and file size
            tpl.find('p').text(data.files[0].name).append('<i>' + formatFileSize(data.files[0].size) + '</i>');

            // Add the HTML to the UL element
            data.context = tpl.appendTo(ul);

            // Initialize the knob plugin
            tpl.find('input').knob();

            // Listen for clicks on the cancel icon
            tpl.find('span').click(function(){

                if(tpl.hasClass('working')){
                    jqXHR.abort();
                }

                tpl.fadeOut(function(){
                    tpl.remove();
                });

            });

            // Automatically upload the file once it is added to the queue
            var jqXHR = data.submit();
        },

        progress: function(e, data){

            // Calculate the completion percentage of the upload
            var progress = parseInt(data.loaded / data.total * 100, 10);

            // Update the hidden input field and trigger a change
            // so that the jQuery knob plugin knows to update the dial
            data.context.find('input').val(progress).change();

            if(progress == 100){
                data.context.removeClass('working');
            }
        },

        fail:function(e, data){
            // Something has gone wrong!
            data.context.addClass('error');
            data.context.find('p').append(data.errorThrown);
        },

        done: function (e, data) {
            console.log(data);
            data.context.addClass('success');
            data.context.find('p').append('Complete');
            if (data.result.status == "sourceFile" || data.result.status == "otherFile" || data.result.status == "referenceFile") {
                
                $("#filesRefresh").html(data.result.html);

                sourceDatatable();

                fileTotalPageCount();

                
                SourceFileUpload();

                ChangeCSS();
                
                
                $.getScript("/js/upload.js");
                

                if (data.result.status == "sourceFile") {
                    msg = "Source File Uploaded successfully.";
                } else if(data.result.status == "otherFile"){
                    msg = "Other File Uploaded successfully.";
                } else if(data.result.status == "referenceFile"){
                    msg = "Reference File Uploaded successfully.";
                }

                toastr.success(msg);
            }
            
            if(document.getElementById("pageName"))
            {
                var pageName = document.getElementById("pageName").value;
                if(pageName == "jobinitial")
                {
                    RecentFileUpload(data.files[0].name);
                }
                else if(pageName == "tag" || pageName == "reviewfiles" || pageName == "finalfiles" || pageName == "qcfiles")
                {
                    DownloadFileBoxReload();
                }
            }
        }


});


    // Prevent the default action when a file is dropped on the window
    $(document).on('drop dragover', function (e) {
        e.preventDefault();
    });

    // Helper function that formats the file sizes
    function formatFileSize(bytes) {
        if (typeof bytes !== 'number') {    return ''; }
        if (bytes >= 1000000000) {          return (bytes / 1000000000).toFixed(2) + ' GB'; }
        if (bytes >= 1000000) {             return (bytes / 1000000).toFixed(2) + ' MB';    }
        return (bytes / 1000).toFixed(2) + ' KB';
    }

});






