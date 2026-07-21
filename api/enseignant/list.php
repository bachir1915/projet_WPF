<?php
// enseignant/list.php
require_once __DIR__ . '/../db.php';

try {
    $sql = "SELECT e.id, e.nom, e.prenom, e.email, e.specialite, e.id_classe, 
                   IFNULL(c.nom, 'Non assigné') AS nom_classe 
            FROM enseignant e 
            LEFT JOIN classe c ON e.id_classe = c.id 
            ORDER BY e.nom ASC, e.prenom ASC";
    $stmt = $pdo->query($sql);
    $enseignants = $stmt->fetchAll();
    echo json_encode($enseignants);
} catch (Exception $e) {
    echo json_encode([]);
}
