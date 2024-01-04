'use strict';

require('dotenv').config();

const config = {
    port: process.env.PORT || 8080,
    sessionKey: 'Authorization',
    domain: process.env.AUTH0_DOMAIN || '',
    clientId: process.env.AUTH0_CLIENT_ID || '',
    clientSecret: process.env.AUTH0_CLIENT_SECRET || '',
    audience: process.env.AUTH0_AUDIENCE || '',
    pictureUrl:
        'https://unity.com/sites/default/files/styles/social_media_sharing/public/2022-02/U_Logo_White_CMYK.jpg',
    localTokenPath: `${__dirname}/token-info.json`,
    refreshTokenViaTimeSec: 500,
    timeToRefreshSec: 23.95 * 60 * 60
}

module.exports = {
    ...config,
    loginUrl: `https://${config.domain}/authorize?response_type=code&client_id=${config.clientId}&redirect_uri=http://localhost:3000&scope=offline_access&audience=${config.audience}&state=login-example`
};