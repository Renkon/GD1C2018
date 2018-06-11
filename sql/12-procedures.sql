-- Procedure de seguridad q explota si no tenes permisos de ejecutar eso
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO]
	(@id_rol INT, @id_funcionalidad INT)
AS
BEGIN
	IF (NOT EXISTS(SELECT id_rol, id_funcionalidad
				   FROM [EL_MONSTRUO_DEL_LAGO_MASER].[rolesXfuncionalidades]
				   WHERE id_rol = @id_rol AND id_funcionalidad = @id_funcionalidad))
	BEGIN
		RAISERROR('Su rol activo no tiene los permisos correspondientes', 20, 1) WITH LOG
	END
END

GO

-- Obtiene el listado de funcionalidades
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_FUNCIONALIDADES]
AS
BEGIN
	SELECT id_funcionalidad, descripcion_funcionalidad
	FROM [EL_MONSTRUO_DEL_LAGO_MASER].[funcionalidades]
END

GO

-- Obtiene el ID de un usuario dummy (para los visitantes)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_USUARIO_DUMMY]
AS
BEGIN
    SELECT 1 id_usuario
END

GO

-- Obtiene el ID del rol guest (que es 1 por nuestra lógicaa)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_ROL_GUEST]
AS
BEGIN
    SELECT 1 id_rol
END

GO

-- Se ejecuta para crear un rol nuevo
CREATE TYPE listaDeFuncionalidades AS TABLE
	(id_funcionalidad INT)

GO

CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_NUEVO_ROL](@id_rol_user INT, @nombre_rol NVARCHAR(255), @funcionalidades listaDeFuncionalidades READONLY)
AS
BEGIN

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 4

	BEGIN TRY
		DECLARE @id_funcionalidad       INT
		DECLARE @id_rol                 INT
		DECLARE cursor_funcionalidades CURSOR FOR
				SELECT id_funcionalidad
				FROM @funcionalidades

		BEGIN TRANSACTION

		INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[roles]
			(nombre_rol)
		VALUES(@nombre_rol)

		-- Seteamos el ID del ROL CREADO
		SET @id_rol = SCOPE_IDENTITY();

		-- Vamos insertando en la muchos a muchos para agregarle funcionalidades
		OPEN cursor_funcionalidades
		FETCH cursor_funcionalidades INTO @id_funcionalidad
		WHILE(@@FETCH_STATUS = 0)
		BEGIN
			INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[rolesXfuncionalidades]
				(id_rol,id_funcionalidad)
			VALUES (@id_rol,@id_funcionalidad)
		FETCH cursor_funcionalidades INTO @id_funcionalidad
		END

		COMMIT TRANSACTION

		CLOSE cursor_funcionalidades
		DEALLOCATE cursor_funcionalidades
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION

		IF CURSOR_STATUS('global', 'cursor_funcionalidades') = 1 BEGIN
			CLOSE cursor_funcionalidades
		END
		DEALLOCATE cursor_funcionalidades;

		THROW
	END CATCH
END

GO

-- Se ejecuta para traer los roles
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_ROLES_FILTRADOS]
(@nombre NVARCHAR(255), @funcionalidad INT, @soloActivos BIT)
AS
BEGIN
        SELECT DISTINCT r.id_rol, nombre_rol, estado_rol
        FROM [EL_MONSTRUO_DEL_LAGO_MASER].[roles] r
        JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[rolesXfuncionalidades] rxf
        ON r.id_rol = rxf.id_rol
        WHERE LOWER(nombre_rol) LIKE '%' + LOWER(@nombre) + '%'
        AND (@funcionalidad = -1 OR id_funcionalidad = @funcionalidad)
        -- Azucar sintáctico para no tener que crear dos selects distintos
        -- Uno con filtro y otro sin filtro
        AND (@soloActivos = 0 OR estado_rol = 1)
        -- Lo mismo para roles, para que traiga solo los activos, o todos
END

GO

-- Obtiene las funcionalidades de un rol en particular
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_FUNCIONALIDADES_DE_ROL]
        (@id_rol INT)
AS
BEGIN
        SELECT id_funcionalidad
        FROM [EL_MONSTRUO_DEL_LAGO_MASER].[rolesXfuncionalidades]
        WHERE id_rol = @id_rol
END

GO

-- Deshabilita un rol en particular
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[DESHABILITAR_ROL]
        (@id_rol_user INT, @id_rol INT)
AS
BEGIN
	
	EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 6
	
    UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[roles]
    SET estado_rol = 0
    WHERE id_rol = @id_rol
END

GO

-- Modifica un rol por su ID
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[MODIFICAR_ROL]
        (@id_rol_user INT, @id_rol INT, @nombre_rol NVARCHAR(255), @funcionalidades listaDeFuncionalidades READONLY, @estado BIT)
AS
BEGIN

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 5

    DECLARE @id_funcionalidad    INT

    DECLARE cursor_funcionalidades CURSOR FOR
        SELECT id_funcionalidad
        FROM @funcionalidades
	
	BEGIN TRY
		BEGIN TRANSACTION
		--Updateamos el rol
		UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[roles]
			SET nombre_rol = @nombre_rol, estado_rol = @estado
			WHERE id_rol = @id_rol

		--Borramos todas las funcoinalidades de ese rol
		DELETE FROM [EL_MONSTRUO_DEL_LAGO_MASER].[rolesXfuncionalidades]
		WHERE id_rol = @id_rol

		-- Insertamos todas las funcoinalidades que nos mandan en la lista
		OPEN cursor_funcionalidades
		FETCH cursor_funcionalidades INTO @id_funcionalidad
			WHILE(@@FETCH_STATUS = 0)
			BEGIN
				INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[rolesXfuncionalidades]
					(id_rol,id_funcionalidad)
				VALUES (@id_rol, @id_funcionalidad)
							FETCH cursor_funcionalidades INTO @id_funcionalidad
			END

		COMMIT TRANSACTION
		CLOSE cursor_funcionalidades
		DEALLOCATE cursor_funcionalidades
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION

		IF CURSOR_STATUS('global', 'cursor_funcionalidades') = 1 BEGIN
			CLOSE cursor_funcionalidades
		END
		DEALLOCATE cursor_funcionalidades;

		THROW
	END CATCH
END

GO

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


CREATE TYPE listaDeRoles AS TABLE
(id_rol INT)

GO

CREATE TYPE listaDeHoteles AS TABLE
(id_hotel INT)

GO

-- Se ejecuta para crear un usuario nuevo
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_USUARIO](@id_rol_user INT, @usuario_cuenta NVARCHAR(255), @contraseña_cuenta CHAR(64), @roles listaDeRoles READONLY, 
@hoteles listaDeHoteles READONLY, @nombre_usuario NVARCHAR(255), @apellido_usuario NVARCHAR(255), @id_tipo_documento INT, @numero_documento_usuario NUMERIC(18,0), @correo_usuario NVARCHAR(255), @telefono_usuario NVARCHAR(100), @direccion_usuario NVARCHAR(255), @fecha_nacimiento_usuario DATETIME)
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
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[MODIFICAR_USUARIO](@id_rol_user INT, @id_usuario INT, @usuario_cuenta NVARCHAR(255), @contraseña_cuenta CHAR(64), @roles listaDeRoles READONLY,
	@hoteles listaDeHoteles READONLY, @nombre_usuario NVARCHAR(255), @apellido_usuario NVARCHAR(255), @id_tipo_documento INT, @numero_documento_usuario NUMERIC(18,0), @correo_usuario NVARCHAR(255), 
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

-- Usado para hacer el login
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[LOGIN_USUARIO]
	(@usuario_cuenta NVARCHAR(255), @contraseña_cuenta CHAR(64))
AS
BEGIN
	DECLARE @id_usuario		INT
	DECLARE @pass_real		CHAR(64)
	DECLARE @intentos		NUMERIC(1,0)
	DECLARE @estado			BIT

	-- Valida si existe el usuario con ese numero
	SELECT @id_usuario = c.id_usuario, @pass_real = contraseña_cuenta, 
		   @intentos = intentos_acceso_cuenta, @estado = estado_usuario
	FROM [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas] c
	JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios] u
	ON c.id_usuario = u.id_usuario
	WHERE usuario_cuenta = LOWER(@usuario_cuenta) -- es unique key, devuelve unica siempre

	IF (@id_usuario IS NULL) BEGIN
		SELECT -3 id_usuario -- No hay usuario (devuelvo un user con -3)
	END
	ELSE IF (LOWER(@contraseña_cuenta) != LOWER(@pass_real)) BEGIN
		SELECT -1 id_usuario -- Credenciales inválidas (devuelvo un user con -1)

		IF (@intentos = 2) BEGIN -- Este fue el tercer intento (el 2 era el guardado previamente)
			UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios]
			SET estado_usuario = 0
			WHERE id_usuario = @id_usuario

			UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas]
			SET intentos_acceso_cuenta = 0
			WHERE id_usuario = @id_usuario

			SET @estado = 0
		END

		-- Aumentamos la cantidad del contador (si sigue aactivo)
		IF (@estado = 1) BEGIN
			UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas]
			SET intentos_acceso_cuenta = @intentos + 1
			WHERE id_usuario = @id_usuario
		END
	END
	ELSE IF (@estado = 0) BEGIN
		SELECT -2 id_usuario -- Usuario deshabilitado (devuelvo un user con -2)
	END
	ELSE BEGIN
		SELECT id_usuario, nombre_usuario, apellido_usuario, id_tipo_documento, numero_documento_usuario,
			   correo_usuario, telefono_usuario, direccion_usuario, fecha_nacimiento_usuario, estado_usuario
		FROM [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios]
		WHERE id_usuario = @id_usuario

		-- Reinicio el contador
		UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas]
		SET intentos_acceso_cuenta = 0
		WHERE id_usuario = @id_usuario
	END
END

GO



