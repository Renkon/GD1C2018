-- Este procedure se usa para ver que reservas estan en un periodo (sirve para no cerrar un hotel si tiene reservas en un periodo)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_CANTIDAD_RESERVAS_EN_PERIODO_HOTEL]
    (@fecha_inicio DATETIME, @fecha_fin DATETIME, @id_hotel INT)
AS
BEGIN
    SELECT COUNT(r.id_reserva)
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r 
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] rxh 
        ON (r.id_reserva = rxh.id_reserva) 
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] h 
        ON (h.id_habitacion = rxh.id_habitacion)
    WHERE id_hotel = @id_hotel 
    AND @fecha_inicio <= fecha_fin_reserva
    AND fecha_inicio_reserva <= @fecha_fin
    AND id_estado_reserva IN (1, 2, 6, 7)
END

GO

-- Este procedure revisa si hay una habitacion ocupada en cierto periodo (usado para cierre temporal de hoteles)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[HABITACION_RESERVADA_EN_INTERVALO]
    (@fecha_inicio DATETIME, @fecha_fin DATETIME, @id_habitacion INT)
AS
BEGIN
    SELECT COUNT(r.id_reserva)
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] rXh
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r 
        ON (r.id_reserva = rXh.id_reserva)
    WHERE id_habitacion = @id_habitacion 
    AND @fecha_inicio <= fecha_fin_reserva
    AND fecha_inicio_reserva <= @fecha_fin
    AND id_estado_reserva IN (1, 2, 6, 7)
END

GO

-- Este procedure obtiene los regimenes de las reservas de un hotel, para validar que no 
-- estemos borrando un regimen siendo usaado
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[REGIMENES_USADOS_POR_RESERVAS_DE_HOTEL](@fecha_hoy DATETIME, @id_hotel INT)
AS
BEGIN
    SELECT DISTINCT r.id_regimen, descripcion_regimen, precio_base_regimen, estado_regimen
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] rh
        ON r.id_reserva = rh.id_reserva
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] h
        ON rh.id_habitacion = h.id_habitacion
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes] rr
        ON r.id_regimen = rr.id_regimen
    WHERE fecha_fin_reserva >= @fecha_hoy 
    AND id_hotel = @id_hotel 
    AND id_estado_reserva IN (1, 2, 6, 7)
END

GO

