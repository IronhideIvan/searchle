create schema account;

create table account.users (
  user_id uuid not null,
  email_address varchar(80) not null,
  created_date timestamp not null
);