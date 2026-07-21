<?php
// matiere/list.php
require_once __DIR__ . '/../db.php';

try {
    $sql = "SELECT m.id, m.nom, m.coefficient, m.id_enseignant, 
                   IFNULL(CONCAT(ens.prenom, ' ', ens.nom), 'Aucun') AS nom_enseignant 
            FROM matiere m 
            LEFT JOIN enseignant ens ON m.id_enseignant = ens.id 
            ORDER BY m.nom ASC";
    $stmt = $pdo->query($sql);
    $matieres = $stmt->fetchAll();
    echo json_encode($matieres);
} catch (Exception $e) {
    echo json_encode([]);
}
