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

if (process.env.ENVIRONMENT == "PRODUCTION") {
	config = ProductionConfig
}
else {
	config = DevelopmentConfig
}

module.exports = config
