const request = require("request");
const dotenv = require("dotenv");
dotenv.config();

const options = {
    method: 'POST',
    url: `${process.env.AUTH0_URL}/oauth/token`,
    headers: { 'content-type': 'application/json' },
    form: {
        refresh_token: process.env.AUTH0_REFRESH,
        client_id: process.env.AUTH0_CLIENT_ID,
        client_secret: process.env.AUTH0_CLIENT_SECRET,
        code: process.env.AUTH0_CODE,
        audience: `${process.env.AUTH0_AUDIENCE}`,
        grant_type: "refresh_token",
        scope: "offline_access"
    }
};

request(options, function (error, response, body) {
    if (error) throw new Error(error);

    console.log(body);
});