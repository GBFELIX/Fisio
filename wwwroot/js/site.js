//document.getElementById("checkall").addEventListener("change", function () {
//    var checkboxes = document.querySelectorAll(".checkbox-item");
//    checkboxes.forEach(cb => cb.checked = this.checked);
//});
function contar() {
    return document.querySelectorAll(".checkbox-item:checked").length
}
function abrirModal() {
    var total = contar();
    if (total == 0) {
        alert("Nenhum item selecionado.");
        return;
    }
}