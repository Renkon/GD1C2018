CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[TOP5_HOTELES_RESERVAS_CANCELADAS]
    (@inicio DATETIME, @fin DATETIME)
AS
BEGIN
    SELECT TOP 5 ho.id_hotel, nombre_hotel, COUNT(cr.id_cancelacion_reserva) cancelaciones
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles] ho
    LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] h
        ON ho.id_hotel = h.id_hotel
    LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] rxh
        ON h.id_habitacion = rxh.id_habitacion
    LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[cancelaciones_reserva] cr
        ON rxh.id_reserva = cr.id_reserva
    WHERE ( fecha_cancelacion_reserva >= @inicio 
            AND fecha_cancelacion_reserva <= @fin )
    OR fecha_cancelacion_reserva IS NULL
    GROUP BY ho.id_hotel, ho.nombre_hotel
    ORDER BY COUNT(cr.id_cancelacion_reserva) DESC, nombre_hotel
END

GO

--

CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[TOP5_HOTELES_MAYOR_CANTIDAD_CONSUMOS]
    (@inicio DATETIME, @fin DATETIME)
AS
BEGIN
    SELECT id_hotel, nombre_hotel, SUM(cantidad_consumibles_total) cantidad_consumibles_total
    FROM (
                SELECT hot.id_hotel, nombre_hotel, ISNULL(cantidad_consumo, 0) cantidad_consumibles_total
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[consumos] c 
                RIGHT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] h
                    ON (h.id_habitacion = c.id_habitacion) 
                RIGHT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles] hot
                    ON (h.id_hotel = hot.id_hotel)
                WHERE ( fecha_consumo >= @inicio 
                        AND fecha_consumo <= @fin )
                OR (fecha_consumo IS NULL AND cantidad_consumo IS NULL)
            UNION ALL
                SELECT hot.id_hotel, nombre_hotel, 0 cantidad_consumibles_total
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[consumos] c 
                RIGHT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] h
                    ON (h.id_habitacion = c.id_habitacion) 
                RIGHT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles] hot
                    ON (h.id_hotel = hot.id_hotel)
                WHERE ( fecha_consumo >= @inicio 
                        AND fecha_consumo <= @fin )
                OR (fecha_consumo IS NULL AND cantidad_consumo IS NOT NULL)
    ) data
    GROUP BY id_hotel, nombre_hotel
    ORDER BY SUM(cantidad_consumibles_total) DESC, nombre_hotel
END

GO

--


