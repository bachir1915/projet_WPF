<?php
$db_server = "127.0.0.1";
$db_user = "root";
$db_pass = "";
$db_name = "bdpersonne";

try {
    // 1. Connection to MySQL server
    $db = new PDO("mysql:host=$db_server;charset=utf8", $db_user, $db_pass);
    $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

    // 2. Create database
    $db->exec("CREATE DATABASE IF NOT EXISTS $db_name CHARACTER SET utf8 COLLATE utf8_general_ci");
    $db->exec("USE $db_name");

    // 3. Drop existing tables to ensure clean structure for new schema
    $db->exec("SET FOREIGN_KEY_CHECKS = 0;");
    $db->exec("DROP TABLE IF EXISTS note;");
    $db->exec("DROP TABLE IF EXISTS etudiant;");
    $db->exec("DROP TABLE IF EXISTS classe;");
    $db->exec("SET FOREIGN_KEY_CHECKS = 1;");

    // 4. Create table 'classe'
    $db->exec("CREATE TABLE classe (
        id INT AUTO_INCREMENT PRIMARY KEY,
        nom VARCHAR(100) NOT NULL UNIQUE
    ) ENGINE=InnoDB DEFAULT CHARSET=utf8;");

    // 5. Create table 'etudiant'
    $db->exec("CREATE TABLE etudiant (
        id INT AUTO_INCREMENT PRIMARY KEY,
        nom VARCHAR(100) NOT NULL,
        prenom VARCHAR(100) NOT NULL,
        email VARCHAR(150) NOT NULL UNIQUE,
        id_classe INT NOT NULL,
        FOREIGN KEY (id_classe) REFERENCES classe(id) ON DELETE CASCADE
    ) ENGINE=InnoDB DEFAULT CHARSET=utf8;");

    // 6. Create table 'note'
    $db->exec("CREATE TABLE note (
        id INT AUTO_INCREMENT PRIMARY KEY,
        id_etudiant INT NOT NULL,
        matiere VARCHAR(100) NOT NULL,
        note FLOAT NOT NULL,
        FOREIGN KEY (id_etudiant) REFERENCES etudiant(id) ON DELETE CASCADE
    ) ENGINE=InnoDB DEFAULT CHARSET=utf8;");

    // 7. Insert test dummy data
    // Classes
    $stmt = $db->prepare("INSERT INTO classe (nom) VALUES (?)");
    $stmt->execute(["Licence 3 GL"]);
    $id_l3gl = $db->lastInsertId();
    
    $stmt->execute(["Master 1 SI"]);
    $id_m1si = $db->lastInsertId();

    // Students
    $stmt = $db->prepare("INSERT INTO etudiant (nom, prenom, email, id_classe) VALUES (?, ?, ?, ?)");
    $stmt->execute(["Diop", "Awa", "awa.diop@mail.com", $id_l3gl]);
    $id_awa = $db->lastInsertId();

    $stmt->execute(["Fall", "Moussa", "moussa.fall@mail.com", $id_m1si]);
    $id_moussa = $db->lastInsertId();

    // Grades
    $stmt = $db->prepare("INSERT INTO note (id_etudiant, matiere, note) VALUES (?, ?, ?)");
    $stmt->execute([$id_awa, "C# Programming", 16.5]);
    $stmt->execute([$id_awa, "Database Admin", 14.0]);
    $stmt->execute([$id_moussa, "C# Programming", 18.0]);
    $stmt->execute([$id_moussa, "Web Services", 15.0]);

    echo "Base de données '$db_name' initialisée avec succès !<br>";
    echo "Tables 'classe', 'etudiant' et 'note' créées avec des données de test.<br>";
    echo "<strong>Configuration réussie !</strong>";

} catch (PDOException $e) {
    echo "Erreur lors de la configuration de la base de données : " . $e->getMessage();
}
?>
