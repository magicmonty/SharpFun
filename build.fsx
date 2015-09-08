// include Fake lib
#r "packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Paket

// Properties
let testDir = "Test/bin/Debug"
let deployDir = "deploy"

// Targets
Target "Clean" (fun _ ->
  !!"**/bin"
  |> DeleteDirs

  !!"**/obj"
  |> DeleteDirs

  DeleteDir deployDir
)

Target "Default" (fun _ ->
  trace "Build successful"
)

Target "BuildRelease" (fun _ ->
    !!"*.sln"
    |> MSBuildReleaseExt "" ["Platform", "Any CPU"] "Build"
    |> Log "ReleaseBuild-Output"
)

Target "BuildDebug" (fun _ ->
    !!"*.sln"
    |> MSBuild "" "Build" ["Platform", "Any CPU"]
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
                 TemplateFile = "paket.template"
                 WorkingDir = "."
                 Version = GetAssemblyVersionString("Functional/bin/Release/Pagansoft.Functional.dll")
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
