set path= %path%.
%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild    TDS.sln  /p:Configuration=Debug "/p:Platform=Any CPU"

pause