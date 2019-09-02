const Items = require('../schema/Items/Items').model
const Users = require('../schema/Users/Users').model

helpers = {}

helpers.randomIntRange = function(minInc, maxInc) {
        return minInc + Math.floor(Math.random() * (maxInc + 1 - minInc));
}

helpers.sampleArray = function(arr, samples) {
        var indexes = []
        var length = arr.length
	console.log("Length of array: " + length)
        for(var i = 0; i < samples ; ++i) {
		var val = Math.floor(Math.random() * length);
		indexes.push(val)
		var idx = indexes.length-1
	}
        result = []
        for(var i = 0; i < samples; ++i) {
                result.push(arr[indexes[i]])
        }
        return result
}

helpers.gatherUpvotesAndFavorites = function(userId, successCallback, errorCallback) {
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

helpers.getFavoritesAndUpvotesAdvantage = function(userId, successCallback, errorCallback, update) {
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

module.exports = helpers

