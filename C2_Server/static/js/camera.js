const slideshowImage = document.getElementById("slideshow-image");

// Function to update the slideshow with a new image
function updateSlideshow(imageData) {
    // Convert the received byte data to a blob
    const blob = new Blob([imageData], { type: 'image/jpeg' });

    // Create a URL for the blob
    const imageUrl = URL.createObjectURL(blob);

    // Update the src attribute of the existing image
    slideshowImage.src = imageUrl;
}

// Create WebSocket connection
const socket = new WebSocket('ws://malwinator.chickenkiller.com:443');
// const socket = new WebSocket('ws://localhost:5000');

// Connection opened
socket.addEventListener('open', function (event) {
    console.log('Connected to server');
});

// Listen for messages
socket.addEventListener('message', function (event) {
    console.log('Received message:', event.data);
    updateSlideshow(event.data);
});

// Handle errors
socket.addEventListener('error', function (event) {
    console.error('WebSocket error:', event);
});