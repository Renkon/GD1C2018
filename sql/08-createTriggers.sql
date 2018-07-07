CREATE TRIGGER [EL_MONSTRUO_DEL_LAGO_MASER].[add_users_for_new_hotel] ON [EL_MONSTRUO_DEL_LAGO_MASER].[hoteles]
AFTER INSERT AS
BEGIN
    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles]
    SELECT 1, id_hotel
    FROM inserted

    INSERT INTO [EL_MONSTRUO_DEL_LAGO_MASER].[usuariosXhoteles]
    SELECT 2, id_hotel
    FROM inserted
END;

GO
