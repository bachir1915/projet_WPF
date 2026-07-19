<?php
header('Content-Type: application/json');

include "db.php";

$id = isset($_POST['id']) ? (int)$_POST['id'] : 0;

if ($id <= 0) {
    echo json_encode(["success" => false, "message" => "ID invalide"]);
    exit;
}

$stmt = $db->prepare(
  "SELECT nom, prenom, age FROM utilisateur WHERE id = ?"
);

$stmt->execute([$id]);

$result = $stmt->fetch(PDO::FETCH_ASSOC);

echo json_encode([
  "result" => $result
]);
?>
