-- Obtiene los regimenes activos de un hotel
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_REGIMENES_ACTIVOS_DE_HOTEL] (@id_hotel INT)
AS
BEGIN
    SELECT r.id_regimen, descripcion_regimen, precio_base_regimen, estado_regimen
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes] r
	JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[hotelesXregimenes] hXr
	ON r.id_regimen = hXr.id_regimen
	WHERE estado_regimen = 1
	AND id_hotel = @id_hotel
END

GO

-- Este procedure se encarga de actualizar las reservas vencidas por no show (se ejecuta cuando se intenta ingresar una reserva)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[CANCELAR_RESERVAS_VENCIDAS] (@today DATETIME)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
		
		-- Primero updateo las reservas que estén "activas"
		-- Si ya pasó el día inicial, la seteamos en 5
		UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
		SET id_estado_reserva = 5
		FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
		LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e ON e.id_reserva = r.id_reserva
		WHERE id_estadia IS NULL
		AND @today > fecha_inicio_reserva
		AND id_estado_reserva IN (1, 2)

		-- Finalmente para las desconocidas
		-- (generadas por la migración)
		UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
		SET id_estado_reserva = 8
		FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
		LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e
		ON e.id_reserva = r.id_reserva
		WHERE id_estadia IS NULL
		AND @today > fecha_inicio_reserva
		AND id_estado_reserva = 7

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		RAISERROR('No se pudo hacer update de reservas vencidas', 18, 1) WITH LOG
	END CATCH
END

GO

-- Este procedure obtiene las habitaciones disponibles para cierta reserva
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_HABITACIONES_DISPONIBLES_RESERVA]
    (@id_rol INT, @today DATETIME, @fecha_inicio DATETIME, @fecha_fin DATETIME, @id_hotel INT, @id_reserva INT)
AS
BEGIN

    -- Validación de seguridad
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol, 1

    -- Actualizo las reservas vencidas para que estén disponibles
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[CANCELAR_RESERVAS_VENCIDAS] @today

        -- Si contiene al menos una ROW, el hotel estará cerrado
    IF (EXISTS (SELECT fecha_inicio_cierre_temporal_hotel, fecha_fin_cierre_temporal_hotel
                FROM EL_MONSTRUO_DEL_LAGO_MASER.cierres_temporales_hotel
                WHERE @fecha_inicio <= fecha_fin_cierre_temporal_hotel
                AND @fecha_fin >= fecha_inicio_cierre_temporal_hotel
                AND id_hotel = @id_hotel)) BEGIN
        SELECT -1 id_habitacion
                RETURN
    END

        -- Todas las habitaciones disponibles
                SELECT * FROM EL_MONSTRUO_DEL_LAGO_MASER.habitaciones
                WHERE id_hotel = @id_hotel
        EXCEPT -- Excepto las que estén reservadas
                SELECT h.* FROM EL_MONSTRUO_DEL_LAGO_MASER.habitaciones h
                JOIN EL_MONSTRUO_DEL_LAGO_MASER.reservasXhabitaciones rXh ON h.id_habitacion = rXh.id_habitacion
                JOIN EL_MONSTRUO_DEL_LAGO_MASER.reservas r ON rXh.id_reserva = r.id_reserva
                WHERE id_hotel = @id_hotel
                AND rXh.id_reserva <> @id_reserva
                AND @fecha_inicio < fecha_fin_reserva
                AND @fecha_fin > fecha_inicio_reserva
                AND id_estado_reserva IN (1, 2, 6, 7)
        EXCEPT -- Y las que estén cerradas temporalmente
                SELECT h.* FROM EL_MONSTRUO_DEL_LAGO_MASER.habitaciones h
                JOIN EL_MONSTRUO_DEL_LAGO_MASER.cierres_temporales_habitacion c ON h.id_habitacion = c.id_habitacion
                WHERE @fecha_inicio <= fecha_fin_cierre_temporal_habitacion
                AND @fecha_fin >= fecha_inicio_cierre_temporal_habitacion
                AND id_hotel = @id_hotel
END

GO

-- Obtengo el ID del regimen all inclusive
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_REGIMEN_ALL_INCLUSIVE]
AS
BEGIN
	SELECT id_regimen
	FROM [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes]
	WHERE descripcion_regimen = 'All inclusive'
END

GO

-- Esta vista devuelve los clientes totales (los migrados + los que tienen errores)
CREATE VIEW [EL_MONSTRUO_DEL_LAGO_MASER].[clientes_completos]
AS
	SELECT * FROM [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
        WHERE id_cliente != 0
	UNION
	SELECT DISTINCT
		-1 id_cliente,
		nombre_cliente, 
		apellido_cliente, 
		6 id_tipo_documento, 
		numero_documento_cliente, 
		correo_cliente, 
		null telefono_cliente, 
		domicilio_calle_cliente, 
		domicilio_numero_cliente, 
		domicilio_piso_cliente, 
		domicilio_departamento_cliente, 
		null ciudad_cliente, 
		null id_pais, 
		nacionalidad_cliente, 
		fecha_nacimiento_cliente,
		0 estado_cliente
	FROM [EL_MONSTRUO_DEL_LAGO_MASER].[migracion_errores]
GO

-- Este procedure valida que no haya un cliente con dado documento
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_DOCUMENTO_UNICO_CLIENTE]
    (@id_tipo_documento INT, @numero_documento_cliente NUMERIC(18,0))
AS
BEGIN
    IF (EXISTS (SELECT * FROM [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
                WHERE id_tipo_documento = @id_tipo_documento
                AND numero_documento_cliente = @numero_documento_cliente))
    BEGIN
	    SELECT 0
    END ELSE BEGIN
	    SELECT 1
    END
END

GO

-- Este procedure valida si hay un cliente con ese correo
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_CORREO_UNICO_CLIENTE]
    (@correo NVARCHAR(255))
AS
BEGIN
    IF (EXISTS (SELECT * FROM [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
                WHERE correo_cliente = @correo))
    BEGIN
	    SELECT 0 
    END ELSE BEGIN
	    SELECT 1
    END
END

GO

CREATE TYPE [EL_MONSTRUO_DEL_LAGO_MASER].[listaDeHabitaciones] AS TABLE
(id_habitacion INT)

GO

-- Este procedimiento hace el insert de la reserva al sistema
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_RESERVA]
    (@id_rol_user INT, @fecha_realizacion DATETIME, @fecha_inicio DATETIME, @fecha_fin DATETIME,
    @id_cliente INT, @id_regimen INT, @habitaciones EL_MONSTRUO_DEL_LAGO_MASER.listaDeHabitaciones READONLY,
    @id_usuario INT)
AS
BEGIN
    DECLARE @id_reserva     INT
    DECLARE @id_habitacion  INT

    DECLARE cursor_habitaciones CURSOR FOR
        SELECT id_habitacion
        FROM @habitaciones

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 1

    IF (EXISTS(SELECT * FROM [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
               WHERE id_cliente = @id_cliente
               AND estado_cliente = 0)) BEGIN
        RAISERROR('El cliente está deshabilitado. No puede realizar reservas', 20, 1) WITH LOG
    END

    BEGIN TRY
        BEGIN TRANSACTION
            -- Primero, insertamos la nueva reserva
            INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
                (fecha_realizacion_reserva, fecha_inicio_reserva, fecha_fin_reserva, id_cliente, id_regimen, id_estado_reserva)
            VALUES (@fecha_realizacion, @fecha_inicio, @fecha_fin, @id_cliente, @id_regimen, 1)

            -- Seteamos el ID reserva
            SET @id_reserva = SCOPE_IDENTITY()

            -- Ahora insertamos la transaccion en el log de generacion y modificacion
            INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[generacion_modificacion_reservas]
                (id_reserva, id_usuario, tipo_generacion_modificacion_reserva, fecha_generacion_modificacion_reserva)
            VALUES (@id_reserva, @id_usuario, 'G', @fecha_realizacion)

            -- Finalmente, insertamos en clientes por habitaciones haciendo un loop del cursor
            OPEN cursor_habitaciones
            FETCH cursor_habitaciones INTO @id_habitacion

            WHILE (@@FETCH_STATUS = 0) BEGIN
                INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones]
                    (id_reserva, id_habitacion)
                VALUES (@id_reserva, @id_habitacion)

                FETCH cursor_habitaciones INTO @id_habitacion
            END
        COMMIT TRANSACTION

        -- Hago un select para poder tomarlo desde la aplicación
        SELECT @id_reserva

        CLOSE cursor_habitaciones
        DEALLOCATE cursor_habitaciones
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION

        IF CURSOR_STATUS('global', 'cursor_habitaciones') = 1 BEGIN
            CLOSE cursor_habitaciones
        END
        DEALLOCATE cursor_habitaciones;

        THROW
    END CATCH
END

GO

-- Este procedure obtiene información de una reserva
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_RESERVA]
    (@id_reserva INT, @today DATETIME)
AS
BEGIN
    SELECT fecha_realizacion_reserva, fecha_inicio_reserva, fecha_fin_reserva, id_regimen
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
    LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e
        ON r.id_reserva = e.id_reserva
    WHERE r.id_reserva = @id_reserva
    AND id_estado_reserva IN (1, 2, 7)
    AND fecha_inicio_reserva > @today 
    AND id_estadia IS NULL
END

GO

-- Este procedure obtiene los tipos de habitación de una reserva
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_TIPOS_HABITACION_RESERVA]
    (@id_reserva INT)
AS
BEGIN
    SELECT h.id_tipo_habitacion, descripcion_tipo_habitacion, porcentual_tipo_habitacion, cantidad_huespedes_tipo_habitacion 
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] rxh
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] h
        ON rxh.id_habitacion = h.id_habitacion
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[tipos_habitacion] th
        ON h.id_tipo_habitacion = th.id_tipo_habitacion
    WHERE id_reserva = @id_reserva
END

GO

-- Obtengo el ID de hotel relacionado de una reserva
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_HOTEL_DE_RESERVA]
    (@id_reserva INT)
AS
BEGIN
    SELECT id_hotel FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
    LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] rXh
        ON r.id_reserva = rXh.id_reserva
    LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] h
        ON rXh.id_habitacion = h.id_habitacion
    WHERE r.id_reserva = @id_reserva
END

GO

-- Modifica una reserva con datos nuevos
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[MODIFICAR_RESERVA]
    (@id_usuario INT, @id_rol_user INT, @fecha_inicio DATETIME, @fecha_fin DATETIME, @id_regimen INT,
    @id_reserva INT, @fecha_hoy DATETIME, @habitaciones EL_MONSTRUO_DEL_LAGO_MASER.listaDeHabitaciones READONLY)
AS
BEGIN
    DECLARE @id_habitacion  INT
    DECLARE cursor_habitaciones CURSOR FOR
        SELECT id_habitacion
        FROM @habitaciones

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 2

    BEGIN TRY
        BEGIN TRANSACTION
        UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
        SET fecha_inicio_reserva = @fecha_inicio, fecha_fin_reserva = @fecha_fin, 
            id_estado_reserva = 2, id_regimen = @id_regimen
        WHERE id_reserva = @id_reserva

        DELETE FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones]
        WHERE id_reserva = @id_reserva 

        OPEN cursor_habitaciones
        FETCH cursor_habitaciones INTO @id_habitacion
        WHILE (@@FETCH_STATUS = 0) BEGIN
            INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones]
                (id_reserva, id_habitacion)
            VALUES (@id_reserva, @id_habitacion)

            FETCH cursor_habitaciones INTO @id_habitacion
        END

        CLOSE cursor_habitaciones
        DEALLOCATE cursor_habitaciones

        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[generacion_modificacion_reservas]
            (fecha_generacion_modificacion_reserva, tipo_generacion_modificacion_reserva, id_usuario, id_reserva)
        VALUES (@fecha_hoy, 'M', @id_usuario, @id_reserva)

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION

        IF CURSOR_STATUS('global', 'cursor_habitaciones') = 1 BEGIN
            CLOSE cursor_habitaciones
        END
        DEALLOCATE cursor_habitaciones;

        THROW
    END CATCH
END

GO

