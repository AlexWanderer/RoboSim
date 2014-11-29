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
k = 40
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
if (angle>35) then
angle=35
end
if (angle<-35) then
angle=-35
end
lib.motorSpeed("M3",1,100-coef)
lib.motorSpeed("M4",1,-100+coef)
lib.motorTurn("M1", angle)
lib.motorTurn("M2", angle)
lib.motorTurn("M3", -angle)
lib.motorTurn("M4", -angle)
lib.motorSpeed("M1",1,200 -coef )
lib.motorSpeed("M2",1,-200 +coef )



if((dist<0.5)and(happed ==0)) then
happed = 1
lib.motorTurn("M1",0)
lib.motorTurn("M2", 0)
lib.motorTurn("M3", 0)
lib.motorTurn("M4", 0)
lib.toggleGrabber("GR1")
lib.motorSpeed("M1",1,-400)
lib.motorSpeed("M3",1,-400)
lib.motorSpeed("M4",1,400)
lib.motorSpeed("M2",1,400)
sys.wait(0.3)
lib.motorSpeed("M1",1,-400)
lib.motorSpeed("M3",1,-400)
lib.motorSpeed("M4",1,-400)
lib.motorSpeed("M2",1,-400)
sys.wait(3.95)
end


--lib.thruster("THR1",500)
sys.wait(0.05)
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
