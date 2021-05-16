### AccessBorrowPlace

* Nombre: AccessBorrowPlace
* Autor: Carlos Corcobado
* Fecha: 10/10/2020
* Descripción: Permite al usuario tener acceso al BorrowPlace para visualizar todos los productos compartidos por el resto de usuarios.
* Actores: Usuario
* Precondiciones: 
    * El usuario debe estar registrado y activo en el sistema
    * El usuario debe estar autenticado previamente en el sistema

* Flujo Normal
    * (1) El sistema muestra un listado ordenado por proximidad de ubicación con el listado de productos que el resto de usuarios de la plataforma han compartido
    * (2) El sistema además presenta una serie de opciones de búsqueda para filtrar la información de forma más precisa BorrowPlaceSearch [link](13_BorrowPlaceSearch.md)

* Flujo Alternativo
    * (2A) El sistema no puede recuperar el listado de productos y muestra el error

* Poscondiciones: 
    * El usuario puede ver los productos del catalogo

