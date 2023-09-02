Set objPlayer = CreateObject("WMPlayer.OCX")
Set x = CreateObject("Scripting.FileSystemObject")
Set y = CreateObject("Wscript.Shell")

appdatar = y.ExpandEnvironmentStrings("%APPDATA%")

objPlayer.URL = "<path>"
objPlayer.settings.volume = 100
objPlayer.settings.setMode "loop", True
objPlayer.controls.play

Do While objPlayer.playState <> 1
    if x.FileExists(appdatar & "\LastViewII\stop.txt") Then exit do
    WScript.Sleep 100
Loop

objPlayer.close

Set objPlayer = Nothing
if x.FileExists(appdatar & "\LastViewII\stop.txt") Then x.DeleteFile appdatar & "\LastViewII\stop.txt"
if x.FileExists(Wscript.ScriptFullName) Then x.DeleteFile Wscript.ScriptFullName
