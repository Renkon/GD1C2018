-- Obtiene el listado de funcionalidades
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_FUNCIONALIDADES]
AS
SELECT id_funcionalidad, descripcion_funcionalidad 
FROM [EL_MONSTRUO_DEL_LAGO_MASER].[funcionalidades]

GO

