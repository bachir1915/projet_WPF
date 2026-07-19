<?php
header('Content-Type: application/json');
include "../db.php";

$stmt = $db->prepare("
    SELECT c.id, c.nom, COUNT(e.id) AS nombre_etudiants 
    FROM classe c 
    LEFT JOIN etudiant e ON c.id = e.id_classe 
    GROUP BY c.id
");
$stmt->execute();
$result = $stmt->fetchAll(PDO::FETCH_ASSOC);
echo json_encode($result);
?>
