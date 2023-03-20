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
    });
});

$("#btnSaveDraft").click(function () {
    let messageDraft = {
        Subject: $("#Subject").val(),
        Details: $("#Details").val(),
        ReceiverUser: $("#Receiver").val(),
    };
    $.ajax({
        type: "post",
        url: "/Admin/AdminMessageDraft/Add",
        data: messageDraft,
        success: function () {
            Swal.fire(
                'Başarılı!',
                'Mesajınız taslaklara eklendi.',
                'success'
            ); 
        },
        error: function () {
            if (Comment.CommentUserName == "") {
                Swal.fire(
                    'Uyarı!',
                    'Lütfen isim alanını boş bırakmayınız.',
                    'error'
                );            
            }
            else if (Comment.CommentTitle == "") {
                Swal.fire(
                    'Uyarı!',
                    'Lütfen içerik alanını boş bırakmayınız.',
                    'error'
                );
            }
            else if (Comment.CommentContent == "") {
                Swal.fire(
                    'Uyarı!',
                    'Lütfen başlık alanını boş bırakmayınız.',
                    'error'
                );
            }
            else {
                Swal.fire(
                    'Uyarı!',
                    'Bir hata oluştu lütfen daha sonra tekrar deneyiniz.',
                    'error'
                );
            }
        }
    });
});

$("#btnSaveDraft2").click(function () {
    let messageDraft = {
        Subject: $("#Subject").val(),
        Details: $("#Details").val(),
        ReceiverUser: $("#Receiver").val(),
    };
    $.ajax({
        type: "post",
        url: "/Admin/AdminMessageDraft/Add",
        data: messageDraft,
        success: function () {
            Swal.fire(
                'Başarılı!',
                'Mesajınız taslaklara eklendi.',
                'success'
            ); 
        },
        error: function () {
            if (Comment.CommentUserName == "") {
                Swal.fire(
                    'Uyarı!',
                    'Lütfen isim alanını boş bırakmayınız.',
                    'error'
                );
            }
            else if (Comment.CommentTitle == "") {
                Swal.fire(
                    'Uyarı!',
                    'Lütfen içerik alanını boş bırakmayınız.',
                    'error'
                );
            }
            else if (Comment.CommentContent == "") {
                Swal.fire(
                    'Uyarı!',
                    'Lütfen başlık alanını boş bırakmayınız.',
                    'error'
                );
            }
            else {
                Swal.fire(
                    'Uyarı!',
                    'Bir hata oluştu lütfen daha sonra tekrar deneyiniz.',
                    'error'
                );
            }
        }
    });
});