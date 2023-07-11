let isUnreadMessage;

function GetUnreadMessageCount() {
    $(document).ready(function () {
        $.ajax({
            url: '/Admin/AdminMessage/GetUnreadMessagesCount',
            type: "GET",
            success: function (data) {
                if (data != '0') {
                    isUnreadMessage = true;
                }
            }
        });
    });
}


GetUnreadMessageCount();
function GetMessageListForDropDown() {
    $(document).ready(function () {
        let badgeHtml = '<i class="fa fa-envelope">';
        if (isUnreadMessage) {
            badgeHtml += '<span class="label label-warning">' + data + '</span>';
        }
        badgeHtml += '</i>';
        $("#UnreadMessagesCountBadge").html(badgeHtml);
    });
};

GetMessageListForDropDown();

function GetMessageListForMessageFolder() {
    $(document).ready(function () {
        let badgeHtml = '<i class="fa fa-inbox "></i> Gelen Mesajlar ';
        if (isUnreadMessage != null) {
            badgeHtml += '<span class="label label-warning float-right">' + data + '</span>';
        }
        badgeHtml += '</i>';
        $("#messageFolderBadge").html(badgeHtml);
    });
};

GetMessageListForMessageFolder();

function GetMessageDraftListForMessageFolder() {
    $(document).ready(function () {
        $.ajax({
            url: '/Admin/AdminMessage/GetDraftMessagesCount',
            type: "GET",
            success: function (data) {
                let badgeHtml = '<i class="fa fa-file-text-o "></i> Taslaklar';
                if (data != '0') {
                    badgeHtml += '<span class="label label-danger float-right">' + data + '</span>';
                }
                $("#messageFolderDraftBadge").html(badgeHtml);
            }
        });
    });
};

GetMessageDraftListForMessageFolder();