local lib = require "io"
local graph = require "graph"
local sys = require "sys"

a=1
sys.trace("Script 1 Loaded!")
sys.trace(a)
while true do
lib.motorSpeed("M1",1,200)
lib.motorSpeed("M2",1,200)
lib.motorSpeed("M3",1,200)
lib.motorSpeed("M4",1,200)

sys.wait(0.05)
end