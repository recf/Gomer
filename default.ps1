Properties {
    $build_dir = Split-Path $psake.build_script_file
}

Task Default -Depends Extract-Todos,Extract-Usage

Task Extract-Todos {
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

	$encoding = New-Object System.Text.UTF8Encoding($true)
	[System.IO.File]::WriteAllLines($sourceTodoFile, $todoLines, $encoding)
}

Task Extract-Usage {
    $usageFile = "$build_dir\doc\Usage.adoc"

    ./patch-path

    $openListing = "[listing]`n----"
    $closeListing = "----"
    $lines = @()

    $topLevel = gomer help

    $lines += "= Usage"
    $lines += ""

    $lines += $openListing
    $lines += $topLevel.Trim()
    $lines += $closeListing

    $verbs = $topLevel | where { $_ -match "^\s+(\w+)\s+-\s+" } | % { $matches[1] }

    foreach ($verb in $verbs)
    {
        $lines += ""
        $lines += "[[$verb-command]]"
        $lines += "== ``$verb`` Command"
        $lines += ""

        $lines += $openListing
        $lines += gomer help $verb
        $lines += $closeListing
    }

	$encoding = New-Object System.Text.UTF8Encoding($true)
	[System.IO.File]::WriteAllLines($usageFile, $lines, $encoding)
}
