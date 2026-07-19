<?php
header('Content-Type: application/json');
include "../db.php";

$id = isset($_POST['id']) ? (int)$_POST['id'] : 0;
$nom = $_POST['nom'] ?? '';

if ($id <= 0 || empty($nom)) {
    echo json_encode(["success" => false, "message" => "Données invalides"]);
    exit;
}

try {
    $stmt = $db->prepare("UPDATE classe SET nom = ? WHERE id = ?");
    $result = $stmt->execute([$nom, $id]);
    echo json_encode(["success" => $result]);
} catch (PDOException $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
?>
