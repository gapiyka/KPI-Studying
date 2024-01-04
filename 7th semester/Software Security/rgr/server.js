const tls = require('tls');
const fs = require('fs');
const crypto = require('crypto');

const options = {
    key: fs.readFileSync('server-key.pem'),
    cert: fs.readFileSync('server-cert.pem'),
};

const server = tls.createServer(options, (cleartextStream) => {
    console.log('Server: Client connected');

    cleartextStream.on('data', (data) => {
        console.log('Server received data:', data.toString());

        if (data.toString().includes('hello')) {
            const serverHelloMessage = 'Hello from the server!';
            cleartextStream.write(serverHelloMessage);

            // Generate a premaster secret for demonstration purposes (replace with your logic)
            const premasterSecret = 'ThisIsAPremasterSecret';

            // Encrypt the premaster secret using the client's public key with PKCS#1 v1.5 padding
            const encryptedPremasterSecret = crypto.publicEncrypt(
                {
                    key: fs.readFileSync('client-cert.pem'),  // Client's public key
                    padding: crypto.constants.RSA_PKCS1_PADDING,
                },
                Buffer.from(premasterSecret, 'utf8')  // Ensure proper encoding
            );

            // Send the encrypted premaster secret to the client
            cleartextStream.write(encryptedPremasterSecret);
        }
    });

    cleartextStream.on('end', () => {
        console.log('Server: Client disconnected');
    });
});

server.listen(8080, () => {
    console.log('Server listening on port 8080');
});