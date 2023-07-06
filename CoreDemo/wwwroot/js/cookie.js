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

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}