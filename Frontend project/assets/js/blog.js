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