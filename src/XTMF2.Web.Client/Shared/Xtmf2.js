
window.afterRender = () => {
    $('#sidebar-toggle').on('click', function () {
        $('#main-sidebar').toggleClass('active');
    });
}

window.xtmf2 = {
    hideOverlay: () => {
        $('#opaque').css('display','none');
    }
}