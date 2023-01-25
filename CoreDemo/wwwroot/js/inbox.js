function GetContactList(Search) {
    $(document).ready(function () {
        $.ajax({
            url: '/Admin/AdminMessage/GetInboxMessages',
            type: "GET",
            success: function (data) {
                let jsonData = jQuery.parseJSON(data);
                let tablehtml = '<table class="table table-hover table-mail"><tbody>';
                data: { search: Search }
                $.each(jsonData, (index, item) => {
                    var message = `${item.Message}`;
                    if (message.length > 75) {
                        message = message.substring(0, 75) + "...";
                    }
                    var isRead = `${item.MessageStatus}`;
                    var read = "";
                    if (isRead == "false") {
                        isReadClass = "fa-envelope";
                        read = "unread";
                    }
                    else {
                        isReadClass = "fa-envelope-open";
                        read = "read";
                    }

                    var date = moment(`${item.MessageDate}`).format('DD-MM-YYYY hh:mm');

                    tablehtml += `<tr class="${read}">
                                <td class="check-mail">
                                    <input type="checkbox" class="i-checks mailbox-messages" id="${item.MessageID}">
                                </td>
                                <td class="mail-ontact"><a href="/Admin/AdminMessage/Read/${item.MessageID}">${item.SenderUser.NameSurname}</a></td>
                                <td class="mail-subject"><a href="/Admin/AdminMessage/Read/${item.MessageID}">${item.Details}</a></td>
                                <td class=""><i class="fa fa-paperclip"></i></td>
                                <td class="text-right mail-date">${date}</td>
                            </tr>`;
                });
                tablehtml += '</tbody></table>';
                $("#mailList").html(tablehtml);
                //GetContactCount();
            }
        });
    });
};

GetContactList();

$('.checkbox-toggle').click(function () {
    var clicks = $(this).data('clicks')
    if (clicks) {
        AllCheckboxSetUnchecked();
    } else {
        AllCheckboxSetChecked();
    }
    $(this).data('clicks', !clicks)
});

function AllCheckboxSetChecked() {
    $('.mailbox-messages input[type=\'checkbox\']').prop('checked', true)
    $('.checkbox-toggle .fa.fa-square-o').removeClass('fa-square-o').addClass('fa-check-square')
}

function AllCheckboxSetUnchecked() {
    $('.mailbox-messages input[type=\'checkbox\']').prop('checked', false)
    $('.checkbox-toggle .fa.fa-check-square').removeClass('fa-check-square').addClass('fa-square-o')
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
            success: function (data) {               
                AllCheckboxSetUnchecked();
                $('#select-all-checkbox').click();
                GetContactList();       
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
            success: function (data) {                
                GetContactList();
                $('#select-all-checkbox').click();
                AllCheckboxSetUnchecked();               
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
            success: function (data) {              
                GetContactList();          
                $('#select-all-checkbox').click();
                AllCheckboxSetUnchecked();
            }
        });       
    });
});

function GetContactCount() {
    $(document).ready(function () {
        $.ajax({
            url: '/Contact/GetUnreadContactCount',
            type: "GET",
            success: function (data) {
                var badgeHtml = 'Contact';
                if (data != '0') {
                    badgeHtml += ' <span class="right badge badge-danger">';
                    badgeHtml += data;
                    badgeHtml += '</span>';
                }
                $("#MessageCountBadge").html(badgeHtml);
            }
        });
    });
};