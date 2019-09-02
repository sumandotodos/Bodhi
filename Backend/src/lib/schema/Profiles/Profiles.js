const mongoose = require('mongoose')

const ContactOptionSchema = new mongoose.Schema({
	description: String,
	allow: Boolean
})

const ProfileSchema = new mongoose.Schema({
	_userid: String,
	about: String,
	phone: String,
	email: String,
	contactoptions: [ContactOptionSchema]
})
mongoose.model('Profiles', ProfileSchema)

module.exports.schema = ProfileSchema
module.exports.model = mongoose.model('Profiles')
