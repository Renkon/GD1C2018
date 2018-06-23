INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios] VALUES ('dummy','dummy', 6, 0, 'dummy@dummy.com', '', '', convert(datetime, '2001-01-01', 105), 1);
INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios] VALUES ('Administrador','General', 6, 1, 'admin@admin.com', '', '', convert(datetime, '2001-01-01', 105), 1);
INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas] VALUES (2, 'admin', 'e6b87050bfcb8143fcb8db0170a4dc9ed00d904ddd3e2a4ad1b1e8dc0fdc9be7', 0);
INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[clientes] (nombre_cliente, apellido_cliente, id_tipo_documento, numero_documento_cliente, correo_cliente)
VALUES ('Cliente', 'Dummy', 6, -1, 'clienteDummy@emdlm.com');

GO

