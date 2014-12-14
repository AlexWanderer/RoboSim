local lib = require "mylib"
_maga = 5
local function main()
lib.trace(_maga)


_maga = _maga + 3
lib.trace(_maga)
end


local function babada()		
	_maga = _maga + 2
	lib.trace(_maga)
end

return {
    main           = main,
	babada		  = babada,
}