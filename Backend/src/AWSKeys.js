var DevelopmentKeys = {
	accessKeyId: '<accessKeyId>',
	secretAccessKey: '<secretAccessKey'>
}

var ProductionKeys = {
	accessKeyId: '<accessKeyId>',
        secretAccessKey: '<secretAccessKey'>
}

if (process.env.ENVIRONMENT == "PRODUCTION") {
	keys = DevelopmentKeys
}
else {
	keys = ProductionKeys
}

module.exports = keys
