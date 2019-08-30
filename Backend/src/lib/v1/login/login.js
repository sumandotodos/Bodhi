var express = require('express')
var helpers = require('../../helpers')
var router = express.Router()
const mongoose = require('mongoose')
var UniqueIds = require('../../schema/UniqueId/UniqueId').model
var Users = require('../../schema/Users/Users').model

inRAMUserList = []

router.get('/handle/:id', function(req, res) {
        const userid = req.params["id"]
        GetUserHandle(userid, res)
})

router.get('/handle', function(req, res) {
        const currentUser = req.headers["userid"]
        GetUserHandle(currentUser, res)
})

router.put('/handle', function(req, res) {
        const currentUser = req.headers["userid"]
        var newHandle = req.body

        console.log(" putting new handle for user " + currentUser + ": " + newHandle)

	if(!ValidateHandle(newHandle)) {

		res.status(400).json({result:'invalid handle'})

	}
	else {

        	Users.findOne({_id:currentUser}, function(err, user) {
                	if(err != null) {
                        	res.status(500).json({error:err})
                	}
                	else if(user == null) {
                        	res.status(404).json({result:'not found'})
                	}
                	else {
                        	user.handle = newHandle
             			user.markModified('handle')
				user.save()
		        	res.json({result:'success'})
                	}
        	})

	}
})

function findUserInList(user) {
	index = inRAMUserList.map( (e) => (e.id == user) ).indexOf(true)
	return index;
}

router.get('/uniqueid', function(req, res) {
	GetAndIncrementCounter(function(result) {
		res.json({result:result})
	}, function(err) {
		res.status(500).json({result:'error', error:err})
	})
})

function GetAndIncrementCounter(callbackSuccess, callbackError) {
	UniqueIds.findOne({_id:"0"}, function(err, uid) {
                if(err != null) {
                        callbackError(err)
                }
                else if(uid == null) {
                        UniqueIds.create({_id:"0",counter:0}, function(err, uid) {
                                if(err!=null) {
                                	callbackError(err)
				}
                                else {
                                        callbackSuccess(0)
                                }
                        })
                }
                else {
                        var result = uid.counter
                        uid.counter++
                        uid.save()
                        callbackSuccess(result)
                }
        })
}

router.get('/:id', function(req, res) {
	var token
	const userid = req.params["id"]
	if(findUserInList(userid) != -1) {
		res.json({token:inRAMUserList[index].token})
	}
	else {
		newToken = helpers.generateRandomString(12)
		inRAMUserList.append({id:userid,token:newToken})
		res.json({token:newToken})
	}
})

router.get('/user/:deviceuuid/:apptoken', function(req, res) {
	console.log("getting bodhi user id from app token: " + req.params["apptoken"])
	const apptoken = req.params["apptoken"]
	const deviceuuid = req.params["deviceuuid"]
	Users.findOne({appid:apptoken}, function(err, user) {
		if(err!=null) {
			res.send(500).json({result:err})
		}
		else if (user==null) {
			GetAndIncrementCounter( function(result) {
				Users.create({_id:result, appid:apptoken, deviceuuid:deviceuuid, handle:result,
						views:0, upvotes:0, downvotes:0, favoritized:0}, function(err, user) {
					if(err!=null) {
						console.log("error: " + err)
						res.status(500).json({result:'error', error:err})
					}
					else {
						console.log("AOK")
						res.json({result:user._id})
					}
				})
			}, function(err) {
				console.log("error: " + err)
				res.status(500).json({result:'error', error:err})
			})
		}
		else {
			console.log("AOK")
			res.json({result:user._id})
		}
	})
})

function GetUserHandle(id, res) {
	Users.findOne({_id:id}, function(err, user) {
		if(err != null) {
			res.status(500).json({error:err})
		}
		else if(user == null) {
			res.status(404).json({result:'not found'})
		}
		else {
			res.json({result:user.handle})
		}
	})
}

function ValidateHandle(handle) {
	if(handle.length < 4) {
		return false
	}
	if(handle.length > 32) {
		return false
	}
	if(handle.indexOf(" ") != -1) {
		return false
	}
	if(handle.indexOf(":") != -1) {
		return false
	}
	if(handle.indexOf("/") != -1) {
		return false
	}
	return true
}

module.exports = router
