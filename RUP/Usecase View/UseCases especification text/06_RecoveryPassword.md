### RecoveryPassword

* Descripción:

    El usuario desde el Login puede recuperar su password, se le solicitará el email con el que se registró, y se le manda una notificación para resetear su password. 

* Actores:

    El usuario

* PreCondiciones:

    El usuario debe estar en el Login.

* Flujo Normal:

    1- El usuario pide al sistema "Recordar Password"

    2- El sistema le solicita email

    3- El sistema comprueba si existe email

    4- El sistema notifica al usuario como seguir con el usecase SetNewPassword.

    5- El sistema redirige al login 

* Flujo Alternativo:

    3A- Si el sistema no tiene ese email registrado el proceso se cancela

* Poscondiciones:

    El usuario recibe un email para resetear su password.