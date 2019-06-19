var express = require('express')
var router = express.Router()

router.get('/list', function(req, res) {
	console.log("Getting items for user " + req.headers["userid"])
	res.json({result:'Some items'})
})

router.get('/list/:id', function(req, res) {
	console.log("Getting items for user " + req.params["id"])
        res.json({result:'Some items'})
})

router.get('/:id', function(req, res) {
	console.log("Gets item " + req.params["id"])
	res.json({result:'Item ' + req.params["id"]})
})

router.delete('/:id', function(req, res) {
	console.log("Deletes item " + req.params["id"])
	res.json({result:'success'})
})

module.exports = router
