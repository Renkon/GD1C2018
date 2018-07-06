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
    SELECT convert(datetime, '20010101', 112), inicio, inicio + noches, 0, -1, 7
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

    SELECT CAST(NULL as INT)                          id_hotel,
           Hotel_Ciudad                               ciudad_hotel,
           Hotel_Calle                                domicilio_calle_hotel,
           Hotel_Nro_Calle                            domicilio_numero_hotel,
           Hotel_CantEstrella                         cantidad_estrellas_hotel,
           Hotel_Recarga_Estrella                     recarga_por_estrellas_hotel,
           CAST(NULL as INT)                          id_habitacion,
           Habitacion_Numero                          numero_habitacion,
           Habitacion_Piso                            piso_habitacion,
           Habitacion_Frente                          ubicacion_habitacion,
           Habitacion_Tipo_Codigo                     id_tipo_habitacion,
           CAST(NULL as INT)                          id_regimen,
           Regimen_Descripcion                        descripcion_regimen,
           Regimen_Precio                             precio_base_regimen,
           Reserva_Codigo                             id_reserva,
           CAST(NULL as INT)                          id_estadia,
           Estadia_Fecha_Inicio                       fecha_ingreso_estadia,
           Estadia_Fecha_Inicio + Estadia_Cant_Noches fecha_egreso_estadia,
           Consumible_Codigo                          id_consumible,
           CAST(NULL as INT)                          id_consumo,
           CAST(NULL as INT)                          id_item_factura,
           Item_Factura_Cantidad                      cantidad_item_factura,
           Item_Factura_Monto                         precio_unitario_item_factura,
           Factura_Nro                                id_factura,
           CAST(NULL as INT)                          id_cliente,
           Cliente_Pasaporte_Nro                      numero_documento_cliente,
           Cliente_Apellido                           apellido_cliente,
           Cliente_Nombre                             nombre_cliente,
           Cliente_Fecha_Nac                          fecha_nacimiento_cliente,
           Cliente_Mail                               correo_cliente,
           Cliente_Dom_Calle                          domicilio_calle_cliente,
           Cliente_Nro_Calle                          domicilio_numero_cliente,
           Cliente_Piso                               domicilio_piso_cliente,
           Cliente_Depto                              domicilio_departamento_cliente,
           Cliente_Nacionalidad                       nacionalidad_cliente
    INTO #master
    FROM [gd_esquema].[Maestra]
    
    -- Necesitamos esta secuencia para los consumos (que no son distinct)
    CREATE SEQUENCE [EL_MONSTRUO_DEL_LAGO_MASER].[secuencia_consumos]
    AS INT
    START WITH 1
    INCREMENT BY 1
    
    -- Y esta para los items de factura
    CREATE SEQUENCE [EL_MONSTRUO_DEL_LAGO_MASER].[secuencia_items_factura]
    AS INT
    START WITH 1
    INCREMENT BY 1
    
    UPDATE #master
    SET id_consumo = NEXT VALUE FOR [EL_MONSTRUO_DEL_LAGO_MASER].[secuencia_consumos]
    WHERE id_consumible IS NOT NULL
    AND   fecha_ingreso_estadia IS NOT NULL
    
    UPDATE #master
    SET id_item_factura = NEXT VALUE FOR [EL_MONSTRUO_DEL_LAGO_MASER].[secuencia_items_factura]
    WHERE id_factura IS NOT NULL
    
    DROP SEQUENCE [EL_MONSTRUO_DEL_LAGO_MASER].[secuencia_consumos]
    DROP SEQUENCE [EL_MONSTRUO_DEL_LAGO_MASER].[secuencia_items_factura]
    
    ------------------------------------------------ HOTELES ------------------------------------------------
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles]
        (ciudad_hotel, domicilio_calle_hotel, domicilio_numero_hotel, cantidad_estrellas_hotel, recarga_por_estrellas_hotel)
    SELECT DISTINCT ciudad_hotel, domicilio_calle_hotel, domicilio_numero_hotel, cantidad_estrellas_hotel, recarga_por_estrellas_hotel
    FROM #master
    
    -- Seteamos el id_hotel en la tabla temporal para seguir con el procesamiento
    UPDATE #master
    SET id_hotel = h.id_hotel
    FROM #master temp
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles] h
    ON  h.ciudad_hotel = temp.ciudad_hotel
    AND h.domicilio_calle_hotel = temp.domicilio_calle_hotel
    AND h.domicilio_numero_hotel = temp.domicilio_numero_hotel
    AND h.cantidad_estrellas_hotel = temp.cantidad_estrellas_hotel
    AND h.recarga_por_estrellas_hotel = temp.recarga_por_estrellas_hotel
    
    -- Limpiamos las columnas que no necesitamos más
    ALTER TABLE #master
    DROP COLUMN ciudad_hotel, domicilio_calle_hotel, domicilio_numero_hotel, cantidad_estrellas_hotel, recarga_por_estrellas_hotel
    ------------------------------------------------ HOTELES ------------------------------------------------
    ---------------------------------------------- HABITACIONES ---------------------------------------------
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones]
        (id_hotel, numero_habitacion, piso_habitacion, ubicacion_habitacion, id_tipo_habitacion)
    SELECT DISTINCT id_hotel, numero_habitacion, piso_habitacion, ubicacion_habitacion, id_tipo_habitacion
    FROM #master
    
    -- Seteamos el id_habitacion para seguir el procesamiento
    UPDATE #master
    SET id_habitacion = h.id_habitacion
    FROM #master temp
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] h
    ON  h.id_hotel = temp.id_hotel
    AND h.numero_habitacion = temp.numero_habitacion
    AND h.piso_habitacion = temp.piso_habitacion
    AND h.ubicacion_habitacion = temp.ubicacion_habitacion
    AND h.id_tipo_habitacion = temp.id_tipo_habitacion
    
    -- Insertamos a la relación muchos a muchos entre reservas y habitaciones
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones]
        (id_reserva, id_habitacion)
    SELECT DISTINCT id_reserva, id_habitacion
    FROM #master
    
    -- Limpiamos las columnas que no necesitamos más
    ALTER TABLE #master
    DROP COLUMN numero_habitacion, piso_habitacion, ubicacion_habitacion, id_tipo_habitacion
    ---------------------------------------------- HABITACIONES ---------------------------------------------
    ----------------------------------------------- REGIMENES -----------------------------------------------
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes]
        (descripcion_regimen, precio_base_regimen)
    SELECT DISTINCT descripcion_regimen, precio_base_regimen
    FROM #master
    
    -- Seteamos el id_regimen para seguir el procesamiento
    UPDATE #master
    SET id_regimen = r.id_regimen
    FROM #master temp
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes] r
    ON  r.descripcion_regimen = temp.descripcion_regimen
    AND r.precio_base_regimen = temp.precio_base_regimen
    
    -- Insertamos a la relación muchos a muchos entre hoteles y regimenes
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[hotelesXregimenes]
        (id_hotel, id_regimen)
    SELECT DISTINCT id_hotel, id_regimen
    FROM #master
    
    -- Updateamos en reservas el regimen usado
    UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
        SET id_regimen = temp.id_regimen
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
    JOIN #master temp
        ON r.id_reserva = temp.id_reserva
    
    -- Limpiamos las columnas que no necesitamos más
    ALTER TABLE #master
    DROP COLUMN descripcion_regimen, precio_base_regimen, id_hotel, id_regimen
    ----------------------------------------------- REGIMENES -----------------------------------------------
    -----------------------------------------------  ESTADIAS -----------------------------------------------
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
        (id_reserva, id_usuario_ingreso, id_usuario_egreso, fecha_ingreso_estadia, fecha_egreso_estadia)
    SELECT DISTINCT id_reserva, 1, 1, fecha_ingreso_estadia, fecha_egreso_estadia
    FROM #master
    WHERE fecha_ingreso_estadia IS NOT NULL
    
    -- Seteamos el id_estadia para seguir el procesamiento
    UPDATE #master
    SET id_estadia = e.id_estadia
    FROM #master temp
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e
    ON  e.id_reserva = temp.id_reserva
    AND e.fecha_ingreso_estadia = temp.fecha_ingreso_estadia
    AND e.fecha_egreso_estadia = temp.fecha_egreso_estadia
    
    -- Updateamos en facturas la estadía
    UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[facturas]
        SET id_estadia = temp.id_estadia
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[facturas] f
    JOIN #master temp
        ON f.id_factura = temp.id_factura
    
    -- Limpiamos las columnas que no necesitamos más
    ALTER TABLE #master
    DROP COLUMN fecha_ingreso_estadia, fecha_egreso_estadia
    -----------------------------------------------  ESTADIAS -----------------------------------------------
    -----------------------------------------------  CONSUMOS -----------------------------------------------
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[consumos]
        (id_consumible, id_estadia, id_habitacion, cantidad_consumo)
    SELECT id_consumible, id_estadia, id_habitacion, cantidad_item_factura 
    FROM #master
    WHERE id_consumo IS NOT NULL
    ORDER BY id_consumo
    
    -- Limpiamos las columnas que no necesitamos más
    ALTER TABLE #master
    DROP COLUMN id_consumible, id_habitacion
    -----------------------------------------------  CONSUMOS -----------------------------------------------
    ---------------------------------------------  ITEMS FACTURA --------------------------------------------
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[items_factura]
        (id_factura, id_consumo, precio_unitario_item_factura, cantidad_item_factura)
    SELECT id_factura, id_consumo, precio_unitario_item_factura, cantidad_item_factura
    FROM #master
    WHERE id_item_factura IS NOT NULL
    
    -- Limpiamos las columnas que no necesitamos más
    ALTER TABLE #master
    DROP COLUMN id_consumo, id_item_factura, cantidad_item_factura, precio_unitario_item_factura, id_factura
    ---------------------------------------------  ITEMS FACTURA --------------------------------------------
    -----------------------------------------------  CLIENTES -----------------------------------------------
    CREATE TABLE #clientes_premigracion (
        nombre_cliente NVARCHAR(255),
        apellido_cliente NVARCHAR(255),
        numero_documento_cliente NUMERIC(18, 0),
        correo_cliente NVARCHAR(255),
        domicilio_calle_cliente NVARCHAR(255),
        domicilio_numero_cliente NUMERIC(18, 0),
        domicilio_piso_cliente NUMERIC(18, 0),
        domicilio_departamento_cliente NVARCHAR(50),
        nacionalidad_cliente NVARCHAR(20),
        fecha_nacimiento_cliente DATETIME
    )
    
    -- Creamos una tabla temporal con unique indexes que no exploten al hacer bulk insert
    CREATE UNIQUE INDEX temp_clientes_numero_documento
    ON #clientes_premigracion(numero_documento_cliente)
    WITH IGNORE_DUP_KEY
    
    CREATE UNIQUE INDEX temp_clientes_correo
    ON #clientes_premigracion(correo_cliente)
    WITH IGNORE_DUP_KEY
    
    -- Poblamos la tabla
    INSERT INTO #clientes_premigracion
    SELECT DISTINCT nombre_cliente, apellido_cliente, numero_documento_cliente, correo_cliente, domicilio_calle_cliente,
        domicilio_numero_cliente, domicilio_piso_cliente, domicilio_departamento_cliente, nacionalidad_cliente, fecha_nacimiento_cliente
    FROM #master
    
    -- Insertamos los clientes
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
        (nombre_cliente, apellido_cliente, numero_documento_cliente, correo_cliente, domicilio_calle_cliente,
        domicilio_numero_cliente, domicilio_piso_cliente, domicilio_departamento_cliente, nacionalidad_cliente, fecha_nacimiento_cliente)
    SELECT nombre_cliente, apellido_cliente, numero_documento_cliente, correo_cliente, domicilio_calle_cliente,
        domicilio_numero_cliente, domicilio_piso_cliente, domicilio_departamento_cliente, nacionalidad_cliente, fecha_nacimiento_cliente 
    FROM #clientes_premigracion
    
    -- Borramos la tabla temporal
    DROP TABLE #clientes_premigracion
    
    -- Seteamos el id_cliente para seguir el procesamiento
    UPDATE #master
    SET id_cliente = c.id_cliente
    FROM #master temp
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[clientes] c
    ON  c.nombre_cliente = temp.nombre_cliente
    AND c.apellido_cliente = temp.apellido_cliente 
    AND c.numero_documento_cliente = temp.numero_documento_cliente
    AND c.correo_cliente = temp.correo_cliente
    AND c.domicilio_calle_cliente = temp.domicilio_calle_cliente
    AND c.domicilio_numero_cliente = temp.domicilio_numero_cliente
    AND c.domicilio_piso_cliente = temp.domicilio_piso_cliente
    AND c.domicilio_departamento_cliente = temp.domicilio_departamento_cliente
    AND c.nacionalidad_cliente = temp.nacionalidad_cliente
    AND c.fecha_nacimiento_cliente = temp.fecha_nacimiento_cliente 
    
    -- Insertamos a la relación muchos a muchos entre clientes y estadias
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[clientesXestadias]
    SELECT DISTINCT id_cliente, id_estadia
    FROM #master
    WHERE id_estadia IS NOT NULL
    AND   id_cliente IS NOT NULL
    
    -- Updateamos en reservas al cliente
    UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
        SET id_cliente = temp.id_cliente
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
    JOIN #master temp
        ON r.id_reserva = temp.id_reserva
    WHERE temp.id_cliente IS NOT NULL
    -----------------------------------------------  CLIENTES -----------------------------------------------
    -- Preparamos la tabla de errores de migración con los datos restantes.
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[migracion_errores]
    SELECT *
    FROM #master
    WHERE id_cliente IS NULL
    
    DROP TABLE #master

END

GO

EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[MIGRAR_DATOS]

GO

