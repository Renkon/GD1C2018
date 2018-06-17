-- Obtiene el listado de regimenes
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_REGIMENES]
AS
BEGIN
    SELECT id_regimen, descripcion_regimen, precio_base_regimen, estado_regimen
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes]
END

GO

-- Tipo necesario para los regimenes
CREATE TYPE [EL_MONSTRUO_DEL_LAGO_MASER].[listaDeRegimenes] AS TABLE
    (id_regimen INT)

GO

-- Agrega un nuevo hotel
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[AGREGAR_NUEVO_HOTEL](@id_rol_user INT, @nombre_hotel NVARCHAR(255), @correo_hotel NVARCHAR(255),
 @telefono_hotel NVARCHAR(100), @ciudad_hotel NVARCHAR(255), @domicilio_calle_hotel NVARCHAR(255), @domicilio_numero_hotel NUMERIC(18,0),
 @cantidad_estrellas_hotel NUMERIC(18,0), @id_pais INT, @fecha_creacion_hotel DATETIME, @recarga_por_estrellas_hotel NUMERIC(18,0), @regimenes EL_MONSTRUO_DEL_LAGO_MASER.listaDeRegimenes READONLY)
AS
BEGIN
    DECLARE @id_reg   INT 
    DECLARE @id_hotel INT

    DECLARE cursor_regimenes CURSOR FOR
        SELECT id_regimen
        FROM  @regimenes
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 13
    
	BEGIN TRY
		BEGIN TRANSACTION
		INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles]
		(nombre_hotel, correo_hotel, telefono_hotel, ciudad_hotel, domicilio_calle_hotel, domicilio_numero_hotel,
		cantidad_estrellas_hotel, id_pais, fecha_creacion_hotel, recarga_por_estrellas_hotel)
		VALUES (@nombre_hotel, @correo_hotel, @telefono_hotel, @ciudad_hotel, @domicilio_calle_hotel, @domicilio_numero_hotel,
		@cantidad_estrellas_hotel, @id_pais, @fecha_creacion_hotel, @recarga_por_estrellas_hotel)

		SET @id_hotel = SCOPE_IDENTITY();

		OPEN cursor_regimenes
			FETCH cursor_regimenes INTO @id_reg
			WHILE(@@FETCH_STATUS = 0)
			BEGIN
				INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[hotelesXregimenes]
					(id_hotel,id_regimen)
				VALUES (@id_hotel, @id_reg)
				FETCH cursor_regimenes INTO @id_reg
			END
		COMMIT TRANSACTION
		CLOSE cursor_regimenes
		DEALLOCATE cursor_regimenes
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION

        IF CURSOR_STATUS('global', 'cursor_regimenes') = 1 BEGIN
            CLOSE cursor_regimenes
        END
        DEALLOCATE cursor_regimenes;

        THROW
	END CATCH
	
END

GO

-- Trae los hoteles con los filtros posibles.
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_HOTELES_FILTRADOS](@nombre nvarchar(255), @estrellas numeric(18,0),
    @ciudad nvarchar(255), @id_pais int)
AS
BEGIN

    SELECT id_hotel, nombre_hotel, correo_hotel, telefono_hotel, ciudad_hotel,
	   domicilio_calle_hotel, domicilio_numero_hotel, cantidad_estrellas_hotel, id_pais, 
           fecha_creacion_hotel, recarga_por_estrellas_hotel
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles]
    WHERE LOWER(nombre_hotel) LIKE '%' + LOWER(@nombre) + '%'
    AND (@estrellas = 0 OR cantidad_estrellas_hotel = @estrellas)
    AND LOWER(ciudad_hotel) LIKE '%' + LOWER(@ciudad) + '%'
    AND (@id_pais = -1 OR id_pais = @id_pais)
END

GO

-- Obtiene los regimenes pero de un cierto hotel (solo el ID)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_REGIMENES_DE_UN_HOTEL] (@id_hotel INT)
AS
BEGIN
    SELECT id_regimen
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[hotelesXregimenes]
    WHERE id_hotel = @id_hotel
END

GO

-- Ingresa un nuevo cierre temporal
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_CIERRE_TEMPORAL_HOTEL]
    (@id_rol_user INT, @inicio DATETIME, @fin DATETIME, @id_hotel INT, @motivo NVARCHAR(2500))
AS
BEGIN
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 15

	IF (EXISTS(SELECT * FROM [EL_MONSTRUO_DEL_LAGO_MASER].[cierres_temporales_hotel]
	           WHERE (@inicio <= fecha_fin_cierre_temporal_hotel)
			   AND (fecha_inicio_cierre_temporal_hotel <= @fin)
                           AND id_hotel = @id_hotel))
	BEGIN
		RAISERROR('50001 - Ese hotel ya tiene un cierre temporal durante ese margen de fechas', 20, 1) WITH LOG 
	END

	INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[cierres_temporales_hotel]
	VALUES (@inicio, @fin, @id_hotel, @motivo)
END

GO

-- Este procedure modifica un hotel
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[MODIFICAR_HOTEL] (@id_rol_user INT, @id_hotel INT, @nombre_hotel NVARCHAR(255), @correo_hotel NVARCHAR(255),
    @telefono_hotel NVARCHAR(100), @ciudad_hotel NVARCHAR(255), @domicilio_calle_hotel NVARCHAR(255), @domicilio_numero_hotel NVARCHAR(255),
    @cantidad_estrellas_hotel NUMERIC(18,0), @id_pais INT, @fecha_creacion_hotel DATETIME, @recarga_por_estrellas_hotel NUMERIC(18,0), @regimenes EL_MONSTRUO_DEL_LAGO_MASER.listaDeRegimenes READONLY)
AS
BEGIN
    DECLARE @id_reg INT
    DECLARE cursor_regimenes CURSOR FOR
         SELECT id_regimen
         FROM @regimenes

    -- Comprobamos si el usuario tiene la funcionalidad de modificar hoteles
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 14
	
	BEGIN TRY
	    BEGIN TRANSACTION
        -- Actualizamos al hotel
        UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles]
        SET nombre_hotel = @nombre_hotel, correo_hotel = @correo_hotel, telefono_hotel = @telefono_hotel, ciudad_hotel = @ciudad_hotel, 
            domicilio_calle_hotel = @domicilio_calle_hotel, domicilio_numero_hotel = @domicilio_numero_hotel, cantidad_estrellas_hotel = @cantidad_estrellas_hotel, 
            id_pais = @id_pais, fecha_creacion_hotel = @fecha_creacion_hotel, recarga_por_estrellas_hotel = @recarga_por_estrellas_hotel
        WHERE id_hotel = @id_hotel
	    
        --Borramos todos los regimenes del hotel
        DELETE FROM [EL_MONSTRUO_DEL_LAGO_MASER].[hotelesXregimenes]
        WHERE id_hotel = @id_hotel
	    
        --Agregamos los nuevos regimenes con un cursor.
        OPEN cursor_regimenes
        FETCH cursor_regimenes INTO @id_reg
        WHILE (@@FETCH_STATUS = 0) BEGIN
            INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[hotelesXregimenes]
                (id_hotel,id_regimen)
            VALUES(@id_hotel,@id_reg)
            FETCH cursor_regimenes INTO @id_reg
        END

		COMMIT TRANSACTION
        CLOSE cursor_regimenes
        DEALLOCATE cursor_regimenes
	END TRY
	BEGIN CATCH
	    ROLLBACK TRANSACTION

        IF CURSOR_STATUS('global', 'cursor_regimenes') = 1 BEGIN
            CLOSE cursor_regimenes
        END
        DEALLOCATE cursor_regimenes;
	END CATCH
END

GO


