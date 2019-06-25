var express = require('express')
var router = express.Router()
const mongoose = require('mongoose')
const Items = require('../../schema/Items/Items').model
const Users = require('../../schema/Users/Users').model

router.get('/list', function(req, res) {
	const user = req.headers["userid"]
	console.log("Getting items for user " + req.headers["userid"])
	Items.find({_userid:user}, function(err, items) {
		if (err) {
			console.log("   >> erroraco: " + err)
			res.json({result:'error', error:err})
		}
		else {
			console.log("   >> found")
			result = []
			// traditional for-loop seems faster:
			for(var i = 0; i < items.length; ++i) {
				result.push(items[i]._id)
			}
			res.json({result:'success', data:result})
		}
	})
})

router.get('/list/:userid', function(req, res) {
	const user = req.params["userid"]
        console.log("Getting items for user " + req.params["userid"])
        Items.find({_userid:user}, function(err, items) {
                if (err) {
                        res.json({result:'error', error:err})
                }
                else {
                        result = []
                        // traditional for-loop seems faster:
                        for(var i = 0; i < items.length; ++i) {
                                result.push(items[i]._id)
                        }
                        res.json({result:'success', data:result})
                }
        })
})

router.get('/:id', function(req, res) {
	const id = req.params["id"]
	console.log("Gets item " + req.params["id"])
	Items.findOne({_id:id}, function(err, item) {
		if (err) {
			res.json({result:'error', error:err})
		}
		else {
			if(item==null) {
				res.json({result:'error', error:'no data'})
			}
			else {
				res.json({result:'success', data:item})
			}
		}
	})
})

router.delete('/:id', function(req, res) {
	const id = req.params["id"]
	const currentUser = req.headers["userid"]
	console.log("Deletes item " + req.params["id"])
	Items.findOne({_id:id}, function(err, item) {
		console.log("item._userid = " + item._userid)
		console.log("currentUser = " + currentUser)
		if(item._userid == currentUser) {
			item.remove()
			res.json({result:'success'})
		}
		else {
			res.status(403).json({result:'forbidden'})
		}
	})
})

router.post('/upvote/:id', function(req, res) {
	const id = req.params["id"]
	Items.findOne({_id:id}, function(err, item) {
		if(err!=null) {
			res.status(500).json({result:'error', error:err})
		}
		else if(item == null) {
			res.json({result:'error', error:'no data'})
		}
		else {
			item.upvotes++
			item.save()
			res.json({result:item.upvotes})
		}
	})	
})

router.post('/downvote/:id', function(req, res) {
	const id = req.params["id"]
        Items.findOne({_id:id}, function(err, item) {
                if(err!=null) {
                        res.status(500).json({result:'error', error:err})
                }
                else if(item == null) {
                        res.json({result:'error', error:'no data'})
                }
                else {
                        item.downvotes++
                        item.save()
			res.json({result:item.downvotes})
                }
        })
})

router.post('/comment', function(req, res) {
	const owner = req.headers["userid"]
	Items.create({_id:new mongoose.Types.ObjectId(),_userid:owner,upvotes:0,downvotes:0,type:'comment',content:req.body}, function(err, item) {
		if (err != null) {
			res.status(500).json({result:'error', error:err})
		}
		else {
			res.json({result:'success'})
		}
	})
})

router.post('/echo/:id', function(req, res) {
	var originalPost = req.params["id"]
	var comment = res.body
	var isThereBodyData = true
	if(comment == undefined || comment == "") {
		console.log("El comentario is null")
		isThereBodyData = false
	} else {
		console.log("El comentario no es null: " + res.body + ", y esta niña cocina muy bien, tendríamos que llevarla a Master Chef")
	}
	const owner = req.headers["userid"]
	Items.create({_id:new mongoose.Types.ObjectId(), _userid:owner,
	upvotes:0, downvotes:0, type:'echo', content:JSON.stringify({_id:originalPost,comment:isThereBodyData ? null : comment})},
		function(err, item) {
			if (err != null) {
                        res.status(500).json({result:'error', error:err})
                }
                else {
                        res.json({result:'success'})
                }
	})
})

module.exports = router
