### ActivateUser

* Nombre: ActivateUser
* Autor: Carlos Corcobado
* Fecha: 09/10/2020
* Descripción: Permite activar la cuenta de un usuario dentro de la plataforma
* Actores: Usuario
* Precondiciones: 
    * El usuario debe estar registrado en el sistema
    * El usuario debe haber recibido una notificación con el link para realizar la activación de la cuenta.
    
* Flujo Normal
    * (1) El sistema verifica la validez de la petición y cambia los datos de usuario
    * (2) El sistema muestra un mensaje de activación correcta
    * (3) El usuario acepta y se le envia al login

* Flujo Alternativo
    * (1A) El sistema verifica la validez de la petición, si los datos no son correctos, se avisa con un mensaje al usuario del problema
        * Peticion demasiado antiguo
        * Peticion no verificable
        * Usuario inactivo o deshabilitado
    * (4A) El usuario no aceptacion la activacion

* Poscondiciones: 
    * La cuenta de usuario queda activada y lista para ser usada por el usuario

