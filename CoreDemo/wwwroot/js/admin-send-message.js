$(document).ready(function () {
    $('#Details').summernote({
        height: 300
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
        success: function (func) {
            alert("Mesajınız taslaklara eklendi.");
        },
        error: function (func) {
            if (Comment.CommentUserName == "") {
                alert("Lütfen isim alanını boş bırakmayınız.");
            }
            else if (Comment.CommentTitle == "") {
                alert("Lütfen içerik alanını boş bırakmayınız.")
            }
            else if (Comment.CommentContent == "") {
                alert("Lütfen başlık alanını boş bırakmayınız.");
            }
            else {
                alert("Bir hata oluştu lütfen daha sonra tekrar deneyiniz.");
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
        success: function (func) {
            alert("Mesajınız taslaklara eklendi.");
        },
        error: function (func) {
            if (Comment.CommentUserName == "") {
                alert("Lütfen isim alanını boş bırakmayınız.");
            }
            else if (Comment.CommentTitle == "") {
                alert("Lütfen içerik alanını boş bırakmayınız.")
            }
            else if (Comment.CommentContent == "") {
                alert("Lütfen başlık alanını boş bırakmayınız.");
            }
            else {
                alert("Bir hata oluştu lütfen daha sonra tekrar deneyiniz.");
            }
        }
    });
});