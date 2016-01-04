# NOTE: BOM is not recommended, and I think is what sometimes mucks up Asciidoc
# rendering in github, and hilighting in emacs, so we are writing to files
# without BOM. When creating the UTF-8 encoding object, we have to specify false
# explicitly. Otherwise it is marked as neither UTF-8 with, or without BOM.

Properties {
    $build_dir = Split-Path $psake.build_script_file
	$encoding = New-Object System.Text.UTF8Encoding($false)
    $config = "Release"
}

Task Default -Depends Generate-Docs

Task Generate-Docs -Depends Generate-Todo
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

Task Build-CleanArtifacts -Depends Clean-Artifacts,Build-Artifacts

Task Build-Artifacts -Depends Build-ReadmeHtmlArtifact,Build-ZipArtifact

Task Create-ArtifactsDir {
    if (-not (Test-Path ./artifacts))
    {
        echo "Creating artifacts directory"
        mkdir artifacts
    }

    rm ./artifacts/* -r
}

Task Clean-ArtifactsDir {
    if (-not (Test-Path ./artifacts))
    {
        return
    }

    echo "Cleaning artifacts directory"
    rm ./artifacts/* -r
}

Task Build-ReadmeHtmlArtifact -Depends Create-ArtifactsDir {
    echo "Creating README.html"
    asciidoctor ./README.adoc -D ".\artifacts\"
}

Task Build-ZipArtifact -Depends Create-ArtifactsDir  {

    $bin = ".\Gomer\bin\$config"

    $workingdir = ".\artifacts\Gomer"
    mkdir $workingdir

    echo "Copying contents of $bin -> $workingdir"
    xcopy "$bin\*" "$workingdir"

    echo "Creating zip"
    7z a ./artifacts/gomer.zip $workingdir

    rm $workingdir -r
}

Task Build-ExeArtifact -Depends Create-ArtifactsDir  {

    $bin = ".\Gomer\bin\$config"

    echo "Creating merged gomer.exe"
    ilmerge /targetplatform:"v4" /out:./artifacts/gomer.exe /ndebug /wildcards "$bin\gomer.exe" "$bin\*.dll"
}
