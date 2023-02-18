function WriterMessageNotification{
    $(document).ready(function () {
        $.ajax({
            url: '/Message/GetMessageList',
            type: "GET",
            success: function (data) {
                let jsonData = jQuery.parseJSON(data);
                if (jsonData != "") {
                    var tableHtml = '';
                    var readClass = "";
                    $.each(jsonData, (index, item) => {
                        var message = `${item.Message}`;
                        if (message.length > 50) {
                            message = message.substring(0, 50) + "...";
                        }
                        var isRead = `${item.MessageStatus}`;
                        var readTitle = "";
                        if (isRead == "false") {
                            readTitle = "okunmadı";
                            readClass = "count-symbol";
                        }
                        else {
                            readTitle = "okundu";
                        }

                        var date = moment(`${item.MessageDate}`).format('DD-MM-YYYY hh:mm');

                        tableHtml += `
                    <div class="dropdown-divider"></div>
                    <a class="dropdown-item preview-item" href="/Message/MessageDetails/${item.MessageID}">
                    <div class="preview-thumbnail">
                    <img src="${item.SenderUser.ImageUrl}" alt="${item.SenderUser.NameSurname} image" class="profile-pic">
                     </div>
                    <div class="preview-item-content d-flex align-items-start flex-column justify-content-center">
                    <h6>
                    ${item.SenderUser.NameSurname}- ${item.Subject}
                    </h6>
                    <p class="text-gray mb-0">
                    ${date}
                    <b>${readTitle}</b>
                    </p>
                    </div>
                    </a>`
                    });
                    $("#MessageNotificationDiv").html(tableHtml);
                    $("#messageNotificationSpan").addClass(readClass);
                }
            }
        });
    });
};
WriterMessageNotification();