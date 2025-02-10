//ode treba stavit import fetcha za filmove iz api.js

document.addEventListener("DOMContentLoaded", async () => {
  const moviesContainer = document.getElementById("movies-container");
  const movies = await fetchMovies(); //ili kakogod da se nazove funkcija za fethcanje filmova

  if (movies - length === 0) {
    moviesContainer.innerHTML = "<p>No movies found.</p>";
    return;
  }

  movies.forEach((movie) => {
    const movieElement = document.createElement("div");
    movieElement.classList.add("movie-item");
    movieElement.innerHTML = `
    <h3>${movie.title} (${movie.release_year})</h3>
    <p>${movie.summary}</p>
    <button data-id="${movie.id}" class="view-details">View Details</button>`;
    moviesContainer.appendChild(movieElement);
  });

  document.querySelectorAll(".view-details").forEach((button) => {
    button.addEventListener("click", (e) => {
      const movieId = e.target.dataset.id;
      loadMovieDetails(movieId);
    });
  });
});

const loadMovieDetails = async (movieId) => {
  window.location.hash = `#movie-${movieId}`; //ode treba dodat tocan url za dohvat filmova, prije znaka # ja mislim
  document.getElementById(
    "movies-container"
  ).innerHTML = `<p>Loading movie details...</p>`;
  import("./movies.js").then((module) => module.displayMovieDetails(movieId));
};
