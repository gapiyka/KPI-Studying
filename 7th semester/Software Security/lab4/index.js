'use strict';

const express = require('express');
const bodyParser = require('body-parser');
const cookieParser = require('cookie-parser');
const path = require('path');
const httpConstants = require('http-constants');
const { expressjwt: expressJwt } = require('express-jwt');
const jwksRsa = require('jwks-rsa');
const config = require('./config');
const appToken = require('./app-token');
const userToken = require('./user-token');
const userModel = require('./user-crud');
const AttemptManager = require('./attempt-manager');
const DataBase = require('./database');
const { getPublicKey } = require('./public-key');
const { verifyToken } = require('./jwt-utils');
const { authByCode } = require('./auth-code');
require('dotenv').config();

const attemptsManager = new AttemptManager();
const tokensStorage = new DataBase(path.join(config.localTokenPath));
const app = express();

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(cookieParser());

const SESSION_KEY = config.sessionKey;

const checkJwt = expressJwt({
    secret: jwksRsa.expressJwtSecret({
        cache: true,
        rateLimit: true,
        jwksRequestsPerMinute: 5,
        jwksUri: `https://${config.domain}/.well-known/jwks.json`,
    }),

    audience: config.audience,
    issuer: `https://${config.domain}/`,
    algorithms: ['RS256'],
});

app.use(async (req, res, next) => {
    const authorizationHeader = req.get(SESSION_KEY);
    if (!authorizationHeader) return next();
    const accessToken = authorizationHeader.split(' ')[1];
    const payload = await verifyToken(accessToken);
    if (payload) {
        req.userId = payload.sub;
        if (payload.exp - config.timeToRefreshSec <= Date.now()) {
            const { refreshToken } = tokensStorage.getData(req.userId);
            const { accessToken, expiresIn } = await userToken.refreshUserToken(refreshToken);
            res.setHeader('AccessToken', accessToken);
            res.setHeader('expiresDate', Date.now() + expiresIn * 1000);
        }
        console.log(`User with id ${req.userId} authorized by Access Token`);
    } else {
        console.log('Not valid authorization header');
    }

    next();
});

app.get('/', async (req, res) => {
    const queryParams = req.query;

    if (
        queryParams &&
        queryParams.code &&
        queryParams.state === config.state
    ) {
        try {
            const { code } = queryParams;
            const { accessToken, expiresIn, refreshToken } = await authByCode(code);
            res.setHeader('AccessToken', accessToken);
            res.setHeader('expiresDate', Date.now() + expiresIn * 1000);
            const { sub: userId } = userToken.getPayloadFromToken(accessToken);
            tokensStorage.upsert(userId, { refreshToken });
        } catch { }
    }
    res.sendFile(path.join(__dirname + '/index.html'));
});

app.get('/userinfo', checkJwt, async (req, res) => {
    if (req.userId) {
        const userData = await userModel.getUserById(req.userId);

        return res.json({
            username: `${userData.name}(${userData.email})`,
            logout: 'http://localhost:3000/logout',
        });
    }
    res.status(httpConstants.codes.UNAUTHORIZED).send();
});

app.get('/register', (req, res) => {
    res.sendFile(path.join(__dirname + '/register.html'));
});

app.get('/logout', async (req, res) => {
    try {
        const userId = req.userId;

        if (!userId) {
            return res.status(httpConstants.codes.UNAUTHORIZED).send();
        }

        console.log(`User with id ${userId} successfully logout`);
        await tokensStorage.deleteByKey(userId);
        res.redirect('/');
    } catch (err) {
        console.error(err);
        res.status(httpConstants.codes.INTERNAL_SERVER_ERROR).send();
    }
});

app.get('/login', async (req, res) => {
    res.redirect(config.loginUrl);
});

app.post('/api/login', async (req, res) => {
    const { login, password } = req.body;
    if (!attemptsManager.canLogin(login))
        return res
            .status(httpConstants.codes.UNAUTHORIZED)
            .json({ waitTime: attemptsManager.waitTime });

    try {
        const { accessToken, expiresIn, refreshToken } =
            await userToken.getUserAccessToken(login, password);

        const { sub: userId } = userToken.getPayloadFromToken(accessToken);
        tokensStorage.upsert(userId, { refreshToken });

        console.log(`User with id ${userId} (${login}) successfully login`);
        res.json({
            token: accessToken,
            expiresDate: Date.now() + expiresIn * 1000,
        });
    } catch (err) {
        console.error(err);
        res.status(httpConstants.codes.INTERNAL_SERVER_ERROR).send();
    }
});

app.get('/api/refresh', async (req, res) => {
    try {
        const userId = req.userId;

        if (!userId) return res.status(httpConstants.codes.UNAUTHORIZED).send();

        const { refreshToken: refreshTokenDb } = tokensStorage.getData(userId);
        if (refreshTokenDb) {
            const { accessToken, expiresIn } = await userToken.refreshUserToken(
                refreshTokenDb
            );
            console.log(`Refresh token for user with id ${req.userId}`);
            res.json({
                token: accessToken,
                expiresDate: Date.now() + expiresIn * 1000,
            });
        }

        res.status(httpConstants.codes.UNAUTHORIZED).send();
    } catch (err) {
        console.error(err);
        res.status(httpConstants.codes.INTERNAL_SERVER_ERROR).send();
    }
});

app.post('/api/register', async (req, res) => {
    try {
        const userOptions = req.body;
        const user = await userModel.createUser(userOptions);

        console.log(
            `User with id ${user.user_id} (${user.email}) successfully registered`
        );
        res.json({ redirect: '/' });
    } catch (err) {
        console.error(err);
        res.status(httpConstants.codes.INTERNAL_SERVER_ERROR).send();
    }
});

app.listen(config.port, async () => {
    console.log(`Example app listening on port ${config.port}`);

    const appAccessToken = await appToken.getAppAccessToken();
    const publicKey = await getPublicKey();
    console.log({ appAccessToken });
});