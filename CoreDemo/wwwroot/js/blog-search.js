$('#search-btn').click(function (e) {
    var search = $("#txt-search").val();
    var query = '/Blog/Index?Search=' + search;
    console.log(query);
    window.location = query;
}); 