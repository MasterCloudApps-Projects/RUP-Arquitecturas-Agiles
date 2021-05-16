### ShowUserProfile

* Nombre: ShowUserProfile
* Autor: Fran Arboleya
* Fecha: 9/10/2020
* Descripción:
    * El usuario consulta su informacion en el sistema y este redirecciona  a la pagina solicitada. El sistema le mostrara al usuario toda la información personal, de productos y deals referentes al usuario logado.
* Actores: Usuario
* Pre Condiciones
    * El usuario debe estar logado previamente en el sistema
* Flujo Normal
    1. El sistema recupera la información de usuario
    2. El sistema recupera la información de sharyproducts 
    3. El sistema recuepra la información de borrows 
    4. El sistema muestra la información recuperada
* Flujo Alternativo

    1.1 El sistema encuentra un error al recuperar la información y muestra un mensaje de error

    2.1  El usuario no tiene aun ningun producto dado de alta para mostrar, el sistema muestra el listado vacio

    3.1 El usuario no tiene aun ningun deal para mostrar, el sistema muestra un listado vacio
* Poscondiciones
    * Muestra la pagina solicitada, con la informacion requerida y recuperada por el sistema
