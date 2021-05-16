### UserLogin

* Nombre: UserLogin
* Autor: Fran Arboleya
* Fecha: 9/10/2020
* Descripción:
    * Permite al usuario logarse dentro del sistema, si este ya posee un usuario registrado
* Actores: Usuario
* Pre Condiciones:
    * Debe existir el usuario en el sistema previamente al login
* Flujo Normal
    1. El usuario introduce su email
    2. El usuario introduce su password y hace clic en el boton de Login
    3. El sistema valida el login
    4. El sistema muestra la pagina principal
* Flujo Alternativo

    1.1 El mail introducido no cumple el formato correcto, el sistema mostrara un mensaje.
    
    3.1 El sistema valida los datos introducidos (email/password), y alguno de los dos o los dos son incorrectos. El sistema muestra un mensaje de error
* Poscondiciones
    * El sistema redirige al usuario a la pagina principal
