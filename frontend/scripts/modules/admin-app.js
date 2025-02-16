import { getMovieList } from "../api/api.js";
import { displayMovieDetails } from "./movie-details.js";
import { createLogoutButton } from "./logout.js";
import { getAuthToken } from "./auth.js";

export function initAdminApp() {
  async function initialize() {
    const moviesContainer = document.getElementById("movies-container");

    const logoutButton = createLogoutButton();
    document.body.prepend(logoutButton);

    let movies = await getMovieList();
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

  async function loadMovieDetails(movieId) {
    window.location.hash = `#film-${movieId}`;

    document.getElementById(
      "movies-container"
    ).innerHTML = `<p>Loading the details data...</p>`;

    displayMovieDetails(movieId);
  }
  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", initialize);
  } else {
    initialize();
  }
}
