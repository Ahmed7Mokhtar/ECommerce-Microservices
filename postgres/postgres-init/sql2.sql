-- Create the table if it does not exist
CREATE TABLE public."Users"
(
    "Id" uuid NOT NULL,
    "Name" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Email" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Password" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Gender" smallint NOT NULL,
    CONSTRAINT "Users_pkey" PRIMARY KEY ("Id")
);


INSERT INTO public."Users"("Id", "Name", "Email", "Password", "Gender")
VALUES('c32f8b42-60e6-4c02-90a7-9143ab37189f', 'Rooney', 'rooney@test.com', '123456', 1);