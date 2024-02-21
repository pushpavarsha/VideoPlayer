function playVideo(fileName) {
    var videoPlayer = document.getElementById("videoPlayer");
    videoPlayer.src = "/Media/" + encodeURIComponent(fileName);
    videoPlayer.play();
}

function loadView(viewName) {
    $.ajax({
        url: '/Upload/' + viewName,
        type: 'GET',
        data: { partial: true }, 
        success: function (data) {
            $('#content-main').html(data);
        },
        error: function () {
            alert('Failed to load view ' + viewName);
        }
    });
}

function SubmitFiles(viewName) {
    var formData = new FormData($('#uploadForm')[0]);
    $.ajax({
        url: '/Upload/' + viewName,
        type: 'POST',
        data: { videoFiles: formData},
        success: function (data) {
            $('#content-main').html(data);
        },
        error: function (xhr, status, error) {
            if (xhr.status === 413) {
                var errorMessage = "File(s) too large. Please try again.";
                displayModelError(errorMessage);
            } else {
                var errorMessage = "An error occurred while uploading files. Please try again later.";
                displayModelError(errorMessage);
            }
        }
    });
    return false;
}
function displayModelError(errorMessage) {
    $('.alert-danger').html('<span><i><b>' + errorMessage + '</b></i></span><br>');
    $('.alert-danger').show();
}