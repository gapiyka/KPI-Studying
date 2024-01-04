'use strict';

const httpConstants = require('http-constants');
const fsp = require('fs/promises');
const requestCallback = require('request');
const { promisify } = require('util');

const config = require('./config');
const options = require('./request-options');

const hourInSec = 60 * 60;
const request = promisify(requestCallback);
const defaultTokenOptions = options.getAppTokenOptions();

const readTokenInfo = async () => {
    try {
        const buffer = await fsp.readFile(config.localTokenPath, 'utf-8');
        const json = JSON.parse(buffer);
        if (json.expiryDate <= Date.now()) {
            return null;
        }

        return json.tokenInfo;
    } catch (err) {
        return null;
    }
};

const storeTokenInfo = async (tokenInfo) => {
    try {
        const tokenValidTimeMsec = (tokenInfo.expires_in - hourInSec) * 1000;
        const buffer = JSON.stringify({
            tokenInfo,
            expiryDate: Date.now() + tokenValidTimeMsec,
        });
        await fsp.writeFile(config.localTokenPath, buffer);
    } catch (err) {
        return null;
    }
};

const getAppAccessToken = async (tokenOptions = defaultTokenOptions) => {
    let tokenInfo = await readTokenInfo();

    if (!tokenInfo) {
        const tokenResponse = await request(tokenOptions);
        if (tokenResponse.statusCode != httpConstants.codes.OK) {
            const { statusCode, statusMessage, body } = tokenResponse;
            console.dir({ statusCode, statusMessage, body });
            return;
        }

        tokenInfo = JSON.parse(tokenResponse.body);
        await storeTokenInfo(tokenInfo);
    }

    return tokenInfo;
};

module.exports = {
    readTokenInfo,
    storeTokenInfo,
    getAppAccessToken,
};