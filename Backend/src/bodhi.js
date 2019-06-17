const config = require('./lib/config')
const express = require('express')
const s3urlgen = require('./lib/s3urlGenerator')

app = express()

app.get("/uploadUrl/", function(req, res) {
	randomname = 
		Math.random().toString(36).substring(2, 15) + 
		Math.random().toString(36).substring(2, 15) + 
		".mp4";
	url = s3urlgen.s3putGen(randomname)
	console.log(randomname)
	console.dir(url)
	res.json(url)
})

console.log("Listening on port 7675");
app.listen(7675);
