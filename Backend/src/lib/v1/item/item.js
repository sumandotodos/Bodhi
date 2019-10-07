var express = require('express')
var router = express.Router()
const mongoose = require('mongoose')
const Items = require('../../schema/Items/Items').model
const Messages = require('../../schema/Messages/Messages').model
const Users = require('../../schema/Users/Users').model
const Favorites = require('../../schema/Favorites/Favorites').model
const VideoMessages = require('../../schema/VideoMessages/VideoMessages').model
const PendingMessages = require('../../schema/VideoMessages/PendingMessages').model
const Avatars = require('../../schema/Avatars/Avatars').model
const helpers = require('../Helpers')
const contents = require('../../contents/contents')
const s3manager = require('../../s3urlGenerator')
const videoencoder = require('../../videoencoding/videoencoding')
const fs = require('fs')

function addMessage(userId, toUserId, type, content, contentid, extra, cb) {
	Messages.create({
        	_id: mongoose.Types.ObjectId(),
                _fromuserid:userId,
                _touserid:toUserId,
                type: type,
                content:content,
                contentid:contentid,
                extra:extra,
                viewed: false
        }, function(err, newmsg) {
		cb(err, newmsg)
	}) 
}

function addPendingMessage(fromuser, touser, content, contentid, extra, cb) {
	PendingMessages.create({
		_id: mongoose.Types.ObjectId(),
		fromuser:fromuser,
		touser:touser,
		content:content,
		contentid:contentid,
		extra:extra
	}, function(err, newpm) {
		cb(err, newpm)
	})
}

function addVideoMessage(fromuser, touser, cB) {
	console.log("Add video message called")
	VideoMessages.findOne({fromuser:fromuser,touser:touser}, function(err, vdmsg) {
		console.log(" >> find one callback.... ")
		if(err!=null) {
			console.log("  >> there was error")
			cB(0)
		}
		else {
			if(vdmsg == null) {
				console.log("  >> no previous record found, creating one ")
				VideoMessages.create({
					_id: mongoose.Types.ObjectId(),
					fromuser:fromuser,
					touser:touser,
					count:1	
				}, function(err, newVdMsg) {
					if(err!=null) {
						cB(0)
					}
					else {
						cB(1)
					}
				})
			}
			else {
				console.log("  >> previous record found, increasing count")
				vdmsg.count++
				vdmsg.markModified('count')
				vdmsg.save(function(err, modifiedvsmsg) {
					if(err!=null) {
						cB(0)
					}
					else {
						cB(modifiedvsmsg.count)
					}
				})
			}
		}
	})
}

function PendingVideosToMessages(fromuser, touser) {
	PendingMessages.find({
                fromuser:fromuser,
                touser:touser}, function(err, msgs) {
			msgs.forEach(function(msg) {
			   addMessage(msg.fromuser,
                                   msg.touser,
                                   'You can now Watch',
                                   msg.content,
                                   msg.contentid,
				   msg.extra,
                                   function(err, msg) {
                                      if(err != null) {
                                         console.log("Error: " + err)
                                      }
                                      else {
                                         console.log("Message added OK")
                                      }
                            })
			    msg.remove()
			})
	})
} 

function getNumberOfVideoMessages(fromuser, touser, cB) {
	console.log("getNumberOfVideoMessages called: " + fromuser + ", " + touser)
	VideoMessages.findOne({fromuser:fromuser,touser:touser}, function(err, vdmsg) {
		if(err != null) {
			cB(0)
		}
		else if(vdmsg==null) {
			cB(0)
		}
		else {
			cB(vdmsg.count)
		}
	})
}

router.put('/video/:touserid/:questionid', function(req, res) {
	console.log(" >>>> post video called params version")
	const userId = req.headers["userid"]
	const toUserId = req.params["touserid"]
	const questionId = req.params["questionid"]
	const data = req.body
	res.json({result:'upload success'})
	videoencoder.encode(data, function(err, tempFileObj) {
		if(err!=null) {
			//res.status(500).json(err)
			console.log("Encode error: " + err)
		}	
		else {
			console.log("    >>> encoding seems ok")
			var fullpath = tempFileObj.directory + "/" + tempFileObj.outfile
			console.log("    >>> Finished encoding: " + fullpath)
			fileObj = generateRandomName(userId)
			var readStream = fs.createReadStream(fullpath)
			readStream.pipe(s3manager.uploadStreamWrapper(
					fileObj.filename, 
					tempFileObj.directory,
					(err) => {
						if(err == null) {
							console.log("Adding message to user " + toUserId)
							contents.resolveContent(questionId, function(err, content) {

								if(err) {
									console.log("Error retrieving content: " + err)
									return
								}
								console.log("contents resolved successfully")
								
								addVideoMessage(userId, toUserId, (count) => {
									PendingVideosToMessages(toUserId, userId)
								} )
								getNumberOfVideoMessages(toUserId, userId, (count) => {
									if(count == 0) {
										addMessage(userId,
											toUserId,
											'Answer to Watch',
											content,
											questionId,
											fileObj.filename, 
											function(err, msg) {
                                                                        		   if(err != null) {
                                                                                	        console.log("Error: " + err)
                                                                        		   }
                                                                        		   else {
                                                                                	        console.log("Message added OK")
                                                                        		   }
                                                                		})
										addPendingMessage(userId, 
											toUserId, 
											content, 
											questionId, 
											fileObj.filename, 
											function(err,nv) {
												if(err != null) {
													console.log("Error: " + err)
												}		
												else {
													console.log("Pending video added ok")
												}
											}
										)
									}
									else {
										console.log("Question Answered branch")
										addMessage(userId,
											toUserId,
											'Question Answered',
											content,
											questionId,
											fileObj.filename, 
											function(err, msg) {
                                                                                	   if(err != null) {
                                                                                              console.log("Error: " + err)
                                                                                	   }
                                                                                	   else {
                                                                                              console.log("Message added OK")
                                                                                	   } 
                                                                                })
									}
								})							
	
							})					
						}
					}))	
		}
	})
})

router.put('/video', function(req, res) {
	console.log(" >>>> post video called ")
	const userId = req.headers["userid"]
	const data = req.body
	res.json({result:'upload success'})
	videoencoder.encode(data, function(err, tempFileObj) {
		if(err!=null) {
			//res.status(500).json(err)
			console.log("Encode error: " + err)
		}	
		else {
			console.log("    >>> encoding seems ok")
			var fullpath = tempFileObj.directory + "/" + tempFileObj.outfile
			console.log("    >>> Finished encoding: " + fullpath)
			fileObj = generateRandomName(userId)
			var readStream = fs.createReadStream(fullpath)
			readStream.pipe(s3manager.uploadStreamWrapper(fileObj.filename, tempFileObj.directory))	
		}
	})
})

router.get('/downloadurl/:colonseparatedfilepath', function(req, res) {
	const userId = req.headers["userid"]
	const filename = req.params["colonseparatedfilepath"]
	const filepath = filename.replace(/:/g, "/")
	url = s3manager.s3getGen(filepath)
	res.json(url)
})

function generateRandomName(userId) {
	const randomId = Math.random().toString(36).substring(2, 15) +
                Math.random().toString(36).substring(2, 15)
    randomname =
        "video/" +
        userId + "/" +
        randomId +
        ".mp4";
    var file = {}
	file.filename = randomname
    file.id = randomId
    return file
}

function generateUploadUrl(userId) {
        var randomFileName = generateRandomName(userId)
	url = s3manager.s3putGen(randomFileName.filename)
        url.filename = randomFileName.filename
	url.id = randomFileName.id
        return url
}

/*router.get('/uploadurl', function(req, res) {
	const userId = req.headers["userid"]
	url = getUploadUrl(userId)
	res.json(url)
})*/

router.put('/avatar', function(req, res) {
        const currentUser = req.headers["userid"]

        console.log("putting avatar called")

        var BufferData = req.body
	console.log("  >>  storing " + BufferData.length + " bytes")
 
	Avatars.findOne({_id:currentUser}, function(err, av) {
		if(err != null) {
                        res.status(500).json({error:err})
                }
		else if (av == null) {
			Avatars.create({_id:currentUser, avatar:BufferData}, function(err, newAvatar) {
		                if(err != null) {
                		        res.status(500).json({error:err})
                		}
                		else if(newAvatar == null) {
                        		res.status(500).json({error:'could not add new avatar data'})
                		}
                		else {
                        		res.json({result:'kosher'})
                		}
        		})
		}
		else {
			av.avatar = BufferData
			av.markModified('avatar');
			av.save()
		}
	})

})

function RetrieveAvatarForUser(user, res) {
	Avatars.findOne({_id:user}, function(err, av) {
		if(err != null) {
                	res.status(500).json({error:err})
                }
		if(av == null) {
			res.status(404).json({result:'not found'})
		}
		else {
			res.set('Content-Type', 'image/jpeg');
        		res.end(av.avatar);
		}
	})
}

router.get('/favoritesandupvotescount', function(req, res) {
        const currentUser = req.headers["userid"]
        helpers.gatherUpvotesAndFavorites(currentUser,
                function(result) {
                        res.json(result)
                },
                function(error) {
                        res.status(500).json(error)
                }
        )
})

router.get('/avatar', function(req, res) {
        const user = req.headers["userid"]
	RetrieveAvatarForUser(user, res)
})

router.get('/avatar/:id', function(req, res) {
	const user = req.params["id"]
	RetrieveAvatarForUser(user, res)
})

router.get('/list', function(req, res) {
	const user = req.headers["userid"]
	console.log("Getting items for user " + req.headers["userid"])
	Items.find({_userid:user}, function(err, items) {
		if (err) {
			console.log("   >> erroraco: " + err)
			res.status(500).json({result:'error', error:err})
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

router.get('/comments/:id', function(req, res) {
        const owner = req.params["id"]
        Items.find({_userid:owner}, function(err, items) {
                result = []
                if(items != null) {
                        for(var i = 0; i < items.length; ++i) {
                                result.push(items[i])
                        }
                }
                res.json({result:result})
        })
})

router.get('/comments', function(req, res) {
        const owner = req.headers["userid"]
        Items.find({_userid:owner}, function(err, items) {
                result = []
		if(items != null) {
                        for(var i = 0; i < items.length; ++i) {
                                result.push(items[i])
                        }
                }
                res.json({result:result})
        })
})

router.get('/comment/:id', function(req, res) {
        const owner = req.headers["userid"]
        const id = req.params["id"]
	// incremening views should go here, i presume...
	Items.findOne({_id:id}, function(err, item) {
		if(err != null) {
			res.status(500).json({result:'error', error:err})
		}
		else if(item == null) {
			res.status(404).json({result:'not found'})
		}
		else {
			item.views = item.views+1
			item.markModified('views')
			item.save()
			res.json(item)
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
/*
function gatherUpvotesAndFavorites(userId, successCallback, errorCallback) {
	Items.find({_userid:userId}, function(err, items) {
		if(err != null) {
			errorCallback(err)
		}
		else if (items == null) {
			successCallback({upvotes:0, favorites:0})
		}
		else {
			var upvotes = 0
			var favorites = 0
			for(var i = 0; i < items.length; ++i) {
				favorites += items[i].favoritized
				upvotes += items[i].upvotes
			}
			successCallback({upvotes:upvotes,favorites:favorites})
		}
	})
}
*/
router.get('/favorites', function(req, res) {
        const currentUser = req.headers["userid"]

        console.log("Asking for favorites of user " + currentUser)

        Favorites.findOne({_userid:currentUser}, function(err, fav) {
                if(err != null) {
                        res.status(500).json({result:'error', error:err})
                }
		else if (fav == null) {
			res.json({result:'warning', warning:'user does not exist'})
		}
                else {
                        res.json({favorites:fav.favorites})
                }
        })
})


router.get('/favorites/:id', function(req, res) {
        const userId = req.params["id"]

        console.log("Asking for favorites of user " + userId)

        Favorites.findOne({_userid:userId}, function(err, fav) {
                if(err != null) {
                        res.status(500).json({result:'error', error:err})
                }
                else if (fav == null) {
                        res.json({result:'warning', warning:'user does not exist'})
                }
                else {
                        res.json({favorites:fav.favorites})
                }
        })
})


router.get('/favoritequestion/:userid', function(req, res) {
	console.log("  >> calling item/favoritequestion/:userid")
        const currentUser = req.headers["userid"]
        Favorites.findOne({_userid:currentUser}, function(err, fav) {
                if(err != null) {
                        res.status(500).json({result:'error', error:err})
                }
                else if (fav == null) {
                        res.json({result:""})
                }
                else {
                        res.json({result:fav.favoritequestion})
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

router.put('/favoritequestion', function(req, res) {
	var qid = req.body
	const currentUser = req.headers["userid"]
	console.log(" >> putting fav question for user " + currentUser + ": " + qid)
	Favorites.findOne({_userid:currentUser}, function(err, fav) {
		if(err != null) {
                        res.status(500).json({result:'error', error:err})
                }
                else if (fav == null) {
                        Favorites.create({_userid:currentUser, favoritequestion:qid, favorites:[qid]}, function(err, newFav) {
                                if(err != null) {
                                        res.status(500).json({result:'error', error:err})
                                }
                                else {
                                        success = true
                                        res.json({result:'success'})
                                }
                        })
                }
		else {
			fav.favoritequestion = qid
			fav.markModified('favoritequestion')
			fav.save()
			res.json({result:'success'})
		}
	})
})

router.post('/favorite/:id', function(req, res) {
	const id = req.params["id"]
        const currentUser = req.headers["userid"]

	console.log("Favoriting: " + id + " by " + currentUser)

	Favorites.findOne({_userid:currentUser}, function(err, fav) {
		var success = false
		if(err != null) {
			res.status(500).json({result:'error', error:err})
		} 
		else if (fav == null) {
			var newFavQuestion = ""
			if(helpers.isQuestion(id)) {
				newFavQuestion = id
			}
			Favorites.create({_userid:currentUser, favoritequestion:newFavQuestion, favorites:[id]}, function(err, newFav) {
				if(err != null) {
					res.status(500).json({result:'error', error:err})
				} 
				else {
					success = true	
					res.json({result:'success'})
				}
			})
		}
		else {
			if(fav.favorites.length == 0 && helpers.isQuestion(id)) {
				fav.favoritequestion = id
				fav.markModified('favoritequestion')
			}
			fav.favorites.push(id)
			fav.save()
			success = true
			res.json({result:'success'})
		}
		if(success) {
			Items.findOne({_id:id}, function(err, item) {
                                        if(err == null && item != null) {
                                                item.favoritized++
						item.save()
						//var owner = item._userid
                                                //Users.findOne({_id:owner}, function(err, user) {
                                                //        if(err == null && user != null) {
                                                //                user.favoritized++
                                                //                user.save()
                                                //        }
                                                //})
                                        }
                         })
		}
	})	
})

router.post('/exchangefavorite/:from/:to', function(req, res) {
	const from = parseInt(req.params["from"])
	const to = parseInt(req.params["to"])
	const currentUser = req.headers["userid"]	

	console.log("Exchanging favorite items " + from + " and " + to)

	Favorites.findOne({_userid:currentUser}, function(err, favs) {
		if(err != null) {
                        res.status(500).json({result:'error', error:err})
                }
		else if(favs == null) {
			res.status(400).json({result:'inexistent items'})
		}
		else if(to>=favs.favorites.length || from>favs.favorites.length) {
			res.status(400).json({result:'inexistent items'})
		}
		else {
			//console.log("   >> Favs before: " + JSON.stringify(favs.favorites))
			[favs.favorites[from], favs.favorites[to]] = [favs.favorites[to], favs.favorites[from]]
			//t = favs.favorites[to]
			//favs.favorites[to] = favs.favorites[from]
			//favs.favorites[from] = t 
			favs.save()
			favs.markModified('favorites');
			//console.log("   >> Favs after: " + JSON.stringify(favs.favorites))
			res.json({result:'success'})
		}
	})
})

router.get('/favorite/:id', function(req, res) {
	const id = req.params["id"]
	const currentUser = req.headers["userid"]

	console.log("Consulting if " + id + " is favved by " + currentUser + "...")

	Favorites.findOne({_userid:currentUser}, function(err, fav) {
		if(err != null) {
			res.status(500).json({result:'error', error:err})
		}
		else {
			var result = (fav.favorites.indexOf(id) != -1)
			console.log("  >>> ...and the result is: " + result)
			res.json({result:result})
		}
	})
})

router.delete('/favorite/:id', function(req, res) {
	const id = req.params["id"]
        const currentUser = req.headers["userid"]

	console.log("Defavoriting: " + id + " by " + currentUser)

	Favorites.findOne({_userid:currentUser}, function(err, fav) {
		if(err != null) {
                        res.status(500).json({result:'error', error:err})
                }
		else {
			var indexOfFav = fav.favorites.indexOf(id)
			if(indexOfFav != -1) {
				fav.favorites.splice(indexOfFav,1)
				fav.markModified('favorites')
				fav.save()
				//Items.findOne({_id:id}, function(err, item) {
				//	if(err == null && item != null) {
						//var owner = item._userid
						//Users.findOne({_id:owner}, function(err, user) {
                                        	//	if(err == null && user != null) {
                                                //		user.favoritized = user.favoritized > 0 ? user.favoritized - 1 : 0
                                                //		user.save()
                                        	//	}
                                		//})
				//	}
				//})
				res.json({result:'success'})
			}
			else {
				res.status(400).json({result:'error', error:(id+" was not favorited by user "+currentUser)})
			}
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
			// do not check state change: not critical
			item.upvotes++
			item.save()
			//var owner = item._userid
			//Users.findOne({_id:owner}, function(err, user) {
			//	if(err == null && user != null) {
			//		user.upvotes++
			//		user.save()
			//	}
			//})	
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
			// do not check state change: not critical
                        item.downvotes++
                        item.save()
			//var owner = item._userid
			//Users.findOne({_id:owner}, function(err, user) {
                        //        if(err == null && user != null) {
                        //                user.downvotes++
                        //                user.save()
                        //        }
                        //})
			res.json({result:item.downvotes})
                }
        })
})

router.post('/comment/:prefix', function(req, res) {
	const owner = req.headers["userid"]
	const prefix = req.params["prefix"]
	newId = prefix + mongoose.Types.ObjectId().toString()	
	Items.create({_id:newId,_userid:owner,views:0,favoritized:0,upvotes:0,downvotes:0,type:'comment',content:req.body.comment,validated:false}, function(err, item) {
		if (err != null) {
			res.status(500).json({result:'error', error:err})
		}
		else {
			res.json({result:'success'})
		}
	})
})

router.put('/comment', function(req, res) {
        const owner = req.headers["userid"]
	// should remove this one?
        console.log(" put comment called, user: " + owner + ", comment body: " + JSON.stringify(req.body))

	res.json({result:'success'})
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
	Items.create({_id:new mongoose.Types.ObjectId(), _userid:owner, views:0, favoritized:0,
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
