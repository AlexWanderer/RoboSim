local lib = require "io"
_maga = 0
yaya = 0
tt = 0
lib.wait(0.1)
--lib.trace("BR:")
while true do
br = lib.getBrightness(2)
lib.trace("BR:" .. br)
	if(br>0.9) then
		lib.move(1,1,50)
		lib.move(2,1,300)
		
	end
	if(br<0.9) then
		lib.move(2,-1,150)
		lib.move(1,1,400)
		--lib.trace("delay!")
		lib.wait(0.2)
		
	end

lib.wait(0.01)
end

lib.trace("Finished!")
