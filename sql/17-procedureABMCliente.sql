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
    AND id_cliente != 0
END

GO

-- Crea un cliente (sin validar permisos)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_NUEVO_CLIENTE_SIN_VALIDACION] (@nombre nvarchar(255), @apellido nvarchar(255), @id_documento int,
    @numero_documento numeric(18,0), @correo nvarchar(255),@telefono nvarchar(100),@domicilio_calle nvarchar(255),@domicilio_numero numeric(18,0),
    @domicilio_piso numeric(18,0), @domicilio_departamento nvarchar(50), @ciudad nvarchar(255), @pais int, @nacionalidad nvarchar(255), @fecha_nacimiento datetime)
AS
BEGIN
    IF (@domicilio_piso = 0) BEGIN
        SET @domicilio_piso = NULL
    END

    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[clientes]
        (nombre_cliente, apellido_cliente, id_tipo_documento, numero_documento_cliente, correo_cliente, telefono_cliente, domicilio_calle_cliente, domicilio_numero_cliente,
        domicilio_piso_cliente, domicilio_departamento_cliente, ciudad_cliente, id_pais, nacionalidad_cliente, fecha_nacimiento_cliente)
    VALUES(@nombre, @apellido, @id_documento, @numero_documento, @correo, @telefono, @domicilio_calle, @domicilio_numero, @domicilio_piso, @domicilio_departamento,
        @ciudad, @pais, @nacionalidad, @fecha_nacimiento)

    SELECT SCOPE_IDENTITY() id_cliente
END

GO

-- Crea un cliente (con validación)
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_NUEVO_CLIENTE](@id_rol_user INT, @nombre nvarchar(255), @apellido nvarchar(255), @id_documento int,
    @numero_documento numeric(18,0), @correo nvarchar(255),@telefono nvarchar(100),@domicilio_calle nvarchar(255),@domicilio_numero numeric(18,0),
    @domicilio_piso numeric(18,0), @domicilio_departamento nvarchar(50), @ciudad nvarchar(255), @pais int, @nacionalidad nvarchar(255), @fecha_nacimiento datetime)
AS
BEGIN

    EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[VALIDAR_ROL_USUARIO] @id_rol_user, 10

	EXEC [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_NUEVO_CLIENTE_SIN_VALIDACION] 
	    @nombre, @apellido, @id_documento, @numero_documento, @correo, @telefono, @domicilio_calle ,@domicilio_numero,
        @domicilio_piso, @domicilio_departamento, @ciudad, @pais, @nacionalidad, @fecha_nacimiento
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

-- Filtra y obtiene los clientes de nuestro sistema, y los del sistema anterior
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[OBTENER_CLIENTES_COMPLETOS_FILTRADOS]
    (@id_documento int, @numero_documento numeric(18,0), @correo nvarchar(255))
AS
BEGIN
    SELECT TOP 100 id_cliente, nombre_cliente, apellido_cliente, id_tipo_documento, numero_documento_cliente, correo_cliente, telefono_cliente, domicilio_calle_cliente,
        domicilio_numero_cliente, domicilio_piso_cliente, domicilio_departamento_cliente, ciudad_cliente, id_pais, nacionalidad_cliente, fecha_nacimiento_cliente, estado_cliente 
	FROM [EL_MONSTRUO_DEL_LAGO_MASER].[clientes_completos]
    WHERE LOWER(correo_cliente) LIKE '%' + LOWER(@correo) + '%'
    AND (@id_documento = -1 OR id_tipo_documento = @id_documento)
    AND (@numero_documento = 0 OR CAST (numero_documento_cliente AS VARCHAR) LIKE '%' + CAST (@numero_documento AS VARCHAR) + '%')
    AND id_cliente != 0
END

GO

-- Este procedure se usa cuando se re-registra un cliente existente previo a la migración
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[INSERTAR_CLIENTE_PREEXISTENTE]
    (@nombre_cliente NVARCHAR(255), @apellido_cliente NVARCHAR(255), @id_tipo_documento INT,
        @numero_documento_cliente NUMERIC(18,0), @correo_cliente NVARCHAR(255), @telefono_cliente NVARCHAR(100),
        @domicilio_calle_cliente NVARCHAR(255), @domicilio_numero_cliente NUMERIC(18,0), @domicilio_piso_cliente NUMERIC(18,0),
        @domicilio_departamento_cliente NVARCHAR(50), @ciudad_cliente NVARCHAR(255), @id_pais INT,
        @nacionalidad_cliente NVARCHAR(20), @fecha_nacimiento_cliente DATETIME)
AS
BEGIN
    DECLARE @id_reserva INT
        DECLARE @id_estadia INT
        DECLARE @id_cliente INT

    DECLARE cursor_clientes_preexistentes CURSOR FOR
        SELECT id_estadia, id_reserva
        FROM [EL_MONSTRUO_DEL_LAGO_MASER].[migracion_errores]
        WHERE apellido_cliente = @apellido_cliente
        AND domicilio_departamento_cliente = @domicilio_departamento_cliente
        AND domicilio_calle_cliente = @domicilio_calle_cliente
        AND fecha_nacimiento_cliente = @fecha_nacimiento_cliente
        AND nacionalidad_cliente = @nacionalidad_cliente
        AND nombre_cliente = @nombre_cliente
        AND domicilio_numero_cliente = @domicilio_numero_cliente
        AND domicilio_piso_cliente = @domicilio_piso_cliente

    BEGIN TRY
            BEGIN TRANSACTION

        -- Hacemos el insert a la tabla de clientes primero
        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[clientes] (nombre_cliente, apellido_cliente, id_tipo_documento,
            numero_documento_cliente, correo_cliente, telefono_cliente, domicilio_calle_cliente, domicilio_numero_cliente,
            domicilio_piso_cliente, domicilio_departamento_cliente, ciudad_cliente, id_pais, nacionalidad_cliente,
            fecha_nacimiento_cliente)
        VALUES (@nombre_cliente, @apellido_cliente, @id_tipo_documento, @numero_documento_cliente, @correo_cliente,
            @telefono_cliente, @domicilio_calle_cliente, @domicilio_numero_cliente, @domicilio_piso_cliente,
            @domicilio_departamento_cliente, @ciudad_cliente, @id_pais, @nacionalidad_cliente, @fecha_nacimiento_cliente)

        SET @id_cliente = SCOPE_IDENTITY();

        OPEN cursor_clientes_preexistentes
        FETCH cursor_clientes_preexistentes INTO @id_estadia, @id_reserva

        WHILE (@@FETCH_STATUS = 0) BEGIN
                    IF (@id_reserva IS NOT NULL) BEGIN
                UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[reservas]
                SET id_cliente = @id_cliente
                WHERE id_reserva = @id_reserva
            END
            IF (@id_estadia IS NOT NULL
                AND NOT EXISTS (SELECT *
                                FROM [EL_MONSTRUO_DEL_LAGO_MASER].[clientesXestadias]
                                WHERE id_cliente = @id_cliente
                                AND id_estadia = @id_estadia)) BEGIN
                INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[clientesXestadias]
                    (id_cliente, id_estadia)
                VALUES (@id_cliente, @id_estadia)
            END

            DELETE FROM [EL_MONSTRUO_DEL_LAGO_MASER].[migracion_errores]
            WHERE CURRENT OF cursor_clientes_preexistentes

            FETCH cursor_clientes_preexistentes INTO @id_estadia, @id_reserva
        END

        SELECT @id_cliente

        COMMIT TRANSACTION
        CLOSE cursor_clientes_preexistentes
        DEALLOCATE cursor_clientes_preexistentes
    END TRY
    BEGIN CATCH
            ROLLBACK TRANSACTION

        IF CURSOR_STATUS('global', 'cursor_clientes_preexistentes') = 1 BEGIN
            CLOSE cursor_clientes_preexistentes
        END
        DEALLOCATE cursor_clientes_preexistentes;

                THROW
    END CATCH
END

GO

