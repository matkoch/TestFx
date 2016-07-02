[CmdletBinding()]
Param(
  [Parameter()] [switch] $Full = $false,
  [Parameter()] [switch] $Compile = $false,
  [Parameter()] [switch] $Package = $false,
  [Parameter()] [switch] $Test = $false,
  [Parameter()] [switch] $Cover = $false,
  [Parameter()] [switch] $Inspect = $false
)

if ($Full) {
	$Comile = $true
	$Package = $true
	$Test = $true
	$Cover = $true
	$Inspect = $true
}

Write-Host $Inspect