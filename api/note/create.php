<?php
// note/create.php
require_once __DIR__ . '/../db.php';

$id_etudiant = $_POST['id_etudiant'] ?? 0;
$matiere = $_POST['matiere'] ?? '';
$note = $_POST['note'] ?? '';

if (empty($id_etudiant) || empty($matiere) || $note === '') {
    echo json_encode(["success" => false, "message" => "Tous les champs sont requis"]);
    exit();
}

try {
    $stmt = $pdo->prepare("INSERT INTO note (id_etudiant, matiere, valeur) VALUES (?, ?, ?)");
    $stmt->execute([$id_etudiant, $matiere, (float)$note]);
    $id = $pdo->lastInsertId();
    echo json_encode(["success" => true, "id" => $id, "message" => "Note créée"]);
} catch (Exception $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
