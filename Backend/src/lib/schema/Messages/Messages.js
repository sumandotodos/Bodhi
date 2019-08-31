const mongoose = require('mongoose')

const MessageSchema = new mongoose.Schema({
	_id: String,
	_fromuserid: String,
	_touserid: String,
	type: String,
	content: String,
	extra: String,
	viewed: Boolean
})
mongoose.model('Messages', MessageSchema)

module.exports.schema = MessageSchema
module.exports.model = mongoose.model('Messages')
