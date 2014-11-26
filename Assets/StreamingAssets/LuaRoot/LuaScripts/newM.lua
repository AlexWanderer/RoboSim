local lib = require "io"
_maga = 0
yaya = 0
tt = 0
br= 0
lib.wait(0.1)
--lib.trace("BR:")
while true do
--lib.trace("TEST")
br = lib.getCSBr("CS1");
lib.motorSpeed("M1",1,400)
lib.motorSpeed("M3",1,400)
lib.motorSpeed("M4",1,400)
lib.motorSpeed("M2",1,400)
lib.thruster("THR1",500)
lib.wait(0.1)
end

lib.trace("Finished!")
