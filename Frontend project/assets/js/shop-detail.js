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
// shop detail product area start
const allHoverImages = document.querySelectorAll(".small-image div img");
const imgContainer = document.querySelector(".img-container");
window.addEventListener("DOMContentLoaded", () => {
  allHoverImages[0].parentElement.classList.add("active");
});
allHoverImages.forEach((image) => {
  image.addEventListener("mouseover", () => {
    imgContainer.querySelector("img").src = image.src;
    resetActiveImg();
    image.parentElement.classList.add("active");
  });
});
function resetActiveImg() {
  allHoverImages.forEach((img) => {
    img.parentElement.classList.remove("active");
  });
}
// shop detail product area end
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

closeModal.addEventListener("click", (event) => {
  event.preventDefault();
  if (event.target === modal) {
    modal.style.display = "none";
  }
});
// modal section end
