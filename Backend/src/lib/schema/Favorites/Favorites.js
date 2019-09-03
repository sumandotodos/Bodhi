const mongoose = require('mongoose')

const FavoriteSchema = new mongoose.Schema({
	_userid: String,
	favoritequestion: String,
	favorites: [String]
})
mongoose.model('Favorites', FavoriteSchema)

module.exports.schema = FavoriteSchema
module.exports.model = mongoose.model('Favorites')
