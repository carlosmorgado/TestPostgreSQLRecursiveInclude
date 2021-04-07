CREATE TABLE "TestClasses" (
    "Id" UUID,
    PRIMARY KEY("Id"));
    
CREATE TABLE "TestClassLinks" (
    "Id" UUID,
    "FromTestClassId" UUID,
    "ToTestClassId" UUID,
    PRIMARY KEY("Id"),
    CONSTRAINT fk_from_testClases
        FOREIGN KEY("FromTestClassId")
            REFERENCES "TestClasses"("Id"),
    CONSTRAINT fk_to_testClases
        FOREIGN KEY("ToTestClassId")
            REFERENCES "TestClasses"("Id"));