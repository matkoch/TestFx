param($installPath, $toolsPath, $package, $project)

$path = [System.IO.Path]

Copy-Item $path::Combine($toolsPath, "Formatting.DotSettings") $project.FileName.Replace(".csproj", ".csproj.DotSettings")