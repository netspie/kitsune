function EditableList_disableIndents(parentId) {
    var parentElement = document.getElementById(parentId);

    var treeViewArrows = parentElement.querySelectorAll('.mud-treeview-item-arrow');
    treeViewArrows.forEach(function (el) {
        el.style.width = 0;
        el.style.padding = 0;
        el.style.margin = 0;
    });

    var treeViewContents = parentElement.querySelectorAll('.mud-treeview-item-content');
    treeViewContents.forEach(function (el) {
        el.style.padding = 0;
        el.style.margin = 0;
    });
}

function EditableList_removeMenuIconPadding(parentId) {
    var parentElement = document.getElementById(parentId);

    var elements = parentElement.querySelectorAll('.mud-icon-button');
    elements.forEach(function (el) {
        el.style.paddingTop = 0;
        el.style.paddingBottom = 0;
        el.style.background = "none";

        el.style.transition = "color 0.3s"; // Add smooth transition for color change
        el.addEventListener("mouseover", function () {
            el.style.color = "black"; // Change color to black on hover
        });
        el.addEventListener("mouseout", function () {
            el.style.color = ""; // Revert to default color on mouseout
        });
    });
}
    