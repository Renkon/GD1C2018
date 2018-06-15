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

