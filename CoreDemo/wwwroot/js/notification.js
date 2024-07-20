document.addEventListener("DOMContentLoaded", function () {

const connection = new signalR.HubConnectionBuilder()
    .withUrl(`/ReceiveNotification`)
            .build();

    connection.on("ReceiveNotification", function (message) {
        console.log(message);
            toastr.info(message, 'Notification', {
                closeButton: true,
                progressBar: true,
                positionClass: "toast-top-right",
                timeOut: "0",
                "stack": false,
                "newestOnTop": false,
            });
            WriterMessageNotification();
        });

        connection.start().catch(function (err) {
            console.error('SignalR connection error:', err.toString());
        });

});