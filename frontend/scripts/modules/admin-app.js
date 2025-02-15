import {
  getMovieList,
  createMovie,
  updateMovie,
  deleteMovie,
} from "../api/api.js";
import { displayMovieDetails } from "./movie-details.js";

export function initAdminApp() {
  //! dohvacanje tokena???
  async function initialize() {
    const moviesContainer = document.getElementById("movies-container");
    const header = document.querySelector("h2");

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
        <h3>${movie.title} (${movie.release_year})</h3>
        <p>${movie.summary}</p>
        <button data-id="${movie.id}" class="view-details">Details</button>
        <button data-id="${movie.id}" class="edit-movie">Edit</button>
        <button data-id="${movie.id}" class="delete-movie">Delete</button>
      `;

      moviesContainer.appendChild(movieElement);
    });

    document.querySelectorAll(".view-details").forEach((button) => {
      button.addEventListener("click", async (e) => {
        const movieId = parseInt(e.target.dataset.id, 10);
        loadMovieDetails(movieId);
      });
    });

    document.querySelectorAll(".edit-movie").forEach((button) => {
      button.addEventListener("click", (e) => {
        const movieId = parseInt(e.target.dataset.id, 10);
        console.log("✏️ Edit Movie:", movieId);
        openMovieModal(movieId);
      });
    });

    document.querySelectorAll(".delete-movie").forEach((button) => {
      button.addEventListener("click", async (e) => {
        const movieId = parseInt(e.target.dataset.id, 10);
        if (confirm("Are you sure you want to delete this movie?")) {
          try {
            await deleteMovie(movieId, token);
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

  function openMovieModal(movieId = null) {
    const modal = document.getElementById("movie-modal");
    const form = document.getElementById("movie-form");
    const titleInput = document.getElementById("movie-title");
    const yearInput = document.getElementById("movie-year");
    const summaryInput = document.getElementById("movie-summary");

    if (!modal || !form || !titleInput || !yearInput || !summaryInput) {
      console.error("One or more modal elements are missing.");
      return;
    }

    if (movieId) {
      const movieElement = document.querySelector(
        `[data-id='${movieId}']`
      )?.parentNode;
      if (!movieElement) {
        console.error("Movie element not found.");
        return;
      }

      const titleElement = movieElement.querySelector("h3");
      const summaryElement = movieElement.querySelector("p");

      if (titleElement) {
        titleInput.value = titleElement.textContent.split(" (")[0];
      }
      if (summaryElement) {
        summaryInput.value = summaryElement.textContent;
      }
      const yearMatch = titleElement?.textContent.match(/\d{4}/);
      if (yearMatch) {
        yearInput.value = yearMatch[0];
      }
    } else {
      form.reset();
    }

    modal.style.display = "block";

    form.onsubmit = async (e) => {
      e.preventDefault();
      const movieData = {
        title: titleInput.value,
        release_year: parseInt(yearInput.value, 10),
        summary: summaryInput.value,
      };

      try {
        if (movieId) {
          await updateMovie(movieId, movieData, token);
        } else {
          await createMovie(movieData, token);
        }
        modal.style.display = "none";
        location.reload();
      } catch (error) {
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
