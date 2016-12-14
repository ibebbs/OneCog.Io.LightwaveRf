#r @"..\tools\FAKE\tools\FakeLib.dll"

open Fake
open System.IO;
 
// Properties
let deployDir = @".\deploy\"
 
// version info
let version = environVarOrDefault "PackageVersion" "1.0.0.0"  // or retrieve from CI server
let summary = "Open source, portable .NET library fopr interacting with the LightwaveRf WifiLink."
let copyright = "onecog.solutions, 2014"
let tags = "portable LightwaveRf"
let description = "Open source, portable .NET library fopr interacting with the LightwaveRf WifiLink."

let allAssemblies = [ "OneCog.Io.LightwaveRf.dll"; "OneCog.Io.LightwaveRf.pdb"; ]
let sourcePath = "src"
let projectPath = "OneCog.Io.LightwaveRf"
let configuration = "Release"
let binPath = "bin"

let libDir = "lib"
let srcDir = "src"
let win8Target = "portable-win81+wpa81"
let wp8Target = "wp8"
let net45Target = "net45"
let sl5Target = "sl5"
let pclTarget = "portable-net45+wp8+win81+wpa81"
let uapTarget = "uap10.0"
let netStandardTarget = "netstandard1.1"
let srcTarget = "src"
 
// Targets
Target "Clean" (fun _ ->
    CleanDirs [ deployDir ]
)
 
Target "Build" (fun _ ->
   !! "./src/**/*.csproj"
     |> MSBuild null "Build" 
          [ 
            "Configuration", "Release"
            "Platform", "AnyCPU"
         ]
     |> Log "AppBuild-Output: "
)

Target "Package" (fun _ ->

    CopyWithSubfoldersTo deployDir [ !! "./src/OneCog.Io.LightwaveRf/*.cs" ]
    let deployPath = deployDir @@ projectPath

    let dependencies = getDependencies (sourcePath @@ projectPath @@ "packages.config")

    printfn "Dependencies from '%s' are: %A" (sourcePath @@ projectPath @@ "packages.config") dependencies

    allAssemblies |> List.map(fun a -> sourcePath @@ projectPath @@ binPath @@ configuration @@ a) |> Copy (deployPath)

    let win8Files = allAssemblies |> List.map(fun a -> (projectPath @@ a, Some(Path.Combine(libDir, win8Target)), None))
    let wp8Files = allAssemblies  |> List.map(fun a -> (projectPath @@ a, Some(Path.Combine(libDir, wp8Target)), None))
    let net45Files = allAssemblies |> List.map(fun a -> (projectPath @@ a, Some(Path.Combine(libDir, net45Target)), None))
    let sl5Files = allAssemblies |> List.map(fun a -> (projectPath @@ a, Some(Path.Combine(libDir, sl5Target)), None))
    let pclFiles = allAssemblies |> List.map(fun a -> (projectPath @@ a, Some(Path.Combine(libDir, pclTarget)), None))
    let uapFiles = allAssemblies |> List.map(fun a -> (projectPath @@ a, Some(Path.Combine(libDir, uapTarget)), None))
    let netStandardFiles = allAssemblies |> List.map(fun a -> (projectPath @@ a, Some(Path.Combine(libDir, netStandardTarget)), None))
    let srcFiles = [ (@"src\**\*.*", Some "src", None) ]

    NuGet (fun p -> 
        {p with
            Authors = [ "Ian Bebbington" ]
            Project = "OneCog.Io.LightwaveRf"
            Description = description
            Summary = summary
            Copyright = copyright
            Tags = tags
            OutputPath = deployDir
            WorkingDir = deployDir
            SymbolPackage = NugetSymbolPackage.Nuspec
            Dependencies = dependencies
            Version = version
            Files = win8Files @ wp8Files @ net45Files @ sl5Files @ pclFiles @ uapFiles @ netStandardFiles @ srcFiles
            Publish = false }) 
            "./build/OneCog.Io.LightwaveRf.nuspec"
)

Target "Run" (fun _ -> 
    trace "FAKE build complete"
)
  
// Dependencies
"Clean"
  ==> "Build"
  ==> "Package"
  ==> "Run"
 
// start build
RunTargetOrDefault "Run"