<?php
// etudiant/create.php
require_once __DIR__ . '/../db.php';

$nom = $_POST['nom'] ?? '';
$prenom = $_POST['prenom'] ?? '';
$email = $_POST['email'] ?? '';
$id_classe = $_POST['id_classe'] ?? 0;

if (empty($nom) || empty($prenom) || empty($email) || empty($id_classe)) {
    echo json_encode(["success" => false, "message" => "Tous les champs sont requis"]);
    exit();
}

try {
    $stmt = $pdo->prepare("INSERT INTO etudiant (nom, prenom, email, id_classe) VALUES (?, ?, ?, ?)");
    $stmt->execute([$nom, $prenom, $email, $id_classe]);
    $id = $pdo->lastInsertId();
    echo json_encode(["success" => true, "id" => $id, "message" => "Étudiant créé"]);
} catch (Exception $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
