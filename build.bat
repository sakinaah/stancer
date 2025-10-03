@echo off
echo Building XStance...

cd src
dotnet build --configuration Release

if %errorlevel% == 0 (
    echo Build successful!
    echo Copying files...
    
    if exist "bin\x64\Release\net452\xstance.net.dll" (
        copy "bin\x64\Release\net452\xstance.net.dll" "..\xstance.net.dll"
    ) else (
        echo xstance.net.dll not found!
    )
    
    if exist "bin\x64\Release\net452\Newtonsoft.Json.dll" (
        copy "bin\x64\Release\net452\Newtonsoft.Json.dll" "..\Newtonsoft.Json.dll"
    ) else (
        echo Newtonsoft.Json.dll not found!
    )
    
    if exist "bin\x64\Release\net452\MenuAPI.dll" (
        copy "bin\x64\Release\net452\MenuAPI.dll" "..\MenuAPI.dll"
    ) else (
        echo MenuAPI.dll not found!
    )
    
    echo Done! Files copied to root directory.
) else (
    echo Build failed!
    pause
)

cd ..
pause