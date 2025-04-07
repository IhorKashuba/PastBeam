

const sidebar = document.getElementById("sidebar");
const menuToggle = document.getElementById("menu-toggle");
const menuClose = document.getElementById("menu-close");

menuToggle.addEventListener("click", function () {
    sidebar.classList.toggle("open");
});

menuClose.addEventListener("click", function () {
    sidebar.classList.remove("open");
});