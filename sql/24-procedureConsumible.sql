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


