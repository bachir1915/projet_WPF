<?php
// enseignant/create.php
require_once __DIR__ . '/../db.php';

$nom = $_POST['nom'] ?? '';
$prenom = $_POST['prenom'] ?? '';
$email = $_POST['email'] ?? '';
$specialite = $_POST['specialite'] ?? '';
$id_classe = $_POST['id_classe'] ?? 0;

if (empty($nom) || empty($prenom) || empty($email) || empty($specialite)) {
    echo json_encode(["success" => false, "message" => "Tous les champs requis ne sont pas remplis"]);
    exit();
}

try {
    $stmt = $pdo->prepare("INSERT INTO enseignant (nom, prenom, email, specialite, id_classe) VALUES (?, ?, ?, ?, ?)");
    $stmt->execute([$nom, $prenom, $email, $specialite, $id_classe]);
    $id = $pdo->lastInsertId();
    echo json_encode(["success" => true, "id" => $id, "message" => "Enseignant créé"]);
} catch (Exception $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
