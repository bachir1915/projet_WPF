<?php
header('Content-Type: application/json');
include "../db.php";

$id_etudiant = isset($_POST['id_etudiant']) ? (int)$_POST['id_etudiant'] : 0;
$matiere = $_POST['matiere'] ?? '';
$note = isset($_POST['note']) ? (float)$_POST['note'] : -1.0;

if ($id_etudiant <= 0 || empty($matiere) || $note < 0 || $note > 20) {
    echo json_encode(["success" => false, "message" => "Données invalides. Note doit être comprise entre 0 et 20."]);
    exit;
}

try {
    $stmt = $db->prepare("INSERT INTO note (id_etudiant, matiere, note) VALUES (?, ?, ?)");
    $result = $stmt->execute([$id_etudiant, $matiere, $note]);
    echo json_encode(["success" => $result, "id" => $db->lastInsertId()]);
} catch (PDOException $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
?>
