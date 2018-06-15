-- Usado para hacer el login
CREATE PROCEDURE [EL_MONSTRUO_DEL_LAGO_MASER].[LOGIN_USUARIO]
	(@usuario_cuenta NVARCHAR(255), @contraseña_cuenta CHAR(64))
AS
BEGIN
	DECLARE @id_usuario		INT
	DECLARE @pass_real		CHAR(64)
	DECLARE @intentos		NUMERIC(1,0)
	DECLARE @estado			BIT

	-- Valida si existe el usuario con ese numero
	SELECT @id_usuario = c.id_usuario, @pass_real = contraseña_cuenta, 
		   @intentos = intentos_acceso_cuenta, @estado = estado_usuario
	FROM [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas] c
	JOIN [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios] u
	ON c.id_usuario = u.id_usuario
	WHERE usuario_cuenta = LOWER(@usuario_cuenta) -- es unique key, devuelve unica siempre

	IF (@id_usuario IS NULL) BEGIN
		SELECT -3 id_usuario -- No hay usuario (devuelvo un user con -3)
	END
	ELSE IF (LOWER(@contraseña_cuenta) != LOWER(@pass_real)) BEGIN
		SELECT -1 id_usuario -- Credenciales inválidas (devuelvo un user con -1)

		IF (@intentos = 2) BEGIN -- Este fue el tercer intento (el 2 era el guardado previamente)
			UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios]
			SET estado_usuario = 0
			WHERE id_usuario = @id_usuario

			UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas]
			SET intentos_acceso_cuenta = 0
			WHERE id_usuario = @id_usuario

			SET @estado = 0
		END

		-- Aumentamos la cantidad del contador (si sigue aactivo)
		IF (@estado = 1) BEGIN
			UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas]
			SET intentos_acceso_cuenta = @intentos + 1
			WHERE id_usuario = @id_usuario
		END
	END
	ELSE IF (@estado = 0) BEGIN
		SELECT -2 id_usuario -- Usuario deshabilitado (devuelvo un user con -2)
	END
	ELSE BEGIN
		SELECT id_usuario, nombre_usuario, apellido_usuario, id_tipo_documento, numero_documento_usuario,
			   correo_usuario, telefono_usuario, direccion_usuario, fecha_nacimiento_usuario, estado_usuario
		FROM [EL_MONSTRUO_DEL_LAGO_MASER].[usuarios]
		WHERE id_usuario = @id_usuario

		-- Reinicio el contador
		UPDATE [EL_MONSTRUO_DEL_LAGO_MASER].[cuentas]
		SET intentos_acceso_cuenta = 0
		WHERE id_usuario = @id_usuario
	END
END

GO

