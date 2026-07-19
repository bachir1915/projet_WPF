<?php
header('Content-Type: application/json');

include "db.php";

$nom = $_POST['nom'] ?? '';
$prenom = $_POST['prenom'] ?? '';
$age = isset($_POST['age']) ? (int)$_POST['age'] : 0;

if (empty($nom) || empty($prenom)) {
    echo json_encode(["success" => false, "message" => "Nom et prénom obligatoires"]);
    exit;
}

$stmt = $db->prepare(
  "INSERT INTO utilisateur (nom, prenom, age) VALUES (?, ?, ?)"
);

$result = $stmt->execute([$nom, $prenom, $age]);

echo json_encode([
  "success" => $result
]);
?>
