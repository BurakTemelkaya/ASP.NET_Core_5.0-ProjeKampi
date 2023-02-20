$("#btnSubscribe").click(function () {
    let mail = {
        Mail: $("#txtEmail").val()
    };
    $.ajax({
        type: "post",
        url: "/NewsLetter/SubscribeMail",
        data: mail,
        success: function (func) {
            alert("Mail bültenimize abone oldunuz, teşekkürler.");
        },
        error: function (func) {
            if (mail.Mail == "") {
                alert("Lütfen mail alanını boş bırakmayınız.");
            }
            else {
                alert("Girdiğiniz mail adresinin bültenimize aboneliği bulunmaktadır.");
            }
        }
    });
});
$("#btnFooterSubscribe").click(function () {
    let mail = {
        Mail: $("#txtFooterEmail").val()
    };
    $.ajax({
        type: "post",
        url: "/NewsLetter/SubscribeMail",
        data: mail,
        success: function (func) {
            alert("Mail bültenimize abone oldunuz, teşekkürler.");
        },
        error: function (func) {
            if (mail.Mail == "") {
                alert("Lütfen mail alanını boş bırakmayınız.");
            }
            else {
                alert("Girdiğiniz mail adresinin bültenimize aboneliği bulunmaktadır.");
            }
        }
    });
});  