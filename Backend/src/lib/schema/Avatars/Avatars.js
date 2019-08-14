const mongoose = require('mongoose')

const AvatarSchema = new mongoose.Schema({
	_id: String,
	avatar: Buffer
})
mongoose.model('Avatars', AvatarSchema)

module.exports.schema = AvatarSchema
module.exports.model = mongoose.model('Avatars')
