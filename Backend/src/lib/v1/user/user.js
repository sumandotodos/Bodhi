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

router.get('/profile', function(req, res) {
	console.log("getting profile")
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

module.exports = router
