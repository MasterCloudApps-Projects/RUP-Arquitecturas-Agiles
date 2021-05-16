### CreateBorrow

* Nombre: CreateBorrow
* Autor: Fran Arboleya
* Fecha: 9/10/2020
* Descripción:
    * El usuario solicita el borrow. El sistema creara 1 borrow y este quedara en estado pendiente de validación.
* Actores: Usuario
* Pre Condiciones
    * El producto tiene que estar disponible para hacer borrow
    * El producto debe aparece en las busquedas del SharePlace
* Flujo Normal
    1. El sistema crea el borrow
    2. El sistema deja el estado del borrow en pdte de validacion
    3. El Sistema muetra un mensaje de completado
* Flujo Alternativo

    2.1  El producto ya no esta disponible en el sistema, se muestra un mensaje de error 
* Poscondiciones
    * El producto esta pendiente de ser validado por el usuario owner del producto

DUDA-NOTA: un producto puede tener mas de una solicitud de borrow?? cuando un producto esta pendiente puede aparecer en el SharePlace
SOLUCION DE LA DUDA: Si tenemos multiples solicitudes, para poder tener una SAGA en la validacion del borrow