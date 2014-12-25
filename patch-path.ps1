$script:codedir = Split-Path $myinvocation.mycommand.path
$script:bin = "$codedir\Gomer.Cli\bin\Debug"

if ($env:Path.Contains($bin))
{
    echo "already patched"
}
else
{
    echo "patching Path"
    $env:path += ";$bin"
}
