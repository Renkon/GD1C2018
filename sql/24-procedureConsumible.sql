-- Este procedure obtiene la habitaaciones de una estadia (sirve paraa consumibles de las habitaciones)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_HABITACIONES_DE_ESTADIA]
    (@id_estadia INT)
AS
BEGIN
    DECLARE @id_reserva INT
    
    SELECT @id_reserva = id_reserva
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
    WHERE id_estadia = @id_estadia

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_HABITACIONES_DE_RESERVA] @id_reserva
END

GO

-- Este procedure obtiene las propiedades de una estadia
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_ESTADIA]
    (@id_rol_user INT, @id_estadia INT)
AS
BEGIN

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 23

    SELECT fecha_ingreso_estadia, fecha_egreso_estadia, consumos_cerrados
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
    WHERE id_estadia = @id_estadia
END

GO

-- Este procedure obtiene todos los consumibles por un filtro
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_CONSUMIBLES_FILTRADOS]
    (@descripcion NVARCHAR(255))
AS
BEGIN
    SELECT id_consumible, precio_consumible, descripcion_consumible
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[consumibles]
    WHERE LOWER(descripcion_consumible) LIKE '%' + LOWER(@descripcion) + '%'
END

GO

-- Este procedure ingresa un consumo y devuelve su ID
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[AGREGAR_CONSUMO]
    (@id_rol_user INT, @id_consumible INT, @id_estadia INT,
    @id_habitacion INT, @fecha_consumo DATETIME, @cantidad_consumo INT)
AS
BEGIN
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 23

    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[consumos]
        (id_consumible, id_estadia,id_habitacion, fecha_consumo, cantidad_consumo)
    VALUES(@id_consumible, @id_estadia, @id_habitacion, @fecha_consumo, @cantidad_consumo)

    SELECT SCOPE_IDENTITY() id_consumo
END

GO

-- Este procedure obtiene la información de un consumo para listarla
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_CONSUMOS_DE_ESTADIA]
    (@id_estadia INT)
AS
BEGIN
    SELECT id_consumo, fecha_consumo, h.id_habitacion, numero_habitacion, 
        c.id_consumible, descripcion_consumible, precio_consumible, cantidad_consumo
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[consumos] c 
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] h 
        ON (c.id_habitacion = h.id_habitacion) 
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[consumibles] co 
        ON (co.id_consumible = c.id_consumible)
    WHERE id_estadia = @id_estadia
END

GO

-- Este procedure se utiliza para actualizar un consumo
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[MODIFICAR_CONSUMO]
    (@id_rol_user INT, @id_consumo INT, @id_consumible INT, @id_habitacion INT, 
    @fecha_consumo DATETIME, @cantidad_consumo INT)
AS
BEGIN
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 23

    UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[consumos]
    SET id_consumible = @id_consumible, id_habitacion = @id_habitacion, 
        fecha_consumo = @fecha_consumo, cantidad_consumo = @cantidad_consumo
    WHERE id_consumo = @id_consumo 
END

GO

-- Este procedure elimina un consumo
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[BORRAR_CONSUMO]
    (@id_rol_user INT, @id_consumo INT)
AS
BEGIN
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 23

    DELETE FROM [EL_MONSTRUO_DEL_LAGO_MASER].[consumos]
    WHERE id_consumo = @id_consumo
END

GO

-- Este procedure cierra los consumos de una estadìa (ya no se va a poder ingresar mas consumos ni modificarlos)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[CERRAR_CONSUMO_ESTADIA](@id_rol_user INT, @id_estadia INT)
AS
BEGIN
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 23

    UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
    SET consumos_cerrados = 1
    WHERE id_estadia = @id_estadia
END

GO

