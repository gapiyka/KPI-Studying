const request = require("request-promise");
const dotenv = require("dotenv");
dotenv.config();

async function createUser() {
  const raw = {
    email: "gapiy.denis@gmail.com",
    user_metadata: {},
    blocked: false,
    email_verified: false,
    app_metadata: {},
    given_name: "Gapiyka",
    family_name: "Gapiyka",
    name: "Gapiyka",
    nickname: "Gapiyka",
    picture: "https://unity.com/sites/default/files/styles/social_media_sharing/public/2022-02/U_Logo_White_CMYK.jpg",
    user_id: "5123121",
    connection: "Username-Password-Authentication",
    password: "gap41K!mamut",
    verify_email: false,
  };

  const requestOptions = {
    method: 'POST',
    url: `${process.env.AUTH0_URL}/api/v2/users`,
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${process.env.AUTH0_TOKEN}`,
    },
    body: JSON.stringify(raw),
  };

  try {
    const response = await request(requestOptions);
    console.log(response);
  } catch (error) {
    console.error("Error creating user:", error.message);
  }
}

createUser();
