
-- install extensions

-- create role
create role todo_role with
  LOGIN
  NOSUPERUSER
  INHERIT
  NOCREATEDB
  NOCREATEROLE
  NOREPLICATION;

alter role todo_role
  password 'changeit';

-- create schema
create schema if not exists todo
  authorization postgres;

-- grant usage
grant usage on schema todo to todo_role;
alter default privileges in schema todo grant select, insert, update, delete on tables to todo_role;
grant all on schema todo to postgres;
