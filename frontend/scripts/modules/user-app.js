import { getMovieList } from "../api/api.js";
import { displayMovieDetails } from "./movie-details.js";
import { createLogoutButton } from "./logout.js";

export function initUserApp() {
  async function initialize() {
    const moviesContainer = document.getElementById("movies-container");

    const logoutButton = createLogoutButton();
    document.body.prepend(logoutButton);

    const filterContainer = document.createElement("div");
    filterContainer.classList.add("filter-container");

    const genreSelect = document.createElement("select");
    genreSelect.innerHTML = `
      <option value="">All Genres</option>
      <option value="Action">Action</option>
      <option value="Comedy">Comedy</option>
      <option value="Drama">Drama</option>
      <option value="Horror">Horror</option>
    `;

    const yearInput = document.createElement("input");
    yearInput.type = "number";
    yearInput.placeholder = "Enter year";
    yearInput.min = "1900";
    yearInput.max = new Date().getFullYear();

    const sortSelect = document.createElement("select");
    sortSelect.innerHTML = `
      <option value="">Sort by</option>
      <option value="rating_desc">Rating (High to Low)</option>
      <option value="rating_asc">Rating (Low to High)</option> //?
    `;

    const filterButton = document.createElement("button");
    filterButton.textContent = "Filter Movies";

    filterContainer.appendChild(genreSelect);
    filterContainer.appendChild(yearInput);
    filterContainer.appendChild(sortSelect);
    filterContainer.appendChild(filterButton);

    moviesContainer.before(filterContainer);

    async function displayMovies(filters = {}) {
      moviesContainer.innerHTML = "<p>Loading movies...</p>";

      let movies = await getMovieList(filters);
      moviesContainer.innerHTML = "";

      if (!movies || movies.length === 0) {
        moviesContainer.innerHTML = "<p>No movies available.</p>";
        return;
      }

      movies.forEach((movie) => {
        const movieElement = document.createElement("div");
        movieElement.classList.add("movie-item");

        movieElement.innerHTML = `
          <h3>${movie.title} (${movie.releaseYear})</h3>
          <p>${movie.summary}</p>
          <button data-id="${movie.id}" class="view-details">Details</button>
        `;

        moviesContainer.appendChild(movieElement);
      });

      document.querySelectorAll(".view-details").forEach((button) => {
        button.addEventListener("click", async (e) => {
          const movieId = parseInt(e.target.dataset.id, 10);
          loadMovieDetails(movieId);
        });
      });
    }

    filterButton.addEventListener("click", () => {
      const genre = genreSelect.value;
      const year = yearInput.value ? parseInt(yearInput.value, 10) : null;
      const sort = sortSelect.value;

      displayMovies({ genre, year, sort });
    });

    displayMovies();
  }

  async function loadMovieDetails(movieId) {
    window.location.hash = `#film-${movieId}`;
    document.getElementById(
      "movies-container"
    ).innerHTML = `<p>Loading the details data...</p>`;
    displayMovieDetails(movieId);
  }

  if (document.readyState === "loading")
    document.addEventListener("DOMContentLoaded", initialize);
  else initialize();
}
