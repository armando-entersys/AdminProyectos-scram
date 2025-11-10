$file = Get-ChildItem 'MKT*.docx' | Select-Object -First 1
Add-Type -AssemblyName System.IO.Compression.FileSystem
$zip = [System.IO.Compression.ZipFile]::OpenRead($file.FullName)
$entry = $zip.Entries | Where-Object { $_.Name -eq 'document.xml' }
$stream = $entry.Open()
$reader = New-Object System.IO.StreamReader($stream)
$content = $reader.ReadToEnd()
$reader.Close()
$stream.Close()
$zip.Dispose()

[xml]$xml = $content
$ns = New-Object Xml.XmlNamespaceManager($xml.NameTable)
$ns.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")

$paragraphs = $xml.SelectNodes("//w:p", $ns)
foreach ($p in $paragraphs) {
    $texts = $p.SelectNodes(".//w:t", $ns)
    $line = ""
    foreach ($t in $texts) {
        $line += $t.InnerText
    }
    if ($line.Trim()) {
        Write-Output $line
    }
}
