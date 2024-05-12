const WebSocket = require('ws');
const http = require('http');

// Create a server
const server = http.createServer((req, res) => {
    if (req.method === 'POST' && req.url === '/upload') {
        let body = [];
        req.on('data', (chunk) => {
            body.push(chunk);
        }).on('end', () => {
            body = Buffer.concat(body);

            // Find the index where JPEG marker (0xFF, 0xD8) starts
            const startIndex = body.indexOf(Buffer.from([0xFF, 0xD8]));

            // If the JPEG marker is not found, respond with an error
            if (startIndex === -1) {
                console.log('JPEG marker not found');
                res.statusCode = 400;
                res.end('JPEG marker not found');
                return;
            }

            // Slice the buffer to extract the JPEG data starting from the marker
            const jpegData = body.slice(startIndex);

            // Send the JPEG data to clients via WebSocket
            wss.clients.forEach(function each(client) {
                if (client.readyState === WebSocket.OPEN) {
                    client.send(jpegData);
                }
            });

            // Respond to the POST request
            res.statusCode = 200;
            res.end('Frame received and sent');
        });
    } else {
        // Handle other requests or methods
        res.statusCode = 404;
        res.end('Not Found');
    }
});

// WebSocket server
const wss = new WebSocket.Server({ server });

wss.on('connection', function connection(ws) {
    console.log('Client connected');
});

// Start the server
const port = 3000;
server.listen(port, () => {
    console.log(`Server running at http://localhost:${port}/`);
});
