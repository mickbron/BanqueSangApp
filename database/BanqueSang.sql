-- ===============================
-- BASE DE DONNEES : BANQUE_DE_SANG
-- ===============================

CREATE DATABASE IF NOT EXISTS banque_de_sang;
USE banque_de_sang;

-- ===============================
-- TABLE 1 : DONNEUR
-- ===============================

CREATE TABLE DONNEUR (
    id_donneur INT AUTO_INCREMENT PRIMARY KEY,
    donneur_nom VARCHAR(100) NOT NULL,
    donneur_prenom VARCHAR(100) NOT NULL,
    donneur_adresse VARCHAR(255) NOT NULL,
    donneur_telephone VARCHAR(20) NOT NULL,
    donneur_date_naissance DATE NOT NULL,
    donneur_groupe_sanguin ENUM('A+', 'A-', 'B+', 'B-', 'O+', 'O-', 'AB+', 'AB-') NOT NULL,
    donneur_statut_eligibilite ENUM('Éligible', 'Non Éligible') DEFAULT 'Éligible'
);

-- ===============================
-- TABLE 2 : PERSONNEL
-- ===============================

CREATE TABLE PERSONNEL (
    id_personnel INT AUTO_INCREMENT PRIMARY KEY,
    personnel_nom VARCHAR(100) NOT NULL,
    personnel_prenom VARCHAR(100) NOT NULL,
    personnel_adresse VARCHAR(255) NOT NULL,
    personnel_telephone VARCHAR(20) NOT NULL,
    personnel_date_naissance DATE NOT NULL,
    personnel_fonction VARCHAR(100) NOT NULL,
    personnel_login VARCHAR(100) UNIQUE NOT NULL,
    personnel_password VARCHAR(255) NOT NULL,
    personnel_actif BOOLEAN DEFAULT TRUE
);

-- ===============================
-- TABLE 3 : TEST
-- ===============================

CREATE TABLE TEST (
    id_test INT AUTO_INCREMENT PRIMARY KEY,
    test_nom VARCHAR(100) NOT NULL
);

-- ===============================
-- TABLE 4 : CENTRE_COLLECTE
-- ===============================

CREATE TABLE CENTRE_COLLECTE (
    id_centre INT AUTO_INCREMENT PRIMARY KEY,
    centre_nom VARCHAR(100) NOT NULL,
    centre_adresse VARCHAR(255) NOT NULL
);

-- ===============================
-- TABLE 5 : HOPITAL
-- ===============================

CREATE TABLE HOPITAL (
    id_hopital INT AUTO_INCREMENT PRIMARY KEY,
    hopital_nom VARCHAR(100) NOT NULL,
    hopital_ville VARCHAR(100) NOT NULL
);

-- ===============================
-- TABLE 6 : DON
-- ===============================

CREATE TABLE DON (
    id_don INT AUTO_INCREMENT PRIMARY KEY,
    id_donneur INT NOT NULL,
    id_centre INT,
    don_date DATE NOT NULL,
    don_statut ENUM('En attente', 'Validé', 'Rejeté') DEFAULT 'En attente',
    don_volume_ml INT CHECK (don_volume_ml > 0),

    CONSTRAINT fk_don_donneur FOREIGN KEY (id_donneur)
        REFERENCES DONNEUR(id_donneur)
        ON DELETE RESTRICT,

    CONSTRAINT fk_don_centre FOREIGN KEY (id_centre)
        REFERENCES CENTRE_COLLECTE(id_centre)
        ON DELETE RESTRICT
);

-- ===============================
-- TABLE 7 : SANG
-- ===============================

CREATE TABLE SANG (
    id_sang INT AUTO_INCREMENT PRIMARY KEY,
    id_centre INT,
    id_hopital INT,
    sang_type_composant VARCHAR(50) NOT NULL,
    sang_date_creation DATE NOT NULL,
    sang_date_peremption DATE NOT NULL,
    sang_disponible BOOLEAN DEFAULT TRUE,

    CONSTRAINT fk_sang_centre FOREIGN KEY (id_centre)
        REFERENCES CENTRE_COLLECTE(id_centre)
        ON DELETE RESTRICT,

    CONSTRAINT fk_hopital FOREIGN KEY (id_hopital)
        REFERENCES HOPITAL(id_hopital)
        ON DELETE RESTRICT
);

-- ===============================
-- TABLE 8 : RESULTAT_TEST (association entre PERSONNEL, SANG et TEST)
-- ===============================

CREATE TABLE RESULTAT_TEST (
    id_resultat_test INT AUTO_INCREMENT PRIMARY KEY,
    id_personnel INT NOT NULL,
    id_sang INT NOT NULL,
    id_test INT NOT NULL,
    date_test DATETIME NOT NULL,
    resultat VARCHAR(100),
    commentaire TEXT,
    statut_test ENUM('En attente', 'Validé', 'Rejeté') DEFAULT 'En attente',

    CONSTRAINT fk_resultat_personnel FOREIGN KEY (id_personnel)
        REFERENCES PERSONNEL(id_personnel)
        ON DELETE RESTRICT,
    CONSTRAINT fk_resultat_sang FOREIGN KEY (id_sang)
        REFERENCES SANG(id_sang)
        ON DELETE RESTRICT,
    CONSTRAINT fk_resultat_test FOREIGN KEY (id_test)
        REFERENCES TEST(id_test)
        ON DELETE RESTRICT,

    CONSTRAINT resultat_unique UNIQUE (id_personnel, id_sang, id_test, date_test)
);

-- ===============================
-- TABLE 9 : SERVICE
-- ===============================

CREATE TABLE SERVICE (
    id_service INT AUTO_INCREMENT PRIMARY KEY,
    id_hopital INT NOT NULL,
    service_nom VARCHAR(100) NOT NULL,
    service_description VARCHAR(100),
    service_adresse VARCHAR(255),

    CONSTRAINT fk_service_hopital FOREIGN KEY (id_hopital)
        REFERENCES HOPITAL(id_hopital)
        ON DELETE RESTRICT
);

-- ===============================
-- TABLE 10 : PATIENT
-- ===============================

CREATE TABLE PATIENT (
    id_patient INT AUTO_INCREMENT PRIMARY KEY,
    id_service INT,
    patient_nom VARCHAR(100) NOT NULL,
    patient_prenom VARCHAR(100) NOT NULL,
    patient_adresse VARCHAR(255) NOT NULL,
    patient_telephone VARCHAR(20) NOT NULL,
    patient_date_naissance DATE NOT NULL,
    patient_groupe_sanguin ENUM('A+', 'A-', 'B+', 'B-', 'O+', 'O-', 'AB+', 'AB-') NOT NULL,

    CONSTRAINT fk_service FOREIGN KEY (id_service)
        REFERENCES SERVICE (id_service)
        ON DELETE RESTRICT
);
