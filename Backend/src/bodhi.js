const config = require('./lib/config')
const express = require('express')
const s3urlgen = require('./lib/s3urlGenerator')
const db = require('./lib/db')
const bodyParser = require('body-parser')
const item = require('./lib/v1/item/item')
const login = require('./lib/v1/login/login')

function checkUserToken(req, res, next) {
	if (req.headers["userid"] != "") {
		next()
	}	
	else {
		res.status(404).end("{result:\"user token needed\"}")
	}
}

function checkPSK(req, res, next) {
	if (req.headers["psk"] == config.psk) {
		next()
	}
	else {
		res.status(403).end("{result:\"forbidden\"}")
	}
}

function checkUserNotNull(req, res, next) {
	console.log("userid in headers: " + req.headers["userid"])
	if((req.headers["userid"] != undefined) && (req.headers["userid"].length>3)) {
		next()
	}
	else {
		res.status(400).end("{result:\"Need to specify user\"}")
	}
}

app = express()
app.use(bodyParser.text());
app.use(checkPSK)
//app.use(checkUserNotNull)

app.get("/healthcheck", function(req, res) {
	res.json({result:'success'})
})

app.get("/uploadUrl/:user", function(req, res) {
	randomname = 
		"video/" + 
		req.param('user') + "/" + 
		Math.random().toString(36).substring(2, 15) + 
		Math.random().toString(36).substring(2, 15) + 
		".mp4";
	url = s3urlgen.s3putGen(randomname)
	console.log(randomname)
	console.dir(url)
	res.json(url)
})

app.use('/v1/item', item)
app.use('/v1/login', login)

console.log("Listening on port 7675");
app.listen(7675);
