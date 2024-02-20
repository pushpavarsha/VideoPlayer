function playVideo(fileName) {
    var videoPlayer = document.getElementById("videoPlayer");
    videoPlayer.src = "/Media/" + encodeURIComponent(fileName);
    videoPlayer.play();
}