const config = require('./config')
const AWS = require('aws-sdk')

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

function linkGen(configObj) {
	const params = {
                Bucket: configObj.bucket,
                Key: configObj.filename,
                Expires: configObj.expiry,
                ContentType: configObj.contentType,
                ACL: 'bucket-owner-full-control'
        }
	const url = s3.getSignedUrl(configObj.method.toLowerCase()+'Object', params)
        return { url:url, error:null }
}

s3urlgen.s3putGen = function (filename) {
	return linkGen({bucket:bucket,
                filename:filename,
                expiry:signedUrlExpireSeconds,
                contentType:'video/mp4',
                method:'put'
        })
}

s3urlgen.s3getGen = function(filename) {
	const params = {
                Bucket: bucket,
                Key: filename,
                Expires: signedUrlExpireSeconds
        }
        const url = s3.getSignedUrl('getObject', params)
        return { url:url, error:null }
}

s3urlgen.uploadStreamWrapper = function(filename) {
	var pass = new stream.PassThrough();
	console.log("upload strem: uploading to s3 = " + filename)
  	var params = {Bucket: bucket, Key: filename, Body: pass};
  	s3.upload(params, function(err, data) {
    		console.log(err, data);
  	});
  	return pass;
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
