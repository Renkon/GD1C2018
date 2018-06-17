
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
CREATE TYPE [EL_MONSTRUO_DEL_LAGO_MASER].[listaDeFuncionalidades] AS TABLE
	(id_funcionalidad INT)

GO

CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_NUEVO_ROL](@id_rol_user INT, @nombre_rol NVARCHAR(255), @funcionalidades EL_MONSTRUO_DEL_LAGO_MASER.listaDeFuncionalidades READONLY)
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
        (@id_rol_user INT, @id_rol INT, @nombre_rol NVARCHAR(255), @funcionalidades EL_MONSTRUO_DEL_LAGO_MASER.listaDeFuncionalidades READONLY, @estado BIT)
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

