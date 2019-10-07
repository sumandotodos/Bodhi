const mongoose = require('mongoose')

const VideoMessageSchema = new mongoose.Schema({
	_id: String,
	fromuser: String,
	touser: String,
	count: Number
})
mongoose.model('VideoMessages', VideoMessageSchema)

module.exports.schema = VideoMessageSchema
module.exports.model = mongoose.model('VideoMessages')
