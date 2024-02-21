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
function SubmitFiles() {
    var formData = new FormData($('#uploadForm')[0]);
    $.ajax({
        url: '/Upload/UploadFiles',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            $('#content-main').html(data);
        },
        error: function (xhr, status, error) {
            var errorMessage;
            if (xhr.status === 413) {
                errorMessage = "File(s) too large. Please try again.";
            } else {
                errorMessage = "An error occurred while uploading files. Please try again later.";
            }
            $('#content-main').html('<div class="alert alert-danger">' + errorMessage + '</div>');
        }
    });
    return false;
}
