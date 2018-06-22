-- Si ya está facturado, es porque ya cerro los consumos.
UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
SET consumos_cerrados = 1
WHERE id_estadia IN (SELECT DISTINCT id_estadia 
                     FROM [EL_MONSTRUO_DEL_LAGO_MASER].[facturas])
