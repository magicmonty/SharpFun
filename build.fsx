// include Fake lib
#r "packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Paket

// Properties
let buildDir = "./build"
let releaseDir = buildDir @@ "release"
let testDir = buildDir @@ "test"
let deployDir = buildDir @@ "deploy"

// Targets
Target "Clean" (fun _ -> CleanDir buildDir)
Target "Default" (fun _ -> trace "Hello World from FAKE")
Target "BuildRelease" (fun _ -> 
    !!"*.sln"
    |> MSBuildRelease releaseDir "Build"
    |> Log "ReleaseBuild-Output")
Target "BuildDebug" (fun _ -> 
    !!"*.sln"
    |> MSBuildDebug testDir "Build"
    |> Log "DebugBuild-Output: ")
Target "Test" (fun _ -> 
    !!(testDir @@ "Test.dll") |> NUnit(fun p -> 
                                     { p with DisableShadowCopy = true
                                              OutputFile = testDir @@ "TestResults.xml" }))
Target "CreatePackage" (fun _ -> 
    Pack(fun p -> 
        { p with OutputPath = deployDir
                 TemplateFile = "Functional/Functional.paket.template"
                 WorkingDir = "Functional"
                 Version = GetAssemblyVersionString(releaseDir @@ "Pagansoft.Functional.dll")
                 ToolPath = ".paket/paket.exe" }))
// Dependencies
"Clean" ==> "BuildDebug" ==> "Test" ==> "BuildRelease" ==> "CreatePackage" ==> "Default"
// start build
RunTargetOrDefault "Default"
