CREATE TABLE "Files"
(
    "Id" BIGINT PRIMARY KEY,
    "Edges" TEXT,
    "Text" TEXT,
    "EdgesColor" TEXT,
    "CreationDateTime" timestamp with time zone,
    "ModifyDateTime" timestamp with time zone,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT false
);

