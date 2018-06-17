-- Obtiene el listado de tipos de habitaciòn
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_TIPOS_HABITACION]
AS
BEGIN
    SELECT id_tipo_habitacion, descripcion_tipo_habitacion, porcentual_tipo_habitacion, cantidad_huespedes_tipo_habitacion
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[tipos_habitacion]
END

GO

-- Inserta una habitación a la tabla de habitaciones
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_HABITACION] (@id_rol_user INT,@id_hotel INT, @numero_habitacion numeric(18,0), @piso_habitacion numeric(18,0),
    @ubicacion_habitacion nvarchar(50), @id_tipo_habitacion INT, @descripcion_habitacion NVARCHAR(255))
AS
BEGIN
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 16

    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones]
        (id_hotel, numero_habitacion, piso_habitacion, ubicacion_habitacion, id_tipo_habitacion, descripcion_habitacion)
    VALUES(@id_hotel, @numero_habitacion, @piso_habitacion, @ubicacion_habitacion, @id_tipo_habitacion, @descripcion_habitacion)
END

GO

-- Usado para obtener las habitaciones filtradas
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_HABITACIONES_FILTRADAS](@id_hotel INT, @numero_habitacion numeric(18,0), 
    @piso_habitacion numeric(18,0), @id_tipo_habitacion INT)
AS
BEGIN
    SELECT id_habitacion, numero_habitacion, piso_habitacion, ubicacion_habitacion, id_tipo_habitacion, descripcion_habitacion
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones]
    WHERE id_hotel = @id_hotel 
    AND (@numero_habitacion = 0 OR CAST(numero_habitacion AS NVARCHAR) LIKE '%' + CAST(@numero_habitacion AS NVARCHAR) + '%')
    AND (@piso_habitacion = 0 OR CAST(piso_habitacion AS NVARCHAR) LIKE '%' + CAST(@piso_habitacion AS NVARCHAR) + '%')
    AND (@id_tipo_habitacion = -1 OR id_tipo_habitacion = @id_tipo_habitacion)
END

GO

-- Modifica un hotel existente
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[MODIFICAR_HABITACION] (@id_rol_user INT, @id_habitacion INT, 
    @numero_habitacion numeric(18,0), @piso_habitacion numeric(18,0), @ubicacion_habitacion NVARCHAR(50), @descripcion_habitacion NVARCHAR(255))
AS
BEGIN
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 17

    UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones]
    SET numero_habitacion = @numero_habitacion, piso_habitacion = @piso_habitacion, ubicacion_habitacion = @ubicacion_habitacion, 
        descripcion_habitacion = @descripcion_habitacion
    WHERE id_habitacion = @id_habitacion
END

GO

-- Agrega un cierre temporal de la habitación
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_CIERRE_TEMPORAL_HABITACION]
    (@id_rol_user INT, @inicio DATETIME, @fin DATETIME, @id_habitacion INT, @motivo NVARCHAR(2500))
AS
BEGIN
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 18

        IF (EXISTS(SELECT * FROM [EL_MONSTRUO_DEL_LAGO_MASER].[cierres_temporales_habitacion]
                   WHERE (@inicio <= fecha_fin_cierre_temporal_habitacion)
                           AND (fecha_inicio_cierre_temporal_habitacion <= @fin)
                           AND id_habitacion = @id_habitacion))
        BEGIN
                RAISERROR('50001 - Esa habitación ya tiene un cierre temporal durante ese margen de fechas', 20, 1) WITH LOG
        END

        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[cierres_temporales_habitacion]
        VALUES (@inicio, @fin, @id_habitacion, @motivo)
END

GO

