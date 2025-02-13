import { getMovieList } from "../api/api.js";
// import { displayMovieDetails } from "./movies.js"; ?

console.log("aaaaaa");
export function initAdminApp() {
  console.log("fffff");

  async function initialize() {
    console.log("cccccc");

    const moviesContainer = document.getElementById("movies-container");

    let movies = await getMovieList();

    if (!movies || movies.length === 0) {
      moviesContainer.innerHTML = "<p>Nema dostupnih filmova.</p>";
      return;
    }

    movies.forEach((movie) => {
      const movieElement = document.createElement("div");
      movieElement.classList.add("movie-item");

      movieElement.innerHTML = `
        <h3>${movie.title} (${movie.release_year})</h3>
        <p>${movie.summary}</p>
        <button data-id="${movie.id}" class="view-details">Pogledaj detalje</button>
      `;

      moviesContainer.appendChild(movieElement);
    });

    // Ovo treba dovršiti dalje
    document.querySelectorAll(".view-details").forEach((button) => {
      button.addEventListener("click", async (e) => {
        const movieId = parseInt(e.target.dataset.id, 10);

        loadMovieDetails(movieId);
      });
    });
  }

  async function loadMovieDetails(movieId) {
    window.location.hash = `#film-${movieId}`;

    document.getElementById(
      "movies-container"
    ).innerHTML = `<p>Učitavanje detalja filma...</p>`;

    displayMovieDetails(movieId);
  }

  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", initialize);
  } else {
    initialize();
  }
}
