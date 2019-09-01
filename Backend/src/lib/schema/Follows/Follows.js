const mongoose = require('mongoose')

const FollowSchema = new mongoose.Schema({
	_userid: String,
	follows: [String]
})
mongoose.model('Follows', FollowSchema)

module.exports.schema = FollowSchema
module.exports.model = mongoose.model('Follows')
