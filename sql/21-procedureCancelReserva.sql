-- Cancela una reserva existente
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[CANCELAR_RESERVA]
    (@id_rol_user INT, @id_reserva INT, @motivo_cancelacion_reserva NVARCHAR(2500),
    @fecha_cancelacion_reserva DATETIME, @id_usuario INT)
AS
BEGIN
    DECLARE @id_estado_reserva INT
    DECLARE @id_usuario_dummy INT

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 3

    BEGIN TRY
        BEGIN TRANSACTION

        -- Con este bloque obtenemos el id del usuario dummy
        CREATE TABLE #dummyTable (id_usuario_dummy INT)

        insert into #dummyTable
        EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_USUARIO_DUMMY]

        SELECT @id_usuario_dummy = id_usuario_dummy 
        FROM #dummyTable

        DROP TABLE #dummyTable
        -- Ya obtuvimos el id del usuario dummy

        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[cancelaciones_reserva]
            (id_reserva, motivo_cancelacion_reserva,fecha_cancelacion_reserva,id_usuario)
        VALUES (@id_reserva, @motivo_cancelacion_reserva, @fecha_cancelacion_reserva, @id_usuario)

        IF (@id_usuario = @id_usuario_dummy) BEGIN
            SET @id_estado_reserva = 4
        END ELSE BEGIN
            SET @id_estado_reserva = 3
        END

        UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
        SET id_estado_reserva = @id_estado_reserva
        WHERE id_reserva = @id_reserva

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        THROW
    END CATCH
END

GO

