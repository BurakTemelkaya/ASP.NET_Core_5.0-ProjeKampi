var blogId = 0;

$("#btnSendComment").click(function () {
    var captcharesponse = grecaptcha.getResponse();

    let BlogScore = 0;
    if (document.getElementById("5-star-rating").checked == true) {
        BlogScore = 5;
    }
    else if (document.getElementById("4-star-rating").checked == true) {
        BlogScore = 4;
    }
    else if (document.getElementById("3-star-rating").checked == true) {
        BlogScore = 3;
    }
    else if (document.getElementById("2-star-rating").checked == true) {
        BlogScore = 2;
    }
    else if (document.getElementById("1-star-rating").checked == true) {
        BlogScore = 1;
    }
    let Comment = {
        BlogId: $("#blogId").val(),
        CommentUserName: $("#CommentUserName").val(),
        CommentTitle: $("#CommentTitle").val(),
        CommentContent: $("#CommentContent").val(),
        BlogScore: BlogScore
    };
    $.ajax({
        type: "post",
        url: "/Comment/PartialAddComment",
        data: {
            comment: Comment, blogId: blogId, captcharesponse: captcharesponse
        },
        success: function () {
            alert("Yorumunuz başarıyla gönderildi teşekkürler..");            
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
    grecaptcha.reset();
});       