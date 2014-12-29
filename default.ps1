# NOTE: BOM is not recommended, and I think is what sometimes mucks up Asciidoc
# rendering in github, and hilighting in emacs, so we are writing to files
# without BOM. When creating the UTF-8 encoding object, we have to specify false
# explicitly. Otherwise it is marked as neither UTF-8 with, or without BOM.

Properties {
    $build_dir = Split-Path $psake.build_script_file
	$encoding = New-Object System.Text.UTF8Encoding($false)
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

	[System.IO.File]::WriteAllLines($sourceTodoFile, $todoLines, $encoding)
}

Task Extract-Usage {
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
