window.SetLocalStorage = (key, value) => {
    localStorage.setItem(key, value);
}

window.GetLocalStorage = (key) => {
    return localStorage.getItem(key);
}

window.ClearLocalStorage = (key) => {
    localStorage.clear();
}

window.ShowModal = function (id) {
    var modal = new bootstrap.Modal(document.getElementById(id));
    modal.show();
    return modal;
}

window.HideModal = function (id) {
    var modal = document.getElementById(id)
    bootstrap.Modal.getInstance(modal).hide();
}

window.SetupScanner = function (dotNetInstance, modalId, id) {

    var modal = ShowModal(modalId);

    var modalEle = document.getElementById(modalId);

    const questionRegex = /^https:\/\/scavhunt\.bbarrett\.me\/q\/(?<id>\d{5})$/m;

    var qrFound = function (result) {

        var match = result.data.match(questionRegex);
        if (match) {
            console.log('HIT', match[1]);
            modal.hide();
            dotNetInstance.invokeMethodAsync("QRResult", match[1]);

        }
        else {
            console.log('miss', result.data);
        }
    }

    import('/js/qr-scanner/qr-scanner.min.js').then((module) => {
        const QrScanner = module.default;
        // do something with QrScanner
        const qrScanner = new QrScanner(
            document.getElementById(id),
            result => qrFound(result),
            { /* your options or returnDetailedScanResult: true if you're not specifying any other options */ },
        );

        modalEle.addEventListener("hide.bs.modal", event => {
            qrScanner.stop();
        });

        qrScanner.start();
    });
}

window.SelectValue = function (id, value) {
    document.getElementById(id).selectedIndex = value;
}