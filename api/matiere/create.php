<?php
// matiere/create.php
require_once __DIR__ . '/../db.php';

$nom = $_POST['nom'] ?? '';
$coefficient = $_POST['coefficient'] ?? 1;
$id_enseignant = $_POST['id_enseignant'] ?? 0;

if (empty($nom)) {
    echo json_encode(["success" => false, "message" => "Le nom de la matière est requis"]);
    exit();
}

try {
    $stmt = $pdo->prepare("INSERT INTO matiere (nom, coefficient, id_enseignant) VALUES (?, ?, ?)");
    $stmt->execute([$nom, (int)$coefficient, (int)$id_enseignant]);
    $id = $pdo->lastInsertId();
    echo json_encode(["success" => true, "id" => $id, "message" => "Matière créée"]);
} catch (Exception $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
