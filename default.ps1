# NOTE: BOM is not recommended, and I think is what sometimes mucks up Asciidoc
# rendering in github, and hilighting in emacs, so we are writing to files
# without BOM. When creating the UTF-8 encoding object, we have to specify false
# explicitly. Otherwise it is marked as neither UTF-8 with, or without BOM.

Properties {
    $build_dir = Split-Path $psake.build_script_file
	$encoding = New-Object System.Text.UTF8Encoding($false)
    $config = "Release"
    $version_major = 1
    $version_minor = 1
    $version_patch = 0
    $version_label = 'pre'
}

Task Default -Depends Generate-Docs,Patch-Version

Task Generate-Docs -Depends Generate-Todo,Generate-Usage,Generate-Schema
Task Generate-Todo {
    $sourceTodoFile = "$build_dir\Source-TODO.adoc"

	$excludeFolders = @('bin', 'obj', 'packages')

	$excludePattern = [string]::Join('|', ($excludeFolders | % {'\\' + $_ + '\\?' }))
	$todoMark = '// ' + 'TODO'

	$pwd = (pwd).Path + '\'

	$todos = gci $build_dir -r `
			| ? { $_.FullName -ne $readMeFile } `
			| ? { $_.FullName -notmatch $excludePattern } `
			| select-string $todoMark `
			| % { new-object PSObject `
					-Prop @{ Path = $_.Path.Replace($build_dir, '').Replace('\', '/'); `
							 LineNum = $_.LineNumber; `
							 Task = $_.Line.SubString($_.Line.IndexOf($todoMark) + $todoMark.Length + 1).Trim() `
						   } }

	$todoLines = @()

	$file = $null
	foreach ($todo in $todos)
	{
		if ($file -ne $todo.Path)
		{
			$file = $todo.Path
			$todoLines += "* link:.$($todo.Path)[]"
		}
		$todoLines += "** link:.$($todo.Path)#L$($todo.LineNum)[$($todo.LineNum)] $($todo.Task)"
	}

	[System.IO.File]::WriteAllLines($sourceTodoFile, $todoLines, $encoding)
}

Task Generate-Usage {
    $usageFile = "$build_dir\doc\Usage.adoc"

    ./patch-path

    echo $replacements

    $lines = @()

    $topLevel = gomer help

    $lines += "= Usage"
    $lines += ""

    $lines += "[listing]"
	$lines += "----"
    $lines += $topLevel.Trim()
    $lines += "----"

    $verbs = $topLevel | where { $_ -match "^\s+([\w-]+)\s+-\s+" } | % { $matches[1] }

    foreach ($verb in $verbs)
    {
        # auto-gen help has this structure:
        # blank line
        # command
        # blank line
        # options
        # blank line
        # blank line

        # Remove all blank lines, then put one back between command and options

        $help = gomer help $verb | ? { $_ -ne "" }

        $today = Get-Date -Format "yyyy-MM-dd"

        $command = $help[0]
        $options = $help[1..$help.Length] | `
          %{ $_ -replace "default: $today", 'default: <today>' `
                -replace "default: $env:USERNAME", 'default: <current user>' }

        $lines += ""
        $lines += "[[$verb-command]]"
        $lines += "== ``$verb`` Command"
        $lines += ""

		$lines += "[listing]"
		$lines += "----"
        $lines += $command
        $lines += ""
        $lines += $options
		$lines += "----"
    }

	[System.IO.File]::WriteAllLines($usageFile, $lines, $encoding)
}

Task Generate-Schema {
    ./patch-path

    $version = "0" + $version_major
    $path = "$build_dir\doc\schemas\Gomer-v$version.schema.json"
    gomer schema -o $path
}

Task Patch-Version -Depends Patch-AssemblyInfo
Task Patch-AssemblyInfo {

    $version = [string]::Join('.', @($version_major,
                                     $version_minor,
                                     $version_patch))

    $infoVersion = "$version-$version_label".Trim('-')

    echo "Setting version = $version"
    echo "Setting informational version = $infoVersion"

    # Patching assembly version attributes
    $verAttrPattern = "\[assembly: (Assembly\w*Version)\(`"([\d.]+)`"\)\]"
    $results = ls -r -include AssemblyInfo.cs | Select-String $verAttrPattern

    Write-Verbose "Patching"
    foreach ($result in $results)
    {
        $filePath = $result.Path
        $relPath = $filePath.Replace($build_dir, '')
        $lineNumber = $result.LineNumber

        $attribute = $result.Matches.Groups[1].Value

        $contents = [System.IO.File]::ReadAllLines($result.Path, $encoding)

        $newVersion = $version
        if ($attribute -eq "AssemblyInformationalVersion")
        {
            $newVersion = $infoVersion
        }

        $newLine = "[assembly: $attribute(`"$newVersion`")]"

        $contents[$lineNumber - 1] = $newLine

        Write-Verbose "$relPath : $lineNumber : $newLine"

    	[System.IO.File]::WriteAllLines($result.Path, $contents, $encoding)
    }
}

Task Build-Artifacts -Depends Clean-Artifacts,Build-ReadmeHtmlArtifact,Build-ExeArtifact
Task Clean-Artifacts {
    if (-not (Test-Path ./artifacts))
    {
        echo "Creating artifacts directory"
        mkdir artifacts
    }

    rm ./artifacts/* -r
}

Task Build-ReadmeHtmlArtifact {
    echo "Creating README.html"
    asciidoctor ./README.adoc -D ".\artifacts\"
}

Task Build-ExeArtifact {

    $bin = ".\Gomer.Cli\bin\$config"

    echo "Creating merged gomer.exe"
    ilmerge /targetplatform:"v4" /out:./artifacts/gomer.exe /ndebug /wildcards "$bin\gomer.exe" "$bin\*.dll"
}
