-- Obtiene los hoteles
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_HOTELES]
AS
BEGIN
    SELECT id_hotel, nombre_hotel, correo_hotel, telefono_hotel, ciudad_hotel, domicilio_calle_hotel, domicilio_numero_hotel,
		   cantidad_estrellas_hotel, id_pais, fecha_creacion_hotel, recarga_por_estrellas_hotel
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles] 
END

GO

-- Obtiene los roles
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_ROLES]
AS
BEGIN
    SELECT id_rol, nombre_rol, estado_rol
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[roles] 
END

GO

-- Obtiene los hoteles pero de un cierto usuario (solo el ID)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_HOTELES_DE_UN_USUARIO] (@id_usuario INT)
AS
BEGIN
    SELECT id_hotel
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles]
    WHERE id_usuario = @id_usuario
END

GO

-- Obtiene los roles pero solo de un usuario (y solo el ID de estos)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_ROLES_DE_UN_USUARIO] (@id_usuario INT)
AS
BEGIN
    SELECT id_rol
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXroles]
    WHERE id_usuario = @id_usuario
END

GO

-- Devuelve un pais en base a su id
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_PAIS] (@id_pais INT)
AS
BEGIN
    SELECT nombre_pais
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[paises]
    WHERE id_pais = @id_pais
END

GO

-- Trae todos los tipos de documento
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_TIPOS_DOCUMENTO]
AS
BEGIN
    SELECT id_tipo_documento, nombre_tipo_documento, sigla_tipo_documento
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[tipos_documento]
END

GO


CREATE TYPE [EL_MONSTRUO_DEL_LAGO_MASER].[listaDeRoles] AS TABLE
(id_rol INT)

GO

CREATE TYPE [EL_MONSTRUO_DEL_LAGO_MASER].[listaDeHoteles] AS TABLE
(id_hotel INT)

GO

-- Se ejecuta para crear un usuario nuevo
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_USUARIO](@id_rol_user INT, @usuario_cuenta NVARCHAR(255), @contraseña_cuenta CHAR(64), @roles EL_MONSTRUO_DEL_LAGO_MASER.listaDeRoles READONLY, 
@hoteles EL_MONSTRUO_DEL_LAGO_MASER.listaDeHoteles READONLY, @nombre_usuario NVARCHAR(255), @apellido_usuario NVARCHAR(255), @id_tipo_documento INT, @numero_documento_usuario NUMERIC(18,0), @correo_usuario NVARCHAR(255), @telefono_usuario NVARCHAR(100), @direccion_usuario NVARCHAR(255), @fecha_nacimiento_usuario DATETIME)
AS
BEGIN
    DECLARE @id_usuario             INT
    DECLARE @id_rol                 INT
    DECLARE @id_hotel				INT
    
    DECLARE cursor_roles CURSOR FOR
		SELECT id_rol
		FROM @roles

    DECLARE cursor_hoteles CURSOR FOR
        SELECT id_hotel
        FROM @hoteles

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 7

	BEGIN TRY
		BEGIN TRANSACTION
        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios]
            (nombre_usuario, apellido_usuario, id_tipo_documento, numero_documento_usuario, correo_usuario, telefono_usuario, direccion_usuario,fecha_nacimiento_usuario)
		VALUES(@nombre_usuario, @apellido_usuario, @id_tipo_documento, @numero_documento_usuario, @correo_usuario, @telefono_usuario, @direccion_usuario, @fecha_nacimiento_usuario)

        -- Seteamos el ID del USUARIO CREADO
                SET @id_usuario = SCOPE_IDENTITY();

        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas]
                        (id_usuario, usuario_cuenta, contraseña_cuenta)
        VALUES(@id_usuario, LOWER(@usuario_cuenta), LOWER(@contraseña_cuenta))


		-- Vamos insertando los roles del usuario
		OPEN cursor_roles
		FETCH cursor_roles INTO @id_rol
		WHILE(@@FETCH_STATUS = 0) BEGIN
			INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXroles]
				(id_usuario, id_rol)
			VALUES (@id_usuario, @id_rol)
			FETCH cursor_roles INTO @id_rol
		END
		CLOSE cursor_roles
		DEALLOCATE cursor_roles

		-- Vamos insertando los hoteles del usuario
		OPEN cursor_hoteles
		FETCH cursor_hoteles INTO @id_hotel
		WHILE(@@FETCH_STATUS = 0) BEGIN
			INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles]
				(id_usuario, id_hotel)
			VALUES (@id_usuario, @id_hotel)
		FETCH cursor_hoteles INTO @id_hotel
		END

		COMMIT TRANSACTION
		CLOSE cursor_hoteles
		DEALLOCATE cursor_hoteles
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION

		IF CURSOR_STATUS('global', 'cursor_roles') = 1 BEGIN
			CLOSE cursor_roles
		END
		DEALLOCATE cursor_roles;

		IF CURSOR_STATUS('global', 'cursor_hoteles') = 1 BEGIN
			CLOSE cursor_hoteles
		END
		DEALLOCATE cursor_hoteles;

		THROW
	END CATCH
END

GO

-- Obtiene los usuarios filtrados
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_USUARIOS_FILTRADOS]
	(@usuario_cuenta NVARCHAR(255), @id_rol INT, @nombre_usuario NVARCHAR(255), @apellido_usuario NVARCHAR(255),
	@id_tipo_documento INT, @numero_documento_usuario NUMERIC(18,0), @correo_usuario NVARCHAR(255), @id_hotel INT, @soloActivos BIT)
AS
BEGIN
        SELECT DISTINCT u.id_usuario, usuario_cuenta, nombre_usuario, 
			apellido_usuario, id_tipo_documento, numero_documento_usuario, correo_usuario, telefono_usuario, 
			direccion_usuario, fecha_nacimiento_usuario, estado_usuario 
        FROM [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios] u
		JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas] c
		ON u.id_usuario = c.id_usuario
        JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles] uxh
        ON u.id_usuario = uxh.id_usuario
		JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXroles] uxr
		ON u.id_usuario = uxr.id_usuario
        WHERE LOWER(usuario_cuenta) LIKE '%' + LOWER(@usuario_cuenta) + '%'
        AND (@id_rol = -1 OR id_rol = @id_rol)
		AND LOWER(nombre_usuario) LIKE '%' + LOWER(@nombre_usuario) + '%'
		AND LOWER(apellido_usuario) LIKE '%' + LOWER(@apellido_usuario) + '%'
		AND (@id_tipo_documento = -1 OR id_tipo_documento = @id_tipo_documento)
		AND (@numero_documento_usuario = 0 OR CAST(numero_documento_usuario AS VARCHAR) LIKE '%' + CAST(@numero_documento_usuario AS VARCHAR) + '%')
		AND LOWER(correo_usuario) LIKE '%' + LOWER(@correo_usuario) + '%'
		AND (@id_hotel = -1 OR id_hotel = @id_hotel)
        AND (@soloActivos = 0 OR estado_usuario = 1)
END

GO

-- Deshabilita un usuario en particular
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[DESHABILITAR_USUARIO]
        (@id_rol_user INT, @id_usuario INT)
AS
BEGIN

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 9

    UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios]
    SET estado_usuario = 0
    WHERE id_usuario = @id_usuario
END

GO

-- Modifica un usuario en particular
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[MODIFICAR_USUARIO](@id_rol_user INT, @id_usuario INT, @usuario_cuenta NVARCHAR(255), @contraseña_cuenta CHAR(64), @roles EL_MONSTRUO_DEL_LAGO_MASER.listaDeRoles READONLY,
	@hoteles EL_MONSTRUO_DEL_LAGO_MASER.listaDeHoteles READONLY, @nombre_usuario NVARCHAR(255), @apellido_usuario NVARCHAR(255), @id_tipo_documento INT, @numero_documento_usuario NUMERIC(18,0), @correo_usuario NVARCHAR(255), 
	@telefono_usuario NVARCHAR(100), @direccion_usuario NVARCHAR(255), @fecha_nacimiento_usuario DATETIME, @estado BIT)
AS
BEGIN
    DECLARE @id_rol                 INT
    DECLARE @id_hotel               INT

    DECLARE cursor_roles CURSOR FOR
                SELECT id_rol
                FROM @roles

    DECLARE cursor_hoteles CURSOR FOR
        SELECT id_hotel
        FROM @hoteles

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 8

        BEGIN TRY
				BEGIN TRANSACTION
				UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios]
				SET nombre_usuario = @nombre_usuario,
					apellido_usuario = @apellido_usuario,
					id_tipo_documento = @id_tipo_documento,
					numero_documento_usuario = @numero_documento_usuario,
					correo_usuario = @correo_usuario,
					telefono_usuario = @telefono_usuario,
					direccion_usuario = @direccion_usuario,
					fecha_nacimiento_usuario = @fecha_nacimiento_usuario,
					estado_usuario = @estado
				WHERE id_usuario = @id_usuario

				UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas]
				SET usuario_cuenta = LOWER(@usuario_cuenta),
					contraseña_cuenta = CASE @contraseña_cuenta
					WHEN 'e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855' THEN contraseña_cuenta
					ELSE LOWER(@contraseña_cuenta)
					END
				WHERE id_usuario = @id_usuario

				-- Limpiamos relaciones muchos a muchos
				DELETE FROM [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles]
				WHERE id_usuario = @id_usuario

				DELETE FROM [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXroles]
				WHERE id_usuario = @id_usuario

                -- Vamos insertando los roles del usuario
                OPEN cursor_roles
                FETCH cursor_roles INTO @id_rol
                WHILE(@@FETCH_STATUS = 0) BEGIN
                        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXroles]
                                (id_usuario, id_rol)
                        VALUES (@id_usuario, @id_rol)
                        FETCH cursor_roles INTO @id_rol
                END
                CLOSE cursor_roles
                DEALLOCATE cursor_roles
                -- Vamos insertando los hoteles del usuario
                OPEN cursor_hoteles
                FETCH cursor_hoteles INTO @id_hotel
                WHILE(@@FETCH_STATUS = 0) BEGIN
                        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles]
                                (id_usuario, id_hotel)
                        VALUES (@id_usuario, @id_hotel)
                FETCH cursor_hoteles INTO @id_hotel
                END

                COMMIT TRANSACTION
                CLOSE cursor_hoteles
                DEALLOCATE cursor_hoteles
        END TRY
        BEGIN CATCH
                ROLLBACK TRANSACTION

                IF CURSOR_STATUS('global', 'cursor_roles') = 1 BEGIN
                        CLOSE cursor_roles
                END
                DEALLOCATE cursor_roles;

                IF CURSOR_STATUS('global', 'cursor_hoteles') = 1 BEGIN
                        CLOSE cursor_hoteles
                END
                DEALLOCATE cursor_hoteles;

                THROW
        END CATCH
END


GO

