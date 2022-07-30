-- Define global class type for clickabledata.
class_type = 
{
	NULL   = 0,
	BTN    = 1,
	TUMB   = 2,
	SNGBTN = 3,
	LEV    = 4
}

-- Mock out the get_option_value and get_aircraft_type functions that don't exist in this environment.
function get_option_value(x)
	return nil
end