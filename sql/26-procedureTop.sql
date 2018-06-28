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

GO

--

CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[TOP5_HOTELES_DIAS_CERRADO]
    (@inicio DATETIME, @fin DATETIME)
AS
BEGIN
    SELECT TOP 5 id_hotel, nombre_hotel, SUM(dias_cerrado) dias_cerrado
    FROM (
        -- Generamos todos los hoteles primero
            SELECT id_hotel, nombre_hotel, 0 dias_cerrado
            FROM [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles]
        UNION ALL
        -- Obtenemos la suma de dias reservados
            SELECT h.id_hotel, nombre_hotel, DATEDIFF(day, EL_MONSTRUO_DEL_LAGO_MASER.MAXDATE(@inicio, fecha_inicio_cierre_temporal_hotel),
                EL_MONSTRUO_DEL_LAGO_MASER.MINDATE(@fin, fecha_fin_cierre_temporal_hotel)) + 1 dias_cerrado
            FROM [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles] h
            JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[cierres_temporales_hotel] c
                ON h.id_hotel = c.id_hotel
            WHERE ((@inicio <= fecha_fin_cierre_temporal_hotel)
            AND (fecha_inicio_cierre_temporal_hotel <= @fin))
        ) data
    GROUP BY id_hotel, nombre_hotel
    ORDER BY dias_cerrado DESC, nombre_hotel
END

GO

--

CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[TOP5_HABITACIONES_MAYOR_CANTIDAD_DIAS_ESTADIAS]
    (@inicio DATETIME, @fin DATETIME, @fechaHoy DATETIME)
AS
BEGIN
    SELECT TOP 5 id_habitacion, id_hotel, nombre_hotel, numero_habitacion, 
        SUM(cantidad_dias) cantidad_dias, SUM(cantidad_estadias) cantidad_estadias
    FROM (
                -- Generamos todas las habitaciones
                SELECT DISTINCT ha.id_habitacion, ha.id_hotel, nombre_hotel, numero_habitacion, 0 cantidad_dias, 0 cantidad_estadias
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] ha
                INNER JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles] ho
                    ON ha.id_hotel = ho.id_hotel
                LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] rh
                    ON ha.id_habitacion = rh.id_habitacion
                LEFT JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e
                    ON rh.id_reserva = e.id_reserva
            UNION ALL
                -- Ahora los que tienen finalizada la estadía (no importa el día de hoy)
                SELECT ha.id_habitacion, ha.id_hotel, nombre_hotel, numero_habitacion, 
                    ISNULL(SUM(DATEDIFF(day, EL_MONSTRUO_DEL_LAGO_MASER.MAXDATE(@inicio, fecha_ingreso_estadia),
                    EL_MONSTRUO_DEL_LAGO_MASER.MINDATE(@fin, fecha_egreso_estadia))), 0) cantidad_dias, COUNT(DISTINCT id_estadia) cantidad_estadias
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] ha
                INNER JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles] ho
                    ON ha.id_hotel = ho.id_hotel
                INNER JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] rh
                    ON ha.id_habitacion = rh.id_habitacion
                INNER JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e
                    ON rh.id_reserva = e.id_reserva
                WHERE fecha_egreso_estadia IS NOT NULL
                AND ((@inicio <= fecha_egreso_estadia)
                AND (fecha_ingreso_estadia <= @fin))
                GROUP BY ha.id_habitacion, ha.id_hotel, nombre_hotel, numero_habitacion
            UNION ALL
                -- Ahora los que están en curso (fecha de fin sería la fecha de hoy)
                SELECT ha.id_habitacion, ha.id_hotel, nombre_hotel, numero_habitacion, 
                    ISNULL(SUM(DATEDIFF(day, EL_MONSTRUO_DEL_LAGO_MASER.MAXDATE(@inicio, fecha_ingreso_estadia),
                    EL_MONSTRUO_DEL_LAGO_MASER.MINDATE(@fin, @fechaHoy))), 0) cantidad_dias, COUNT(DISTINCT id_estadia) cantidad_estadias
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] ha
                INNER JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles] ho
                    ON ha.id_hotel = ho.id_hotel
                INNER JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] rh
                    ON ha.id_habitacion = rh.id_habitacion
                INNER JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e
                    ON rh.id_reserva = e.id_reserva
                WHERE fecha_egreso_estadia IS NULL
                AND ((@inicio <= @fechaHoy)
                AND (fecha_ingreso_estadia <= @fin))
                GROUP BY ha.id_habitacion, ha.id_hotel, nombre_hotel, numero_habitacion
        ) data
    GROUP BY id_habitacion, id_hotel, nombre_hotel, numero_habitacion
    ORDER BY cantidad_dias DESC, nombre_hotel
END

GO

--

CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[TOP5_CLIENTES_PUNTOS]
    (@inicio DATETIME, @fin DATETIME, @fechaHoy DATETIME)
AS
BEGIN
    SELECT TOP 5 nombre_cliente, apellido_cliente, correo_cliente, SUM(puntos) puntos
    FROM (
            ---- Generamos los clientes
                SELECT id_cliente, nombre_cliente, apellido_cliente, correo_cliente, 0 puntos
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
            -- Ahora unimos los consumos de los clientes
            UNION ALL
                SELECT cl.id_cliente, nombre_cliente, apellido_cliente, correo_cliente, (precio_consumible * cantidad_consumo)/10 puntos
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[consumos] c
                JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[consumibles] co
                    ON c.id_consumible = co.id_consumible
                JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e
                    ON c.id_estadia = e.id_estadia
                JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
                    ON e.id_reserva = r.id_reserva
                JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[clientes] cl
                    ON r.id_cliente = cl.id_cliente
                WHERE fecha_consumo >= @inicio
                AND fecha_consumo <= @fin
            -- Ahora unimos las estadías COMPLETAS
            UNION ALL
                SELECT c.id_cliente, nombre_cliente, apellido_cliente, correo_cliente, 
                    (DATEDIFF(day,  EL_MONSTRUO_DEL_LAGO_MASER.MAXDATE(fecha_ingreso_estadia, @inicio), 
                                    EL_MONSTRUO_DEL_LAGO_MASER.MINDATE(fecha_egreso_estadia, @fin)) * 
                    EL_MONSTRUO_DEL_LAGO_MASER.OBTENER_COSTO_DIARIO_ESTADIA(id_estadia))/20 puntos
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e
                JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
                    ON e.id_reserva = r.id_reserva
                JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[clientes] c
                    ON r.id_cliente = c.id_cliente
                WHERE ((@inicio <= fecha_egreso_estadia)
                    AND (fecha_ingreso_estadia <= @fin))
            -- Y finalmente las estadías pendientes
            UNION ALL
                SELECT c.id_cliente, nombre_cliente, apellido_cliente, correo_cliente,
                    (DATEDIFF(day,  EL_MONSTRUO_DEL_LAGO_MASER.MAXDATE(fecha_ingreso_estadia, @inicio), 
                                    EL_MONSTRUO_DEL_LAGO_MASER.MINDATE(@fechaHoy, @fin)) * 
                    EL_MONSTRUO_DEL_LAGO_MASER.OBTENER_COSTO_DIARIO_ESTADIA(id_estadia))/20 puntos
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e
                JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
                    ON e.id_reserva = r.id_reserva
                JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[clientes] c
                    ON r.id_cliente = c.id_cliente
                WHERE fecha_egreso_estadia IS NULL
                AND ((@inicio <= @fechaHoy)
                AND (fecha_ingreso_estadia <= @fin))
                AND @fechaHoy >= fecha_ingreso_estadia
        ) data
    WHERE id_cliente <> 0
    GROUP BY nombre_cliente, apellido_cliente, correo_cliente
    ORDER BY puntos DESC, nombre_cliente, apellido_cliente
END

GO

