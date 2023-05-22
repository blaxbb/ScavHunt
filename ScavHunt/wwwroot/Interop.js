window.SetLocalStorage = (key, value) => {
    localStorage.setItem(key, value);
}

window.GetLocalStorage = (key) => {
    return localStorage.getItem(key);
}

window.ClearLocalStorage = (key) => {
    localStorage.clear();
}

window.closeNav = (id) => {
    var collapse = document.getElementById(id);
    var bsCollapse = new bootstrap.Collapse(collapse, {
        toggle: false
    });
    bsCollapse.hide();
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

window.InitSortable = function (id) {
    var root = document.getElementById(id);

    if (root == null)
    {
        return;
    }

    Sortable.create(root, {
        handle: '.oi-grid-four-up',
        animation: 150,
        group: "g"
    });
    var children = root.querySelectorAll(".list-group");
    [...children].forEach(e => Sortable.create(e, {
            handle: '.oi-grid-four-up',
            animation: 150,
            group: "g"
        })
    );
}

window.GetQuestionTree = function (id) {
    var root = document.getElementById(id);
    var res = _BuildQuestionTree(root);
    return res.children;
}

_BuildQuestionTree = function (element) {
    if (element == null) {
        return;
    }
    
    var nodes = element.querySelectorAll(":scope > .list-group-item");
    var children = [];
    [...nodes].forEach(n => {
        children.push(_BuildQuestionTree(n.querySelector(".list-group")));
    });

    return { id: element.id.substring(13), children: children };
}

//https://docs.microsoft.com/en-us/aspnet/core/blazor/file-downloads?view=aspnetcore-6.0
async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

function triggerFileDownload(fileName, url) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}