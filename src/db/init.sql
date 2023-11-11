USE db;
CREATE TABLE nots (
   id VARCHAR(16) NOT NULL,
   conteudo TEXT,
   data_criacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
   PRIMARY KEY (id)
);

INSERT INTO db.nots (id, conteudo) VALUES ('dsdfghjklaqwerta','## vamos la\nTeste');
SELECT * FROM db.nots;
SELECT * FROM db.nots WHERE id='asdfghjklaqwerty';
