CREATE DATABASE MEDICINA

USE MEDICINA


CREATE TABLE Paciente (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto NVARCHAR(100) NOT NULL,
    FechaNacimiento NVARCHAR(50) NOT NULL,
    Sexo NVARCHAR(50) NOT NULL,
    DPI NVARCHAR(13) UNIQUE NOT NULL, 
	estado TINYINT not null, 
	fechaingreso date
);



-- Tabla de Especialidades
CREATE TABLE Especialidad (
    Codigo INT PRIMARY KEY,
    Descripcion NVARCHAR(100) NOT NULL,
    EspecialidadPrincipalID INT NULL,
    FOREIGN KEY (EspecialidadPrincipalID) REFERENCES Especialidad(Codigo)
);

-- Tabla de Médicos
CREATE TABLE Medico (
    Codigo INT PRIMARY KEY,
    NombreCompleto NVARCHAR(100) NOT NULL,
    DPI CHAR(13) UNIQUE NOT NULL,
    Colegiado NVARCHAR(50),
    Sexo NVARCHAR(50) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    CodigoVerificacion CHAR(4),
    EspecialidadID INT NULL,
    SubEspecialidadID INT NULL,
    JefeInmediatoID INT NULL,
    FOREIGN KEY (EspecialidadID) REFERENCES Especialidad(Codigo),
    FOREIGN KEY (SubEspecialidadID) REFERENCES Especialidad(Codigo),
    FOREIGN KEY (JefeInmediatoID) REFERENCES Medico(Codigo)
);

-- Tabla de Citas o Consultas
CREATE TABLE Cita (
    Codigo INT PRIMARY KEY,
    PacienteID INT NOT NULL,
    MedicoID INT NOT NULL,
    FechaHoraInicio DATETIME NOT NULL,
    FechaHoraFin DATETIME NOT NULL,
    Estado NVARCHAR(20) CHECK (Estado IN ('Pendiente', 'En progreso', 'Finalizada', 'Cancelada')),
    FechaProximaCita DATETIME NULL,
    FOREIGN KEY (PacienteID) REFERENCES Paciente(Codigo),
    FOREIGN KEY (MedicoID) REFERENCES Medico(Codigo)
);

-- Tabla de Anexos de la Cita
CREATE TABLE Anexo (
    Codigo INT PRIMARY KEY,
    CitaID INT NOT NULL,
    Tipo NVARCHAR(50),
    Contenido VARBINARY(MAX) NULL,
    Observacion NVARCHAR(MAX) NULL,
    FOREIGN KEY (CitaID) REFERENCES Cita(Codigo)
);

-- Tabla de Medicamentos
CREATE TABLE Medicamento (
    Codigo INT PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255)
);

-- Tabla de relación entre Citas y Medicamentos (muchos a muchos)
CREATE TABLE CitaMedicamento (
    CitaID INT NOT NULL,
    MedicamentoID INT NOT NULL,
    Dosis NVARCHAR(50),
    Frecuencia NVARCHAR(50),
    PRIMARY KEY (CitaID, MedicamentoID),
    FOREIGN KEY (CitaID) REFERENCES Cita(Codigo),
    FOREIGN KEY (MedicamentoID) REFERENCES Medicamento(Codigo)
);

-- CONSULTAS ESTADÍSTICAS

-- 1. Total de consultas en progreso por especialidad
SELECT e.Descripcion AS Especialidad, COUNT(*) AS TotalEnProgreso
FROM Cita c
JOIN Medico m ON c.MedicoID = m.Codigo
JOIN Especialidad e ON m.EspecialidadID = e.Codigo
WHERE c.Estado = 'En progreso'
GROUP BY e.Descripcion;



-- 2. Total de consultas pendientes por médico
SELECT m.NombreCompleto AS Medico, COUNT(*) AS TotalPendientes
FROM Cita c
JOIN Medico m ON c.MedicoID = m.Codigo
WHERE c.Estado = 'Pendiente'
GROUP BY m.NombreCompleto;

-- 3. Consulta con mayor duración (en minutos)
SELECT TOP 1
    p.NombreCompleto AS Paciente,
    m.NombreCompleto AS Medico,
    DATEDIFF(MINUTE, c.FechaHoraInicio, c.FechaHoraFin) AS DuracionMinutos
FROM Cita c
JOIN Paciente p ON c.PacienteID = p.Codigo
JOIN Medico m ON c.MedicoID = m.Codigo
ORDER BY DuracionMinutos DESC;

-- 4. Consulta con menor duración (en segundos)
SELECT TOP 1
    p.NombreCompleto AS Paciente,
    m.NombreCompleto AS Medico,
    DATEDIFF(SECOND, c.FechaHoraInicio, c.FechaHoraFin) AS DuracionSegundos
FROM Cita c
JOIN Paciente p ON c.PacienteID = p.Codigo
JOIN Medico m ON c.MedicoID = m.Codigo
ORDER BY DuracionSegundos ASC;

-- 5. Cliente con más medicamentos recetados
SELECT TOP 1
    p.NombreCompleto,
    COUNT(cm.MedicamentoID) AS TotalMedicamentos
FROM CitaMedicamento cm
JOIN Cita c ON cm.CitaID = c.Codigo
JOIN Paciente p ON c.PacienteID = p.Codigo
GROUP BY p.NombreCompleto
ORDER BY TotalMedicamentos DESC;

-- 6. Médico hombre que ha atendido a más pacientes mujeres
SELECT TOP 1
    m.NombreCompleto,
    COUNT(*) AS TotalPacientesMujeres
FROM Cita c
JOIN Medico m ON c.MedicoID = m.Codigo
JOIN Paciente p ON c.PacienteID = p.Codigo
WHERE m.Sexo = 'M' AND p.Sexo = 'F'
GROUP BY m.NombreCompleto
ORDER BY TotalPacientesMujeres DESC;

-- 7. Cada subespecialidad con el total de consultas realizadas por pacientes hombres
SELECT es.Descripcion AS Subespecialidad, COUNT(*) AS TotalConsultasHombres
FROM Cita c
JOIN Paciente p ON c.PacienteID = p.Codigo
JOIN Medico m ON c.MedicoID = m.Codigo
JOIN Especialidad es ON m.SubEspecialidadID = es.Codigo
WHERE p.Sexo = 'M'
GROUP BY es.Descripcion;




SELECT * FROM Paciente

drop procedure sp_Paciente_CRUD

EXEC sp_Paciente_CRUD 1,NULL,'HOLA','10-05-2025','MASCULINO','2563';

CREATE PROCEDURE sp_Paciente_CRUD
    @p_opcion INT,
    @p_Codigo INT = NULL,
    @p_nombrecompleto NVARCHAR(100) = NULL,
    @p_fechanacimiento NVARCHAR(50) = NULL,
    @p_sexo NVARCHAR(20) = NULL,
    @p_DPI NVARCHAR(13) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_opcion = 1
    BEGIN

        INSERT INTO Paciente (NombreCompleto, FechaNacimiento, Sexo, DPI, estado, fechaingreso)
        VALUES (@p_nombrecompleto, @p_fechanacimiento, @p_sexo, @p_DPI, 1, getdate());
    END
    ELSE IF @p_opcion = 2
    BEGIN

        SELECT * FROM Paciente WHERE DPI = @p_DPI;
    END
    ELSE IF @p_opcion = 3
    BEGIN

        SELECT * FROM Paciente;
    END
    ELSE IF @p_opcion = 4
    BEGIN
       
        DELETE FROM Paciente WHERE DPI = @p_DPI;
    END
    ELSE
    BEGIN
        RAISERROR('Opción inválida', 16, 1);
    END
END;
