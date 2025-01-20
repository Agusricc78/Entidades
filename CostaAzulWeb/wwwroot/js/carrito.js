document.addEventListener("DOMContentLoaded", function () {
    // Evento para aumentar la cantidad
    document.querySelectorAll(".btn-increment").forEach(button => {
        button.addEventListener("click", function () {
            const productoId = this.dataset.id;
            const stock = parseInt(this.dataset.stock, 10);
            const cantidadInput = document.getElementById(`cantidad-${productoId}`);
            let cantidadActual = parseInt(cantidadInput.value, 10);

            if (cantidadActual < stock) {
                cantidadInput.value = cantidadActual + 1; // Incrementar cantidad
            }
        });
    });

    // Evento para disminuir la cantidad
    document.querySelectorAll(".btn-decrement").forEach(button => {
        button.addEventListener("click", function () {
            const productoId = this.dataset.id;
            const cantidadInput = document.getElementById(`cantidad-${productoId}`);
            let cantidadActual = parseInt(cantidadInput.value, 10);

            if (cantidadActual > 1) {
                cantidadInput.value = cantidadActual - 1; // Decrementar cantidad
            }
        });
    });

    // Evento para actualizar cantidad
    document.querySelectorAll(".btn-update").forEach(button => {
        button.addEventListener("click", function (event) {
            event.preventDefault(); // Evitar el envío completo del formulario

            const productoId = this.closest("form").querySelector("input[name='Id_Producto']").value;
            const carritoId = this.closest("form").querySelector("input[name='Id_Carrito']").value;
            const cantidadInput = document.getElementById(`cantidad-${productoId}`); // Tomar valor del input visible
            const cantidad = parseInt(cantidadInput.value, 10); // Valor actualizado del input

            // Llamar al controlador para actualizar la cantidad
            fetch('/Carrito/ActualizarCant', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                body: new URLSearchParams({
                    Id_Producto: productoId,
                    Id_Carrito: carritoId,
                    Cantidad: cantidad
                })
            })
                .then(response => {
                    if (response.ok) {
                        return response.text();
                    } else {
                        throw new Error('Error al actualizar la cantidad.');
                    }
                })
                .then(() => {
                    alert('La cantidad del producto se actualizó correctamente.');
                    location.reload(); // Recargar la página para reflejar los cambios
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('No se pudo actualizar la cantidad del producto.');
                });
        });
    });
});

