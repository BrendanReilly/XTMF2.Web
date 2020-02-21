
window.notyf = new Notyf();
window.successMessage = (message) => {
    window.notyf.success(message);
};

window.afterRender = function() {
    $('#sidebar-toggle').on('click', function () {
        $('#main-sidebar').toggleClass('active');
    });
}
