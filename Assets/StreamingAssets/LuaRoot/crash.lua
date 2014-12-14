local lib = require "io"
local graph = require "graph"
local sys = require "sys"
sys.wait(0.05)
a = 0
angle = 0
while true do
--a = a + 0.1
--angle = math.sin(a)
--angle = angle * 25
--sys.trace(angle)
lib.motorSpeed("M3",1,-900)
lib.motorSpeed("M4",1,900)
lib.motorTurn("M1", 0)
lib.motorTurn("M2", 0)
lib.motorTurn("M3", 0)
lib.motorTurn("M4", 0)
lib.motorSpeed("M1",1,900 )
lib.motorSpeed("M2",1,-900 )

sys.wait(0.1)
end





