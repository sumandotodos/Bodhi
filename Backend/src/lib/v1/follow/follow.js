var express = require('express')
var router = express.Router()
const mongoose = require('mongoose')
const Items = require('../../schema/Items/Items').model
const Users = require('../../schema/Users/Users').model
const Favorites = require('../../schema/Favorites/Favorites').model
const Avatars = require('../../schema/Avatars/Avatars').model
const Follows = require('../../schema/Follows/Follows').model
const helpers = require('../Helpers')

router.post('/:otheruserid', function(req, res) {
	const userid = req.header["userid"]
	const otheruser = req.params["otheruserid"]
	Follows.findOne({_userid:userid}, function(err, fol) {
		if(err != null) {
                        res.status(500).json(res);
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
	const userid = req.header["userid"]
	const otheruser = req.params["otheruserid"]
	Follows.findOne({_userid:userid}, function(err, fol) {
                if(err != null) {
                        res.status(500).json(res);
                }
                if(fol == null) {
                        res.status(404).json({result:'user '+ otheruser  +' not found'});
                }
                else {
			var indexOfOtherUser = fol.follows.indexOf(otheruser)
			if(indexOfOtherUser != -1) {
                        	fol.follows.splice(indexOfOtherUser, 1)
                        	fol.markModified('follows')
                        	fol.save()
                        	res.json({result:'success'})
			}
			else {
				res.status(404).json({result:'user '+ otheruser  +' not found'});
			}
                }
        })
})

router.get('/', function(req, res) {
	const userid = req.header["userid"]
	Follows.findOne({_userid:userid}, function(err, fol) {
		if(err != null) {
			res.status(500).json(res);
		}
		else if(fol == null) {
			res.json([])
		}
		else {
			res.json(fol.follows)
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

module.exports = router
