-- Obtiene los regimenes activos de un hotel
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_REGIMENES_ACTIVOS_DE_HOTEL] (@id_hotel INT)
AS
BEGIN
    SELECT r.id_regimen, descripcion_regimen, precio_base_regimen, estado_regimen
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes] r
	JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[hotelesXregimenes] hXr
	ON r.id_regimen = hXr.id_regimen
	WHERE estado_regimen = 1
	AND id_hotel = @id_hotel
END
