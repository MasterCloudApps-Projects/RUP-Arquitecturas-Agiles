### UserLogOut

* Nombre: UserLogOut
* Autor: Carlos Corcobado
* Fecha: 09/10/2020
* Descripción: Permite al usuario salir de la plataforma de forma segura
* Actores: Usuario
* Precondiciones: 
    * El usuario debe estar registrado y activo en el sistema
    * El usuario debe estar autenticado previamente en el sistema
    
* Flujo Normal
    * (1) El sistema verifica que la petición es correcta así como los datos del usuario
    * (2) El sistema limpia los datos de sesión asociados al usuario
    * (3) El sistema registra la fecha y hora de la acción
    * (4) El sistema redirige al usuario a la página de Login

* Flujo Alternativo
    * (1A) El sistema verifica la validez de la petición, si los datos no son correctos, se avisa con un mensaje al usuario del problema
        * Sessión caducada
        * Usuario no válido
        * Usuario inactivo o deshabilitado
    * (2A) El sistema NO limpia los datos de sesión asociados al usuario, y redirige al usuario a la página de error

* Poscondiciones: 
    * En la cuenta de usuario queda el registro con la fecha/hora del logout
    * El usuario tiene que volverse a logear para acceder a cualquier opción del sistema
