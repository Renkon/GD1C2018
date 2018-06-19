UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles]
SET nombre_hotel = nombre_hotel + ' ' + CAST(id_hotel AS NVARCHAR)

GO
