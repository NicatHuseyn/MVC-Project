// header section start
let responsivNavbar = document.querySelector(".responsive-header-container");
document.getElementById("menu-btn").onclick = () => {
  responsivNavbar.classList.toggle("active");
  cartItem.classList.remove("active");
  search.classList.remove("active");
};
let search = document.querySelector(".search-form");
document.getElementById("search-btn").onclick = () => {
  search.classList.toggle("active");
  responsivNavbar.classList.remove("active");
  cartItem.classList.remove("active");
};
let cartItem = document.querySelector(".cart-items-container");
document.getElementById("cart-btn").onclick = () => {
  cartItem.classList.toggle("active");
  responsivNavbar.classList.remove("active");
  search.classList.remove("active");
};
window.onscroll = () => {
  responsivNavbar.classList.remove("active");
  cartItem.classList.remove("active");
  search.classList.remove("active");
};
// header section end
// swipper slider start
var swiper = new Swiper(".swiper-container", {
  effect: "flip",
  grabCursor: true,
  loop: true,
  pagination: {
    el: ".swipper-pagination",
  },
  navigation: {
    nextEl: ".swiper-button-next",
    prevEl: ".swiper-button-prev",
  },
});
// swipper slider start
// scroll top start

let calcScrollValue = () => {
  let scrollProgress = document.getElementById("progress");
  let progressValue = document.getElementById("progress-value");
  let pos = document.documentElement.scrollTop;
  let calcHeight =
    document.documentElement.scrollHeight -
    document.documentElement.clientHeight;
  let scrollValue = Math.round((pos * 100) / calcHeight);
  if (pos > 100) {
    scrollProgress.style.display = "grid";
  } else {
    scrollProgress.style.display = "none";
  }
  scrollProgress.addEventListener("click", () => {
    document.documentElement.scrollTop = 0;
  });
};

window.onscroll = calcScrollValue;
window.onload = calcScrollValue;
// scroll top end
// modal section start
const openModalIcon = document.querySelectorAll(".fa-eye");
const modal = document.getElementById("myModal");
const closeModal = modal.querySelector(".close");

openModalIcon.forEach((eyeIcon) => {
  eyeIcon.addEventListener("click", () => {
    modal.style.display = "block";
  });
});

closeModal.addEventListener("click", () => {
  modal.style.display = "none";
});


// modal section end
