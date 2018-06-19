-- Procedimiento que crea los tipos de habitación con IDENTITY (respetando los valores recibidos)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[CREAR_TIPOS_HABITACION] AS
BEGIN
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[tipos_habitacion]
    SELECT descripcion, porcentual, cantidad
    FROM (
            SELECT DISTINCT 
                    Habitacion_Tipo_Codigo codigo,
                    Habitacion_Tipo_Descripcion descripcion,
                    Habitacion_Tipo_Porcentual porcentual,
                    cantidad = CASE Habitacion_Tipo_Descripcion
                        WHEN 'Base Simple'    THEN 1
                        WHEN 'Base Doble'     THEN 2
                        WHEN 'Base Triple'    THEN 3
                        WHEN 'Base Cuadruple' THEN 4
                        WHEN 'King'           THEN 5
                        ELSE -1
                    END
            FROM [gd_esquema].[Maestra]
         ) tipos 
    ORDER BY codigo ASC;
END
GO

-- Procedimiento que crea las reservas con IDENTITY (respetando los valores recibidos)
-- Requiere editar el cliente y el regimen a posteriori de la ejecución
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[CREAR_RESERVAS] AS
BEGIN
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
    SELECT '2018-01-01', inicio, inicio + noches, 1, -1, -1, 7
    FROM (
            SELECT DISTINCT Reserva_Codigo codigo, Reserva_Fecha_Inicio inicio, Reserva_Cant_Noches noches
            FROM [gd_esquema].[Maestra]
         ) reservas
    ORDER BY codigo
END
GO

-- Procedimiento que crea los consumibles con IDENTITY (respetando los valores recibidos)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[CREAR_CONSUMIBLES] AS
BEGIN
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[consumibles]
    SELECT precio, descripcion
    FROM (
            SELECT DISTINCT Consumible_Codigo codigo, Consumible_Descripcion descripcion, Consumible_Precio precio
            FROM [gd_esquema].[Maestra]
         ) consumibles
    WHERE codigo IS NOT NULL
    ORDER BY codigo
END
GO

-- Procedimiento que crea las facturas con IDENTITY (respetando los valores recibidos)
-- Requiere editar la estadía a posteriori de la ejecución
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[CREAR_FACTURAS] AS
BEGIN
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[facturas]
    SELECT fecha, total, -1, 13, ''
    FROM (
            SELECT DISTINCT Factura_Nro numero, Factura_Fecha fecha, Factura_Total total
            FROM [gd_esquema].[Maestra]
         ) facturas
    WHERE numero IS NOT NULL
    ORDER BY numero
END
GO

-- Procedure principal de la migración
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[MIGRAR_DATOS] AS
BEGIN
    -- Ejecuciones previas
    -- Debido a que necesitamos que se inserten respetando el orden de código
    -- (porque lo hicimos con IDENTITY), los ejecutamos por separado
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[CREAR_TIPOS_HABITACION];
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[CREAR_RESERVAS];
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[CREAR_CONSUMIBLES];
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[CREAR_FACTURAS];

    DECLARE @currentRow                                INT = 1;
    -- Campos para la tabla de Hoteles
    DECLARE @id_hotel                                INT;
    DECLARE @ciudad_hotel                            NVARCHAR(255);
    DECLARE @domicilio_calle_hotel                    NVARCHAR(255);
    DECLARE @domicilio_numero_hotel                    NUMERIC(18,0);
    DECLARE @cantidad_estrellas_hotel                NUMERIC(18,0);
    DECLARE @id_pais                                INT;
    DECLARE @recarga_por_estrellas_hotel            NUMERIC(18,0);

    -- Campos para la tabla de Habitaciones
    DECLARE @id_habitacion                            INT;
    DECLARE @numero_habitacion                        NUMERIC(18,0);
    DECLARE @piso_habitacion                        NUMERIC(18,0);
    DECLARE @ubicacion_habitacion                    NVARCHAR(50);
    DECLARE @descripcion_habitacion                    NVARCHAR(255);
    
    -- Campos para la tabla de Tipos de habitación
    DECLARE @id_tipo_habitacion                        INT;
    DECLARE @descripcion_tipo_habitacion            NVARCHAR(255);
    DECLARE @porcentual_tipo_habitacion                NUMERIC(18,2);

    -- Campos para la tabla de Regimenes
    DECLARE @id_regimen                                INT;
    DECLARE @descripcion_regimen                    NVARCHAR(255);
    DECLARE @precio_base_regimen                    NUMERIC(18,2);
    DECLARE @estado_regimen                            BIT = 1;

    -- Campos para la tabla de Reservas
    DECLARE @id_reserva                                INT;
    DECLARE @fecha_realizacion_reserva                DATETIME;
    DECLARE @fecha_inicio_reserva                    DATETIME;
    DECLARE @cant_noches_reserva                    NUMERIC(18,0);
    DECLARE @fecha_fin_reserva                        DATETIME;
    DECLARE @id_usuario                                INT = 1;
    DECLARE @id_estado_reserva                        INT = 7;

    -- Campos para la tabla de Estadias
    DECLARE @id_estadia                                INT;
    DECLARE @fecha_ingreso_estadia                    DATETIME;
    DECLARE @cant_dias_estadia                        NUMERIC(18,0);
    DECLARE @fecha_egreso_estadia                    DATETIME;

    -- Campos para la tabla de Consumibles
    DECLARE @id_consumible                            INT;
    DECLARE @precio_consumible                        NUMERIC(18,2);
    DECLARE @descripcion_consumible                    NVARCHAR(255);

    -- Campos para la tabla de Items Factura
    DECLARE @id_item_factura                        INT;
    DECLARE @precio_unitario_item_factura            NUMERIC(18,2);
    DECLARE @descripcion_item_factura                NVARCHAR(255);
    DECLARE @cantidad_item_factura                    NUMERIC(18,0);

    -- Campos para la tabla de Consumo
    DECLARE @id_consumo                                INT;
    DECLARE @fecha_hora_consumo                        DATETIME;

    -- Campos para la tabla de Facturas
    DECLARE @id_factura                                INT;
    DECLARE @fecha_factura                            DATETIME;
    DECLARE @total_factura                            NUMERIC(18,2);
    DECLARE @id_forma_de_pago                        INT = 13;

    -- Campos para la tabla de Clientes
    DECLARE @id_cliente                                INT;
    DECLARE @nombre_cliente                            NVARCHAR(255);
    DECLARE @apellido_cliente                        NVARCHAR(255);
    DECLARE @id_tipo_documento                        INT = 6;
    DECLARE @numero_documento_cliente                NUMERIC(18,0);
    DECLARE @correo_cliente                            NVARCHAR(255);
    DECLARE @telefono_cliente                        NVARCHAR(100);
    DECLARE @domicilio_calle_cliente                NVARCHAR(255);
    DECLARE @domicilio_numero_cliente                NUMERIC(18,0);
    DECLARE @domicilio_piso_cliente                    NUMERIC(18,0);
    DECLARE @domicilio_departamento_cliente            NVARCHAR(50);
    DECLARE @nacionalidad_cliente                    NVARCHAR(20);
    DECLARE @fecha_nacimiento_cliente                DATETIME;
    
    -- El super cursor
    DECLARE maestra_cursor CURSOR FOR
        SELECT    Hotel_Ciudad,
                Hotel_Calle,
                Hotel_Nro_Calle,
                Hotel_CantEstrella,
                Hotel_Recarga_Estrella,
                Habitacion_Numero,
                Habitacion_Piso,
                Habitacion_Frente,
                Habitacion_Tipo_Codigo,
                Habitacion_Tipo_Descripcion,
                Habitacion_Tipo_Porcentual,
                Regimen_Descripcion,
                Regimen_Precio,
                Reserva_Fecha_Inicio,
                Reserva_Codigo,
                Reserva_Cant_Noches,
                Reserva_Fecha_Inicio + Reserva_Cant_Noches,
                Estadia_Fecha_Inicio,
                Estadia_Cant_Noches,
                Estadia_Fecha_Inicio + Estadia_Cant_Noches,
                Consumible_Codigo,
                Consumible_Descripcion,
                Consumible_Precio,
                Item_Factura_Cantidad,
                Item_Factura_Monto,
                Factura_Nro,
                Factura_Fecha,
                Factura_Total,
                Cliente_Pasaporte_Nro,
                Cliente_Apellido,
                Cliente_Nombre,
                Cliente_Fecha_Nac,
                Cliente_Mail,
                Cliente_Dom_Calle,
                Cliente_Nro_Calle,
                Cliente_Piso,
                Cliente_Depto,
                Cliente_Nacionalidad
        FROM [gd_esquema].[Maestra];

        OPEN maestra_cursor;
        FETCH maestra_cursor INTO    @ciudad_hotel,
                                    @domicilio_calle_hotel,
                                    @domicilio_numero_hotel,
                                    @cantidad_estrellas_hotel,
                                    @recarga_por_estrellas_hotel,
                                    @numero_habitacion,
                                    @piso_habitacion,
                                    @ubicacion_habitacion,
                                    @id_tipo_habitacion,
                                    @descripcion_tipo_habitacion,
                                    @porcentual_tipo_habitacion,
                                    @descripcion_regimen,
                                    @precio_base_regimen,
                                    @fecha_inicio_reserva,
                                    @id_reserva,
                                    @cant_noches_reserva,
                                    @fecha_fin_reserva,
                                    @fecha_ingreso_estadia,
                                    @cant_dias_estadia,
                                    @fecha_egreso_estadia,
                                    @id_consumible,
                                    @descripcion_consumible,
                                    @precio_consumible,
                                    @cantidad_item_factura,
                                    @precio_unitario_item_factura,
                                    @id_factura,
                                    @fecha_factura,
                                    @total_factura,
                                    @numero_documento_cliente,
                                    @apellido_cliente,
                                    @nombre_cliente,
                                    @fecha_nacimiento_cliente,
                                    @correo_cliente,
                                    @domicilio_calle_cliente,
                                    @domicilio_numero_cliente,
                                    @domicilio_piso_cliente,
                                    @domicilio_departamento_cliente,
                                    @nacionalidad_cliente;
        
        WHILE (@@FETCH_STATUS = 0)
        BEGIN
            
            RAISERROR(N'Procesando row %d', 0, 1, @currentRow) WITH NOWAIT;

            BEGIN TRY
                ---------------------- HOTELES -------------------------------
                SELECT @id_hotel = id_hotel
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles]
                WHERE ciudad_hotel = @ciudad_hotel
                AND domicilio_calle_hotel = @domicilio_calle_hotel
                AND domicilio_numero_hotel = @domicilio_numero_hotel
                AND cantidad_estrellas_hotel = @cantidad_estrellas_hotel
                AND recarga_por_estrellas_hotel = @recarga_por_estrellas_hotel
            
                IF @id_hotel IS NULL
                BEGIN
                    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles]
                        (ciudad_hotel, domicilio_calle_hotel, domicilio_numero_hotel, cantidad_estrellas_hotel, recarga_por_estrellas_hotel)
                    VALUES (@ciudad_hotel, @domicilio_calle_hotel, @domicilio_numero_hotel, @cantidad_estrellas_hotel, @recarga_por_estrellas_hotel);
                    SET @id_hotel = SCOPE_IDENTITY();
                END
                ---------------------- HOTELES -------------------------------
                ---------------------- HABITACIONES --------------------------
                SELECT @id_habitacion = id_habitacion
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones]
                WHERE numero_habitacion = @numero_habitacion
                AND piso_habitacion = @piso_habitacion
                AND ubicacion_habitacion = @ubicacion_habitacion
                AND id_tipo_habitacion = @id_tipo_habitacion

                IF @id_habitacion IS NULL
                BEGIN
                    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones]
                        (id_hotel, numero_habitacion, piso_habitacion, ubicacion_habitacion, id_tipo_habitacion)
                    VALUES (@id_hotel, @numero_habitacion, @piso_habitacion, @ubicacion_habitacion, @id_tipo_habitacion);
                    SET @id_habitacion = SCOPE_IDENTITY();
                END

                -- Populamos reservasXhabitaciones
                IF (NOT EXISTS(SELECT *
                               FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones]
                               WHERE id_reserva = @id_reserva
                               AND id_habitacion = @id_habitacion
                              ))
                BEGIN
                    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones]
                    VALUES (@id_reserva, @id_habitacion);
                END
                ---------------------- HABITACIONES --------------------------
                ---------------------- REGIMENES -----------------------------
                SELECT @id_regimen = id_regimen
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes]
                WHERE descripcion_regimen = @descripcion_regimen
                AND precio_base_regimen = @precio_base_regimen

                IF @id_regimen IS NULL
                BEGIN
                    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes]
                        (descripcion_regimen, precio_base_regimen)
                    VALUES (@descripcion_regimen, @precio_base_regimen);
                    SET @id_regimen = SCOPE_IDENTITY();
                END

                -- Populamos hotelesXregimenes
                IF (NOT EXISTS(SELECT *
                               FROM [EL_MONSTRUO_DEL_LAGO_MASER].[hotelesXregimenes]
                               WHERE id_hotel = @id_hotel
                               AND id_regimen = @id_regimen
                              ))
                BEGIN
                    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[hotelesXregimenes]
                    VALUES (@id_hotel, @id_regimen);
                END

                -- Updateamos en reservas el regimen usado..
                UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
                SET id_regimen = @id_regimen
                WHERE id_reserva = @id_reserva;
                ---------------------- REGIMENES -----------------------------
                ---------------------- ESTADIAS ------------------------------
                IF (@fecha_ingreso_estadia IS NOT NULL
                    AND @fecha_egreso_estadia IS NOT NULL)
                BEGIN
                    SELECT @id_estadia = id_estadia
                    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
                    WHERE fecha_ingreso_estadia = @fecha_ingreso_estadia
                    AND fecha_egreso_estadia = @fecha_egreso_estadia
                    AND id_reserva = @id_reserva

                    IF @id_estadia IS NULL
                    BEGIN
                        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
                            (id_reserva, id_usuario, fecha_ingreso_estadia, fecha_egreso_estadia)
                        VALUES (@id_reserva, 1, @fecha_ingreso_estadia, @fecha_egreso_estadia);
                        SET @id_estadia = SCOPE_IDENTITY();
                    END

                    -- Updateamos en facturas la estadía
                    UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[facturas]
                    SET id_estadia = @id_estadia
                    WHERE id_factura = @id_factura;
                END
                ---------------------- ESTADIAS ------------------------------
                ---------------------- CONSUMOS ------------------------------
                IF (@id_consumible IS NOT NULL
                    AND @id_estadia IS NOT NULL)
                BEGIN
                    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[consumos]
                        (id_consumible, id_estadia, id_habitacion)
                    VALUES (@id_consumible, @id_estadia, @id_habitacion)
                    SET @id_consumo = SCOPE_IDENTITY();
                END
                ---------------------- CONSUMOS ------------------------------
                ---------------------- ITEMS FACTURA -------------------------
                IF (@precio_unitario_item_factura IS NOT NULL
                    AND @cantidad_item_factura IS NOT NULL
                    AND @id_factura IS NOT NULL)
                BEGIN
                    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[items_factura]
                        (id_factura, id_consumo, precio_unitario_item_factura, cantidad_item_factura)
                    VALUES (@id_factura, @id_consumo, @precio_unitario_item_factura, @cantidad_item_factura)
                    SET @id_item_factura = SCOPE_IDENTITY();
                END
                ---------------------- ITEMS FACTURA -------------------------
                ---------------------- CLIENTES ------------------------------
                SELECT @id_cliente = id_cliente
                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
                WHERE numero_documento_cliente = @numero_documento_cliente
                AND apellido_cliente = @apellido_cliente
                AND nombre_cliente = @nombre_cliente
                AND fecha_nacimiento_cliente = @fecha_nacimiento_cliente
                AND correo_cliente = @correo_cliente
                AND domicilio_calle_cliente = @domicilio_calle_cliente
                AND domicilio_numero_cliente = @domicilio_numero_cliente
                AND domicilio_piso_cliente = @domicilio_piso_cliente
                AND domicilio_departamento_cliente = @domicilio_departamento_cliente
                AND nacionalidad_cliente = @nacionalidad_cliente;
            
                IF @id_cliente IS NULL
                BEGIN
                    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
                        (nombre_cliente, apellido_cliente, numero_documento_cliente, correo_cliente, domicilio_calle_cliente, domicilio_numero_cliente, domicilio_piso_cliente, domicilio_departamento_cliente, nacionalidad_cliente, fecha_nacimiento_cliente)
                    VALUES (@nombre_cliente, @apellido_cliente, @numero_documento_cliente, @correo_cliente, @domicilio_calle_cliente, @domicilio_numero_cliente, @domicilio_piso_cliente, @domicilio_departamento_cliente, @nacionalidad_cliente, @fecha_nacimiento_cliente);
                    SET @id_cliente = SCOPE_IDENTITY();
                END

                -- Populamos clientesXestadias
                IF (@id_estadia IS NOT NULL)
                BEGIN
                    IF (NOT EXISTS(SELECT *
                                   FROM [EL_MONSTRUO_DEL_LAGO_MASER].[clientesXestadias]
                                   WHERE id_cliente = @id_cliente
                                   AND id_estadia = @id_estadia
                                  ))
                    BEGIN
                        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[clientesXestadias]
                        VALUES (@id_cliente, @id_estadia);
                    END
                END
				
				-- Updateamos en reservas el cliente
                UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
                SET id_cliente = @id_cliente
                WHERE id_reserva = @id_reserva;
				
                ---------------------- CLIENTES ------------------------------

                -- Finalmente defino nulo los IDs utilizados.
                SET @id_hotel = NULL;
                SET @id_habitacion = NULL;
                SET @id_regimen = NULL;
                SET @id_estadia = NULL;
                SET @id_consumo = NULL;
                SET @id_item_factura = NULL;
                SET @id_cliente = NULL;
            END TRY
            BEGIN CATCH
                INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[migracion_errores]
                VALUES (ERROR_MESSAGE(),
                    @ciudad_hotel,
                    @domicilio_calle_hotel,
                    @domicilio_numero_hotel,
                    @cantidad_estrellas_hotel,
                    @recarga_por_estrellas_hotel,
                    @numero_habitacion,
                    @piso_habitacion,
                    @ubicacion_habitacion,
                    @id_tipo_habitacion,
                    @descripcion_tipo_habitacion,
                    @porcentual_tipo_habitacion,
                    @descripcion_regimen,
                    @precio_base_regimen,
                    @fecha_inicio_reserva,
                    @id_reserva,
                    @cant_noches_reserva,
                    @fecha_ingreso_estadia,
                    @cant_dias_estadia,
                    @id_consumible,
                    @descripcion_consumible,
                    @precio_consumible,
                    @cantidad_item_factura,
                    @precio_unitario_item_factura,
                    @id_factura,
                    @fecha_factura,
                    @total_factura,
                    @numero_documento_cliente,
                    @apellido_cliente,
                    @nombre_cliente,
                    @fecha_nacimiento_cliente,
                    @correo_cliente,
                    @domicilio_calle_cliente,
                    @domicilio_numero_cliente,
                    @domicilio_piso_cliente,
                    @domicilio_departamento_cliente,
                    @nacionalidad_cliente)
            END CATCH

            -- Y hago el fetch
            FETCH maestra_cursor INTO    @ciudad_hotel,
                                    @domicilio_calle_hotel,
                                    @domicilio_numero_hotel,
                                    @cantidad_estrellas_hotel,
                                    @recarga_por_estrellas_hotel,
                                    @numero_habitacion,
                                    @piso_habitacion,
                                    @ubicacion_habitacion,
                                    @id_tipo_habitacion,
                                    @descripcion_tipo_habitacion,
                                    @porcentual_tipo_habitacion,
                                    @descripcion_regimen,
                                    @precio_base_regimen,
                                    @fecha_inicio_reserva,
                                    @id_reserva,
                                    @cant_noches_reserva,
                                    @fecha_fin_reserva,
                                    @fecha_ingreso_estadia,
                                    @cant_dias_estadia,
                                    @fecha_egreso_estadia,
                                    @id_consumible,
                                    @descripcion_consumible,
                                    @precio_consumible,
                                    @cantidad_item_factura,
                                    @precio_unitario_item_factura,
                                    @id_factura,
                                    @fecha_factura,
                                    @total_factura,
                                    @numero_documento_cliente,
                                    @apellido_cliente,
                                    @nombre_cliente,
                                    @fecha_nacimiento_cliente,
                                    @correo_cliente,
                                    @domicilio_calle_cliente,
                                    @domicilio_numero_cliente,
                                    @domicilio_piso_cliente,
                                    @domicilio_departamento_cliente,
                                    @nacionalidad_cliente;

        SET @currentRow = @currentRow + 1;

        END

        CLOSE maestra_cursor;
        DEALLOCATE maestra_cursor;
END
GO

