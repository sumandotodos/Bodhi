const awskeys = require('../../../../AWSKeys')

var config = {}

const DevelopmentConfig = {
	accessKeyId: '<accessKeyId>', 
	secretAccessKey: '<secretAccessKey>',
	bucketName: 'fg-volare-videos-development',	
	region: 'eu-west-1',
	psk: 'vQb9BpkcLGQWlmAild4B',
	dbhost: 'localhost',
	dbport: '27017',
	tempdir: '/tmp/video'
}

const ProductionConfig = {
	accessKeyId: '<accessKeyId>',
        secretAccessKey: '<secretAccessKey>',
        bucketName: 'myfoldraccel',
	region: 'eu-west-2',
	psk: 'KcbUv1heC80IGJyWlqlx',
	dbhost: 'localhost',
	dbport: '27017',
	tempdir: '/tmp/video'
}

function patchInEnv()
{
	if (process.env.AWSACCESSKEY != undefined && process.env.AWSACCESSKEY != "") {
		config.accessKeyId = process.env.AWSACCESSKEY
	}
	else {
		config.accessKeyId = awskeys.accessKeyId
		
	}
	if (process.env.AWSSECRETKEY != undefined && process.env.AWSSECRETKEY != "") {
		config.secretAccessKey = process.env.AWSSECRETKEY
	}
	else {
		config.secretAccessKey = awskeys.secretAccessKey
	}

	console.log("Using AWS access key: " + config.accessKeyId)
	console.log("Using AWS secret key: " + config.secretAccessKey)
	//if (process.env.DBHOST != "")
	//{
	//	config.dbhost = process.env.DBHOST
	//}
	//if (process.env.DBPORT != "") 
	//{
	//	config.dbport = process.env.DBPORT
	//}
}

if (process.env.ENVIRONMENT == "PRODUCTION") {
	config = ProductionConfig
}
else {
	config = DevelopmentConfig
}
patchInEnv()
console.log("Config: " + JSON.stringify(config))
module.exports = config
