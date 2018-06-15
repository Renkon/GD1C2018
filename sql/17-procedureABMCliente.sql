-- Obtiene la lista de paises
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_PAISES]
AS
BEGIN
    SELECT id_pais, nombre_pais
    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[paises]
END

GO

-- Trae los clientes con los filtros posibles.
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_CLIENTES_FILTRADOS](@nombre nvarchar(255), @apellido nvarchar(255),@id_documento int,
    @numero_documento numeric(18,0), @correo nvarchar(255), @soloActivos BIT)
AS
BEGIN

    SELECT TOP 100 id_cliente, nombre_cliente, apellido_cliente, id_tipo_documento, numero_documento_cliente, correo_cliente, telefono_cliente, domicilio_calle_cliente,
        domicilio_numero_cliente, domicilio_piso_cliente, domicilio_departamento_cliente, ciudad_cliente, id_pais, nacionalidad_cliente, fecha_nacimiento_cliente, estado_cliente    FROM [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
    WHERE LOWER(nombre_cliente) LIKE '%' + LOWER(@nombre) + '%'
    AND LOWER(apellido_cliente) LIKE '%' + LOWER(@apellido) + '%'
	AND LOWER(correo_cliente) LIKE '%' + LOWER(@correo) + '%'
    AND (@id_documento = -1 OR id_tipo_documento = @id_documento)
    AND (@numero_documento = 0 OR CAST (numero_documento_cliente AS VARCHAR) LIKE '%' + CAST (@numero_documento AS VARCHAR) + '%')
    AND (@soloActivos = 0 OR estado_cliente = 1)
END

GO

-- Crea un cliente
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_NUEVO_CLIENTE](@id_rol_user INT,@nombre nvarchar(255), @apellido nvarchar(255),@id_documento int,
    @numero_documento numeric(18,0), @correo nvarchar(255),@telefono nvarchar(100),@domicilio_calle nvarchar(255),@domicilio_numero numeric(18,0),
    @domicilio_piso numeric(18,0), @domicilio_departamento nvarchar(50), @ciudad nvarchar(255), @pais int, @nacionalidad nvarchar(255), @fecha_nacimiento datetime)
AS
BEGIN

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 10

	IF (@domicilio_piso = 0) BEGIN
		SET @domicilio_piso = NULL
	END

    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
    (nombre_cliente, apellido_cliente, id_tipo_documento, numero_documento_cliente, correo_cliente, telefono_cliente, domicilio_calle_cliente, domicilio_numero_cliente,
    domicilio_piso_cliente, domicilio_departamento_cliente, ciudad_cliente, id_pais, nacionalidad_cliente, fecha_nacimiento_cliente)
    VALUES(@nombre, @apellido, @id_documento, @numero_documento, @correo, @telefono, @domicilio_calle, @domicilio_numero, @domicilio_piso, @domicilio_departamento, 
            @ciudad, @pais, @nacionalidad, @fecha_nacimiento)
END

GO

-- Modifica aun cliente
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[MODIFICAR_CLIENTE](@id_rol_user INT,@id_cliente INT,@nombre nvarchar(255), @apellido nvarchar(255),@id_documento int,
    @numero_documento numeric(18,0), @correo nvarchar(255),@telefono nvarchar(100),@domicilio_calle nvarchar(255),@domicilio_numero numeric(18,0),
    @domicilio_piso numeric(18,0), @domicilio_departamento nvarchar(50), @ciudad nvarchar(255), @pais int, @nacionalidad nvarchar(255), @fecha_nacimiento datetime, @estado_cliente BIT)
AS
BEGIN
    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 11
    UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
    SET nombre_cliente = @nombre,apellido_cliente = @apellido,id_tipo_documento = @id_documento,numero_documento_cliente = @numero_documento,
    correo_cliente = @correo,telefono_cliente = @telefono,domicilio_calle_cliente = @domicilio_calle,domicilio_numero_cliente = @domicilio_numero,
    domicilio_piso_cliente = @domicilio_piso,domicilio_departamento_cliente = @domicilio_departamento ,ciudad_cliente = @ciudad ,
    id_pais = @pais,nacionalidad_cliente = @nacionalidad,fecha_nacimiento_cliente =  @fecha_nacimiento,estado_cliente = @estado_cliente
    WHERE id_cliente = @id_cliente 
END

GO

-- Borra un cliente
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[DESHABILITAR_CLIENTE]
        (@id_rol_user INT, @id_cliente INT)
AS
BEGIN

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 12

    UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
    SET estado_cliente = 0
    WHERE id_cliente = @id_cliente
END

GO
