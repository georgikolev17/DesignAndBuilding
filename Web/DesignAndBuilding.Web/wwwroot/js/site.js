// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let connection = null;

connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationshub")
    .build();

connection.on('RecieveNewNotificationMessage', (msg) => {
    if (msg.length != 0) {
        for (var i in msg) {
            var div = document.getElementById('alert');
            div.innerHTML += `<div class="alert alert-success alert-dismissible fade show" role="alert">
                <p>${msg[i].message}<p/>
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>`;
            console.log(msg[i].message);
            //var alert = document.createElement('div');
            //alert.classList.add('alert');
            //alert.classList.add('alert-primary');
            //alert.innerHTML = msg[i].message;
            //alert.innerHTML.add('<button type="button" class="btn-close" aria-label="Close"></button>');
            //div.appendChild(alert);
            document.appendChild('div');
        }
    }
})

connection.start()
    .catch(err => console.error(err.toString()))
    .then(() => {
        connection.invoke('CheckForNewNotifications')
    });
