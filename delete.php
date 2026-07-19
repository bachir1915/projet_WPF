<?php
header('Content-Type: application/json');

include "db.php";

$id = isset($_POST['id']) ? (int)$_POST['id'] : 0;

if ($id <= 0) {
    echo json_encode(["success" => false, "message" => "ID invalide"]);
    exit;
}

$stmt = $db->prepare(
  "DELETE FROM utilisateur WHERE id = ?"
);

$result = $stmt->execute([$id]);
   
echo json_encode([
  "id" => $id,
  "success" => $result
]);
?>
