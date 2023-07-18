$('#search-btn').click(function (e) {
    var search = $("#txt-search").val();
    if (search != '') {
        const params = GetURLSearchParams();
        const id = params.id;
        var query = '/Blog/Index?Search=' + search;
        if (id != null) {
            query = `/Blog/Index/?id=${id}&Search=` + search;
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
        $('#search-btn').click();
    }
});

$(".categoryLink").click(function (e) {

    e.preventDefault();

    const oldParams = GetURLSearchParams();

    const search = oldParams.Search;

    var linkURL = $(this).attr("href");

    const id = $(this).attr("name");;

    if (search != null) {
        linkURL = `/Blog/Index/?id=${id}&Search=` + search;
    }

    window.location.href = linkURL;

});

function GetURLSearchParams(link) {
    return new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
}