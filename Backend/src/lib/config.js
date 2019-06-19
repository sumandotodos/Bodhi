var config = {}

const DevelopmentConfig = {
	accessKeyId: '<accessKeyId>', 
	secretAccessKey: '<secretAccessKey>',
	bucketName: 'myfoldraccel',	
	region: 'eu-west-2'
}

const ProductionConfig = {
	accessKeyId: '<accessKeyId>',
        secretAccessKey: '<secretAccessKey>',
        bucketName: 'myfoldraccel',
	region: 'eu-west-2'
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
