const config = require('./config')
const AWS = require('aws-sdk')
//var Minio = require('minio')

AWS.config.update(
	{
		accessKeyId: config.accessKeyId,
		secretAccessKey: config.secretAccessKey,
		signatureVersion: 'v4',
		region: config.region
	})
console.log("  >> s3 using accessKey: " + config.accessKeyId + ", " 
	+ ", secretKey: " + config.secretAccessKey + ", bucket name: " +
	config.bucketName)
//AWS.config.update({region: 'eu-west-2'})

var s3 = new AWS.S3()

var s3urlgen = {}

const bucket = config.bucketName
const signedUrlExpireSeconds = 5 * 60

s3urlgen.s3putGen = function (name) {
	const params = {
                Bucket: bucket,
                Key: name,
                Expires: signedUrlExpireSeconds,
		ContentType: 'video/mp4',
                ACL: 'bucket-owner-full-control'
        }
	console.dir(params)
	const url = s3.getSignedUrl('putObject', params)
	return { url:url, error:null }
}

//s3urlgen.s3Client = new Minio.Client(
//	{
//                endPoint: 's3.amazonaws.com',
//		useSSL: true,
//                accessKey: config.accessKeyId,
//                secretKey: config.secretAccessKey
//        }
//)

//s3urlgen.s3putGen = function(fname) {
//	s3urlgen.s3Client.presignedPutObject(bucket, fname, 5*60).then((url) => { return url })	
//}

module.exports = s3urlgen
