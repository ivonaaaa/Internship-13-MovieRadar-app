import {
  getMovieList,
  createMovie,
  updateMovie,
  deleteMovie,
} from "../api/api.js";
import { displayMovieDetails } from "./movie-details.js";
import { createLogoutButton } from "./logout.js";

const genres = ["action", "crime", "horror", "comedy", "drama"];
const cloudName = "dyt7wsphu";
const uploadPreset = "movie_uploads";

export function initAdminApp() {
  async function initialize() {
    const logoutButton = createLogoutButton();
    document.body.prepend(logoutButton);

    const listUsersButton = document.createElement("button");
    listUsersButton.id = "users-btn";
    listUsersButton.innerText = "Users list";
    listUsersButton.addEventListener("click", () => {
      window.location.href = "users.html";
    });
    document.body.prepend(listUsersButton);

    await renderMoviesList();
  }

  async function renderMoviesList() {
    const moviesContainer = document.getElementById("movies-container");
    moviesContainer.innerHTML = "";

    let header = document.getElementById("movies-header");
    if (!header) {
      header = document.createElement("h2");
      header.id = "movies-header";
      header.textContent = "Movies List";
      document.body.insertBefore(header, moviesContainer);
    }

    let addNewButton = document.querySelector(".add-new");
    if (!addNewButton) {
      addNewButton = document.createElement("button");
      addNewButton.textContent = "Add New";
      addNewButton.classList.add("add-new");
      addNewButton.addEventListener("click", () => openMovieModal());
      header.insertAdjacentElement("afterend", addNewButton);
    }

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
        <img src="${movie.imageLink || "placeholder.jpg"}" alt="${
        movie.title
      }" class="movie-cover" />
        <p>${movie.summary}</p>
        <button data-id="${movie.id}" class="view-details">Details</button>
        <button data-id="${movie.id}" class="edit-movie">Edit</button>
        <button data-id="${movie.id}" class="delete-movie">Delete</button>
      `;

      moviesContainer.appendChild(movieElement);
    });

    attachEventListeners();
  }

  function attachEventListeners() {
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
            await renderMoviesList();
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
            <input type="hidden" id="movie-id" value="" />
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

            <label for="movie-cover">Cover Image:</label>
            <input type="file" id="movie-cover" accept="image/*" />
            <img id="preview-image" src="" alt="Image Preview" style="display: none; max-width: 100px; margin-top: 10px;"/>

            <button type="submit">Save</button>
            <button type="button" id="close-modal">Cancel</button>
          </form>
        </div>
      `;
      document.body.prepend(modal); //
      document.getElementById("close-modal").addEventListener("click", () => {
        modal.style.display = "none";
      });

      document
        .getElementById("movie-cover")
        .addEventListener("change", (event) => {
          const file = event.target.files[0];
          if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
              document.getElementById("preview-image").src = e.target.result;
              document.getElementById("preview-image").style.display = "block";
            };
            reader.readAsDataURL(file);
          }
        });
    }
    return modal;
  }

  async function uploadImageToCloudinary(file) {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("upload_preset", uploadPreset);

    const response = await fetch(
      `https://api.cloudinary.com/v1_1/${cloudName}/image/upload`,
      {
        method: "POST",
        body: formData,
      }
    );

    if (!response.ok) {
      throw new Error("Image upload failed");
    }

    const data = await response.json();

    return data.secure_url;
  }

  async function openMovieModal(movieId = null) {
    const modal = ensureMovieModalExists();
    const form = document.getElementById("movie-form");
    form.reset();

    const movieIdField = document.getElementById("movie-id");
    movieIdField.value = movieId || "";

    if (movieId) {
      try {
        const movies = await getMovieList();
        const movie = movies.find((m) => m.id === movieId);

        if (movie) {
          form["movie-title"].value = movie.title;
          form["movie-year"].value = movie.releaseYear;
          form["movie-summary"].value = movie.summary;
          form["movie-genre"].value = movie.genre;

          if (movie.imageLink) {
            document.getElementById("preview-image").src = movie.imageLink;
            document.getElementById("preview-image").style.display = "block";
          }
        }
      } catch (error) {
        console.error("Failed to fetch movie details:", error);
      }
    }

    modal.style.display = "block";

    form.onsubmit = async (e) => {
      e.preventDefault();

      const fileInput = document.getElementById("movie-cover");
      let coverImageUrl = "";

      if (fileInput.files.length > 0) {
        try {
          coverImageUrl = await uploadImageToCloudinary(fileInput.files[0]);
        } catch (error) {
          alert("Failed to upload image.");
          return;
        }
      } else if (movieId) {
        const previewImg = document.getElementById("preview-image");
        coverImageUrl =
          previewImg.style.display !== "none" ? previewImg.src : "";
      }

      const movieData = {
        title: form["movie-title"].value,
        releaseYear: parseInt(form["movie-year"].value, 10),
        summary: form["movie-summary"].value,
        genre: form["movie-genre"].value,
        imageLink: coverImageUrl,
      };

      try {
        if (movieId) {
          await updateMovie(movieId, movieData);
        } else {
          await createMovie(movieData);
        }

        modal.style.display = "none";
        await renderMoviesList();
      } catch (error) {
        alert(
          `Failed to ${movieId ? "update" : "create"} movie: ${error.message}`
        );
      }
    };
  }

  initialize();
}
