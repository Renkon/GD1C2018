-- Obtiene el listado de funcionalidades
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_FUNCIONALIDADES]
AS
SELECT id_funcionalidad, descripcion_funcionalidad 
FROM [EL_MONSTRUO_DEL_LAGO_MASER].[funcionalidades]

GO

-- Obtiene el ID de un usuario dummy (para los visitantes)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_USUARIO_DUMMY]
AS
	SELECT 1 id_usuario
GO

-- Obtiene el ID del rol guest (que es 1 por nuestra lógicaa)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_ROL_GUEST]
AS
	SELECT 1 id_rol
GO

-- Se ejecuta para crear un rol nuevo
CREATE TYPE listaDeFuncionalidades AS TABLE
(id_funcionalidad INT)

GO

CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_NUEVO_ROL](@nombre_rol NVARCHAR(255), @funcionalidades listaDeFuncionalidades READONLY) 
AS
BEGIN
	DECLARE @id_funcionalidad	INT
	DECLARE @id_rol				INT
	DECLARE cursor_funcionalidades CURSOR FOR 
		SELECT id_funcionalidad
		FROM @funcionalidades

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

	CLOSE cursor_funcionalidades
	DEALLOCATE cursor_funcionalidades
END

GO


