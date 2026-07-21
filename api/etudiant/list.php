<?php
// etudiant/list.php
require_once __DIR__ . '/../db.php';

try {
    $sql = "SELECT e.id, e.nom, e.prenom, e.email, e.id_classe, c.nom AS nom_classe 
            FROM etudiant e 
            LEFT JOIN classe c ON e.id_classe = c.id 
            ORDER BY e.nom ASC, e.prenom ASC";
    $stmt = $pdo->query($sql);
    $etudiants = $stmt->fetchAll();
    echo json_encode($etudiants);
} catch (Exception $e) {
    echo json_encode([]);
}
