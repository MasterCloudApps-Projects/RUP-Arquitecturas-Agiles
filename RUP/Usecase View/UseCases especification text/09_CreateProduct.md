### CreateProduct

* Nombre: CreateProduct
* Autor: Carlos Corcobado
* Fecha: 10/10/2020
* Descripción: Permite al usuario detallar las características del producto así como publicarlo en el BorrowPlace
* Actores: Usuario
* Precondiciones: 
    * El usuario debe estar registrado y activo en el sistema
    * El usuario debe estar autenticado previamente en el sistema
    
* Flujo Normal
    * (1) El sistema muestra registro de producto
    * (2) El usuario rellena la Descripción del producto
    * (3) El usuario rellena la Familia de producto
    * (4) El usuario rellena la Subfamilia de producto
    * (5) El usuario rellena el Periodo de disponibilidad
    * (6) El usuario rellena las observaciones
    * (7) El usuario rellena el listado de fotos
    * (8) El sistema registra el producto 
    * (9) El sistema muestra el BorrowPlace AccessBorrowPlace [link](12_AccessBorrowPlace.md)
    
* Flujo Alternativo
    * (8A) El sistema verifica la validez de la petición, si los datos no son correctos, se avisa con un mensaje al usuario del problema
    * (8B) El usuario cancela la operación

* Poscondiciones: 
    * El número de productos asociados al usuario es n + 1
    * El producto es visible en el BorrowPlace
