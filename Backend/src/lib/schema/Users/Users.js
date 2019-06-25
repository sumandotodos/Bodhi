const mongoose = require('mongoose')
const Items = require('../Items/Items')

const UserSchema = new mongoose.Schema({
	_id: String,
})
mongoose.model('Users', UserSchema)

module.exports.schema = UserSchema
module.exports.model = mongoose.model('Users')
