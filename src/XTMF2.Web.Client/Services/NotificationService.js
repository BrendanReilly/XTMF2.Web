window.notyf = new Notyf({
  duration: 3000
});

window.xtmfNotificationService = {
  successMessage: message => {
    window.notyf.success(message);
  },
  errorMessage: message => {
    window.notyf.error(message);
  },
  warningMessage: message => {
    window.notyf.warningMessage(message);
  }
};
