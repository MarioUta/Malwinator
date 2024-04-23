uri = "http://localhost:5000"
// uri = "https://malwinator.chickenkiller.com"
// uri = "http://malwinator.chickenkiller.com"
function fetchResult() {
    path = "/getResult"
    const paragraph = document.getElementById('command-result');
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

setInterval(fetchResult, 1000);