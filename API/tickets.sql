CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Users" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY AUTOINCREMENT,
    "UserName" TEXT NOT NULL,
    "Role" TEXT NOT NULL,
    "PasswordSalt" BLOB NOT NULL,
    "HashedPassword" BLOB NOT NULL
);

CREATE TABLE "Categories" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Categories" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NULL,
    "UserId" INTEGER NOT NULL,
    CONSTRAINT "FK_Categories_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Orders" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Orders" PRIMARY KEY AUTOINCREMENT,
    "CreatedDate" TEXT NOT NULL,
    "ConfirmedDate" TEXT NULL,
    "RemovedDate" TEXT NULL,
    "State" TEXT NOT NULL,
    "CustomerId" INTEGER NOT NULL,
    "HandlerId" INTEGER NULL,
    CONSTRAINT "FK_Orders_Users_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Users" ("Id"),
    CONSTRAINT "FK_Orders_Users_HandlerId" FOREIGN KEY ("HandlerId") REFERENCES "Users" ("Id")
);

CREATE TABLE "Products" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Products" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NULL,
    "CategoryId" INTEGER NOT NULL,
    "UnitPrice" INTEGER NOT NULL,
    CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
);

CREATE TABLE "OrdersProducts" (
    "OrderId" INTEGER NOT NULL,
    "ProductId" INTEGER NOT NULL,
    "UnitCount" INTEGER NOT NULL,
    "UnitPrice" INTEGER NOT NULL,
    CONSTRAINT "PK_OrdersProducts" PRIMARY KEY ("OrderId", "ProductId"),
    CONSTRAINT "FK_OrdersProducts_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_OrdersProducts_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Categories_Name" ON "Categories" ("Name");

CREATE INDEX "IX_Categories_UserId" ON "Categories" ("UserId");

CREATE INDEX "IX_Orders_CustomerId" ON "Orders" ("CustomerId");

CREATE INDEX "IX_Orders_HandlerId" ON "Orders" ("HandlerId");

CREATE INDEX "IX_OrdersProducts_ProductId" ON "OrdersProducts" ("ProductId");

CREATE INDEX "IX_Products_CategoryId" ON "Products" ("CategoryId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241015105356_Initial', '8.0.8');

COMMIT;

