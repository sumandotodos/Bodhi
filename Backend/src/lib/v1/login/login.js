var express = require('express)
var helpers = require('../../helpers')
var router = express.Router()

inRAMUserList = []

function findUserInList(user) {
	index = inRAMUserList.map( (e) => (e.id == user) ).indexOf(true)
	return index;
}

router.get('/:id', function(req, res) {
	var token
	const userid = req.params["id"]
	if(findUserInList(userid) != -1) {
		res.json({token:inRAMUserList[index].token})
	}
	else {
		newToken = helpers.generateRandomString(12)
		inRAMUserList.append({id:userid,token:newToken})
		res.json({token:newToken})
	}
})
