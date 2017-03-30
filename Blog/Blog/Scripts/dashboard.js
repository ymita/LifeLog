window.onload = function () {
    var dialogButton = document.getElementById("dialogButton");
    dialogButton.onclick = function () {
        var hiddenIdElem = document.getElementById("id");
        var _idText = this.parentElement.parentElement.cells["0"].textContent;
        var _id = parseInt(_idText);
        hiddenIdElem.value = _id;
    }
}