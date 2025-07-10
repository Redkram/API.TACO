param(
    [string]$ServiceName,
    [string]$Password = "sC7xSn5yG3Zl0Wc",
    [string]$CAFile = "my-ca-root.crt"  # puede quedarse por defecto si quieres
)

$certPath = "$PWD"
$keyFile = "$certPath\$ServiceName.key"
$crtFile = "$certPath\$ServiceName.crt"
$pfxFile = "$certPath\aspnetcore.pfx"
$caFilePath = "$certPath\$CAFile"

Write-Host "Generando certificado para $ServiceName..."
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout $keyFile -out $crtFile -subj "/CN=$ServiceName"

Write-Host "Generando archivo PFX..."
openssl pkcs12 -export -out $pfxFile -inkey $keyFile -in $crtFile -password pass:$Password

Write-Host "Copiando certificado PFX al contenedor ${ContainerName}..."
docker cp $pfxFile "${ContainerName}:/https/aspnetcore.pfx"

if (Test-Path $caFilePath) {
    Write-Host "Archivo CA interno (${CAFile}) encontrado. Puedes copiarlo manualmente si es necesario."
} else {
    Write-Warning "Archivo CA interno (${CAFile}) no encontrado."
}

Write-Host "Proceso terminado para ${ServiceName}"

Remove-Item -Path $crtFile -Force -ErrorAction SilentlyContinue
Remove-Item -Path $keyFile -Force -ErrorAction SilentlyContinue
