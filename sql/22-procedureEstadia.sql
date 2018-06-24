-- Este procedure obtiene las reservas que se puede ingresar/cerrar una estadia
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_RESERVA_APTA_ESTADIA]
    (@id_rol_user INT, @id_usuario INT, @id_reserva INT, @today DATETIME)
AS
BEGIN

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[CANCELAR_RESERVAS_VENCIDAS] @today

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 22

    SELECT DISTINCT fecha_realizacion_reserva, fecha_inicio_reserva, fecha_fin_reserva, id_regimen, id_estado_reserva
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
    LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e
        ON r.id_reserva = e.id_reserva
    LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] rh
        ON r.id_reserva = rh.id_reserva
    LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] h
        ON rh.id_habitacion = h.id_habitacion
    WHERE r.id_reserva = @id_reserva
    AND (
        (id_estado_reserva IN (1, 2, 7) AND fecha_inicio_reserva = @today AND id_estadia IS NULL)
        OR
        (id_estado_reserva = 6 AND id_estadia IS NOT NULL AND fecha_egreso_estadia IS NULL)
    )
    AND id_hotel IN (SELECT id_hotel 
                     FROM [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles]
                     WHERE id_usuario = @id_usuario)
END

GO

-- Obtiene las habitaciones de una reserva
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_HABITACIONES_DE_RESERVA]
    (@id_reserva INT)
AS
BEGIN
    SELECT h.id_habitacion, numero_habitacion, piso_habitacion, 
        ubicacion_habitacion, id_tipo_habitacion, descripcion_habitacion
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] h
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] rXh
        ON h.id_habitacion = rXh.id_habitacion
    WHERE rXh.id_reserva = @id_reserva
END

GO

-- Este procedure obtiene la estadìa relacionada a la reserva
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_ESTADIA_DE_RESERVA]
    (@id_reserva INT)
AS
BEGIN 
    SELECT id_estadia, fecha_ingreso_estadia, fecha_egreso_estadia 
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
    WHERE id_reserva = @id_reserva
END

GO

-- Este procedure devuelve un cliente relacionado a la reserva
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_CLIENTE_DE_RESERVA]
    (@id_reserva INT)
AS
BEGIN 
    SELECT r.id_cliente, nombre_cliente, apellido_cliente, id_tipo_documento,
        numero_documento_cliente, correo_cliente
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[clientes] c
        ON r.id_cliente = c.id_cliente
    WHERE id_reserva = @id_reserva
END

GO

-- Este tipo se usa para el procedure que inserta los clientes x estadias
CREATE TYPE [EL_MONSTRUO_DEL_LAGO_MASER].[listaDeClientes] AS TABLE
    (id_cliente INT)

GO

-- Este procedure hace el ingreso de una estadia
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INGRESAR_ESTADIA]
    (@id_rol_user INT, @id_reserva INT, @id_usuario_ingreso INT, @fecha_ingreso_estadia DATETIME,
    @clientes EL_MONSTRUO_DEL_LAGO_MASER.listaDeClientes READONLY )
AS
BEGIN

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 22

    DECLARE @id_cliente INT
    DECLARE @id_estadia INT

    DECLARE cursor_clientes CURSOR FOR
        SELECT id_cliente
        FROM @clientes

    BEGIN TRY
        BEGIN TRANSACTION

        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
            (id_reserva, id_usuario_ingreso, fecha_ingreso_estadia)
        VALUES(@id_reserva, @id_usuario_ingreso, @fecha_ingreso_estadia)

        SET @id_estadia = SCOPE_IDENTITY()

        OPEN cursor_clientes
        FETCH cursor_clientes INTO @id_cliente
        WHILE (@@FETCH_STATUS = 0) BEGIN
            INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[clientesXestadias]
                (id_cliente, id_estadia)
            VALUES(@id_cliente, @id_estadia)

            FETCH cursor_clientes INTO @id_cliente
        END

        UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
        SET id_estado_reserva = 6
        WHERE id_reserva = @id_reserva

        COMMIT TRANSACTION

        SELECT @id_estadia

        CLOSE cursor_clientes
        DEALLOCATE cursor_clientes
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION

        IF CURSOR_STATUS('global', 'cursor_clientes') = 1 BEGIN
            CLOSE cursor_clientes
        END
        DEALLOCATE cursor_clientes;

        THROW
    END CATCH
END

GO

-- Este procedure hace el egreso de la estadia
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[EGRESAR_ESTADIA]
    (@id_rol_user INT, @id_estadia  INT, @id_usuario_egreso INT, @fecha_egreso_estadia DATETIME)
AS
BEGIN

      EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 22

      UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
      SET fecha_egreso_estadia = @fecha_egreso_estadia, id_usuario_egreso = @id_usuario_egreso
      WHERE id_estadia = @id_estadia

END

GO

-- Este procedure obtiene los clientes de una estadia
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_CLIENTES_ESTADIA]
    (@id_estadia INT)
AS
BEGIN
    SELECT c.id_cliente, nombre_cliente, apellido_cliente,
        id_tipo_documento, numero_documento_cliente, correo_cliente
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[clientesXestadias] cxe 
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[clientes] c 
        ON c.id_cliente = cxe.id_cliente
    WHERE cxe.id_estadia = @id_estadia 
END

GO

