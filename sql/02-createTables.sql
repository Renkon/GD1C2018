-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[clientes] -- 1
(
    id_cliente                      INT IDENTITY(1, 1) PRIMARY KEY,
    nombre_cliente                  NVARCHAR(255),
    apellido_cliente                NVARCHAR(255),
    id_tipo_documento               INT NOT NULL DEFAULT 6,
    numero_documento_cliente        NUMERIC(18,0) NOT NULL,
    correo_cliente                  NVARCHAR(255) NOT NULL UNIQUE,
    telefono_cliente                NVARCHAR(100),
    domicilio_calle_cliente         NVARCHAR(255),
    domicilio_numero_cliente        NUMERIC(18,0),
    domicilio_piso_cliente          NUMERIC(18,0),
    domicilio_departamento_cliente  NVARCHAR(50),
    ciudad_cliente                  NVARCHAR(255),
    id_pais                         INT,
    nacionalidad_cliente            NVARCHAR(20),
    fecha_nacimiento_cliente        DATETIME
    
    UNIQUE(id_tipo_documento, numero_documento_cliente)
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas] -- 2
(    
    id_usuario                 INT NOT NULL PRIMARY KEY,
    usuario_cuenta             NVARCHAR(255) NOT NULL UNIQUE,
    contraseña_cuenta          CHAR(64) NOT NULL,
    intentos_acceso_cuenta     NUMERIC(1,0) DEFAULT 0
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[tipos_documento] -- 3
(
    id_tipo_documento        INT IDENTITY(1, 1) PRIMARY KEY,
    nombre_tipo_documento    NVARCHAR(60) NOT NULL,
    sigla_tipo_documento     CHAR(3) NOT NULL
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[facturas] -- 4
(
    id_factura            INT IDENTITY(2396745, 1) PRIMARY KEY,
    fecha_factura         DATETIME NOT NULL,
    total_factura         NUMERIC(18,2),
    id_estadia            INT NOT NULL,
    id_forma_de_pago      INT NOT NULL,
    detalle_pago          NVARCHAR(1000)
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[formas_de_pago] -- 5
(
    id_forma_de_pago            INT IDENTITY(1, 1) PRIMARY KEY,
    descripcion_forma_de_pago   NVARCHAR(255) NOT NULL
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[consumos] -- 6
(
    id_consumo            INT IDENTITY (1, 1) PRIMARY KEY,
    id_consumible         INT NOT NULL,
    id_estadia            INT NOT NULL,
    id_habitacion         INT NOT NULL,
    fecha_hora_consumo    DATETIME
);


-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[clientesXestadias] -- 7
(
    id_cliente INT NOT NULL,
    id_estadia INT NOT NULL
    
    PRIMARY KEY (id_cliente, id_estadia)
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[cierres_temporales_habitacion] -- 8
(
    id_cierre_temporales_habitacion            INT IDENTITY (1, 1) PRIMARY KEY,
    fecha_inicio_cierre_temporal_habitacion    DATETIME NOT NULL,
    fecha_fin_cierre_temporal_habitacion       DATETIME NOT NULL,
    id_habitacion                              INT NOT NULL,
    motivo_cierre_temporal_habitacion          NVARCHAR(2500)
);


-------------------------------------------------------------------------------------------------------------------------------------------
 CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[items_factura] -- 9
(
    id_item_factura                INT IDENTITY (1, 1) PRIMARY KEY,
    id_factura                     INT NOT NULL,
    id_consumo                     INT NULL,
    precio_unitario_item_factura   NUMERIC(18,2) NOT NULL,
    descripcion_item_factura       NVARCHAR(255) NOT NULL DEFAULT 'Desconocido',
    cantidad_item_factura          NUMERIC(18,0) NOT NULL
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[consumibles] -- 10
(
    id_consumible            INT IDENTITY (2324, 1) PRIMARY KEY,
    precio_consumible        NUMERIC(18,2) NOT NULL,
    descripcion_consumible   NVARCHAR(255) NOT NULL
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[estadias] -- 11
(
    id_estadia                INT IDENTITY (1, 1) PRIMARY KEY,
    id_reserva                INT NOT NULL,
    id_usuario                INT NOT NULL,
    fecha_ingreso_estadia     DATETIME NOT NULL,
    fecha_egreso_estadia      DATETIME NOT NULL
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[estadiasXhabitaciones] -- 12
(
    id_estadia        INT NOT NULL,
    id_habitacion     INT NOT NULL
    
    PRIMARY KEY (id_estadia, id_habitacion)
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[habitaciones] -- 13
(
    id_habitacion            INT IDENTITY (1, 1) PRIMARY KEY,
    id_hotel                 INT NOT NULL,
    numero_habitacion        NUMERIC(18,0) NOT NULL,
    piso_habitacion          NUMERIC(18,0),
    ubicacion_habitacion     NVARCHAR(50),
    id_tipo_habitacion       INT NOT NULL,
    descripcion_habitacion   NVARCHAR(255)
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios] -- 14
(
    id_usuario                    INT IDENTITY (1, 1) PRIMARY KEY,
    nombre_usuario                NVARCHAR(255) NOT NULL,
    apellido_usuario              NVARCHAR(255) NOT NULL,
    id_tipo_documento             INT NOT NULL,
    numero_documento_usuario      NUMERIC(18,0) NOT NULL,
    correo_usuario                NVARCHAR(255) NOT NULL,
    telefono_usuario              NVARCHAR(100) NOT NULL,
    direccion_usuario             NVARCHAR(255) NOT NULL,
    fecha_nacimiento_usuario      DATETIME NOT NULL,
    estado_usuario                BIT NOT NULL DEFAULT 1
    
    UNIQUE(id_tipo_documento, numero_documento_usuario)
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[reservasXhabitaciones] -- 15
(
    id_reserva        INT NOT NULL,
    id_habitacion     INT NOT NULL
    
    PRIMARY KEY(id_reserva, id_habitacion)
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[tipos_habitacion] -- 16
(
    id_tipo_habitacion                    INT IDENTITY (1001, 1) PRIMARY KEY,
    descripcion_tipo_habitacion           NVARCHAR(255),
    porcentual_tipo_habitacion            NUMERIC(18,2) NOT NULL,
    cantidad_huespedes_tipo_habitacion    NUMERIC(18,0) NOT NULL
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXroles] -- 17
(
    id_rol        INT NOT NULL,
    id_usuario    INT NOT NULL
    
    PRIMARY KEY(id_rol, id_usuario)
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[cancelaciones_reserva] -- 18
(
    id_cancelacion_reserva        INT IDENTITY (1, 1) PRIMARY KEY,
    id_reserva                    INT NOT NULL,
    motivo_cancelacion_reserva    NVARCHAR(2500),
    fecha_cancelacion_reserva     DATETIME NOT NULL,
    id_usuario                    INT NOT NULL
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas] -- 19
(
    id_reserva                    INT IDENTITY (10001, 1) PRIMARY KEY,
    fecha_realizacion_reserva     DATETIME NOT NULL,
    fecha_inicio_reserva          DATETIME NOT NULL,
    fecha_fin_reserva             DATETIME NOT NULL,
    id_usuario                    INT NOT NULL,
    id_regimen                    INT NOT NULL,
    id_estado_reserva             INT NOT NULL DEFAULT 1
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[estados_reserva] -- 20
(
    id_estado_reserva                INT IDENTITY (1, 1) PRIMARY KEY,
    descripcion_estados_reserva      NVARCHAR(255) NOT NULL
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles] -- 21
(
    id_usuario    INT NOT NULL,
    id_hotel      INT NOT NULL
    
    PRIMARY KEY (id_usuario, id_hotel)
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[paises] -- 22
(
    id_pais        INT IDENTITY (1, 1) PRIMARY KEY,
    nombre_pais    NVARCHAR(255) NOT NULL
);

-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[roles] -- 23
(
    id_rol        INT IDENTITY (1, 1) PRIMARY KEY,
    nombre_rol    NVARCHAR(255) NOT NULL,
    estado_rol    BIT NOT NULL DEFAULT 1
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[regimenes] -- 24
(
    id_regimen            INT IDENTITY (1, 1) PRIMARY KEY,
    descripcion_regimen   NVARCHAR(255) NOT NULL,
    precio_base_regimen   NUMERIC(18,2) NOT NULL,
    estado_regimen        BIT NOT NULL DEFAULT 1
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[hotelesXregimenes] -- 25
(
    id_hotel        INT NOT NULL,
    id_regimen      INT NOT NULL
    
    PRIMARY KEY (id_hotel, id_regimen)
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles] -- 26
(
    id_hotel                        INT IDENTITY (1, 1) PRIMARY KEY,
    nombre_hotel                    NVARCHAR(255) NOT NULL DEFAULT 'Hotel sin nombre',
    correo_hotel                    NVARCHAR(255) NOT NULL DEFAULT 'hotel@emdlm.com',
    telefono_hotel                  NVARCHAR(100) NOT NULL DEFAULT '0800-000-0000',
    ciudad_hotel                    NVARCHAR(255) NOT NULL,
    domicilio_calle_hotel           NVARCHAR(255) NOT NULL,
    domicilio_numero_hotel          NUMERIC(18,0) NOT NULL,
    cantidad_estrellas_hotel        NUMERIC(18,0) NOT NULL,
    id_pais                         INT NOT NULL DEFAULT 10,
    fecha_creacion_hotel            DATETIME NOT NULL DEFAULT '2000-01-01',
    recarga_por_estrellas_hotel     NUMERIC(18,0) NOT NULL
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[rolesXfuncionalidades] -- 27
(
    id_rol                INT NOT NULL,
    id_funcionalidad      INT NOT NULL
    
    PRIMARY KEY (id_rol, id_funcionalidad)
);

-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[funcionalidades] -- 28
(
    id_funcionalidad            INT IDENTITY (1, 1) PRIMARY KEY,
    descripcion_funcionalidad   NVARCHAR(255) NOT NULL
);


-------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[cierres_temporales_hotel] -- 29
(
    id_cierre_temporal_hotel            INT IDENTITY (1, 1) PRIMARY KEY,
    fecha_inicio_cierre_temporal_hotel  DATETIME NOT NULL,
    fecha_fin_cierre_temporal_hotel     DATETIME NOT NULL,
    id_hotel                            INT NOT NULL,
    motivo_cierre_temporal_hotel        NVARCHAR(2500)
);


-------------------------------------------------------------------------------------------------------------------------------------------


CREATE TABLE [EL_MONSTRUO_DEL_LAGO_MASER].[migracion_errores](
	[Error_Mensaje] [nvarchar](4000) NULL,
	[Hotel_Ciudad] [nvarchar](255) NULL,
	[Hotel_Calle] [nvarchar](255) NULL,
	[Hotel_Nro_Calle] [numeric](18, 0) NULL,
	[Hotel_CantEstrella] [numeric](18, 0) NULL,
	[Hotel_Recarga_Estrella] [numeric](18, 0) NULL,
	[Habitacion_Numero] [numeric](18, 0) NULL,
	[Habitacion_Piso] [numeric](18, 0) NULL,
	[Habitacion_Frente] [nvarchar](50) NULL,
	[Habitacion_Tipo_Codigo] [numeric](18, 0) NULL,
	[Habitacion_Tipo_Descripcion] [nvarchar](255) NULL,
	[Habitacion_Tipo_Porcentual] [numeric](18, 2) NULL,
	[Regimen_Descripcion] [nvarchar](255) NULL,
	[Regimen_Precio] [numeric](18, 2) NULL,
	[Reserva_Fecha_Inicio] [datetime] NULL,
	[Reserva_Codigo] [numeric](18, 0) NULL,
	[Reserva_Cant_Noches] [numeric](18, 0) NULL,
	[Estadia_Fecha_Inicio] [datetime] NULL,
	[Estadia_Cant_Noches] [numeric](18, 0) NULL,
	[Consumible_Codigo] [numeric](18, 0) NULL,
	[Consumible_Descripcion] [nvarchar](255) NULL,
	[Consumible_Precio] [numeric](18, 2) NULL,
	[Item_Factura_Cantidad] [numeric](18, 0) NULL,
	[Item_Factura_Monto] [numeric](18, 2) NULL,
	[Factura_Nro] [numeric](18, 0) NULL,
	[Factura_Fecha] [datetime] NULL,
	[Factura_Total] [numeric](18, 2) NULL,
	[Cliente_Pasaporte_Nro] [numeric](18, 0) NULL,
	[Cliente_Apellido] [nvarchar](255) NULL,
	[Cliente_Nombre] [nvarchar](255) NULL,
	[Cliente_Fecha_Nac] [datetime] NULL,
	[Cliente_Mail] [nvarchar](255) NULL,
	[Cliente_Dom_Calle] [nvarchar](255) NULL,
	[Cliente_Nro_Calle] [numeric](18, 0) NULL,
	[Cliente_Piso] [numeric](18, 0) NULL,
	[Cliente_Depto] [nvarchar](50) NULL,
	[Cliente_Nacionalidad] [nvarchar](255) NULL
);

GO

