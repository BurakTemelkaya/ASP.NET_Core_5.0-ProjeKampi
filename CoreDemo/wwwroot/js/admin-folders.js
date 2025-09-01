// Bu dosyadaki tüm değişkenleri en üstte tanımlayın.
let unreadMessageCount = 0;

// Mesaj sayısını getiren ana fonksiyon
function GetUnreadMessageCount() {
    $.ajax({
        url: '/Admin/AdminMessage/GetUnreadMessagesCount',
        type: 'GET',
        success: function (data) {
            if (data && data != '0') {
                unreadMessageCount = data;
            } else {
                unreadMessageCount = 0;
            }
            // Sayı alındıktan sonra arayüzü güncelleyen fonksiyonları çağırın
            UpdateMessageUI();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            // Hata durumunu konsola yazdırarak sorunun ne olduğunu anlayın.
            console.error("GetUnreadMessagesCount AJAX Hatası:", textStatus, errorThrown);
        }
    });
}

// Taslak sayısını getiren fonksiyon
function GetMessageDraftListForMessageFolder() {
    $.ajax({
        url: '/Admin/AdminMessage/GetDraftMessagesCount',
        type: 'GET',
        success: function (data) {
            let badgeHtml = '<i class="fa fa-file-text-o "></i> Taslaklar';
            if (data && data != '0') {
                badgeHtml += '<span class="label label-danger float-right">' + data + '</span>';
            }
            // HTML'i güncelleyin
            $("#messageFolderDraftBadge").html(badgeHtml);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("GetDraftMessagesCount AJAX Hatası:", textStatus, errorThrown);
        }
    });
}

// Arayüzü güncelleyen tüm fonksiyonları bir araya toplayın
function UpdateMessageUI() {
    // Dropdown için
    let dropdownBadgeHtml = '<i class="fa fa-envelope"></i>';
    if (unreadMessageCount > 0) {
        dropdownBadgeHtml += '<span class="label label-warning">' + unreadMessageCount + '</span>';
    }
    $("#UnreadMessagesCountBadge").html(dropdownBadgeHtml);

    // Mesaj klasörü için
    let folderBadgeHtml = '<i class="fa fa-inbox "></i> Gelen Mesajlar ';
    if (unreadMessageCount > 0) {
        folderBadgeHtml += '<span class="label label-warning float-right">' + unreadMessageCount + '</span>';
    }
    $("#messageFolderBadge").html(folderBadgeHtml);
}


// Sayfa yüklendiğinde SADECE BİR KERE çalışacak olan ana blok
$(document).ready(function () {
    // Sayfa hazır olduğunda ilk verileri çek
    GetUnreadMessageCount();
    GetMessageDraftListForMessageFolder();
});