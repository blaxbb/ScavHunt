// from - https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-enable-qrcodes?view=aspnetcore-6.0
// for scaffolding 2fa qrcode generation

window.addEventListener("load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    new QRCode(document.getElementById("qrCode"),
        {
            text: uri,
            width: 150,
            height: 150
        });
});