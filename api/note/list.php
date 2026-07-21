<?php
// note/list.php
require_once __DIR__ . '/../db.php';

try {
    $sql = "SELECT n.id, n.id_etudiant, CONCAT(e.prenom, ' ', e.nom) AS nom_complet_etudiant, 
                   c.nom AS nom_classe, n.matiere, n.valeur 
            FROM note n 
            JOIN etudiant e ON n.id_etudiant = e.id 
            LEFT JOIN classe c ON e.id_classe = c.id 
            ORDER BY n.id DESC";
    $stmt = $pdo->query($sql);
    $notes = $stmt->fetchAll();
    echo json_encode($notes);
} catch (Exception $e) {
    echo json_encode([]);
}
