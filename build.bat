@ECHO OFF
CLS
.paket\paket install
"packages\FAKE\tools\Fake.exe" build.fsx
PAUSE
