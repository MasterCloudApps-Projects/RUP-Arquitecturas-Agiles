### ShowBorrowList

* Descripción:

    El usuario en su perfil podrá ver un historial de borrows donde podrá ver el detalle de cada uno de ellos y estos estarán ordenados por fecha de creación descendentemente

* Actores:

    El usuario

* PreCondiciones:

    El usuario debe estar logado y en su perfil viendo sus borrows o en el detalle de uno de sus producto viendo los borrows que se han hecho con el.

* Flujo Normal:

    1- El usuario solicita al sistema ver el detalle de borrows
    
    2- El sistema mostrará un listado con todos los borrows, los datos a mostrar son:

        - Fecha de creación
        - Producto involucrado
        - Estado del Borrow
        - Numero de comentarios

    Además el sistema permitira acceder al detalle de cada borrow

* Poscondiciones:

    El usuario tendrá visible el listado de tratos ordenados por fecha de creación descendentemente.