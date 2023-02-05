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
                                    <input type="checkbox" class="i-checks" id="${item.MessageID}">
                                </td>
                                <td class="mail-contact"><a href="/Admin/AdminMessage/Read/${item.MessageID}">${item.SenderUser.NameSurname}</a></td>
                                <td class="mail-subject"><a href="/Admin/AdminMessage/Read/${item.MessageID}">${item.Details}</a></td>
                                <td class=""><i class="fa fa-paperclip"></i></td>
                                <td class="text-right mail-date">${date}</td>
                            </tr>`;
                });
                tablehtml += '</tbody></table>';
                $(".mail-box").html(tablehtml);
                $(document).ready(function () {
                    $('.i-checks').iCheck({
                        checkboxClass: 'icheckbox_square-green',
                        radioClass: 'iradio_square-green',
                    });
                });
                //GetContactCount();
                GetContactListForDropDown();
                GetContactListForMessageFolder();
            }
        });
    });
};

GetContactList();

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
            success: function (data) {
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
            success: function (data) {              
                GetContactList();
                if (!isClickCheckboxToggle) {
                    AllCheckboxSetUnchecked();
                    $('#select-all-checkbox').click();
                }
            }
        });       
    });
});