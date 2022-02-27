/*
 Navicat Premium Data Transfer

 Source Server         : BAMeta_000
 Source Server Type    : SQLite
 Source Server Version : 3030001
 Source Schema         : main

 Target Server Type    : SQLite
 Target Server Version : 3030001
 File Encoding         : 65001

 Date: 27/02/2022 21:58:24
*/

PRAGMA foreign_keys = false;

-- ----------------------------
-- Table structure for DatasourceConnection
-- ----------------------------
DROP TABLE IF EXISTS "DatasourceConnection";
CREATE TABLE "DatasourceConnection" (
  "IID" VARCHAR (50) NOT NULL,
  "Code" VARCHAR (50),
  "Name" VARCHAR (64),
  "ServerName" VARCHAR (64),
  "UserName" VARCHAR (64),
  "Password" VARCHAR (64),
  "DatabaseName" VARCHAR (50),
  "DatabaseType" VARCHAR (50),
  "SystemName" VARCHAR (50),
  PRIMARY KEY ("IID"),
  CONSTRAINT "test" UNIQUE ("IID" ASC)
);

-- ----------------------------
-- Records of DatasourceConnection
-- ----------------------------
INSERT INTO "DatasourceConnection" VALUES ('91768f72-77b6-492d-8b21-76501680eafb', 'U8OLAP', '目标数据库', '39.104.169.202', 'sa', 'sy2018!', 'SYOLAP', 'SQLServer', 'U8OLAP');
INSERT INTO "DatasourceConnection" VALUES ('cfb701de-a1e2-467b-8996-409684d4252f', 'NCDATA', '源数据库连接', '', '', '', '', 'oracle', 'NC');
INSERT INTO "DatasourceConnection" VALUES ('91e34813-60f1-4381-9788-79d97169e79f', 'NCOLAP', '目标数据库', '', '', '', '', 'oracle', 'NCOLAP');
INSERT INTO "DatasourceConnection" VALUES ('a393bc01-c6a6-4838-86f8-62676badbbd2', 'K3OLAP', '目标数据库', '', '', '', '', 'SQLServer', 'K3OLAP');
INSERT INTO "DatasourceConnection" VALUES ('bf124a2f-8550-4255-affb-84ccfd08d5ef', 'SP4', '源数据库', '', '', '', '', 'SQLServer', 'P4');
INSERT INTO "DatasourceConnection" VALUES ('3fbe74b1-9cab-4dff-8276-886f2156c59b', 'DP4', '目标数据库', '', '', '', '', 'SQLServer', 'P4OLAP');
INSERT INTO "DatasourceConnection" VALUES ('08228884-9d06-4bfd-89e9-f601d85685bc', 'EASDATA', '源数据库', '', '', '', '', 'SQLServer', 'EAS');
INSERT INTO "DatasourceConnection" VALUES ('6584531d-628f-417c-8260-d845fd34ae1c', 'CJTDATA', '源数据库', '', '', '', '', 'SQLServer', 'TPlus');
INSERT INTO "DatasourceConnection" VALUES ('d906b291-487a-48b6-9ae0-ec95c0e5c43b', 'U8CS-S', '源数据库', '', '', '', '', 'SQLServer', 'P50');
INSERT INTO "DatasourceConnection" VALUES ('f2ef527c-ea72-444b-9518-b763bb288f29', 'U8CS-D', '目标数据库', '', '', '', '', 'SQLServer', 'P50OLAP');
INSERT INTO "DatasourceConnection" VALUES ('9c5eabfa-6439-48d7-8d17-a771cb381e7e', 'P53-S', '源数据库', '', '', '', '', 'oracle', 'P53');
INSERT INTO "DatasourceConnection" VALUES ('54544d4b-debe-4c79-81bd-7f7481bf37dd', 'Excel', 'olap', '39.104.169.202', 'sa', 'sy2018', 'U8OLAP', 'SQLServer', 'Excel');
INSERT INTO "DatasourceConnection" VALUES ('f746f94a-d4c0-4707-ad87-7507372a6cb0', 999, '星空演示', '39.104.169.202', 'sa', 'sy2018!', 'UFDATA_999_2014', 'SQLServer', 'U8;2014');
INSERT INTO "DatasourceConnection" VALUES ('56ba8fc9-c890-4f65-8237-7ef9f61ca03d', 222, 222, '39.104.169.202', 'sa', 'sy2018!', 'UFDATA_999_2014', 'SQLServer', 'U8;2014');
INSERT INTO "DatasourceConnection" VALUES ('09fc603b-89d7-4658-9f5d-257af7e2afc0', 'Excel', 'localhost', '39.104.169.202', 'sa', 'sy2018!', 'Test', 'SQLServer', 'Excel');

PRAGMA foreign_keys = true;
