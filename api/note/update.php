<?php
// note/update.php
require_once __DIR__ . '/../db.php';

$id = $_POST['id'] ?? 0;
$id_etudiant = $_POST['id_etudiant'] ?? 0;
$matiere = $_POST['matiere'] ?? '';
$note = $_POST['note'] ?? '';

if (empty($id) || empty($id_etudiant) || empty($matiere) || $note === '') {
    echo json_encode(["success" => false, "message" => "Données incomplètes"]);
    exit();
}

try {
    $stmt = $pdo->prepare("UPDATE note SET id_etudiant = ?, matiere = ?, valeur = ? WHERE id = ?");
    $stmt->execute([$id_etudiant, $matiere, (float)$note, $id]);
    echo json_encode(["success" => true, "message" => "Note mise à jour"]);
} catch (Exception $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
