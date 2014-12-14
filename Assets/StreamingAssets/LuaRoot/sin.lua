local lib = require "io"
local graph = require "graph"
local sys = require "sys"
sys.wait(0.05)
a = 0
angle = 0
while true do
a = a + 0.1
angle = math.sin(a)
angle = angle * 25
sys.trace(angle)
lib.motorSpeed("M3",1,-300)
lib.motorSpeed("M4",1,300)
lib.motorTurn("M1", angle)
lib.motorTurn("M2", angle)
lib.motorTurn("M3", -angle)
lib.motorTurn("M4", -angle)
lib.motorSpeed("M1",1,200 )
lib.motorSpeed("M2",1,-200 )

sys.wait(0.1)
end





