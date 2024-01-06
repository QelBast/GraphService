CREATE TABLE "Files"
(
    "Id" UUID PRIMARY KEY NOT NULL DEFAULT gen_random_uuid(),
    "Edges" TEXT NOT NULL DEFAULT '[]',
    "Text" TEXT NOT NULL DEFAULT '',
    "EdgesColor" TEXT DEFAULT 'Red',
    "NodesColor" TEXT DEFAULT 'Black',
    "IsDirected" BOOLEAN DEFAULT false,
    "CreationDateTime" timestamp with time zone,
    "ModifyDateTime" timestamp with time zone,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT false
);

