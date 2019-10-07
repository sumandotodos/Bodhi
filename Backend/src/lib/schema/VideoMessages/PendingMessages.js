const mongoose = require('mongoose')

const PendingMessageSchema = new mongoose.Schema({
	_id: String,
	fromuser: String,
	touser: String,
	content: String,
	contentid: String,
	extra: String
})
mongoose.model('PendingMessages', PendingMessageSchema)

module.exports.schema = PendingMessageSchema
module.exports.model = mongoose.model('PendingMessages')
