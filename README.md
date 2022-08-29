# OrganizationStructure
Pre fungovanie programu je nutne zmenit connection string v subore OrganizationStructureDbContext a nasledne v Package Manager Console spustit prikaz update-database.
Migracia zaroven vlozi do databazy par zamestnancov a vytvori jednoduchu strukturu firma -> 2x divizia -> 2x projekt -> 2x oddelenie a priradi veducich zamestnancov

Program by mal fungovat aj bez zmeny connection stringu. 
