var config = {}

const DevelopmentConfig = {
	accessKeyId: '<accessKeyId>', 
	secretAccessKey: '<secretAccessKey>',
	bucketName: 'myfoldraccel',	
	region: 'eu-west-2',
	psk: 'vQb9BpkcLGQWlmAild4B',
	dbhost: 'localhost',
	dbport: '27017'
}

const ProductionConfig = {
	accessKeyId: '<accessKeyId>',
        secretAccessKey: '<secretAccessKey>',
        bucketName: 'myfoldraccel',
	region: 'eu-west-2',
	psk: 'KcbUv1heC80IGJyWlqlx',
	dbhost: 'localhost',
	dbport: '27017'
}

function patchInEnv()
{
	if (process.env.AWSACCESSKEY != "")
	{
		config.accessKeyId = process.env.AWSACCESSKEY
	}
	if (process.env.AWSSECRETKEY != "")
	{
		config.secretAccessKey = process.env.AWSSECRETKEY
	}
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
