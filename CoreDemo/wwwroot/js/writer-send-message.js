$(document).ready(function () {
    $('#Details').summernote({
        height: 300,
        toolbar: [
            ['style', ['style']],
            ['font', ['fontname', 'fontsize', 'bold', 'italic', 'underline', 'strikethrough', 'clear']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['table', ['table']],
            ['height', ['height']],
            ['insert', ['link', 'picture', 'video']],
            ['view', ['undo', 'redo', 'fullscreen', 'codeview', 'help']],
        ]
    });
});
$(function () {
    $("#Receiver").autocomplete({
        source: "/Message/OnUserNameGet/",
        minLengt: 2
    });
}); 