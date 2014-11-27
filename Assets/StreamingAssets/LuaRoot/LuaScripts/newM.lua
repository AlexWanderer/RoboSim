local lib = require "io"
local graph = require "graph"
_maga = 0
yaya = 0
tt = 0
br= 0
br2 = 0
delate = 0
lib.wait(0.1)
lib.trace("BR:")
graph.drawColor(0,1,0)
graph.clrscr()
graph.drawColor(0,0,1)
graph.drawLine(0,0,128,128)
graph.drawLine(120,128,0,0)

while true do
k = 1.5

dist = lib.getUSDist("USS1")
lib.trace(dist)
--lib.trace(delta)
br = lib.getCSBr("CS1");
--lib.wait(0.01)
br2 = lib.getCSBr("CS2");
delta = br-br2
spd1 = 100*k*math.abs(1-delta)
spd2 = 400*k*math.abs(1-delta)

--lib.trace(spd1)
if (delta == 0) then
lib.motorSpeed("M1",1,400)
lib.motorSpeed("M3",1,400)
lib.motorSpeed("M4",1,-400)
lib.motorSpeed("M2",1,-400)
end
if(delta > 0) then 
lib.motorSpeed("M1",-1,spd1)
lib.motorSpeed("M3",-1,spd1)
lib.motorSpeed("M4",-1,spd2)
lib.motorSpeed("M2",-1,spd2)
--lib.wait(0.03)
end

if(delta < 0) then 
lib.motorSpeed("M1",1,spd2)
lib.motorSpeed("M3",1,spd2)
lib.motorSpeed("M4",1,spd1)
lib.motorSpeed("M2",1,spd1)
--lib.wait(0.03)
end


if(dist<3) then

lib.motorSpeed("M1",1,-400)
lib.motorSpeed("M3",1,-400)
lib.motorSpeed("M4",1,400)
lib.motorSpeed("M2",1,400)
lib.wait(1)
lib.motorSpeed("M1",1,-400)
lib.motorSpeed("M3",1,-400)
lib.motorSpeed("M4",1,-400)
lib.motorSpeed("M2",1,-400)
lib.wait(1.5)
end


--lib.thruster("THR1",500)
lib.wait(0.02)
end

lib.trace("Finished!")
