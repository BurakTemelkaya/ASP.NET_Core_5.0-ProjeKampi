$('.Sil').click(function (e) {
    e.preventDefault();
    var linkURL = $(this).attr("href");
    Swal.fire({
        title: 'UYARI!',
        text: "Kategoriyi silmek istediğinize emin misiniz?",
        icon: 'warning',
        showCancelButton: true,
        showCancelButton: true,
        confirmButtonColor: '#5CBA6C',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet',
        cancelButtonText: 'Hayır'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = linkURL;
            Swal.fire(
                'Başarılı!',
                'Seçilen Mesaj Silindi .',
                'success'
            )
        }
    })
});
$('.DurumAktif').click(function (e) {
    e.preventDefault();
    var linkURL = $(this).attr("href");
    Swal.fire({
        title: 'UYARI!',
        text: 'Kategoriyi Aktif Hale Getirmek İstediğinizden Emin Misiniz ?',
        icon: 'warning',
        showCancelButton: true,
        showCancelButton: true,
        confirmButtonColor: '#5CBA6C',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet',
        cancelButtonText: 'Hayır'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = linkURL;
            Swal.fire(
                'Başarılı!',
                onayMesaji,
                'success'
            )
        }
    })
});
$('.DurumPasif').click(function (e) {
    e.preventDefault();
    var linkURL = $(this).attr("href");
    Swal.fire({
        title: 'UYARI!',
        text: 'Kategoriyi Pasif Hale Getirmek İstediğinizden Emin Misiniz ?',
        icon: 'warning',
        showCancelButton: true,
        showCancelButton: true,
        confirmButtonColor: '#5CBA6C',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet',
        cancelButtonText: 'Hayır'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = linkURL;
            Swal.fire(
                'Başarılı!',
                onayMesaji,
                'success'
            )
        }
    })
});
