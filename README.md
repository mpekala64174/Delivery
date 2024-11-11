Baza SQL


-- Tabela: magazyn
CREATE TABLE magazyn (
    ID_paczki INT PRIMARY KEY IDENTITY(1,1),
    waga DECIMAL(10, 2) NOT NULL,
    rozmiar VARCHAR(50) NOT NULL,
    miejsce_odbioru VARCHAR(100) NOT NULL,
    status VARCHAR(50) NOT NULL
);

-- Tabela: uzytkownicy
CREATE TABLE uzytkownicy (
    ID_uzytkownika INT PRIMARY KEY IDENTITY(1,1),
    imie VARCHAR(50) NOT NULL,
    nazwisko VARCHAR(50) NOT NULL,
    login VARCHAR(50) UNIQUE NOT NULL,
    haslo VARCHAR(255) NOT NULL,
    rola VARCHAR(50) NOT NULL
);

-- Tabela: automat_paczkowy
CREATE TABLE automat_paczkowy (
    ID_automat INT PRIMARY KEY IDENTITY(1,1),
    lokalizacja VARCHAR(100) NOT NULL
);

-- Tabela: transport
CREATE TABLE transport (
    ID_transport INT PRIMARY KEY IDENTITY(1,1),
    ID_uzytkownika INT NOT NULL,
    FOREIGN KEY (ID_uzytkownika) REFERENCES uzytkownicy(ID_uzytkownika)
);

-- Tabela: paczki_transport
CREATE TABLE paczki_transport (
    ID_paczki_transport INT PRIMARY KEY IDENTITY(1,1),
    ID_paczki INT NOT NULL,
    ID_transport INT NOT NULL,
    data_odbioru DATE NOT NULL,
    data_oddania DATE,
    FOREIGN KEY (ID_paczki) REFERENCES magazyn(ID_paczki),
    FOREIGN KEY (ID_transport) REFERENCES transport(ID_transport)
);

-- Tabela: paczki_automat
CREATE TABLE paczki_automat (
    ID_paczki_automat INT PRIMARY KEY IDENTITY(1,1),
    ID_paczki_transport INT NOT NULL,
    ID_automat INT NOT NULL,
    FOREIGN KEY (ID_paczki_transport) REFERENCES paczki_transport(ID_paczki_transport),
    FOREIGN KEY (ID_automat) REFERENCES automat_paczkowy(ID_automat)
);
