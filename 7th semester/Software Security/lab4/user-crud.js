'use strict';

const requestCallback = require('request');
const { promisify } = require('util');
const httpConstants = require('http-constants');

const options = require('./request-options');
const appToken = require('./app-token');
const request = promisify(requestCallback);

const getUserById = async (userId) => {
    const { access_token, token_type } = await appToken.getAppAccessToken();
    const authorizationHeader = `${token_type} ${access_token}`;
    const userGetOptions = options.getUserGetOptions(authorizationHeader, userId);

    const userResponse = await request(userGetOptions);
    if (userResponse.statusCode != httpConstants.codes.OK) {
        const { statusCode, statusMessage, body } = userResponse;
        throw new Error(
            `Auth0 user-token: ${statusCode} ${statusMessage} ${body}`
        );
    }

    const response = JSON.parse(userResponse.body);

    return response;
};

const createUser = async (userInput) => {
    const { access_token, token_type } = await appToken.getAppAccessToken();
    const authorizationHeader = `${token_type} ${access_token}`;

    const { name, surname, nickname, login, password } = userInput;
    const user = options.getUser(name, surname, nickname, login, password);
    const isValidUser = Object.keys(user).reduce(
        (acc, key) => acc && user[key] !== undefined,
        true
    );
    if (!isValidUser) {
        throw new Error(
            `Not all fields of registration: ${statusMessage} ${body}`
        );
    }

    const createUserOptions = options.getUserCreateOptions(
        authorizationHeader,
        user
    );

    const newUserResponse = await request(createUserOptions);
    if (newUserResponse.statusCode != httpConstants.codes.CREATED) {
        const { statusCode, statusMessage, body } = newUserResponse;
        throw new Error(
            `Auth0 user-creation: ${statusCode} ${statusMessage} ${body}`
        );
    }

    const response = JSON.parse(newUserResponse.body);

    return response;
};

module.exports = {
    getUserById,
    createUser,
};