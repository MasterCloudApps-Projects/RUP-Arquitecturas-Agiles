### ShowBorrowDetail

* Nombre: ShowBorrowDetail
* Autor: Fran Arboleya
* Fecha: 9/10/2020
* Descripción:
    * El usuario desea consultar un borrow por diferentes motivos como consulta, validacion o rechazo. El sistema mostrada la información referente al borrow
* Actores: Usuario
* Pre Condiciones
    * Previamente existe un borrow para consultar, validar o rechazar
* Flujo Normal
    1. El sistema muestra la información referente al borrow
* Flujo Alternativo

    1.1 El borrow esta pendiente de validación, el sistema muestra estado "Pendiente de validación"

    1.2.A El usuario valida el borrow
    
    1.2.B El usuario rechaza el borrow
* Poscondiciones
    * El sistema muestra la información del borrow