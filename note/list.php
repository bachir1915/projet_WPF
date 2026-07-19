<?php
header('Content-Type: application/json');
include "../db.php";

$stmt = $db->prepare("
    SELECT n.id, n.id_etudiant, CONCAT(e.prenom, ' ', e.nom) AS nom_complet_etudiant, 
           c.nom AS nom_classe, n.matiere, n.note AS valeur 
    FROM note n 
    JOIN etudiant e ON n.id_etudiant = e.id 
    JOIN classe c ON e.id_classe = c.id
    ORDER BY n.id DESC
");
$stmt->execute();
$result = $stmt->fetchAll(PDO::FETCH_ASSOC);
echo json_encode($result);
?>
