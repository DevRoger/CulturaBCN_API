-- 1. Crear la base de datos y usarla
CREATE DATABASE CulturaBCN;
GO

USE CulturaBCN;
GO

-- 2. Roles de usuario
CREATE TABLE roles (
    id_rol         INT IDENTITY(1,1) PRIMARY KEY,
    nombre         VARCHAR(50)      NOT NULL
);

-- 3. Usuarios (sin almacenar edad: se calcula automáticamente)
CREATE TABLE usuarios (
    id_usuario        INT                    IDENTITY(1000000,1) PRIMARY KEY,
    nombre            VARCHAR(50)            NOT NULL,
    apellidos         VARCHAR(150)           NOT NULL,
    correo            VARCHAR(255)           NOT NULL UNIQUE,
    contrasena_hash   VARCHAR(255)           NOT NULL,
    fecha_nacimiento  DATE                   NOT NULL,
    edad              AS DATEDIFF(
                         YEAR,
                         fecha_nacimiento,
                         GETDATE()
                       ),
    telefono          VARCHAR(20)            NOT NULL,
    foto_url          VARCHAR(255),
    id_rol            INT                    NOT NULL
        CONSTRAINT fk_usuario_rol
        REFERENCES roles(id_rol)
);

-- 4. Salas
CREATE TABLE salas (
    id_sala      INT IDENTITY(2000000,1) PRIMARY KEY,
    nombre       VARCHAR(100)      NOT NULL,
    direccion    VARCHAR(255)      NOT NULL,
    aforo        INT               NOT NULL
);

-- 5. Eventos
CREATE TABLE eventos (
    id_evento     INT IDENTITY(3000000,1) PRIMARY KEY,
    nombre        VARCHAR(255)      NOT NULL,
    descripcion   VARCHAR(1000)     NOT NULL,
    fecha         DATE              NOT NULL,
    hora_inicio   TIME              NOT NULL,
    hora_fin      TIME              NOT NULL,
    lugar         VARCHAR(255)      NOT NULL,
    precio        DECIMAL(10,2)     NOT NULL,
    edad_minima   INT               NOT NULL,
    enumerado     BIT               NOT NULL DEFAULT 0,
    id_sala       INT               NOT NULL
        CONSTRAINT fk_evento_sala
        REFERENCES salas(id_sala)
);

-- 6. Asientos (para eventos enumerados)
CREATE TABLE asientos (
    id_asiento   INT IDENTITY(4000000,1) PRIMARY KEY,
    numero       INT               NOT NULL,
    disponible   BIT               NOT NULL DEFAULT 1,
    id_evento    INT               NOT NULL
        CONSTRAINT fk_asiento_evento
        REFERENCES eventos(id_evento)
);

-- 7. Reservas de entrada
CREATE TABLE reservas_entradas (
    id_reserva    INT IDENTITY(5000000,1) PRIMARY KEY,
    id_asiento    INT               NOT NULL,
    id_usuario    INT               NOT NULL,
    fecha_reserva DATETIME          NOT NULL DEFAULT GETDATE(),
    CONSTRAINT uq_reserva UNIQUE(id_asiento, id_usuario),
    CONSTRAINT fk_reserva_asiento
        FOREIGN KEY (id_asiento) REFERENCES asientos(id_asiento),
    CONSTRAINT fk_reserva_usuario
        FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
);

-- 8. Chats entre usuarios
CREATE TABLE chats (
    id_chat         INT IDENTITY(6000000,1) PRIMARY KEY,
    id_usuario_1    INT               NOT NULL,
    id_usuario_2    INT               NOT NULL,
    fecha_creacion  DATETIME          NOT NULL DEFAULT GETDATE(),
    CONSTRAINT fk_chat_usuario1
        FOREIGN KEY (id_usuario_1) REFERENCES usuarios(id_usuario),
    CONSTRAINT fk_chat_usuario2
        FOREIGN KEY (id_usuario_2) REFERENCES usuarios(id_usuario)
);

-- 9. Mensajes en cada chat
CREATE TABLE mensajes (
    id_mensaje   INT IDENTITY(7000000,1) PRIMARY KEY,
    id_chat      INT               NOT NULL,
    id_usuario   INT               NOT NULL,
    texto        VARCHAR(5000)     NOT NULL,
    fecha_envio  DATETIME          NOT NULL DEFAULT GETDATE(),
    visto        BIT               NOT NULL DEFAULT 0,
    CONSTRAINT fk_mensaje_chat
        FOREIGN KEY (id_chat) REFERENCES chats(id_chat)
        ON DELETE CASCADE,
    CONSTRAINT fk_mensaje_usuario
        FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
);


-- INSERTS DE EJEMPLO --

-- 1. Roles de usuario
INSERT INTO roles (nombre) VALUES
('Gestor'),
('Cliente')
GO

-- 2. Usuarios
INSERT INTO usuarios (
    nombre, apellidos, correo, contrasena_hash,
    fecha_nacimiento, telefono, foto_url, id_rol
) VALUES
('Alice',    'García',    'alice.garcia@ejemplo.com',    '$2a$10$hash1...', '1985-06-15', '+34123456701', NULL, 1),
('Bob',      'López',     'bob.lopez@ejemplo.com',       '$2a$10$hash2...', '1990-01-22', '+34123456702', NULL, 1),
('Carlos',   'Martínez',  'carlos.martinez@ejemplo.com', '$2a$10$hash3...', '1978-11-03', '+34123456703', NULL, 1),
('Diana',    'Sánchez',   'diana.sanchez@ejemplo.com',   '$2a$10$hash4...', '1992-04-17', '+34123456704', NULL, 1),
('Eduardo',  'Fernández', 'eduardo.fernandez@ejemplo.com','$2a$10$hash5...', '1988-09-30', '+34123456705', NULL, 1),
('Fátima',   'Ruiz',      'fatima.ruiz@ejemplo.com',      '$2a$10$hash6...', '1995-12-08', '+34123456706', NULL, 2),
('Gonzalo',  'Torres',    'gonzalo.torres@ejemplo.com',   '$2a$10$hash7...', '1982-02-14', '+34123456707', NULL, 2),
('Helena',   'Morales',   'helena.morales@ejemplo.com',   '$2a$10$hash8...', '1991-07-25', '+34123456708', NULL, 2),
('Iván',     'Ramírez',   'ivan.ramirez@ejemplo.com',     '$2a$10$hash9...', '1987-03-05', '+34123456709', NULL, 2),
('Julia',    'Vázquez',   'julia.vazquez@ejemplo.com',    '$2a$10$hash0...', '1993-10-19', '+34123456710', NULL, 2)
GO

-- 3. Salas
INSERT INTO salas (nombre, direccion, aforo) VALUES
('Gran Teatre',           'C/ Mayor 1, Barcelona',        800),
('Sala Petita',           'C/ Pau Claris 10, Barcelona',  200),
('Auditori 1',            'Av. Diagonal 500, Barcelona', 1200),
('Espai Dansa',           'C/ Provença 250, Barcelona',   150),
('Teatre Lliure',         'C/ Lleida 59, Barcelona',      400),
('Sala Jove',             'Rbla. Catalunya 20, Barcelona',100),
('Sala Clàssica',         'C/ Consell de Cent 300, BCN',  350),
('Sala Oscura',           'C/ Balmes 60, Barcelona',      180),
('Sala Multicultural',    'C/ Hospital 75, Barcelona',    300),
('Espai Exposicions',     'C/ València 400, Barcelona',   500);
GO

-- 4. Eventos
INSERT INTO eventos (
    nombre, descripcion, fecha, hora_inicio, hora_fin,
    lugar, precio, edad_minima, enumerado, id_sala
) VALUES
('Concert Jazz',       'Nit de jazz contemporani.',            '2025-05-10', '20:00','22:00','Gran Teatre',        30.00, 16, 1, 2000000),
('Teatre Infantil',    'Obra familiar per a nens.',            '2025-06-01', '17:00','18:30','Sala Petita',        12.50,  3, 0, 2000001),
('Opera Clàssica',     'Representació d Ópera de Verdi.',      '2025-07-15', '19:30','22:00','Auditori 1',         55.00,  12,1, 2000002),
('Dansa Contemporània','Espectacle de dansa moderna.',         '2025-08-20', '21:00','22:15','Espai Dansa',        25.00,  14,0, 2000003),
('Teatre Experimental','Peça avantguardista en 1 acte.',       '2025-09-05', '18:00','19:00','Teatre Lliure',       18.00,  18,1, 2000004),
('Concert Pop',        'Artista internacional de música pop.', '2025-10-10', '20:00','23:00','Sala Jove',          40.00,  12,0, 2000005),
('Recital Piano',      'Piano solista, obres de Chopin.',      '2025-11-12', '19:00','20:30','Sala Clàssica',      28.00,  0, 1, 2000006),
('Cinema a la Fresca','Projecció a l aire lliure.',           '2025-07-01', '22:00','00:00','Sala Oscura',        10.00,  0, 0, 2000007),
('Fira d Art',         'Exposició i venda d art local.',       '2025-12-03', '10:00','18:00','Sala Multicultural',  5.00,  0, 0, 2000008),
('Exposició Fotogràfica','Mostra fotogràfica temàtica.',        '2025-09-20', '11:00','19:00','Espai Exposicions',   8.00,  0, 0, 2000009);
GO

-- 5. Asientos
INSERT INTO asientos (numero, disponible, id_evento) VALUES
( 1, 1, 3000000),
( 2, 1, 3000000),
( 3, 1, 3000000),
( 4, 1, 3000000),
( 1, 1, 3000002),
( 2, 1, 3000002),
( 1, 1, 3000004),
( 2, 1, 3000004),
( 1, 1, 3000006),
( 2, 1, 3000006);
GO

-- 6. Reservas de entrada
INSERT INTO reservas_entradas (id_asiento, id_usuario) VALUES
(4000000, 1000000),
(4000001, 1000001),
(4000002, 1000002),
(4000003, 1000003),
(4000004, 1000004),
(4000005, 1000005),
(4000006, 1000006),
(4000007, 1000007),
(4000008, 1000008),
(4000009, 1000009);
GO

-- 7. Chats
INSERT INTO chats (id_usuario_1, id_usuario_2) VALUES
(1000000,1000001),
(1000000,1000002),
(1000001,1000002),
(1000002,1000003),
(1000003,1000004),
(1000004,1000005),
(1000005,1000006),
(1000006,1000007),
(1000007,1000008),
(1000008,1000009);
GO

-- 8. Mensajes
INSERT INTO mensajes (id_chat, id_usuario, texto, visto) VALUES
(6000000,1000000,'Hola! Com et va?',             1),
(6000001,1000000,'Vols venir al concert?',       0),
(6000002,1000001,'Sí, quan són les entrades?',   0),
(6000003,1000002,'Bon dia, tens novetats?',      1),
(6000004,1000003,'Recorda la reunió demà.',      0),
(6000005,1000004,'Hem afegit nova exposició.',   1),
(6000006,1000005,'Et va bé el dissabte?',        0),
(6000007,1000006,'Gràcies pel feedback!',       1),
(6000008,1000007,'On recullo les entrades?',    0),
(6000009,1000008,'Ens veiem a la inauguració.',  1);
GO