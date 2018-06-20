ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
ADD FOREIGN KEY (id_tipo_documento) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[tipos_documento](id_tipo_documento);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
ADD FOREIGN KEY (id_pais) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[paises](id_pais);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas]
ADD FOREIGN KEY (id_usuario) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios](id_usuario);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[facturas]
ADD FOREIGN KEY (id_estadia) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[estadias](id_estadia);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[facturas]
ADD FOREIGN KEY (id_forma_de_pago) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[formas_de_pago](id_forma_de_pago);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[consumos]
ADD FOREIGN KEY (id_consumible) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[consumibles](id_consumible);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[consumos]
ADD FOREIGN KEY (id_estadia) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[estadias](id_estadia);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[consumos]
ADD FOREIGN KEY (id_habitacion) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones](id_habitacion);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[clientesXestadias]
ADD FOREIGN KEY (id_cliente) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[clientes](id_cliente);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[clientesXestadias]
ADD FOREIGN KEY (id_estadia) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[estadias](id_estadia);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[cierres_temporales_habitacion]
ADD FOREIGN KEY (id_habitacion) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones](id_habitacion);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[items_factura]
ADD FOREIGN KEY (id_factura) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[facturas](id_factura);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[items_factura]
ADD FOREIGN KEY (id_consumo) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[consumos](id_consumo);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
ADD FOREIGN KEY (id_reserva) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[reservas](id_reserva);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
ADD FOREIGN KEY (id_usuario_ingreso) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios](id_usuario);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[estadias]
ADD FOREIGN KEY (id_usuario_egreso) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios](id_usuario);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones]
ADD FOREIGN KEY (id_hotel) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles](id_hotel);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones]
ADD FOREIGN KEY (id_tipo_habitacion) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[tipos_habitacion](id_tipo_habitacion);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones]
ADD FOREIGN KEY (id_reserva) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[reservas](id_reserva);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones]
ADD FOREIGN KEY (id_habitacion) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones](id_habitacion);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXroles]
ADD FOREIGN KEY (id_rol) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[roles](id_rol);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXroles]
ADD FOREIGN KEY (id_usuario) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios](id_usuario);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[cancelaciones_reserva]
ADD FOREIGN KEY (id_reserva) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[reservas](id_reserva);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[cancelaciones_reserva]
ADD FOREIGN KEY (id_usuario) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios](id_usuario);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
ADD FOREIGN KEY (id_cliente) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[clientes](id_cliente);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
ADD FOREIGN KEY (id_regimen) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes](id_regimen);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
ADD FOREIGN KEY (id_estado_reserva) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[estados_reserva](id_estado_reserva);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles]
ADD FOREIGN KEY (id_usuario) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios](id_usuario);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles]
ADD FOREIGN KEY (id_hotel) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles](id_hotel);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[hotelesXregimenes]
ADD FOREIGN KEY (id_hotel) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles](id_hotel);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[hotelesXregimenes]
ADD FOREIGN KEY (id_regimen) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes](id_regimen);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles]
ADD FOREIGN KEY (id_pais) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[paises](id_pais);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[rolesXfuncionalidades]
ADD FOREIGN KEY (id_rol) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[roles](id_rol);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[rolesXfuncionalidades]
ADD FOREIGN KEY (id_funcionalidad) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[funcionalidades](id_funcionalidad);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[cierres_temporales_hotel]
ADD FOREIGN KEY (id_hotel) REFERENCES [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles](id_hotel);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[generacion_modificacion_reservas]
ADD FOREIGN KEY (id_reserva) REFERENCES [reservas](id_reserva);

ALTER TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[generacion_modificacion_reservas]
ADD FOREIGN KEY (id_usuario) REFERENCES [usuarios](id_usuario);

