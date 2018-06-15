-- Obtiene el listado de regimenes
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_REGIMENES]
AS
BEGIN
    SELECT id_regimen, descripcion_regimen, precio_base_regimen, estado_regimen
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes]
END

GO

-- Tipo necesario para los regimenes
CREATE TYPE listaDeRegimenes AS TABLE
    (id_regimen INT)

GO

-- Agrega un nuevo hotel
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[AGREGAR_NUEVO_HOTEL](@id_rol_user INT, @nombre_hotel NVARCHAR(255), @correo_hotel NVARCHAR(255),
 @telefono_hotel NVARCHAR(100), @ciudad_hotel NVARCHAR(255), @domicilio_calle_hotel NVARCHAR(255), @domicilio_numero_hotel NUMERIC(18,0),
 @cantidad_estrellas_hotel NUMERIC(18,0), @id_pais INT, @fecha_creacion_hotel DATETIME, @recarga_por_estrellas_hotel NUMERIC(18,0), @regimenes listaDeRegimenes READONLY)
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
