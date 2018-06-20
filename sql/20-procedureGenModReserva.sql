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
    (@id_rol INT, @today DATETIME, @fecha_inicio DATETIME, @fecha_fin DATETIME, @id_hotel INT)
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
		AND @fecha_inicio <= fecha_fin_reserva
		AND @fecha_fin >= fecha_inicio_reserva
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
		Cliente_Nombre nombre_cliente, 
		Cliente_Apellido apellido_cliente, 
		6 id_tipo_documento, 
		Cliente_Pasaporte_Nro numero_documento_cliente, 
		Cliente_Mail correo_cliente, 
		null telefono_cliente, 
		Cliente_Dom_Calle domicilio_calle_cliente, 
		Cliente_Nro_Calle domicilio_numero_cliente, 
		Cliente_Piso domicilio_piso_cliente, 
		Cliente_Depto domicilio_departamento_cliente, 
		null ciudad_cliente, 
		null id_pais, 
		Cliente_Nacionalidad nacionalidad_cliente, 
		Cliente_Fecha_Nac fecha_nacimiento_cliente,
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

 
