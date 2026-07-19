<?php
header('Content-Type: application/json');
include "../db.php";

$id = isset($_POST['id']) ? (int)$_POST['id'] : 0;
$nom = $_POST['nom'] ?? '';
$prenom = $_POST['prenom'] ?? '';
$email = $_POST['email'] ?? '';
$id_classe = isset($_POST['id_classe']) ? (int)$_POST['id_classe'] : 0;

if ($id <= 0 || empty($nom) || empty($prenom) || empty($email) || $id_classe <= 0) {
    echo json_encode(["success" => false, "message" => "Données invalides"]);
    exit;
}

try {
    $stmt = $db->prepare("UPDATE etudiant SET nom = ?, prenom = ?, email = ?, id_classe = ? WHERE id = ?");
    $result = $stmt->execute([$nom, $prenom, $email, $id_classe, $id]);
    echo json_encode(["success" => $result]);
} catch (PDOException $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
?>
