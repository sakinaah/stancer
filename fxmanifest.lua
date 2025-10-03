fx_version 'cerulean'
game 'gta5'

name 'XStance'
description 'Custom VStancer - Vehicle Stance Editor'
author 'Custom Build'
version '1.0.0'

client_scripts {
    '@menu/MenuAPI.lua', -- Ensure MenuAPI is loaded
    'config.lua',
    'client/menu.lua',
    'client/main.lua'
}

files {
    'html/index.html',
    'html/style.css',
    'html/script.js'
}

ui_page 'html/index.html'