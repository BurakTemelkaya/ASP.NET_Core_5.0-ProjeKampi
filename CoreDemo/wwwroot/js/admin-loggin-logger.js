$('#btn-search').click(function (e) {
    var search = $("#txt-search").val();
    if (search != '') {
        const params = GetURLSearchParams();
        const page = params.page;
        var query = '/Admin/AdminLoginLogger/Index?Page=1&userName=' + search;
        window.location = query;
    }
    else {
        Swal.fire(
            'Hata!',
            'Lütfen kayıtları aranacak kullanıcının adını giriniz.',
            'error'
        )
    }
});

$("#txt-search").keyup(function (event) {
    if (event.keyCode === 13) {
        $('#btn-search').click();
    }
});

$(function () {
    $("#txt-search").autocomplete({
        source: "/Message/OnUserNameGet/",
    });
});

function GetURLSearchParams() {
    return new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
}