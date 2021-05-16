### UnsharedProduct

* Nombre: UnsharedProduct
* Autor: Fran Arboleya
* Fecha: 9/10/2020
* Descripción:
    * El usuario tiene un producto para compartir y quiere dejar de tenerlo disponible para compartir. El sistema le permite realizar esta accion de "UnShary" y le mostrara un mensaje de accion completada al acabar.
* Actores: Usuario
* Pre Condiciones
    * El usuario debe tener al menos 1 producto dado de alta y que no este asociado a un borrow activo en el momento de la accion de "UnShary"
    * El usuario ha seleccionado un producto para descompartir
* Flujo Normal
    1. El sistema muestra un mensaje de confirmacion
    2. El usuario confirma la accion
    3. El sistema muestra un mensaje de completado
    4. El sistema marca el producto como no visible para borrow
* Flujo Alternativo

    3.1.A El producto esta asociado a un borrow en curso, el sistema no le muestra la opcion de "UnShary"

    3.1.B El producto esta asociado a un borrow pdte de validar, el sistema no le muestra la opcion de "UnShary"

    2.1 El usuario cancela la accion, el producto sigue estando disponible para "Shary"
* Poscondiciones
    * El producto ya no aparece en la lista del SharyPlace
