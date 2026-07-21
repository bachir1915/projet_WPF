<?php
// classe/create.php
require_once __DIR__ . '/../db.php';

$nom = $_POST['nom'] ?? '';

if (empty($nom)) {
    echo json_encode(["success" => false, "message" => "Le nom de la classe est requis"]);
    exit();
}

try {
    $stmt = $pdo->prepare("INSERT INTO classe (nom) VALUES (?)");
    $stmt->execute([$nom]);
    $id = $pdo->lastInsertId();
    echo json_encode(["success" => true, "id" => $id, "message" => "Classe créée"]);
} catch (Exception $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
