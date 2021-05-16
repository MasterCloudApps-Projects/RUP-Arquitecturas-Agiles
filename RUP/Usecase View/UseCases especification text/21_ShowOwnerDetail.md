### ShowUserScore

* Nombre: ShowUserScore
* Autor: Carlos Corcobado
* Fecha: 11/10/2020
* Descripción: Permite a un usuario visualizar la valoraciones realizadas sobre otro usuario
* Actores: Usuario
* Precondiciones: 
    * El lender debe estar registrado y activo en el sistema
    * El lender debe estar autenticado previamente en el sistema
    * Debe existir un borrow finalizado y valorado

* Flujo Normal
    * (1) El usuario solicita las valoraciones de un usuario concreto desde un producto o un borrow
    * (2) El sistema muestra una pantalla con la información de toda las valoraciones realziadas sobre ese usuario

* Flujo Alternativo
    * (2A) El sistema no puede recuperar los datos y muestra una pantalla con el error solicitando al usuario que lo intente más tarde


TODO -> mostrar informacion del usuario
        mostrar informacion de valoraciones
        mostrar informacion de productos disponibles