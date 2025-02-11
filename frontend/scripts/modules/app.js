//! ode treba stavit pravi import fetcha za filmove iz pravog api.js
import { fetchMovies } from "../../ivona-test/ivona-api.js";
import { displayMovieDetails } from "./movies.js";

document.addEventListener("DOMContentLoaded", async () => {
  const moviesContainer = document.getElementById("movies-container");
  const movies = await fetchMovies(); //! ili kakogod da se nazove funkcija za fethcanje filmova

  if (movies.length === 0) {
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

  document.querySelectorAll(".view-details").forEach((button) => {
    button.addEventListener("click", (e) => {
      const movieId = parseInt(e.target.dataset.id, 10);
      loadMovieDetails(movieId);
    });
  });
});

async function loadMovieDetails(movieId) {
  window.location.hash = `#movie-${movieId}`; //! ode treba dodat tocan url za dohvat filmova, prije znaka # ja mislim
  document.getElementById(
    "movies-container"
  ).innerHTML = `<p>Učitavanje detalja filma...</p>`;
  displayMovieDetails(movieId);
}
