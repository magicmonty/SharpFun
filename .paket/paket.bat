@ECHO OFF
%~dp0paket.bootstrapper.exe --self
%~dp0paket.bootstrapper.exe
%~dp0paket.exe %1 %2 %3 %4 %5 %6 %7 %8
