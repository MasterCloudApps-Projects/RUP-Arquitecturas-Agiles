### SetScoreBorrow

* Nombre: SetScoreBorrow
* Autor: Carlos Corcobado
* Fecha: 11/10/2020
* Descripción: Permite al borrower valorar tanto al lender como al borrow
* Actores: Usuario - Borrower
* Precondiciones: 
    * El borrower debe estar registrado y activo en el sistema
    * El borrower debe estar autenticado previamente en el sistema
    * El borrower debe ser el solicitante del borrow a valorar
    * El borrow a valorar debe haber terminado

* Flujo Normal
    * (1) El sistema muestra una pantalla con la siguiente información:
        * Información básica del producto compartido
        * Información básica del usuario que publicó el producto
    * (2) El usuario marca la puntuación deseada
    * (3) El sistema registra la valoración del borrow
    * (4) El sistema muestra un mensaje indicando que la valoración se ha realizado correctamente

* Flujo Alternativo
    * (4A) El usuario cancela la operación
    * (5A) El sistema no puede guardar la puntuación realizada y muestra error

* Poscondiciones: 
    * El borrower no puede volver a valorar el borrow

