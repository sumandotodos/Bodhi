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

router.post('/advantageReport', function(req, res) {
	
})
/*function getFavoritesAndUpvotesAdvantage(userId, successCallback, errorCallback, update) {
	gatherUpvotesAndFavorites(usedId,
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
						user.markMofified('upvotes')
						user.save()
					}
					successCallback({
						favorites:result.favorites-lastFavs,
						upvotes:result.upvotes-lastUpvotes
					})
				}
			})
		},
		function(error) {
			errorCallback(error)
		}
	)
}
*/
router.get('/getuploadurl', function(req, res) {
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
})

router.post('/answertoquestion/:qid/:toid/:downloadurl', function(req, res) {
	const currentUser = req.headers["userid"]
	const toUser = req.params["toid"]
	const downloadUrl = req.params["downloadurl"]
	const questionId = req.params["qid"]
// if it is not built in contents, retrieve question from items...	
	Uploads.create({
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
	})
})

router.post('/connectrequest/:toid', function(req, res) {
	const currentUser = req.headers["userid"]
	const toUser = req.params["toid"]
	Messages.create({
		_id: mongoose.Types.ObjectId(),
		_fromuserid:currentUser, 
		_touserid:toUser, 
		type: 'Connect Request',
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

module.exports = router

