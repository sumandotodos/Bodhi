helpers = {}

helpers.toParamSafe = function(url) {
	return url.replace(/\//g, ':')
}

helpers.fromParamSafe = function(urls) {
	return urls.replace(/:/g, '/')
}

helpers.generateRandomString = function(n) {
	return Math.random().toString(n+2).substring(2)
}

helpers.randomIntRange = function(minInc, maxInc) {
	minInc + Math.floor(Math.random() * (maxInc + 1 - minInc));
}

helpers.sampleArray = function(arr, samples) {
	indexes = []
	var length = arr.length
	for(var i = 0; i < samples ; ++i) {
		indexes.push(helpers.randomIntRange(0,length-1)) 
	}
	result = []
	for(var i = 0; i < samples; ++i) {
		result.push(arr[indexes[i]])
	}
	return result
}

module.exports = helpers
