### ConfirmBorrow

* Nombre: ConfirmBorrow
* Autor: Carlos Corcobado
* Fecha: 11/10/2020
* Descripción: Permite al borrower la aceptación del shary propuesto por el lender.
* Actores: Usuario - Borrower
* Precondiciones: 
    * El usuario debe estar registrado y activo en el sistema
    * El usuario debe estar autenticado previamente en el sistema
    * El lender debe haber compartido un producto
    * Debe existir un borrower

* Flujo Normal
    * (1) El sistema muestra una pantalla con la siguiente información:
        * Información del producto compartido
        * Información del usuario que solicita el borrow
        * Comentarios realizados por el usuario que solicita el borrow
        * Comentarios realizados por el usuario aceptador del borrow
    * (2) El usuario confirma el borrow
    * (3) El sistema muestra un mensaje indicando que se ha realizado correctamente

* Flujo Alternativo
    * (2A) El usuario pulsa sobre el botón de 'Rechazar' BorrowReject [link](17_RejectBorrow.md)
    * (3A) El sistema no puede cambiar el estado del shary y del producto y muestra el error 

* Poscondiciones: 
    * El lender tiene bloqueado el acceso al mismo hasta que termine el borrow
    * El producto no puede ser visualizado en el BorrowPlace
    * El borrow esta en estado 'Aceptado' y por tanto no puede ser modificado por ninguna de las 2 partes hasta que termine el periodo pactado
