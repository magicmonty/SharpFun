#!/bin/sh
.paket/paket install
mono packages/FAKE/tools/Fake.exe build.fsx
