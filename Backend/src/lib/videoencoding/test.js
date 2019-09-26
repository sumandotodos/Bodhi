var express = require('express')
var multer  = require('multer')
const exec = require("child_process").execSync
const config = require('../config')
const helpers = require('../helpers')
const bodyParser = require('body-parser')
const EncodeDir = config.tempdir
const fs = require('fs')

var upload = multer({ dest: EncodeDir })

var app = express()

app.use(bodyParser.raw({type: 'application/octet-stream', limit : '50mb'}))

function makeEncodeCommand(tempdir, infile) {
	var outfile = helpers.generateRandomString(20) + ".mp4"
	return {
		command: "ffmpeg -i " + EncodeDir + "/" + 
		tempdir + "/" + infile 
		+ " " + EncodeDir + "/" + 
		tempdir + "/" + outfile,
		outfile: outfile
	} 
}

function ConvertVideo() {
	const tempdir = helpers.generateRandomString(20)
	exec("mkdir " + EncodeDir + "/" + tempdir)
}

app.get("/", function(req, res) {
	res.json({res:'cool'})
})

app.post('/', function (req, res) {
  console.log("called: " + req.body.length)
  const infilename = helpers.generateRandomString(20)
  const tempdir = helpers.generateRandomString(20)
  exec("mkdir " + EncodeDir + "/" + tempdir)
  fs.writeFileSync(EncodeDir + "/" + tempdir + "/" + infilename, req.body)
  const encodeCommand = makeEncodeCommand(tempdir, infilename)
  var result = exec(encodeCommand.command)  
 res.json({res:encodeCommand.outfile})
	// req.file is the `avatar` file
  //   // req.body will hold the text fields, if there were any
  
})

//var command = MakeEncodeCommand('shitz.mov')

//var result = exec(command.command)

//console.log(result.toString("utf8"))
app.listen(41666)
