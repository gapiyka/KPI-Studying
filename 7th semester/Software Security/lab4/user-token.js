'use strict';

const httpConstants = require('http-constants');
const requestCallback = require('request');
const { promisify } = require('util');
const jwt = require('jsonwebtoken');

const options = require('./request-options');
const request = promisify(requestCallback);

const getUserAccessToken = async (username, password) => {
    const userTokenOptions = options.getUserTokenOptions(username, password);

    const accessTokenUserResponse = await request(userTokenOptions);
    if (accessTokenUserResponse.statusCode != httpConstants.codes.OK) {
        const { statusCode, statusMessage, body } = accessTokenUserResponse;
        throw new Error(
            `Auth0 user-token: ${statusCode} ${statusMessage} ${body}`
        );
    }

    const response = JSON.parse(accessTokenUserResponse.body);

    return {
        accessToken: response.access_token,
        expiresIn: response.expires_in,
        refreshToken: response.refresh_token,
    };
};

const refreshUserToken = async (refreshToken) => {
    const refreshTokenOptions = options.getRefreshUserTokenOptions(refreshToken);

    const refreshTokenResponse = await request(refreshTokenOptions);
    if (refreshTokenResponse.statusCode != httpConstants.codes.OK) {
        const { statusCode, statusMessage, body } = refreshTokenResponse;
        throw new Error(
            `Auth0 user-token: ${statusCode} ${statusMessage} ${body}`
        );
    }

    const response = JSON.parse(refreshTokenResponse.body);
    return {
        accessToken: response.access_token,
        expiresIn: response.expires_in,
    };
};

const getPayloadFromToken = (token) => {
    try {
        return jwt.decode(token);
    } catch (error) {
        return null;
    }
};

module.exports = {
    getUserAccessToken,
    getPayloadFromToken,
    refreshUserToken,
};