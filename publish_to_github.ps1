# Upload 8 ORIGINAL Word reports to the already existing public repository.
# Requires gh and Git, with GitHub authorization. No force pushes.
$ErrorActionPreference = 'Stop'
$repo = 'Girfor/EnergyMeter-System'
$docs = Join-Path $PSScriptRoot 'Reports'
if (!(Test-Path $docs)) { throw 'Missing Reports folder' }
if (!(Get-Command gh -ErrorAction SilentlyContinue)) { throw 'Install GitHub CLI: https://cli.github.com/' }
if (!(Get-Command git -ErrorAction SilentlyContinue)) { throw 'Install Git: https://git-scm.com/' }
& gh auth status
if ($LASTEXITCODE -ne 0) { throw 'Run gh auth login first' }
$tmp = Join-Path $env:TEMP ('energymeter-' + [guid]::NewGuid().ToString('N'))
& gh repo clone $repo $tmp
if ($LASTEXITCODE -ne 0) { throw 'Failed to clone repository' }
$target = Join-Path $tmp 'Reports'
New-Item -ItemType Directory -Force -Path $target | Out-Null
Copy-Item -Path (Join-Path $docs '*.docx') -Destination $target -Force
& git -C $tmp add Reports
& git -C $tmp diff --cached --quiet
if ($LASTEXITCODE -ne 0) {
    & git -C $tmp commit -m 'Add original DOCX coursework reports'
    if ($LASTEXITCODE -ne 0) { throw 'Failed to commit' }
    & git -C $tmp push origin main
    if ($LASTEXITCODE -ne 0) { throw 'Failed to push' }
}
Write-Host 'Reports uploaded (or already identical) successfully'
