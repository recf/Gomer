Properties {
    $build_dir = Split-Path $psake.build_script_file
}

Task Default -Depends ExtractTodos
Task ExtractTodos {
	$readMeFile = "$build_dir\README.adoc"

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

	$todoContents = cat $readMeFile

	$srcTodoDelim = '=== From Source'

	$srcTodoLineNum = $todoContents.IndexOf($srcTodoDelim)

	$cleanTodoLines = $todoContents[0..$srcTodoLineNum] + ""

	$file = $null
	foreach ($todo in $todos)
	{
		if ($file -ne $todo.Path)
		{
			$file = $todo.Path
			$cleanTodoLines += "* link:.$($todo.Path)[]"
		}
		$cleanTodoLines += "** link:.$($todo.Path)#L$($todo.LineNum)[$($todo.LineNum)] $($todo.Task)"
	}

	$encoding = New-Object System.Text.UTF8Encoding($true)
	[System.IO.File]::WriteAllLines($readMeFile, $cleanTodoLines, $encoding)
}
