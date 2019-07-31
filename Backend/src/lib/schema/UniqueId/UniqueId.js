const mongoose = require('mongoose')

const UniqueIdSchema = new mongoose.Schema({
	_id: String,
	counter: Number
})
mongoose.model('UniqueIds', UniqueIdSchema)

module.exports.schema = UniqueIdSchema
module.exports.model = mongoose.model('UniqueIds')
