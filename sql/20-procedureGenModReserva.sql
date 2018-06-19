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

