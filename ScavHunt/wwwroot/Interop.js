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
}

window.HideModal = function (id) {
    var modal = document.getElementById(id)
    bootstrap.Modal.getInstance(modal).hide();
}