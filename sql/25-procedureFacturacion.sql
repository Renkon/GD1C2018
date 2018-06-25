-- Este procedure obtiene los datos para iniciar el proceso de facturación
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_DATOS_ESTADIA_FACTURACION]
    (@id_estadia INT)
AS
BEGIN
    SELECT fecha_ingreso_estadia, fecha_egreso_estadia, 
    fecha_inicio_reserva, fecha_fin_reserva, id_regimen
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
        ON (e.id_reserva = r.id_reserva)
    WHERE id_estadia = @id_estadia
    AND consumos_cerrados = 1
    AND fecha_egreso_estadia IS NOT NULL
    AND id_estadia NOT IN (SELECT DISTINCT id_estadia
                           FROM [EL_MONSTRUO_DEL_LAGO_MASER].[facturas])
END

GO

-- Funcion que obtiene el costo diario de una estadia
CREATE FUNCTION [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_COSTO_DIARIO_ESTADIA]
(@id_estadia INT)
RETURNS NUMERIC(18,2)
AS
BEGIN
    DECLARE @costo NUMERIC(18,2)
    SELECT @costo = SUM((precio_base_regimen * cantidad_huespedes_tipo_habitacion * (1 + (porcentual_tipo_habitacion/100))) +
           (cantidad_estrellas_hotel * recarga_por_estrellas_hotel))
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r
        ON e.id_reserva = r.id_reserva
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes] rr
        ON r.id_regimen = rr.id_regimen
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] rxh
        ON r.id_reserva = rxh.id_reserva
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] h
        ON rxh.id_habitacion = h.id_habitacion
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[tipos_habitacion] th
        ON h.id_tipo_habitacion = th.id_tipo_habitacion
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles] hh
        ON h.id_hotel = hh.id_hotel
    WHERE id_estadia = @id_estadia

    RETURN @costo
END

GO

-- Este procedure obtiene el costo diario de una estadia/reserva
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[CALCULAR_COSTO_DIARIO_ESTADIA]
    (@id_estadia INT)
AS
BEGIN
    SELECT EL_MONSTRUO_DEL_LAGO_MASER.OBTENER_COSTO_DIARIO_ESTADIA(@id_estadia)
END

GO

-- Obtengo el ID del regimen all inclusive moderado
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_REGIMEN_ALL_INCLUSIVE_MODERADO]
AS
BEGIN
	SELECT id_regimen
	FROM [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes]
	WHERE descripcion_regimen = 'All inclusive moderado'
END

GO

-- Este procedure obtiene el regimen de estadía
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_REGIMEN_DE_ESTADIA](@id_estadia INT)
AS
BEGIN
    SELECT r.id_regimen, descripcion_regimen, precio_base_regimen, estado_regimen
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] e 
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] r 
        ON (e.id_reserva = r.id_reserva) 
    JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes] re 
        ON (r.id_regimen = re.id_regimen) 
    WHERE id_estadia = @id_estadia
END

GO

-- Este procedure obtiene las formas de pago
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_FORMAS_DE_PAGO]
AS
BEGIN
    SELECT id_forma_de_pago, descripcion_forma_de_pago
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[formas_de_pago]
END

GO

-- Type necesario para insertar facturas
CREATE TYPE [EL_MONSTRUO_DEL_LAGO_MASER].[listaDeItems] AS TABLE
    (id_consumo INT,
    precio_unitario_item_factura NUMERIC(18,2),
    descripcion_item_factura NVARCHAR(255), 
    cantidad_item_factura NUMERIC(18,0))

GO

-- Este procedure ingresa la factura con los items
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[CREAR_FACTURA]
    (@id_rol_user INT, @fecha_factura DATETIME, @total_factura NUMERIC(18,2), @id_estadia INT, @id_forma_de_pago INT, 
    @detalle_pago NVARCHAR(1000), @listaDeItems EL_MONSTRUO_DEL_LAGO_MASER.listaDeItems READONLY)
AS
BEGIN
    DECLARE @id_consumo                     INT
    DECLARE @precio_unitario_item_factura   NUMERIC(18,2)
    DECLARE @descripcion_item_factura       NVARCHAR(255)
    DECLARE @cantidad_item_factura          NUMERIC(18,0)
    DECLARE @id_factura                     INT

    DECLARE cursor_items CURSOR FOR
        SELECT id_consumo, precio_unitario_item_factura, 
            descripcion_item_factura, cantidad_item_factura
        FROM @listaDeItems

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 24

    BEGIN TRY
        BEGIN TRANSACTION

        -- Primero insertamos la factura
        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[facturas]
            (fecha_factura, total_factura, id_estadia, id_forma_de_pago, detalle_pago)
        VALUES(@fecha_factura, @total_factura, @id_estadia, @id_forma_de_pago, @detalle_pago)

        SET @id_factura = SCOPE_IDENTITY()

        -- Y luego insertamos los items de factura
        OPEN cursor_items 
        FETCH cursor_items INTO @id_consumo, 
                                @precio_unitario_item_factura, 
                                @descripcion_item_factura, 
                                @cantidad_item_factura

        WHILE (@@FETCH_STATUS = 0) BEGIN
            INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[items_factura]
                (id_factura, id_consumo, precio_unitario_item_factura, 
                descripcion_item_factura, cantidad_item_factura)
            VALUES(@id_factura, @id_consumo, @precio_unitario_item_factura, 
                @descripcion_item_factura, @cantidad_item_factura)

            FETCH cursor_items INTO @id_consumo, 
                                    @precio_unitario_item_factura, 
                                    @descripcion_item_factura, 
                                    @cantidad_item_factura
        END

        SELECT @id_factura id_factura

        COMMIT TRANSACTION
        CLOSE cursor_items 
        DEALLOCATE cursor_items 
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION

        IF CURSOR_STATUS('global', 'cursor_items') = 1 BEGIN
            CLOSE cursor_items
        END
        DEALLOCATE cursor_items;

        THROW
    END CATCH
    

END

GO

