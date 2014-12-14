local lib = require "io"
lib.drawColor(1,1,0)
lib.wait(0.2)
while true do

lib.drawPixel(math.random(127),math.random(127))
--lib.redraw()

lib.wait(0.2)
end
