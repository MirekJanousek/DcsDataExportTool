-- This file mocks i18 functions that are required by DCS modules

local i_18n = { locales = {} }

local currentLocale = 'en' -- the default language

function i_18n.setLocale(newLocale)
  currentLocale = newLocale
  assert(i_18n.locales[currentLocale], ("The locale %q was unknown"):format(newLocale))
end

function i_18n.get_locale()
  return "en", "US"
end

local function translate(id)
  local result = id
  return result
end

i_18n.translate = translate

setmetatable(i_18n, {__call = function(_,...) return translate(id) end})

return i_18n