$('#btn-search').click(function (e) {
    var search = $("#txt-search").val();
    if (search != '') {
        const params = GetURLSearchParams();
        const page = params.page;
        var query = '/Admin/AdminLog/Index?Search=' + search;
        if (page != null) {
            query = `/Admin/AdminLog/Index/?Page=${page}&Search=${search}`;
        }
        window.location = query;
    }
    else {
        Swal.fire(
            'Hata!',
            'Lütfen aranacak kelimeyi giriniz.',
            'error'
        )
    }
});

$("#txt-search").keyup(function (event) {
    if (event.keyCode === 13) {
        $('#btn-search').click();
    }
});

function GetURLSearchParams() {
    return new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
}