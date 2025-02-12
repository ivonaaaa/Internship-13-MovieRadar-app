import { getMovieList } from "./api.js";
//import { displayMovieDetails } from "./movies.js";

document.addEventListener("DOMContentLoaded", async () => {
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

  //ovo treba dovrsit dalje
  document.querySelectorAll(".view-details").forEach((button) => {
    button.addEventListener("click", async (e) => {
      const movieId = parseInt(e.target.dataset.id, 10);

      loadMovieDetails(movieId);
    });
  });
});

async function loadMovieDetails(movieId) {
  window.location.hash = `#film-${movieId}`;

  document.getElementById(
    "movies-container"
  ).innerHTML = `<p>Učitavanje detalja filma...</p>`;

  displayMovieDetails(movieId);
}
