var express = require('express')
var router = express.Router()
const mongoose = require('mongoose')
const Items = require('../../schema/Items/Items').model
const Users = require('../../schema/Users/Users').model
const Favorites = require('../../schema/Favorites/Favorites').model
const CommsPreferences = require('../../schema/CommsPreferences/CommsPreferences').model
const Avatars = require('../../schema/Avatars/Avatars').model
const Follows = require('../../schema/Follows/Follows').model
const helpers = require('../Helpers')

router.post('/:otheruserid', function(req, res) {
	const userid = req.headers["userid"]
	const otheruser = req.params["otheruserid"]
	Follows.findOne({_userid:userid}, function(err, fol) {
		if(err != null) {
                        res.status(500).json(err);
                }
		if(fol == null) {
			Follows.create({
				_userid:userid,
				follows: [otheruser]
			}, function(err, newfol) {
				if(err != null) {
                        		res.status(500).json(res);
                		}
				else {
					res.json({result:'success'})
				}
			})
		}
		else {
			if(fol.follows.indexOf(otheruser) == -1) {
				fol.follows.push(otheruser)
				fol.markModified('follows')
				fol.save()
			}
			res.json({result:'success'})
		}
	})
})

router.delete('/:otheruserid', function(req, res) {
	const userid = req.headers["userid"]
	const otheruser = req.params["otheruserid"]
	console.log("follow delete " + otheruser + " called")
	Follows.findOne({_userid:userid}, function(err, fol) {
                if(err != null) {
			console.log(" .... server err: " + err)
                        res.status(500).json(err);
                }
                if(fol == null) {
			console.log(" .... user not found 1")
		        res.json({result:'user '+ otheruser  +' not found'});
                }
                else {
			var indexOfOtherUser = fol.follows.indexOf(otheruser)
			if(indexOfOtherUser != -1) {
                        	fol.follows.splice(indexOfOtherUser, 1)
                        	fol.markModified('follows')
                        	fol.save()
				console.log(" .... success ")
                        	res.json({result:'success'})
			}
			else {
				console.log(".... user not found 2")
				res.json({result:'user '+ otheruser  +' not found'});
			}
                }
        })
})

router.get('/', function(req, res) {
        const userid = req.headers["userid"]
        console.log("Asking for followeds of user " + userid)
	Follows.findOne({_userid:userid}, function(err, fol) {
                if(err != null) {
                        res.status(500).json(err);
                }
                else if(fol == null) {
			console.log("was not found....., so []")
                        res.json({result:[]})
                }
                else {
                        Users.find({}, function(err, users) {
                                if(err != null) {
                                        res.status(500).json(res);
                                }
                                else if(users == null) {
                                        res.json({result:[]})
                                }
                                else {
                                        result = []
                                        for(var i = 0; i < users.length; ++i) {
                                                if(fol.follows.indexOf(users[i]._id) != -1) {
                                                        result.push(users[i])
                                                }
                                        }
                                        res.json({result:result})
                                }
                        })
                }
        })
})

router.get('/:otheruserid', function(req, res) {
	const userid = req.headers["userid"]
	const otheruserid = req.params["otheruserid"]
	Follows.findOne({_userid:userid}, function(err, fol) {
		if(err != null) {
			res.status(500).json(err)
		}
		else if (fol == null) {
			res.status(200).json({result:false})
		}
		else {
			var boolres = fol.follows.indexOf(otheruserid) != -1
			res.status(200).json({result:boolres})
		}
	}) 
})

const MaxUsers = 6

function GetRandomUsers(maxUsers, successCallback, errorCallback) {
	Users.find({}, function(err, users) {
		if(err != null) {
			errorCallback(err)
		}
		else if(users == null) {
			successCallback([])
		}
		else {
			successCallback(helpers.sampleArray(users, maxUsers))
		}
	})
}

router.get('/randomusers/:session/:cursor/:max', function(req, res) {
	const currentUser = req.headers["userid"]
	const session = req.params["session"]
	var cursor = parseInt(req.params["cursor"])
	var max = parseInt(req.params["max"])
	Users.find({}, function(err, users) {
		if(err != null) {
			res.status(500).json(err)
		}
		else if(users == null) {
			res.json({result:[]})
		}
		else {
			var currentUserIndex = helpers.conditionalIndexOf(users, (u) => { return u._id == currentUser })
			console.log("  >> ignoring index " + currentUserIndex + " belonging to user " + currentUser)
			var usersLength = users.length
			res.json({result:helpers.sampleArrayWithIndexes(users,
				helpers.indexSamples(usersLength, session, currentUserIndex, cursor, max))}) 
		}
	})
})

router.get('/randomusers', function(req, res) {
	GetRandomUsers(MaxUsers,
		function(users) {
			res.json({result:users})
		},
		function(error) {
			res.status(500).json(error)
		}
	)
})

router.get('/randomusers/:max', function(req, res) {
	const max = req.params["max"]
	GetRandomUsers(max,
		function(users) {
			res.json({result:users})
		},
		function(error) {
			res.status(500).json(error)
		}
	)
})

router.get('/commprefs/:user/:otheruser', function(req, res) {
	const user = req.params["user"]
	const otheruser = req.params["otheruser"]
	CommsPreferences.findOne({fromuserid:user,touserid:otheruser}, function(err, pref) {
		if(err!=null) {
			res.status(500).json(err)
		}
		else if (pref == null) {
			res.json({result:-1})
		}
		else {
			res.json({result:pref.index})
		}
	})
})

router.put('/commprefs/:otheruser/:index', function(req, res) {
	console.log("put commprefs called")
	const user = req.headers["userid"]
	const otheruser = req.params["otheruser"]
	const index = parseInt(req.params["index"])
	console.log("Putting comms " + user + "->" + otheruser + " to " + index)
	CommsPreferences.findOne({fromuserid:user,touserid:otheruser}, function(err, pref) {
		if(err!=null) {
			res.status(500).json(err)
		}
		else if (pref == null) {
			CommsPreferences.create({
				_id:  mongoose.Types.ObjectId(),
				fromuserid: user,
				touserid: otheruser,
				index: index
			}, function(err, newpref) {
				if(err!=null) {
					res.status(500).json(err)
				}
				else {
					res.json({result:'success'})
				}
			})
		}
		else {
			pref.index = index
			pref.save(function(err) {
				if(err!=null) {
					res.status(500).json(err)
				}
				else {
					res.json({result:'success'})
				}
			})
		}
	})
})

module.exports = router
