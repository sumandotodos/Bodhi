const db = require('./lib/db')
const Items = require('./lib/schema/Items/Items')
const Users = require('./lib/schema/Users/Users')

Items.model.find({_userid:"jaramorl@hotmail.net"}, function(err, items) {
	console.log("Found: " + items.length + " items")
})

/*
console.log("Inserting an item")
newItem = Items.model.create({_id:"yaoyaoyao",_userid:"jaramorl@hotmail.net",upvotes:0,downvotes:0,type:'comment',content:'shit'}, function(err, item) {
	if(err) {
		console.log("This was the error: " + err)
	}
	else {
		console.log("Item created successfully?!?")
	}
})
console.log("Inserting an item")
newItem = Items.model.create({_id:"y242342343",_userid:"jaramorl@hotmail.net",upvotes:20,downvotes:4,type:'comment',content:'Cacaolat para todos 😀'}, function(err, item) {
        if(err) {
                console.log("This was the error: " + err)
        }
        else {
                console.log("Item created successfully?!?")
        }
})
console.log("Inserting an item")
newItem = Items.model.create({_id:"yaoidfigusyaoyao",_userid:"jaramorl@hotmail.net",upvotes:0,downvotes:0,type:'comment',content:'shit shake'}, function(err, item) {
        if(err) {
                console.log("This was the error: " + err)
        }
        else {
                console.log("Item created successfully?!?")
        }
})

console.log("Inserting an user")
newUser = Users.model.create({_id:"jaramorl@hotmail.net",items:[]}, function(err, user) {
	if(err) {
                console.log("This was the error: " + err)
        }
        else {
                console.log("User created successfully?!?")
        }
})
*/


//Items.model.find().distinct("_id", function(err, ids) {
//	console.dir(ids)
//	console.log("First element: " + ids[0])
//})
