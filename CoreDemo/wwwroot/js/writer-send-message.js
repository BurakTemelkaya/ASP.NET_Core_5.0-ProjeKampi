$(document).ready(function () {
    $('#Details').summernote({
        height: 300
    });
});
$(function () {
    $("#Receiver").autocomplete({
        source: "/Message/OnUserNameGet/",
        minLengt: 3
    });
}); 