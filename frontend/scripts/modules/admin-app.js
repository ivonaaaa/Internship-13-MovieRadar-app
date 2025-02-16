import {
  getMovieList,
  createMovie,
  updateMovie,
  deleteMovie,
} from "../api/api.js";
import { displayMovieDetails } from "./movie-details.js";
import { createLogoutButton } from "./logout.js";
import { getAuthToken } from "./auth.js";

const genres = ["action", "crime", "horror", "comedy", "drama"];

export function initAdminApp() {
  async function initialize() {
    const listUsersButton = document.createElement("button");
    listUsersButton.id = "users-btn";
    listUsersButton.innerText = "Users list";
    listUsersButton.addEventListener("click", () => {
      // Preusmjeri na users.html
      window.location.href = "users.html";
    });
    document.body.prepend(listUsersButton);
    
    const moviesContainer = document.getElementById("movies-container");

    const header = document.createElement("h2");
    header.textContent = "Movies List";
    document.body.insertBefore(header, moviesContainer);

    const addNewButton = document.createElement("button");
    addNewButton.textContent = "Add New";
    addNewButton.classList.add("add-new");
    addNewButton.addEventListener("click", () => openMovieModal());
    header.insertAdjacentElement("afterend", addNewButton);

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
        <button data-id="${movie.id}" class="edit-movie">Edit</button>
        <button data-id="${movie.id}" class="delete-movie">Delete</button>
      `;

      moviesContainer.appendChild(movieElement);
    });

    document.querySelectorAll(".view-details").forEach((button) => {
      button.addEventListener("click", async (e) => {
        e.preventDefault();
        const movieId = parseInt(e.target.dataset.id, 10);
        loadMovieDetails(movieId);
      });
    });

    document.querySelectorAll(".edit-movie").forEach((button) => {
      button.addEventListener("click", (e) => {
        e.preventDefault();
        const movieId = parseInt(e.target.dataset.id, 10);
        openMovieModal(movieId);
      });
    });

    document.querySelectorAll(".delete-movie").forEach((button) => {
      button.addEventListener("click", async (e) => {
        e.preventDefault();
        const movieId = parseInt(e.target.dataset.id, 10);
        if (confirm("Are you sure you want to delete this movie?")) {
          try {
            await deleteMovie(movieId);
            location.reload();
          } catch (error) {
            alert("Failed to delete movie.");
          }
        }
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

  function ensureMovieModalExists() {
    let modal = document.getElementById("movie-modal");

    if (!modal) {
      modal = document.createElement("div");
      modal.id = "movie-modal";
      modal.style.display = "none";
      modal.innerHTML = `
        <div class="modal-content">
          <h2>Movie Form</h2>
          <form id="movie-form">
            <label for="movie-title">Title:</label>
            <input type="text" id="movie-title" required />
            
            <label for="movie-year">Release Year:</label>
            <input type="number" id="movie-year" required />

            <label for="movie-summary">Summary:</label>
            <textarea id="movie-summary" required></textarea>

            <label for="movie-genre">Genre:</label>
            <select id="movie-genre" required>
              ${genres
                .map((genre) => `<option value="${genre}">${genre}</option>`)
                .join("")}
            </select>

            <button type="submit">Save</button>
            <button type="button" id="close-modal">Cancel</button>
          </form>
        </div>
      `;
      document.body.appendChild(modal);
      document.getElementById("close-modal").addEventListener("click", () => {
        modal.style.display = "none";
      });
    }
    return modal;
  }

  function openMovieModal(movieId = null) {
    const modal = ensureMovieModalExists();
    const form = document.getElementById("movie-form");
    const titleInput = document.getElementById("movie-title");
    const yearInput = document.getElementById("movie-year");
    const summaryInput = document.getElementById("movie-summary");
    const genreSelect = document.getElementById("movie-genre");

    form.reset();

    modal.style.display = "block";

    form.onsubmit = async (e) => {
      e.preventDefault();
      const movieData = {
        title: titleInput.value,
        releaseYear: parseInt(yearInput.value, 10),
        summary: summaryInput.value,
        genre: genreSelect.value,
      };

      try {
        let resultMovie;
        if (movieId) {
          resultMovie = await updateMovie(movieId, movieData);

          const movieElement = document.querySelector(
            `[data-id='${movieId}']`
          ).parentElement;
          if (movieElement) {
            movieElement.querySelector(
              "h3"
            ).textContent = `${resultMovie.title} (${resultMovie.releaseYear})`;
            movieElement.querySelector("p").textContent = resultMovie.summary;
          }
        } else {
          resultMovie = await createMovie(movieData);

          const moviesContainer = document.getElementById("movies-container");
          const movieElement = document.createElement("div");
          movieElement.classList.add("movie-item");

          movieElement.innerHTML = `
            <h3>${resultMovie.title} (${resultMovie.releaseYear})</h3>
            <p>${resultMovie.summary}</p>
            <button data-id="${resultMovie.id}" class="view-details">Details</button>
            <button data-id="${resultMovie.id}" class="edit-movie">Edit</button>
            <button data-id="${resultMovie.id}" class="delete-movie">Delete</button>
          `;

          moviesContainer.appendChild(movieElement);
        }

        modal.style.display = "none";
      } catch (error) {
        console.error("Error saving movie:", error);
        alert("Error saving movie.");
      }
    };
  }

  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", initialize);
  } else {
    initialize();
  }
}
