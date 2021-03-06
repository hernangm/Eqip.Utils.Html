#========================================
# Nuget functions
#========================================

function Create-Nupkg ($nu_spec_or_trans_file_path, $version, $configuration) {
    $params= @{}
    $params.base_directory = [System.IO.Path]::GetDirectoryName($nu_spec_or_trans_file_path)
    $params.nuspec_file_name = [System.IO.Path]::GetFileNameWithoutExtension($nu_spec_or_trans_file_path) + ".csproj"
    $params.nuspec_file_path =  "$($params.base_directory)\$($params.nuspec_file_name)"
    $params.nupkg_file_path = "$($params.base_directory)\bin\Release"
    Write-HashTable-Debug $params "Nuspec variables"
    
    Write-Host "`n"
    Write-Progress "Creating nupkg from $($params.nuspec_file_name)`n, version: $version."
    Exec { & $tools.nuget pack $params.nuspec_file_path -Sym -Prop Configuration=$configuration -Version $version -OutputDirectory $params.nupkg_file_path }
	Write-Host "`n"
	Write-Success "  Packing Succeeded!"
	return
}

function Push-Nupkg ($nuget_package_file_path) {
    Write-Debug "File to push $nuget_package_file_path"

    $vars1 = ReadVariables $keyfile
    Write-HashTable-Debug($vars1)

    $filename = [System.IO.Path]::GetFileName($nuget_package_file_path)
    Write-Progress "About to push '$filename' to Nuget gallery`n"
    Exec { & $tools.nuget push $nuget_package_file_path -ApiKey "$($vars1.Username):$($vars1.Password)" -Source $($vars1.Feed) }
	Write-Host "`n"
	Write-Success "  Pushed!"
	Write-Host "`n"
    return
}

function Create-Nuspec-From-Nutrans ($params, $version) {
    Write-Progress "Creating nuspec $($params.nuspec_file) from $($params.nuspec_shared_file) with transform $($params.nutrans_file)"
    Exec { msbuild $($tools.transform) /p:Source=$($params.nuspec_shared_file_path) /p:Transform=$($params.nutrans_file_path) /p:Destination=$($params.nuspec_file_path) /v:minimal /nologo }
	Write-Success
	return
}

function Update-Nuspec ($path) {
    $xmlDoc = [System.Xml.XmlDocument](get-content $path -Encoding UTF8)
    Include-Files-Nuspec $xmlDoc
    Update-Nuspec-Version $xmlDoc $version
    $xmlDoc.Save($path)
	return
}

function Include-Files-Nuspec([System.Xml.XmlDocument]$xmlDoc) {
    if($pack_source_code_only -eq $true) {
		Write-Host "`n`n    Copying source files:"
        Get-ChildItem "$($conf.project_dir)\*.cs" | foreach {
			$params = @{}
            $params.source_file = $($_.Name)
			$params.source_file_path = $($_.FullName)
			$params.destination_file = "$($params.source_file).pp"
			$params.destination_file_path = "$($conf.project_release_dir)\$($params.destination_file)"
			Write-HashTable-Debug $params "Copy source code variables"
            Write-Progress "Copying $($params.source_file) as $($params.destination_file)"
            Copy-Item -Path $($params.source_file_path) -Destination $($params.destination_file_path)
			Write-Success
        } | Out-Null
		
		
		Write-Host "`n`n    Adding source files to nuspec:"
        $files = $xmlDoc.package.files

        if (-not $files) {
			Write-Host "    " -NoNewline
            Write-Warning "<files> node does not exists. It will be created."
            $files = $xmlDoc.CreateElement('files')
            $xmlDoc.package.AppendChild($files) | Out-Null
        }

        $current = "$($conf.project_release_dir)\"
        $relative = RelativePath $current "$($conf.project_dir)\"
		$target = 'content\extensions'
        Get-ChildItem "$($current)\*.cs.pp" | foreach { 
            $file_to_include = "$($relative)$($_.Name)"
            Write-Progress "Adding Source '$file_to_include' to target '$target'"
            $file = $xmlDoc.CreateElement('file')
            $file.SetAttribute('src',$file_to_include)
            $file.SetAttribute('target',$target)
            $files.AppendChild($file)
			Write-Success
        } | Out-Null
    }
    return
}


function Update-Nuspec-Version([System.Xml.XmlDocument]$xml, $version) {
    $metadata = $xml.SelectSingleNode("//*[local-name()='metadata']")
    $node = $xml.SelectSingleNode("//*[local-name()='version']")
	Write-Host "`n`n    Configuring version in nuspec"
    if($update_version_on_nuspec) {
		Write-Progress "Version set to $version"
        if ($node) {
            $node.InnerText =  $node.InnerText.Replace("{version}",$version)
        } else {
            $node = $xml.CreateElement('version')
            $node.InnerText = $version
            $metadata.AppendChild($node)
        }
		Write-Success
    } else {
		Write-Progress "Removing version in nuspec"
	    if ($node) {
	        $comment = $xml.CreateComment($node.OuterXml)
	        $metadata.RemoveChild($node)
	    } else {
	        $comment = $xml.CreateComment("<version>" + $version + "</version>")
	    }
	    $metadata.AppendChild($comment)
		Write-Success "Transformed to comment"
    }
	return
}


<#
function Create-Nuspec ($name, $version) {
    $shared_nuspec_file = "$($nuget.templates_dir)\$($nuget.shared_templates_prefix).Shared.nutrans"
    $nutrans_file_path = "$($nuget.templates_dir)\$name.nutrans"
    $nuspec_filename = $name.nuspec
    $nuspec_file_path = "$($nuget.nuspecs_dir)\$nuspec_filename"

    Write-Host "Creating nuspec $name.nuspec ($nuspec_file_path)" -ForegroundColor Green
    Exec { msbuild $transform_xml /p:Source=$shared_nuspec_file /p:Transform=$nutrans_file_path /p:Destination=$nuspec_file_path /v:minimal /nologo }
    Update-Nuspec-Version $nuspec_file $version
    $nuspec_file
    
    $source_path = "$($nuget.nuspecs_dir)\$($name).nuspec"
    $destination_filename = "$($name).$($version).nuspec"
    $destination_path = "$($nuget.nuspecs_dir)\$($destination_filename)"
    
    Copy-Item $source_path $destination_path
    
    $destination_path
    return
}
#>

#========================================
# /Nuget functions
#========================================
