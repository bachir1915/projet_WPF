<?php
// etudiant/update.php
require_once __DIR__ . '/../db.php';

$id = $_POST['id'] ?? 0;
$nom = $_POST['nom'] ?? '';
$prenom = $_POST['prenom'] ?? '';
$email = $_POST['email'] ?? '';
$id_classe = $_POST['id_classe'] ?? 0;

if (empty($id) || empty($nom) || empty($prenom) || empty($email) || empty($id_classe)) {
    echo json_encode(["success" => false, "message" => "Données incomplètes"]);
    exit();
}

try {
    $stmt = $pdo->prepare("UPDATE etudiant SET nom = ?, prenom = ?, email = ?, id_classe = ? WHERE id = ?");
    $stmt->execute([$nom, $prenom, $email, $id_classe, $id]);
    echo json_encode(["success" => true, "message" => "Étudiant mis à jour"]);
} catch (Exception $e) {
    echo json_encode(["success" => false, "message" => $e->getMessage()]);
}
