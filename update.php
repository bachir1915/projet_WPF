<?php
header('Content-Type: application/json');

include "db.php";

$id = isset($_POST['id']) ? (int)$_POST['id'] : 0;
$nom = $_POST['nom'] ?? '';
$prenom = $_POST['prenom'] ?? '';
$age = isset($_POST['age']) ? (int)$_POST['age'] : 0;

if ($id <= 0 || empty($nom) || empty($prenom)) {
    echo json_encode(["success" => false, "message" => "Données invalides"]);
    exit;
}

$stmt = $db->prepare(
  "UPDATE utilisateur
    SET nom = ?, prenom = ?, age = ?
    WHERE id = ?"
);

$result = $stmt->execute([$nom, $prenom, $age, $id]);

echo json_encode([
  "success" => $result
]);
?>
