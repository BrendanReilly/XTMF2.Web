window.notyf = new Notyf();

window.xtmfNotificationService = {
    successMessage: (message) => {
        window.notyf.success(message);
    },errorMessage: (message) => {
        window.notyf.success(message);
    },
    warningMessage: (message) => {
        window.notyf.warningMessage(message);
    },
}