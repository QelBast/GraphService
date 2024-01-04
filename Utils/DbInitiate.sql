CREATE TABLE "Files"
(
    "Id" guid PRIMARY KEY,
    "Edges" TEXT,
    "Text" TEXT,
    "EdgesColor" TEXT,
    "NodesColor" TEXT,
    "CreationDateTime" timestamp with time zone,
    "ModifyDateTime" timestamp with time zone,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT false
);

