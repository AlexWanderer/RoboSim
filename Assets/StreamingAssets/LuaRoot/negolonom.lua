local lib = require "io"
local graph = require "graph"
local sys = require "sys"
br= 0
br2 = 0
delate = 0
sys.wait(0.1)
r=0.0
g=0.0
b=0.0
happed = 0
ang= 0
while true do
k = 50
lib.cSColor("CS1")
r = lib.cSR("CS1")
g = lib.cSG("CS1")
b = lib.cSB("CS1")
if( r>0.5 and g>0.5 and b<0.3) then
sys.trace("YELLOW")
end

--lib.motorTurn("MT1",ang)
--lib.paint("PNT1")
dist = lib.getUSDist("USS1")

br = lib.getCSBr("CS1");
br2 = lib.getCSBr("CS2");
delta = br-br2



coef = delta*2*k
angle = delta*k

sys.trace(angle)
if (angle>35) then
angle=35
end
if (angle<-35) then
angle=-35
end
lib.motorSpeed("M3",1,-300)
lib.motorSpeed("M4",1,300)
lib.motorTurn("M1", -angle)
lib.motorTurn("M2", -angle)
lib.motorTurn("M3", 0)
lib.motorTurn("M4", 0)
lib.motorSpeed("M1",1,200 )
lib.motorSpeed("M2",1,-200 )

if(math.abs(angle) > 30) then
sys.wait(0.5)
end



--lib.thruster("THR1",500)
sys.wait(0.02)
end

sys.trace("Finished!")




function sign(x)
   if x<0 then
     return -1
   elseif x>0 then
     return 1
   else
     return 0
   end
end
