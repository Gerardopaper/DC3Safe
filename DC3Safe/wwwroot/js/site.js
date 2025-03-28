function showToast(content) {
    var toast = bootstrap.Toast.getOrCreateInstance(document.getElementById("toast"));
    document.getElementById("toast-body").innerHTML = content;
    toast.show();
}