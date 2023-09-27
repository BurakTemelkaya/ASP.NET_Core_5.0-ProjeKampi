function WriterMessageNotification() {
    $(document).ready(function () {
        GetUnreadMessageInfo();
        $.ajax({
            url: '/Message/GetMessageList',
            type: "GET",
            success: function (data) {
                let jsonData = jQuery.parseJSON(data);
                if (jsonData != "") {
                    var tableHtml = '';
                    $.each(jsonData, (index, item) => {
                        var message = `${item.Message}`;
                        if (message.length > 50) {
                            message = message.substring(0, 50) + "...";
                        }
                        var isRead = `${item.MessageStatus}`;
                        var readTitle = "";
                        if (isRead == "false") {
                            readTitle = "okunmadı";
                            
                        }
                        else {
                            readTitle = "okundu";                            
                        }
                        var date = moment(`${item.MessageDate}`).format('DD-MM-YYYY hh:mm');

                        tableHtml += `
                    <div class="dropdown-divider"></div>
                    <a class="dropdown-item preview-item" href="/Message/MessageDetails/${item.MessageID}">
                    <div class="preview-thumbnail">
                    <img src="${item.SenderImageUrl}" alt="${item.SenderNameSurname} image" class="profile-pic">
                     </div>
                    <div class="preview-item-content d-flex align-items-start flex-column justify-content-center">
                    <h6>
                    ${item.SenderNameSurname}- ${item.Subject}
                    </h6>
                    <p class="text-gray mb-0">
                    ${date}
                    <b>${readTitle}</b>
                    </p>
                    </div>
                    </a>`
                    });
                    $("#MessageNotificationDiv").html(tableHtml);
                }
            }
        });       
    });
};

function GetUnreadMessageInfo() {
    $(document).ready(function () {
        $.ajax({
            url: '/Message/GetUnreadMessagesCount',
            type: "GET",
            success: function (data) {
                var notificationMessageHtml = "";
                if (data == 0) {
                    $("#messageNotificationSpan").removeClass('count-symbol');                    
                    notificationMessageHtml = '<h6 class="p-3 mb-0 text-center">Şu anda yeni bir mesajın yok :(</h6>';
                }
                else {
                    $("#messageNotificationSpan").addClass('count-symbol');
                    notificationMessageHtml = `<h6 class="p-3 mb-0 text-center">${data} tane yeni mesajın var :)</h6>`;                   
                }
                $("#WriterMessageNotificationUnreadMessageInfoDiv").html(notificationMessageHtml);
            }
        });
    });
};

WriterMessageNotification();