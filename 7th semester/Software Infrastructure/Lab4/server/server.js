const express = require('express');
const mongoose = require('mongoose');
const { Client } = require('pg');
const bodyParser = require('body-parser');

const app = express();
const port = 3000;

// MongoDB Connection
mongoose.connect('mongodb://localhost:27017/cinema_db', { useNewUrlParser: true, useUnifiedTopology: true });
const db = mongoose.connection;
db.on('error', console.error.bind(console, 'MongoDB connection error:'));

// PostgreSQL Connection
const postgresClient = new Client({
    user: 'postgres_user',
    host: 'localhost',
    database: 'cinema_db',
    password: 'postgres_password',
    port: 5432,
});
postgresClient.connect();

app.use(bodyParser.json());

// MongoDB Models
const Viewer = mongoose.model('Viewer', {
    code: String,
    fullName: String,
    age: Number,
    gender: String,
    orders: [{ type: mongoose.Schema.Types.ObjectId, ref: 'Order' }],
});

const Ticket = mongoose.model('Ticket', {
    code: String,
    date: Date,
    seatNumber: Number,
    time: String,
    movieTitle: String,
});

const Order = mongoose.model('Order', {
    viewer: { type: mongoose.Schema.Types.ObjectId, ref: 'Viewer' },
    ticket: { type: mongoose.Schema.Types.ObjectId, ref: 'Ticket' },
});

// PostgreSQL Models
const Seller = mongoose.model('Seller', {
    code: String,
    fullName: String,
    age: Number,
    gender: String,
    // Add other properties as needed
});

const getAllMovies = async () => {
    const result = await postgresClient.query('SELECT * FROM movies');
    return result.rows;
};

// Express Routes
app.get('/viewers', async (req, res) => {
    try {
        const viewers = await Viewer.find().populate('orders');
        res.json(viewers);
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

app.get('/sellers', async (req, res) => {
    try {
        const sellers = await Seller.find();
        res.json(sellers);
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

app.post('/sellers', async (req, res) => {
    try {
        const seller = await Seller.create(req.body);
        res.json(seller);
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

app.get('/movies', async (req, res) => {
    try {
        const movies = await getAllMovies();
        res.json(movies);
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

app.post('/viewers', async (req, res) => {
    try {
        const viewer = await Viewer.create(req.body);
        res.json(viewer);
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});