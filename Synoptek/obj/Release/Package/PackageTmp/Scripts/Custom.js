//Common Function to show error message
function ShowErrorMessage(errMessage) {
    var isDebugging = false;
    if (isDebugging)
        bootbox.alert(errMessage);
}
//Common Function to log ajax call error message
function LogError(errorCode, errMessage, actionPath) {
    $.ajax({
        url: "/HomePage/Error",
        data: { errorCode: errorCode, errMessage: errMessage, actionPath: actionPath },
        cache: false,
        success: function (result) {

        }
    });
}

$(document).ready(function () {

    $(document).ajaxError(function (event, request, settings) {

        //alert(new Error().stack);
        var errorCode = request.status;
        var errMessage = request.statusText;
        var actionPath = event.currentTarget.location.pathname;

        if (request.status == 404) {
            errMessage = "File " + settings.url + " " + errMessage;
        }
        bootbox.alert(errMessage);
        LogError(errorCode, errMessage, actionPath);

    });

});
