local lib = require "io"
local graph = require "graph"
local sys = require "sys"
_maga = 0
yaya = 0
tt = 0
br= 0
br2 = 0
delate = 0
sys.wait(0.1)
--sys.trace("BR:")
graph.drawColor(0,1,0)
graph.clrscr()
graph.drawColor(0,0,1)
graph.drawLine(10,20,10,10)
graph.drawLine(128,128,0,0)
graph.drawRect(24,54,20,20,1)
graph.drawColor(1,0,0)
graph.drawRect(24,54,20,20,0)
happed = 0
ang= -35
while true do
k = 3
lib.motorTurn("MT1",ang)
ang = ang + 0.1
dist = lib.getUSDist("USS1")
sys.trace(dist)
--lib.trace(delta)
br = lib.getCSBr("CS1");
--lib.wait(0.01)
br2 = lib.getCSBr("CS2");
delta = br-br2
spd1 = 50*k*math.abs(1-delta)
spd2 = 500*k*math.abs(1-delta)

--lib.trace(spd1)
if (delta == 0) then
lib.motorSpeed("M1",1,800)
lib.motorSpeed("M3",1,800)
lib.motorSpeed("M4",1,-800)
lib.motorSpeed("M2",1,-800)
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


if((dist<0.5)and(happed ==0)) then
happed = 1
lib.toggleGrabber("GR1")
lib.motorSpeed("M1",1,-400)
lib.motorSpeed("M3",1,-400)
lib.motorSpeed("M4",1,400)
lib.motorSpeed("M2",1,400)
sys.wait(1)
lib.motorSpeed("M1",1,-400)
lib.motorSpeed("M3",1,-400)
lib.motorSpeed("M4",1,-400)
lib.motorSpeed("M2",1,-400)
sys.wait(2)
end


--lib.thruster("THR1",500)
sys.wait(0.02)
end

sys.trace("Finished!")
