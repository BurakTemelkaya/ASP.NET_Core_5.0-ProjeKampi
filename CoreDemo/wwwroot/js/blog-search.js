$('#search-btn').click(function (e) {
    var search = $("#txt-search").val();
    if (search!='') {
        var query = '/Blog/Index?Search=' + search;
        window.location = query;
    }
});

$("#txt-search").keyup(function (event) {
    if (event.keyCode === 13) {
        $('#search-btn').click();
    }
});