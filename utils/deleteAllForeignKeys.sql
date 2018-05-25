SELECT 'ALTER TABLE ' + schema_name(Schema_id)+'.'+ object_name(parent_object_id) + ' DROP CONSTRAINT ' + name 
FROM sys.foreign_keys f1;

