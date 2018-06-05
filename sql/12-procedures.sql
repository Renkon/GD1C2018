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
