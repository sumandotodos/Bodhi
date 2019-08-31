const mongoose = require('mongoose')

const UploadSchema = new mongoose.Schema({
	_id: String,
	_fromuserid: String,
	_touserid: String,
	url: String,
	views: Number,
	favoritized: Number,
	validated: Boolean
})
mongoose.model('Uploads', UploadSchema)

module.exports.schema = UploadSchema
module.exports.model = mongoose.model('Uploads')
