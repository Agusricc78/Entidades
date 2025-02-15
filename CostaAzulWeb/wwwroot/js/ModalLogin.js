
    $(document).ready(function () {

        verificarEstadoLogin();

    // Manejar el envío del formulario de login
    $('#loginForm').on('submit', function (event) {
        event.preventDefault();
    $('#loginErrorMessage').hide();
    const formData = {
        Username: $('#username').val(),
    Password: $('#password').val()
                };

    if (!formData.Username || !formData.Password) {
        $('#loginErrorMessage').text('Por favor, complete todos los campos.').show();
    return;
                }

    $.ajax({
        url: '@Url.Action("ValidUser", "Login")',
    type: 'POST',
    data: formData,
    success: function (response) {
                        if (response.success) {
        $('#loginModal').modal('hide');
    verificarEstadoLogin();

    setTimeout(function () {
        window.location.reload();

                            }, 200);
                        } else {
        $('#loginErrorMessage').text(response.message).show();
                        }
                    },
    error: function () {
        $('#loginErrorMessage').text('Ocurrió un error al procesar la solicitud.').show();
                    }
                });
            });
        });

    function verificarEstadoLogin() {
        $.ajax({
            url: '@Url.Action("Estado", "Login")',
            type: 'GET',
            success: function (response) {
                if (response.isLoggedIn) {
                    $('#logoutButton').show();
                    $('#loginButton').hide();
                    if (response.userId == 1) {
                        $('#gestionProductos').show();
                    }
                } else {
                    $('#loginButton').show();
                    $('#logoutButton').hide();
                    $('#gestionProductos').hide();
                }
            },
            error: function () {
                console.error('Error al verificar el estado de login.');
            }
        });
        }