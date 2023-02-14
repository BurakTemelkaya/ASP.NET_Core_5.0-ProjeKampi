
function GetMessageList() {
    $.ajax({
        url: '/Message/GetMessageList',
        type: "GET",
        data: { search: $('#search').val() },
        success: function (data) {
            let jsonData = jQuery.parseJSON(data);
            let tablehtml = `<table class="table table-responsive">
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
                    var read = "";
                    var readHtml = "";
                    if (isRead == "false") {
                        readHtml = "<p>Okundu olarak işaretle</p>";
                        read = "bg-secondary";
                    }
                    else {
                        readHtml = "<p>Okunmadı olarak işaretle</p>";
                        read = "";
                    }

                    var date = moment(`${item.MessageDate}`).format('DD-MM-YYYY hh:mm');

                    tablehtml += `<tr class="${read}">
                <td scope="row">
                    <div class="form-check form-check-flat form-check-primary">
                        <label class="form-check-label">
                          <input type="checkbox" class="form-check-input" id="${item.MessageID}">
                        </label>
                      </div>
                </td>
                <td>${item.Subject}</td>
                <td>${item.SenderUser.NameSurname} </td>
                <td>${item.Details}</td>
                <td>${date}</td>
                <td><a href="/Message/MessageDetails/${item.MessageID}" class="btn btn-primary">Mesajı Aç</a></td>
                <td>
                    <a href="/Message/MarkUsUnreadInbox/${item.MessageID}" class="btn btn-gradient-dark">
                    ${readHtml}
                    </a>
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
    $('.i-checks').attr("checked", "true");
    $('.icheckbox_square-green').addClass(' checked');
    $('.checkbox-toggle .fa.fa-square-o').removeClass('fa-square-o').addClass('fa-check-square');
}

function AllCheckboxSetUnchecked() {
    $('.i-checks').prop('checked', "false");
    $('.icheckbox_square-green.checked').removeClass('icheckbox_square-green checked').addClass('icheckbox_square-green');
    $('.checkbox-toggle .fa.fa-check-square').removeClass('fa-check-square').addClass('fa-square-o');
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