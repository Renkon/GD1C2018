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
    SELECT TOP 5 id_hotel, nombre_hotel, SUM(cantidad_consumibles_total) cantidad_consumibles_total
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

-- Esta funcion devuelve la fecha mayor entre dos fechas
CREATE FUNCTION [EL_MONSTRUO_DEL_LAGO_MASER].[MAXDATE] (
    @date1        DATETIME,
    @date2        DATETIME
) RETURNS DATETIME AS
BEGIN
    DECLARE @date DATETIME;

    IF (@date1 IS NULL) OR (@date2 IS NULL)
        SET @date = NULL;

    IF (@date1 > @date2)
        SET @date = @date1;
    ELSE
        SET @date = @date2;

    RETURN @date;
END

GO

-- Esta funcion devuelve la fecha menor entre dos fechas
CREATE FUNCTION [EL_MONSTRUO_DEL_LAGO_MASER].[MINDATE] (
    @date1        DATETIME,
    @date2        DATETIME
) RETURNS DATETIME AS
BEGIN
    DECLARE @date DATETIME;

    IF (@date1 IS NULL) OR (@date2 IS NULL)
        SET @date = NULL;

    IF (@date1 < @date2)
        SET @date = @date1;
    ELSE
        SET @date = @date2;

    RETURN @date;
END

--
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[TOP5_HOTELES_DIAS_CERRADO]
    (@inicio DATETIME, @fin DATETIME)
AS
BEGIN
    SELECT TOP 5 h.id_hotel, nombre_hotel, 
        ISNULL(SUM(DATEDIFF(day, EL_MONSTRUO_DEL_LAGO_MASER.MAXDATE(@inicio, fecha_inicio_cierre_temporal_hotel), 
        EL_MONSTRUO_DEL_LAGO_MASER.MINDATE(@fin, fecha_fin_cierre_temporal_hotel)) + 1), 0) dias_cerrado
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles] h
    LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[cierres_temporales_hotel] c
        ON h.id_hotel = c.id_hotel
    WHERE ((@inicio <= fecha_fin_cierre_temporal_hotel)
        AND (fecha_inicio_cierre_temporal_hotel <= @fin))
    OR id_cierre_temporal_hotel IS NULL
    GROUP BY h.id_hotel, nombre_hotel
    ORDER BY ISNULL(SUM(DATEDIFF(day, EL_MONSTRUO_DEL_LAGO_MASER.MAXDATE(@inicio, fecha_inicio_cierre_temporal_hotel), 
        EL_MONSTRUO_DEL_LAGO_MASER.MINDATE(@fin, fecha_fin_cierre_temporal_hotel)) + 1), 0) DESC, nombre_hotel
END

GO

