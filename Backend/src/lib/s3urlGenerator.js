const config = require('./config')
const AWS = require('aws-sdk')

AWS.config.update(
	{
		accessKeyId: config.accessKeyId,
		secretAccessKey: config.secretAccessKey
	})

//AWS.config.update({region: 'eu-west-2'})

var s3 = new AWS.S3()

var s3urlgen = {}

const bucket = config.bucketName
const signedUrlExpireSeconds = 60

s3urlgen.s3putGen = function (name) {
	const params = {
                Bucket: bucket,
                Key: name,
                Expires: signedUrlExpireSeconds,
                ACL: 'bucket-owner-full-control'
        }
	console.dir(params)
	const url = s3.getSignedUrl('putObject', params)
	return { url:url, error:null }
}

module.exports = s3urlgen
