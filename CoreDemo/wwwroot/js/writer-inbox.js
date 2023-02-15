
function GetMessageList() {
    $.ajax({
        url: '/Message/GetMessageList',
        type: "GET",
        data: { search: $('#search').val() },
        success: function (data) {
            let jsonData = jQuery.parseJSON(data);
            let tablehtml = `<table class="table">
                <thead>
                <tr>
                    <th scope="row">#</th>
                    <th>Konu</th>
                    <th>Gönderen</th>
                    <th>İçerik</th>
                    <th>Tarih</th>
                    <th>Mesajı Aç</th>
                    <th>Okundu/Okunmadı Olarak İşaretle</th>
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
                        isReadClass = "fa-envelope-open";
                        read = "bg-secondary";
                        isReadTitle = "Okundu olarak işaretle";
                    }
                    else {
                        isReadClass = "fa-envelope";
                        read = "";
                        isReadTitle = "Okunmadı olarak işaretle";
                    }

                    var date = moment(`${item.MessageDate}`).format('DD-MM-YYYY hh:mm');

                    tablehtml += `<tr class="${read}">
                <td scope="row">
                         <div class="form-check">
                           <label class="form-check-label">
                             <input type="checkbox" class="form-check-input mail-check" id="${item.MessageID}"> </label>
                        </div>
                </td>
                <td>${item.Subject}</td>
                <td>${item.SenderUser.NameSurname} </td>
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
};


GetMessageList();

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
            url: '/Admin/AdminMessage/MarkReadMessages',
            data: { selectedItems: selected },
            success: function () {
                GetMessageList();
                if (!isClickCheckboxToggle) {
                    AllCheckboxSetUnchecked();
                    $('#select-all-checkbox').click();
                }
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
            url: '/Admin/AdminMessage/MarkUnreadMessages',
            data: { selectedItems: selected },
            success: function () {
                GetContactList();
                if (!isClickCheckboxToggle) {
                    AllCheckboxSetUnchecked();
                    $('#select-all-checkbox').click();
                }
            }
        });
    });
});

$(document).ready(function () {
    $("#btnDeleteMessages").click(function () {
        var selected = [];
        $('input:checked').each(function () {
            selected.push($(this).attr("id"));
        });
        $.ajax({
            type: 'POST',
            url: '/Admin/AdminMessage/DeleteMessages',
            data: { selectedItems: selected },
            success: function () {
                GetContactList();
                if (!isClickCheckboxToggle) {
                    AllCheckboxSetUnchecked();
                    $('#select-all-checkbox').click();
                }
            }
        });
    });
});

function GetMessageListForDropDown() {
    $(document).ready(function () {
        $.ajax({
            url: '/Admin/AdminMessage/GetUnreadMessagesCount',
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