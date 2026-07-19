<?php
header('Content-Type: application/json');
include "../db.php";

$nom = $_POST['nom'] ?? '';
if (empty($nom)) {
    echo json_encode(["success" => false, "message" => "Nom de classe obligatoire"]);
    exit;
}

try {
    $stmt = $db->prepare("INSERT INTO classe (nom) VALUES (?)");
    $result = $stmt->execute([$nom]);
    echo json_encode(["success" => $result, "id" => $db->lastInsertId()]);
} catch (PDOException $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
?>
