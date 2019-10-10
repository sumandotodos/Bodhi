const mongoose = require('mongoose')

const CommsPrefSchema = new mongoose.Schema({
	_id: String,
	fromuserid: String,
	touserid: String,
	index: Number
})
mongoose.model('CommsPreferences', CommsPrefSchema)

module.exports.schema = CommsPrefSchema
module.exports.model = mongoose.model('CommsPreferences')
