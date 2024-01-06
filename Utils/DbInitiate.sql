CREATE TABLE "Nodes"
(
    "Id" BIGINT PRIMARY KEY NOT NULL,
    "Color" TEXT DEFAULT 'Black',
    "Shape" TEXT DEFAULT 'Ellipse',
    "Text" TEXT NOT NULL DEFAULT '',
    "Label" TEXT
);

CREATE TABLE "Edges"
(
    "Id" BIGINT PRIMARY KEY NOT NULL,
    "Color" TEXT DEFAULT 'Red',
    "FromNodeId" REFERENCES "Nodes" ("Id"),
    "ToNodeId" REFERENCES  "Nodes" ("Id"),
    "Label" TEXT NOT NULL DEFAULT ''
);

CREATE TABLE "GraphEdgesCollections"
(
    "Id" BIGINT PRIMARY KEY NOT NULL,
    "EdgeId" BIGINT REFERENCES "Edges" ("Id"),
);

CREATE TABLE "Files"
(
    "Id" UUID PRIMARY KEY NOT NULL DEFAULT gen_random_uuid(),
    "GraphId" BIGINT REFERENCES "GraphInfos" ("Id"),
    "Text" TEXT NOT NULL,
    "IsDirected" BOOLEAN NOT NULL DEFAULT false,
    "CreationDateTime" timestamp with time zone,
    "ModifyDateTime" timestamp with time zone,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT false
);
