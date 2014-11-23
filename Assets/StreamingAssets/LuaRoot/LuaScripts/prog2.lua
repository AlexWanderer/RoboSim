local lib = require "io"
_maga = 0
yaya = 0
tt = 0
while true do
br = lib.getBrightness(1)
	if(br>0.7) then
		lib.move(1,1,50)
		lib.move(2,1,300)
		
	end
	if(br<0.65) then
		lib.move(2,-1,150)
		lib.move(1,1,400)
		lib.wait(0.3)
	end

lib.wait(0.04)
end

lib.trace("Finished!")
