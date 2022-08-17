--
-- db users
--
CREATE USER wordnet30_admin WITH SUPERUSER password 'password!1';
CREATE USER wordnet30_appusr WITH SUPERUSER password 'password!1';

--
-- Name: wordnet30; Type: DATABASE; Schema: -; Owner: -
--

CREATE DATABASE wordnet30 WITH TEMPLATE = template0 ENCODING = 'UTF8';
