### AddCommentToBorrow

* Descripción:

    El usuario desde el Detalle del borrow podrá añadir comentarios al mismo mientras no este finalizado o cancelado.

* Actores:

    El usuario

* PreCondiciones:

    El usuario debe estar en el Detalle del borrow y por lo tanto logado.
    Además el trato no debe estar ni finalizado ni cancelado.

* Flujo Normal:

    1- El usuario pide al sistema "Añadir comentario"

    2- El sistema le solicita un texto.

    3- El usuario ordena grabar el comentario en el borrow.
    
    4- El sistema notifica a las personas participantes en el borrow.

* Poscondiciones:

    El usuario habrá añadido un nuevo comentario al borrow y se le habrá notificado a los usuarios participantes en el borrow.