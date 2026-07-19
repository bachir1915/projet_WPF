<?php
header('Content-Type: application/json');
include "../db.php";

$nom = $_POST['nom'] ?? '';
$prenom = $_POST['prenom'] ?? '';
$email = $_POST['email'] ?? '';
$id_classe = isset($_POST['id_classe']) ? (int)$_POST['id_classe'] : 0;

if (empty($nom) || empty($prenom) || empty($email) || $id_classe <= 0) {
    echo json_encode(["success" => false, "message" => "Tous les champs sont obligatoires"]);
    exit;
}

try {
    $stmt = $db->prepare("INSERT INTO etudiant (nom, prenom, email, id_classe) VALUES (?, ?, ?, ?)");
    $result = $stmt->execute([$nom, $prenom, $email, $id_classe]);
    echo json_encode(["success" => $result, "id" => $db->lastInsertId()]);
} catch (PDOException $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
?>
