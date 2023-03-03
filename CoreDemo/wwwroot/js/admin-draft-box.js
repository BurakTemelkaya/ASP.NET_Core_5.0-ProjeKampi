function GetDraftList() {
    $.ajax({
        url: '/Admin/AdminMessageDraft/GetMessageDraftList',
        type: "GET",
        success: function (data) {
            let jsonData = jQuery.parseJSON(data);
            let tablehtml = '<table class="table table-hover table-mail table-responsive"><tbody>';
            $.each(jsonData, (index, item) => {
                var message = `${item.Message}`;
                if (message.length > 75) {
                    message = message.substring(0, 75) + "...";
                }

                tablehtml += `<tr class="read">
                                <td class="check-mail col-1">
                                    <input type="checkbox" class="i-checks" id="${item.MessageDraftID}">
                                </td>
                                <td class="mail-contact col-2"><a href="/Admin/AdminMessageDraft/Edit/${item.MessageDraftID}">${item.ReceiverUser}</a></td>
                                <td class="mail-subject col-3"><a href="/Admin/AdminMessageDraft/Edit/${item.MessageDraftID}">${item.Subject}</a></td>
                                <td class="mail-content col-4"><a href="/Admin/AdminMessageDraft/Edit/${item.MessageDraftID}">${item.Details} ...</a></td>
                                <td class="mail- col-2"><a href="/Admin/AdminMessageDraft/Edit/${item.MessageDraftID}" class="btn btn-info text-white">Taslağı Düzenle</a></td>
                                <td class="col-2"><a href="/Admin/AdminMessage/SendMessage/${item.MessageDraftID}" class="btn btn-success text-white">Mesaj Gönder</a></td>
                            </tr>`
                });

                $(document).ready(function () {
                    $('.i-checks').iCheck({
                        checkboxClass: 'icheckbox_square-green',
                        radioClass: 'iradio_square-green',
                    });
                });
            
            tablehtml += '</tbody></table>';
            $(".mail-box").html(tablehtml);
            GetMessageDraftListForMessageFolder();
        }
    });
};


GetDraftList();

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
    $("#btnDeleteDrafts").click(function () {
        var selected = [];
        $('input:checked').each(function () {
            selected.push($(this).attr("id"));
        });
        $.ajax({
            type: 'POST',
            url: '/Admin/AdminMessageDraft/DeleteMessageDrafts',
            data: { selectedItems: selected },
            success: function (data) {
                GetDraftList();
                if (!isClickCheckboxToggle) {
                    AllCheckboxSetUnchecked();
                    $('#select-all-checkbox').click();
                }
            }
        });
    });
});

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