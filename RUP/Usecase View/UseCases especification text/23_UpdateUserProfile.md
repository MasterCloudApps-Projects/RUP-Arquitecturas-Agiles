### UpdateUserProfile

* Nombre: UpdateUserProfile
* Autor: Fran Arboleya
* Fecha: 9/10/2020
* Descripción:
    * El usuario ha accedido a la consulta de informacion de su perfil, y puede modificar todos sus datos excepto el mail.
* Actores: Usuario
* Pre Condiciones
    * El usuario debe estar logado previamente en el sistema
    * El usuario debe haber accedido a la pagina de consulta de perfil de usuario
* Flujo Normal
    1. El sistema va a recuperar la información solicitada
    2. El sistema muestra en las cajas de texto la informacion soliticada
    3. El usuario puede modificar su password
    4. El usuario puede modificar su direccion
    5. El usuario puede modificar su nombre
    6. El usuario puede modificar su/s apellido/s
    7. El usuario puede modificar su fecha de nacimiento
    8. El usuario puede modificar su telefono
    9. El usuario hace clic en el boton "Actualizar datos"

* Flujo Alternativo

    1.1 Existe un error al recuperar la informacion de usuario, el sistema muestra un mensaje de error

    7.1 El usuario introduce un formato incorrecto de fecha de nacimiento y el sistema muestra un mensaje de error

    8.1 El usuario introduce un formato incorrecto de telefono y el sistema muestra un mensaje de error

    9.1 Se produce un error a la hora de guardar la informacion cambiada, el mensaje muestra un mensaje de error
* Poscondiciones
    * Se muestra un mensaje de información de usuario modificada correctamente