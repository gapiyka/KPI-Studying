const request = require("request");
const dotenv = require("dotenv");
dotenv.config();

const options = {
  method: 'POST',
  url: `${process.env.AUTH0_URL}/oauth/token`,
  headers: { 'content-type': 'application/json' },
  form: {
    client_id: process.env.AUTH0_CLIENT_ID,
    client_secret: process.env.AUTH0_CLIENT_SECRET,
    audience: `${process.env.AUTH0_URL}/api/v2/`,
    grant_type: "client_credentials",
  }
};

request(options, function (error, response, body) {
  if (error) throw new Error(error);

  console.log(body);
});