<?php
header('Content-Type: application/json');
include "../db.php";

$id = isset($_POST['id']) ? (int)$_POST['id'] : 0;
$id_etudiant = isset($_POST['id_etudiant']) ? (int)$_POST['id_etudiant'] : 0;
$matiere = $_POST['matiere'] ?? '';
$note = isset($_POST['note']) ? (float)$_POST['note'] : -1.0;

if ($id <= 0 || $id_etudiant <= 0 || empty($matiere) || $note < 0 || $note > 20) {
    echo json_encode(["success" => false, "message" => "Données invalides"]);
    exit;
}

try {
    $stmt = $db->prepare("UPDATE note SET id_etudiant = ?, matiere = ?, note = ? WHERE id = ?");
    $result = $stmt->execute([$id_etudiant, $matiere, $note, $id]);
    echo json_encode(["success" => $result]);
} catch (PDOException $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
?>
