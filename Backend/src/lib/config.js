var config = {}

const DevelopmentConfig = {
	accessKeyId: 'AKIA2R2AUIKVPTDBQTOR', 
	secretAccessKey: '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',
	bucketName: 'myfoldraccel'	
}

const ProductionConfig = {
	accessKeyId: 'AKIA2R2AUIKVPTDBQTOR',
        secretAccessKey: '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',
        bucketName: 'myfoldraccel'
}

if (process.env.ENVIRONMENT == "PRODUCTION") {
	config = ProductionConfig
}
else {
	config = DevelopmentConfig
}

module.exports = config
