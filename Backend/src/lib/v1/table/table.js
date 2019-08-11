var express = require('express')
var router = express.Router()
const mongoose = require('mongoose')
const Items = require('../../schema/Items/Items').model
const Tables = require('../../schema/Tables/Tables').model
const Users = require('../../schema/Users/Users').model
const Favorites = require('../../schema/Favorites/Favorites').model

router.get('/unapproved', function(req, res) {

	console.log("called get unapproved")

	Items.find({validated:false}, function(err, items) {
		if(err!=null) {
			res.send(500).json({error:err})
		} 
		if(items == null) {
			console.log(" >> items is shit")
			res.json({result:[]})
		}
		else {
			console.log(" >> " + items.length + " in result")
			result = []
			for(var i = 0; i < items.length; ++i) {
				result.push(items[i]._id)
			}
			res.json({result:result})
		}
	})
})

router.get('/unapproved/:prefix', function(req, res) {
	const prefix = req.params["prefix"]
	console.log("El prefixo es: " + prefix)
	Items.find({$and:[{_id:RegExp("^"+prefix)}, {validated:false}]}, function(err, items) {
                if(err!=null) {
                        res.send(500).json({error:err})
                }
                if(items == null) {
                        console.log(" >> items is shit")
                        res.json({result:[]})
                }
                else {
                        console.log(" >> " + items.length + " in result")
                        result = []
                        for(var i = 0; i < items.length; ++i) {
                                result.push(items[i]._id)
                        }
                        res.json({result:result})
                }
        })
})

router.post('/table', function(req, res) {
	newTable = req.body
	console.log("Parsed body:")
	console.dir(newTable)
	newTable.ids.forEach(function(id) {
		Items.findOne({_id:id}, function(err, item) {
			if(err == null && item != null) {
				item.validated = true
				item.save()
			}
		})
	})
	newId = newTable.prefix + ":" + mongoose.Types.ObjectId().toString()
	Tables.create({_id:newId, contentIds:newTable.ids}, function(err, newTable) {
		if(err!=null) {
			res.send(500).json({error:err})
		}
		else {
			res.json({result:'success'})
		}
	})
})

router.get('/table/:prefix', function(req, res) {
	const prefix = req.params["prefix"]
	Tables.find({_id:RegExp("^"+prefix)}, function(err, tables) {
		if(err!=null) {
                        res.send(500).json({error:err})
                }
                else if(tables == null) {
                        res.json([])
                }
                else {
                        res.json(tables)
                }
	})
})

router.get('/tables', function(req, res) {
        Tables.find({}, function(err, tables) {
                if(err!=null) {
                        res.send(500).json({error:err})
                }
                else if(tables == null) {
                        res.json([])
                }
                else {
                        res.json(tables)
                }
        })
})

module.exports = router
