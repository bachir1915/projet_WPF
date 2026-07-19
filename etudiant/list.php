<?php
header('Content-Type: application/json');
include "../db.php";

$stmt = $db->prepare("
    SELECT e.id, e.nom, e.prenom, e.email, e.id_classe, c.nom AS nom_classe 
    FROM etudiant e 
    JOIN classe c ON e.id_classe = c.id
    ORDER BY e.id DESC
");
$stmt->execute();
$result = $stmt->fetchAll(PDO::FETCH_ASSOC);
echo json_encode($result);
?>
