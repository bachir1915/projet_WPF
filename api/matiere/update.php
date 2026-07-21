<?php
// matiere/update.php
require_once __DIR__ . '/../db.php';

$id = $_POST['id'] ?? 0;
$nom = $_POST['nom'] ?? '';
$coefficient = $_POST['coefficient'] ?? 1;
$id_enseignant = $_POST['id_enseignant'] ?? 0;

if (empty($id) || empty($nom)) {
    echo json_encode(["success" => false, "message" => "Données incomplètes"]);
    exit();
}

try {
    $stmt = $pdo->prepare("UPDATE matiere SET nom = ?, coefficient = ?, id_enseignant = ? WHERE id = ?");
    $stmt->execute([$nom, (int)$coefficient, (int)$id_enseignant, $id]);
    echo json_encode(["success" => true, "message" => "Matière mise à jour"]);
} catch (Exception $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
