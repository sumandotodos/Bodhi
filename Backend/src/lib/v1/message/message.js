var express = require('express')
var router = express.Router()
const mongoose = require('mongoose')
const Items = require('../../schema/Items/Items').model
const Messages = require('../../schema/Messages/Messages').model
const Users = require('../../schema/Users/Users').model
const Favorites = require('../../schema/Favorites/Favorites').model
const Avatars = require('../../schema/Avatars/Avatars').model
const s3urlgen = require('../../s3urlGenerator')
const Uploads = require('../../schema/Uploads/Uploads').model
const helpers = require('../Helpers')

router.get('/', function(req, res) {
	const currentUser = req.headers["userid"]
	Messages.find({_touserid:currentUser}, function(err, msgs) {
		if(err != null) {
			res.status(500).json({error:err})
		}
		else if (msgs == null) {
			res.json({result:[]})		
		}
		else {
			for(var i = 0; i < msgs.length; ++i) {
				msgs[i].viewed = true
				msgs[i].markModified('viewed')
				msgs[i].save()
			}
			res.json({result:msgs})
		}
	})
})

router.get('/unreadcount', function(req, res) {
	const currentUser = req.headers["userid"]
	Messages.find({_touserid:currentUser}, function(err, msgs) {
		if(err != null) {
			res.status(500).json({error:err})
		}
		else if(msgs == null) {
			res.json({result:0})	
		}
		else {
			var unreadMessages = 0
			for(var i = 0; i < msgs.length; ++i) {
				if(msgs[i].viewed == false) {
					unreadMessages++
				}
			}
			res.json({result:unreadMessages})
		}
	})
})

function attemptToIssueAdvantageReport(userid, callbackSuccess, callbackError) {
	getFavoritesAndUpvotesAdvantage(userid,
                function(result) {
                        if(result.favorites > 0 || result.upvotes > 0) {
                                Messages.create({
                                        _id: mongoose.Types.ObjectId(),
                                        _fromuserid:"system",
                                        _touserid:userid,
                                        type: 'Performance Report',
                                        content: '',
                                        extra: JSON.stringify(result),
                                        viewed: false
                                }, function(err, msg) {
                                        if(err != null) {
                                                //res.status(500).json({error:err})
                                        	callbackError(err)
					}
                                        else {
                                                //res.json({result:'success'})
                                                callbackSuccess({result:'success'})
                                        }
                                })
                        }
                        else {
                                //res.json(result)
                        	callbackSuccess(result)
			}

                },
                function(err) {
                        //res.status(500).json(err)
                	callbackError(err)
		},
                true
        )
}

router.post('/advantagereport', function(req, res) {
	const userid = req.headers["userid"]
	attemptToIssueAdvantageReport(userid, 
		function(result) {
			res.json(result)
		},
		function(err) {
			res.status(500).json(err)
		}
	)
})

setInterval(function() {

	Users.find({}, function(err, user) {
		if(err == null && user != null) {
			attemptToIssueAdvantageReport(user._id, 
				function(result) {
					// nothing here
				},
				function(err) {
					console.log("Error issuing advatage report: " + err)
				}
			)	
		}
	})

	}, 1000 * 60 * 60 * 24)


function getFavoritesAndUpvotesAdvantage(userId, successCallback, errorCallback, update) {
	helpers.gatherUpvotesAndFavorites(userId,
		function(result) {
			Users.findOne({_id:userId}, function(err, user) {
				if(err != null) {
					errorCallback(err)
				}
				else if (user == null) {
					successCallback(result)
				}
				else {
					var lastFavs = user.favoritized
					var lastUpvotes = user.upvotes
					if(update) {
						user.favoritized = result.favorites
						user.upvotes = result.upvotes
						user.markModified('favoritized')
						user.markModified('upvotes')
						user.save()
					}
					successCallback({
						favorites:result.favorites>lastFavs ? result.favorites : -1,
						upvotes:result.upvotes>lastUpvotes ? result.upvotes : -1
					})
				}
			})
		},
		function(error) {
			errorCallback(error)
		}
	)
}

/*router.get('/getuploadurl', function(req, res) {
	const currentUser = req.headers["userid"]
	randomname =
                "video/" +
                currentUser + "/" +
                Math.random().toString(36).substring(2, 15) +
                Math.random().toString(36).substring(2, 15) +
                ".mp4";
        url = s3urlgen.s3putGen(randomname)
	console.log(randomname)
        console.dir(url)
        res.json(url)
})*/

router.post('/', function(req, res) {
	var obj = req.body
	Messages.create({
                        _id: mongoose.Types.ObjectId(),
                        _fromuserid:obj.fromuserid,
                        _touserid:obj.touserid,
                        type: obj.type,
                        content: obj.content,
                        extra: obj.extra,
                        viewed: false
                  }, function(err, msg) {
                        if(err != null) {
                                res.status(500).json({error:err})
                        }
                        else {
                                res.json({result:'success'})
                        }
                  })
})

router.post('/answertoquestion/:qid/:toid/:downloadurl', function(req, res) {
	const currentUser = req.headers["userid"]
	const toUser = req.params["toid"]
	const downloadUrl = req.params["downloadurl"]
	const questionId = req.params["qid"]
// if it is not built in contents, retrieve question from items...	
	/*Uploads.create({
		_fromuserid: currentUser,
		_touserid: toUser,
		url: downloadUrl,
		views: 0,
		favoritized: 0,
		validated: false
	}, function(err, upl) {
		if(err != null) {
			res.status(500).json({error:err})
		}
		else {
			
		}
	})*/
	Messages.create({
                        _id: mongoose.Types.ObjectId(),
                        _fromuserid:currentUser,
                        _touserid:toUser,
                        type: 'Question Answered',
                        content: '',
                        extra: '',
                        viewed: false
                  }, function(err, msg) {
                        if(err != null) {
                                res.status(500).json({error:err})
                        }
                        else {
                                res.json({result:'success'})
                        }
                  })	
})

router.post('/connectrequest/:toid', function(req, res) {
	const currentUser = req.headers["userid"]
	const toUser = req.params["toid"]
	Users.findOne({_id:currentUser}, function(err, user) {
	
		if(err != null) {
			res.status(500).json(err)
		}	
		else if(user == null) {
			res.status(500).json({result:'error retrieving user'})
		}
		else {
		  var currentUserHandle = user.handle
		  Messages.create({
			_id: mongoose.Types.ObjectId(),
			_fromuserid:currentUser, 
			_touserid:toUser, 
			type: 'Connect Request',
			content: '',
			extra: currentUserHandle,
			viewed: false
		  }, function(err, msg) {
			if(err != null) {
				res.status(500).json({error:err})
			} 
			else {
				res.json({result:'success'})
			}
		  })
		}		

	})
})

router.delete('/:id', function(req, res) {
	var id = req.params["id"]
	console.log("Delete message " + id + " called")
	Messages.findOneAndRemove({_id:id}, function(err) {
		if(err != null) {
			res.status(500).json(err)
		}
		else {
			res.json({result:'success'})
		}
	})
})

module.exports = router

