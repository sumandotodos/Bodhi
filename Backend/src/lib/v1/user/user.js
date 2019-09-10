var express = require('express')
var router = express.Router()
const mongoose = require('mongoose')
const Items = require('../../schema/Items/Items').model
const Users = require('../../schema/Users/Users').model
const Favorites = require('../../schema/Favorites/Favorites').model
const Profiles = require('../../schema/Profiles/Profiles').model
const Avatars = require('../../schema/Avatars/Avatars').model
const Follows = require('../../schema/Follows/Follows').model
const helpers = require('../Helpers')

function blankProfile() {
	return {
		about: "",
		phone: "",
		email: "",
		contactoptions: []
	}
}

router.get('/profile/:id', function(req, res) {
        console.log("getting profile for user " + req.params["id"])
        userId = req.params["id"]
        Profiles.findOne({_userid:userId}, function(err, profile) {
                if(err != null) {
                        res.status(500).json(err)
                }
                else if(profile == null) {
                        res.json(blankProfile())
                }
                else {
                        res.json(profile)
                }
        })
})

router.get('/profile', function(req, res) {
	console.log("getting profile for user in headers")
	currentUser = req.headers["userid"]
	Profiles.findOne({_userid:currentUser}, function(err, profile) {
		if(err != null) {
			res.status(500).json(err)
		}
		else if(profile == null) {
			res.json(blankProfile())
		}
		else {
			res.json(profile)
		}
	})
})

router.put('/profile', function(req, res) {
	console.log("putting profile")
	currentUser = req.headers["userid"]
	newProfileObject = req.body
	Profiles.findOne({_userid:currentUser}, function(err, profile) {
		if(err != null) {
                        res.status(500).json(err)
                }
		else if(profile == null) {
			Profiles.create({
				_userid:currentUser,
				about:newProfileObject.about,
				phone:newProfileObject.phone,
				email:newProfileObject.email,
				contactoptions:newProfileObject.contactoptions
			}, function(err, newprof) {
				if(err != null) {
                        		res.status(500).json(err)
                		}
			})
		}
		else {
			profile.about = newProfileObject.about
			profile.phone = newProfileObject.phone
			profile.email = newProfileObject.email
			profile.contactoptions = newProfileObject.contactoptions
			profile.markModified('about')
			profile.markModified('phone')
			profile.markModified('email')
			profile.markModified('contactoptions')
			profile.save()
			res.json({result:'success'})
		}
	})
})

function GetUserIndex(user, successCallback, errorCallback) {
	Users.find({}, function(err, users) {
                if(err != null) {
                	errorCallback(err)
                }
                else if(users == null) {
                	successCallback({result:-1})
                }
                else {
                        successCallback({result:helpers.conditionalIndexOf(users,
				(u) => { return u._id == user })})
                }
        })
}

router.get('/index', function(req, res) {
	const currentUser = req.headers["userid"]
	GetUserIndex(currentUser,
		function(data) {
			res.json(data)
		},
		function(error) {
			res.status(500).json(error)
		}
	)
})

router.get('/profileandquestion/:user', function(req, res) {
	const user = req.params["user"]
	var result = {}
	console.log("1")
	Favorites.findOne({_userid:user}, function(err, fav) {
		if(err != null) {
                	res.status(500).json(err)
                	return
		}
		else if(fav == null) {
			result.favquestionid = ""
			result.favquestion = ""
		}
		else {
			if(helpers.isLocalContent(fav.favoritequestion)) {
				result.favquestionid = fav.favoritequestion
				result.favquestion = ""
			}
			else {
				result.favquestionid = fav.favoritequestion
				Items.findOne({_id:fav.favoritequestion}, function(err, item) {
					if(err != null) {
						res.status(500).json(err)
					}
					else if(item == null) {
						result.favquestion = ""
					}
					else {
						result.favquestion = item.content
					}
				})
			}
		}
		console.log("en este punto " + result + " user: " + user)
		Profiles.findOne({_userid:user}, function(err, profile) {
			if(err != null) {
				console.log(" 2")
				res.status(500).json(err)
			}
			else if (profile == null) {
				console.log(" 3")
				result.profile = ""
				result.contactoptions = []
				res.json(result)
			}
			else {
				console.log(" 4")
				result.profile = profile.about
				result.contactoptions = profile.contactoptions
				res.json(result)
			}	
		})
	})
})

module.exports = router
