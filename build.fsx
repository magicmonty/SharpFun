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
Target "Clean" (fun _ ->
  CleanDir buildDir
)

Target "Default" (fun _ ->
  trace "Build successful"
)

Target "BuildRelease" (fun _ ->
    !!"*.sln"
    |> MSBuildReleaseExt releaseDir ["Platform", "Any CPU"] "Build"
    |> Log "ReleaseBuild-Output"
)

Target "BuildDebug" (fun _ ->
    !!"*.sln"
    |> MSBuild testDir "Build" [
      ("Configuration", "Debug")
      ("Platform", "Any CPU")]
    |> Log "DebugBuild-Output: "
)

Target "Test" (fun _ ->
    !!(testDir @@ "Test.dll")
      |> NUnit(fun p ->
          { p with DisableShadowCopy = true
                   OutputFile = testDir @@ "TestResults.xml" })
)

Target "CreatePackage" (fun _ ->
    Pack(fun p ->
        { p with OutputPath = deployDir
                 TemplateFile = "Functional/Functional.paket.template"
                 WorkingDir = "Functional"
                 Version = GetAssemblyVersionString(releaseDir @@ "Pagansoft.Functional.dll")
                 ToolPath = ".paket/paket.exe" })
)

Target "PushPackage" (fun _ ->
    Push(fun p ->
        { p with WorkingDir = deployDir })
)

// Dependencies
"Clean"
  ==> "BuildDebug"
  ==> "Test"
  ==> "BuildRelease"
  ==> "CreatePackage"
  ==> "Default"

"Clean"
  ==> "BuildDebug"
  ==> "Test"
  ==> "BuildRelease"
  ==> "CreatePackage"
  ==> "PushPackage"

// start build
RunTargetOrDefault "Default"
