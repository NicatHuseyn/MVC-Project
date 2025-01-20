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
// fiter product section start

//const categoryButtons = document.querySelectorAll(".categoryBtn");
//const menuItems = document.querySelectorAll(".menu1");

//categoryButtons.forEach((button) => {
//  button.addEventListener("click", () => {
//    const selectedCategory = button.getAttribute("data-category");
//    menuItems.forEach((item) => {
//      const itemCategory = item.getAttribute("data-category");
//      if (selectedCategory === "all" || itemCategory === selectedCategory) {
//        item.style.display = "inline-flex";
//      } else {
//        item.style.display = "none";
//      }
//    });
//  });
//});
$(function () {
    $(document).on('click', '#categories .categoryBtn', function (e) {
        e.preventDefault();
        let category = $(this).attr('data-id');
        console.log(category)
        let products = $('.menu1');

        console.log(products)

        products.each(function (e) {
            if (category === $(this).attr('data-id')) {
                  console.log(this)
                $(this).fadeIn();

            }
            else {
                $(this).hide();
            }
        })
        if (category === 'All') {
            products.fadeIn();
        }
    })
})


// fiter product section end
