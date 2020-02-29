

window.modelSystemEditor = {
    /**
     * 
     * @param {any} elementRef
     * @param {any} objectRef
     */
    enableInteract: function(elementRef, objectRef) {
        interact(elementRef)
            .resizable({
                // resize from all edges and corners
                edges: { left: true, right: true, bottom: true, top: true },

                modifiers: [
                    // keep the edges inside the parent
                    interact.modifiers.restrictEdges({
                        outer: "parent"
                    }),

                    // minimum size
                    interact.modifiers.restrictSize({
                        min: { width: 150, height: 150 }
                    })
                ],

                inertia: true
            })
            .draggable({})
            /*/.draggable({
                inertia: false,
                modifiers: [
                    interact.modifiers.restrictRect({
                        restriction: "parent",
                        endOnly: true
                    }),
                ],
                autoScroll: true,
                onmove: (event) => onMove(event, objectRef),
                // onend: (event) => onEnd(event, objectRef)

            }) */
            .on('dragmove', (event) => onMove(event,objectRef))
            .on("resizemove", (event) => onResizeMove(event, objectRef));
            
    }
};

/**
 * 
 * @param {any} event
 * @param {any} objectRef
 */
function onResizeMove(event, objectRef) {
    var target = event.target;
    var x = (parseFloat(target.getAttribute("data-x")) || 0);
    var y = (parseFloat(target.getAttribute("data-y")) || 0); 

    // update the element's style
    target.style.width = event.rect.width + "px";
    target.style.height = event.rect.height + "px";

    // translate when resizing from top or left edges
    x += event.deltaRect.left;
    y += event.deltaRect.top;

    target.style.webkitTransform = target.style.transform =
        "translate(" + x + "px," + y + "px)";

    target.setAttribute("data-x", x);
    target.setAttribute("data-y", y);

}


/**
 * 
 * @param {any} event
 * @param {any} objectRef
 */
function onEnd(event, objectRef) {
    var target = event.target;
    // keep the dragged position in the data-x/data-y attributes
    var x = (parseFloat(target.getAttribute("data-x")) || 0) + event.dx;
    var y = (parseFloat(target.getAttribute("data-y")) || 0) + event.dy;

    console.log(objectRef);
    console.log(event);
    objectRef.invokeMethodAsync("UpdateBounds", x, y, 0, 0);
}

/**
 * 
 * @param {any} event
 * @param {any} objectRef
 */
function onMove(event, objectRef) {
    var target = event.target;
    // keep the dragged position in the data-x/data-y attributes
    var x = (parseFloat(target.getAttribute("data-x")) || 0) + event.dx;
    var y = (parseFloat(target.getAttribute("data-y")) || 0) + event.dy;

    // translate the element
    target.style.webkitTransform =
        target.style.transform =
        "translate(" + x + "px, " + y + "px)";

    // update the position attributes
    target.setAttribute("data-x", x);
    target.setAttribute("data-y", y);
}