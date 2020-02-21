
window.afterRender = () => {
    $('#sidebar-toggle').on('click', function () {
        $('#main-sidebar').toggleClass('active');
    });
}
