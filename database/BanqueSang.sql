-- ===============================
-- BASE DE DONNEES : BANQUE_DE_SANG
-- ===============================

DROP DATABASE IF EXISTS banque_de_sang;
CREATE DATABASE banque_de_sang CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
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
                         donneur_statut_eligibilite ENUM('ELIGIBLE', 'NON_ELIGIBLE') DEFAULT 'ELIGIBLE',
                         donneur_dernier_don DATE NULL
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
                           personnel_fonction ENUM('ADMINISTRATEUR', 'TECHNICIEN', 'MEDECIN') NOT NULL,
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
-- TABLE 6 : SERVICE
-- ===============================

CREATE TABLE SERVICE (
                         id_service INT AUTO_INCREMENT PRIMARY KEY,
                         id_hopital INT NOT NULL,
                         service_nom VARCHAR(100) NOT NULL,
                         service_description VARCHAR(255),
                         service_adresse VARCHAR(255),

                         CONSTRAINT fk_service_hopital FOREIGN KEY (id_hopital)
                             REFERENCES HOPITAL(id_hopital)
                             ON DELETE RESTRICT
);

-- ===============================
-- TABLE 7 : PATIENT
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

                         CONSTRAINT fk_patient_service FOREIGN KEY (id_service)
                             REFERENCES SERVICE(id_service)
                             ON DELETE RESTRICT
);

-- ===============================
-- TABLE 8 : DON
-- ===============================

CREATE TABLE DON (
                     id_don INT AUTO_INCREMENT PRIMARY KEY,
                     id_donneur INT NOT NULL,
                     id_centre INT,
                     don_date DATE NOT NULL,
                     don_statut ENUM('EN_ATTENTE', 'VALIDE', 'REJETE') DEFAULT 'EN_ATTENTE',
                     don_volume_ml INT NOT NULL CHECK (don_volume_ml > 0),

                     CONSTRAINT fk_don_donneur FOREIGN KEY (id_donneur)
                         REFERENCES DONNEUR(id_donneur)
                         ON DELETE RESTRICT,

                     CONSTRAINT fk_don_centre FOREIGN KEY (id_centre)
                         REFERENCES CENTRE_COLLECTE(id_centre)
                         ON DELETE RESTRICT
);

-- ===============================
-- TABLE 9 : SANG
-- ===============================

CREATE TABLE SANG (
                      id_sang INT AUTO_INCREMENT PRIMARY KEY,
                      id_don INT NOT NULL,
                      id_centre INT,
                      id_hopital INT NULL,
                      sang_type_composant ENUM('SANG_TOTAL', 'GLOBULES_ROUGES', 'PLASMA', 'PLAQUETTES') NOT NULL,
                      sang_date_creation DATE NOT NULL,
                      sang_date_peremption DATE NOT NULL,
                      sang_disponible BOOLEAN DEFAULT FALSE,

                      CONSTRAINT fk_sang_don FOREIGN KEY (id_don)
                          REFERENCES DON(id_don)
                          ON DELETE RESTRICT,

                      CONSTRAINT fk_sang_centre FOREIGN KEY (id_centre)
                          REFERENCES CENTRE_COLLECTE(id_centre)
                          ON DELETE RESTRICT,

                      CONSTRAINT fk_sang_hopital FOREIGN KEY (id_hopital)
                          REFERENCES HOPITAL(id_hopital)
                          ON DELETE RESTRICT
);

-- ===============================
-- TABLE 10 : RESULTAT_TEST
-- ===============================

CREATE TABLE RESULTAT_TEST (
                               id_resultat_test INT AUTO_INCREMENT PRIMARY KEY,
                               id_personnel INT NOT NULL,
                               id_sang INT NOT NULL,
                               id_test INT NOT NULL,
                               date_test DATETIME NOT NULL,
                               resultat ENUM('NEGATIF', 'POSITIF', 'EN_ATTENTE') DEFAULT 'EN_ATTENTE',
                               commentaire TEXT,
                               statut_test ENUM('EN_ATTENTE', 'VALIDE', 'REJETE') DEFAULT 'EN_ATTENTE',

                               CONSTRAINT fk_resultat_personnel FOREIGN KEY (id_personnel)
                                   REFERENCES PERSONNEL(id_personnel)
                                   ON DELETE RESTRICT,

                               CONSTRAINT fk_resultat_sang FOREIGN KEY (id_sang)
                                   REFERENCES SANG(id_sang)
                                   ON DELETE RESTRICT,

                               CONSTRAINT fk_resultat_test FOREIGN KEY (id_test)
                                   REFERENCES TEST(id_test)
                                   ON DELETE RESTRICT,

                               CONSTRAINT resultat_unique UNIQUE (id_sang, id_test)
);

-- ===============================
-- TABLE 11 : DEMANDE_SANG
-- ===============================

CREATE TABLE DEMANDE_SANG (
                              id_demande INT AUTO_INCREMENT PRIMARY KEY,
                              id_patient INT NOT NULL,
                              id_sang INT NULL,
                              id_personnel INT NOT NULL,
                              type_composant ENUM('SANG_TOTAL', 'GLOBULES_ROUGES', 'PLASMA', 'PLAQUETTES') NOT NULL,
                              quantite_poches INT NOT NULL CHECK (quantite_poches > 0),
                              urgence ENUM('NORMAL', 'URGENT', 'CRITIQUE') DEFAULT 'NORMAL',
                              date_demande DATETIME NOT NULL,
                              commentaire TEXT,
                              statut ENUM('EN_ATTENTE', 'RESERVEE', 'TRAITEE', 'ANNULEE') DEFAULT 'EN_ATTENTE',

                              CONSTRAINT fk_demande_patient FOREIGN KEY (id_patient)
                                  REFERENCES PATIENT(id_patient)
                                  ON DELETE RESTRICT,

                              CONSTRAINT fk_demande_sang FOREIGN KEY (id_sang)
                                  REFERENCES SANG(id_sang)
                                  ON DELETE SET NULL,

                              CONSTRAINT fk_demande_personnel FOREIGN KEY (id_personnel)
                                  REFERENCES PERSONNEL(id_personnel)
                                  ON DELETE RESTRICT
);

-- ===============================
-- DONNEES DE TEST
-- ===============================

INSERT INTO CENTRE_COLLECTE (centre_nom, centre_adresse) VALUES
                                                             ('Centre Principal Bruxelles', 'Rue de la Santé 10, Bruxelles'),
                                                             ('Centre Nord', 'Avenue du Don 25, Bruxelles');

INSERT INTO HOPITAL (hopital_nom, hopital_ville) VALUES
                                                     ('Hôpital Saint-Luc', 'Bruxelles'),
                                                     ('Hôpital Erasme', 'Bruxelles');

INSERT INTO SERVICE (id_hopital, service_nom, service_description, service_adresse) VALUES
                                                                                        (1, 'Urgences', 'Service des urgences', 'Rue de la Santé 10'),
                                                                                        (1, 'Chirurgie', 'Service de chirurgie', 'Rue de la Santé 10'),
                                                                                        (2, 'Transfusion', 'Service de transfusion', 'Route de Lennik 808');

INSERT INTO TEST (test_nom) VALUES
                                ('VIH'),
                                ('Hépatite B'),
                                ('Hépatite C'),
                                ('Syphilis'),
                                ('Numération globulaire'),
                                ('Groupage ABO');

INSERT INTO DONNEUR (
    donneur_nom,
    donneur_prenom,
    donneur_adresse,
    donneur_telephone,
    donneur_date_naissance,
    donneur_groupe_sanguin,
    donneur_statut_eligibilite,
    donneur_dernier_don
) VALUES
      ('Dupont', 'Jean', 'Rue Exemple 1', '0488000001', '1990-05-12', 'A+', 'ELIGIBLE', NULL),
      ('Martin', 'Sophie', 'Rue Exemple 2', '0488000002', '1985-03-22', 'O-', 'ELIGIBLE', NULL),
      ('Bernard', 'Luc', 'Rue Exemple 3', '0488000003', '2008-01-10', 'B+', 'NON_ELIGIBLE', NULL);

INSERT INTO PATIENT (
    id_service,
    patient_nom,
    patient_prenom,
    patient_adresse,
    patient_telephone,
    patient_date_naissance,
    patient_groupe_sanguin
) VALUES
      (1, 'Durand', 'Alice', 'Rue Patient 1', '0477000001', '1995-04-10', 'A+'),
      (2, 'Lambert', 'Paul', 'Rue Patient 2', '0477000002', '1978-08-21', 'O+'),
      (3, 'Moreau', 'Emma', 'Rue Patient 3', '0477000003', '2001-11-30', 'AB+');

-- Mot de passe temporaire à remplacer plus tard par un vrai hash BCrypt
-- Pour le backend, on mettra ensuite un hash généré pour "Password123!"
INSERT INTO PERSONNEL (
    personnel_nom,
    personnel_prenom,
    personnel_adresse,
    personnel_telephone,
    personnel_date_naissance,
    personnel_fonction,
    personnel_login,
    personnel_password,
    personnel_actif
) VALUES
      ('Admin', 'Principal', 'Rue Admin 1', '0499000001', '1980-01-01', 'ADMINISTRATEUR', 'admin', 'Password123!', TRUE),
      ('Tech', 'Banque', 'Rue Tech 1', '0499000002', '1992-02-02', 'TECHNICIEN', 'technicien', 'Password123!', TRUE),
      ('Medecin', 'Hopital', 'Rue Medecin 1', '0499000003', '1988-03-03', 'MEDECIN', 'medecin', 'Password123!', TRUE);