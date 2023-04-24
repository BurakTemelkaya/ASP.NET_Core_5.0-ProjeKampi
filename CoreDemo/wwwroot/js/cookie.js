function SetThema() {
    var checkBox = document.getElementById("thema-toggle");
    if (checkBox.checked == true) {
        $.ajax({
            url: '/Thema/Set',
            type: "POST",
            data: { data: "dark" },
            success: function () {
                setTimeout(() => {
                    document.location.reload();
                }, 100);
            },
        });
    }
    else {
        $.ajax({
            url: '/Thema/Set',
            type: "POST",
            data: { data: "light" },
            success: function () {
                setTimeout(() => {
                    document.location.reload();
                }, 100);
            }
        });
    }


}