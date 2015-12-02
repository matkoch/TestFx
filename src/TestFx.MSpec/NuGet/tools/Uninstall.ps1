param($installPath, $toolsPath, $package, $project)

$path = [System.IO.Path]

Remove-Item $project.FileName.Replace(".csproj", ".csproj.DotSettings")