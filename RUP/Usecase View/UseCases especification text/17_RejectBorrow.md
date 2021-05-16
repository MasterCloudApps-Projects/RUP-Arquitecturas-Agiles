### RejectBorrow

* Nombre: RejectBorrow
* Autor: Fran Arboleya
* Fecha: 9/10/2020
* Descripción:
    * El usuario recibe una notificacion de borrow pdte de validar. Este acceder al detalle del borrow y lo marca como rechazado. El sistema marcara el sistema como rechazado y notificara al usuario solicitante del rechazo
* Actores: Usuario
* Pre Condiciones
    * Debe existir un borrow pendiente de validacion
* Flujo Normal
    1. El usuario recibe una notificacion de borrow pdte de validacion
    2. El usuario accede al borrow
    3. El usuario marca la accion de "Rechazar" borrow
    4. El sistema muestra una notificacion de rechazo del borrow
* Flujo Alternativo

    2.1.A El usuario ha aceptado otro borrow antes, el sistema mostrara un mensaje
    
    2.1.B El usuario ha marcado el producto como descompartido antes, el sistema mostrara un mensaje
* Poscondiciones
    * El producto sigue disponible para compartir