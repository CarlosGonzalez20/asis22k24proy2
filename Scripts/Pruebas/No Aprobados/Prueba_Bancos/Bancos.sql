CREATE DATABASE BancoDB;
USE BancoDB;

-- Tabla: btl_banco
CREATE TABLE tbl_banco (
    pk_id_banco INT AUTO_INCREMENT PRIMARY KEY,
    nombre_banco VARCHAR(100) NOT NULL
);

-- Tabla: btl_cuentabancaria
CREATE TABLE tbl_cuentabancaria (
    pk_id_cuenta INT AUTO_INCREMENT PRIMARY KEY,
    fk_banco_id INT NOT NULL,
    numero_cuenta VARCHAR(20) UNIQUE NOT NULL,
    saldo_cuenta DECIMAL(10,2) NOT NULL,
    tipo_cuenta VARCHAR(50) NOT NULL,
    CONSTRAINT fk_banco FOREIGN KEY (fk_banco_id) REFERENCES tbl_banco(pk_id_banco)
);

-- Tabla: btl_movimientobancario
CREATE TABLE tbl_movimientobancario (
    pk_id_movimiento_ban INT AUTO_INCREMENT PRIMARY KEY,
    fk_no_cuenta INT NOT NULL,
    fecha_movimiento DATE NOT NULL,
    tipo_movimiento VARCHAR(50) NOT NULL,
    monto_movimiento DECIMAL(10,2) NOT NULL,
    descripcion_movimiento TEXT,
    metodo_pago VARCHAR(50),
    estado TINYINT(1),
    CONSTRAINT fk_cuenta FOREIGN KEY (fk_no_cuenta) REFERENCES tbl_cuentabancaria(pk_id_cuenta)
);

-- Tabla: btl_transaccion
CREATE TABLE tbl_transaccion (
    pk_id_transaccion INT AUTO_INCREMENT PRIMARY KEY,
    fk_cuenta_id INT NOT NULL,
    fecha_transaccion DATETIME NOT NULL,
    monto_transaccion DECIMAL(10,2) NOT NULL,
    tipo_transaccion TINYINT(1) NOT NULL,
    CONSTRAINT fk_cuenta_transaccion FOREIGN KEY (fk_cuenta_id) REFERENCES tbl_cuentabancaria(pk_id_cuenta)
);

create table tbl_tipoCambio (
	pk_id_tipo_cambio INT AUTO_INCREMENT PRIMARY KEY,
    nombre_moneda VARCHAR (50) NOT NULL,
    valor_moneda decimal (5,3) NOT NULL,
    valorCambio_moneda decimal (5,3) NOT NULL,
    estatus TINYINT (1) DEFAULT 1
);