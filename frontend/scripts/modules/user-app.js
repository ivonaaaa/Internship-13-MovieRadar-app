import { getMovieList } from "../api/api.js";
import { displayMovieDetails } from "./movie-details.js";
import { createLogoutButton } from "./logout.js";

export function initUserApp() {
  async function initialize() {
    const moviesContainer = document.getElementById("movies-container");

    let header = document.getElementById("movies-header");
    if (!header) {
      header = document.createElement("h2");
      header.id = "movies-header";
      header.textContent = "Movies List";
      document.body.insertBefore(header, moviesContainer);
    }

    const existingFilterContainer = document.querySelector(".filter-container");
    if (existingFilterContainer) {
      existingFilterContainer.remove();
    }

    const logoutButton = createLogoutButton();
    document.body.prepend(logoutButton);

    const filterContainer = document.createElement("div");
    filterContainer.classList.add("filter-container");

    const genreSelect = document.createElement("select");
    genreSelect.innerHTML = `
      <option value="">All Genres</option>
      <option value="Action">action</option>
      <option value="Comedy">comedy</option>
      <option value="Drama">drama</option>
      <option value="Horror">horror</option>
      <option value="Crime">crime</option>
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
      <option value="rating_asc">Rating (Low to High)</option>
    `;

    const filterButton = document.createElement("button");
    filterButton.textContent = "Filter Movies";

    const sortAlphabeticallyButton = document.createElement("button");
    sortAlphabeticallyButton.textContent = "Sort A-Z";

    filterContainer.appendChild(genreSelect);
    filterContainer.appendChild(yearInput);
    filterContainer.appendChild(sortSelect);
    filterContainer.appendChild(filterButton);
    filterContainer.appendChild(sortAlphabeticallyButton);

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
          <img src="${movie.imageLink || "placeholder.jpg"}" alt="${
          movie.title
        }" class="movie-cover" />
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

    function sortMoviesAlphabetically() {
      const movieItems = Array.from(document.querySelectorAll(".movie-item"));

      const sortedMovies = movieItems.sort((a, b) => {
        const titleA = a.querySelector("h3").textContent.toLowerCase();
        const titleB = b.querySelector("h3").textContent.toLowerCase();
        return titleA.localeCompare(titleB);
      });

      moviesContainer.innerHTML = "";
      sortedMovies.forEach((movie) => moviesContainer.appendChild(movie));
    }

    filterButton.addEventListener("click", () => {
      const genre = genreSelect.value;
      const year = yearInput.value ? parseInt(yearInput.value, 10) : null;
      const sort = sortSelect.value;

      displayMovies({ genre, year, sort });
    });

    sortAlphabeticallyButton.addEventListener(
      "click",
      sortMoviesAlphabetically
    );

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
