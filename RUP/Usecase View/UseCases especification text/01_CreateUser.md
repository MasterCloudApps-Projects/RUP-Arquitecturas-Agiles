### CreateUser

* Nombre: CreateUser
* Autor: Fran Arboleya
* Fecha: 9/10/2020
* Descripción: 
    * Permite registar a un usuario en el sistema. El sistema comprobara si existe o no el usuario. Si no existe permitira la creación del usuario, en caso contrario mostrata un mensaje de error
* Actores: Usuario
* Pre Condiciones:
    * El usuario no debe estar registrado previamente
* Flujo Normal:
    1. El sistema mostrara la pagina de registro de usuario
    2. El usuario introduce su nombre
    3. El usuario introduce sus apellidos
    4. El usuario introduce su direccion
    5. El usuario introduce su fecha de nacimiento
    6. El usuario introduce a su telefono
    7. El usuario debe rellenar la informacion referente a su mail
    8. El usuario introduce la password seleccionada de su cuenta
    9. El sistema registra el usuario
    10. El sistema muestra un mensaje de usuario creado correctamente
* Flujo Alternativo
  
    5.1. El usuario introduce caracteres prohibidos en la fecha de nacimiento, el sistema le mostrara un mensaje de error
    6.1.A El usuario introduce caracteres prohibidos en el telefono, el sistema le mostrara un mensaje de error
    6.1.A El usuario introduce una longitud de caracteres en el telefono que supera el maximo permitido, el sistema le mostrara un mensaje de error
    8.1 El usuario ha introducido una password que no cumple la politica de password definida, el sistema lanza un mensaje de error.
    
    9.1 El sistema comprueba la existencia del usuario en el sistema, si esta duplicado lanza un mensaje de error 
* Poscondiciones
    * El usuario esta dado de alta en el sistema