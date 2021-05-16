### DeactivateUser

* Descripción:

    El usuario desde su perfil puede dar de baja su cuenta y se le mandará una notificación indicandoselo. 

* Actores:

    El usuario

* PreCondiciones:

    El usuario debe estar en su perfil, por lo que debe estar logado y no tener ningun trato en curso.

* Flujo Normal:

    1- El usuario da la orden al sistema de "Darse de Baja"

    2- El sistema le pide si desea continuar o cancelar con la baja

    3- Se notifica al usuario

    4- El sistema redirige al login  

* Flujo Alternativo:

    2A- El usuario desea cancelar, el proceso se cancela

    2B1- El usuario desea continuar, el usuario tiene tratos en curso, se cancela la baja del usuario

    2B2- El usuario desea continuar, el sistema valida que no tiene tratos en curso, da de baja el usuario

* Poscondiciones:

    El usuario estará dado de baja