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
// price filter start
const inputRange = document.querySelectorAll(".input-range input");
const inputPrice = document.querySelectorAll(".price-input input");
let progress = document.querySelector(".progress-bar");

const priceGap = 200;

inputPrice.forEach((input) => {
  input.addEventListener("input", (e) => {
    let minVal = parseInt(inputPrice[0].value);
    let maxVal = parseInt(inputPrice[1].value);
    if (maxVal - minVal >= priceGap && maxVal <= 10000 && minVal >= 0) {
      if (e.target.className === "min-input") {
        inputRange[0].value = minVal;
        progress.style.left = (minVal / inputRange[0].max) * 100 + "%";
      } else {
        inputRange[1].value = maxVal;
        progress.style.right = 100 - (maxVal / inputRange[1].max) * 100 + "%";
      }
    }
  });
});

inputRange.forEach((input) => {
  input.addEventListener("input", (e) => {
    let minVal = parseInt(inputRange[0].value);
    let maxVal = parseInt(inputRange[1].value);
    if (maxVal - minVal < priceGap) {
      if (e.target.className === "range-min") {
        inputRange[0].value = maxVal - priceGap;
      } else {
        inputRange[1].value = minVal + priceGap;
      }
    } else {
      inputPrice[0].value = minVal;
      inputPrice[1].value = maxVal;
      progress.style.left = (minVal / inputRange[0].max) * 100 + "%";
      progress.style.right = 100 - (maxVal / inputRange[1].max) * 100 + "%";
    }
  });
});
// price filter end
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
