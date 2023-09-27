
async function GetMessageList() {
    $(document).ready(function () {
        $.ajax({
            url: '/Message/GetMessageList',
            type: "GET",
            data: { search: $('#search').val() },
            success: function (data) {
                let jsonData = jQuery.parseJSON(data);
                let tablehtml = `<table class="table table-responsive col-12">
                <thead>
                <tr>
                    <th scope="row">#</th>
                    <th class="col-2">Konu</th>
                    <th class="col-2">Gönderen</th>
                    <th class="col-3">İçerik</th>
                    <th class="col-1">Tarih</th>
                    <th class="col-1">Mesajı Aç</th>
                    <th class="col-2">Okundu/Okunmadı Olarak İşaretle</th>
                </tr>
                </thead >
                <tbody>`;
                if (jsonData != "") {
                    $.each(jsonData, (index, item) => {
                        var message = `${item.Message}`;
                        if (message.length > 75) {
                            message = message.substring(0, 75) + "...";
                        }
                        var isRead = `${item.MessageStatus}`;
                        var isReadClass = "";
                        var read = "";
                        var isReadTitle = "";
                        if (isRead == "false") {
                            isReadClass = "fa-envelope-open text-white";
                            read = "bg-secondary";
                            isReadTitle = "Okundu olarak işaretle";
                        }
                        else {
                            isReadClass = "fa-envelope text-dark";
                            read = "";
                            isReadTitle = "Okunmadı olarak işaretle";
                        }

                        var date = moment(`${item.MessageDate}`).format('DD-MM-YYYY hh:mm');

                        tablehtml += `<tr class="${read}">
                <td scope="row">
                <div class="float-right">
                      <input type="checkbox" class="mail-check form-check-input" id="${item.MessageID}"> 
                </div>
                </td>              
                <td>${item.Subject}</td>
                <td>${item.SenderNameSurname} </td>
                <td>${item.Details}</td>
                <td>${date}</td>
                <td><a href="/Message/MessageDetails/${item.MessageID}" class="btn btn-primary">Mesajı Aç</a></td>
                <td>
                <i class="fa ${isReadClass} pointer col-12" onclick="MarkChangedMessage(${item.MessageID})" title="${isReadTitle}"></i>
                </td>
            </tr>`;
                    });
                }
                else {
                    tablehtml += `<tr class="read">
                                <td>Aramanızla eşleşen mesajınız bulunmamaktadır.</td>
                            </tr>`;
                }
                tablehtml += '</tbody></table>';
                $("#mail-box").html(tablehtml);
                GetMessageListForDropDown();
                GetMessageListForMessageFolder();
            }
        });

    });
};

var isClickCheckboxToggle;

$('.checkbox-toggle').click(function () {
    var clicks = $(this).data('clicks')
    if (clicks) {
        AllCheckboxSetUnchecked();
    } else {
        AllCheckboxSetChecked();
    }
    isClickCheckboxToggle = clicks;
    $(this).data('clicks', !clicks)
});

function AllCheckboxSetChecked() {
    $('.mail-check').prop('checked', true);
    $('.mdi.mdi-checkbox-blank-outline').removeClass('mdi mdi-checkbox-blank-outline').addClass('mdi mdi-checkbox-marked');
}

function AllCheckboxSetUnchecked() {
    $('.mail-check').prop('checked', false);
    $('.mdi.mdi-checkbox-marked').removeClass('mdi mdi-checkbox-marked').addClass('mdi mdi-checkbox-blank-outline');
}

$(document).ready(function () {
    $("#btnReadMessages").click(function () {
        var selected = [];
        $('input:checked').each(function () {
            selected.push($(this).attr("id"));
        });

        $.ajax({
            type: 'POST',
            url: '/Message/MarkReadMessages',
            data: { selectedItems: selected },
            success: function () {
                GetMessageList();
                if (!isClickCheckboxToggle) {
                    AllCheckboxSetUnchecked();
                    $('#select-all-checkbox').click();
                }
                WriterMessageNotification();
            }
        });
    });
});

function MarkChangedMessage(id) {
    $.ajax({
        type: 'POST',
        url: '/Message/MarkChanged',
        data: { id: id },
        success: function (data) {
            GetMessageList();
            WriterMessageNotification();
        }
    });
};

$(document).ready(function () {
    $("#btnUnreadMessages").click(function () {
        var selected = [];
        $('input:checked').each(function () {
            selected.push($(this).attr("id"));
        });

        $.ajax({
            type: 'POST',
            url: '/Message/MarkUnreadMessages',
            data: { selectedItems: selected },
            success: function () {
                GetMessageList();
                if (!isClickCheckboxToggle) {
                    AllCheckboxSetUnchecked();
                    $('#select-all-checkbox').click();
                    WriterMessageNotification();
                }
            }
        });
    });
});

$(document).ready(function () {
    $("#btnDeleteMessages").click(function () {
        Swal.fire({
            title: 'UYARI!',
            text: "Seçilen mesajları silmek istediğinize emin misiniz?",
            icon: 'warning',
            showCancelButton: true,
            showCancelButton: true,
            confirmButtonColor: '#5CBA6C',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet',
            cancelButtonText: 'Hayır'
        }).then((result) => {
            if (result.isConfirmed) {
                var selected = [];
                $('input:checked').each(function () {
                    selected.push($(this).attr("id"));
                });
                $.ajax({
                    type: 'POST',
                    url: '/Message/DeleteMessages',
                    data: { selectedItems: selected },
                    success: function () {
                        GetMessageList();
                        if (!isClickCheckboxToggle) {
                            AllCheckboxSetUnchecked();
                            $('#select-all-checkbox').click();
                            WriterMessageNotification();
                        }
                    }
                });
            };
        });
    });
});

function GetMessageListForDropDown() {
    $(document).ready(function () {
        $.ajax({
            url: '/Message/GetUnreadMessagesCount',
            type: "GET",
            success: function (data) {
                let badgeHtml = '<i class="fa fa-envelope">';
                if (data != '0') {
                    badgeHtml += '<span class="label label-warning">' + data + '</span>';
                }
                badgeHtml += '</i>';
                $("#UnreadMessagesCountBadge").html(badgeHtml);
            }
        });
    });
};

function GetMessageListForMessageFolder() {
    $(document).ready(function () {
        $.ajax({
            url: '/Admin/AdminMessage/GetUnreadMessagesCount',
            type: "GET",
            success: function (data) {
                let badgeHtml = '<i class="fa fa-inbox "></i> Gelen Mesajlar ';
                if (data != '0') {
                    badgeHtml += '<span class="label label-warning float-right">' + data + '</span>';
                }
                badgeHtml += '</i>';
                $("#messageFolderBadge").html(badgeHtml);
            }
        });
    });
};

function GetMessageDraftListForMessageFolder() {
    $(document).ready(function () {
        let badgeHtml = '<i class="fa fa-file-text-o"> Taslaklar </i>';
        $.ajax({
            url: '/Admin/AdminMessage/GetDraftMessagesCount',
            type: "GET",
            success: function (data) {
                if (data != '0') {
                    badgeHtml += '<span class="label label-danger float-right">' + data + '</span>';
                }
            }
        });
        $("#messageFolderDraftBadge").html(badgeHtml);
    });
};