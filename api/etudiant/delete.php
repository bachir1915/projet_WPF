<?php
// etudiant/delete.php
require_once __DIR__ . '/../db.php';

$id = $_POST['id'] ?? 0;

if (empty($id)) {
    echo json_encode(["success" => false, "message" => "ID manquant"]);
    exit();
}

try {
    $stmt = $pdo->prepare("DELETE FROM etudiant WHERE id = ?");
    $stmt->execute([$id]);
    echo json_encode(["success" => true, "message" => "Étudiant supprimé"]);
} catch (Exception $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
