$("#btnSubscribe").click(function () {
    let mail = {
        Mail: $("#txtEmail").val()
    };
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
            if (mail.Mail == "") {
                Swal.fire({
                    icon: 'error',
                    title: 'Hata !',
                    text: "Lütfen mail alanını boş bırakmayınız."
                })
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Hata !',
                    text: "Girdiğiniz mail adresinin bültenimize aboneliği bulunmaktadır."
                })
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
            Swal.fire({
                icon: 'success',
                title: 'Başarılı !',
                text: "Mail bültenimize abone oldunuz, teşekkürler."
            })
        },
        error: function (func) {
            if (mail.Mail == "") {
                Swal.fire({
                    icon: 'error',
                    title: 'Hata !',
                    text: "Lütfen mail alanını boş bırakmayınız."
                })
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Hata !',
                    text: "Girdiğiniz mail adresinin bültenimize aboneliği bulunmaktadır."
                })
            }
        }
    });
});  