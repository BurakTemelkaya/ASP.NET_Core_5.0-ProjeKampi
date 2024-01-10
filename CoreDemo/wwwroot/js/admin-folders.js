let isUnreadMessage;
let unreadMessageCount;
let isLoad;

GetUnreadMessageCount();

function GetUnreadMessageCount() {
    $(document).ready(function () {
        $.ajax({
            url: '/Admin/AdminMessage/GetUnreadMessagesCount',
            type: "GET",
            success: function (data) {
                if (data != '0') {
                    isUnreadMessage = true;
                    unreadMessageCount = data;

                }
                Load();
                isLoad = true;
            }
        });
    });
}


function GetMessageListForDropDown() {
    $(document).ready(function () {
        let badgeHtml = '<i class="fa fa-envelope">';
        if (isUnreadMessage) {
            badgeHtml += '<span class="label label-warning">' + unreadMessageCount + '</span>';
        }
        badgeHtml += '</i>';
        $("#UnreadMessagesCountBadge").html(badgeHtml);
    });
};

function GetMessageListForMessageFolder() {
    $(document).ready(function () {
        let badgeHtml = '<i class="fa fa-inbox "></i> Gelen Mesajlar ';
        if (isUnreadMessage != null) {
            badgeHtml += '<span class="label label-warning float-right">' + unreadMessageCount + '</span>';
        }
        badgeHtml += '</i>';
        $("#messageFolderBadge").html(badgeHtml);
    });
};

function GetMessageDraftListForMessageFolder() {
    $(document).ready(function () {
        $.ajax({
            url: '/Admin/AdminMessage/GetDraftMessagesCount',
            type: "GET",
            success: function (data) {
                console.log(data)
                let badgeHtml = '<i class="fa fa-file-text-o "></i> Taslaklar';
                if (data != 0) {
                    badgeHtml += '<span class="label label-danger float-right">' + data + '</span>';
                }
                $("#messageFolderDraftBadge").html(badgeHtml);
            }
        });
    });
};

function Load() {
    $(document).ready(function () {
        GetMessageListForDropDown();
        GetMessageListForMessageFolder();
        GetMessageDraftListForMessageFolder();
    });
}
