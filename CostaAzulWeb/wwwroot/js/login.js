document.getElementById("loginSubmit").addEventListener("click", function () {
    const form = document.getElementById("loginForm");
    const formData = new FormData(form);

    fetch('/Login/ValidUser', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            const loginMessage = document.getElementById("loginMessage");
            if (data.success) {
                loginMessage.innerHTML = `<div class="alert alert-success">${data.message}</div>`;
                setTimeout(() => {
                    window.location.reload(); // Recargar la página principal
                }, 1000);
            } else {
                loginMessage.innerHTML = `<div class="alert alert-danger">${data.message}</div>`;
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
});
