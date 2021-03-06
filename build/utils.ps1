#========================================
# Helper functions
#========================================

function ReadVariables($file) {
    $vars = @{}
    Get-Content $file | Foreach-Object {
       $var = $_.Split('=')
       $vars.Add($var[0],$var[1])
    }
    return $vars
}

function Clean-Directory ($dir) {
    Write-Progress "Cleaning $dir"
    if (Test-Path $dir) { 
        Remove-Item $dir -Force -Recurse -ErrorAction SilentlyContinue | Out-Null
    }
    #New-Item $dir -ItemType Directory | Out-Null
	Write-Success "Cleaned"
    return
}


function Write-HashTable-Debug ($hash, [string]$title) {
	$output = "`n" + (Header $title)
	$output += ($hash.GetEnumerator() | Sort-Object -Property Name  | Format-Table -Wrap -AutoSize| Out-String )
    Write-Debug $output
	return
}

function Header ([string]$title) {
	$output = ""
	if ($title) {
		$length = $title.Length * 1.5
		$output += "`n$('-' * $length)`n $title`n$('-' * $length)`n"
	}
	return $output
}

function Write-Header ([string]$title) {
	Write-Host (Header $title) -foregroundcolor DarkGray
	return
}

function Write-Progress ([string]$title) {
	Write-Host "  - $title" -nonewline -ForegroundColor Gray
	return
}

function Write-Success ([string]$title) {
	$output = @{$true="OK";$false=$title}[[string]::IsNullOrWhiteSpace($title)]
	Write-Host "  $output" -ForegroundColor Green
	return
}

function RelativePath
{
    param
    (
        [string]$path = $(throw "Missing: path"),
        [string]$basepath = $(throw "Missing: base path")
    )
    
    return [system.io.path]::GetFullPath($path).SubString([system.io.path]::GetFullPath($basepath).Length)
}   

#========================================
# /Helper functions
#========================================
