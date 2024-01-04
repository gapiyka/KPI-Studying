const tls = require('tls');
const fs = require('fs');
const crypto = require('crypto');
const uuid = require('uuid');

const options = {
    key: fs.readFileSync('client-key.pem'),
    cert: fs.readFileSync('client-cert.pem'),
};
process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
const socket = tls.connect(8080, 'localhost', options, () => {
    console.log('Client: Connected to server');
    const randomUuid = uuid.v4();

    // Send the "hello" message with the random UUID to the server
    socket.write(`hello-${randomUuid}`);

    // Generate a premaster secret (for demonstration purposes, using a simple string)
    const premasterSecret = 'ThisIsAPremasterSecret';

    console.log('Client: Encrypting premaster secret:', premasterSecret);

    // Encrypt the premaster secret using the server's public key
    const encryptedPremasterSecret = crypto.publicEncrypt(
        {
            key: fs.readFileSync('server-cert.pem'),  // Server's public key
            padding: crypto.constants.RSA_PKCS1_PADDING,
        },
        Buffer.from(premasterSecret)
    );

    socket.write(encryptedPremasterSecret);

    socket.on('data', (data) => {
        console.log('Client received data:', data.toString());
    });

    socket.on('end', () => {
        console.log('Client: Connection closed');
    });
});