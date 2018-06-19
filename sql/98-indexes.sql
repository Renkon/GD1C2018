-- Indices de cliente
CREATE INDEX index_cliente_nombre ON [EL_MONSTRUO_DEL_LAGO_MASER].[clientes] (nombre_cliente)
CREATE INDEX index_cliente_apellido ON [EL_MONSTRUO_DEL_LAGO_MASER].[clientes] (apellido_cliente)

-- Indices de reservas
CREATE INDEX index_fecha_inicio_reserva ON [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] (fecha_inicio_reserva)
CREATE INDEX index_fecha_inicio_fin_reserva ON [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] (fecha_inicio_reserva, fecha_fin_reserva)

GO
