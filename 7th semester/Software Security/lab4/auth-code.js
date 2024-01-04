'use strict';

const requestCallback = require('request');
const { promisify } = require('util');
const httpConstants = require('http-constants');

const options = require('./request-options');
const request = promisify(requestCallback);

const authByCode = async (code) => {
    const codeAuthOptions = options.getCodeOptions(code);

    const tokenResponse = await request(codeAuthOptions);
    if (tokenResponse.statusCode != httpConstants.codes.OK) {
        const { statusCode, statusMessage, body } = tokenResponse;
        throw new Error(
            `Auth0 user-token: ${statusCode} ${statusMessage} ${body}`
        );
    }

    const response = JSON.parse(tokenResponse.body);

    return {
        accessToken: response.access_token,
        expiresIn: response.expires_in,
        refreshToken: response.refresh_token,
    };
};

module.exports = {
    authByCode,
};