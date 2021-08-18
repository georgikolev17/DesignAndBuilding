let connection = null;

setupConnection = async function start() {
    connection = await new signalR.HubConnectionBuilder()
        .withUrl("/notificationshub")
        .build();

    await connection.on('RecieveNewNotificationMessage', (msg) => {
        console.log(msg);
        if (msg.length != 0) {
            for (var i in msg) {
                var div = document.getElementById('alert');
                div.innerHTML += `<div class="alert alert-success alert-dismissible fade show" role="alert">
                <p>${msg[i]}<p/>
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>`;
                console.log(msg[i]);
                document.appendChild('div');
            }
        }
    })

    await connection.start()
        .catch(err => console.error(err.toString()))
        .then(() => {
            connection.invoke('CheckForNewNotifications')
        });
}

setupConnection();
