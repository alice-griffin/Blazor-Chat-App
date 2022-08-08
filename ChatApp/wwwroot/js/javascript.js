function getBoundingClientRect(id) {
    const position = document.getElementById(id).getBoundingClientRect().toJSON();
    return position;
}

function stopDragging() {
    document.onmouseup = null;
    document.onmousemove = null;
}

function setStyle(id, pos1, pos2) {
    var todoEl = document.getElementById(id); 
    todoEl.style.top = (todoEl.offsetTop - pos2) + "px";
    todoEl.style.left = (todoEl.offsetLeft - pos1) + "px";
}