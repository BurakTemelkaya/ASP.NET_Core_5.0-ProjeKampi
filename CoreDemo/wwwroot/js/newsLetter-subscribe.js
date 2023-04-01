
function validateEmail($email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test($email);
}

$("#btnSubscribe").click(function () {
    let mail = {
        Mail: $("#txtEmail").val()
    };
    var errorMessage = "";
    if (mail.Mail == "") {
        errorMessage = "Lütfen mail alanını boş bırakmayınız.";
    }
    else if (!validateEmail(mail.Mail)) {
        errorMessage = "Lütfen geçerli bir mail adresi giriniz.";
    }

    if (errorMessage != "") {
        Swal.fire({
            icon: 'error',
            title: 'Hata !',
            text: errorMessage
        })
    }
    else {
        $.ajax({
            type: "post",
            url: "/NewsLetter/SubscribeMail",
            data: mail,
            success: function (func) {
                Swal.fire({
                    icon: 'success',
                    title: 'Başarılı !',
                    text: "Mail bültenimize abone oldunuz, teşekkürler."
                })
            },
            error: function (func) {
                Swal.fire({
                    icon: 'error',
                    title: 'Hata !',
                    text: func.responseText
                })
            }
        });
    }

});
$("#btnFooterSubscribe").click(function () {
    let mail = {
        Mail: $("#txtFooterEmail").val()
    };

    var errorMessage = "";
    if (mail.Mail == "") {
        errorMessage = "Lütfen mail alanını boş bırakmayınız.";
    }
    else if (!validateEmail(mail.Mail)) {
        errorMessage = "Lütfen geçerli bir mail adresi giriniz.";
    }

    if (errorMessage != "") {
        Swal.fire({
            icon: 'error',
            title: 'Hata !',
            text: errorMessage
        })
    }
    else {
        $.ajax({
            type: "post",
            url: "/NewsLetter/SubscribeMail",
            data: mail,
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Başarılı !',
                    text: "Mail bültenimize abone oldunuz, teşekkürler."
                })
            },
            error: function (func) {
                Swal.fire({
                    icon: 'error',
                    title: 'Hata !',
                    text: func.responseText
                });
            }
        });
    }
});  