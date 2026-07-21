<?php
// classe/update.php
require_once __DIR__ . '/../db.php';

$id = $_POST['id'] ?? 0;
$nom = $_POST['nom'] ?? '';

if (empty($id) || empty($nom)) {
    echo json_encode(["success" => false, "message" => "Données incomplètes"]);
    exit();
}

try {
    $stmt = $pdo->prepare("UPDATE classe SET nom = ? WHERE id = ?");
    $stmt->execute([$nom, $id]);
    echo json_encode(["success" => true, "message" => "Classe mise à jour"]);
} catch (Exception $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
