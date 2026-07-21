<?php
// classe/list.php
require_once __DIR__ . '/../db.php';

try {
    $sql = "SELECT c.id, c.nom, COUNT(e.id) AS nombre_etudiants 
            FROM classe c 
            LEFT JOIN etudiant e ON c.id = e.id_classe 
            GROUP BY c.id, c.nom 
            ORDER BY c.nom ASC";
    $stmt = $pdo->query($sql);
    $classes = $stmt->fetchAll();
    echo json_encode($classes);
} catch (Exception $e) {
    echo json_encode([]);
}
