const mongoose = require('mongoose')

const ItemSchema = new mongoose.Schema({
	_id: String,
	_userid: String,
	upvotes: Number,
	downvotes: Number,
	type: String,
	content: String
})
mongoose.model('Items', ItemSchema)

module.exports.schema = ItemSchema
module.exports.model = mongoose.model('Items')
