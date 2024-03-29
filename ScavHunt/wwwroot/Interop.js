﻿window.SetLocalStorage = (key, value) => {
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
    if (id) {
        var modal = document.getElementById(id)
        var instance = bootstrap.Modal.getInstance(modal);
        if (instance) {
            instance.hide();
        }
    }

    removeAllClass('modal-backdrop');

    document.body.classList.remove("modal-open");
    document.body.style = "";
}

removeAllClass = function (className) {
    var elements = document.getElementsByClassName(className);
    for (let element of elements) {
        element.parentNode.removeChild(element);
    }
}

window.SetupScanner = function (dotNetInstance, modalId, id) {

    removeAllClass('scan-region-highlight');

    var modal = ShowModal(modalId);

    var modalEle = document.getElementById(modalId);

    var qrFound = function (result) {
        dotNetInstance.invokeMethodAsync("QRResult", result.data);
    }

    import('/js/qr-scanner/qr-scanner.min.js').then(async (module) => {
        const QrScanner = module.default;
        // do something with QrScanner
        const qrScanner = new QrScanner(
            document.getElementById(id),
            result => qrFound(result),
            {
                highlightScanRegion: true,
                preferredCamera: localStorage.getItem("defaultCamera")
            },
        );

        modalEle.addEventListener("hide.bs.modal", event => {
            qrScanner.stop();
        });

        qrScanner.start();
        
        const cameras = await QrScanner.listCameras(true);

        const select = document.getElementById('camera-select');

        select.addEventListener("change", () => {
            if (select.value) {
                localStorage.setItem("defaultCamera", select.value);
                qrScanner.setCamera(select.value);
            }
        });

        select.innerHTML = "";

        var defaultOption = document.createElement('option');
        defaultOption.innerHTML = "Select a camera";
        select.appendChild(defaultOption);

        for (camera of cameras) {
            var option = document.createElement('option');
            option.value = camera.id;
            option.innerHTML = camera.label;
            select.appendChild(option);
        }
    });
}

window.SelectValue = function (id, value) {
    document.getElementById(id).selectedIndex = value;
}

window.InitSortable = function (id) {
    var root = document.getElementById(id);

    if (root == null) {
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

window.CreateQrCode = function (id, text) {
    new QRCode(document.getElementById(id),
        {
            text: text
        }
    );
}

window.CreateTooltips = function (selector) {
    const tooltips = document.querySelectorAll(selector);
    tooltips.forEach(t => bootstrap.Tooltip.getOrCreateInstance(t));
}