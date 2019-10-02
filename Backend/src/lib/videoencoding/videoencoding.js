var express = require('express')
var multer  = require('multer')
const exec = require("child_process").exec
const config = require('../config')
const helpers = require('../helpers')
const bodyParser = require('body-parser')
const EncodeDir = config.tempdir
const fs = require('fs')

var encoding = {}

function makeEncodeCommand(tempdir, infile) {
	var outfile = helpers.generateRandomString(20) + ".mp4"
	return {
		command: "ffmpeg -i " + EncodeDir + "/" + 
		tempdir + "/" + infile 
		+ " -vf scale=-1:512 " + EncodeDir + "/" + 
		tempdir + "/" + outfile,
		outfile: outfile, 
		directory: EncodeDir + "/" + tempdir
	} 
}

encoding.encode = function (data, callback) {
  	console.log(" >> encoding.encode   called   with length: " + data.length)
  	const infilename = helpers.generateRandomString(20)
  	const tempdir = helpers.generateRandomString(20)
  	exec("mkdir " + EncodeDir + "/" + tempdir, null, function(err) {
		if(err) {
			callback(err, null)
		}
		else {
			console.log("  created temp dir successfully")
			fs.writeFile(EncodeDir + "/" + tempdir + "/" + infilename, data, function(err) {
				if(err) {
					callback(err, null)
				}
				else {
					console.log("   wrote temp input file successfully")
					const encodeCommand = makeEncodeCommand(tempdir, infilename)
					console.log("   encode command: " + JSON.stringify(encodeCommand))
					var result = exec(encodeCommand.command, null, function(err) {
						if(err) {
							callback(err, null)
						}
						else {
							console.log("   encoding finished, calling callback with success")
							callback(null, {outfile:encodeCommand.outfile, directory:encodeCommand.directory})
						}
					})
				}
			})
		}
  	})
}

module.exports = encoding
