.\nuget.exe push (gci *.nupkg  -Exclude *.symbols.* | select -last 1) 