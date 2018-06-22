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


