const mongoose = require('mongoose')

const TableSchema = new mongoose.Schema({
	_id: String,
	contentIds: [String],
})
mongoose.model('Tables', TableSchema)

module.exports.schema = TableSchema
module.exports.model = mongoose.model('Tables')
