const random = require('random')
const seedrandom = require('seedrandom')
 
random.use(seedrandom('32fudfhdf4t54rjhgsdfsdf'))

console.log( random.int(0,100) )

random.use(seedrandom('shit'))

console.log ( random.int(0,100) )
