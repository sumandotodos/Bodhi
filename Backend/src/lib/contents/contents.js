const Items = require('../schema/Items/Items').model

var contents = {}

contents.isLocalContent = function(id) {
	var fields = id.split(':')
	if(fields.length < 1) {
		return false
	}
	var n = parseInt(fields[0])
	return n >= 0
}

contents.resolveContent = function(id, callback) {
	if(contents.isLocalContent(id)) {
		callback(null, "local")
	}
	else {
		Items.findOne({_id:id}, function(err, item) {
			if (err) {
				callback(err, "error")
			}
			else {
				if(item==null) {
					callback("No data", "error")
				}
				else {
					console.log(id + " resolved as: '" + item.content + "'")
					callback(null, item.content)
				}
			}
		})
	}
}

module.exports = contents
