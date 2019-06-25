const mongoose = require('mongoose')
const config = require('./config')

console.dir(config)

mongoose.connect('mongodb://'+config.dbhost+':'+config.dbport+'/bodhi', {useNewUrlParser: true});
var db = mongoose.connection
db.on('error', console.error.bind(console, 'connection error:'));
db.once('open', function() {
  console.log("Connected to " + config.dbhost + ":" + config.dbport)
});

