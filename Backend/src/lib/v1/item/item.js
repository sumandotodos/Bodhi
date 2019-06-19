var express = require('express')
var router = express.Router()

router.get('/:id', function(req, res) {
	console.log("Gets item " + req.params["id"])
})

router.delete('/:id', function(req, res) {
	console.log("Deletes item " + req.params["id"])
})
