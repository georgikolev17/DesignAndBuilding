console.log('Bids script loaded successfully!');

setupConnection = async function start() {
    let connection = null;
    connection = await new signalR.HubConnectionBuilder()
        .withUrl("/bidshub")
        .build();

    await connection.on('newbidplaced', (obj) => {
        console.log(obj);
        let new_bid = obj['price'];
        let bids = document.getElementById('bids');
        for (var i = 0; i < bids.rows.length; i++) {
            let cells = bids.rows[i].cells;

            let price = cells[4].innerHTML.split(' ')[0].replace(',', '.');
            if (new_bid <= price) {
                let new_row = bids.insertRow(i);
                let c1 = new_row.insertCell();
                let c2 = new_row.insertCell();
                let c3 = new_row.insertCell();
                let c4 = new_row.insertCell();
                let c5 = new_row.insertCell();
                let c6 = new_row.insertCell();

                c1.innerHTML = i;
                c5.innerHTML = obj['price'];
                c6.innerHTML = obj['now'];
                break;
            }
        }
        document.getElementById('bids').childNodes[1].rows = bids;
        console.log(bids);
    })

    await connection.start()
        .catch(err => console.error(err.toString()))
        .then(() => {
            console.log(connection);
            // connection.invoke('NewBid', '1', 'testUser', 5.4);
        });
}



setupConnection();
