CREATE TRIGGER [EL_MONSTRUO_DEL_LAGO_MASER].[add_users_for_new_hotel] ON [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles]
AFTER INSERT AS
BEGIN
        DECLARE @id_hotel INT;
        SELECT @id_hotel = id_hotel FROM inserted;

        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles]
        VALUES (1, @id_hotel);

        INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles]
        VALUES (2, @id_hotel);
END;

GO
