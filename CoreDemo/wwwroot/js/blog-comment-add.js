var blogId = 0;

$("#btnSendComment").click(function () {
    var captcharesponse = grecaptcha.getResponse();

    let BlogScore = 0;
    if (document.getElementById("5-star-rating").checked) {
        BlogScore = 5;
    }
    else if (document.getElementById("4-star-rating").checked) {
        BlogScore = 4;
    }
    else if (document.getElementById("3-star-rating").checked) {
        BlogScore = 3;
    }
    else if (document.getElementById("2-star-rating").checked) {
        BlogScore = 2;
    }
    else if (document.getElementById("1-star-rating").checked) {
        BlogScore = 1;
    }
    const Comment = {
        BlogId: $("#blogId").val(),
        CommentUserName: $("#CommentUserName").val(),
        CommentTitle: $("#CommentTitle").val(),
        CommentContent: $("#CommentContent").val(),
        BlogScore: BlogScore
    };

    var text = "";
    if (Comment.CommentUserName == "") {
        text += 'Lütfen isim alanını boş bırakmayınız.<br/>';
    }
    else if (Comment.CommentUserName.length < 5) {
        text += 'İsim 5 karekterden az olamaz.<br/>';
    }
    if (Comment.CommentTitle == "") {
        text += 'Lütfen başlık alanını boş bırakmayınız.<br/>';
    }
    else if (Comment.CommentTitle.length < 5) {
        text += 'Başlık 5 karekterden az olamaz.<br/>';
    }
    if (Comment.CommentContent == "") {
        text += 'Lütfen içerik alanını boş bırakmayınız.<br/>';
    }
    else if (Comment.CommentContent.length < 10) {
        text += 'İçerik 10 karekterden az olamaz.<br/>';
    }
    if (BlogScore == 0) {
        text += 'Lütfen puan seçiniz.<br/>';
    }
    if (captcharesponse == "") {
        text += 'Lütfen doğrulamayı yapınız.<br/>';
    }

    if (text != "") {
        Swal.fire({
            icon: 'error',
            title: 'Hata !',
            html: text
        });
    }
    else {
        $.ajax({
            type: "post",
            url: "/Comment/PartialAddComment",
            data: {
                comment: Comment, blogId: blogId, captcharesponse: captcharesponse
            },
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Başarılı !',
                    text: "Yorumunuz başarıyla gönderildi, teşekkürler.."
                }).then(function () {
                    ReloadEvents(blogId);
                });
            },
            error: function (func) {

                if (func.responseText == "Recaptcha error.") {
                    Swal.fire({
                        icon: 'error',
                        title: 'Hata !',
                        text: "Lütfen doğrulamayı yapınız."
                    });
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Hata !',
                        text: "Bir hata oluştu lütfen daha sonra tekrar deneyiniz."
                    });
                }
            }
        });

        grecaptcha.reset();

        $("#blogId").val("");
        $("#CommentUserName").val("");
        $("#CommentTitle").val("");
        $("#CommentContent").val("");
    }
});

function ReloadEvents(id) {
    $.ajax({
        url: `/Comment/RefreshCommentListByBlogComponent?id=${id}`,
        success: function (data) {
            $('#comments').html(data);
            window.location.hash = '#comment-top';
        }
    })
} 