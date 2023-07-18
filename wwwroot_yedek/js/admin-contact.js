
function GetContactList() {
    $(document).ready(function () {
        $.ajax({
            url: '/Admin/AdminContact/GetContactList',
            type: "GET",
            data: { search: $('#search').val() },
            success: function (data) {
                let jsonData = jQuery.parseJSON(data);
                let tablehtml = '<table class="table table-hover table-mail"><tbody>';
                if (jsonData != "") {
                    $.each(jsonData, (index, item) => {
                        var message = `${item.ContactMessage}`;
                        if (message.length > 75) {
                            message = message.substring(0, 75) + "...";
                        }
                        var isRead = `${item.ContactStatus}`;
                        var read = "";
                        if (isRead == "false") {
                            isReadClass = "fa-envelope";
                            read = "unread";
                        }
                        else {
                            isReadClass = "fa-envelope-open";
                            read = "read";
                        }

                        var date = moment(`${item.ContactDate}`).format('DD-MM-YYYY hh:mm');

                        tablehtml += `<tr class="${read}">
                                <td class="check-mail">
                                    <input type="checkbox" class="i-checks" id="${item.ContactID}">
                                </td>
                                <td class="mail-contact"><a href="/Admin/AdminContact/Read/${item.ContactID}">${item.ContactUserName}</a></td>
                                <td class="mail-subject"><a href="/Admin/AdminContact/Read/${item.ContactID}"> <b> ${item.ContactSubject} </b> - ${message}</a></td>
                                <td class="text-right mail-date">${date}</td>
                            </tr>`;
                    });

                    $(document).ready(function () {
                        $('.i-checks').iCheck({
                            checkboxClass: 'icheckbox_square-green',
                            radioClass: 'iradio_square-green',
                        });
                    });
                }
                else {
                    tablehtml += `<tr class="read">
                                <td class="mail-contact">Aramanızla eşleşen mesajınız bulunmamaktadır.</td>
                            </tr>`;
                }
                tablehtml += '</tbody></table>';
                $(".mail-box").html(tablehtml);
                GetMessageListForDropDown();
                GetMessageListForMessageFolder();
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
    $("#btnReadContacts").click(function () {
        var selected = [];
        $('input:checked').each(function () {
            selected.push($(this).attr("id"));
        });

        $.ajax({
            type: 'POST',
            url: '/Admin/AdminContact/MarkReadContacts',
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
    $("#btnUnreadContacts").click(function () {
        var selected = [];
        $('input:checked').each(function () {
            selected.push($(this).attr("id"));
        });

        $.ajax({
            type: 'POST',
            url: '/Admin/AdminContact/MarkUnreadContacts',
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
    $("#btnDeleteContacts").click(function () {
        Swal.fire({
            title: 'UYARI!',
            text: "Seçilen iletişim mesajlarını silmek istediğinize emin misiniz?",
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
                    url: '/Admin/AdminContact/DeleteContacts',
                    data: { selectedItems: selected },
                    success: function () {
                        GetContactList();
                        if (!isClickCheckboxToggle) {
                            AllCheckboxSetUnchecked();
                            $('#select-all-checkbox').click();
                        }
                    }
                });
            }
        })
    });
});