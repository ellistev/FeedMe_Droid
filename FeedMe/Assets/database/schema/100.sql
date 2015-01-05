CREATE TABLE "Locations"(
"Id" integer primary key autoincrement not null ,
"Name" varchar );
GO

CREATE TABLE "MMLocation"(
"Id" integer primary key autoincrement not null ,
"UUID" varchar ,
"Major" integer ,
"Minor" integer ,
"LocationId" integer );
GO

CREATE TABLE "GPSLocation"(
"Id" integer primary key autoincrement not null,
"BtDevicesId" integer,
"Address" varchar,
"LatitudeLongitude" varchar,
"Altitude" varchar );
GO

CREATE TABLE "BtDevices"(
"Id" integer primary key autoincrement not null ,
"Name" varchar ,
"Type" varchar ,
"MacAddress" varchar ,
"Strength" varchar ,
"UUID" varchar ,
"Major" varchar ,
"Minor" varchar,
"TimeFound" varchar
 );
GO

 CREATE TABLE "DBSchemaVersion"(
"Id" integer primary key autoincrement not null ,
"LastDBSchemaVersion" integer
 );
GO

  CREATE TABLE "DBDataVersion"(
"Id" integer primary key autoincrement not null ,
"LastDBDataVersion" integer
 );
GO