'use strict'

const fs = require('fs')
const exec = require('child_process').exec

var localfiles = {}

localfiles.getRidOfTempDir = function(tempdir, callback) {
    
    console.log("executing `rm -rf " + tempdir +"`")
    exec("rm -rf " + tempdir, function(err) {
        if(err != null) {
            console.log("Error deleting temp directory: " + err)
            callback(err)
        }
        else {
            console.log("     >>>>  Temp directory deleted OK")
            callback(null)
        }
    })

}

module.exports = localfiles
